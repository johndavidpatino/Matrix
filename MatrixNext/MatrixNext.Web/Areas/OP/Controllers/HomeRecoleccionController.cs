using MatrixNext.Data.Models.OP;
using MatrixNext.Data.Services.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controller para Dashboard de Recolección (HomeRecoleccion)
/// Mostrará métricas, trabajos activos, producción y alertas
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.4
/// </summary>
[Area("OP")]
[Authorize]
public class HomeRecoleccionController : Controller
{
    private readonly IHomeRecoleccionDashboardService _dashboardService;
    private readonly ILogger<HomeRecoleccionController> _logger;

    public HomeRecoleccionController(
        IHomeRecoleccionDashboardService dashboardService,
        ILogger<HomeRecoleccionController> logger)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    /// <summary>
    /// Dashboard principal con métricas, trabajos activos, gráficos
    /// Validar permiso 54 (acceso base a módulo OP)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            // TODO: Implementar validación de permiso 54 (acceso base)
            // if (!await _permisosService.TienePermisoAsync(userId, 54))
            //     return Forbid();

            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var idUnidad = long.Parse(User.FindFirstValue("IdUnidad") ?? "0");

            // Obtener dashboard completo
            var dashboard = await _dashboardService.ObtenerDashboardCompletoAsync(
                idUnidad > 0 ? idUnidad : null);

            _logger.LogInformation(
                "Dashboard accedido. Usuario: {UsuarioId}, Unidad: {IdUnidad}, Métricas: {MetricasCount}",
                usuarioId, idUnidad, dashboard.Metricas.Count);

            return View(dashboard);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando dashboard");
            TempData["Error"] = "Error al cargar el dashboard. Intente nuevamente.";
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }

    /// <summary>
    /// Obtiene trabajos activos (para modal o tabla detallada)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ObtenerTrabajosActivos(int? limite = 20)
    {
        try
        {
            var idUnidad = long.Parse(User.FindFirstValue("IdUnidad") ?? "0");
            var trabajos = await _dashboardService.ObtenerTrabajosActivosAsync(
                idUnidad > 0 ? idUnidad : null,
                limite);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_TrabajosActivos", trabajos);
            }

            return View("TrabajosActivos", trabajos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo trabajos activos");
            var errorMessage = "Error al obtener trabajos activos";

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = errorMessage });
            }

            TempData["Error"] = errorMessage;
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Obtiene trabajos en riesgo (para alerta)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ObtenerTrabajosEnRiesgo()
    {
        try
        {
            var idUnidad = long.Parse(User.FindFirstValue("IdUnidad") ?? "0");
            var trabajosEnRiesgo = await _dashboardService.ObtenerTrabajosEnRiesgoAsync(
                idUnidad > 0 ? idUnidad : null);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_TrabajosEnRiesgo", trabajosEnRiesgo);
            }

            return View("TrabajosEnRiesgo", trabajosEnRiesgo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo trabajos en riesgo");
            var errorMessage = "Error al obtener trabajos en riesgo";

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = errorMessage });
            }

            TempData["Error"] = errorMessage;
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Obtiene datos de producción diaria para gráfico
    /// Retorna JSON para gráfico de Chart.js o similar
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ObtenerGraficoProduccion(int diasAtras = 7)
    {
        try
        {
            var produccion = await _dashboardService.ObtenerProduccionDiariaAsync(diasAtras);

            var chartData = new
            {
                labels = produccion.Select(x => x.Fecha.ToString("dd/MM")).ToList(),
                planeadas = produccion.Select(x => x.EncuestasPlaneadas).ToList(),
                ejecutadas = produccion.Select(x => x.EncuestasEjecutadas).ToList(),
                porcentajeAvance = produccion.Select(x => x.ProcentajeAvance).ToList()
            };

            return Json(chartData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo gráfico producción");
            return Json(new { success = false, message = "Error al obtener datos del gráfico" });
        }
    }

    /// <summary>
    /// Obtiene métricas consolidadas (para actualización AJAX)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ObtenerMetricas()
    {
        try
        {
            var idUnidad = long.Parse(User.FindFirstValue("IdUnidad") ?? "0");
            var metricas = await _dashboardService.ObtenerMetricasAsync(
                idUnidad > 0 ? idUnidad : null);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, data = metricas });
            }

            return View("Metricas", metricas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo métricas");
            return Json(new { success = false, message = "Error al obtener métricas" });
        }
    }

    /// <summary>
    /// Endpoint para actualizar dashboard en tiempo real (polling/refresh)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ActualizarDashboard()
    {
        try
        {
            var idUnidad = long.Parse(User.FindFirstValue("IdUnidad") ?? "0");
            var dashboard = await _dashboardService.ObtenerDashboardCompletoAsync(
                idUnidad > 0 ? idUnidad : null);

            return Json(new
            {
                success = true,
                metricas = dashboard.Metricas,
                trabajos = dashboard.TrabajosActivos,
                periodo = dashboard.PeriodoReporte,
                actualizado = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando dashboard");
            return Json(new { success = false, message = "Error al actualizar dashboard" });
        }
    }
}
