/// <summary>
/// Interface para service de distribución de entrevistas
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.2.1-12.2.3
/// </summary>
namespace MatrixNext.Data.Services.PY;

using MatrixNext.Data.Models.PY;

public interface IDistribucionService
{
    // Sprint 12.2.1: Distribución de Entrevistas
    Task<(bool success, string message)> DistribuirEntrevistasAsync(DistribuirPorUnidadDto distribucion, long usuarioId);
    Task<ResumenDistribucionDto> ObtenerResumenDistribucionAsync(long idTrabajo);
    Task<List<DistribucionEntrevistaDto>> ObtenerDistribucionesAsync(long idTrabajo);
    Task<List<CuotaDistribucionDto>> ObtenerCuotasAsync(long idDistribucion);
    
    // Sprint 12.2.2: Variables de Control
    Task<List<VariableControlDto>> ObtenerVariablesControlAsync(long idTrabajo);
    Task<(bool success, string message, long id)> CrearVariableControlAsync(VariableControlDto variable, long usuarioId);
    Task<(bool success, string message)> ActualizarVariableControlAsync(VariableControlDto variable, long usuarioId);
    Task<(bool success, string message)> EliminarVariableControlAsync(long idVariable, long usuarioId);
    
    // Sprint 12.2.3: InHome Visit
    Task<List<InHomeVisitDto>> ObtenerInHomeVisitsAsync(long idTrabajo);
    Task<(bool success, string message, long id)> CrearInHomeVisitAsync(InHomeVisitDto visita, long usuarioId);
    Task<(bool success, string message)> ActualizarInHomeVisitAsync(InHomeVisitDto visita, long usuarioId);
    Task<(bool success, string message)> CambiarEstadoVisitaAsync(long idVisita, string nuevoEstado, long usuarioId);
}
