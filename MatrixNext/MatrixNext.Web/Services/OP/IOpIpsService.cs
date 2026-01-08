using MatrixNext.Web.ViewModels.OP;

namespace MatrixNext.Web.Services.OP;

public sealed record IpsExportResult(string PhysicalPath, string PublicRelativePath);

public interface IOpIpsService
{
    Task<IpsRevisionViewModel> ObtenerRevisionesAsync(long? trabajoId, CancellationToken cancellationToken = default);
    Task<bool> GuardarRevisionAsync(IpsRevisionUpdateModel model, CancellationToken cancellationToken = default);
    Task<IpsExportResult> ExportarRevisionesAsync(long? trabajoId, CancellationToken cancellationToken = default);
}
