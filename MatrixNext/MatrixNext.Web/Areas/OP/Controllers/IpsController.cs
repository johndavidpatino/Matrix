using MatrixNext.Web.Services.OP;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace MatrixNext.Web.Areas.OP.Controllers;

[Area("OP")]
[Authorize]
public class IpsController : Controller
{
    private readonly IOpIpsService _ipsService;

    public IpsController(IOpIpsService ipsService)
    {
        _ipsService = ipsService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(long? trabajoId)
    {
        var model = await _ipsService.ObtenerRevisionesAsync(trabajoId);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Guardar(IpsRevisionUpdateModel model)
    {
        var success = await _ipsService.GuardarRevisionAsync(model);
        TempData["IpsMessage"] = success ? "Revisión guardada" : "No se pudo guardar";
        return RedirectToAction(nameof(Index), new { trabajoId = model.TrabajoId });
    }

    [HttpGet]
    public async Task<IActionResult> Exportar(long? trabajoId)
    {
        var result = await _ipsService.ExportarRevisionesAsync(trabajoId);
        return PhysicalFile(result.PhysicalPath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(result.PhysicalPath));
    }
}
