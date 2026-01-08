using MatrixNext.Web.ViewModels.OP;

namespace MatrixNext.Web.Services.OP;

public interface IOpSupervisionService
{
    Task<IReadOnlyList<UsuarioDto>> ObtenerUsuariosActivosAsync(CancellationToken cancellationToken = default);
    Task<bool> GuardarSupervisionAsync(GuardarSupervisionRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SupervisionHistoryRow>> ObtenerHistoricoAsync(long trabajoId, CancellationToken cancellationToken = default);
    Task<SupervisionSummary> ObtenerResumenAsync(long trabajoId, CancellationToken cancellationToken = default);
    Task<SupervisionSummary> ObtenerResumenGeneralAsync(CancellationToken cancellationToken = default);
}
