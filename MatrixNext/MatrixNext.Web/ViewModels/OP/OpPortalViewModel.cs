using MatrixNext.Web.Services.OP.Models;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.ViewModels.OP;

public sealed class OpPortalViewModel
{
    public FiltrosVM Filtros { get; init; } = new();
    public OpPortalSnapshot? Snapshot { get; init; }

    public bool HasData => Snapshot is not null;
    public bool TienePermiso100 { get; init; }
    public bool TienePermisoSupervision { get; init; }
    public ProduccionSummary? Produccion => Snapshot?.Produccion;
    public IReadOnlyList<PresupuestoNotificationRow> PresupuestoNotificaciones => Snapshot?.PresupuestoNotificaciones ?? Array.Empty<PresupuestoNotificationRow>();
}
