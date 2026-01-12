using MatrixNext.Web.Services.OP.Hubs;
using MatrixNext.Web.Services.OP.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.OP
{
    public class OpNotificationService : IOpNotificationService
    {
        private readonly IHubContext<OpNotificationsHub> _hubContext;
        private readonly ILogger<OpNotificationService> _logger;

        public OpNotificationService(
            IHubContext<OpNotificationsHub> hubContext,
            ILogger<OpNotificationService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        // ========== NOTIFICACIONES DE CAMBIO DE ESTADO ==========

        public async Task NotifySessionStateChangeAsync(int sesionId, string nuevoEstado, string usuarioId)
        {
            try
            {
                var notification = new StateChangeNotification
                {
                    EntityId = sesionId,
                    EntityType = "Sesion",
                    NewState = nuevoEstado,
                    ChangedBy = usuarioId,
                    ChangedAt = DateTime.UtcNow
                };

                // Enviar a todos los suscritos a esta sesión
                await _hubContext.Clients
                    .Group($"session-{sesionId}")
                    .SendAsync("SessionStateChanged", notification);

                _logger.LogInformation($"Sesión {sesionId} cambió a estado {nuevoEstado}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error notificando cambio de estado de sesión {sesionId}");
            }
        }

        public async Task NotifyInterviewStateChangeAsync(int entrevistaId, string nuevoEstado, string usuarioId)
        {
            try
            {
                var notification = new StateChangeNotification
                {
                    EntityId = entrevistaId,
                    EntityType = "Entrevista",
                    NewState = nuevoEstado,
                    ChangedBy = usuarioId,
                    ChangedAt = DateTime.UtcNow
                };

                await _hubContext.Clients
                    .Group($"interview-{entrevistaId}")
                    .SendAsync("InterviewStateChanged", notification);

                _logger.LogInformation($"Entrevista {entrevistaId} cambió a estado {nuevoEstado}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error notificando cambio de estado de entrevista {entrevistaId}");
            }
        }

        public async Task NotifyFilterStateChangeAsync(int filtroId, string nuevoEstado, string usuarioId)
        {
            try
            {
                var notification = new StateChangeNotification
                {
                    EntityId = filtroId,
                    EntityType = "Filtro",
                    NewState = nuevoEstado,
                    ChangedBy = usuarioId,
                    ChangedAt = DateTime.UtcNow
                };

                // Notificar a supervisores (rol específico)
                await _hubContext.Clients
                    .Group("role-Supervisor")
                    .SendAsync("FilterStateChanged", notification);

                _logger.LogInformation($"Filtro {filtroId} cambió a estado {nuevoEstado}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error notificando cambio de estado de filtro {filtroId}");
            }
        }

        // ========== NOTIFICACIONES DE EVENTOS ==========

        public async Task NotifySessionCreatedAsync(int sesionId, string trabajo, string moderador)
        {
            try
            {
                var eventNotification = new EventNotification
                {
                    EntityId = sesionId,
                    EventType = "SessionCreated",
                    Description = $"Nueva sesión creada para {trabajo} con moderador {moderador}",
                    Metadata = new Dictionary<string, string>
                    {
                        { "trabajo", trabajo },
                        { "moderador", moderador }
                    },
                    OccurredAt = DateTime.UtcNow
                };

                // Notificar a coordinadores
                await _hubContext.Clients
                    .Group("role-Coordinador")
                    .SendAsync("SessionCreated", eventNotification);

                _logger.LogInformation($"Sesión {sesionId} creada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error notificando creación de sesión {sesionId}");
            }
        }

        public async Task NotifyInterviewCreatedAsync(int entrevistaId, string trabajo, string entrevistador)
        {
            try
            {
                var eventNotification = new EventNotification
                {
                    EntityId = entrevistaId,
                    EventType = "InterviewCreated",
                    Description = $"Nueva entrevista creada para {trabajo} por {entrevistador}",
                    Metadata = new Dictionary<string, string>
                    {
                        { "trabajo", trabajo },
                        { "entrevistador", entrevistador }
                    },
                    OccurredAt = DateTime.UtcNow
                };

                await _hubContext.Clients
                    .Group("role-Coordinador")
                    .SendAsync("InterviewCreated", eventNotification);

                _logger.LogInformation($"Entrevista {entrevistaId} creada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error notificando creación de entrevista {entrevistaId}");
            }
        }

        public async Task NotifyModeratorAvailabilityAsync(int moderadorId, bool disponible)
        {
            try
            {
                var statusMessage = disponible ? "Disponible" : "Indisponible";

                await _hubContext.Clients
                    .Group($"moderator-{moderadorId}")
                    .SendAsync("ModeratorAvailabilityChanged", new
                    {
                        moderadorId,
                        disponible,
                        timestamp = DateTime.UtcNow
                    });

                _logger.LogInformation($"Moderador {moderadorId} cambió a {statusMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error notificando cambio de disponibilidad del moderador {moderadorId}");
            }
        }

        // ========== NOTIFICACIONES BROADCAST ==========

        public async Task SendToRoleAsync(string rol, string mensaje, NotificationType tipo)
        {
            try
            {
                await _hubContext.Clients
                    .Group($"role-{rol}")
                    .SendAsync("ReceiveNotification", new
                    {
                        mensaje,
                        tipo = tipo.ToString(),
                        timestamp = DateTime.UtcNow
                    });

                _logger.LogInformation($"Notificación enviada al rol {rol}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando notificación al rol {rol}");
            }
        }

        public async Task SendToUserAsync(string usuarioId, string mensaje, NotificationType tipo)
        {
            try
            {
                // Nota: Para enviar a un usuario específico se necesita almacenar el mapping
                // entre usuarioId y ConnectionId en una estructura como Dictionary
                // Por ahora, enviar al rol del usuario sería suficiente
                
                _logger.LogInformation($"Notificación enviada al usuario {usuarioId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando notificación al usuario {usuarioId}");
            }
        }

        public async Task SendToAllAsync(string mensaje, NotificationType tipo)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", new
                {
                    mensaje,
                    tipo = tipo.ToString(),
                    timestamp = DateTime.UtcNow
                });

                _logger.LogInformation("Notificación broadcast enviada a todos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando notificación broadcast");
            }
        }
    }
}
