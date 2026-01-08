using System.Collections.Generic;

namespace MatrixNext.Web.ViewModels.OP;

public sealed class IpsRevisionViewModel
{
    public long? TrabajoId { get; init; }

    public IReadOnlyList<IpsRevisionRowViewModel> Revisiones { get; init; } = Array.Empty<IpsRevisionRowViewModel>();
}

public sealed class IpsRevisionRowViewModel
{
    public long Id { get; init; }
    public long TrabajoId { get; init; }
    public string Trabajo { get; init; } = string.Empty;
    public string Pregunta { get; init; } = string.Empty;
    public string Observacion { get; init; } = string.Empty;
    public string DescripcionObservacion { get; init; } = string.Empty;
    public string Estado { get; init; } = string.Empty;
    public string Instrumento { get; init; } = string.Empty;
    public DateTime FechaHoraObservacion { get; init; }
}

public sealed class IpsRevisionUpdateModel
{
    public long Id { get; init; }
    public long TrabajoId { get; init; }
    public string Observacion { get; init; } = string.Empty;
    public string DescripcionObservacion { get; init; } = string.Empty;
    public string Instrumento { get; init; } = string.Empty;
    public string RespuestaProgramador { get; init; } = string.Empty;
    public string Estado { get; init; } = string.Empty;
    public string Rechazar { get; init; } = string.Empty;
}
