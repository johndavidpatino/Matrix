using MatrixNext.Data.DTOs.INV;
using MatrixNext.Data.Services.INV;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.INV.Controllers;

/// <summary>
/// Controller para reportes de inventario (legalizaciones y remanente)
/// Migrado de: WebMatrix/Inventario/ReporteLegalizaciones.aspx, ReporteRemanente.aspx
/// </summary>
[Area("INV")]
[Authorize]
public class ReportesController : Controller
{
    private readonly IReportesInvService _service;
    private readonly ILogger<ReportesController> _logger;

    public ReportesController(IReportesInvService service, ILogger<ReportesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    #region Reporte Legalizaciones

    /// <summary>
    /// Vista principal de reporte de legalizaciones
    /// GET: /INV/Reportes/Legalizaciones
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Legalizaciones()
    {
        try
        {
            ViewBag.BUs = await _service.ObtenerBUsAsync();
            ViewBag.TiposArticulo = await _service.ObtenerTiposArticuloAsync();
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar vista de legalizaciones");
            TempData["Error"] = "Error al cargar la página de reportes";
            return RedirectToAction("Index", "RegistroArticulos");
        }
    }

    /// <summary>
    /// Buscar legalizaciones con filtros
    /// POST: /INV/Reportes/BuscarLegalizaciones
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> BuscarLegalizaciones([FromBody] ReporteLegalizacionFiltrosDto filtros)
    {
        try
        {
            var datos = await _service.ObtenerReporteLegalizacionesAsync(filtros);
            return Json(new { success = true, data = datos });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar legalizaciones");
            return Json(new { success = false, message = "Error al obtener los datos" });
        }
    }

    /// <summary>
    /// Exportar legalizaciones a Excel
    /// POST: /INV/Reportes/ExportarLegalizaciones
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ExportarLegalizaciones([FromBody] ReporteLegalizacionFiltrosDto filtros)
    {
        try
        {
            var bytes = await _service.ExportarLegalizacionesExcelAsync(filtros);
            var fileName = $"ReporteLegalizaciones_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al exportar legalizaciones");
            return BadRequest(new { success = false, message = "Error al exportar" });
        }
    }

    #endregion

    #region Reporte Remanente

    /// <summary>
    /// Vista principal de reporte de remanente
    /// GET: /INV/Reportes/Remanente
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Remanente()
    {
        try
        {
            ViewBag.TiposArticulo = await _service.ObtenerTiposArticuloAsync();
            ViewBag.TiposProducto = await _service.ObtenerTiposProductoAsync();
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar vista de remanente");
            TempData["Error"] = "Error al cargar la página de reportes";
            return RedirectToAction("Index", "RegistroArticulos");
        }
    }

    /// <summary>
    /// Buscar remanente con filtros
    /// POST: /INV/Reportes/BuscarRemanente
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> BuscarRemanente([FromBody] ReporteRemanenteFiltrosDto filtros)
    {
        try
        {
            var datos = await _service.ObtenerReporteRemanenteAsync(filtros);
            return Json(new { success = true, data = datos });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar remanente");
            return Json(new { success = false, message = "Error al obtener los datos" });
        }
    }

    /// <summary>
    /// Exportar remanente a Excel
    /// POST: /INV/Reportes/ExportarRemanente
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ExportarRemanente([FromBody] ReporteRemanenteFiltrosDto filtros)
    {
        try
        {
            var bytes = await _service.ExportarRemanenteExcelAsync(filtros);
            var fileName = $"ReporteRemanente_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al exportar remanente");
            return BadRequest(new { success = false, message = "Error al exportar" });
        }
    }

    #endregion
}
