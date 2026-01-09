using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Services.Shared;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controller para Campo Cualitativo: captura de sesiones/entrevistas/observaciones y exportaciones
/// Ref: CampoCualitativo.aspx.vb (346 LOC)
/// Tarea: OP-C02
/// </summary>
[Area("OP")]
[Authorize]
[Route("OP/Cualitativo/Campo")]
public class CualitativoCampoController : Controller
{
    private readonly IOpIFieldService _iFieldService;
    private readonly IExportService _exportService;
    private readonly ILogger<CualitativoCampoController> _logger;

    public CualitativoCampoController(
        IOpIFieldService iFieldService,
        IExportService exportService,
        ILogger<CualitativoCampoController> logger)
    {
        _iFieldService = iFieldService;
        _exportService = exportService;
        _logger = logger;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(int trabajoId)
    {
        // TODO: Cargar sesiones/entrevistas/observaciones del trabajo
        // Placeholder: retornar vista básica con tabs
        ViewBag.TrabajoId = trabajoId;
        return View();
    }

    /// <summary>
    /// Exportación Excel de captura de campo
    /// </summary>
    [HttpGet("ExportExcel")]
    public async Task<IActionResult> ExportExcel(int trabajoId)
    {
        try
        {
            // TODO: Reemplazar con datos reales
            var rows = new[] { new { Sesion = "S1", Entrevistas = 5, Observaciones = 2 } };
            var bytes = await _exportService.ExportarExcelAsync(rows.ToList(), $"Campo_{trabajoId}_{DateTime.Now:yyyyMMdd}");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Campo_{trabajoId}.xlsx");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando Excel campo trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error exportando Excel";
            return RedirectToAction("Index", new { trabajoId });
        }
    }

    /// <summary>
    /// Exportación ICS (calendario) para sesiones
    /// </summary>
    [HttpGet("ExportIcs")]
    public IActionResult ExportIcs(int trabajoId)
    {
        try
        {
            // ICS básico; TODO cargar sesiones reales
            var ics = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nPRODID:-//MatrixNext//OP_Cualitativo//EN\r\n" +
                      "BEGIN:VEVENT\r\nUID:" + Guid.NewGuid() + "\r\nDTSTAMP:" + DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ") + "\r\n" +
                      "DTSTART:" + DateTime.UtcNow.AddDays(1).ToString("yyyyMMddTHHmmssZ") + "\r\n" +
                      "DTEND:" + DateTime.UtcNow.AddDays(1).AddHours(2).ToString("yyyyMMddTHHmmssZ") + "\r\n" +
                      "SUMMARY:Sesion Cualitativa Trabajo " + trabajoId + "\r\n" +
                      "END:VEVENT\r\nEND:VCALENDAR\r\n";
            var bytes = System.Text.Encoding.UTF8.GetBytes(ics);
            return File(bytes, "text/calendar", $"Campo_{trabajoId}.ics");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando ICS campo trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error exportando ICS";
            return RedirectToAction("Index", new { trabajoId });
        }
    }
}
