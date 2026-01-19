using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.Modules.TH.Capacitaciones.Models;

/// <summary>
/// DTO para listar capacitaciones
/// </summary>
public class CapacitacionDto
{
    public long Id { get; set; }
    public string? Ubicacion { get; set; }
    public DateTime? Fecha { get; set; }
    public byte? Duracion { get; set; }
    public string? Actividad { get; set; }
    public long? ResponsableId { get; set; }
    public string? ResponsableNombre { get; set; }
    public string? Capacitador { get; set; }
    public string? ObjetivoActividad { get; set; }
    public string? ModoEvaluacion { get; set; }
    public long? TrabajoId { get; set; }
    public string? NumeroTrabajo { get; set; }
    public int TotalParticipantes { get; set; }
    public int TotalAprobados { get; set; }
}

/// <summary>
/// DTO para crear/editar capacitación
/// </summary>
public class CapacitacionCreateEditDto
{
    public long Id { get; set; }

    [Required(ErrorMessage = "La ubicación es requerida")]
    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string Ubicacion { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha es requerida")]
    public DateTime Fecha { get; set; }

    [Required(ErrorMessage = "La duración es requerida")]
    [Range(1, 255, ErrorMessage = "La duración debe estar entre 1 y 255 horas")]
    public byte Duracion { get; set; }

    [Required(ErrorMessage = "La actividad es requerida")]
    [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
    public string Actividad { get; set; } = string.Empty;

    [Required(ErrorMessage = "El responsable es requerido")]
    public long ResponsableId { get; set; }

    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string? Capacitador { get; set; }

    [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
    public string? ObjetivoActividad { get; set; }

    [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
    public string? ModoEvaluacion { get; set; }

    public long? TrabajoId { get; set; }
}

/// <summary>
/// DTO para participantes de una capacitación
/// </summary>
public class CapacitacionParticipanteDto
{
    public long Id { get; set; }
    public long CapacitacionId { get; set; }
    public long ParticipanteId { get; set; }
    public string? ParticipanteNombre { get; set; }
    public string? ParticipanteIdentificacion { get; set; }
    public string? Eficacia { get; set; }
    public string? OportunidadMejora { get; set; }
    public bool Aprobo { get; set; }
    
    // Propiedades adicionales para la vista
    public string? Cedula => ParticipanteIdentificacion;
    public string? NombreCompleto => ParticipanteNombre;
    public bool Asistio { get; set; }
    public string? Observaciones { get; set; }
}

/// <summary>
/// DTO para agregar participante a capacitación
/// </summary>
public class CapacitacionParticipanteCreateDto
{
    [Required(ErrorMessage = "La capacitación es requerida")]
    public long CapacitacionId { get; set; }

    [Required(ErrorMessage = "El participante es requerido")]
    public long ParticipanteId { get; set; }

    public string? Eficacia { get; set; }
    public string? OportunidadMejora { get; set; }
    public bool Aprobo { get; set; }
}

/// <summary>
/// DTO para actualizar participante
/// </summary>
public class CapacitacionParticipanteUpdateDto
{
    public long Id { get; set; }
    public long CapacitacionId { get; set; }
    public long ParticipanteId { get; set; }
    public string? Eficacia { get; set; }
    public string? OportunidadMejora { get; set; }
    public bool Aprobo { get; set; }
}

/// <summary>
/// DTO para buscar personas disponibles para agregar como participantes
/// </summary>
public class PersonaCapacitacionDto
{
    public long Id { get; set; }
    public string? Identificacion { get; set; }
    public string? Nombre { get; set; }
    public long? ContratistaId { get; set; }
    public string? NombreContratista { get; set; }
}

/// <summary>
/// Parámetros para buscar personas
/// </summary>
public class BuscarPersonasCapacitacionParams
{
    public long? Identificacion { get; set; }
    public string? Nombre { get; set; }
    public long? ContratistaId { get; set; }
    public string? NombreContratista { get; set; }
    public long? CapacitacionId { get; set; }
    public int? SonParticipantes { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

/// <summary>
/// DTO para combo de responsables
/// </summary>
public class ResponsableComboDto
{
    public long Id { get; set; }
    public string? Nombre { get; set; }
}
