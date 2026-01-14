using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Models;
using MatrixNext.Web.Services.Dashboard;

namespace MatrixNext.Web.Controllers;

/// <summary>
/// SPRINT 9: Home Controller with Dashboard Service
/// Controlador para la página de inicio con agregación de datos de múltiples módulos
/// </summary>
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDashboardService _dashboardService;

    public HomeController(
        ILogger<HomeController> logger,
        IDashboardService dashboardService)
    {
        _logger = logger;
        _dashboardService = dashboardService;
    }

    /// <summary>
    /// Index - Página principal del dashboard
    /// Carga todos los widgets necesarios (tareas, proyectos, cotizaciones, etc.)
    /// Performance target: < 2 segundos
    /// </summary>
    public async Task<IActionResult> Index()
    {
        try
        {
            // Obtener información del usuario autenticado desde claims
            var claimUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var nombreCompleto = User.FindFirst("NombreCompleto")?.Value;

            ViewData["UserId"] = claimUserId;
            ViewData["UserName"] = userName;
            ViewData["NombreCompleto"] = nombreCompleto;

            var resolvedUserId = claimUserId ?? userName ?? "Unknown";
            var dashboard = await _dashboardService.GetDashboardAsync(resolvedUserId);
            dashboard ??= new DashboardViewModel();

            return View(dashboard);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando dashboard");
            // Retornar vista con error en lugar de romper la página
            return View("Index", new DashboardViewModel 
            { 
                Error = "Error al cargar el dashboard: " + ex.Message,
                LoadedAt = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    /// Quick actions para navegación contexual
    /// </summary>
    [HttpGet]
    public IActionResult NewQuote()
    {
        _logger.LogInformation("Usuario {UserId} abre formulario de nueva cotización", User.Identity?.Name ?? "system");
        return RedirectToAction("Create", "EasyQuote");
    }

    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    /// <summary>
    /// API endpoint: Refrescar datos del dashboard (AJAX)
    /// Invalida cache y recarga datos
    /// </summary>
    [HttpPost]
    [Produces("application/json")]
    public async Task<IActionResult> RefreshDashboard()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.Identity?.Name
                ?? "Unknown";

            var dashboard = await _dashboardService.GetDashboardAsync(userId);

                return Json(new {
                    success = true,
                    data = dashboard,
                    loadedAt = dashboard?.LoadedAt ?? DateTime.UtcNow
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refrescando dashboard");
            return Json(new { 
                success = false, 
                error = ex.Message 
            });
        }
    }

    /// <summary>
    /// API endpoint: Obtener un widget específico (carga perezosa)
    /// Permite cargar widgets bajo demanda sin recargar toda la página
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    public async Task<IActionResult> Widget(string widgetName)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.Identity?.Name
                ?? "Unknown";

            object widgetData = widgetName?.ToLower() switch
            {
                "tasks" => await _dashboardService.GetPendingTasksAsync(userId),
                "projects" => await _dashboardService.GetActiveProjectsAsync(userId),
                "quotes" => await _dashboardService.GetRecentQuotesAsync(userId),
                "absences" => await _dashboardService.GetUpcomingAbsencesAsync(userId),
                "documents" => await _dashboardService.GetDocumentStatsAsync(userId),
                "metrics" => await _dashboardService.GetProductionMetricsAsync(),
                _ => new { error = "Widget no encontrado" }
            };

            return Json(new { success = true, data = widgetData });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando widget {WidgetName}", widgetName);
            return Json(new { success = false, error = ex.Message });
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
