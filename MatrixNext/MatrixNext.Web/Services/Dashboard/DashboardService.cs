using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models;
using MatrixNext.Web.Models.EQ;
using MatrixNext.Web.Models.PY;
using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.DTOs;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Services.Dashboard
{
    /// <summary>
    /// SPRINT 9: Home Dashboard Service
    /// Agregador de datos de múltiples módulos para visualización en dashboard
    /// 
    /// Responsabilidades:
    /// - Cargar datos de PY (Proyectos), OP (Operaciones), CU (Cuentas), TH (Talentos), FI (Financiero), GD (Documentos)
    /// - Cachear datos agresivamente para performance < 2 segundos
    /// - Validar permisos por rol del usuario
    /// - Calcular KPIs y métricas
    /// 
    /// Widgets incluidos:
    /// 1. Mis tareas pendientes (CORE)
    /// 2. Proyectos activos (PY)
    /// 3. Cotizaciones EasyQuote (EQ)
    /// 4. Ausencias registradas (TH)
    /// 5. Documentos pendientes de aprobación (GD)
    /// 6. Gráficos de producción/ventas (métricas)
    /// 7. Links contextuales (navegación rápida)
    /// 
    /// Status: SPRINT 9 PASO 1
    /// </summary>
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardAsync(string userId);
        Task<List<TaskSummary>> GetPendingTasksAsync(string userId);
        Task<List<ProjectSummary>> GetActiveProjectsAsync(string userId);
        Task<List<QuoteSummary>> GetRecentQuotesAsync(string userId);
        Task<List<AbsenceSummary>> GetUpcomingAbsencesAsync(string userId);
        Task<DocumentStatistics> GetDocumentStatsAsync(string userId);
        Task<ProductionMetrics> GetProductionMetricsAsync();
    }

    public class DashboardService : IDashboardService
    {
        private readonly MatrixDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly ILogger<DashboardService> _logger;
        private const string CACHE_KEY_PREFIX = "dashboard_";
        private const int CACHE_DURATION_MINUTES = 15; // Cache de 15 minutos

        public DashboardService(
            MatrixDbContext context,
            IMemoryCache cache,
            ILogger<DashboardService> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Carga el dashboard completo para un usuario
        /// Incluye: tareas, proyectos, cotizaciones, ausencias, documentos, métricas
        /// </summary>
        public async Task<DashboardViewModel> GetDashboardAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Cargando dashboard para usuario {UserId}", userId);

                // Intentar obtener del cache primero
                var cacheKey = $"{CACHE_KEY_PREFIX}full_{userId}";
                if (_cache.TryGetValue(cacheKey, out DashboardViewModel? cachedDashboard) && cachedDashboard != null)
                {
                    _logger.LogDebug("Dashboard obtenido del cache para usuario {UserId}", userId);
                    return cachedDashboard;
                }

                // Cargar datos en paralelo para mejor performance
                var tasksTask = GetPendingTasksAsync(userId);
                var projectsTask = GetActiveProjectsAsync(userId);
                var quotesTask = GetRecentQuotesAsync(userId);
                var absencesTask = GetUpcomingAbsencesAsync(userId);
                var docsTask = GetDocumentStatsAsync(userId);
                var metricsTask = GetProductionMetricsAsync();

                await Task.WhenAll(tasksTask, projectsTask, quotesTask, absencesTask, docsTask, metricsTask);

                // Obtener resultados de las tareas ya completadas (no bloquea, son awaited)
                var pendingTasks = await tasksTask;
                var activeProjects = await projectsTask;
                var recentQuotes = await quotesTask;
                var upcomingAbsences = await absencesTask;
                var documentStats = await docsTask;
                var productionMetrics = await metricsTask;

                var dashboard = new DashboardViewModel
                {
                    LoadedAt = DateTime.UtcNow,
                    PendingTasks = pendingTasks,
                    ActiveProjects = activeProjects,
                    RecentQuotes = recentQuotes,
                    UpcomingAbsences = upcomingAbsences,
                    DocumentStats = documentStats,
                    ProductionMetrics = productionMetrics,
                    TaskCount = pendingTasks.Count,
                    ProjectCount = activeProjects.Count,
                    QuoteCount = recentQuotes.Count
                };

                // Cachear resultado
                _cache.Set(cacheKey, dashboard, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

                _logger.LogInformation("Dashboard cargado exitosamente para usuario {UserId}", userId);
                return dashboard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando dashboard para usuario {UserId}", userId);
                // Retornar dashboard vacío para no romper la UI
                return new DashboardViewModel
                {
                    LoadedAt = DateTime.UtcNow,
                    PendingTasks = new List<TaskSummary>(),
                    ActiveProjects = new List<ProjectSummary>(),
                    RecentQuotes = new List<QuoteSummary>(),
                    UpcomingAbsences = new List<AbsenceSummary>(),
                    DocumentStats = new DocumentStatistics(),
                    ProductionMetrics = new ProductionMetrics(),
                    Error = "Error al cargar el dashboard. Por favor intente nuevamente."
                };
            }
        }

        /// <summary>
        /// Widget 1: Tareas pendientes del usuario (de CORE)
        /// Obtiene WorkFlows asignados al usuario con estado diferente a Completada
        /// </summary>
        public async Task<List<TaskSummary>> GetPendingTasksAsync(string userId)
        {
            try
            {
                var cacheKey = $"{CACHE_KEY_PREFIX}tasks_{userId}";
                if (_cache.TryGetValue(cacheKey, out List<TaskSummary>? cached) && cached != null)
                    return cached;

                if (!long.TryParse(userId, out long userIdParsed))
                {
                    _logger.LogWarning("UserId inválido para GetPendingTasksAsync: {UserId}", userId);
                    return new List<TaskSummary>();
                }

                // Obtener WorkFlows asignados al usuario
                var tasks = await _context.WorkFlows
                    .Where(w => w.UsuariosAsignados.Any(ua => ua.IdUsuario == userIdParsed) &&
                               w.Estado != "Completada" && w.Estado != "Anulada")
                    .OrderByDescending(w => w.FechaVencimiento)
                    .Take(10)
                    .Select(w => new TaskSummary
                    {
                        Id = w.Id,
                        Titulo = w.Observaciones ?? $"Tarea {w.Id}",
                        Descripcion = string.Empty,
                        FechaVencimiento = w.FechaVencimiento ?? DateTime.Now.AddDays(30),
                        Prioridad = GetPrioridadTarea(w.Prioridad),
                        Estado = w.Estado ?? "Desconocido"
                    })
                    .ToListAsync();

                _cache.Set(cacheKey, tasks, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
                _logger.LogDebug("Tareas pendientes obtenidas: {Count} para usuario {UserId}", tasks.Count, userId);
                return tasks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo tareas pendientes para usuario {UserId}", userId);
                return new List<TaskSummary>();
            }
        }

        /// <summary>
        /// Widget 2: Proyectos activos del usuario (de PY)
        /// Obtiene Proyectos donde el usuario es Gerente o responsable
        /// </summary>
        public async Task<List<ProjectSummary>> GetActiveProjectsAsync(string userId)
        {
            try
            {
                var cacheKey = $"{CACHE_KEY_PREFIX}projects_{userId}";
                if (_cache.TryGetValue(cacheKey, out List<ProjectSummary>? cached) && cached != null)
                    return cached;

                if (!long.TryParse(userId, out long userIdParsed))
                {
                    _logger.LogWarning("UserId inválido para GetActiveProjectsAsync: {UserId}", userId);
                    return new List<ProjectSummary>();
                }

                // Obtener Proyectos activos donde es gerente o responsable
                var projects = await _context.Proyectos
                    .Where(p => p.IdGerenteProyectos == userIdParsed &&
                               p.Activo) // Solo proyectos activos
                    .OrderByDescending(p => p.FechaCreacion)
                    .Take(10)
                    .Select(p => new ProjectSummary
                    {
                        Id = p.Id,
                        Nombre = p.Nombre ?? string.Empty,
                        Cliente = string.Empty, // TODO: Agregar relación con Cliente en Proyecto
                        FechaInicio = p.FechaCreacion,
                        FechaFinal = p.FechaModificacion,
                        Estado = GetEstadoProyecto(p.Estado),
                        Progreso = CalcularProgresoProyecto(p)
                    })
                    .ToListAsync();

                _cache.Set(cacheKey, projects, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
                _logger.LogDebug("Proyectos activos obtenidos: {Count} para usuario {UserId}", projects.Count, userId);
                return projects;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo proyectos activos para usuario {UserId}", userId);
                return new List<ProjectSummary>();
            }
        }

        /// <summary>
        /// Widget 3: Cotizaciones EasyQuote recientes (de EQ)
        /// </summary>
        public async Task<List<QuoteSummary>> GetRecentQuotesAsync(string userId)
        {
            try
            {
                var cacheKey = $"{CACHE_KEY_PREFIX}quotes_{userId}";
                if (_cache.TryGetValue(cacheKey, out List<QuoteSummary>? cached) && cached != null)
                    return cached;

                // TODO: Conectar con tabla de usuarios creadores cuando esté disponible
                // Por ahora, retornar solo cotizaciones recientes sin filtro de usuario
                var quotes = await _context.EqQuoteHeaders
                    .OrderByDescending(q => q.FechaCreacion)
                    .Take(5)
                    .Select(q => new QuoteSummary
                    {
                        Id = q.Id,
                        PropuestaNombre = q.PropuestaNombre ?? string.Empty,
                        Cliente = q.Cliente ?? string.Empty,
                        FechaCreacion = q.FechaCreacion,
                        Estado = q.SL ?? string.Empty,
                        MontoEstimado = q.ValorGMU ?? q.ValorProveedorExterno ?? q.ValorProveedorInternacional ?? 0
                    })
                    .ToListAsync();

                _cache.Set(cacheKey, quotes, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
                return quotes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo cotizaciones recientes");
                return new List<QuoteSummary>();
            }
        }

        /// <summary>
        /// Widget 4: Ausencias próximas del usuario (de TH)
        /// Obtiene solicitudes de ausencia aprobadas en los próximos 30 días
        /// </summary>
        public async Task<List<AbsenceSummary>> GetUpcomingAbsencesAsync(string userId)
        {
            try
            {
                var cacheKey = $"{CACHE_KEY_PREFIX}absences_{userId}";
                if (_cache.TryGetValue(cacheKey, out List<AbsenceSummary>? cached) && cached != null)
                    return cached;

                if (!long.TryParse(userId, out long userIdParsed))
                {
                    _logger.LogWarning("UserId inválido para GetUpcomingAbsencesAsync: {UserId}", userId);
                    return new List<AbsenceSummary>();
                }

                // Obtener ausencias aprobadas del usuario en los próximos 30 días
                var today = DateTime.Now.Date;
                var future30Days = today.AddDays(30);

                // Usar MatrixNext.Data context si está disponible, sino usar Web context
                var absences = new List<AbsenceSummary>();
                
                // Intenta usar la tabla de ausencias si existe en el contexto principal
                // Por ahora, retornar lista vacía - será completado en siguiente fase
                // cuando se integre con MatrixNext.Data.Context.MatrixDbContext
                
                _cache.Set(cacheKey, absences, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
                _logger.LogDebug("Ausencias próximas obtenidas: {Count} para usuario {UserId}", absences.Count, userId);
                return absences;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo ausencias próximas para usuario {UserId}", userId);
                return new List<AbsenceSummary>();
            }
        }

        /// <summary>
        /// Widget 5: Estadísticas de documentos (de GD)
        /// Obtiene conteos de documentos por estado
        /// </summary>
        public async Task<DocumentStatistics> GetDocumentStatsAsync(string userId)
        {
            try
            {
                var cacheKey = $"{CACHE_KEY_PREFIX}docs_{userId}";
                if (_cache.TryGetValue(cacheKey, out DocumentStatistics? cached) && cached != null)
                    return cached;

                if (!long.TryParse(userId, out long userIdParsed))
                {
                    _logger.LogWarning("UserId inválido para GetDocumentStatsAsync: {UserId}", userId);
                    return new DocumentStatistics();
                }

                // TODO: Conectar con tabla de documentos cuando GD esté migrado
                // Por ahora, retornar estadísticas base
                var stats = new DocumentStatistics
                {
                    Total = 0,
                    PendingApproval = 0,
                    ApprovedLastWeek = 0,
                    RejectedLastWeek = 0
                };

                _cache.Set(cacheKey, stats, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
                _logger.LogDebug("Estadísticas de documentos obtenidas para usuario {UserId}", userId);
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo estadísticas de documentos para usuario {UserId}", userId);
                return new DocumentStatistics();
            }
        }

        /// <summary>
        /// Widget 6: Métricas de producción/ventas (agregadas, cacheadas agresivamente)
        /// </summary>
        public async Task<ProductionMetrics> GetProductionMetricsAsync()
        {
            try
            {
                var cacheKey = $"{CACHE_KEY_PREFIX}metrics_global";
                if (_cache.TryGetValue(cacheKey, out ProductionMetrics? cached) && cached != null)
                    return cached;

                var thisMonth = DateTime.UtcNow;
                var quotesThisMonth = await _context.EqQuoteHeaders
                    .Where(q => q.FechaCreacion.Year == thisMonth.Year &&
                                q.FechaCreacion.Month == thisMonth.Month)
                    .ToListAsync();

                var metrics = new ProductionMetrics
                {
                    TotalQuotesThisMonth = quotesThisMonth.Count,
                    
                    TotalRevenueThisMonth = (decimal)quotesThisMonth
                        .Sum(q => (double)(q.ValorGMU ?? q.ValorProveedorExterno ?? q.ValorProveedorInternacional ?? 0)),
                    
                    AverageQuoteValue = quotesThisMonth.Count > 0 
                        ? (decimal)quotesThisMonth
                            .Where(q => q.ValorGMU != null || q.ValorProveedorExterno != null || q.ValorProveedorInternacional != null)
                            .Average(q => (double)(q.ValorGMU ?? q.ValorProveedorExterno ?? q.ValorProveedorInternacional ?? 0))
                        : 0,
                    
                    LastUpdated = DateTime.UtcNow
                };

                // Cache más largo para métricas globales (30 minutos)
                _cache.Set(cacheKey, metrics, TimeSpan.FromMinutes(30));
                return metrics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo métricas de producción");
                return new ProductionMetrics();
            }
        }

        /// <summary>
        /// Invalida el cache del dashboard para un usuario
        /// Llamar después de cambios en datos relacionados
        /// </summary>
        public void InvalidateUserDashboard(string userId)
        {
            _cache.Remove($"{CACHE_KEY_PREFIX}full_{userId}");
            _cache.Remove($"{CACHE_KEY_PREFIX}tasks_{userId}");
            _cache.Remove($"{CACHE_KEY_PREFIX}projects_{userId}");
            _cache.Remove($"{CACHE_KEY_PREFIX}quotes_{userId}");
            _cache.Remove($"{CACHE_KEY_PREFIX}absences_{userId}");
            _cache.Remove($"{CACHE_KEY_PREFIX}docs_{userId}");
            _logger.LogInformation("Cache invalidado para usuario {UserId}", userId);
        }

        /// <summary>
        /// Helper: Convierte prioridad numérica a texto
        /// </summary>
        private string GetPrioridadTarea(int prioridad)
        {
            return prioridad switch
            {
                1 => "Normal",
                2 => "Alta",
                3 => "Baja",
                _ => "Normal"
            };
        }

        /// <summary>
        /// Helper: Convierte estado numérico de Proyecto a texto
        /// </summary>
        private string GetEstadoProyecto(int estado)
        {
            return estado switch
            {
                1 => "Nuevo",
                2 => "En progreso",
                3 => "Cerrado",
                _ => "Desconocido"
            };
        }

        /// <summary>
        /// Helper: Calcula el progreso de un proyecto basado en sus trabajos
        /// </summary>
        private int CalcularProgresoProyecto(Proyecto proyecto)
        {
            if (proyecto.Trabajos == null || proyecto.Trabajos.Count == 0)
                return 0;

            // Porcentaje basado en cantidad de trabajos cerrados (Estado == 3)
            var trabajosCerrados = proyecto.Trabajos.Count(t => t.Estado == 3);
            return (trabajosCerrados * 100) / proyecto.Trabajos.Count;
        }
    }

    /// <summary>
    /// ViewModels y DTOs para el Dashboard
    /// </summary>
    public class DashboardViewModel
    {
        public DateTime LoadedAt { get; set; }
        public List<TaskSummary> PendingTasks { get; set; } = new();
        public List<ProjectSummary> ActiveProjects { get; set; } = new();
        public List<QuoteSummary> RecentQuotes { get; set; } = new();
        public List<AbsenceSummary> UpcomingAbsences { get; set; } = new();
        public DocumentStatistics DocumentStats { get; set; } = new();
        public ProductionMetrics ProductionMetrics { get; set; } = new();

        // Counters para UI
        public int TaskCount { get; set; }
        public int ProjectCount { get; set; }
        public int QuoteCount { get; set; }

        // Error handling
        public string? Error { get; set; }
        public bool HasError => !string.IsNullOrEmpty(Error);
    }

    public class TaskSummary
    {
        public long Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaVencimiento { get; set; }
        public string Prioridad { get; set; } = string.Empty; // Alta, Media, Baja
        public string Estado { get; set; } = string.Empty; // Pendiente, En progreso, Completada
    }

    public class ProjectSummary
    {
        public long Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int Progreso { get; set; } // 0-100
    }

    public class QuoteSummary
    {
        public long Id { get; set; }
        public string PropuestaNombre { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public decimal MontoEstimado { get; set; }
    }

    public class AbsenceSummary
    {
        public long Id { get; set; }
        public string Tipo { get; set; } = string.Empty; // Vacaciones, Incapacidad, Permiso
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public string Estado { get; set; } = string.Empty; // Aprobada, Pendiente
    }

    public class DocumentStatistics
    {
        public int Total { get; set; }
        public int PendingApproval { get; set; }
        public int ApprovedLastWeek { get; set; }
        public int RejectedLastWeek { get; set; }
    }

    public class ProductionMetrics
    {
        public int TotalQuotesThisMonth { get; set; }
        public decimal TotalRevenueThisMonth { get; set; }
        public decimal AverageQuoteValue { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
