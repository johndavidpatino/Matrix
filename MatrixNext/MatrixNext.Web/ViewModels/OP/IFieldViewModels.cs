namespace MatrixNext.Web.ViewModels.OP;

public sealed record IFieldProjectDto(int ProjectId, string NombreProjecto, int? TrabajoId, bool? Activo);

public sealed record IFieldConfigRow(int ConfigId, string UsuarioIfield, long? Encuestador, long? Supervisor, string Usuario, DateTime? FechaConfig);

public sealed record IFieldPendingRow(int IdIfield, long? NumEncuesta, string Encuestador, string Ciudad, string FechaEncuesta, DateTime FechaSync);

public sealed class IFieldViewModel
{
    public int TipoBusqueda { get; init; }
    public IReadOnlyList<IFieldProjectDto> Projects { get; init; } = Array.Empty<IFieldProjectDto>();
    public IFieldProjectDto? SelectedProject { get; init; }
    public IReadOnlyList<IFieldConfigRow> Configuracion { get; init; } = Array.Empty<IFieldConfigRow>();
    public IReadOnlyList<IFieldPendingRow> Pendientes { get; init; } = Array.Empty<IFieldPendingRow>();
    public string JobBook { get; init; } = string.Empty;
    public string NuevaConfiguracion { get; init; } = string.Empty;
}

public sealed record IFieldAddConfigInput(int ProjectId, string UserIField, long Encuestador, long Supervisor, long UsuarioId);

