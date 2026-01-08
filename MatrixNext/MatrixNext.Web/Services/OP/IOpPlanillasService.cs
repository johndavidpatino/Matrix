using MatrixNext.Web.ViewModels.OP;

namespace MatrixNext.Web.Services.OP;

public interface IOpPlanillasService
{
    Task<PlanillasAprobacionViewModel> ObtenerPlanillasAsync(CancellationToken cancellationToken = default);
    Task<bool> AprobarPlanillaAsync(long trabajoId, long usuarioId, CancellationToken cancellationToken = default);
    Task<bool> RechazarPlanillaAsync(long trabajoId, long usuarioId, CancellationToken cancellationToken = default);
}
