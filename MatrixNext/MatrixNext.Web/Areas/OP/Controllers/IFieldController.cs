using MatrixNext.Web.Services.OP;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.OP.Controllers;

[Area("OP")]
[Authorize]
public class IFieldController : Controller
{
    private readonly IOpIFieldService _iFieldService;

    public IFieldController(IOpIFieldService iFieldService)
    {
        _iFieldService = iFieldService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int tipo = 1, int? projectId = null)
    {
        var projects = await _iFieldService.GetProjectsAsync(tipo);
        var selected = projectId.HasValue ? await _iFieldService.GetProjectAsync(projectId.Value) : null;
        var config = selected != null ? await _iFieldService.GetProjectConfigAsync(selected.ProjectId) : Array.Empty<IFieldConfigRow>();
        var pending = selected != null ? await _iFieldService.GetPendientesAsync(selected.ProjectId) : Array.Empty<IFieldPendingRow>();

        var model = new IFieldViewModel
        {
            TipoBusqueda = tipo,
            Projects = projects,
            SelectedProject = selected,
            Configuracion = config,
            Pendientes = pending,
            JobBook = selected?.TrabajoId?.ToString() ?? string.Empty
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProject(int projectId, int trabajoId)
    {
        await _iFieldService.UpdateProjectJobBookAsync(projectId, trabajoId);
        TempData["IFieldMessage"] = "Trabajo asociado actualizado.";
        return RedirectToAction(nameof(Index), new { projectId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddConfig(int projectId, string entries)
    {
        var inputs = ParseEntries(projectId, entries).ToList();
        if (!inputs.Any())
        {
            TempData["IFieldMessage"] = "No se detectaron líneas válidas.";
            return RedirectToAction(nameof(Index), new { projectId });
        }

        await _iFieldService.InsertConfigItemsAsync(inputs);
        TempData["IFieldMessage"] = "Configuraciones agregadas.";
        return RedirectToAction(nameof(Index), new { projectId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveConfig(int projectId, int configId)
    {
        await _iFieldService.RemoveConfigItemAsync(configId);
        TempData["IFieldMessage"] = "Configuración removida.";
        return RedirectToAction(nameof(Index), new { projectId });
    }

    private IEnumerable<IFieldAddConfigInput> ParseEntries(int projectId, string entries)
    {
        var lines = entries.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var userId = GetCurrentUserId();
        foreach (var line in lines)
        {
            var parts = line.Trim().Split('\t');
            if (parts.Length < 3) continue;
            if (!long.TryParse(parts[1], out var enc)) continue;
            if (!long.TryParse(parts[2], out var sup)) continue;

            yield return new IFieldAddConfigInput(projectId, parts[0], enc, sup, userId);
        }
    }

    private long GetCurrentUserId()
    {
        return long.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
    }
}
