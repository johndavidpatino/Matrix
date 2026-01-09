using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Services.OP.Models;
using System.Text;

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
    private readonly IOpProgramacionService _programacionService;
    private readonly ILogger<CualitativoCampoController> _logger;

    public CualitativoCampoController(
        IOpProgramacionService programacionService,
        ILogger<CualitativoCampoController> logger)
    {
        _programacionService = programacionService;
        _logger = logger;
    }

    /// <summary>
    /// Index - Listado de programaciones de campo del trabajo
    /// Ref: CampoCualitativo.aspx.vb (placeholder básico)
    /// </summary>
    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(long trabajoId)
    {
        try
        {
            var (success, programaciones, error) = await _programacionService.ObtenerProgramacionesPorTrabajoAsync(trabajoId);

            if (!success)
            {
                TempData["Error"] = error;
                return View(new List<ProgramacionCampoVm>());
            }

            ViewBag.TrabajoId = trabajoId;
            return View(programaciones);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando campo trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error cargando sesiones de campo";
            ViewBag.TrabajoId = trabajoId;
            return View(new List<ProgramacionCampoVm>());
        }
    }

    /// <summary>
    /// Exportar programaciones de campo a Excel
    /// Ref: CampoCualitativo.aspx.vb líneas 302-336 (btnExportar_Click)
    /// Reutiliza OpProgramacionService.ExportarProgramacionesExcelAsync
    /// </summary>
    [HttpGet("ExportExcel")]
    public async Task<IActionResult> ExportExcel(long trabajoId)
    {
        try
        {
            var (success, data, error) = await _programacionService.ExportarProgramacionesExcelAsync(trabajoId);

            if (!success)
            {
                _logger.LogWarning("Error exportando Excel campo trabajo {TrabajoId}: {Error}", trabajoId, error);
                TempData["Error"] = error;
                return RedirectToAction("Index", new { trabajoId });
            }

            var fileName = $"Campo_Trabajo_{trabajoId}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando Excel campo trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error exportando datos a Excel";
            return RedirectToAction("Index", new { trabajoId });
        }
    }

    /// <summary>
    /// Exportar sesiones programadas a archivo ICS (iCalendar)
    /// Ref: CampoCualitativo.aspx.vb líneas 223-256 (Crear_Archivo_ICS + imbDescargarCita_Click)
    /// Genera eventos de calendario para importar en Outlook/Google Calendar
    /// </summary>
    [HttpGet("ExportIcs")]
    public async Task<IActionResult> ExportIcs(long trabajoId)
    {
        try
        {
            var (success, programaciones, error) = await _programacionService.ObtenerProgramacionesPorTrabajoAsync(trabajoId);

            if (!success || programaciones == null || !programaciones.Any())
            {
                _logger.LogWarning("No hay programaciones para exportar trabajo {TrabajoId}", trabajoId);
                TempData["Warning"] = "No hay sesiones programadas para exportar";
                return RedirectToAction("Index", new { trabajoId });
            }

            var sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("PRODID:-//MatrixNext//OP_Cualitativo//EN");
            sb.AppendLine("CALSCALE:GREGORIAN");

            foreach (var prog in programaciones.Where(p => p.FechaProgramada.HasValue))
            {
                var fechaInicio = prog.FechaProgramada!.Value;
                var fechaFin = fechaInicio.AddHours(2); // Duración estimada 2 horas

                sb.AppendLine("BEGIN:VEVENT");
                sb.AppendLine($"UID:{Guid.NewGuid()}");
                sb.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMdd\\THHmmss\\Z}");
                sb.AppendLine($"DTSTART:{fechaInicio.ToUniversalTime():yyyyMMdd\\THHmmss\\Z}");
                sb.AppendLine($"DTEND:{fechaFin.ToUniversalTime():yyyyMMdd\\THHmmss\\Z}");
                sb.AppendLine($"SUMMARY:Sesión Campo - {prog.EntrevistadoNombre}");
                sb.AppendLine($"DESCRIPTION:Trabajo ID: {trabajoId} - Estado: {prog.EstadoDescripcion}");
                
                if (!string.IsNullOrEmpty(prog.DireccionCita))
                    sb.AppendLine($"LOCATION:{prog.DireccionCita}");
                
                sb.AppendLine("CLASS:PUBLIC");
                sb.AppendLine("STATUS:CONFIRMED");
                sb.AppendLine("END:VEVENT");
            }

            sb.AppendLine("END:VCALENDAR");

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            var fileName = $"Campo_Trabajo_{trabajoId}_{DateTime.Now:yyyyMMdd}.ics";
            
            return File(bytes, "text/calendar", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando ICS campo trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error generando archivo de calendario";
            return RedirectToAction("Index", new { trabajoId });
        }
    }
}
