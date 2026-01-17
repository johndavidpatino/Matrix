using MatrixNext.Web.Services.EQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MatrixNext.Web.Areas.EQ.Controllers;

[Area("EQ")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class EasyQuoteSeedController : Controller
{
    private readonly EqSeedService _seedService;
    private readonly ILogger<EasyQuoteSeedController> _logger;

    public EasyQuoteSeedController(
        EqSeedService seedService,
        ILogger<EasyQuoteSeedController> logger)
    {
        _seedService = seedService;
        _logger = logger;
    }

    /// <summary>
    /// Página de administración de seed de maestras EasyQuote
    /// GET: /EQ/EasyQuoteSeed/Index
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var status = await _seedService.CheckMasterDataStatusAsync();
        return View(status);
    }

    /// <summary>
    /// Ejecuta seed de todas las maestras (skip si ya tienen datos)
    /// POST: /EQ/EasyQuoteSeed/SeedAll
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SeedAll()
    {
        try
        {
            _logger.LogInformation("Admin iniciando seed de maestras EasyQuote...");
            var result = await _seedService.SeedAllMasterTablesAsync(force: false);

            if (result.Success)
            {
                TempData["SuccessMessage"] = $"✅ Seed exitoso: {result.TablasSeeded.Count} tablas seeded, {result.TablasSkipped.Count} skipped. " +
                                              $"Tablas: {string.Join(", ", result.TablasSeeded)}";
            }
            else
            {
                TempData["ErrorMessage"] = $"❌ Error durante seed: {result.ErrorMessage}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ejecutando seed de maestras EasyQuote");
            TempData["ErrorMessage"] = "❌ Error inesperado al ejecutar seed. Por favor intente nuevamente.";
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Fuerza seed de todas las maestras (sobrescribe datos existentes)
    /// POST: /EQ/EasyQuoteSeed/ForceSeed
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForceSeed()
    {
        try
        {
            _logger.LogWarning("Admin forzando seed de maestras EasyQuote (sobrescribir)...");
            
            // Primero limpia
            await _seedService.ClearAllMasterTablesAsync();
            
            // Luego siembra
            var result = await _seedService.SeedAllMasterTablesAsync(force: true);

            if (result.Success)
            {
                TempData["SuccessMessage"] = $"✅ Force seed exitoso: {result.TablasSeeded.Count} tablas re-seeded. " +
                                              $"Tablas: {string.Join(", ", result.TablasSeeded)}";
            }
            else
            {
                TempData["ErrorMessage"] = $"❌ Error durante force seed: {result.ErrorMessage}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ejecutando force seed de maestras EasyQuote");
            TempData["ErrorMessage"] = "❌ Error inesperado al ejecutar force seed. Por favor intente nuevamente.";
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Limpia todas las maestras (SOLO DESARROLLO)
    /// POST: /EQ/EasyQuoteSeed/ClearAll
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ClearAll()
    {
        try
        {
            _logger.LogWarning("Admin limpiando TODAS las maestras EasyQuote...");
            await _seedService.ClearAllMasterTablesAsync();
            TempData["SuccessMessage"] = "✅ Maestras limpiadas exitosamente";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error limpiando maestras EasyQuote");
            TempData["ErrorMessage"] = "❌ Error al limpiar maestras. Por favor intente nuevamente.";
        }

        return RedirectToAction(nameof(Index));
    }
}
