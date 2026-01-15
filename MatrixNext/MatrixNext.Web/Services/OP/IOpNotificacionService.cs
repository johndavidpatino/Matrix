namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Interfaz para servicio de notificaciones por email del módulo OP
/// Ref: WebMatrix/Emails/*.aspx + EnviarCorreo.vb
/// </summary>
public interface IOpNotificacionService
{
    /// <summary>
    /// Notificar programación creada al entrevistado
    /// </summary>
    Task<(bool Success, string Error)> NotificarProgramacionCreadaAsync(long programacionId);

    /// <summary>
    /// Enviar recordatorio 24h antes de sesión
    /// </summary>
    Task<(bool Success, string Error)> EnviarRecordatorioSesionAsync(long programacionId);

    /// <summary>
    /// Notificar cambio de estado de programación
    /// </summary>
    Task<(bool Success, string Error)> NotificarCambioEstadoProgramacionAsync(
        long programacionId, string estadoAnterior, string estadoNuevo);

    /// <summary>
    /// Notificar ficha técnica completada
    /// </summary>
    Task<(bool Success, string Error)> NotificarFichaCompletadaAsync(long fichaId);

    /// <summary>
    /// Notificar asignación de moderador
    /// </summary>
    Task<(bool Success, string Error)> NotificarAsignacionModeradorAsync(
        long programacionId, long moderadorId);

    /// <summary>
    /// Enviar reporte diario de programaciones
    /// </summary>
    Task<(bool Success, string Error)> EnviarReporteDiarioAsync(DateTime fecha);
}
