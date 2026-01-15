using MatrixNext.Data.Models.OP;

namespace MatrixNext.Data.Adapters.OP;

/// <summary>
/// Adapter para acceso a datos del Dashboard HomeRecoleccion
/// Obtiene métricas de recolección operativa desde BD
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.4
/// </summary>
public interface IHomeRecoleccionDashboardAdapter
{
    /// <summary>
    /// Obtiene trabajos activos para el dashboard
    /// SP: OP_Trabajos_Activos (por unidad, estado, coordinador)
    /// </summary>
    Task<IEnumerable<TrabajoActivoDashboardDto>> ObtenerTrabajosActivosAsync(long? idUnidad = null, int? limite = 10);

    /// <summary>
    /// Obtiene métricas consolidadas de recolección
    /// Calcula: total activos, en pausa, completados, en riesgo
    /// </summary>
    Task<IEnumerable<DashboardMetricaDto>> ObtenerMetricasAsync(long? idUnidad = null);

    /// <summary>
    /// Obtiene producción diaria de los últimos N días
    /// Para gráfico de tendencia
    /// </summary>
    Task<IEnumerable<ProduccionDiariaDto>> ObtenerProduccionDiariaAsync(int diasAtras = 7);

    /// <summary>
    /// Obtiene trabajos en riesgo (bajo avance, próximos a vencer)
    /// </summary>
    Task<IEnumerable<TrabajoActivoDashboardDto>> ObtenerTrabajosEnRiesgoAsync(long? idUnidad = null);

    /// <summary>
    /// Obtiene resumen de coordinadores activos y su carga de trabajo
    /// </summary>
    Task<IEnumerable<(string NombreCoordinador, int TrabajosAsignados, int EncuestasPlaneadas)>> ObtenerCargaCoordinadoresAsync();
}
