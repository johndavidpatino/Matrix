using MatrixNext.Web.Services.OP;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.OP.Controllers;

[Area("OP")]
[Authorize]
public class ProduccionController : Controller
{
    private readonly IOpProduccionService _produccionService;

    public ProduccionController(IOpProduccionService produccionService)
    {
        _produccionService = produccionService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(long trabajoId, string identificacion = "")
    {
        var unidades = await _produccionService.ObtenerUnidadesAsync(string.IsNullOrWhiteSpace(identificacion) ? null : identificacion.AsLongOrNull(), CancellationToken.None);
        var actividades = await _produccionService.ObtenerActividadesAsync(null, null, CancellationToken.None);
        var jbes = await _produccionService.ObtenerJbeAsync(1, null, CancellationToken.None);
        var registros = await _produccionService.ObtenerProduccionAsync(null, null, string.IsNullOrWhiteSpace(identificacion) ? null : identificacion, null, CancellationToken.None);

        var model = new ProduccionViewModel
        {
            Identificacion = identificacion,
            TrabajoId = trabajoId,
            UsuarioId = GetCurrentUserId(),
            Unidades = unidades,
            Actividades = actividades,
            SubActividades = actividades,
            Jbes = jbes,
            Registros = registros
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Guardar(GuardarRegistroRequest request)
    {
        request = request with { UsuarioId = GetCurrentUserId() };
        await _produccionService.GuardarRegistroAsync(request);
        TempData["ProduccionMessage"] = "Registro guardado.";
        return RedirectToAction(nameof(Index), new { trabajoId = request.TrabajoId });
    }

    private long GetCurrentUserId()
    {
        return long.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
    }
}

internal static class StringExtensions
{
    public static long? AsLongOrNull(this string value)
    {
        return long.TryParse(value, out var result) ? result : (long?)null;
    }
}
