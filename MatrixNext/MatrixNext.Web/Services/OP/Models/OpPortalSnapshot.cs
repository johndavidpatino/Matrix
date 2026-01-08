using MatrixNext.Web.Models.PY;
using MatrixNext.Web.ViewModels;
using MatrixNext.Web.ViewModels.OP;

namespace MatrixNext.Web.Services.OP.Models;

public sealed record OpPortalSnapshot(
    DateTime GeneratedAt,
    PaginationResultVM<Trabajo> Trabajos,
    IReadOnlyDictionary<int, int> EstadoCounts,
    IReadOnlyDictionary<int, string> MetodologiasMap,
    SupervisionSummary Supervision,
    ProduccionSummary Produccion,
    IReadOnlyList<PresupuestoNotificationRow> PresupuestoNotificaciones);
