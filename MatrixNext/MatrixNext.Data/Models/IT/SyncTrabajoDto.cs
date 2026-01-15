using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.Models.IT;

/// <summary>
/// DTO para operaciones de sincronización sobre trabajos
/// </summary>
public class SyncTrabajoDto
{
    /// <summary>
    /// ID del trabajo
    /// </summary>
    [Required(ErrorMessage = "El ID del trabajo es requerido")]
    [Range(1, long.MaxValue, ErrorMessage = "El ID del trabajo debe ser mayor a 0")]
    public long TrabajoId { get; set; }
}

/// <summary>
/// DTO para actualizar una respuesta de encuesta
/// </summary>
public class SyncActualizarRespuestaDto
{
    /// <summary>
    /// ID del trabajo
    /// </summary>
    [Required(ErrorMessage = "El ID del trabajo es requerido")]
    [Range(1, long.MaxValue, ErrorMessage = "El ID del trabajo debe ser mayor a 0")]
    public long TrabajoId { get; set; }

    /// <summary>
    /// Número de sujeto/encuesta (SbjNum)
    /// </summary>
    [Required(ErrorMessage = "El número de sujeto es requerido")]
    public decimal SbjNum { get; set; }

    /// <summary>
    /// Campo de captura de datos (DCP_Descripcion, e.g., "Res_Fecha")
    /// </summary>
    [Required(ErrorMessage = "El campo DCP es requerido")]
    public string DCP { get; set; } = string.Empty;

    /// <summary>
    /// Nuevo valor para la respuesta
    /// </summary>
    [Required(ErrorMessage = "El nuevo valor es requerido")]
    public string NuevoValor { get; set; } = string.Empty;
}

/// <summary>
/// DTO para operaciones sobre encuestas piloto
/// </summary>
public class SyncEncuestaPilotoDto
{
    /// <summary>
    /// Número de sujeto/encuesta (SbjNum)
    /// </summary>
    [Required(ErrorMessage = "El número de sujeto es requerido")]
    public decimal SbjNum { get; set; }
}
