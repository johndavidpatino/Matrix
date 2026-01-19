using MatrixNext.Data.DTOs.RP;
using MatrixNext.Data.Services.RP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.RP.Controllers;

/// <summary>
/// Controller para avance de campo
/// Migrado de: WebMatrix/RP_Reportes/AvanceDeCampo.aspx
/// SP: REP_AvanceCampoGeneral, REP_AvanceCampoxCiudad, REP_AvancePorcentualAreas, etc.
/// </summary>
[Area("RP")]
[Authorize]
public class AvanceCampoController : Controller
{
    private readonly IAvanceCampoService _service;
    private readonly ILogger<AvanceCampoController> _logger;

    public AvanceCampoController(IAvanceCampoService service, ILogger<AvanceCampoController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Vista principal de avance de campo
    /// GET: /RP/AvanceCampo/Index?trabajoId=123
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(long? trabajoId)
    {
        try
        {
            if (!trabajoId.HasValue)
            {
                TempData["Warning"] = "Debe especificar un trabajo";
                return View(new AvanceCampoViewModel());
            }

            var viewModel = await _service.PrepararViewModelAsync(trabajoId.Value);
            
            if (!viewModel.TieneDatos)
            {
                TempData["Warning"] = "El trabajo no tiene datos de estimación de producción";
            }

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar avance de campo. TrabajoId: {TrabajoId}", trabajoId);
            TempData["Error"] = "Error al cargar el avance de campo";
            return View(new AvanceCampoViewModel { TrabajoId = trabajoId ?? 0 });
        }
    }

    /// <summary>
    /// Vista de diálogo (popup) para avance de campo
    /// GET: /RP/AvanceCampo/Dialog?trabajoId=123
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Dialog(long trabajoId)
    {
        try
        {
            var viewModel = await _service.PrepararViewModelAsync(trabajoId);
            return PartialView("_AvanceCampoDialog", viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar diálogo avance campo. TrabajoId: {TrabajoId}", trabajoId);
            return PartialView("_AvanceCampoDialog", new AvanceCampoViewModel { TrabajoId = trabajoId });
        }
    }

    /// <summary>
    /// Obtener avance general via AJAX
    /// GET: /RP/AvanceCampo/AvanceGeneral?trabajoId=123
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> AvanceGeneral(long trabajoId)
    {
        try
        {
            var avance = await _service.ObtenerAvanceGeneralAsync(trabajoId);
            return Json(new { success = true, data = avance });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener avance general. TrabajoId: {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error al obtener datos" });
        }
    }

    /// <summary>
    /// Obtener avance por ciudad via AJAX
    /// GET: /RP/AvanceCampo/AvanceCiudad?trabajoId=123
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> AvanceCiudad(long trabajoId)
    {
        try
        {
            var datos = await _service.ObtenerAvancePorCiudadAsync(trabajoId);
            return Json(new { success = true, data = datos });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener avance ciudad. TrabajoId: {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error al obtener datos" });
        }
    }

    /// <summary>
    /// Obtener avance por áreas via AJAX
    /// GET: /RP/AvanceCampo/AvanceAreas?trabajoId=123
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> AvanceAreas(long trabajoId)
    {
        try
        {
            var datos = await _service.ObtenerAvancePorAreasAsync(trabajoId);
            return Json(new { success = true, data = datos });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener avance áreas. TrabajoId: {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error al obtener datos" });
        }
    }

    /// <summary>
    /// Obtener remanentes via AJAX
    /// GET: /RP/AvanceCampo/Remanentes?trabajoId=123
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Remanentes(long trabajoId)
    {
        try
        {
            var datos = await _service.ObtenerRemanentesAsync(trabajoId);
            return Json(new { success = true, data = datos });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener remanentes. TrabajoId: {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error al obtener datos" });
        }
    }

    /// <summary>
    /// Obtener matriz de cumplimiento via AJAX
    /// GET: /RP/AvanceCampo/MatrizCumplimiento?trabajoId=123
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> MatrizCumplimiento(long trabajoId)
    {
        try
        {
            var datos = await _service.ObtenerMatrizCumplimientoAsync(trabajoId);
            return Json(new { success = true, data = datos });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener matriz. TrabajoId: {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error al obtener datos" });
        }
    }

    /// <summary>
    /// Exportar avance de campo a Excel
    /// GET: /RP/AvanceCampo/Exportar?trabajoId=123
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Exportar(long trabajoId)
    {
        try
        {
            var bytes = await _service.ExportarExcelAsync(trabajoId);
            var fileName = $"AvanceCampo_Trabajo_{trabajoId}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al exportar avance campo. TrabajoId: {TrabajoId}", trabajoId);
            TempData["Error"] = "Error al exportar";
            return RedirectToAction("Index", new { trabajoId });
        }
    }
}
