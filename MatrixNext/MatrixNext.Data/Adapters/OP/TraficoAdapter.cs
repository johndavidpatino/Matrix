/// <summary>
/// Adapter para gestión de tráfico de encuestas
/// 
/// VALIDACIÓN BD 2025-01:
/// TABLA REAL: OP_TraficoEncuestas
/// Columnas: id, TrabajoId, Ciudad, Cantidad, UsuarioEnvia, UnidadEnvia, FechaEnvio, 
///           ObservacionesEnvio, UsuarioRecibe, UnidadRecibe, FechaRecibo, ObservacionesRecibo,
///           Devolucion, MotivoDevolucion
/// 
/// TABLAS INEXISTENTES que se intentaban usar:
/// - PY_Trabajos (existe: PY_Trabajo)
/// - OP_Unidades (existe: US_Unidades)
/// - TH_Empleado (existe: TH_Personas)
/// - OP_TraficoPersonal (NO EXISTE - lógica especulativa)
/// - OP_UnidadesPermisos (NO EXISTE)
/// - US_PermisosUsuario (existe: US_PermisosUsuarios)
///
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.9
/// </summary>
namespace MatrixNext.Data.Adapters.OP;

using Dapper;
using MatrixNext.Data.Models.OP;
using Microsoft.Extensions.Logging;
using System.Data;

public class TraficoAdapter : ITraficoAdapter
{
    private readonly IDbConnection _connection;
    private readonly ILogger<TraficoAdapter> _logger;

