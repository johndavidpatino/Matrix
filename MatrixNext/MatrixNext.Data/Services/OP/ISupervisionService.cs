/// <summary>
/// Interface para servicio de supervisión telefónica
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.10
/// </summary>
namespace MatrixNext.Data.Services.OP;

using MatrixNext.Data.Models.OP;

public interface ISupervisionService
{
    Task<List<SupervisionTelefonicaDto>> ObtenerSupervisionesAsync(FiltrosSupervisionDto filtros, long usuarioId);
    Task<ResumenSupervisionDto> ObtenerResumenAsync(long idTrabajo, DateTime? fechaInicio, DateTime? fechaFin, long usuarioId);
    Task<(bool Success, string Message, long IdSupervision)> RegistrarSupervisionAsync(RegistroSupervisionDto registro, long usuarioId);
    Task<List<ChecklistSupervisionDto>> ObtenerChecklistAsync(long idSupervision, long usuarioId);
    Task<List<CatalogoSupervisionDto>> ObtenerCatalogosAsync(string tipo, long usuarioId, long? idTrabajo = null);
}
