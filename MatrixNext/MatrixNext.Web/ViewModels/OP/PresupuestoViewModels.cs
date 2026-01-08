namespace MatrixNext.Web.ViewModels.OP;

public sealed record SolicitudPresupuestoCompletoRequest
{
    public long TrabajoId { get; init; }
    public long UsuarioId { get; init; }
    public bool Jornada { get; init; }
    public bool Agendamiento { get; init; }
    public bool Encuesta { get; init; }
    public bool Reclutamiento { get; init; }
    public int? General { get; init; }
    public int? NSE1y2 { get; init; }
    public int? NSE3y4 { get; init; }
    public int? NSE5y6 { get; init; }
    public int? Muestra { get; init; }
    public int? VrSugeridoContratista { get; init; }
    public string Observacion { get; init; } = string.Empty;
}

public sealed record SolicitudPresupuestoSimplificadoRequest
{
    public long TrabajoId { get; init; }
    public long UsuarioId { get; init; }
    public string Observacion { get; init; } = string.Empty;
}

public sealed record SolicitudPresupuestoState(long TrabajoId, bool TieneSolicitud, string Observacion);

public sealed class PresupuestoViewModel
{
    public long TrabajoId { get; init; }
    public bool TieneSolicitud { get; init; }
    public string ObservacionActual { get; init; } = string.Empty;
    public SolicitudPresupuestoCompletoRequest Completo { get; init; } = new();
    public SolicitudPresupuestoSimplificadoRequest Simplificado { get; init; } = new();
}
