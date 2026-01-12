/**
 * Módulo de Notificaciones para OP_Cualitativo
 * Usa SignalR para recibir notificaciones en tiempo real
 */

const OpNotificationsClient = (() => {
    'use strict';

    let connection = null;
    let isConnected = false;

    // ========== INICIALIZACIÓN ==========
    const init = async () => {
        connection = new signalR.HubConnectionBuilder()
            .withUrl("/hubs/op-notifications")
            .withAutomaticReconnect()
            .build();

        // Event handlers
        connection.on("SessionStateChanged", onSessionStateChanged);
        connection.on("InterviewStateChanged", onInterviewStateChanged);
        connection.on("FilterStateChanged", onFilterStateChanged);
        connection.on("SessionCreated", onSessionCreated);
        connection.on("InterviewCreated", onInterviewCreated);
        connection.on("ModeratorAvailabilityChanged", onModeratorAvailabilityChanged);
        connection.on("ReceiveNotification", onReceiveNotification);

        // Reconexión
        connection.onreconnected(() => {
            console.log("Reconectado a notificaciones");
            isConnected = true;
        });

        connection.onreconnecting(() => {
            console.log("Intentando reconectar a notificaciones...");
        });

        connection.onclose(() => {
            console.log("Desconectado de notificaciones");
            isConnected = false;
        });

        try {
            await connection.start();
            isConnected = true;
            console.log("Conectado a notificaciones");
        } catch (error) {
            console.error("Error conectando a notificaciones:", error);
            setTimeout(init, 5000); // Reintentar en 5 segundos
        }
    };

    // ========== EVENT HANDLERS ==========

    const onSessionStateChanged = (notification) => {
        console.log("Cambio de estado de sesión:", notification);
        showNotification(
            `Sesión ${notification.entityId} cambió a ${notification.newState}`,
            'info'
        );
        // Disparar evento personalizado para que otros módulos se enturen
        window.dispatchEvent(new CustomEvent('session-state-changed', { detail: notification }));
    };

    const onInterviewStateChanged = (notification) => {
        console.log("Cambio de estado de entrevista:", notification);
        showNotification(
            `Entrevista ${notification.entityId} cambió a ${notification.newState}`,
            'info'
        );
        window.dispatchEvent(new CustomEvent('interview-state-changed', { detail: notification }));
    };

    const onFilterStateChanged = (notification) => {
        console.log("Cambio de estado de filtro:", notification);
        showNotification(
            `Filtro ${notification.entityId} cambió a ${notification.newState}`,
            'warning'
        );
        window.dispatchEvent(new CustomEvent('filter-state-changed', { detail: notification }));
    };

    const onSessionCreated = (eventNotification) => {
        console.log("Nueva sesión creada:", eventNotification);
        showNotification(
            `Nueva sesión para ${eventNotification.metadata.trabajo}`,
            'success'
        );
        window.dispatchEvent(new CustomEvent('session-created', { detail: eventNotification }));
    };

    const onInterviewCreated = (eventNotification) => {
        console.log("Nueva entrevista creada:", eventNotification);
        showNotification(
            `Nueva entrevista para ${eventNotification.metadata.trabajo}`,
            'success'
        );
        window.dispatchEvent(new CustomEvent('interview-created', { detail: eventNotification }));
    };

    const onModeratorAvailabilityChanged = (data) => {
        console.log("Cambio de disponibilidad del moderador:", data);
        const status = data.disponible ? 'Disponible' : 'Indisponible';
        showNotification(
            `Moderador ${data.moderadorId} ahora está ${status}`,
            'info'
        );
        window.dispatchEvent(new CustomEvent('moderator-availability-changed', { detail: data }));
    };

    const onReceiveNotification = (notification) => {
        console.log("Notificación recibida:", notification);
        showNotification(notification.mensaje, notification.tipo.toLowerCase());
    };

    // ========== SUSCRIPCIONES ==========

    const subscribeToSessionNotifications = async (sesionId) => {
        if (connection && isConnected) {
            try {
                await connection.invoke("SubscribeToSessionNotifications", sesionId);
                console.log(`Suscrito a notificaciones de sesión ${sesionId}`);
            } catch (error) {
                console.error("Error suscribiéndose a sesión:", error);
            }
        }
    };

    const subscribeToInterviewNotifications = async (entrevistaId) => {
        if (connection && isConnected) {
            try {
                await connection.invoke("SubscribeToInterviewNotifications", entrevistaId);
                console.log(`Suscrito a notificaciones de entrevista ${entrevistaId}`);
            } catch (error) {
                console.error("Error suscribiéndose a entrevista:", error);
            }
        }
    };

    const subscribeToModeratorNotifications = async (moderadorId) => {
        if (connection && isConnected) {
            try {
                await connection.invoke("SubscribeToModeratorNotifications", moderadorId);
                console.log(`Suscrito a notificaciones del moderador ${moderadorId}`);
            } catch (error) {
                console.error("Error suscribiéndose a moderador:", error);
            }
        }
    };

    const subscribeToRoleNotifications = async (rol) => {
        if (connection && isConnected) {
            try {
                await connection.invoke("SubscribeToRoleNotifications", rol);
                console.log(`Suscrito a notificaciones del rol ${rol}`);
            } catch (error) {
                console.error("Error suscribiéndose a rol:", error);
            }
        }
    };

    // ========== UTILIDADES ==========

    const showNotification = (message, type = 'info') => {
        const toastHtml = `
            <div class="toast align-items-center text-white bg-${getBgColor(type)} border-0" 
                 role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body">
                        ${message}
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                </div>
            </div>
        `;
        
        const container = document.createElement('div');
        container.className = 'toast-container position-fixed top-0 end-0 p-3';
        container.innerHTML = toastHtml;
        document.body.appendChild(container);

        const toastElement = container.querySelector('.toast');
        const toast = new bootstrap.Toast(toastElement);
        toast.show();

        // Limpiar elemento después de que desaparezca
        setTimeout(() => container.remove(), 5000);
    };

    const getBgColor = (type) => {
        const colors = {
            'success': 'success',
            'error': 'danger',
            'warning': 'warning',
            'info': 'info'
        };
        return colors[type] || 'info';
    };

    // ========== API PÚBLICA ==========
    return {
        init,
        subscribeToSessionNotifications,
        subscribeToInterviewNotifications,
        subscribeToModeratorNotifications,
        subscribeToRoleNotifications,
        isConnected: () => isConnected
    };
})();

// Inicializar cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', OpNotificationsClient.init);
