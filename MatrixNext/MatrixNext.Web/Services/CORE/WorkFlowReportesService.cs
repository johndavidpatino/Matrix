using Dapper;
using MatrixNext.Web.ViewModels;
using System.Data;
using System.Data.SqlClient;

namespace MatrixNext.Web.Services.CORE;

/// <summary>
/// Interfaz para reportes y indicadores del WorkFlow
/// Ref: REP_IndicadoresCumplimientoTareas (SP legacy)
/// </summary>
public interface IWorkFlowReportesService
{
    /// <summary>
    /// Obtiene indicadores de cumplimiento de tareas (porcentaje completadas a tiempo)
    /// </summary>
    Task<List<IndicadorCumplimientoVM>> ObtenerIndicadoresCumplimiento(int? mes = null, int? año = null);

    /// <summary>
    /// Obtiene tareas vencidas por usuario
    /// </summary>
    Task<List<TareaVencidaVM>> ObtenerTareasVencidas(long idUsuario);

    /// <summary>
    /// Obtiene estadísticas generales del WorkFlow
    /// </summary>
    Task<EstadisticasWorkFlowVM> ObtenerEstadisticas();
}

/// <summary>
/// ViewModel para indicadores de cumplimiento
/// Ref: REP_IndicadoresCumplimientoTareas_Result.vb
/// </summary>
public class IndicadorCumplimientoVM
{
    public short? Año { get; set; }
    public string? Mes { get; set; }
    public string? Grupo { get; set; }
    public byte? Porcentaje { get; set; }
    public short? Cumplidos { get; set; }
    public short? Planeados { get; set; }
}

/// <summary>
/// ViewModel para tareas vencidas
/// </summary>
public class TareaVencidaVM
{
    public long IdWorkFlow { get; set; }
    public long IdTarea { get; set; }
    public long IdTrabajo { get; set; }
    public string? Estado { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public int DíasVencida { get; set; }
    public string? Observaciones { get; set; }
}

/// <summary>
/// ViewModel para estadísticas generales
/// </summary>
public class EstadisticasWorkFlowVM
{
    public int TareasActivas { get; set; }
    public int TareasCompletadas { get; set; }
    public int TareasVencidas { get; set; }
    public int TareasEnProgreso { get; set; }
    public decimal PorcentajeCumplimiento { get; set; }
    public DateTime FechaActualizacion { get; set; }
}

/// <summary>
/// Implementación de reportes del WorkFlow usando SPs legacy
/// </summary>
public class WorkFlowReportesService : IWorkFlowReportesService
{
    private readonly IDbConnection _connection;
    private readonly ILogger<WorkFlowReportesService> _logger;

    public WorkFlowReportesService(
        IDbConnection connection,
        ILogger<WorkFlowReportesService> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene indicadores de cumplimiento usando SP legacy
    /// Ref: REP_IndicadoresCumplimientoTareas
    /// </summary>
    public async Task<List<IndicadorCumplimientoVM>> ObtenerIndicadoresCumplimiento(int? mes = null, int? año = null)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Mes", mes);
            parameters.Add("@Año", año ?? DateTime.UtcNow.Year);

            var resultado = await _connection.QueryAsync<IndicadorCumplimientoVM>(
                "dbo.REP_IndicadoresCumplimientoTareas",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 60
            );

            _logger.LogInformation($"Indicadores obtenidos. Mes: {mes}, Año: {año}");
            return resultado.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo indicadores de cumplimiento");
            return new List<IndicadorCumplimientoVM>();
        }
    }

    /// <summary>
    /// Obtiene tareas vencidas por usuario
    /// </summary>
    public async Task<List<TareaVencidaVM>> ObtenerTareasVencidas(long idUsuario)
    {
        try
        {
            var sql = @"
                SELECT 
                    wf.Id AS IdWorkFlow,
                    wf.IdTarea,
                    wf.IdTrabajo,
                    wf.Estado,
                    wf.FechaVencimiento,
                    DATEDIFF(DAY, wf.FechaVencimiento, GETUTCDATE()) AS DíasVencida,
                    wf.Observaciones
                FROM CORE_WorkFlow wf
                INNER JOIN CORE_WorkFlow_UsuariosAsignados wua 
                    ON wf.Id = wua.IdWorkFlow AND wua.IdUsuario = @IdUsuario AND wua.Activo = 1
                WHERE wf.FechaVencimiento < GETUTCDATE()
                    AND wf.Estado NOT IN ('Completada', 'Anulada')
                ORDER BY wf.FechaVencimiento ASC
            ";

            var resultado = await _connection.QueryAsync<TareaVencidaVM>(
                sql,
                new { IdUsuario = idUsuario }
            );

            _logger.LogInformation($"Tareas vencidas obtenidas para usuario {idUsuario}: {resultado.Count()}");
            return resultado.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error obteniendo tareas vencidas del usuario {idUsuario}");
            return new List<TareaVencidaVM>();
        }
    }

    /// <summary>
    /// Obtiene estadísticas generales del WorkFlow
    /// </summary>
    public async Task<EstadisticasWorkFlowVM> ObtenerEstadisticas()
    {
        try
        {
            var sql = @"
                SELECT 
                    SUM(CASE WHEN Estado NOT IN ('Completada', 'Anulada') THEN 1 ELSE 0 END) AS TareasActivas,
                    SUM(CASE WHEN Estado = 'Completada' THEN 1 ELSE 0 END) AS TareasCompletadas,
                    SUM(CASE WHEN FechaVencimiento < GETUTCDATE() AND Estado NOT IN ('Completada', 'Anulada') THEN 1 ELSE 0 END) AS TareasVencidas,
                    SUM(CASE WHEN Estado = 'EnProgreso' THEN 1 ELSE 0 END) AS TareasEnProgreso,
                    CAST(SUM(CASE WHEN Estado = 'Completada' THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*), 0) AS DECIMAL(5,2)) AS PorcentajeCumplimiento
                FROM CORE_WorkFlow
                WHERE Estado IS NOT NULL
            ";

            var resultado = await _connection.QuerySingleOrDefaultAsync<EstadisticasWorkFlowVM>(sql);

            if (resultado == null)
            {
                resultado = new EstadisticasWorkFlowVM();
            }

            resultado.FechaActualizacion = DateTime.UtcNow;

            _logger.LogInformation("Estadísticas del WorkFlow obtenidas");
            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo estadísticas del WorkFlow");
            return new EstadisticasWorkFlowVM { FechaActualizacion = DateTime.UtcNow };
        }
    }
}
