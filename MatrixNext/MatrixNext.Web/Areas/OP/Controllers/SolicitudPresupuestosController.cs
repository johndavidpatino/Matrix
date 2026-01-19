using MatrixNext.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controller para solicitudes de presupuestos internos de trabajo (OP Cuantitativo)
/// </summary>
[Area("OP")]
[Authorize]
public class SolicitudPresupuestosController : Controller
{
    private readonly ISolicitudPresupuestoInternoService _service;
    private readonly ILogger<SolicitudPresupuestosController> _logger;

    public SolicitudPresupuestosController(
        ISolicitudPresupuestoInternoService service,
        ILogger<SolicitudPresupuestosController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Vista principal de solicitud de presupuesto interno
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(long? trabajoId)
    {
        // Si no se pasa TrabajoId, intentar obtener de Session
        if (!trabajoId.HasValue)
        {
            var trabajoIdSession = HttpContext.Session.GetInt32("TrabajoId");
            if (!trabajoIdSession.HasValue)
            {
                TempData["Error"] = "No se ha seleccionado un trabajo";
                return RedirectToAction("Index", "Trabajos");
            }
            trabajoId = trabajoIdSession.Value;
        }

        var viewModel = await _service.PrepararSolicitudAsync(trabajoId.Value);

        return View(viewModel);
    }

    /// <summary>
    /// Crea solicitud de presupuesto interno
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Solicitar(long trabajoId, string observacion)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var (success, message) = await _service.CrearSolicitudAsync(trabajoId, observacion, userId);

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

        return RedirectToAction(nameof(Index), new { trabajoId });
    }
}
