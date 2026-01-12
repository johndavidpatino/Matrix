namespace MatrixNext.Web.DTOs;

/// <summary>
/// DTO para crear una nueva tarea (WorkFlow) alineado al modelo.
/// </summary>
public class CreateTaskDto
{
    /// <summary>ID del trabajo asociado</summary>
    public required long IdTrabajo { get; set; }

    /// <summary>ID de la tarea (catálogo CORE_Tareas)</summary>
    public required long IdTarea { get; set; }

    /// <summary>Tipo de hilo (workflow)</summary>
    public required int IdTipoHilo { get; set; }

    /// <summary>Prioridad numérica (1=Normal, 2=Alta, 3=Baja)</summary>
    public int Prioridad { get; set; } = 1;

    /// <summary>Observaciones o descripción inicial</summary>
    public string? Observaciones { get; set; }

    /// <summary>Fecha de vencimiento (opcional)</summary>
    public DateTime? FechaVencimiento { get; set; }
}

/// <summary>
/// DTO para actualizar datos de una tarea.
/// </summary>
public class UpdateTaskDto
{
    /// <summary>Nuevas observaciones (opcional)</summary>
    public string? Observaciones { get; set; }

    /// <summary>Nueva prioridad (opcional)</summary>
    public int? Prioridad { get; set; }

    /// <summary>Nueva fecha de vencimiento (opcional)</summary>
    public DateTime? FechaVencimiento { get; set; }
}

/// <summary>
/// DTO para asignar una tarea a usuarios.
/// </summary>
public class AssignTaskDto
{
    /// <summary>IDs de los usuarios a asignar</summary>
    public required List<long> IdUsuarios { get; set; }

    /// <summary>Rol de cada usuario: Ejecutor, Revisor, Supervisor</summary>
    public string Rol { get; set; } = "Ejecutor";

    /// <summary>Comentario sobre la asignación (opcional)</summary>
    public string? Comentario { get; set; }
}

/// <summary>
/// DTO para escalar una tarea.
/// </summary>
public class EscalateTaskDto
{
    /// <summary>ID del usuario que recibirá la tarea escalada</summary>
    public required long IdUsuarioDestino { get; set; }

    /// <summary>Motivo de la escalada</summary>
    public required string Motivo { get; set; }

    /// <summary>Comentario adicional (opcional)</summary>
    public string? Comentario { get; set; }
}

/// <summary>
/// DTO de lectura para una tarea (WorkFlow).
/// </summary>
public class WorkFlowDto
{
    public long Id { get; set; }
    public long IdTrabajo { get; set; }
    public long IdTarea { get; set; }
    public int IdTipoHilo { get; set; }
    public string? Estado { get; set; }
    public int Prioridad { get; set; }
    public string? Observaciones { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaVencimiento { get; set; }
}

/// <summary>
/// DTO para respuesta paginada.
/// </summary>
public class PaginatedResponse<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalItems { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (TotalItems + PageSize - 1) / PageSize;
}
