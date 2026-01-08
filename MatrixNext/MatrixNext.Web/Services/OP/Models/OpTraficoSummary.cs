using MatrixNext.Web.Models.PY;

namespace MatrixNext.Web.Services.OP.Models;

public sealed record OpTraficoSummary(
    DateTime GeneratedAt,
    long? SelectedTrabajoId,
    string? SelectedTrabajoName,
    IReadOnlyCollection<Trabajo> Trabajos,
    IReadOnlyCollection<TraficoCiudadDto> Ciudades);
