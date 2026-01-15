using Microsoft.AspNetCore.SignalR;
using MatrixNext.Web.Models.CORE;

namespace MatrixNext.Web.Hubs;

/// <summary>
/// Hub de SignalR para eventos en tiempo real del WorkFlow
/// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T3 (notificaciones)
/// Eventos:
/// - TareaCreada: Nueva tarea asignada al usuario
/// - TareaAsignada: Usuario fue agregado como responsable
/// - EstadoCambiado: Estado de tarea cambió
/// - ObservacionAgregada: Nuevo comentario en tarea
/// - TareaVencida: Tarea llegó a fecha de vencimiento
/// </summary>
public class WorkFlowHub : Hub
{
    private readonly ILogger<WorkFlowHub> _logger;

    public WorkFlowHub(ILogger<WorkFlowHub> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Se ejecuta cuando un cliente conecta al hub.
    /// El usuario se suscribe a un grupo específico para recibir notificaciones de sus tareas.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst("sub")?.Value ?? Context.ConnectionId;
        
        // Agregar usuario a su grupo específico
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        
        _logger.LogInformation($"Usuario {userId} conectado al WorkFlowHub. ConnectionId: {Context.ConnectionId}");
        
        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Se ejecuta cuando un cliente se desconecta.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst("sub")?.Value ?? Context.ConnectionId;
        _logger.LogInformation($"Usuario {userId} desconectado del WorkFlowHub.");
        
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Cliente llama para obtener confirmación de conexión exitosa.
    /// </summary>
    public async Task Conectado()
    {
        var userId = Context.User?.FindFirst("sub")?.Value ?? "Anónimo";
        await Clients.Caller.SendAsync("ConexionExitosa", new { mensaje = "Conectado al centro de tareas", usuario = userId });
    }

    /// <summary>
    /// Notifica a un usuario que una nueva tarea fue asignada.
    /// Llamado desde GestionTareasService después de crear tarea.
    /// </summary>
    public async Task NotificarTareaCreada(long idWorkFlow, long idUsuario, string nombreTarea, string nombreTrabajo)
    {
        var notificacion = new
        {
            tipo = "TareaCreada",
            idWorkFlow,
            nombreTarea,
            nombreTrabajo,
            timestamp = DateTime.UtcNow,
            mensaje = $"Nueva tarea asignada: {nombreTarea} (Trabajo: {nombreTrabajo})"
        };

        await Clients.Group($"user_{idUsuario}").SendAsync("NotificacionTarea", notificacion);
        
        _logger.LogInformation($"Notificación TareaCreada enviada a usuario {idUsuario}. WorkFlow: {idWorkFlow}");
    }

    /// <summary>
    /// Notifica a usuarios asignados a una tarea que su estado cambió.
    /// Llamado desde GestionTareasService.CambiarEstado.
    /// </summary>
    public async Task NotificarEstadoCambiado(long idWorkFlow, string estadoAnterior, string estadoNuevo, List<long> usuariosAsignados)
    {
        var notificacion = new
        {
            tipo = "EstadoCambiado",
            idWorkFlow,
            estadoAnterior,
            estadoNuevo,
            timestamp = DateTime.UtcNow,
            mensaje = $"Estado de tarea cambió: {estadoAnterior} → {estadoNuevo}"
        };

        // Notificar a todos los usuarios asignados
        foreach (var userId in usuariosAsignados)
        {
            await Clients.Group($"user_{userId}").SendAsync("NotificacionTarea", notificacion);
        }
        
        _logger.LogInformation($"Notificación EstadoCambiado enviada. WorkFlow: {idWorkFlow}");
    }

    /// <summary>
    /// Notifica que se agregó un comentario a una tarea.
    /// </summary>
    public async Task NotificarObservacionAgregada(long idWorkFlow, long idUsuarioAutor, string nombreUsuario, string observacion, List<long> usuariosAsignados)
    {
        var notificacion = new
        {
            tipo = "ObservacionAgregada",
            idWorkFlow,
            idUsuarioAutor,
            nombreUsuario,
            observacion,
            timestamp = DateTime.UtcNow,
            mensaje = $"{nombreUsuario} agregó un comentario"
        };

        // Notificar a todos los usuarios asignados
        foreach (var userId in usuariosAsignados)
        {
            // No notificar al que escribió el comentario
            if (userId != idUsuarioAutor)
            {
                await Clients.Group($"user_{userId}").SendAsync("NotificacionTarea", notificacion);
            }
        }
        
        _logger.LogInformation($"Notificación ObservacionAgregada enviada para WorkFlow: {idWorkFlow}");
    }

    /// <summary>
    /// Notifica que una tarea fue escalada (cambio de prioridad o asignación).
    /// </summary>
    public async Task NotificarTareaEscalada(long idWorkFlow, string motivo, List<long> supervisores)
    {
        var notificacion = new
        {
            tipo = "TareaEscalada",
            idWorkFlow,
            motivo,
            timestamp = DateTime.UtcNow,
            mensaje = $"Tarea escalada: {motivo}"
        };

        // Notificar a supervisores
        foreach (var supervisorId in supervisores)
        {
            await Clients.Group($"user_{supervisorId}").SendAsync("NotificacionTarea", notificacion);
        }
        
        _logger.LogInformation($"Notificación TareaEscalada enviada. WorkFlow: {idWorkFlow}");
    }

    /// <summary>
    /// Notifica que una tarea fue anulada.
    /// </summary>
    public async Task NotificarTareaAnulada(long idWorkFlow, string motivo, List<long> usuariosAsignados)
    {
        var notificacion = new
        {
            tipo = "TareaAnulada",
            idWorkFlow,
            motivo,
            timestamp = DateTime.UtcNow,
            mensaje = $"Tarea anulada: {motivo}"
        };

        // Notificar a todos los usuarios asignados
        foreach (var userId in usuariosAsignados)
        {
            await Clients.Group($"user_{userId}").SendAsync("NotificacionTarea", notificacion);
        }
        
        _logger.LogInformation($"Notificación TareaAnulada enviada. WorkFlow: {idWorkFlow}");
    }
}
