using MatrixNext.Web.Services.OP;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.OP.Controllers;

[Area("OP")]
[Authorize]
public class PresupuestosController : Controller
{
    private readonly IOpPresupuestosService _presupuestosService;

    public PresupuestosController(IOpPresupuestosService presupuestosService)
    {
        _presupuestosService = presupuestosService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(long trabajoId)
    {
        if (trabajoId <= 0)
        {
            TempData["PresupuestoMessage"] = "Proporcione un TrabajoId válido.";
            return View(new PresupuestoViewModel());
        }

        var estado = await _presupuestosService.ObtenerEstadoAsync(trabajoId);

        var model = new PresupuestoViewModel
        {
            TrabajoId = trabajoId,
            TieneSolicitud = estado?.TieneSolicitud ?? false,
            ObservacionActual = estado?.Observacion ?? string.Empty,
            Completo = new SolicitudPresupuestoCompletoRequest
            {
                TrabajoId = trabajoId,
                UsuarioId = GetCurrentUserId()
            },
            Simplificado = new SolicitudPresupuestoSimplificadoRequest
            {
                TrabajoId = trabajoId,
                UsuarioId = GetCurrentUserId()
            }
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GuardarCompleta(SolicitudPresupuestoCompletoRequest request)
    {
        var success = await _presupuestosService.GuardarSolicitudCompletaAsync(request);
        TempData["PresupuestoMessage"] = success ? "Solicitud de presupuesto completo guardada." : "No fue posible guardar la solicitud.";

        return RedirectToAction(nameof(Index), new { trabajoId = request.TrabajoId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GuardarSimplificada(SolicitudPresupuestoSimplificadoRequest request)
    {
        var success = await _presupuestosService.GuardarSolicitudSimplificadaAsync(request);
        TempData["PresupuestoMessage"] = success ? "Solicitud simplificada registrada." : "No fue posible guardar la observación.";

        return RedirectToAction(nameof(Index), new { trabajoId = request.TrabajoId });
    }

    private long GetCurrentUserId()
    {
        var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (long.TryParse(idClaim, out var id))
        {
            return id;
        }
        return 0;
    }
}
