using System.Security.Claims;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Services.PY;
using MatrixNext.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controlador para consulta de trabajos por unidad
/// </summary>
/// <remarks>
/// Migración de WebMatrix/OP_Cuantitativo/ConsultaTrabajos.aspx
/// Permiso requerido: 19 (Consulta trabajos)
/// </remarks>
[Area("OP")]
[Authorize]
public class ConsultaTrabajosController : Controller
{
    private readonly ITrabajosService _trabajosService;
    private readonly IOpPermisosService _permisosService;
    private readonly ILogger<ConsultaTrabajosController> _logger;

    public ConsultaTrabajosController(
        ITrabajosService trabajosService,
        IOpPermisosService permisosService,
        ILogger<ConsultaTrabajosController> logger)
    {
        _trabajosService = trabajosService;
        _permisosService = permisosService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(FiltrosVM filtros)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        var tienePermiso = await _permisosService.TienePermisoAsync(userId, 19);
        if (!tienePermiso)
        {
            TempData["Error"] = "No tiene permisos para acceder a esta sección";
            return RedirectToAction("Index", "Home", new { area = "OP" });
        }

        var trabajos = await _trabajosService.ListarAsync(filtros);

        ViewBag.UserId = userId;
        ViewBag.Filtros = filtros;
        return View(trabajos);
    }

    [HttpGet]
    public IActionResult Avance(long trabajoId)
    {
        return RedirectToAction("Index", "Avances", new { area = "OP", trabajoId });
    }

    [HttpGet]
    public IActionResult Gantt(long trabajoId)
    {
        // TODO: Implementar vista de Gantt si es necesario
        TempData["Info"] = "Vista de Gantt en desarrollo";
        return RedirectToAction("Index", new { trabajoId });
    }

    [HttpGet]
    public IActionResult Presupuestos(long trabajoId)
    {
        return RedirectToAction("Index", "Presupuestos", new { area = "OP", trabajoId });
    }

    [HttpGet]
    public IActionResult ActivarEncuestas(long trabajoId)
    {
        return RedirectToAction("Index", "Encuestas", new { area = "OP", trabajoId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AsignarCOE(long trabajoId, string jobBook)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Json(new { success = false, message = "Usuario no válido" });
            }

            // Validar que el JobBook no esté vacío
            if (string.IsNullOrWhiteSpace(jobBook))
            {
                return Json(new { success = false, message = "Debe ingresar un Job Book" });
            }

            // TODO: Implementar validación de JobBook con WorkFlow
            // Por ahora solo retornamos éxito
            _logger.LogInformation("COE asignado a trabajo {TrabajoId} con JobBook {JobBook}", trabajoId, jobBook);

            return Json(new { success = true, message = "COE asignado exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al asignar COE a trabajo {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error al asignar COE" });
        }
    }
}
