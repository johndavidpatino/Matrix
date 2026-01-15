/// <summary>
/// Interface para adapter de productividad multi-roles consolidado
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.8
/// </summary>
namespace MatrixNext.Data.Adapters.OP;

using MatrixNext.Data.Models.OP;

public interface IProductividadAdapter
{
    /// <summary>
    /// Obtiene planillas según filtros y rol del usuario
    /// </summary>
    Task<List<ProductividadPlanillaDto>> ObtenerPlanillasPorRolAsync(FiltrosProductividadDto filtros, string rol, long usuarioId);

    /// <summary>
    /// Obtiene resumen de productividad por corte/mes
    /// </summary>
    Task<ResumenProductividadDto> ObtenerResumenAsync(int año, int mes, int corte, long? idTrabajo = null);

    /// <summary>
    /// Aprueba una planilla (actualiza estado y monto autorizado)
    /// </summary>
    Task<bool> AprobarPlanillaAsync(AprobacionPlanillaDto aprobacion);

    /// <summary>
    /// Rechaza una planilla (actualiza estado y observaciones)
    /// </summary>
    Task<bool> RechazarPlanillaAsync(long idPlanilla, string observaciones, long usuarioId);

    /// <summary>
    /// Valida si un usuario tiene permiso para una acción en un trabajo
    /// </summary>
    Task<bool> TienePermisoAsync(long usuarioId, long idTrabajo, string accion, string rol);

    /// <summary>
    /// Calcula corte 16-15 para una fecha específica
    /// </summary>
    Task<(int Corte, int Mes, int Año)> CalcularCorte16_15Async(DateTime fecha);

    /// <summary>
    /// Obtiene permisos del usuario según sus roles asignados
    /// </summary>
    Task<PermisosProductividadDto> ObtenerPermisosUsuarioAsync(long usuarioId);

    /// <summary>
    /// Obtiene lista de trabajos asignados al usuario según su rol
    /// </summary>
    Task<List<dynamic>> ObtenerTrabajosAsignadosAsync(long usuarioId, string rol);
}
