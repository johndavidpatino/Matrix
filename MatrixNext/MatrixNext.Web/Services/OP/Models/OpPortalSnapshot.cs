using MatrixNext.Web.Models.PY;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Services.OP.Models;

public sealed record OpPortalSnapshot(
    DateTime GeneratedAt,
    PaginationResultVM<Trabajo> Trabajos,
    IReadOnlyDictionary<int, int> EstadoCounts,
    IReadOnlyDictionary<int, string> MetodologiasMap);
