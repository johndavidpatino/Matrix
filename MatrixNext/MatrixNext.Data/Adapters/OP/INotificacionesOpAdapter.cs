using MatrixNext.Data.Models.OP;

namespace MatrixNext.Data.Adapters.OP;

/// <summary>
/// Adapter para obtener destinatarios de notificaciones en FichaCuantitativa
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.5
/// </summary>
public interface INotificacionesOpAdapter
{
    /// <summary>
    /// Obtiene coordinador del trabajo
    /// SP: Consultará PY_Trabajos.IdCoordinador → TH_Usuario
    /// </summary>
    Task<DestinatarioEmailDto?> ObtenerCoordinadorTrabajoAsync(long idTrabajo);

    /// <summary>
    /// Obtiene usuarios COE (Centro de Operaciones Especializadas) por unidad
    /// SP: US_Usuarios_GetByRole con filtro "COE" o similar
    /// </summary>
    Task<IEnumerable<DestinatarioEmailDto>> ObtenerCoeUnidadAsync(long? idUnidad = null);

    /// <summary>
    /// Obtiene PMO (Project Manager Office) del trabajo
    /// SP: PY_Trabajos_GetPmo o similar
    /// </summary>
    Task<DestinatarioEmailDto?> ObtenerPmoTrabajoAsync(long idTrabajo);

    /// <summary>
    /// Obtiene todos los destinatarios recomendados para una notificación
    /// Combina: Coordinador + COE + PMO
    /// </summary>
    Task<IEnumerable<DestinatarioEmailDto>> ObtenerDestinatariosAsync(long idTrabajo);
}
