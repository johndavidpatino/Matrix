using System.Security.Claims;
using MatrixNext.Web.Services.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controlador para gestión de trabajos de Call Center
/// </summary>
/// <remarks>
/// Migración de WebMatrix/OP_Cuantitativo/TrabajosCallCenter.aspx
/// Permiso requerido: 101 (Coordinador Call Center)
/// </remarks>
[Area("OP")]
[Authorize]
public class TrabajosCallCenterController : Controller
{
    private readonly IOpCoordinacionService _coordinacionService;
    private readonly IOpPermisosService _permisosService;
    private readonly ILogger<TrabajosCallCenterController> _logger;

    public TrabajosCallCenterController(
        IOpCoordinacionService coordinacionService,
        IOpPermisosService permisosService,
        ILogger<TrabajosCallCenterController> logger)
    {
        _coordinacionService = coordinacionService;
        _permisosService = permisosService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(long? trabajoId, string? nombre, string? jobBook, int? estado)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        var tienePermiso = await _permisosService.TienePermisoAsync(userId, 101);
        if (!tienePermiso)
        {
            TempData["Error"] = "No tiene permisos para acceder a esta sección";
            return RedirectToAction("Index", "Home", new { area = "OP" });
        }

        var trabajos = await _coordinacionService.ObtenerTrabajosCallCenterAsync(trabajoId, nombre, jobBook, estado);

        ViewBag.UserId = userId;
        return View(trabajos);
    }

    [HttpGet]
    public async Task<IActionResult> PersonalAsignado(long trabajoId, int? ciudadId)
    {
        var personal = await _coordinacionService.ObtenerPersonalAsignadoAsync(trabajoId, ciudadId);
        return Json(new { success = true, data = personal });
    }

    [HttpGet]
    public async Task<IActionResult> EncuestadoresDisponibles(long trabajoId, int? ciudadId)
    {
        var personal = await _coordinacionService.ObtenerPersonalDisponibleAsync(trabajoId, ciudadId);
        return Json(new { success = true, data = personal });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AsignarEncuestador(long trabajoId, long personalId, int? ciudadId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Json(new { success = false, message = "Usuario no válido" });
        }

        var resultado = await _coordinacionService.AsignarPersonalAsync(trabajoId, personalId, ciudadId, userId);
        return Json(new { success = resultado, message = resultado ? "Encuestador asignado exitosamente" : "Error al asignar encuestador" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RetirarEncuestador(long asignacionId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Json(new { success = false, message = "Usuario no válido" });
        }

        var resultado = await _coordinacionService.RetirarPersonalAsync(asignacionId, userId);
        return Json(new { success = resultado, message = resultado ? "Encuestador retirado exitosamente" : "Error al retirar encuestador" });
    }
}
