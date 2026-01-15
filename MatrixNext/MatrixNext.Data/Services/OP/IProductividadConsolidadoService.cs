/// <summary>
/// Interface para servicio consolidado de productividad multi-roles
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.8
/// </summary>
namespace MatrixNext.Data.Services.OP;

using MatrixNext.Data.Models.OP;

public interface IProductividadConsolidadoService
{
    /// <summary>
    /// Obtiene planillas según rol y filtros aplicados
    /// </summary>
    Task<List<ProductividadPlanillaDto>> ObtenerPlanillasAsync(FiltrosProductividadDto filtros, long usuarioId);

    /// <summary>
    /// Obtiene resumen de productividad por periodo
    /// </summary>
    Task<ResumenProductividadDto> ObtenerResumenProductividadAsync(int año, int mes, int corte, long? idTrabajo, long usuarioId);

    /// <summary>
    /// Aprueba una o varias planillas (endpoint genérico)
    /// </summary>
    Task<(bool Success, string Message)> AprobarPlanillasAsync(List<AprobacionPlanillaDto> aprobaciones, long usuarioId);

    /// <summary>
    /// Rechaza una planilla con observaciones
    /// </summary>
    Task<(bool Success, string Message)> RechazarPlanillaAsync(long idPlanilla, string observaciones, long usuarioId);

    /// <summary>
    /// Valida si usuario puede realizar una acción sobre una planilla
    /// </summary>
    Task<bool> PuedeRealizarAccionAsync(long usuarioId, long idPlanilla, string accion);

    /// <summary>
    /// Obtiene permisos y rol del usuario
    /// </summary>
    Task<PermisosProductividadDto> ObtenerPermisosYRolAsync(long usuarioId);

    /// <summary>
    /// Obtiene lista de trabajos accesibles según rol del usuario
    /// </summary>
    Task<List<dynamic>> ObtenerTrabajosDisponiblesAsync(long usuarioId);
}
