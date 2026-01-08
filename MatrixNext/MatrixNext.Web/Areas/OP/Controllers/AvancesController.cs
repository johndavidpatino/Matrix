using System.Linq;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.OP.Controllers;

[Area("OP")]
[Authorize]
public class AvancesController : Controller
{
    private readonly IOpAvancesService _opAvancesService;
    private readonly ILogger<AvancesController> _logger;

    public AvancesController(IOpAvancesService opAvancesService, ILogger<AvancesController> logger)
    {
        _opAvancesService = opAvancesService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var snapshot = await _opAvancesService.GetSnapshotAsync();

        var viewModel = new OpAvancesViewModel
        {
            LastUpdated = snapshot.LastUpdated,
            FocusNote = snapshot.FocusNote,
            Flows = snapshot.Flows.Select(flow => new OpFlowViewModel
            {
                Title = flow.Title,
                WebForms = flow.WebForms,
                CoreProjectDependencies = flow.CoreProjectDependencies,
                Status = flow.Status,
                NextAction = flow.NextAction,
                ReferenceDoc = flow.ReferenceDoc
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult RedirectToDocs()
    {
        return Redirect("/docs/OP/OP_CUANTITATIVO_AVANCE.md");
    }
}
