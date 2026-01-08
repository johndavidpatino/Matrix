using MatrixNext.Web.ViewModels.OP;

namespace MatrixNext.Web.Services.OP;

public interface IOpPresupuestosService
{
    Task<SolicitudPresupuestoState?> ObtenerEstadoAsync(long trabajoId, CancellationToken cancellationToken = default);
    Task<bool> GuardarSolicitudCompletaAsync(SolicitudPresupuestoCompletoRequest request, CancellationToken cancellationToken = default);
    Task<bool> GuardarSolicitudSimplificadaAsync(SolicitudPresupuestoSimplificadoRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PresupuestoNotificationRow>> ObtenerUltimasSolicitudesAsync(int limit = 5, CancellationToken cancellationToken = default);
}
