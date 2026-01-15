using MatrixNext.Data.Models.OP;

namespace MatrixNext.Data.Services.OP;

/// <summary>
/// Servicio para envío de notificaciones en FichaCuantitativa
/// Coordina obtención de destinatarios y envío de emails
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.5
/// </summary>
public interface IOpNotificacionService
{
    /// <summary>
    /// Obtiene lista de destinatarios para notificación de trabajo
    /// </summary>
    Task<IEnumerable<DestinatarioEmailDto>> ObtenerDestinatariosAsync(long idTrabajo);

    /// <summary>
    /// Envía email de creación/actualización de FichaCuantitativa
    /// </summary>
    Task<(bool Success, string Message)> NotificarCreacionFichaAsync(
        long idTrabajo,
        string numeroTrabajo,
        string codigoProyecto,
        string nombreProyecto,
        long usuarioId);

    /// <summary>
    /// Envía email de cambio de estado de trabajo
    /// </summary>
    Task<(bool Success, string Message)> NotificarCambioEstadoAsync(
        long idTrabajo,
        string numeroTrabajo,
        string estadoAnterior,
        string estadoNuevo,
        string? observaciones = null,
        long? usuarioId = null);

    /// <summary>
    /// Envía email de cierre de trabajo
    /// </summary>
    Task<(bool Success, string Message)> NotificarCierreTrabajoAsync(
        long idTrabajo,
        string numeroTrabajo,
        string codigoProyecto,
        string? observaciones = null,
        long? usuarioId = null);

    /// <summary>
    /// Envía email customizado con parámetros específicos
    /// </summary>
    Task<(bool Success, string Message)> NotificarCustomizadoAsync(
        ParamsNotificacionFichaDto parametros);
}
