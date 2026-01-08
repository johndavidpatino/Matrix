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
    private readonly ILogger<PortalController> _logger;

    public PortalController(IOpPortalService portalService, ILogger<PortalController> logger)
    {
        _portalService = portalService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(FiltrosVM filtros)
    {
        var snapshot = await _portalService.ObtenerPortalAsync(filtros);
        var viewModel = new OpPortalViewModel
        {
            Filtros = filtros ?? new FiltrosVM(),
            Snapshot = snapshot
        };
        return View(viewModel);
    }
}
