using MatrixNext.Web.ViewModels.OP;
using MatrixNext.Web.Services.OP.Models;

namespace MatrixNext.Web.Services.OP;

public sealed record IpsExportResult(string PhysicalPath, string PublicRelativePath);

public interface IOpIpsService
{
    // Métodos legacy (mantener por compatibilidad)
    Task<IpsRevisionViewModel> ObtenerRevisionesAsync(long? trabajoId, CancellationToken cancellationToken = default);
    Task<bool> GuardarRevisionAsync(IpsRevisionUpdateModel model, CancellationToken cancellationToken = default);
    Task<IpsExportResult> ExportarRevisionesAsync(long? trabajoId, CancellationToken cancellationToken = default);

    // Nuevos métodos para Sprint 3 (OP-I01)
    Task<(bool success, List<IpsRevisionVm> data, string error)> ObtenerRevisionesAsync(
        long? trabajoId, int? procesoId, string? metodo, string? userRol);

    Task<(bool success, List<ProcesoIpsVm> data, string error)> ObtenerProcesosAsync();

    Task<(bool success, string error)> NotificarProcesoAsync(long id, long usuarioId);

    Task<(bool success, string error)> RechazarProcesoAsync(long id, long usuarioId, string observaciones);

    Task<(bool success, string error)> ActualizarEstadoAsync(
        long id, int nuevoEstado, long usuarioId, string? observaciones);

    Task<byte[]> ExportarRevisionesExcelAsync(
        long? trabajoId, int? procesoId, string? metodo, string? userRol);
}
