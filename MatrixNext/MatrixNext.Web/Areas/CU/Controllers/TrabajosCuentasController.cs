using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using MatrixNext.Data.Modules.CU.TrabajosCuentas.Models;
using MatrixNext.Data.Modules.CU.TrabajosCuentas.Services;

namespace MatrixNext.Web.Areas.CU.Controllers;

/// <summary>
/// Controller para gestión de Trabajos de Cuenta
/// Migrado de: WebMatrix/CU_Cuentas/TrabajosCuentas.aspx
/// SP: CU_Trabajos_Get
/// </summary>
[Area("CU")]
[Authorize]
public class TrabajosCuentasController : Controller
{
    private readonly ITrabajoCuentaService _service;
    private readonly ILogger<TrabajosCuentasController> _logger;

    public TrabajosCuentasController(ITrabajoCuentaService service, ILogger<TrabajosCuentasController> logger)
    {
        _service = service;
        _logger = logger;
    }

    private long GetCurrentUserId()
    {
        var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User?.FindFirst("Id")?.Value;
        return long.TryParse(idClaim, out var id) ? id : 0;
    }

    #region Vistas

    /// <summary>
    /// GET: /CU/TrabajosCuentas?estudioId=123
    /// Página principal - Lista de trabajos por estudio
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(long? estudioId)
    {
        if (!estudioId.HasValue)
        {
            TempData["ErrorMessage"] = "Debe seleccionar un estudio primero";
            return RedirectToAction("Index", "Estudios", new { area = "CU" });
        }

        var viewModel = await _service.PrepararViewModelAsync(estudioId.Value);
        return View(viewModel);
    }

    #endregion

    #region API AJAX

    /// <summary>
    /// GET: /CU/TrabajosCuentas/Buscar
    /// Buscar trabajos con filtros
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Buscar([FromQuery] TrabajoCuentaBusquedaParams filtros)
    {
        var trabajos = await _service.ObtenerTrabajosAsync(filtros);
        return PartialView("_ListaTrabajos", trabajos);
    }

    /// <summary>
    /// GET: /CU/TrabajosCuentas/Obtener/5
    /// Obtener un trabajo por ID
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Obtener(long id)
    {
        var trabajo = await _service.ObtenerTrabajoPorIdAsync(id);
        if (trabajo == null)
        {
            return NotFound(new { success = false, message = "Trabajo no encontrado" });
        }
        return Json(new { success = true, data = trabajo });
    }

    /// <summary>
    /// GET: /CU/TrabajosCuentas/Detalle/5
    /// Ver detalle de un trabajo (modal)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Detalle(long id)
    {
        var trabajo = await _service.ObtenerTrabajoPorIdAsync(id);
        if (trabajo == null)
        {
            return NotFound();
        }
        return PartialView("_Detalle", trabajo);
    }

    #endregion
}
