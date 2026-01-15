/// <summary>
/// Adapter para gestión de tráfico de encuestas
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

    public async Task<List<TraficoEncuestaDto>> ObtenerMovimientosAsync(FiltrosTraficoDto filtros)
    {
        try
        {
            var query = @"
                SELECT 
                    t.IdMovimiento, t.IdTrabajo, tr.NumeroTrabajo,
                    t.IdUnidadOrigen, uo.Nombre AS NombreUnidadOrigen,
                    t.IdUnidadDestino, ud.Nombre AS NombreUnidadDestino,
                    t.TipoMovimiento, t.CantidadEnviada, t.CantidadRecibida,
                    (t.CantidadEnviada - ISNULL(t.CantidadRecibida, 0)) AS Discrepancia,
                    t.FechaEnvio, t.FechaRecepcion,
                    t.EnviadoPor, CONCAT(ue.Nombres, ' ', ue.Apellidos) AS NombreEnviador,
                    t.RecibidoPor, CONCAT(ur.Nombres, ' ', ur.Apellidos) AS NombreReceptor,
                    t.Observaciones, t.ObservacionesDiscrepancia, t.Estado, t.Ciudad
                FROM OP_TraficoEncuestas t
                INNER JOIN PY_Trabajos tr ON t.IdTrabajo = tr.IdTrabajo
                INNER JOIN OP_Unidades uo ON t.IdUnidadOrigen = uo.IdUnidad
                INNER JOIN OP_Unidades ud ON t.IdUnidadDestino = ud.IdUnidad
                LEFT JOIN TH_Empleado ue ON t.EnviadoPor = ue.IdEmpleado
                LEFT JOIN TH_Empleado ur ON t.RecibidoPor = ur.IdEmpleado
                WHERE 1=1
                    AND (@IdTrabajo IS NULL OR t.IdTrabajo = @IdTrabajo)
                    AND (@IdUnidadOrigen IS NULL OR t.IdUnidadOrigen = @IdUnidadOrigen)
                    AND (@IdUnidadDestino IS NULL OR t.IdUnidadDestino = @IdUnidadDestino)
                    AND (@FechaInicio IS NULL OR t.FechaEnvio >= @FechaInicio)
                    AND (@FechaFin IS NULL OR t.FechaEnvio <= @FechaFin)
                    AND (@TipoMovimiento IS NULL OR t.TipoMovimiento = @TipoMovimiento)
                    AND (@Estado IS NULL OR t.Estado = @Estado)
                    AND (@SoloConDiscrepancia = 0 OR (t.CantidadEnviada - ISNULL(t.CantidadRecibida, 0)) <> 0)
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

    public async Task<ResumenTraficoDto> ObtenerResumenPorUnidadAsync(int idUnidad, long? idTrabajo = null)
    {
        try
        {
            var query = @"
                SELECT 
                    @IdUnidad AS IdUnidad,
                    u.Nombre AS NombreUnidad,
                    SUM(CASE WHEN IdUnidadOrigen = @IdUnidad THEN CantidadEnviada ELSE 0 END) AS TotalEnviado,
                    SUM(CASE WHEN IdUnidadDestino = @IdUnidad AND CantidadRecibida IS NOT NULL THEN CantidadRecibida ELSE 0 END) AS TotalRecibido,
                    SUM(CASE WHEN TipoMovimiento = 'Devolución' THEN CantidadEnviada ELSE 0 END) AS TotalDevuelto,
                    SUM(CASE WHEN Estado = 'EnTransito' THEN CantidadEnviada ELSE 0 END) AS EnTransito,
                    SUM(CASE WHEN (CantidadEnviada - ISNULL(CantidadRecibida, 0)) <> 0 THEN 1 ELSE 0 END) AS TotalDiscrepancias,
                    MAX(FechaEnvio) AS UltimoMovimiento
                FROM OP_TraficoEncuestas t
                INNER JOIN OP_Unidades u ON u.IdUnidad = @IdUnidad
                WHERE (IdUnidadOrigen = @IdUnidad OR IdUnidadDestino = @IdUnidad)
                    AND (@IdTrabajo IS NULL OR IdTrabajo = @IdTrabajo)
                GROUP BY u.Nombre";

            var resumen = await _connection.QuerySingleAsync<ResumenTraficoDto>(query, new { IdUnidad = idUnidad, IdTrabajo = idTrabajo });
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

    public async Task<long> RegistrarEnvioAsync(EnvioEncuestasDto envio)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", envio.IdTrabajo);
            parameters.Add("@IdUnidadOrigen", envio.IdUnidadOrigen);
            parameters.Add("@IdUnidadDestino", envio.IdUnidadDestino);
            parameters.Add("@TipoMovimiento", "Envío");
            parameters.Add("@CantidadEnviada", envio.Cantidad);
            parameters.Add("@FechaEnvio", DateTime.Now);
            parameters.Add("@EnviadoPor", envio.EnviadoPor);
            parameters.Add("@Observaciones", envio.Observaciones);
            parameters.Add("@Ciudad", envio.Ciudad);
            parameters.Add("@Estado", "EnTransito");
            parameters.Add("@IdMovimiento", dbType: DbType.Int64, direction: ParameterDirection.Output);

            await _connection.ExecuteAsync(
                "OP_TraficoEncuestas_Enviar",
                parameters,
                commandType: CommandType.StoredProcedure);

            var idMovimiento = parameters.Get<long>("@IdMovimiento");
            _logger.LogInformation("Registrado envío {Id}: {Cantidad} encuestas de unidad {Origen} a {Destino}",
                idMovimiento, envio.Cantidad, envio.IdUnidadOrigen, envio.IdUnidadDestino);
            return idMovimiento;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registrando envío de encuestas");
            throw;
        }
    }

    public async Task<bool> RegistrarRecepcionAsync(RecepcionEncuestasDto recepcion)
    {
        try
        {
            var query = @"
                UPDATE OP_TraficoEncuestas
                SET CantidadRecibida = @CantidadRecibida,
                    FechaRecepcion = @FechaRecepcion,
                    RecibidoPor = @RecibidoPor,
                    ObservacionesDiscrepancia = @ObservacionesDiscrepancia,
                    Estado = CASE WHEN @CantidadRecibida = CantidadEnviada THEN 'Recibido' ELSE 'RecibidoConDiscrepancia' END
                WHERE IdMovimiento = @IdMovimiento
                    AND Estado = 'EnTransito'";

            var rowsAffected = await _connection.ExecuteAsync(query, new
            {
                recepcion.IdMovimiento,
                recepcion.CantidadRecibida,
                FechaRecepcion = DateTime.Now,
                recepcion.RecibidoPor,
                recepcion.ObservacionesDiscrepancia
            });

            if (rowsAffected > 0)
            {
                _logger.LogInformation("Registrada recepción de movimiento {Id}: {Cantidad} encuestas", 
                    recepcion.IdMovimiento, recepcion.CantidadRecibida);
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

    public async Task<bool> RegistrarDevolucionAsync(DevolucionEncuestasDto devolucion)
    {
        try
        {
            var query = @"
                INSERT INTO OP_TraficoEncuestas 
                    (IdTrabajo, IdUnidadOrigen, IdUnidadDestino, TipoMovimiento, CantidadEnviada, 
                     FechaEnvio, EnviadoPor, Observaciones, Estado)
                SELECT IdTrabajo, IdUnidadDestino, IdUnidadOrigen, 'Devolución', @CantidadDevuelta,
                       @FechaDevolucion, @DevueltoPor, @MotivoDevolucion, 'Devuelto'
                FROM OP_TraficoEncuestas
                WHERE IdMovimiento = @IdMovimiento";

            var rowsAffected = await _connection.ExecuteAsync(query, new
            {
                devolucion.IdMovimiento,
                devolucion.CantidadDevuelta,
                FechaDevolucion = DateTime.Now,
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

    public async Task<List<PersonalTraficoDto>> ObtenerPersonalAsignadoAsync(long idMovimiento)
    {
        try
        {
            var query = @"
                SELECT 
                    p.IdAsignacion, p.IdMovimiento, p.IdEmpleado,
                    CONCAT(e.Nombres, ' ', e.Apellidos) AS NombreEmpleado,
                    e.NumeroIdentificacion, p.Cargo, p.CantidadAsignada,
                    p.FechaAsignacion, p.AsignadoPor
                FROM OP_TraficoPersonal p
                INNER JOIN TH_Empleado e ON p.IdEmpleado = e.IdEmpleado
                WHERE p.IdMovimiento = @IdMovimiento
                ORDER BY p.FechaAsignacion DESC";

            var personal = await _connection.QueryAsync<PersonalTraficoDto>(query, new { IdMovimiento = idMovimiento });
            _logger.LogInformation("Obtenido personal asignado a movimiento {Id}: {Count} registros", 
                idMovimiento, personal.Count());
            return personal.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo personal asignado a movimiento {Id}", idMovimiento);
            throw;
        }
    }

    public async Task<bool> AsignarPersonalAsync(AsignacionPersonalDto asignacion)
    {
        try
        {
            var query = @"
                INSERT INTO OP_TraficoPersonal 
                    (IdMovimiento, IdEmpleado, Cargo, CantidadAsignada, FechaAsignacion, AsignadoPor)
                VALUES (@IdMovimiento, @IdEmpleado, @Cargo, @CantidadAsignada, @FechaAsignacion, @AsignadoPor)";

            var rowsAffected = await _connection.ExecuteAsync(query, new
            {
                asignacion.IdMovimiento,
                asignacion.IdEmpleado,
                asignacion.Cargo,
                asignacion.CantidadAsignada,
                FechaAsignacion = DateTime.Now,
                asignacion.AsignadoPor
            });

            if (rowsAffected > 0)
            {
                _logger.LogInformation("Asignado personal {IdEmpleado} a movimiento {Id}: {Cantidad} encuestas",
                    asignacion.IdEmpleado, asignacion.IdMovimiento, asignacion.CantidadAsignada);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error asignando personal a movimiento {Id}", asignacion.IdMovimiento);
            throw;
        }
    }

    public async Task<bool> ValidarCantidadDisponibleAsync(long idTrabajo, int idUnidadOrigen, int cantidad)
    {
        try
        {
            var query = @"
                SELECT 
                    (SELECT ISNULL(SUM(CantidadEnviada), 0) 
                     FROM OP_TraficoEncuestas 
                     WHERE IdTrabajo = @IdTrabajo AND IdUnidadDestino = @IdUnidad) -
                    (SELECT ISNULL(SUM(CantidadEnviada), 0) 
                     FROM OP_TraficoEncuestas 
                     WHERE IdTrabajo = @IdTrabajo AND IdUnidadOrigen = @IdUnidad) AS Disponible";

            var disponible = await _connection.ExecuteScalarAsync<int>(query, new { IdTrabajo = idTrabajo, IdUnidad = idUnidadOrigen });
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

    public async Task<bool> ValidarPermisoUnidadAsync(long usuarioId, int idUnidad)
    {
        try
        {
            // Permiso 117 (Verificación), 118 (Captura), 119 (Crítica), 120 (RMC)
            var permisos = new[] { 117, 118, 119, 120 };
            var query = @"
                SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
                FROM US_PermisosUsuario pu
                INNER JOIN OP_UnidadesPermisos up ON pu.IdPermiso = up.IdPermiso
                WHERE pu.IdUsuario = @UsuarioId
                    AND up.IdUnidad = @IdUnidad
                    AND pu.IdPermiso IN @Permisos";

            var tienePermiso = await _connection.ExecuteScalarAsync<bool>(query, new { UsuarioId = usuarioId, IdUnidad = idUnidad, Permisos = permisos });
            _logger.LogInformation("Usuario {UserId} {Resultado} permiso para unidad {IdUnidad}",
                usuarioId, tienePermiso ? "tiene" : "NO tiene", idUnidad);
            return tienePermiso;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando permiso de unidad");
            return false;
        }
    }
}
