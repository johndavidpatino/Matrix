using MatrixNext.Web.ViewModels.OP;

namespace MatrixNext.Web.Services.OP;

public interface IOpPlanillasService
{
    Task<PlanillasAprobacionViewModel> ObtenerPlanillasAsync(CancellationToken cancellationToken = default);
}
