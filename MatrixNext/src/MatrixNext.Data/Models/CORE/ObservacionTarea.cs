using MatrixNext.Data.Models.Base;

namespace MatrixNext.Data.Models.CORE;

/// <summary>
/// Auditoría de cambios en tareas (WorkFlow).
/// Registra cada cambio de estado, asignación o actualización con usuario y timestamp.
/// </summary>
public class ObservacionTarea : BaseEntity
{
    /// <summary>
    /// Identificador del WorkFlow (Tarea) modificada.
    /// </summary>
    public long IdWorkFlow { get; set; }

    /// <summary>
    /// Identificador del usuario que realizó la acción.
    /// </summary>
    public long IdUsuario { get; set; }

    /// <summary>
    /// Observación o comentario sobre el cambio.
    /// Puede ser nulo si es un cambio automático.
    /// </summary>
    public string? Observacion { get; set; }

    /// <summary>
    /// Tipo de operación realizada: "Crear", "Asignar", "CambiarEstado", "Anular", "Comentario", etc.
    /// </summary>
    public string? TipoOperacion { get; set; }

    /// <summary>
    /// Timestamp de cuándo ocurrió el cambio.
    /// </summary>
    public DateTime FechaHora { get; set; } = DateTime.UtcNow;

    // Navegación

    /// <summary>
    /// Referencia a la tarea (WorkFlow) que fue modificada.
    /// </summary>
    public WorkFlow? WorkFlow { get; set; }
}
