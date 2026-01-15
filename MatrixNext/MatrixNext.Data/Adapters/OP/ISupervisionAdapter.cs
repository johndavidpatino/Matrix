/// <summary>
/// Interface para adapter de supervisión telefónica
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.10
/// </summary>
namespace MatrixNext.Data.Adapters.OP;

using MatrixNext.Data.Models.OP;

public interface ISupervisionAdapter
{
    Task<List<SupervisionTelefonicaDto>> ObtenerSupervisionesAsync(FiltrosSupervisionDto filtros);
    Task<ResumenSupervisionDto> ObtenerResumenAsync(long idTrabajo, DateTime? fechaInicio, DateTime? fechaFin);
    Task<long> RegistrarSupervisionAsync(RegistroSupervisionDto registro);
    Task<List<ChecklistSupervisionDto>> ObtenerChecklistAsync(long idSupervision);
    Task<List<CatalogoSupervisionDto>> ObtenerOperadoresActivosAsync(long? idTrabajo = null);
    Task<List<CatalogoSupervisionDto>> ObtenerSupervisoresActivosAsync();
    Task<bool> ValidarPermisoSupervisionAsync(long usuarioId);
}
