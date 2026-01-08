using System.Security.Claims;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Services.PY;
using MatrixNext.Web.ViewModels.OP;
using MatrixNext.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.OP.Controllers;

[Area("OP")]
[Authorize]
public class EncuestasController : Controller
{
    private readonly IOpEncuestasService _encuestasService;
    private readonly ITrabajosService _trabajosService;
    private readonly ILogger<EncuestasController> _logger;

    public EncuestasController(
        IOpEncuestasService encuestasService,
        ITrabajosService trabajosService,
        ILogger<EncuestasController> logger)
    {
        _encuestasService = encuestasService;
        _trabajosService = trabajosService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await ConstruirModeloAsync();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activar(OpEncuestaFormModel form)
    {
        if (!ModelState.IsValid)
        {
            TempData["Mensaje"] = "Complete los campos obligatorios.";
            TempData["Exito"] = "false";
            return RedirectToAction(nameof(Index));
        }

        var usuarioId = ObtenerUsuarioId();
        var exito = await _encuestasService.ActivarEncuestaAsync(form.TrabajoId, form.NumeroEncuesta, form.Observacion, usuarioId);
        TempData["Mensaje"] = exito ? "Encuesta activada correctamente." : "No se pudo activar la encuesta.";
        TempData["Exito"] = exito.ToString().ToLower();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Anular(OpEncuestaFormModel form)
    {
        if (!ModelState.IsValid)
        {
            TempData["Mensaje"] = "Complete los campos obligatorios.";
            TempData["Exito"] = "false";
            return RedirectToAction(nameof(Index));
        }

        var exito = await _encuestasService.AnularEncuestaAsync(form.TrabajoId, form.NumeroEncuesta, form.Observacion);
        TempData["Mensaje"] = exito ? "Encuesta anulada correctamente." : "No se pudo anular la encuesta.";
        TempData["Exito"] = exito.ToString().ToLower();
        return RedirectToAction(nameof(Index));
    }

    private async Task<OpEncuestasViewModel> ConstruirModeloAsync()
    {
        var filtros = new FiltrosVM { PageNumber = 1, PageSize = 100 };
        var trabajos = (await _trabajosService.ListarAsync(filtros)).Items;
        var mensaje = TempData["Mensaje"] as string;
        var exito = TempData["Exito"] as string == "true";

        return new OpEncuestasViewModel
        {
            Trabajos = trabajos,
            Mensaje = mensaje,
            Exito = exito
        };
    }

    private long ObtenerUsuarioId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(claim, out var userId) ? userId : 0;
    }
}
