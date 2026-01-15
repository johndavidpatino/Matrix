namespace MatrixNext.Data.Models.IT;

/// <summary>
/// DTO para el resultado de Sync_Preguntas_Get
/// Representa una pregunta de un trabajo con sus metadatos
/// </summary>
public class SyncPreguntaDto
{
    /// <summary>
    /// ID del estudio/trabajo
    /// </summary>
    public decimal EId { get; set; }

    /// <summary>
    /// ID de la pregunta
    /// </summary>
    public decimal PrId { get; set; }

    /// <summary>
    /// Nombre de la pregunta
    /// </summary>
    public string PrNombre { get; set; } = string.Empty;

    /// <summary>
    /// ID del tipo de pregunta
    /// </summary>
    public short TPId { get; set; }

    /// <summary>
    /// Valores permitidos para la respuesta (separados por coma)
    /// </summary>
    public string? PrValoresPermitidos { get; set; }

    /// <summary>
    /// Orden de la pregunta en el cuestionario
    /// </summary>
    public short PrOrden { get; set; }

    /// <summary>
    /// ID del tipo de campo de captura de datos
    /// </summary>
    public short DCPId { get; set; }

    /// <summary>
    /// Indica si la pregunta es requerida
    /// </summary>
    public bool? PrEsRequerido { get; set; }

    /// <summary>
    /// Descripción del campo de captura (e.g., "Res_Fecha", "Res_Texto")
    /// </summary>
    public string DCPDescripcion { get; set; } = string.Empty;
}
