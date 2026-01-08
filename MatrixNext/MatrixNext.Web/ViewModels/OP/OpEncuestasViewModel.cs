using MatrixNext.Web.Models.PY;

namespace MatrixNext.Web.ViewModels.OP;

public sealed class OpEncuestasViewModel
{
    public IReadOnlyCollection<Trabajo> Trabajos { get; init; } = Array.Empty<Trabajo>();
    public string? Mensaje { get; init; }
    public bool Exito { get; init; }
}
