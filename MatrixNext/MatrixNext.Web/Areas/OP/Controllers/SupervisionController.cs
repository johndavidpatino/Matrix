using MatrixNext.Web.Services.OP;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.OP.Controllers;

[Area("OP")]
[Authorize]
public class SupervisionController : Controller
{
    private readonly IOpSupervisionService _supervisionService;
    private readonly IOpPermisosService _permisosService;

    public SupervisionController(IOpSupervisionService supervisionService, IOpPermisosService permisosService)
    {
        _supervisionService = supervisionService;
        _permisosService = permisosService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(long trabajoId, string identificacion, long operadorId = 0, long supervisorId = 0)
    {
        if (trabajoId <= 0)
        {
            TempData["SupervisionMessage"] = "TrabajoId es requerido.";
            return View(new SupervisionViewModel());
        }

        var usuarios = await _supervisionService.ObtenerUsuariosActivosAsync();
        var historico = await _supervisionService.ObtenerHistoricoAsync(trabajoId);
        var summary = await _supervisionService.ObtenerResumenAsync(trabajoId);
        var userId = GetCurrentUserId();
        if (!await _permisosService.TienePermisoAsync(userId, 157))
        {
            return Forbid();
        }

        var identificacionLong = long.TryParse(identificacion, out var parsedIdentificacion) ? parsedIdentificacion : 0;
        var model = new SupervisionViewModel
        {
            TrabajoId = trabajoId,
            Identificacion = identificacion,
            Operadores = usuarios,
            Supervisores = usuarios,
            Request = new GuardarSupervisionRequest
            {
                TrabajoId = trabajoId,
                Identificacion = identificacionLong,
                OperadorId = operadorId,
                SupervisorId = supervisorId,
                FechaSupervision = DateTime.UtcNow
            },
            Historico = historico,
            Summary = summary
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Guardar(GuardarSupervisionRequest request)
    {
        if (!await _permisosService.TienePermisoAsync(GetCurrentUserId(), 157))
        {
            return Forbid();
        }

        await _supervisionService.GuardarSupervisionAsync(request);
        TempData["SupervisionMessage"] = "SupervisiИn telefИnica registrada.";
        return RedirectToAction(nameof(Index), new { trabajoId = request.TrabajoId, identificacion = request.Identificacion });
    }

    private long GetCurrentUserId()
    {
        var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(idClaim, out var id) ? id : 0;
    }
}
