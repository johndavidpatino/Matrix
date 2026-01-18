/// <summary>
/// Adapter para supervisión telefónica
/// Tabla BD: OP_SupervisionCampoTelefonico
/// Columnas: Id, TrabajoId, IdentificadorCuestionario, Supervisor, Operador, 
///           FechaSupervision, CRI01-CRI13, COM01-COM04, ACC01-ACC04, Observacion
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.10
/// </summary>
namespace MatrixNext.Data.Adapters.OP;

using Dapper;
using MatrixNext.Data.Models.OP;
using Microsoft.Extensions.Logging;
using System.Data;

public class SupervisionAdapter : ISupervisionAdapter
{
    private readonly IDbConnection _connection;
    private readonly ILogger<SupervisionAdapter> _logger;

    public SupervisionAdapter(IDbConnection connection, ILogger<SupervisionAdapter> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene supervisiones telefónicas
    /// Tabla: OP_SupervisionCampoTelefonico (columnas: Id, TrabajoId, Supervisor, Operador, FechaSupervision, etc.)
    /// </summary>
    public async Task<List<SupervisionTelefonicaDto>> ObtenerSupervisionesAsync(FiltrosSupervisionDto filtros)
    {
        try
        {
            // Usar tabla correcta: OP_SupervisionCampoTelefonico
            var query = @"
                SELECT 
                    s.Id AS IdSupervision, 
                    s.TrabajoId AS IdTrabajo, 
                    t.Id AS NumeroTrabajo,
                    s.Operador AS IdOperador, 
                    s.Operador AS NombreOperador,
                    s.Supervisor AS IdSupervisor, 
                    s.Supervisor AS NombreSupervisor,
                    s.FechaSupervision, 
                    s.IdentificadorCuestionario AS NumeroEncuesta, 
                    0 AS CalificacionTotal,
                    'Pendiente' AS ResultadoSupervision, 
                    s.Observacion AS Observaciones, 
                    s.FechaSupervision AS FechaRegistro, 
                    s.Supervisor AS RegistradoPor
                FROM OP_SupervisionCampoTelefonico s
                INNER JOIN PY_Trabajo t ON s.TrabajoId = t.Id
                WHERE 1=1
                    AND (@IdTrabajo IS NULL OR s.TrabajoId = @IdTrabajo)
                    AND (@IdOperador IS NULL OR s.Operador = @IdOperador)
                    AND (@IdSupervisor IS NULL OR s.Supervisor = @IdSupervisor)
                    AND (@FechaInicio IS NULL OR s.FechaSupervision >= @FechaInicio)
                    AND (@FechaFin IS NULL OR s.FechaSupervision <= @FechaFin)
                ORDER BY s.FechaSupervision DESC";

            var supervisiones = await _connection.QueryAsync<SupervisionTelefonicaDto>(query, filtros);
            _logger.LogInformation("Obtenidas {Count} supervisiones telefónicas", supervisiones.Count());
            return supervisiones.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo supervisiones telefónicas");
            throw;
        }
    }

    /// <summary>
    /// Obtiene resumen de supervisión por trabajo
    /// Tabla: OP_SupervisionCampoTelefonico
    /// </summary>
    public async Task<ResumenSupervisionDto> ObtenerResumenAsync(long idTrabajo, DateTime? fechaInicio, DateTime? fechaFin)
    {
        try
        {
            var query = @"
                SELECT 
                    COUNT(*) AS TotalSupervisiones,
                    0 AS Aprobadas,
                    0 AS Rechazadas,
                    0 AS Observadas,
                    0 AS PromedioCalificacion
                FROM OP_SupervisionCampoTelefonico
                WHERE TrabajoId = @IdTrabajo
                    AND (@FechaInicio IS NULL OR FechaSupervision >= @FechaInicio)
                    AND (@FechaFin IS NULL OR FechaSupervision <= @FechaFin)";

            var resumen = await _connection.QuerySingleOrDefaultAsync<ResumenSupervisionDto>(query, 
                new { IdTrabajo = idTrabajo, FechaInicio = fechaInicio, FechaFin = fechaFin });
            
            if (resumen == null)
            {
                resumen = new ResumenSupervisionDto { TotalSupervisiones = 0 };
            }
            
            _logger.LogInformation("Resumen supervisión trabajo {IdTrabajo}: {Total} supervisiones", 
                idTrabajo, resumen.TotalSupervisiones);
            return resumen;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo resumen supervisión para trabajo {IdTrabajo}", idTrabajo);
            throw;
        }
    }

    /// <summary>
    /// Registra una supervisión
    /// NOTA: SP OP_SupervisionCampoTelefonico_Add no existe - usar INSERT directo
    /// Tabla tiene columnas de criterios: CRI01-CRI13, COM01-COM04, ACC01-ACC04
    /// </summary>
    public async Task<long> RegistrarSupervisionAsync(RegistroSupervisionDto registro)
    {
        try
        {
            // Insertar directamente en la tabla ya que no hay SP
            var sql = @"
                INSERT INTO OP_SupervisionCampoTelefonico 
                    (TrabajoId, IdentificadorCuestionario, Supervisor, Operador, FechaSupervision, Observacion)
                VALUES 
                    (@TrabajoId, @NumeroEncuesta, @IdSupervisor, @IdOperador, @FechaSupervision, @Observaciones);
                SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";

            var parameters = new
            {
                TrabajoId = registro.IdTrabajo,
                NumeroEncuesta = registro.NumeroEncuesta,
                IdSupervisor = registro.IdSupervisor,
                IdOperador = registro.IdOperador,
                FechaSupervision = DateTime.Now,
                Observaciones = registro.Observaciones
            };

            var idSupervision = await _connection.ExecuteScalarAsync<long>(sql, parameters);
            
            _logger.LogInformation("Supervisión {Id} registrada para trabajo {TrabajoId}",
                idSupervision, registro.IdTrabajo);
            return idSupervision;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registrando supervisión telefónica");
            throw;
        }
    }

    /// <summary>
    /// Obtiene checklist de supervisión
    /// NOTA: La tabla OP_SupervisionCampoTelefonico no tiene tabla de items separada
    /// Los criterios están como columnas: CRI01-CRI13, COM01-COM04, ACC01-ACC04
    /// </summary>
    public async Task<List<ChecklistSupervisionDto>> ObtenerChecklistAsync(long idSupervision)
    {
        try
        {
            // La estructura de la tabla tiene criterios como columnas, no como items
            // Retornar lista vacía ya que no hay tabla de items
            _logger.LogWarning("La tabla OP_SupervisionCampoTelefonico no tiene items de checklist separados. " +
                "Los criterios están como columnas (CRI01-CRI13, COM01-COM04, ACC01-ACC04)");
            return new List<ChecklistSupervisionDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo checklist de supervisión {Id}", idSupervision);
            throw;
        }
    }

    /// <summary>
    /// Obtiene operadores activos
    /// NOTA: Simplificado - la lógica de permisos puede variar
    /// </summary>
    public async Task<List<CatalogoSupervisionDto>> ObtenerOperadoresActivosAsync(long? idTrabajo = null)
    {
        try
        {
            // Simplificado - verificar estructura de permisos en producción
            var query = @"
                SELECT DISTINCT
                    0 AS IdCatalogo,
                    'Operadores' AS Tipo,
                    p.id AS IdEmpleado,
                    CONCAT(p.nombres, ' ', p.apellidos) AS NombreCompleto,
                    CAST(p.identificacion AS VARCHAR) AS NumeroIdentificacion,
                    1 AS Activo
                FROM TH_Personas p
                WHERE p.activo = 1
                ORDER BY NombreCompleto";

            var operadores = await _connection.QueryAsync<CatalogoSupervisionDto>(query, new { IdTrabajo = idTrabajo });
            _logger.LogInformation("Obtenidos {Count} operadores activos", operadores.Count());
            return operadores.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo operadores activos");
            throw;
        }
    }

    /// <summary>
    /// Obtiene supervisores activos
    /// NOTA: Simplificado - verificar estructura de permisos en producción
    /// </summary>
    public async Task<List<CatalogoSupervisionDto>> ObtenerSupervisoresActivosAsync()
    {
        try
        {
            var query = @"
                SELECT 
                    0 AS IdCatalogo,
                    'Supervisores' AS Tipo,
                    p.id AS IdEmpleado,
                    CONCAT(p.nombres, ' ', p.apellidos) AS NombreCompleto,
                    CAST(p.identificacion AS VARCHAR) AS NumeroIdentificacion,
                    1 AS Activo
                FROM TH_Personas p
                WHERE p.activo = 1
                ORDER BY NombreCompleto";

            var supervisores = await _connection.QueryAsync<CatalogoSupervisionDto>(query);
            _logger.LogInformation("Obtenidos {Count} supervisores activos", supervisores.Count());
            return supervisores.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo supervisores activos");
            throw;
        }
    }

    /// <summary>
    /// Valida permiso de supervisión
    /// NOTA: Simplificado - verificar estructura de permisos en producción
    /// </summary>
    public async Task<bool> ValidarPermisoSupervisionAsync(long usuarioId)
    {
        try
        {
            // Simplificado - asumir que tiene permiso si existe en persona activa
            var query = @"
                SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
                FROM TH_Personas
                WHERE id = @UsuarioId AND activo = 1";

            var tienePermiso = await _connection.ExecuteScalarAsync<bool>(query, new { UsuarioId = usuarioId });
            _logger.LogInformation("Usuario {UserId} {Resultado} permiso de supervisión",
                usuarioId, tienePermiso ? "tiene" : "NO tiene");
            return tienePermiso;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando permiso de supervisión para usuario {UserId}", usuarioId);
            return false;
        }
    }
}
