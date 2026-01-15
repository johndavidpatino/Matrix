/// <summary>
/// Adapter para supervisión telefónica
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

    public async Task<List<SupervisionTelefonicaDto>> ObtenerSupervisionesAsync(FiltrosSupervisionDto filtros)
    {
        try
        {
            var query = @"
                SELECT 
                    s.IdSupervision, s.IdTrabajo, t.NumeroTrabajo,
                    s.IdOperador, CONCAT(o.Nombres, ' ', o.Apellidos) AS NombreOperador,
                    s.IdSupervisor, CONCAT(sup.Nombres, ' ', sup.Apellidos) AS NombreSupervisor,
                    s.FechaSupervision, s.NumeroEncuesta, s.CalificacionTotal,
                    s.ResultadoSupervision, s.Observaciones, s.FechaRegistro, s.RegistradoPor
                FROM OP_SupervisionTelefonica s
                INNER JOIN PY_Trabajos t ON s.IdTrabajo = t.IdTrabajo
                INNER JOIN TH_Empleado o ON s.IdOperador = o.IdEmpleado
                INNER JOIN TH_Empleado sup ON s.IdSupervisor = sup.IdEmpleado
                WHERE 1=1
                    AND (@IdTrabajo IS NULL OR s.IdTrabajo = @IdTrabajo)
                    AND (@IdOperador IS NULL OR s.IdOperador = @IdOperador)
                    AND (@IdSupervisor IS NULL OR s.IdSupervisor = @IdSupervisor)
                    AND (@FechaInicio IS NULL OR s.FechaSupervision >= @FechaInicio)
                    AND (@FechaFin IS NULL OR s.FechaSupervision <= @FechaFin)
                    AND (@ResultadoSupervision IS NULL OR s.ResultadoSupervision = @ResultadoSupervision)
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

    public async Task<ResumenSupervisionDto> ObtenerResumenAsync(long idTrabajo, DateTime? fechaInicio, DateTime? fechaFin)
    {
        try
        {
            var query = @"
                SELECT 
                    COUNT(*) AS TotalSupervisiones,
                    SUM(CASE WHEN ResultadoSupervision = 'Aprobado' THEN 1 ELSE 0 END) AS Aprobadas,
                    SUM(CASE WHEN ResultadoSupervision = 'Rechazado' THEN 1 ELSE 0 END) AS Rechazadas,
                    SUM(CASE WHEN ResultadoSupervision = 'Observado' THEN 1 ELSE 0 END) AS Observadas,
                    AVG(CalificacionTotal) AS PromedioCalificacion
                FROM OP_SupervisionTelefonica
                WHERE IdTrabajo = @IdTrabajo
                    AND (@FechaInicio IS NULL OR FechaSupervision >= @FechaInicio)
                    AND (@FechaFin IS NULL OR FechaSupervision <= @FechaFin)";

            var resumen = await _connection.QuerySingleAsync<ResumenSupervisionDto>(query, 
                new { IdTrabajo = idTrabajo, FechaInicio = fechaInicio, FechaFin = fechaFin });
            
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

    public async Task<long> RegistrarSupervisionAsync(RegistroSupervisionDto registro)
    {
        try
        {
            using (var transaction = _connection.BeginTransaction())
            {
                // 1. Calcular calificación total del checklist
                var puntajeTotal = registro.Checklist.Sum(c => c.Cumple ? c.Puntaje : 0);
                var puntajeMaximo = registro.Checklist.Sum(c => c.Puntaje);
                var calificacion = puntajeMaximo > 0 ? (decimal)puntajeTotal / puntajeMaximo * 100 : 0;

                // 2. Determinar resultado según calificación
                var resultado = calificacion >= 80 ? "Aprobado" : calificacion >= 60 ? "Observado" : "Rechazado";

                // 3. Insertar supervisión principal
                var parameters = new DynamicParameters();
                parameters.Add("@IdTrabajo", registro.IdTrabajo);
                parameters.Add("@IdOperador", registro.IdOperador);
                parameters.Add("@IdSupervisor", registro.IdSupervisor);
                parameters.Add("@FechaSupervision", DateTime.Now);
                parameters.Add("@NumeroEncuesta", registro.NumeroEncuesta);
                parameters.Add("@CalificacionTotal", calificacion);
                parameters.Add("@ResultadoSupervision", resultado);
                parameters.Add("@Observaciones", registro.Observaciones);
                parameters.Add("@FechaRegistro", DateTime.Now);
                parameters.Add("@RegistradoPor", registro.RegistradoPor);
                parameters.Add("@IdSupervision", dbType: DbType.Int64, direction: ParameterDirection.Output);

                await _connection.ExecuteAsync(
                    "OP_SupervisionCampoTelefonico_Save",
                    parameters,
                    transaction,
                    commandType: CommandType.StoredProcedure);

                var idSupervision = parameters.Get<long>("@IdSupervision");

                // 4. Insertar items del checklist
                foreach (var item in registro.Checklist)
                {
                    await _connection.ExecuteAsync(@"
                        INSERT INTO OP_SupervisionChecklistItems 
                            (IdSupervision, CodigoItem, DescripcionItem, Cumple, Puntaje, ObservacionItem)
                        VALUES (@IdSupervision, @CodigoItem, @DescripcionItem, @Cumple, @Puntaje, @ObservacionItem)",
                        new
                        {
                            IdSupervision = idSupervision,
                            item.CodigoItem,
                            item.DescripcionItem,
                            item.Cumple,
                            item.Puntaje,
                            item.ObservacionItem
                        },
                        transaction);
                }

                transaction.Commit();
                _logger.LogInformation("Supervisión {Id} registrada. Calificación: {Calificacion}%, Resultado: {Resultado}",
                    idSupervision, calificacion, resultado);
                return idSupervision;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registrando supervisión telefónica");
            throw;
        }
    }

    public async Task<List<ChecklistSupervisionDto>> ObtenerChecklistAsync(long idSupervision)
    {
        try
        {
            var query = @"
                SELECT IdItem, IdSupervision, CodigoItem, DescripcionItem, Cumple, Puntaje, ObservacionItem
                FROM OP_SupervisionChecklistItems
                WHERE IdSupervision = @IdSupervision
                ORDER BY CodigoItem";

            var items = await _connection.QueryAsync<ChecklistSupervisionDto>(query, new { IdSupervision = idSupervision });
            _logger.LogInformation("Obtenidos {Count} items del checklist para supervisión {Id}", 
                items.Count(), idSupervision);
            return items.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo checklist de supervisión {Id}", idSupervision);
            throw;
        }
    }

    public async Task<List<CatalogoSupervisionDto>> ObtenerOperadoresActivosAsync(long? idTrabajo = null)
    {
        try
        {
            var query = @"
                SELECT DISTINCT
                    0 AS IdCatalogo,
                    'Operadores' AS Tipo,
                    e.IdEmpleado,
                    CONCAT(e.Nombres, ' ', e.Apellidos) AS NombreCompleto,
                    e.NumeroIdentificacion,
                    1 AS Activo
                FROM TH_Empleado e
                INNER JOIN US_PermisosUsuario pu ON e.IdEmpleado = pu.IdUsuario
                WHERE pu.IdPermiso = 157  -- Permiso de Call Center / MyS
                    AND e.Estado = 'Activo'
                    AND (@IdTrabajo IS NULL OR EXISTS (
                        SELECT 1 FROM PY_TrabajosPersonal tp 
                        WHERE tp.IdEmpleado = e.IdEmpleado AND tp.IdTrabajo = @IdTrabajo
                    ))
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

    public async Task<List<CatalogoSupervisionDto>> ObtenerSupervisoresActivosAsync()
    {
        try
        {
            var query = @"
                SELECT 
                    0 AS IdCatalogo,
                    'Supervisores' AS Tipo,
                    e.IdEmpleado,
                    CONCAT(e.Nombres, ' ', e.Apellidos) AS NombreCompleto,
                    e.NumeroIdentificacion,
                    1 AS Activo
                FROM TH_Empleado e
                INNER JOIN US_PermisosUsuario pu ON e.IdEmpleado = pu.IdUsuario
                WHERE pu.IdPermiso IN (100, 135)  -- PMO o Coordinador
                    AND e.Estado = 'Activo'
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

    public async Task<bool> ValidarPermisoSupervisionAsync(long usuarioId)
    {
        try
        {
            var query = @"
                SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
                FROM US_PermisosUsuario
                WHERE IdUsuario = @UsuarioId
                    AND IdPermiso = 157";  // Permiso 157: MyS/Call Center

            var tienePermiso = await _connection.ExecuteScalarAsync<bool>(query, new { UsuarioId = usuarioId });
            _logger.LogInformation("Usuario {UserId} {Resultado} permiso de supervisión (157)",
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
