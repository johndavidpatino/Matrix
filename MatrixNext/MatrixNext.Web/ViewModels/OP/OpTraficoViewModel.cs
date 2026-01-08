using MatrixNext.Web.Models.PY;
using MatrixNext.Web.Services.OP.Models;

namespace MatrixNext.Web.ViewModels.OP;

public sealed class OpTraficoViewModel
{
    public OpTraficoSummary Summary { get; init; } = new(
        DateTime.UtcNow,
        null,
        null,
        Array.Empty<MatrixNext.Web.Models.PY.Trabajo>(),
        Array.Empty<TraficoCiudadDto>());

    public bool HasData => Summary.Trabajos.Any();
}
