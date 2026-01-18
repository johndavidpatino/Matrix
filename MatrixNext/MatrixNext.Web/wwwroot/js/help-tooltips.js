/**
 * MatrixNext - Ayudas Contextuales UX
 * Inicializa tooltips, popovers y helpers de UI
 */

(function () {
    'use strict';

    // Inicializar cuando el DOM esté listo
    document.addEventListener('DOMContentLoaded', function () {
        initTooltips();
        initPopovers();
        initHelpButtons();
    });

    /**
     * Inicializa todos los tooltips de Bootstrap
     */
    function initTooltips() {
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.forEach(function (tooltipTriggerEl) {
            new bootstrap.Tooltip(tooltipTriggerEl, {
                trigger: 'hover focus',
                delay: { show: 200, hide: 100 }
            });
        });
    }

    /**
     * Inicializa todos los popovers de Bootstrap
     */
    function initPopovers() {
        var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
        popoverTriggerList.forEach(function (popoverTriggerEl) {
            new bootstrap.Popover(popoverTriggerEl, {
                trigger: 'focus',
                html: true
            });
        });
    }

    /**
     * Inicializa botones de ayuda contextual
     */
    function initHelpButtons() {
        document.querySelectorAll('[data-help-topic]').forEach(function (btn) {
            btn.addEventListener('click', function () {
                var topic = this.getAttribute('data-help-topic');
                showHelpModal(topic);
            });
        });
    }

    /**
     * Muestra modal de ayuda contextual
     * @param {string} topic - Tema de ayuda a mostrar
     */
    function showHelpModal(topic) {
        var helpContent = getHelpContent(topic);
        if (!helpContent) return;

        // Verificar si existe el modal
        var modalEl = document.getElementById('helpModal');
        if (!modalEl) {
            // Crear modal dinámicamente si no existe
            modalEl = createHelpModal();
        }

        document.getElementById('helpModalTitle').textContent = helpContent.title;
        document.getElementById('helpModalBody').innerHTML = helpContent.body;

        var modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.show();
    }

    /**
     * Crea el modal de ayuda si no existe
     */
    function createHelpModal() {
        var modalHtml = `
            <div class="modal fade" id="helpModal" tabindex="-1">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header bg-primary text-white">
                            <h5 class="modal-title" id="helpModalTitle">
                                <i class="bi bi-question-circle me-2"></i>Ayuda
                            </h5>
                            <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                        </div>
                        <div class="modal-body" id="helpModalBody"></div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>
        `;
        document.body.insertAdjacentHTML('beforeend', modalHtml);
        return document.getElementById('helpModal');
    }

    /**
     * Obtiene el contenido de ayuda según el tema
     * @param {string} topic - Tema de ayuda
     * @returns {object} Objeto con title y body
     */
    function getHelpContent(topic) {
        var helpTopics = {
            // TH - Talento Humano
            'th-empleados': {
                title: 'Administración de Empleados',
                body: `
                    <p>En esta sección puede gestionar la información de los empleados:</p>
                    <ul>
                        <li><strong>Datos Generales:</strong> Identificación, nombre, foto</li>
                        <li><strong>Datos Laborales:</strong> Cargo, área, sede, tipo contrato</li>
                        <li><strong>Datos Personales:</strong> Dirección, contacto, emergencia</li>
                        <li><strong>Nómina:</strong> Información de pagos y deducciones</li>
                    </ul>
                    <div class="alert alert-info small">
                        <i class="bi bi-info-circle me-1"></i>
                        Los cambios en nómina requieren aprobación de RRHH.
                    </div>
                `
            },
            'th-ausencias': {
                title: 'Solicitudes de Ausencia',
                body: `
                    <p>Tipos de ausencia disponibles:</p>
                    <ul>
                        <li><strong>Vacaciones:</strong> Días de descanso remunerado</li>
                        <li><strong>Permisos:</strong> Ausencias cortas justificadas</li>
                        <li><strong>Licencias:</strong> Ausencias prolongadas</li>
                        <li><strong>Incapacidades:</strong> Por enfermedad o accidente</li>
                    </ul>
                    <div class="alert alert-warning small">
                        <i class="bi bi-exclamation-triangle me-1"></i>
                        Las vacaciones requieren mínimo 5 días de anticipación.
                    </div>
                `
            },
            // US - Usuarios
            'us-usuarios': {
                title: 'Gestión de Usuarios',
                body: `
                    <p>Administre los usuarios del sistema:</p>
                    <ul>
                        <li><strong>Crear:</strong> Nuevos usuarios con permisos base</li>
                        <li><strong>Editar:</strong> Modificar datos y permisos</li>
                        <li><strong>Activar/Desactivar:</strong> Control de acceso</li>
                    </ul>
                    <div class="alert alert-info small">
                        <i class="bi bi-info-circle me-1"></i>
                        La contraseña inicial se envía por correo.
                    </div>
                `
            },
            // PY - Proyectos
            'py-proyectos': {
                title: 'Gestión de Proyectos',
                body: `
                    <p>Administre proyectos y trabajos de investigación:</p>
                    <ul>
                        <li><strong>JobBook:</strong> Código único del proyecto</li>
                        <li><strong>Trabajos:</strong> Unidades de trabajo dentro del proyecto</li>
                        <li><strong>Metodología:</strong> F2F, CATI, Online, etc.</li>
                    </ul>
                `
            },
            // OP - Operaciones
            'op-trabajos': {
                title: 'Gestión de Trabajos',
                body: `
                    <p>Portal de operaciones para trabajos de campo:</p>
                    <ul>
                        <li><strong>Muestra:</strong> Distribución por ciudad</li>
                        <li><strong>Estimación:</strong> Planeación de producción</li>
                        <li><strong>Tráfico:</strong> Movimiento de encuestas</li>
                        <li><strong>IPS:</strong> Control de tareas</li>
                    </ul>
                `
            },
            // GD - Gestión Documental
            'gd-documentos': {
                title: 'Gestión Documental',
                body: `
                    <p>Sistema de gestión de documentos ISO:</p>
                    <ul>
                        <li><strong>Maestro:</strong> Catálogo de documentos</li>
                        <li><strong>Solicitudes:</strong> Crear/modificar documentos</li>
                        <li><strong>Aprobaciones:</strong> Workflow de revisión</li>
                        <li><strong>Repositorio:</strong> Archivos versionados</li>
                    </ul>
                `
            },
            // CORE - Workflow
            'core-tareas': {
                title: 'Gestión de Tareas',
                body: `
                    <p>Sistema de workflow y tareas:</p>
                    <ul>
                        <li><strong>Configuración:</strong> Definir plantillas de tareas</li>
                        <li><strong>Asignación:</strong> Responsables por tarea</li>
                        <li><strong>Seguimiento:</strong> Estados y observaciones</li>
                        <li><strong>Indicadores:</strong> Métricas de cumplimiento</li>
                    </ul>
                `
            }
        };

        return helpTopics[topic] || null;
    }

    // Exponer funciones globalmente si es necesario
    window.MatrixHelp = {
        initTooltips: initTooltips,
        showHelpModal: showHelpModal
    };

})();
