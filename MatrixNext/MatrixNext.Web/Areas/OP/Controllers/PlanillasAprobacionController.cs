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
}
