using MatrixNext.Web.Services.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.OP.Controllers;

[Area("OP")]
[Authorize]
public class PlanillasAprobacionController : Controller
{
    private readonly IOpPlanillasService _planillasService;

    public PlanillasAprobacionController(IOpPlanillasService planillasService)
    {
        _planillasService = planillasService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await _planillasService.ObtenerPlanillasAsync();
        return View(model);
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
