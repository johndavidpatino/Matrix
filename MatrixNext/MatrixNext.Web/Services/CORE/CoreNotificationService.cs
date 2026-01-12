using Microsoft.AspNetCore.SignalR;

namespace MatrixNext.Web.Services.CORE;

public class CoreNotificationService : ICoreNotificationService
{
    private readonly IHubContext<CoreNotificationsHub> _hubContext;
    private readonly ILogger<CoreNotificationService> _logger;

    public CoreNotificationService(
        IHubContext<CoreNotificationsHub> hubContext,
        ILogger<CoreNotificationService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task NotifyTaskCreatedAsync(long taskId, string descripcion, long idUsuarioAsignado)
    {
        try
        {
            await _hubContext.Clients
                .User(idUsuarioAsignado.ToString())
                .SendAsync("TaskCreated", new
                {
                    TaskId = taskId,
                    Description = descripcion,
                    Timestamp = DateTime.UtcNow
                });

            _logger.LogInformation("Notificación TaskCreated enviada para tarea {TaskId}", taskId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar notificación TaskCreated para tarea {TaskId}", taskId);
        }
    }

    public async Task NotifyTaskAssignedAsync(long taskId, long idUsuarioAsignado, string rol)
    {
        try
        {
            await _hubContext.Clients
                .User(idUsuarioAsignado.ToString())
                .SendAsync("TaskAssigned", new
                {
                    TaskId = taskId,
                    Role = rol,
                    Timestamp = DateTime.UtcNow
                });

            _logger.LogInformation("Notificación TaskAssigned enviada para tarea {TaskId} a usuario {UserId}", 
                taskId, idUsuarioAsignado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar notificación TaskAssigned para tarea {TaskId}", taskId);
        }
    }

    public async Task NotifyTaskStateChangedAsync(long taskId, string newState)
    {
        try
        {
            await _hubContext.Clients
                .Group($"task-{taskId}")
                .SendAsync("TaskStateChanged", new
                {
                    TaskId = taskId,
                    NewState = newState,
                    Timestamp = DateTime.UtcNow
                });

            _logger.LogInformation("Notificación TaskStateChanged enviada para tarea {TaskId}, estado: {State}", 
                taskId, newState);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar notificación TaskStateChanged para tarea {TaskId}", taskId);
        }
    }

    public async Task NotifyTaskEscalatedAsync(long taskId, long idUsuarioDestino, string motivo)
    {
        try
        {
            await _hubContext.Clients
                .User(idUsuarioDestino.ToString())
                .SendAsync("TaskEscalated", new
                {
                    TaskId = taskId,
                    Reason = motivo,
                    Timestamp = DateTime.UtcNow
                });

            _logger.LogInformation("Notificación TaskEscalated enviada para tarea {TaskId} a usuario {UserId}", 
                taskId, idUsuarioDestino);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar notificación TaskEscalated para tarea {TaskId}", taskId);
        }
    }

    public async Task NotifyTaskClosedAsync(long taskId)
    {
        try
        {
            await _hubContext.Clients
                .Group($"task-{taskId}")
                .SendAsync("TaskClosed", new
                {
                    TaskId = taskId,
                    Timestamp = DateTime.UtcNow
                });

            _logger.LogInformation("Notificación TaskClosed enviada para tarea {TaskId}", taskId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar notificación TaskClosed para tarea {TaskId}", taskId);
        }
    }
}

/// <summary>
/// SignalR Hub para notificaciones de tareas en tiempo real.
/// </summary>
public class CoreNotificationsHub : Hub
{
    private readonly ILogger<CoreNotificationsHub> _logger;

    public CoreNotificationsHub(ILogger<CoreNotificationsHub> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Suscribirse a notificaciones de una tarea específica.
    /// </summary>
    public async Task SubscribeToTaskNotifications(long taskId)
    {
        var groupName = $"task-{taskId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Usuario {UserId} suscrito a notificaciones de tarea {TaskId}", 
            Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, taskId);
    }

    /// <summary>
    /// Suscribirse a notificaciones de tareas asignadas al usuario.
    /// </summary>
    public async Task SubscribeToUserTaskNotifications(long userId)
    {
        var groupName = $"user-tasks-{userId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Usuario {UserId} suscrito a sus notificaciones de tareas", userId);
    }

    /// <summary>
    /// Suscribirse a notificaciones de tareas escaladas.
    /// </summary>
    public async Task SubscribeToEscalatedTaskNotifications(long userId)
    {
        var groupName = $"escalated-tasks-{userId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Usuario {UserId} suscrito a notificaciones de tareas escaladas", userId);
    }

    /// <summary>
    /// Desuscribirse de notificaciones de una tarea.
    /// </summary>
    public async Task UnsubscribeFromTaskNotifications(long taskId)
    {
        var groupName = $"task-{taskId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Usuario desinscrito de notificaciones de tarea {TaskId}", taskId);
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Cliente conectado al hub CoreNotifications: {ConnectionId}", 
            Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Cliente desconectado del hub CoreNotifications: {ConnectionId}", 
            Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}
