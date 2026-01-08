using System.Collections.Generic;

namespace MatrixNext.Web.ViewModels.OP;

public sealed class ProductividadViewModel
{
    public string RolActual { get; init; } = "PMO";
    public IReadOnlyList<ProductividadRowViewModel> Registros { get; init; } = Array.Empty<ProductividadRowViewModel>();
    public IReadOnlyList<string> RolesDisponibles { get; init; } = Array.Empty<string>();
}

public sealed class ProductividadRowViewModel
{
    public long TrabajoId { get; init; }
    public string Trabajo { get; init; } = string.Empty;
    public string Ciudad { get; init; } = string.Empty;
    public string Rol { get; init; } = string.Empty;
    public int Cantidad { get; init; }
    public DateTime FechaEjecucion { get; init; }
    public string Estado { get; init; } = string.Empty;
}
