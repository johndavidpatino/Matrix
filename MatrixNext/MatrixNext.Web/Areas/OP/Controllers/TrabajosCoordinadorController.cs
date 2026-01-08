using System.Security.Claims;
using MatrixNext.Web.Services.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controlador para gestión de trabajos por Coordinador
/// </summary>
/// <remarks>
/// Migración de WebMatrix/OP_Cuantitativo/TrabajosCoordinador.aspx
/// Permiso requerido: 101 (Coordinador)
/// </remarks>
[Area("OP")]
[Authorize]
public class TrabajosCoordinadorController : Controller
{
    private readonly IOpCoordinacionService _coordinacionService;
    private readonly IOpPermisosService _permisosService;
    private readonly ILogger<TrabajosCoordinadorController> _logger;

    public TrabajosCoordinadorController(
        IOpCoordinacionService coordinacionService,
        IOpPermisosService permisosService,
        ILogger<TrabajosCoordinadorController> logger)
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

        var trabajos = await _coordinacionService.ObtenerTrabajosPorCoordinadorAsync(userId, trabajoId, nombre, jobBook, estado);

        ViewBag.UserId = userId;
        ViewBag.TrabajoBuscado = trabajoId;
        return View(trabajos);
    }

    [HttpGet]
    public async Task<IActionResult> CiudadesAsignadas(long trabajoId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Json(new { success = false, message = "Usuario no válido" });
        }

        var ciudades = await _coordinacionService.ObtenerCiudadesAsignadasAsync(userId, trabajoId);
        return Json(new { success = true, data = ciudades });
    }

    [HttpGet]
    public async Task<IActionResult> PersonalAsignado(long trabajoId, int? ciudadId)
    {
        var personal = await _coordinacionService.ObtenerPersonalAsignadoAsync(trabajoId, ciudadId);
        return Json(new { success = true, data = personal });
    }

    [HttpGet]
    public async Task<IActionResult> PersonalDisponible(long trabajoId, int? ciudadId)
    {
        var personal = await _coordinacionService.ObtenerPersonalDisponibleAsync(trabajoId, ciudadId);
        return Json(new { success = true, data = personal });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AsignarPersonal(long trabajoId, long personalId, int? ciudadId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Json(new { success = false, message = "Usuario no válido" });
        }

        var resultado = await _coordinacionService.AsignarPersonalAsync(trabajoId, personalId, ciudadId, userId);
        return Json(new { success = resultado, message = resultado ? "Personal asignado exitosamente" : "Error al asignar personal" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RetirarPersonal(long asignacionId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Json(new { success = false, message = "Usuario no válido" });
        }

        var resultado = await _coordinacionService.RetirarPersonalAsync(asignacionId, userId);
        return Json(new { success = resultado, message = resultado ? "Personal retirado exitosamente" : "Error al retirar personal" });
    }
}
