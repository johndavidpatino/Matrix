namespace MatrixNext.Web.Services.OP.Models;

/// <summary>
/// Configuración de un trabajo OP Cuantitativo
/// </summary>
public record TrabajoOpConfiguracion
{
    public long IdTrabajo { get; init; }
    public short? TipoRecoleccionId { get; init; }
    public DateTime? FechaCreacion { get; init; }
    public long? CreadoPor { get; init; }
    public DateTime? FechaActualizacion { get; init; }
    public long? ActualizadoPor { get; init; }
}

/// <summary>
/// Información resumida de un trabajo para el grid
/// </summary>
public record TrabajoResumen
{
    public long Id { get; init; }
    public string JobBook { get; init; } = string.Empty;
    public string NombreTrabajo { get; init; } = string.Empty;
    public int Estado { get; init; }
    public string EstadoDescripcion { get; init; } = string.Empty;
    public int? Muestra { get; init; }
    public int? NoMedicion { get; init; }
    public string Metodologia { get; init; } = string.Empty;
}

/// <summary>
/// Detalle completo de un trabajo
/// </summary>
public record TrabajoDetalle
{
    public long Id { get; init; }
    public string JobBook { get; init; } = string.Empty;
    public string NombreTrabajo { get; init; } = string.Empty;
    public int Estado { get; init; }
    public string EstadoDescripcion { get; init; } = string.Empty;
    public int? Muestra { get; init; }
    public int? NoMedicion { get; init; }
    public short? OpMetodologiaId { get; init; }
    public string Metodologia { get; init; } = string.Empty;
    public long? ProyectoId { get; init; }
    public short? TipoRecoleccionId { get; init; }
    public string TipoRecoleccion { get; init; } = string.Empty;
    public bool PuedeSerCerrado { get; init; }
}

/// <summary>
/// Tipos de recolección disponibles
/// </summary>
public record TipoRecoleccion
{
    public short Id { get; init; }
    public string Recoleccion { get; init; } = string.Empty;
}
