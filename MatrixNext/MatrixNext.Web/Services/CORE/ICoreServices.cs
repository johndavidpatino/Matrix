using MatrixNext.Web.DTOs;

namespace MatrixNext.Web.Services.CORE;

/// <summary>
/// Servicio para operaciones CRUD de tareas (WorkFlow).
/// </summary>
public interface ICoreTaskService
{
    /// <summary>Crear una nueva tarea</summary>
    Task<WorkFlowDto> CreateTaskAsync(CreateTaskDto createDto);

    /// <summary>Obtener tarea por ID</summary>
    Task<WorkFlowDto?> GetTaskByIdAsync(long taskId);

    /// <summary>Obtener tareas de un usuario con paginación</summary>
    Task<PaginatedResponse<WorkFlowDto>> GetTasksByUserAsync(long userId, int pageNumber, int pageSize);

    /// <summary>Actualizar datos de una tarea</summary>
    Task<WorkFlowDto?> UpdateTaskAsync(long taskId, UpdateTaskDto updateDto);

    /// <summary>Anular una tarea</summary>
    Task<WorkFlowDto?> CancelTaskAsync(long taskId, string? motivo);
}

/// <summary>
/// Servicio para validación y transiciones de flujo de trabajo.
/// </summary>
public interface ICoreWorkflowService
{
    /// <summary>Validar transición de estado permitida</summary>
    Task<bool> IsValidStateTransitionAsync(long taskId, string newState);

    /// <summary>Completar/cerrar una tarea</summary>
    Task<WorkFlowDto?> CompleteTaskAsync(long taskId, string? resultado);

    /// <summary>Cambiar estado de tarea</summary>
    Task<WorkFlowDto?> ChangeTaskStateAsync(long taskId, string newState, string? razon);
}

/// <summary>
/// Servicio para asignación y escalada de tareas.
/// </summary>
public interface ICoreAssignmentService
{
    /// <summary>Asignar tarea a usuario(s)</summary>
    Task<WorkFlowDto?> AssignTaskAsync(long taskId, AssignTaskDto assignDto);

    /// <summary>Escalar tarea a superior</summary>
    Task<WorkFlowDto?> EscalateTaskAsync(long taskId, EscalateTaskDto escalateDto);

    /// <summary>Reasignar tarea a otro usuario</summary>
    Task<WorkFlowDto?> ReassignTaskAsync(long taskId, long newUserId, string? motivo);

    /// <summary>Obtener usuarios disponibles para asignar tarea</summary>
    Task<List<object>> GetAvailableUsersAsync(string modulo);
}

/// <summary>
/// Servicio para notificaciones de tareas en tiempo real (SignalR).
/// </summary>
public interface ICoreNotificationService
{
    /// <summary>Notificar creación de tarea</summary>
    Task NotifyTaskCreatedAsync(long taskId, string descripcion, long idUsuarioAsignado);

    /// <summary>Notificar asignación de tarea</summary>
    Task NotifyTaskAssignedAsync(long taskId, long idUsuarioAsignado, string rol);

    /// <summary>Notificar cambio de estado</summary>
    Task NotifyTaskStateChangedAsync(long taskId, string newState);

    /// <summary>Notificar escalada de tarea</summary>
    Task NotifyTaskEscalatedAsync(long taskId, long idUsuarioDestino, string motivo);

    /// <summary>Notificar cierre de tarea</summary>
    Task NotifyTaskClosedAsync(long taskId);
}

/// <summary>
/// Servicio para auditoría y comentarios en tareas.
/// </summary>
public interface ICoreAuditService
{
    /// <summary>Registrar cambio de tarea en auditoría</summary>
    Task LogTaskChangeAsync(long taskId, long idUsuario, string tipoOperacion, string? detalles);

    /// <summary>Agregar comentario a una tarea</summary>
    Task<object> AddTaskCommentAsync(long taskId, long idUsuario, string comentario);

    /// <summary>Obtener historial de cambios de una tarea</summary>
    Task<List<object>> GetTaskHistoryAsync(long taskId);

    /// <summary>Obtener comentarios de una tarea</summary>
    Task<List<object>> GetTaskCommentsAsync(long taskId);
}
