using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.Modules.TH.HojasVida.Models;

/// <summary>
/// DTO para mostrar una hoja de vida en la lista
/// Basado en: TH_HojasVida_Get_Result
/// </summary>
public class HojaVidaDto
{
    public long Id { get; set; }
    public byte? TipoIdentificacion { get; set; }
    public string? TipoIdentificacionNombre { get; set; }
    public string? Identificacion { get; set; }
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();
    public byte? Edad { get; set; }
    public byte? AnosExperiencia { get; set; }
    public byte? NivelIngles { get; set; }
    public string? NivelInglesDescripcion => NivelIngles switch
    {
        1 => "Básico",
        2 => "Intermedio",
        3 => "Avanzado",
        4 => "Nativo/Bilingüe",
        _ => "N/A"
    };
    public long? NumeroCelular { get; set; }
    public string? Correo { get; set; }
    public short? CiudadResidencia { get; set; }
    public string? CiudadResidenciaNombre { get; set; }
    public short? NivelEducativo { get; set; }
    public string? NivelEducativoNombre { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public short? Profesion { get; set; }
    public string? ProfesionNombre { get; set; }
    public string? Keywords { get; set; }
    public bool TieneEntrevista { get; set; }
}

/// <summary>
/// DTO para crear/editar una hoja de vida
/// </summary>
public class HojaVidaCreateEditDto
{
    public long? Id { get; set; }
    
    public byte TipoIdentificacion { get; set; }
    
    [Required(ErrorMessage = "La identificación es obligatoria")]
    [StringLength(20, ErrorMessage = "La identificación no puede exceder 20 caracteres")]
    public string Identificacion { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombres { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Los apellidos son obligatorios")]
    [StringLength(100, ErrorMessage = "Los apellidos no pueden exceder 100 caracteres")]
    public string Apellidos { get; set; } = string.Empty;
    
    public byte? Edad { get; set; }
    
    public byte? AnosExperiencia { get; set; }
    
    public byte? NivelIngles { get; set; }
    
    public long? NumeroCelular { get; set; }
    
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido")]
    [StringLength(100, ErrorMessage = "El correo no puede exceder 100 caracteres")]
    public string? Correo { get; set; }
    
    public short? CiudadResidencia { get; set; }
    
    public short? NivelEducativo { get; set; }
    
    public short? Profesion { get; set; }
    
    public bool EsActualizacion { get; set; }
}

/// <summary>
/// Parámetros de búsqueda para hojas de vida
/// </summary>
public class BuscarHojasVidaParams
{
    public long? Id { get; set; }
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public byte? NivelIngles { get; set; }
    public string? Keywords { get; set; }
    public byte? AnosExperienciaInicio { get; set; }
    public byte? AnosExperienciaFin { get; set; }
    public short? NivelEducativo { get; set; }
    public short? CiudadResidencia { get; set; }
    public bool? TieneEntrevista { get; set; }
    public short? Profesion { get; set; }
}

/// <summary>
/// DTO para entrevistas de una hoja de vida
/// Basado en: TH_HojasVida_Entrevistas_Get_Result
/// </summary>
public class HojaVidaEntrevistaDto
{
    public long Id { get; set; }
    public long HojaVidaId { get; set; }
    public DateTime FechaEntrevista { get; set; }
    public string? Observaciones { get; set; }
}

/// <summary>
/// DTO para crear una entrevista
/// </summary>
public class HojaVidaEntrevistaCreateDto
{
    [Required(ErrorMessage = "El ID de la hoja de vida es obligatorio")]
    public long HojaVidaId { get; set; }
    
    [Required(ErrorMessage = "La fecha de entrevista es obligatoria")]
    public DateTime FechaEntrevista { get; set; }
    
    [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
    public string? Observaciones { get; set; }
}

/// <summary>
/// DTO para experiencia laboral
/// Basado en: TH_HojasVida_ExperienciasLaborales_Get_Result
/// </summary>
public class HojaVidaExperienciaLaboralDto
{
    public long Id { get; set; }
    public long HojaVidaId { get; set; }
    public string? Empresa { get; set; }
    public float DuracionAnos { get; set; }
}

/// <summary>
/// DTO para crear experiencia laboral
/// </summary>
public class HojaVidaExperienciaLaboralCreateDto
{
    [Required(ErrorMessage = "El ID de la hoja de vida es obligatorio")]
    public long HojaVidaId { get; set; }
    
    [Required(ErrorMessage = "El nombre de la empresa es obligatorio")]
    [StringLength(200, ErrorMessage = "El nombre de empresa no puede exceder 200 caracteres")]
    public string Empresa { get; set; } = string.Empty;
    
    [Range(0.1, 50, ErrorMessage = "La duración debe ser entre 0.1 y 50 años")]
    public float DuracionAnos { get; set; }
}

/// <summary>
/// DTO para keywords (palabras clave)
/// </summary>
public class HojaVidaKeywordDto
{
    public long HojaVidaId { get; set; }
    public string Keyword { get; set; } = string.Empty;
}

/// <summary>
/// DTO para combo de profesiones
/// Basado en: TH_HojasVida_Profesiones_Get_Result
/// </summary>
public class ProfesionComboDto
{
    public short Id { get; set; }
    public string? Profesion { get; set; }
}

/// <summary>
/// DTO para combo de niveles educativos
/// </summary>
public class NivelEducativoComboDto
{
    public short Id { get; set; }
    public string? NivelEducativo { get; set; }
}

/// <summary>
/// DTO para combo de ciudades
/// </summary>
public class CiudadComboDto
{
    public short Id { get; set; }
    public string? Ciudad { get; set; }
}

/// <summary>
/// DTO para combo de tipos de identificación
/// </summary>
public class TipoIdentificacionComboDto
{
    public byte Id { get; set; }
    public string? TipoIdentificacion { get; set; }
}
