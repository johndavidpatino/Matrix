/**
 * Módulo de Reportes OP_Cualitativo
 * Maneja: Sesiones, Entrevistas, Moderadores
 * Exportes: Excel, PDF
 * Filtros: Avanzados con date ranges, autocomplete, multi-select
 */

const OpReportesModule = (() => {
    'use strict';

    // ========== CONFIGURACIÓN ==========
    const API_BASE = '/api/OP/reportes';
    let currentFilters = {};

    // ========== INICIALIZACIÓN ==========
    const init = () => {
        attachEventListeners();
        loadSesiones();
    };

    // ========== EVENT LISTENERS ==========
    const attachEventListeners = () => {
        // Filtros
        document.getElementById('btnApplyFilters').addEventListener('click', applyFilters);
        document.getElementById('btnClearFilters').addEventListener('click', clearFilters);

        // Sesiones
        document.getElementById('btnExportSesionesExcel').addEventListener('click', () => exportSesiones('excel'));
        document.getElementById('btnExportSesionesPdf').addEventListener('click', () => exportSesiones('pdf'));

        // Entrevistas
        document.getElementById('btnExportEntrevistasExcel').addEventListener('click', () => exportEntrevistas('excel'));
        document.getElementById('btnExportEntrevistasPdf').addEventListener('click', () => exportEntrevistas('pdf'));

        // Moderadores
        document.getElementById('btnExportModeradoresExcel').addEventListener('click', exportModeadores);

        // Tabs
        document.getElementById('sesiones-tab').addEventListener('click', loadSesiones);
        document.getElementById('entrevistas-tab').addEventListener('click', loadEntrevistas);
        document.getElementById('moderadores-tab').addEventListener('click', loadModeadores);
    };

    // ========== SESIONES ==========
    const loadSesiones = async () => {
        try {
            const response = await fetch(
                `${API_BASE}/sesiones?${buildQueryString(currentFilters)}`
            );
            const result = await response.json();

            if (result.success) {
                renderSesiones(result.data);
            } else {
                showToast('Error al cargar sesiones', 'error');
            }
        } catch (error) {
            console.error('Error loading sessions:', error);
            showToast('Error al cargar sesiones', 'error');
        }
    };

    const renderSesiones = (sessions) => {
        const tbody = document.getElementById('sesionesBody');
        tbody.innerHTML = '';

        if (sessions.length === 0) {
            tbody.innerHTML = '<tr><td colspan="8" class="text-center text-muted">No hay sesiones</td></tr>';
            return;
        }

        sessions.forEach(session => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${session.sesionId}</td>
                <td><strong>${session.trabajoCodigo}</strong></td>
                <td>${formatDate(session.fechaInicio)}</td>
                <td>${session.duracion} min</td>
                <td>${session.ubicacion}</td>
                <td><span class="badge bg-${getEstadoBadgeClass(session.estado)}">${session.estado}</span></td>
                <td>${session.numeroParticipantes}</td>
                <td>${session.moderador}</td>
            `;
            tbody.appendChild(row);
        });
    };

    const exportSesiones = async (format) => {
        try {
            const endpoint = format === 'excel' 
                ? `${API_BASE}/export-sesiones-excel`
                : `${API_BASE}/export-sesiones-pdf`;
            
            const url = `${endpoint}?${buildQueryString(currentFilters)}`;
            window.location.href = url;
        } catch (error) {
            console.error('Error exporting sessions:', error);
            showToast('Error al exportar sesiones', 'error');
        }
    };

    // ========== ENTREVISTAS ==========
    const loadEntrevistas = async () => {
        try {
            const response = await fetch(
                `${API_BASE}/entrevistas?${buildQueryString(currentFilters)}`
            );
            const result = await response.json();

            if (result.success) {
                renderEntrevistas(result.data);
            } else {
                showToast('Error al cargar entrevistas', 'error');
            }
        } catch (error) {
            console.error('Error loading interviews:', error);
            showToast('Error al cargar entrevistas', 'error');
        }
    };

    const renderEntrevistas = (interviews) => {
        const tbody = document.getElementById('entrevistasBody');
        tbody.innerHTML = '';

        if (interviews.length === 0) {
            tbody.innerHTML = '<tr><td colspan="8" class="text-center text-muted">No hay entrevistas</td></tr>';
            return;
        }

        interviews.forEach(interview => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${interview.entrevistaId}</td>
                <td><strong>${interview.trabajoCodigo}</strong></td>
                <td>${formatDate(interview.fechaEjecucion)}</td>
                <td>${interview.entrevistador}</td>
                <td>${interview.encuestado}</td>
                <td>${interview.duracion} min</td>
                <td>
                    <div class="progress" style="height: 20px;">
                        <div class="progress-bar" role="progressbar" style="width: ${interview.completitud}%">
                            ${interview.completitud.toFixed(0)}%
                        </div>
                    </div>
                </td>
                <td><span class="badge bg-${getEstadoBadgeClass(interview.estado)}">${interview.estado}</span></td>
            `;
            tbody.appendChild(row);
        });
    };

    const exportEntrevistas = async (format) => {
        try {
            const endpoint = format === 'excel' 
                ? `${API_BASE}/export-entrevistas-excel`
                : `${API_BASE}/export-entrevistas-pdf`;
            
            const url = `${endpoint}?${buildQueryString(currentFilters)}`;
            window.location.href = url;
        } catch (error) {
            console.error('Error exporting interviews:', error);
            showToast('Error al exportar entrevistas', 'error');
        }
    };

    // ========== MODERADORES ==========
    const loadModeadores = async () => {
        try {
            const response = await fetch(
                `${API_BASE}/moderadores?${buildQueryString(currentFilters)}`
            );
            const result = await response.json();

            if (result.success) {
                renderModeadores(result.data);
            } else {
                showToast('Error al cargar moderadores', 'error');
            }
        } catch (error) {
            console.error('Error loading moderators:', error);
            showToast('Error al cargar moderadores', 'error');
        }
    };

    const renderModeadores = (moderators) => {
        const tbody = document.getElementById('moderadoresBody');
        tbody.innerHTML = '';

        if (moderators.length === 0) {
            tbody.innerHTML = '<tr><td colspan="7" class="text-center text-muted">No hay moderadores</td></tr>';
            return;
        }

        moderators.forEach(moderator => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${moderator.moderadorId}</td>
                <td><strong>${moderator.nombre}</strong></td>
                <td>${moderator.totalSesiones}</td>
                <td>${moderator.sesionesCompletadas}</td>
                <td>${moderator.horasTotal}h</td>
                <td>${moderator.promedioParticipantes.toFixed(2)}</td>
                <td>${formatDate(moderator.ultimaSesion)}</td>
            `;
            tbody.appendChild(row);
        });
    };

    const exportModeadores = async () => {
        try {
            const url = `${API_BASE}/export-moderadores-excel?${buildQueryString(currentFilters)}`;
            window.location.href = url;
        } catch (error) {
            console.error('Error exporting moderators:', error);
            showToast('Error al exportar moderadores', 'error');
        }
    };

    // ========== FILTROS ==========
    const applyFilters = () => {
        currentFilters = {
            trabajoId: document.getElementById('trabajoId').value,
            fechaDesde: document.getElementById('fechaDesde').value,
            fechaHasta: document.getElementById('fechaHasta').value,
            estado: document.getElementById('estado').value
        };

        // Limpiar valores vacíos
        Object.keys(currentFilters).forEach(key => {
            if (!currentFilters[key]) delete currentFilters[key];
        });

        loadSesiones();
        showToast('Filtros aplicados', 'success');
    };

    const clearFilters = () => {
        document.getElementById('filterForm').reset();
        currentFilters = {};
        loadSesiones();
        showToast('Filtros limpios', 'success');
    };

    // ========== UTILIDADES ==========
    const buildQueryString = (obj) => {
        return Object.entries(obj)
            .map(([key, value]) => `${encodeURIComponent(key)}=${encodeURIComponent(value)}`)
            .join('&');
    };

    const formatDate = (dateString) => {
        if (!dateString) return '-';
        const date = new Date(dateString);
        return date.toLocaleDateString('es-CO');
    };

    const getEstadoBadgeClass = (estado) => {
        const estadoMap = {
            'Completado': 'success',
            'Pendiente': 'warning',
            'Cancelado': 'danger',
            'En Progreso': 'info'
        };
        return estadoMap[estado] || 'secondary';
    };

    const showToast = (message, type = 'info') => {
        const toastHtml = `
            <div class="toast align-items-center text-white bg-${type === 'success' ? 'success' : type === 'error' ? 'danger' : 'info'} border-0" 
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
    };

    // ========== API PÚBLICA ==========
    return {
        init
    };
})();

// Inicializar cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', OpReportesModule.init);
