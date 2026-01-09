using MatrixNext.Web.Services.OP.Models;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Servicio para gestión de fichas técnicas (Entrevista, Sesión, Observación)
/// Ref: ANALISIS_OP_CUALITATIVO_FASE4_FLUJOS2Y3.md § 3.3
/// Tareas: OP-F03 (Fichas)
/// </summary>
public interface IOpFichasTecnicasService
{
    /// <summary>
    /// Obtiene ficha de entrevista para edición
    /// Ref: FichaEntrevista.aspx.vb líneas 41-123 (Page_Load, cargarDatos)
    /// </summary>
    Task<(bool Success, FichaTecnicaVm Data, string Error)> ObtenerFichaEntrevistaAsync(
        long trabajoId);

    /// <summary>
    /// Guarda ficha de entrevista con validaciones
    /// Ref: FichaEntrevista.aspx.vb líneas 125-214 (btnGuardar_Click)
    /// Validaciones: 8 validaciones documentadas en FASE4 § 3.3 PASO 3.2
    /// </summary>
    Task<(bool Success, string Error)> GuardarFichaEntrevistaAsync(
        FichaTecnicaVm ficha, long usuarioId);

    /// <summary>
    /// Entrega ficha y envía correo de notificación
    /// Ref: FichaEntrevista.aspx.vb líneas 216-267 (btnEntregar_Click)
    /// </summary>
    Task<(bool Success, string Error)> EntregarFichaEntrevistaAsync(
        long trabajoId, long usuarioId);

    /// <summary>
    /// Obtiene ficha de sesión (similar a entrevista)
    /// Ref: FichaSesion.aspx (pendiente confirmación)
    /// </summary>
    Task<(bool Success, FichaTecnicaVm Data, string Error)> ObtenerFichaSesionAsync(
        long trabajoId);

    /// <summary>
    /// Guarda ficha de sesión
    /// </summary>
    Task<(bool Success, string Error)> GuardarFichaSesionAsync(
        FichaTecnicaVm ficha, long usuarioId);

    /// <summary>
    /// Obtiene ficha de observación
    /// Ref: FichaObservacion.aspx (pendiente confirmación)
    /// </summary>
    Task<(bool Success, FichaTecnicaVm Data, string Error)> ObtenerFichaObservacionAsync(
        long trabajoId);

    /// <summary>
    /// Guarda ficha de observación
    /// </summary>
    Task<(bool Success, string Error)> GuardarFichaObservacionAsync(
        FichaTecnicaVm ficha, long usuarioId);

    /// <summary>
    /// Obtiene ficha de transcripción (nuevo tipo 4)
    /// </summary>
    Task<(bool Success, FichaTecnicaVm Data, string Error)> ObtenerFichaTranscripcionAsync(
        long trabajoId);

    /// <summary>
    /// Guarda ficha de transcripción
    /// </summary>
    Task<(bool Success, string Error)> GuardarFichaTranscripcionAsync(
        FichaTecnicaVm ficha, long usuarioId);

    /// <summary>
    /// Entrega ficha de transcripción y envía correo
    /// </summary>
    Task<(bool Success, string Error)> EntregarFichaTranscripcionAsync(
        long trabajoId, long usuarioId);

    /// <summary>
    /// Valida presupuesto disponible para incentivos
    /// Ref: FichaEntrevista.aspx.vb líneas 269-305 (ValidarPresupuesto)
    /// </summary>
    Task<(bool Success, decimal Disponible, string Error)> ValidarPresupuestoIncentivosAsync(
        long trabajoId, decimal montoSolicitado);

    /// <summary>
    /// Actualiza estado de Habeas Data
    /// Ref: FichaEntrevista.aspx.vb líneas 307-332 (btnActualizarHabeasData_Click)
    /// </summary>
    Task<(bool Success, string Error)> ActualizarHabeasDataAsync(
        long trabajoId, bool habeasDataFirmado);
}
