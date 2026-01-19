using MatrixNext.Data.DTOs.PY;
using MatrixNext.Data.Services.PY;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.PY.Controllers;

[Area("PY")]
[Authorize]
public class VariablesControlReportesController : Controller
{
    private readonly IVariablesControlService _service;
    private readonly ILogger<VariablesControlReportesController> _logger;

    public VariablesControlReportesController(
        IVariablesControlService service,
        ILogger<VariablesControlReportesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Vista principal de reportes
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            // Cargar empleados para filtro
            var empleados = await _service.ObtenerEmpleadosConEvaluacionAsync();
            ViewBag.Empleados = empleados;
            
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando vista de reportes");
            TempData["Error"] = "Error al cargar la página de reportes";
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }

    /// <summary>
    /// Reporte detallado (AJAX)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ReporteDetallado([FromBody] VariablesControlFiltrosDto filtros)
    {
        try
        {
            var data = await _service.ObtenerReporteVariablesControlAsync(filtros);
            return Json(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generando reporte detallado");
            return Json(new { error = "Error al generar el reporte" });
        }
    }

    /// <summary>
    /// Reporte por mes (AJAX)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ReportePorMes([FromBody] VariablesControlFiltrosDto filtros)
    {
        try
        {
            var data = await _service.ObtenerReporteVariablesControlPorMesAsync(filtros);
            return Json(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generando reporte por mes");
            return Json(new { error = "Error al generar el reporte" });
        }
    }

    /// <summary>
    /// Exportar a Excel
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Exportar([FromBody] ExportarVariablesControlRequest request)
    {
        try
        {
            var bytes = await _service.ExportarReporteExcelAsync(request.Filtros, request.TipoReporte);
            
            var fileName = $"VariablesControl_{request.TipoReporte}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando reporte a Excel");
            return Json(new { success = false, message = "Error al exportar el reporte" });
        }
    }
}

public class ExportarVariablesControlRequest
{
    public VariablesControlFiltrosDto Filtros { get; set; } = new();
    public string TipoReporte { get; set; } = "detallado";
}