    public TraficoAdapter(IDbConnection connection, ILogger<TraficoAdapter> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene movimientos de tráfico
    /// Tabla real: OP_TraficoEncuestas con JOINs a PY_Trabajo y US_Unidades
    /// </summary>
    public async Task<List<TraficoEncuestaDto>> ObtenerMovimientosAsync(FiltrosTraficoDto filtros)
    {
        try
        {
            // Query ajustado a estructura REAL de BD
            var query = @"
                SELECT 
                    t.id AS IdMovimiento, 
                    t.TrabajoId AS IdTrabajo,
                    CAST(tr.id AS VARCHAR) AS NumeroTrabajo,
                    t.UnidadEnvia AS IdUnidadOrigen, 
                    uo.Nombre AS NombreUnidadOrigen,
                    t.UnidadRecibe AS IdUnidadDestino, 
                    ud.Nombre AS NombreUnidadDestino,
                    CASE WHEN t.Devolucion = 1 THEN 'Devolución' ELSE 'Envío' END AS TipoMovimiento,
                    t.Cantidad AS CantidadEnviada, 
                    t.Cantidad AS CantidadRecibida,
                    0 AS Discrepancia,
                    t.FechaEnvio, 
                    t.FechaRecibo AS FechaRecepcion,
                    t.UsuarioEnvia AS EnviadoPor, 
                    p1.nombres + ' ' + p1.apellidos AS NombreEnviador,
                    t.UsuarioRecibe AS RecibidoPor, 
                    p2.nombres + ' ' + p2.apellidos AS NombreReceptor,
                    t.ObservacionesEnvio AS Observaciones, 
                    t.MotivoDevolucion AS ObservacionesDiscrepancia,
                    CASE WHEN t.FechaRecibo IS NOT NULL THEN 'Recibido' ELSE 'EnTransito' END AS Estado,
                    t.Ciudad
                FROM OP_TraficoEncuestas t
                LEFT JOIN PY_Trabajo tr ON t.TrabajoId = tr.id
                LEFT JOIN US_Unidades uo ON t.UnidadEnvia = uo.id
                LEFT JOIN US_Unidades ud ON t.UnidadRecibe = ud.id
                LEFT JOIN TH_Personas p1 ON t.UsuarioEnvia = p1.id
                LEFT JOIN TH_Personas p2 ON t.UsuarioRecibe = p2.id
                WHERE 1=1
                    AND (@IdTrabajo IS NULL OR t.TrabajoId = @IdTrabajo)
                    AND (@IdUnidadOrigen IS NULL OR t.UnidadEnvia = @IdUnidadOrigen)
                    AND (@IdUnidadDestino IS NULL OR t.UnidadRecibe = @IdUnidadDestino)
                    AND (@FechaInicio IS NULL OR t.FechaEnvio >= @FechaInicio)
                    AND (@FechaFin IS NULL OR t.FechaEnvio <= @FechaFin)
                ORDER BY t.FechaEnvio DESC";

            var movimientos = await _connection.QueryAsync<TraficoEncuestaDto>(query, filtros);
            _logger.LogInformation("Obtenidos {Count} movimientos de tráfico", movimientos.Count());
            return movimientos.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo movimientos de tráfico");
            throw;
        }
    }

    /// <summary>
    /// Obtiene resumen de tráfico por unidad
    /// Tabla: OP_TraficoEncuestas con JOIN a US_Unidades
    /// </summary>
    public async Task<ResumenTraficoDto> ObtenerResumenPorUnidadAsync(int idUnidad, long? idTrabajo = null)
    {
        try
        {
            var query = @"
                SELECT 
                    @IdUnidad AS IdUnidad,
                    u.Nombre AS NombreUnidad,
                    ISNULL(SUM(CASE WHEN UnidadEnvia = @IdUnidad THEN Cantidad ELSE 0 END), 0) AS TotalEnviado,
                    ISNULL(SUM(CASE WHEN UnidadRecibe = @IdUnidad AND FechaRecibo IS NOT NULL THEN Cantidad ELSE 0 END), 0) AS TotalRecibido,
                    ISNULL(SUM(CASE WHEN Devolucion = 1 THEN Cantidad ELSE 0 END), 0) AS TotalDevuelto,
                    ISNULL(SUM(CASE WHEN FechaRecibo IS NULL THEN Cantidad ELSE 0 END), 0) AS EnTransito,
                    0 AS TotalDiscrepancias,
                    MAX(FechaEnvio) AS UltimoMovimiento
                FROM OP_TraficoEncuestas t
                CROSS JOIN US_Unidades u
                WHERE u.id = @IdUnidad
                    AND (UnidadEnvia = @IdUnidad OR UnidadRecibe = @IdUnidad)
                    AND (@IdTrabajo IS NULL OR TrabajoId = @IdTrabajo)
                GROUP BY u.Nombre";

            var resumen = await _connection.QueryFirstOrDefaultAsync<ResumenTraficoDto>(query, 
                new { IdUnidad = idUnidad, IdTrabajo = idTrabajo });
            
            if (resumen == null)
            {
                resumen = new ResumenTraficoDto { IdUnidad = idUnidad };
            }
            
            _logger.LogInformation("Resumen tráfico unidad {IdUnidad}: {Enviado} enviados, {Recibido} recibidos", 
                idUnidad, resumen.TotalEnviado, resumen.TotalRecibido);
            return resumen;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo resumen tráfico para unidad {IdUnidad}", idUnidad);
            throw;
        }
    }

    /// <summary>
    /// Registra envío de encuestas
    /// INSERT en OP_TraficoEncuestas con columnas reales
    /// </summary>
    public async Task<long> RegistrarEnvioAsync(EnvioEncuestasDto envio)
    {
        try
        {
            var query = @"
                INSERT INTO OP_TraficoEncuestas 
                    (TrabajoId, Ciudad, Cantidad, UsuarioEnvia, UnidadEnvia, FechaEnvio, ObservacionesEnvio, Devolucion)
                VALUES 
                    (@IdTrabajo, @Ciudad, @Cantidad, @EnviadoPor, @IdUnidadOrigen, GETDATE(), @Observaciones, 0);
                SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";

            var idMovimiento = await _connection.ExecuteScalarAsync<long>(query, new
            {
                envio.IdTrabajo,
                envio.Ciudad,
                envio.Cantidad,
                envio.EnviadoPor,
                IdUnidadOrigen = envio.IdUnidadOrigen,
                envio.Observaciones
            });

            _logger.LogInformation("Registrado envío {Id}: {Cantidad} encuestas de unidad {Origen}",
                idMovimiento, envio.Cantidad, envio.IdUnidadOrigen);
            return idMovimiento;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registrando envío de encuestas");
            throw;
        }
    }

    /// <summary>
    /// Registra recepción de encuestas
    /// UPDATE en OP_TraficoEncuestas
    /// Nota: La BD real no tiene columna de cantidad recibida separada, solo marca fecha de recibo
    /// </summary>
    public async Task<bool> RegistrarRecepcionAsync(RecepcionEncuestasDto recepcion)
    {
        try
        {
            var query = @"
                UPDATE OP_TraficoEncuestas
                SET UsuarioRecibe = @RecibidoPor,
                    FechaRecibo = GETDATE(),
                    ObservacionesRecibo = @ObservacionesDiscrepancia
                WHERE id = @IdMovimiento
                    AND FechaRecibo IS NULL";

            var rowsAffected = await _connection.ExecuteAsync(query, new
            {
                recepcion.IdMovimiento,
                recepcion.RecibidoPor,
                recepcion.ObservacionesDiscrepancia
            });

            if (rowsAffected > 0)
            {
                _logger.LogInformation("Registrada recepción de movimiento {Id}", recepcion.IdMovimiento);
                return true;
            }

            _logger.LogWarning("Movimiento {Id} no pudo ser recibido (estado inválido)", recepcion.IdMovimiento);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registrando recepción de movimiento {Id}", recepcion.IdMovimiento);
            throw;
        }
    }

    /// <summary>
    /// Registra devolución de encuestas
    /// INSERT en OP_TraficoEncuestas con Devolucion=1
    /// </summary>
    public async Task<bool> RegistrarDevolucionAsync(DevolucionEncuestasDto devolucion)
    {
        try
        {
            var query = @"
                INSERT INTO OP_TraficoEncuestas 
                    (TrabajoId, Ciudad, Cantidad, UsuarioEnvia, UnidadEnvia, FechaEnvio, 
                     ObservacionesEnvio, Devolucion, MotivoDevolucion)
                SELECT TrabajoId, Ciudad, @CantidadDevuelta, @DevueltoPor, UnidadRecibe, GETDATE(),
                       @MotivoDevolucion, 1, @MotivoDevolucion
                FROM OP_TraficoEncuestas
                WHERE id = @IdMovimiento";

            var rowsAffected = await _connection.ExecuteAsync(query, new
            {
                devolucion.IdMovimiento,
                devolucion.CantidadDevuelta,
                devolucion.DevueltoPor,
                devolucion.MotivoDevolucion
            });

            if (rowsAffected > 0)
            {
                _logger.LogInformation("Registrada devolución de movimiento {Id}: {Cantidad} encuestas",
                    devolucion.IdMovimiento, devolucion.CantidadDevuelta);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registrando devolución de movimiento {Id}", devolucion.IdMovimiento);
            throw;
        }
    }

    /// <summary>
    /// Obtiene personal asignado a movimiento
    /// NOTA: Tabla OP_TraficoPersonal NO EXISTE - método stub
    /// </summary>
    public Task<List<PersonalTraficoDto>> ObtenerPersonalAsignadoAsync(long idMovimiento)
    {
        _logger.LogWarning("ObtenerPersonalAsignado: Tabla OP_TraficoPersonal no existe en BD");
        throw new NotImplementedException(
            "La tabla OP_TraficoPersonal no existe en la BD CO_Matrix_Intranet. " +
            "La funcionalidad de asignación de personal a tráfico no está implementada.");
    }

    /// <summary>
    /// Asigna personal a movimiento
    /// NOTA: Tabla OP_TraficoPersonal NO EXISTE - método stub
    /// </summary>
    public Task<bool> AsignarPersonalAsync(AsignacionPersonalDto asignacion)
    {
        _logger.LogWarning("AsignarPersonal: Tabla OP_TraficoPersonal no existe en BD");
        throw new NotImplementedException(
            "La tabla OP_TraficoPersonal no existe en la BD CO_Matrix_Intranet. " +
            "La funcionalidad de asignación de personal a tráfico no está implementada.");
    }

    /// <summary>
    /// Valida cantidad disponible para envío
    /// </summary>
    public async Task<bool> ValidarCantidadDisponibleAsync(long idTrabajo, int idUnidadOrigen, int cantidad)
    {
        try
        {
            var query = @"
                SELECT 
                    ISNULL((SELECT SUM(Cantidad) FROM OP_TraficoEncuestas 
                            WHERE TrabajoId = @IdTrabajo AND UnidadRecibe = @IdUnidad AND FechaRecibo IS NOT NULL), 0) -
                    ISNULL((SELECT SUM(Cantidad) FROM OP_TraficoEncuestas 
                            WHERE TrabajoId = @IdTrabajo AND UnidadEnvia = @IdUnidad), 0) AS Disponible";

            var disponible = await _connection.ExecuteScalarAsync<int>(query, 
                new { IdTrabajo = idTrabajo, IdUnidad = idUnidadOrigen });
            var esValido = disponible >= cantidad;

            _logger.LogInformation("Validación cantidad disponible: {Disponible} >= {Solicitado} = {Resultado}",
                disponible, cantidad, esValido);
            return esValido;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando cantidad disponible");
            return false;
        }
    }

    /// <summary>
    /// Valida permiso de usuario para unidad
    /// NOTA: Tabla OP_UnidadesPermisos NO EXISTE - usando US_PermisosUsuarios
    /// </summary>
    public async Task<bool> ValidarPermisoUnidadAsync(long usuarioId, int idUnidad)
    {
        try
        {
            // Simplificado - verificar si usuario tiene algún permiso de operaciones
            var query = @"
                SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
                FROM US_PermisosUsuarios pu
                WHERE pu.IdUsuario = @UsuarioId";

            var tienePermiso = await _connection.ExecuteScalarAsync<bool>(query, new { UsuarioId = usuarioId });
            _logger.LogInformation("Usuario {UserId} {Resultado} permiso general",
                usuarioId, tienePermiso ? "tiene" : "NO tiene");
            return tienePermiso;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando permiso de unidad");
            return false;
        }
    }
}
