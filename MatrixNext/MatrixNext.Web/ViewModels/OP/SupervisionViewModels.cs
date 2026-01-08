namespace MatrixNext.Web.ViewModels.OP;

public sealed record UsuarioDto(long Id, string Nombre);

public sealed record GuardarSupervisionRequest
{
    public long TrabajoId { get; init; }
    public long Identificacion { get; init; }
    public long OperadorId { get; init; }
    public long SupervisorId { get; init; }
    public DateTime FechaSupervision { get; init; }
    public string Observaciones { get; init; } = string.Empty;
    public bool[] CriFlags { get; init; } = new bool[13];
    public int[] ComValues { get; init; } = new int[4];
    public int[] AccValues { get; init; } = new int[4];
}

public sealed class SupervisionViewModel
{
    public long TrabajoId { get; init; }
    public string Identificacion { get; init; } = string.Empty;
    public long SupervisorId { get; init; }
    public long OperadorId { get; init; }
    public DateTime FechaSupervision { get; init; } = DateTime.UtcNow;
    public IReadOnlyList<UsuarioDto> Operadores { get; init; } = Array.Empty<UsuarioDto>();
    public IReadOnlyList<UsuarioDto> Supervisores { get; init; } = Array.Empty<UsuarioDto>();
    public GuardarSupervisionRequest Request { get; init; } = new();
    public IReadOnlyList<SupervisionHistoryRow> Historico { get; init; } = Array.Empty<SupervisionHistoryRow>();
    public SupervisionSummary Summary { get; init; } = new(0, 0, 0);
}

public sealed record SupervisionHistoryRow(int Id, DateTime Fecha, string Operador, string Supervisor, string Observacion);

public sealed record SupervisionSummary(int TotalRegistros, int RegistrosHoy, int Alertas);
