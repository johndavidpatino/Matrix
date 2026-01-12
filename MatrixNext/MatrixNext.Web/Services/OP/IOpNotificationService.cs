using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.OP.Interfaces
{
    /// <summary>
    /// Servicio para notificaciones de cambios de estado en OP_Cualitativo
    /// Usa SignalR para enviar notificaciones en tiempo real a los clientes
    /// </summary>
    public interface IOpNotificationService
    {
        // ========== NOTIFICACIONES DE CAMBIO DE ESTADO ==========

        /// <summary>
        /// Notifica un cambio de estado de sesión a los usuarios conectados
        /// </summary>
        Task NotifySessionStateChangeAsync(int sesionId, string nuevoEstado, string usuarioId);

        /// <summary>
        /// Notifica un cambio de estado de entrevista
        /// </summary>
        Task NotifyInterviewStateChangeAsync(int entrevistaId, string nuevoEstado, string usuarioId);

        /// <summary>
        /// Notifica un cambio de estado de filtro
        /// </summary>
        Task NotifyFilterStateChangeAsync(int filtroId, string nuevoEstado, string usuarioId);


        // ========== NOTIFICACIONES DE EVENTOS ==========

        /// <summary>
        /// Notifica cuando se crea una nueva sesión
        /// </summary>
        Task NotifySessionCreatedAsync(int sesionId, string trabajo, string moderador);

        /// <summary>
        /// Notifica cuando se crea una nueva entrevista
        /// </summary>
        Task NotifyInterviewCreatedAsync(int entrevistaId, string trabajo, string entrevistador);

        /// <summary>
        /// Notifica cuando un moderador está disponible/indisponible
        /// </summary>
        Task NotifyModeratorAvailabilityAsync(int moderadorId, bool disponible);


        // ========== NOTIFICACIONES BROADCAST ==========

        /// <summary>
        /// Envía una notificación a todos los usuarios de un rol específico
        /// </summary>
        Task SendToRoleAsync(string rol, string mensaje, NotificationType tipo);

        /// <summary>
        /// Envía una notificación a un usuario específico
        /// </summary>
        Task SendToUserAsync(string usuarioId, string mensaje, NotificationType tipo);

        /// <summary>
        /// Envía una notificación a todos los usuarios conectados
        /// </summary>
        Task SendToAllAsync(string mensaje, NotificationType tipo);
    }

    public enum NotificationType
    {
        Info,
        Warning,
        Error,
        Success
    }

    public class StateChangeNotification
    {
        public int EntityId { get; set; }
        public string EntityType { get; set; } // "Sesion", "Entrevista", "Filtro"
        public string OldState { get; set; }
        public string NewState { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
    }

    public class EventNotification
    {
        public int EntityId { get; set; }
        public string EventType { get; set; } // "SessionCreated", "InterviewCreated", etc.
        public string Description { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}
