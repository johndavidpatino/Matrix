/// <summary>
/// Interface para adapter de distribución de entrevistas
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.2.1-12.2.3
/// </summary>
namespace MatrixNext.Data.Adapters.PY;

using MatrixNext.Data.Models.PY;

public interface IDistribucionAdapter
{
    // Sprint 12.2.1: Distribución de Entrevistas
    Task<List<DistribucionEntrevistaDto>> ObtenerDistribucionesAsync(long idTrabajo);
    Task<ResumenDistribucionDto> ObtenerResumenAsync(long idTrabajo);
    Task<bool> DistribuirPorUnidadAsync(DistribuirPorUnidadDto distribucion);
    Task<List<CuotaDistribucionDto>> ObtenerCuotasAsync(long idDistribucion);
    Task<bool> ValidarSumaDistribucionAsync(long idTrabajo, int sumaDistribucion);
    
    // Sprint 12.2.2: Variables de Control
    Task<List<VariableControlDto>> ObtenerVariablesControlAsync(long idTrabajo);
    Task<long> CrearVariableControlAsync(VariableControlDto variable);
    Task<bool> ActualizarVariableControlAsync(VariableControlDto variable);
    Task<bool> EliminarVariableControlAsync(long idVariable);
    
    // Sprint 12.2.3: InHome Visit
    Task<List<InHomeVisitDto>> ObtenerInHomeVisitsAsync(long idTrabajo);
    Task<long> CrearInHomeVisitAsync(InHomeVisitDto visita);
    Task<bool> ActualizarInHomeVisitAsync(InHomeVisitDto visita);
    Task<bool> CambiarEstadoVisitaAsync(long idVisita, string nuevoEstado, long usuarioId);
}
