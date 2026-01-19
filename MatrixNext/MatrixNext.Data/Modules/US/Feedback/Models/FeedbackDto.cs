namespace MatrixNext.Data.Modules.US.Feedback.Models;

/// <summary>
/// DTO para Asunto de Feedback
/// SP: CORE_Asunto_Get
/// </summary>
public class AsuntoDto
{
    public int Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
}

/// <summary>
/// DTO para crear Feedback
/// SP: CORE_Feedback_Add
/// </summary>
public class FeedbackCreateDto
{
    public int IdAsunto { get; set; }
    public string Mensaje { get; set; } = string.Empty;
}

/// <summary>
/// DTO para ver Feedback (CORE_Retroalimentacion)
/// </summary>
public class FeedbackDto
{
    public long Id { get; set; }
    public int? TipoMensaje { get; set; }
    public string? TipoMensajeNombre { get; set; }
    public string? Mensaje { get; set; }
    public long? IdUsuarioEnvia { get; set; }
    public string? NombreUsuarioEnvia { get; set; }
    public DateTime? FechaEnvio { get; set; }
    public bool? Solucionado { get; set; }
    public string? Respuesta { get; set; }
    public long? UsuarioResponde { get; set; }
    public string? NombreUsuarioResponde { get; set; }
    public DateTime? FechaSolucion { get; set; }
}

/// <summary>
/// DTO para actualizar respuesta de Feedback
/// </summary>
public class FeedbackUpdateDto
{
    public long Id { get; set; }
    public string? Respuesta { get; set; }
    public bool Solucionado { get; set; }
}
