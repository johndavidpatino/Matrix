using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.Modules.TH.Contratistas.Models;

/// <summary>
/// DTO para listar contratistas
/// </summary>
public class ContratistaDto
{
    public long Identificacion { get; set; }
    public string? Nombre { get; set; }
    public string? Direccion { get; set; }
    public string? Email { get; set; }
    public bool Activo { get; set; }
    public long? CiudadId { get; set; }
    public string? CiudadNombre { get; set; }
    public long? NumeroSymphony { get; set; }
    public string? DescripcionCuenta { get; set; }
    public string? Telefono { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public int? Estado { get; set; }
    public string? EstadoNombre { get; set; }
    public string? Solicitud { get; set; }
    public string? Aprobado { get; set; }
    public string? Observaciones { get; set; }
    public int? Clasificacion { get; set; }
    public string? ClasificacionNombre { get; set; }
    public int TotalServicios { get; set; }
}

/// <summary>
/// DTO para crear/editar contratista
/// </summary>
public class ContratistaCreateEditDto
{
    [Required(ErrorMessage = "La identificación es requerida")]
    public long Identificacion { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(300, ErrorMessage = "Máximo 300 caracteres")]
    public string? Direccion { get; set; }

    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "La ciudad es requerida")]
    public long CiudadId { get; set; }

    [Required(ErrorMessage = "El número Symphony es requerido")]
    public long NumeroSymphony { get; set; }

    [StringLength(300, ErrorMessage = "Máximo 300 caracteres")]
    public string? DescripcionCuenta { get; set; }

    [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "La fecha de ingreso es requerida")]
    public DateTime FechaRegistro { get; set; }

    [Required(ErrorMessage = "El estado es requerido")]
    public int Estado { get; set; }

    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string? Solicitud { get; set; }

    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string? Aprobado { get; set; }

    [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
    public string? Observaciones { get; set; }

    [Required(ErrorMessage = "La clasificación es requerida")]
    public int Clasificacion { get; set; }
    
    /// <summary>
    /// Indica si es una actualización (true) o creación (false)
    /// </summary>
    public bool EsActualizacion { get; set; }
}

/// <summary>
/// DTO para servicios del contratista
/// </summary>
public class ContratistaServicioDto
{
    public long Id { get; set; }
    public long ContratistaId { get; set; }
    public long ServicioId { get; set; }
    public string? NombreServicio { get; set; }
    public bool Estado { get; set; }
}

/// <summary>
/// DTO para agregar servicio a contratista
/// </summary>
public class ContratistaServicioCreateDto
{
    [Required(ErrorMessage = "El contratista es requerido")]
    public long ContratistaId { get; set; }

    [Required(ErrorMessage = "El servicio es requerido")]
    public long ServicioId { get; set; }

    [Required(ErrorMessage = "El nombre del servicio es requerido")]
    public string NombreServicio { get; set; } = string.Empty;

    public bool Estado { get; set; } = true;
}

/// <summary>
/// DTO para actualizar estado de servicio
/// </summary>
public class ContratistaServicioUpdateDto
{
    public long Id { get; set; }
    public bool Estado { get; set; }
}

/// <summary>
/// DTO para log de contratistas
/// </summary>
public class ContratistaLogDto
{
    public long Id { get; set; }
    public long ContratistaId { get; set; }
    public string? Observacion { get; set; }
    public DateTime? Fecha { get; set; }
    public long? UsuarioId { get; set; }
    public string? UsuarioNombre { get; set; }
}

/// <summary>
/// DTO para combo de estados
/// </summary>
public class EstadoContratistaDto
{
    public int Id { get; set; }
    public string? Estado { get; set; }
}

/// <summary>
/// DTO para combo de servicios
/// </summary>
public class ServicioContratistaComboDto
{
    public long Id { get; set; }
    public string? Nombre { get; set; }
}

/// <summary>
/// DTO para combo de clasificaciones
/// </summary>
public class ClasificacionContratistaDto
{
    public int Id { get; set; }
    public string? Clasificacion { get; set; }
}

/// <summary>
/// DTO para combo de ciudades
/// </summary>
public class CiudadComboDto
{
    public long Id { get; set; }
    public string? Ciudad { get; set; }
}

/// <summary>
/// Parámetros de búsqueda de contratistas
/// </summary>
public class BuscarContratistasParams
{
    public long? Identificacion { get; set; }
    public string? Nombre { get; set; }
    public bool? Activo { get; set; }
}
