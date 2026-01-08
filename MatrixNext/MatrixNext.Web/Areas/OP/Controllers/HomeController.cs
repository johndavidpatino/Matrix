using System.Security.Claims;
using MatrixNext.Web.Services.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controlador Home para módulo OP
/// </summary>
/// <remarks>
/// Migración de WebMatrix/OP_Cuantitativo/HomeRecoleccion.aspx y HomeGestion.aspx
/// Permiso requerido: 54 (Acceso a OP)
/// </remarks>
[Area("OP")]
[Authorize]
public class HomeController : Controller
{
    private readonly IOpPortalService _portalService;
    private readonly IOpPermisosService _permisosService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        IOpPortalService portalService,
        IOpPermisosService permisosService,
        ILogger<HomeController> logger)
    {
        _portalService = portalService;
        _permisosService = permisosService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        var tienePermiso = await _permisosService.TienePermisoAsync(userId, 54);
        if (!tienePermiso)
        {
            TempData["Error"] = "No tiene permisos para acceder al módulo OP";
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // Obtener KPIs básicos
        var snapshot = await _portalService.ObtenerPortalAsync(new ViewModels.FiltrosVM());

        ViewBag.UserId = userId;
        ViewBag.TrabajosActivos = snapshot.Trabajos.TotalRecords;
        ViewBag.TienePermiso100 = await _permisosService.TienePermisoAsync(userId, 100);
        ViewBag.TienePermiso101 = await _permisosService.TienePermisoAsync(userId, 101);
        ViewBag.TienePermiso19 = await _permisosService.TienePermisoAsync(userId, 19);

        return View();
    }
}
