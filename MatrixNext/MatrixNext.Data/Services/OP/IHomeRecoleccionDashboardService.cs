using MatrixNext.Data.Models.OP;

namespace MatrixNext.Data.Services.OP;

/// <summary>
/// Servicio de lógica de negocio para Dashboard HomeRecoleccion
/// Coordina obtención de datos, cálculos y transformaciones
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.4
/// </summary>
public interface IHomeRecoleccionDashboardService
{
    /// <summary>
    /// Obtiene el dashboard completo con todas las métricas y datos
    /// </summary>
    Task<HomeRecoleccionDashboardDto> ObtenerDashboardCompletoAsync(long? idUnidad = null);

    /// <summary>
    /// Obtiene solo los trabajos activos para tabla detallada
    /// </summary>
    Task<IEnumerable<TrabajoActivoDashboardDto>> ObtenerTrabajosActivosAsync(long? idUnidad = null, int? limite = 10);

    /// <summary>
    /// Obtiene solo las métricas consolidadas
    /// </summary>
    Task<IEnumerable<DashboardMetricaDto>> ObtenerMetricasAsync(long? idUnidad = null);

    /// <summary>
    /// Obtiene gráfico de producción diaria (tendencia)
    /// </summary>
    Task<IEnumerable<ProduccionDiariaDto>> ObtenerProduccionDiariaAsync(int diasAtras = 7);

    /// <summary>
    /// Obtiene trabajos en riesgo para alerta
    /// </summary>
    Task<IEnumerable<TrabajoActivoDashboardDto>> ObtenerTrabajosEnRiesgoAsync(long? idUnidad = null);

    /// <summary>
    /// Obtiene período de reporte (semana actual o rango personalizado)
    /// </summary>
    string GenerarEtiquetaPeriodo();
}
