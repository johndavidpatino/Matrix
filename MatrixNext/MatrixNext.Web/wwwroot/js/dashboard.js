/**
 * SPRINT 9: Dashboard JavaScript
 * Funcionalidad AJAX para carga dinámica de widgets y refresh
 * 
 * Características:
 * - Refresh automático cada 5 minutos
 * - Carga de widgets individuales (lazy loading)
 * - Skeleton loading mientras se cargan los datos
 * - Manejo de errores con toasts
 * - Fallback a localStorage para datos sin conexión
 * 
 * Dependencias: jQuery, Bootstrap Toast (opcional pero recomendado)
 */

(function() {
    'use strict';

    // Configuración global
    const DASHBOARD_CONFIG = {
        AUTO_REFRESH_INTERVAL: 5 * 60 * 1000, // 5 minutos
        WIDGET_TIMEOUT: 10000, // 10 segundos timeout por widget
        CACHE_EXPIRY: 15 * 60 * 1000, // 15 minutos en localStorage
        WIDGETS: ['tasks', 'projects', 'quotes', 'absences', 'documents', 'metrics']
    };

    // Estado local del dashboard
    let dashboardState = {
        isRefreshing: false,
        lastRefreshTime: null,
        widgetCache: new Map()
    };

    /**
     * Inicializa el dashboard
     * Se llama cuando DOM está listo
     */
    window.initDashboard = function() {
        console.log('[Dashboard] Inicializando...');

        // Cargar widgets visibles inicialmente
        loadVisibleWidgets();

        // Setup observador para lazy loading
        if ('IntersectionObserver' in window) {
            setupLazyLoadingObserver();
        }

        // Auto-refresh cada 5 minutos
        setInterval(function() {
            console.log('[Dashboard] Auto-refresh...');
            refreshDashboard();
        }, DASHBOARD_CONFIG.AUTO_REFRESH_INTERVAL);

        // Listener para botón de refresh manual
        const refreshBtn = document.getElementById('dashboardRefreshBtn');
        if (refreshBtn) {
            refreshBtn.addEventListener('click', refreshDashboard);
        }

        console.log('[Dashboard] Inicialización completada');
    };

    /**
     * Carga los widgets que son visibles en viewport
     */
    function loadVisibleWidgets() {
        DASHBOARD_CONFIG.WIDGETS.forEach(widgetName => {
            const widgetEl = document.querySelector(`[data-widget="${widgetName}"]`);
            if (widgetEl) {
                loadWidget(widgetName, widgetEl);
            }
        });
    }

    /**
     * Setup Intersection Observer para lazy loading
     */
    function setupLazyLoadingObserver() {
        const observer = new IntersectionObserver(function(entries) {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const widgetName = entry.target.dataset.widget;
                    if (widgetName && !entry.target.classList.contains('loaded')) {
                        loadWidget(widgetName, entry.target);
                        observer.unobserve(entry.target);
                    }
                }
            });
        }, {
            rootMargin: '50px' // Cargar 50px antes de que sea visible
        });

        // Observar todos los widgets
        document.querySelectorAll('[data-widget]').forEach(widget => {
            observer.observe(widget);
        });
    }

    /**
     * Carga un widget individual via AJAX
     * @param {string} widgetName - Nombre del widget (tasks, projects, etc)
     * @param {HTMLElement} widgetEl - Elemento contenedor del widget
     */
    function loadWidget(widgetName, widgetEl) {
        if (!widgetEl) return;

        // Verificar si ya está cargado
        if (widgetEl.classList.contains('loaded')) return;

        // Mostrar skeleton loading
        showSkeletonLoading(widgetEl);

        // Hacer request AJAX
        const url = `/Home/Widget?widgetName=${widgetName}`;

        fetch(url, {
            method: 'GET',
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'Content-Type': 'application/json'
            },
            signal: AbortSignal.timeout(DASHBOARD_CONFIG.WIDGET_TIMEOUT)
        })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP ${response.status}: ${response.statusText}`);
            }
            return response.text();
        })
        .then(html => {
            // Reemplazar contenido del widget
            widgetEl.innerHTML = html;
            widgetEl.classList.add('loaded');
            hideSkeletonLoading(widgetEl);

            // Guardar en cache local
            cacheWidget(widgetName, html);

            // Log
            console.log(`[Dashboard] Widget "${widgetName}" cargado exitosamente`);
        })
        .catch(error => {
            console.error(`[Dashboard] Error cargando widget "${widgetName}":`, error);

            // Intentar obtener del cache
            const cachedHtml = getCachedWidget(widgetName);
            if (cachedHtml) {
                widgetEl.innerHTML = cachedHtml;
                widgetEl.classList.add('loaded', 'cached-content');
                hideSkeletonLoading(widgetEl);

                // Mostrar notificación de datos en caché
                showWarningToast(`Widget "${widgetName}" mostrando datos en caché (posiblemente desactualizado)`);
            } else {
                // Mostrar error
                showErrorWidget(widgetEl, `Error cargando ${widgetName}`);
                hideSkeletonLoading(widgetEl);
                showErrorToast(`No se pudo cargar el widget "${widgetName}"`);
            }
        });
    }

    /**
     * Refresh completo del dashboard
     */
    function refreshDashboard() {
        if (dashboardState.isRefreshing) {
            console.warn('[Dashboard] Refresh ya en progreso, descartando...');
            return;
        }

        dashboardState.isRefreshing = true;
        const refreshBtn = document.getElementById('dashboardRefreshBtn');
        if (refreshBtn) {
            refreshBtn.disabled = true;
            refreshBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Actualizando...';
        }

        // Hacer request al endpoint RefreshDashboard
        fetch('/Home/RefreshDashboard', {
            method: 'POST',
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'Content-Type': 'application/json'
            },
            signal: AbortSignal.timeout(DASHBOARD_CONFIG.WIDGET_TIMEOUT * 2)
        })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            console.log('[Dashboard] Refresh completado:', data);

            // Recargar todos los widgets
            dashboardState.widgetCache.clear(); // Limpiar cache
            loadVisibleWidgets();

            // Actualizar timestamp
            dashboardState.lastRefreshTime = new Date();
            updateRefreshTimestamp();

            // Mostrar éxito
            showSuccessToast('Dashboard actualizado');

            // Re-habilitar botón
            if (refreshBtn) {
                refreshBtn.disabled = false;
                refreshBtn.innerHTML = '<i class="fas fa-sync-alt"></i> Actualizar';
            }
        })
        .catch(error => {
            console.error('[Dashboard] Error en refresh:', error);
            showErrorToast('Error actualizando el dashboard');

            // Re-habilitar botón
            if (refreshBtn) {
                refreshBtn.disabled = false;
                refreshBtn.innerHTML = '<i class="fas fa-sync-alt"></i> Actualizar';
            }
        })
        .finally(() => {
            dashboardState.isRefreshing = false;
        });
    }

    /**
     * Muestra skeleton loading en un widget
     */
    function showSkeletonLoading(widgetEl) {
        if (!widgetEl) return;

        const skeleton = `
            <div class="skeleton skeleton-lines">
                <div class="skeleton-line" style="width: 70%; height: 1rem;"></div>
                <div class="skeleton-line" style="width: 100%; height: 0.8rem;"></div>
                <div class="skeleton-line" style="width: 90%; height: 0.8rem;"></div>
            </div>
        `;

        widgetEl.innerHTML = skeleton;
        widgetEl.classList.add('loading');
    }

    /**
     * Oculta skeleton loading
     */
    function hideSkeletonLoading(widgetEl) {
        if (!widgetEl) return;
        widgetEl.classList.remove('loading');
    }

    /**
     * Muestra widget de error
     */
    function showErrorWidget(widgetEl, message) {
        if (!widgetEl) return;

        const errorHtml = `
            <div class="alert alert-warning alert-sm mb-0">
                <i class="fas fa-exclamation-triangle"></i>
                ${message}
            </div>
        `;

        widgetEl.innerHTML = errorHtml;
        widgetEl.classList.add('loaded', 'error');
    }

    /**
     * Actualiza timestamp del último refresh
     */
    function updateRefreshTimestamp() {
        const timestampEl = document.getElementById('dashboardLastRefresh');
        if (timestampEl && dashboardState.lastRefreshTime) {
            const formatted = dashboardState.lastRefreshTime.toLocaleTimeString('es-CO');
            timestampEl.textContent = `Última actualización: ${formatted}`;
        }
    }

    /**
     * Guardar widget en cache local
     */
    function cacheWidget(widgetName, html) {
        const cacheData = {
            html: html,
            timestamp: Date.now()
        };
        dashboardState.widgetCache.set(widgetName, cacheData);

        // También guardar en localStorage
        try {
            localStorage.setItem(`dashboard_widget_${widgetName}`, JSON.stringify(cacheData));
        } catch (e) {
            console.warn('[Dashboard] No se pudo acceder a localStorage:', e);
        }
    }

    /**
     * Obtener widget del cache si está disponible y no expiró
     */
    function getCachedWidget(widgetName) {
        // Primero verificar cache en memoria
        const cached = dashboardState.widgetCache.get(widgetName);
        if (cached && Date.now() - cached.timestamp < DASHBOARD_CONFIG.CACHE_EXPIRY) {
            return cached.html;
        }

        // Luego verificar localStorage
        try {
            const stored = localStorage.getItem(`dashboard_widget_${widgetName}`);
            if (stored) {
                const data = JSON.parse(stored);
                if (Date.now() - data.timestamp < DASHBOARD_CONFIG.CACHE_EXPIRY) {
                    // Restaurar en memoria
                    dashboardState.widgetCache.set(widgetName, data);
                    return data.html;
                }
            }
        } catch (e) {
            console.warn('[Dashboard] Error accediendo a localStorage:', e);
        }

        return null;
    }

    /**
     * Mostrar toast de éxito
     */
    function showSuccessToast(message) {
        showToast(message, 'success');
    }

    /**
     * Mostrar toast de error
     */
    function showErrorToast(message) {
        showToast(message, 'danger');
    }

    /**
     * Mostrar toast de warning
     */
    function showWarningToast(message) {
        showToast(message, 'warning');
    }

    /**
     * Mostrar toast genérico
     * Usando Bootstrap Toast si está disponible
     */
    function showToast(message, type = 'info') {
        // Intentar usar Bootstrap Toast
        if (typeof bootstrap !== 'undefined' && bootstrap.Toast) {
            createBootstrapToast(message, type);
        } else {
            // Fallback a simple alert-div
            createSimpleToast(message, type);
        }
    }

    /**
     * Crear Bootstrap Toast
     */
    function createBootstrapToast(message, type) {
        const toastContainer = document.querySelector('[role="status"]');
        if (!toastContainer) {
            console.warn('[Dashboard] No se encontró toast container');
            return;
        }

        const toastEl = document.createElement('div');
        toastEl.className = `toast align-items-center text-white bg-${type}`;
        toastEl.setAttribute('role', 'alert');
        toastEl.setAttribute('aria-live', 'assertive');
        toastEl.setAttribute('aria-atomic', 'true');

        toastEl.innerHTML = `
            <div class="d-flex">
                <div class="toast-body">
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast" aria-label="Cerrar"></button>
            </div>
        `;

        toastContainer.appendChild(toastEl);

        const toast = new bootstrap.Toast(toastEl);
        toast.show();

        // Remover elemento después de que desaparezca
        toastEl.addEventListener('hidden.bs.toast', function() {
            toastEl.remove();
        });
    }

    /**
     * Crear toast simple sin Bootstrap
     */
    function createSimpleToast(message, type) {
        const alertMap = {
            'success': 'success',
            'danger': 'danger',
            'warning': 'warning',
            'info': 'info'
        };

        const alertType = alertMap[type] || 'info';
        const alertEl = document.createElement('div');
        alertEl.className = `alert alert-${alertType} alert-dismissible fade show position-fixed`;
        alertEl.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
        alertEl.setAttribute('role', 'alert');

        alertEl.innerHTML = `
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Cerrar"></button>
        `;

        document.body.appendChild(alertEl);

        // Auto-remove después de 5 segundos
        setTimeout(() => {
            alertEl.remove();
        }, 5000);
    }

    /**
     * Inicializar cuando DOM está listo
     */
    document.addEventListener('DOMContentLoaded', function() {
        initDashboard();
    });

    // Exportar funciones globales para llamadas externas si es necesario
    window.dashboardApi = {
        refresh: refreshDashboard,
        loadWidget: loadWidget,
        init: initDashboard
    };

})();
