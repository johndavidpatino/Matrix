using MatrixNext.Data.Models.OP;

namespace MatrixNext.Data.Services.OP;

/// <summary>
/// Servicio para cierre de trabajos con validaciones GD
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.6
/// </summary>
public interface ICierreTrabajoService
{
    /// <summary>
    /// Valida requisitos para cierre de trabajo
    /// </summary>
    Task<(bool CanClose, string Message)> ValidarRequisitosParaCierreAsync(long idTrabajo);

    /// <summary>
    /// Ejecuta el cierre completo del trabajo:
    /// 1. Valida documentos
    /// 2. Cambia estado
    /// 3. Envía notificación
    /// </summary>
    Task<(bool Success, string Message)> CerrarTrabajoAsync(long idTrabajo, string? observaciones, long usuarioId);

    /// <summary>
    /// Obtiene información de documentos para la validación
    /// </summary>
    Task<ValidacionDocumentosDto> ObtenerValidacionDocumentosAsync(long idTrabajo);
}
