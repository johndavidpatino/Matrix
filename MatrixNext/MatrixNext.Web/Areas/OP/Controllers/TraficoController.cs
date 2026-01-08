using MatrixNext.Web.Services.OP;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.OP.Controllers;

[Area("OP")]
[Authorize]
public class TraficoController : Controller
{
    private readonly IOpTraficoService _traficoService;
    private readonly ILogger<TraficoController> _logger;

    public TraficoController(IOpTraficoService traficoService, ILogger<TraficoController> logger)
    {
        _traficoService = traficoService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(long? trabajoId)
    {
        var summary = await _traficoService.ObtenerResumenAsync(trabajoId);
        var viewModel = new OpTraficoViewModel
        {
            Summary = summary
        };

        return View(viewModel);
    }
}
