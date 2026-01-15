/**
 * Cliente SignalR para notificaciones de WorkFlow en tiempo real
 * Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T3 (notificaciones)
 * 
 * Uso:
 * 1. Incluir en layout: <script src="/js/workflow-signalr-client.js"></script>
 * 2. En views: <script>WorkFlowNotifications.conectar();</script>
 */

const WorkFlowNotifications = (() => {
    let connection = null;
    let isConnected = false;

    const connect = async () => {
        if (isConnected) {
            console.warn('[WorkFlow] Ya conectado a SignalR');
            return;
        }

        try {
            // Crear conexión con el hub
            connection = new signalR.HubConnectionBuilder()
                .withUrl('/workflowHub', {
                    withCredentials: true
                })
                .withAutomaticReconnect([1000, 3000, 5000, 10000, 30000])
                .withServerTimeout(30000)
                .configureLogging(signalR.LogLevel.Warning)
                .build();

            // Evento: Conexión exitosa
            connection.on('ConexionExitosa', (data) => {
                console.log('[WorkFlow] ✅ Conexión exitosa:', data);
                isConnected = true;
                mostrarNotificacion(data.mensaje, 'success');
            });

            // Evento: Notificación de tarea
            connection.on('NotificacionTarea', (notificacion) => {
                console.log('[WorkFlow] 📩 Notificación recibida:', notificacion);
                procesarNotificacion(notificacion);
            });

            // Evento: Desconexión
            connection.onclose(() => {
                console.log('[WorkFlow] ⚠️ Desconectado de SignalR');
                isConnected = false;
            });

            // Evento: Reconexión
            connection.onreconnected(() => {
                console.log('[WorkFlow] ✅ Reconectado a SignalR');
                isConnected = true;
                mostrarNotificacion('Reconectado al centro de tareas', 'info');
            });

            // Iniciar conexión
            await connection.start();
            console.log('[WorkFlow] ✅ Iniciando conexión SignalR...');

            // Confirmar conexión
            await connection.invoke('Conectado');
        } catch (error) {
            console.error('[WorkFlow] ❌ Error conectando a SignalR:', error);
            // Reintentar en 5 segundos
            setTimeout(connect, 5000);
        }
    };

    const procesarNotificacion = (notificacion) => {
        const { tipo, idWorkFlow, mensaje } = notificacion;

        switch (tipo) {
            case 'TareaCreada':
                onTareaCreada(notificacion);
                break;
            case 'EstadoCambiado':
                onEstadoCambiado(notificacion);
                break;
            case 'ObservacionAgregada':
                onObservacionAgregada(notificacion);
                break;
            case 'TareaEscalada':
                onTareaEscalada(notificacion);
                break;
            case 'TareaAnulada':
                onTareaAnulada(notificacion);
                break;
            default:
                console.warn('[WorkFlow] Tipo de notificación desconocido:', tipo);
        }

        // Mostrar toast genérico
        mostrarNotificacion(mensaje, getTipoToast(tipo));

        // Emitir evento custom para que otras partes del app escuchen
        window.dispatchEvent(new CustomEvent('workflowNotification', { detail: notificacion }));
    };

    const onTareaCreada = (notificacion) => {
        const { idWorkFlow, nombreTarea, nombreTrabajo } = notificacion;
        
        // Reproducir sonido de notificación (opcional)
        reproducirSonido('ding');

        // Actualizar badge de tareas si existe
        actualizarBadgeTareas(1);

        // Log para analytics
        console.log(`[WorkFlow] Nueva tarea creada: ${nombreTarea} en trabajo ${nombreTrabajo}`);
    };

    const onEstadoCambiado = (notificacion) => {
        const { idWorkFlow, estadoAnterior, estadoNuevo } = notificacion;
        
        console.log(`[WorkFlow] Estado de tarea ${idWorkFlow} cambió: ${estadoAnterior} → ${estadoNuevo}`);

        // Si la vista de detalles está abierta, refrescar
        if (window.location.pathname.includes('/gestionar-tareas/') && 
            window.location.pathname.includes(idWorkFlow)) {
            location.reload();
        }
    };

    const onObservacionAgregada = (notificacion) => {
        const { idWorkFlow, nombreUsuario, observacion } = notificacion;
        
        console.log(`[WorkFlow] ${nombreUsuario} agregó comentario en tarea ${idWorkFlow}`);

        // Si el modal de detalles está abierto, refrescar
        const modal = document.getElementById('modalDetallesTarea');
        if (modal && bootstrap.Modal.getInstance(modal)) {
            // Recargar observaciones en el modal
            recargarObservacionesModal(idWorkFlow);
        }
    };

    const onTareaEscalada = (notificacion) => {
        const { idWorkFlow, motivo } = notificacion;
        
        console.log(`[WorkFlow] Tarea ${idWorkFlow} escalada: ${motivo}`);
        reproducirSonido('alert');
    };

    const onTareaAnulada = (notificacion) => {
        const { idWorkFlow, motivo } = notificacion;
        
        console.log(`[WorkFlow] Tarea ${idWorkFlow} anulada: ${motivo}`);

        // Si la tarea está en el grid, marcarla como anulada
        const fila = document.querySelector(`tr[data-id="${idWorkFlow}"]`);
        if (fila) {
            fila.classList.add('table-danger');
        }
    };

    const mostrarNotificacion = (mensaje, tipo = 'info') => {
        // Buscar componente toast existente
        const toastContainer = document.getElementById('toastContainer') || createToastContainer();

        const toastEl = document.createElement('div');
        toastEl.className = `toast align-items-center text-white bg-${getTipoToastBg(tipo)} border-0`;
        toastEl.setAttribute('role', 'alert');
        toastEl.innerHTML = `
            <div class="d-flex">
                <div class="toast-body">
                    ${getIconoNotificacion(tipo)} ${mensaje}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
            </div>
        `;

        toastContainer.appendChild(toastEl);
        const toast = new bootstrap.Toast(toastEl);
        toast.show();

        // Remover después de 5 segundos
        setTimeout(() => toastEl.remove(), 5000);
    };

    const getTipoToast = (tipo) => {
        switch (tipo) {
            case 'TareaCreada':
                return 'success';
            case 'EstadoCambiado':
                return 'info';
            case 'ObservacionAgregada':
                return 'info';
            case 'TareaEscalada':
                return 'warning';
            case 'TareaAnulada':
                return 'danger';
            default:
                return 'info';
        }
    };

    const getTipoToastBg = (tipo) => {
        const map = { success: 'success', info: 'info', warning: 'warning', danger: 'danger', error: 'danger' };
        return map[tipo] || 'info';
    };

    const getIconoNotificacion = (tipo) => {
        const iconos = {
            success: '<i class="fas fa-check-circle"></i>',
            info: '<i class="fas fa-info-circle"></i>',
            warning: '<i class="fas fa-exclamation-circle"></i>',
            danger: '<i class="fas fa-times-circle"></i>'
        };
        return iconos[tipo] || iconos.info;
    };

    const createToastContainer = () => {
        const container = document.createElement('div');
        container.id = 'toastContainer';
        container.className = 'toast-container position-fixed top-0 end-0 p-3';
        document.body.appendChild(container);
        return container;
    };

    const reproducirSonido = (tipo) => {
        // Usar Web Audio API para generar sonido simple
        try {
            const audioContext = new (window.AudioContext || window.webkitAudioContext)();
            const oscillator = audioContext.createOscillator();
            const gainNode = audioContext.createGain();

            oscillator.connect(gainNode);
            gainNode.connect(audioContext.destination);

            if (tipo === 'ding') {
                oscillator.frequency.value = 800;
                gainNode.gain.setValueAtTime(0.1, audioContext.currentTime);
                gainNode.gain.exponentialRampToValueAtTime(0.01, audioContext.currentTime + 0.1);
                oscillator.start();
                oscillator.stop(audioContext.currentTime + 0.1);
            } else if (tipo === 'alert') {
                oscillator.frequency.value = 1000;
                gainNode.gain.setValueAtTime(0.2, audioContext.currentTime);
                gainNode.gain.exponentialRampToValueAtTime(0.01, audioContext.currentTime + 0.2);
                oscillator.start();
                oscillator.stop(audioContext.currentTime + 0.2);
            }
        } catch (e) {
            console.warn('[WorkFlow] No se pudo reproducir sonido:', e);
        }
    };

    const actualizarBadgeTareas = (cantidad) => {
        const badge = document.getElementById('badgeTareasCount');
        if (badge) {
            const count = parseInt(badge.textContent) || 0;
            badge.textContent = count + cantidad;
        }
    };

    const recargarObservacionesModal = async (idWorkFlow) => {
        try {
            const res = await fetch(`/api/core/gestionar-tareas/observaciones/${idWorkFlow}`);
            if (res.ok) {
                // Actualizar lista de observaciones en el modal
                const observaciones = await res.json();
                const listContainer = document.getElementById('observacionesList');
                if (listContainer) {
                    listContainer.innerHTML = observaciones.length > 0 
                        ? observaciones.map(o => `
                            <div class="list-group-item py-2">
                                <div class="d-flex justify-content-between">
                                    <strong>${o.usuario}</strong>
                                    <small class="text-muted">${new Date(o.fechaCreacion).toLocaleString()}</small>
                                </div>
                                <p class="mb-0 small">${o.observacion}</p>
                            </div>
                        `).join('')
                        : '<p class="text-muted text-center py-3">Sin observaciones</p>';
                }
            }
        } catch (error) {
            console.error('[WorkFlow] Error recargando observaciones:', error);
        }
    };

    const desconectar = async () => {
        if (connection) {
            await connection.stop();
            isConnected = false;
            console.log('[WorkFlow] ✅ Desconectado de SignalR');
        }
    };

    // API pública
    return {
        conectar: connect,
        desconectar: desconectar,
        estáConectado: () => isConnected,
        getConnection: () => connection
    };
})();

// Auto-conectar cuando el DOM está listo
document.addEventListener('DOMContentLoaded', () => {
    // Solo conectar si hay usuario autenticado
    const userId = document.querySelector('input[name="UserId"]')?.value;
    if (userId) {
        WorkFlowNotifications.conectar();
    }
});

// Desconectar al cerrar la página
window.addEventListener('beforeunload', () => {
    WorkFlowNotifications.desconectar();
});
