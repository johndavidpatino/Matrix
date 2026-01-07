using MatrixNext.Data.Models.Base;

namespace MatrixNext.Data.Models.CORE;

/// <summary>
/// Relación N:N entre WorkFlow (Tareas) y Usuarios.
/// Registra qué usuario está asignado a cuál tarea y en qué rol.
/// </summary>
public class WorkFlowUsuarioAsignado : BaseEntity
{
    /// <summary>
    /// Identificador del WorkFlow (Tarea).
    /// </summary>
    public long IdWorkFlow { get; set; }

    /// <summary>
    /// Identificador del Usuario asignado.
    /// </summary>
    public long IdUsuario { get; set; }

    /// <summary>
    /// Rol del usuario en esta tarea (ej: "Ejecutor", "Revisor", "Supervisor").
    /// Puede ser nulo si se usa rol por defecto.
    /// </summary>
    public string? Rol { get; set; }

    /// <summary>
    /// Fecha de asignación.
    /// </summary>
    public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indicador de si la asignación está activa.
    /// </summary>
    public bool Activo { get; set; } = true;

    // Navegación

    /// <summary>
    /// Referencia a la tarea (WorkFlow) asignada.
    /// </summary>
    public WorkFlow? WorkFlow { get; set; }
}
