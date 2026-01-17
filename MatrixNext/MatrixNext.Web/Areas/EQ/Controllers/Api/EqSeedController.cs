using MatrixNext.Web.Services.EQ;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.EQ.Controllers.Api;

/// <summary>
/// Endpoint temporal para ejecutar seeds de EasyQuote
/// SOLO DESARROLLO - Remover en producción
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EqSeedController : ControllerBase
{
    private readonly EqSeedService _seedService;
    private readonly ILogger<EqSeedController> _logger;

    public EqSeedController(EqSeedService seedService, ILogger<EqSeedController> logger)
    {
        _seedService = seedService;
        _logger = logger;
    }

    /// <summary>
    /// GET /api/eqseed/status
    /// Verifica estado actual de las maestras
    /// </summary>
    [HttpGet("status")]
    public async Task<IActionResult> GetStatus()
    {
        try
        {
            var status = await _seedService.CheckMasterDataStatusAsync();
            return Ok(new
            {
                success = true,
                allPopulated = status.AllTablesPopulated,
                status = status.ToString(),
                counts = new
                {
                    precios = status.PreciosCount,
                    horas = status.HorasCount,
                    tarifas = status.TarifasCount,
                    costos = status.CostosCount,
                    rates = status.RatesCount,
                    locaciones = status.LocacionesCount
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking master data status");
            return StatusCode(500, new { success = false, error = "Error al verificar estado de maestras. Por favor intente nuevamente." });
        }
    }

    /// <summary>
    /// POST /api/eqseed/run
    /// Ejecuta seed de maestras (solo si NO existen datos)
    /// </summary>
    [HttpPost("run")]
    public async Task<IActionResult> RunSeed([FromQuery] bool force = false)
    {
        try
        {
            var result = await _seedService.SeedAllMasterTablesAsync(force);
            
            if (result.Success)
            {
                return Ok(new
                {
                    success = true,
                    message = "Seed ejecutado exitosamente",
                    seeded = result.TablasSeeded,
                    skipped = result.TablasSkipped,
                    force
                });
            }
            else
            {
                return BadRequest(new
                {
                    success = false,
                    error = result.ErrorMessage
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running seed");
            return StatusCode(500, new { success = false, error = "Error al ejecutar seed. Por favor intente nuevamente." });
        }
    }

    /// <summary>
    /// DELETE /api/eqseed/clear
    /// Limpia todas las maestras (SOLO DESARROLLO)
    /// </summary>
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearAll()
    {
        try
        {
            await _seedService.ClearAllMasterTablesAsync();
            return Ok(new
            {
                success = true,
                message = "Maestras limpiadas (SOLO DESARROLLO)"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing master data");
            return StatusCode(500, new { success = false, error = "Error al limpiar maestras. Por favor intente nuevamente." });
        }
    }
}
