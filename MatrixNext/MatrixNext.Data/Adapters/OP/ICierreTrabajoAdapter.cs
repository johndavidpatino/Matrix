using MatrixNext.Data.Models.OP;

namespace MatrixNext.Data.Adapters.OP;

/// <summary>
/// Adapter para cierre de trabajos con validación GD
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.6
/// </summary>
public interface ICierreTrabajoAdapter
{
    /// <summary>
    /// Obtiene información del trabajo para cierre
    /// </summary>
    Task<CierreTrabajoDto?> ObtenerTrabajoAsync(long idTrabajo);

    /// <summary>
    /// Valida documentos escaneados antes del cierre
    /// Consulta GD_DocumentosEscaneados validando completitud
    /// </summary>
    Task<ValidacionDocumentosDto> ValidarDocumentosAsync(long idTrabajo);

    /// <summary>
    /// Cambia estado del trabajo a "Cerrado"
    /// SP: PY_Trabajos_UpdateEstado
    /// </summary>
    Task<bool> CambiarEstadoACerradoAsync(long idTrabajo, string? observaciones, long usuarioId);

    /// <summary>
    /// Obtiene datos de trabajo para notificación por email
    /// </summary>
    Task<(string NumeroTrabajo, string CodigoProyecto, string NombreProyecto)> ObtenerDatosTrabajoAsync(long idTrabajo);
}
