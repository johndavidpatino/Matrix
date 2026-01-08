using System.Collections.Generic;

namespace MatrixNext.Web.ViewModels.OP;

public sealed class PlanillasAprobacionViewModel
{
    public IReadOnlyList<PlanillaStatusViewModel> StatusTabs { get; init; } = Array.Empty<PlanillaStatusViewModel>();

    public IReadOnlyList<PlanillaRowViewModel> Planillas { get; init; } = Array.Empty<PlanillaRowViewModel>();

    public ProductivitySummaryViewModel Productivity { get; init; } = new();

    public IpsSummaryViewModel Ips { get; init; } = new();

    public IReadOnlyList<ProductivityRowViewModel> ProductividadDetalle { get; init; } = Array.Empty<ProductivityRowViewModel>();

    public IReadOnlyList<IpsRowViewModel> IpsDetalle { get; init; } = Array.Empty<IpsRowViewModel>();
}

public sealed class PlanillaStatusViewModel
{
    public string Title { get; init; } = string.Empty;
    public int Count { get; init; }
    public string Description { get; init; } = string.Empty;
    public string Badge { get; init; } = "secondary";
    public string TabId { get; init; } = string.Empty;
}

public sealed class PlanillaRowViewModel
{
    public long TrabajoId { get; init; }
    public string TrabajoNombre { get; init; } = string.Empty;
    public string Responsable { get; init; } = string.Empty;
    public string Estado { get; init; } = string.Empty;
    public DateTime FechaCarga { get; init; }
    public int Cantidad { get; init; }
    public string Observaciones { get; init; } = string.Empty;
}

public sealed class ProductivitySummaryViewModel
{
    public string Rol { get; init; } = "PMO";
    public int TotalPendientes { get; init; }
    public int TotalAprobadas { get; init; }
    public string Corte { get; init; } = "Corte 16-15";
    public string Nota { get; init; } = string.Empty;
}

public sealed class IpsSummaryViewModel
{
    public int Pendientes { get; init; }
    public int Atendidas { get; init; }
    public string UltimaActualizacion { get; init; } = string.Empty;
    public string Comentario { get; init; } = string.Empty;
}

public sealed class ProductivityRowViewModel
{
    public string Trabajo { get; init; } = string.Empty;
    public string Rol { get; init; } = string.Empty;
    public int Cantidad { get; init; }
    public DateTime Fecha { get; init; }
}

public sealed class IpsRowViewModel
{
    public string Trabajo { get; init; } = string.Empty;
    public string Pregunta { get; init; } = string.Empty;
    public string Observacion { get; init; } = string.Empty;
    public string Estado { get; init; } = string.Empty;
    public DateTime FechaHoraObservacion { get; init; }
}
