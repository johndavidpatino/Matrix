using System.Security.Claims;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.ViewModels.OP;
using MatrixNext.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.OP.Controllers;

[Area("OP")]
[Authorize]
public class PortalController : Controller
{
    private readonly IOpPortalService _portalService;
    private readonly IOpPermisosService _permisosService;
    private readonly ILogger<PortalController> _logger;

    public PortalController(IOpPortalService portalService, IOpPermisosService permisosService, ILogger<PortalController> logger)
    {
        _portalService = portalService;
        _permisosService = permisosService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(FiltrosVM filtros)
    {
        var snapshot = await _portalService.ObtenerPortalAsync(filtros);
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var hasUserId = long.TryParse(userIdClaim, out var userId);
        var tienePermiso = hasUserId && await _permisosService.TienePermisoAsync(userId, 100);
        var tienePermisoSupervision = hasUserId && await _permisosService.TienePermisoAsync(userId, 157);

        var viewModel = new OpPortalViewModel
        {
            Filtros = filtros ?? new FiltrosVM(),
            Snapshot = snapshot,
            TienePermiso100 = tienePermiso,
            TienePermisoSupervision = tienePermisoSupervision
        };
        return View(viewModel);
    }
}
