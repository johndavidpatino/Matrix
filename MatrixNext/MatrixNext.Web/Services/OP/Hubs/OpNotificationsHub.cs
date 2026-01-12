using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.OP.Hubs
{
    /// <summary>
    /// SignalR Hub para notificaciones en tiempo real de OP_Cualitativo
    /// Los clientes se conectan a este hub para recibir notificaciones de cambios de estado
    /// </summary>
    public class OpNotificationsHub : Hub
    {
        private readonly ILogger<OpNotificationsHub> _logger;

        public OpNotificationsHub(ILogger<OpNotificationsHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"Usuario conectado: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation($"Usuario desconectado: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// El cliente se suscribe a notificaciones de una sesión específica
        /// </summary>
        public async Task SubscribeToSessionNotifications(int sesionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"session-{sesionId}");
            _logger.LogInformation($"Usuario suscrito a sesión {sesionId}");
        }

        /// <summary>
        /// El cliente se suscribe a notificaciones de una entrevista específica
        /// </summary>
        public async Task SubscribeToInterviewNotifications(int entrevistaId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"interview-{entrevistaId}");
            _logger.LogInformation($"Usuario suscrito a entrevista {entrevistaId}");
        }

        /// <summary>
        /// El cliente se suscribe a notificaciones de un moderador
        /// </summary>
        public async Task SubscribeToModeratorNotifications(int moderadorId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"moderator-{moderadorId}");
            _logger.LogInformation($"Usuario suscrito a notificaciones del moderador {moderadorId}");
        }

        /// <summary>
        /// El cliente se suscribe a notificaciones de su rol
        /// </summary>
        public async Task SubscribeToRoleNotifications(string rol)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"role-{rol}");
            _logger.LogInformation($"Usuario suscrito a notificaciones del rol {rol}");
        }

        // Métodos que pueden ser llamados desde el cliente (opcionales)
        public async Task SendPing(string mensaje)
        {
            await Clients.Caller.SendAsync("ReceivePing", mensaje, DateTime.UtcNow);
        }
    }
}
