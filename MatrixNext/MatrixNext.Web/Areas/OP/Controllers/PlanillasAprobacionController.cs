using MatrixNext.Data.Models.OP;
using MatrixNext.Data.Services.OP;
using MatrixNext.Web.Services.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MatrixNext.Web.Areas.OP.Controllers;

[Area("OP")]
[Authorize]
public class PlanillasAprobacionController : Controller
{
    private readonly IOpPlanillasService _planillasService;
    private readonly IPlanillasAprobacionService _aprobacionService;
    private readonly ILogger<PlanillasAprobacionController> _logger;

    public PlanillasAprobacionController(
        IOpPlanillasService planillasService,
        IPlanillasAprobacionService aprobacionService,
        ILogger<PlanillasAprobacionController> logger)
    {
        _planillasService = planillasService;
        _aprobacionService = aprobacionService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await _planillasService.ObtenerPlanillasAsync();
        return View(model);
    }

    /// <summary>
    /// Vista de planillas aprobadas con filtros
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> AprobadosIndex(
        bool? revisado = null,
        long? pmoId = null,
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        long? trabajoId = null,
        long? coordinadorId = null)
    {
        try
        {
            var planillas = await _aprobacionService.ObtenerPlanillasAprobadosAsync(
                revisado, pmoId, fechaInicio, fechaFin, trabajoId, coordinadorId);

            // Información de ventana de nómina actual
            var ventana = _aprobacionService.ObtenerVentanaNominaActual();
            ViewBag.VentanaInicio = ventana.Inicio;
            ViewBag.VentanaFin = ventana.Fin;

            return View(planillas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando planillas aprobadas");
            TempData["Error"] = "Error al cargar las planillas aprobadas";
            return View(new PlanillaAprobacionDto[] { });
        }
    }

    /// <summary>
    /// Vista de planillas rechazadas con filtros
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> RechazadosIndex(
        bool? revisado = null,
        long? pmoId = null,
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        long? trabajoId = null,
        long? coordinadorId = null)
    {
        try
        {
            var planillas = await _aprobacionService.ObtenerPlanillasRechazadosAsync(
                revisado, pmoId, fechaInicio, fechaFin, trabajoId, coordinadorId);

            // Información de ventana de nómina actual
            var ventana = _aprobacionService.ObtenerVentanaNominaActual();
            ViewBag.VentanaInicio = ventana.Inicio;
            ViewBag.VentanaFin = ventana.Fin;

            return View(planillas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando planillas rechazadas");
            TempData["Error"] = "Error al cargar las planillas rechazadas";
            return View(new PlanillaAprobacionDto[] { });
        }
    }

    /// <summary>
    /// Aprueba una planilla
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Aprobar(AprobacionPlanillaDto dto)
    {
        if (!ModelState.IsValid)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            TempData["Error"] = "Datos inválidos";
            return RedirectToAction(nameof(AprobadosIndex));
        }

        try
        {
            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            var (success, message) = await _aprobacionService.AprobarPlanillaAsync(
                dto.PlanillaId,
                dto.MontoAutorizado,
                dto.Observaciones,
                usuarioId
            );

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success, message });
            }

            if (success)
            {
                TempData["Success"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction(nameof(AprobadosIndex));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error aprobando planilla {PlanillaId}", dto.PlanillaId);

            var errorMessage = "Error al aprobar la planilla. Por favor intente nuevamente.";

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = errorMessage });
            }

            TempData["Error"] = errorMessage;
            return RedirectToAction(nameof(AprobadosIndex));
        }
    }

    /// <summary>
    /// Rechaza una planilla
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Rechazar(RechazoPlanillaDto dto)
    {
        if (!ModelState.IsValid)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            TempData["Error"] = "Datos inválidos";
            return RedirectToAction(nameof(RechazadosIndex));
        }

        try
        {
            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            var (success, message) = await _aprobacionService.RechazarPlanillaAsync(
                dto.PlanillaId,
                dto.Motivo,
                usuarioId
            );

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success, message });
            }

            if (success)
            {
                TempData["Success"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction(nameof(RechazadosIndex));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rechazando planilla {PlanillaId}", dto.PlanillaId);

            var errorMessage = "Error al rechazar la planilla. Por favor intente nuevamente.";

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = errorMessage });
            }

            TempData["Error"] = errorMessage;
            return RedirectToAction(nameof(RechazadosIndex));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActualizarPlanilla(long trabajoId, string accion)
    {
        var userIdClaim = User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        var usuarioId = long.TryParse(userIdClaim, out var parsed) ? parsed : 0L;

        bool success = accion switch
        {
            "aprobar" => await _planillasService.AprobarPlanillaAsync(trabajoId, usuarioId),
            "rechazar" => await _planillasService.RechazarPlanillaAsync(trabajoId, usuarioId),
            _ => false
        };

        TempData["PlanillaMessage"] = success ? "Planilla actualizada correctamente." : "No fue posible actualizar la planilla.";

        return RedirectToAction(nameof(Index));
    }
}
