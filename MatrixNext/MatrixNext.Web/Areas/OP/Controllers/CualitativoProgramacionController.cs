using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Services.OP.Models;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controller para gestión de programación de campo cualitativo
/// Ref: ProgramacionCampo.aspx.vb (822 LOC)
/// Tareas: OP-P01
/// </summary>
[Area("OP")]
[Authorize]
[Route("OP/Cualitativo/Programacion")]
public class CualitativoProgramacionController : Controller
{
    private readonly IOpProgramacionService _programacionService;
    private readonly IOpNotificacionService _notificacionService;
    private readonly ILogger<CualitativoProgramacionController> _logger;

    public CualitativoProgramacionController(
        IOpProgramacionService programacionService,
        IOpNotificacionService notificacionService,
        ILogger<CualitativoProgramacionController> logger)
    {
        _programacionService = programacionService;
        _notificacionService = notificacionService;
        _logger = logger;
    }

    /// <summary>
    /// Index - Lista de programaciones por trabajo
    /// Ref: ProgramacionCampo.aspx.vb líneas 45-89 (Page_Load)
    /// </summary>
    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(long trabajoId, string? estado = null)
    {
        try
        {
            var (success, data, error) = await _programacionService.ObtenerProgramacionesPorTrabajoAsync(
                trabajoId, estado);

            if (!success)
            {
                TempData["Error"] = error;
                return View(new List<ProgramacionCampoVm>());
            }

            ViewBag.TrabajoId = trabajoId;
            ViewBag.EstadoFiltro = estado;

            return View(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando programaciones trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error cargando programaciones";
            return View(new List<ProgramacionCampoVm>());
        }
    }

    /// <summary>
    /// Crear/Editar programación
    /// Ref: ProgramacionCampo.aspx.vb líneas 125-214 (btnSaveProgramar_Click)
    /// </summary>
    [HttpGet("Edit")]
    public async Task<IActionResult> Edit(long trabajoId, long? id = null)
    {
        try
        {
            ProgramacionCampoVm programacion;

            if (id.HasValue && id.Value > 0)
            {
                // Cargar existente
                var (success, data, error) = await _programacionService.ObtenerProgramacionesPorTrabajoAsync(
                    trabajoId, null);

                if (!success)
                {
                    TempData["Error"] = error;
                    return RedirectToAction("Index", new { trabajoId });
                }

                programacion = data.FirstOrDefault(p => p.Id == id.Value) 
                    ?? new ProgramacionCampoVm { TrabajoId = trabajoId };
            }
            else
            {
                // Nueva programación
                programacion = new ProgramacionCampoVm { TrabajoId = trabajoId };
            }

            // Cargar entrevistados disponibles
            var (successEnt, entrevistados, errorEnt) = await _programacionService.ObtenerEntrevistadosDisponiblesAsync(trabajoId);
            ViewBag.EntrevistadosDisponibles = successEnt ? entrevistados : new List<EntrevistadoDisponibleVm>();

            return View(programacion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando formulario programación");
            TempData["Error"] = "Error cargando formulario";
            return RedirectToAction("Index", new { trabajoId });
        }
    }

    /// <summary>
    /// Guardar programación
    /// Ref: ProgramacionCampo.aspx.vb líneas 125-214 (btnSaveProgramar_Click)
    /// </summary>
    [HttpPost("Save")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(ProgramacionCampoVm programacion)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", programacion);
            }

            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (success, programacionId, error) = await _programacionService.GuardarProgramacionAsync(
                programacion, usuarioId);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, error);
                return View("Edit", programacion);
            }

            // Notificar programación creada (fire and forget, no bloquea respuesta)
            if (programacion.Id == 0) // Solo si es nueva
            {
                _ = _notificacionService.NotificarProgramacionCreadaAsync(programacionId)
                    .ConfigureAwait(false);
            }

            TempData["Success"] = "Programación guardada exitosamente";
            return RedirectToAction("Index", new { trabajoId = programacion.TrabajoId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error guardando programación");
            ModelState.AddModelError(string.Empty, "Error guardando programación");
            return View("Edit", programacion);
        }
    }

    /// <summary>
    /// Cambiar estado de programación (AJAX)
    /// Ref: ProgramacionCampo.aspx.vb líneas 320-365 (CambiarEstado)
    /// </summary>
    [HttpPost("ChangeStatus")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(long id, int estado, string? observaciones)
    {
        try
        {
            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (success, error) = await _programacionService.CambiarEstadoProgramacionAsync(
                id, estado, usuarioId, observaciones);

            if (!success)
            {
                return Json(new { success = false, message = error });
            }

            // Notificar cambio de estado (fire and forget)
            var estadoNuevo = MapearEstadoNumeroATexto(estado);
            _ = _notificacionService.NotificarCambioEstadoProgramacionAsync(id, "Anterior", estadoNuevo)
                .ConfigureAwait(false);

            return Json(new { success = true, message = "Estado actualizado exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cambiando estado programación {Id}", id);
            return Json(new { success = false, message = "Error cambiando estado" });
        }
    }

    /// <summary>
    /// Mapear número de estado a texto descriptivo
    /// </summary>
    private string MapearEstadoNumeroATexto(int estado) => estado switch
    {
        1 => "Confirmado",
        2 => "Cancelado",
        3 => "Reprogramado",
        4 => "Completado",
        5 => "No presentado",
        _ => "Pendiente"
    };

    /// <summary>
    /// Exportar programaciones a Excel
    /// Ref: ProgramacionCampo.aspx.vb líneas 520-618 (ExportarExcel)
    /// </summary>
    [HttpGet("ExportExcel")]
    public async Task<IActionResult> ExportExcel(long trabajoId, string? estado = null)
    {
        try
        {
            var (success, data, error) = await _programacionService.ExportarProgramacionesExcelAsync(
                trabajoId, estado);

            if (!success)
            {
                TempData["Error"] = error;
                return RedirectToAction("Index", new { trabajoId });
            }

            var fileName = $"Programaciones_Trabajo_{trabajoId}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando programaciones trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error exportando programaciones";
            return RedirectToAction("Index", new { trabajoId });
        }
    }

    /// <summary>
    /// Obtener entrevistados disponibles (AJAX para modal)
    /// Ref: ProgramacionCampo.aspx.vb líneas 220-287 (CargarEntrevistados)
    /// </summary>
    [HttpGet("GetEntrevistadosDisponibles")]
    public async Task<IActionResult> GetEntrevistadosDisponibles(long trabajoId)
    {
        try
        {
            var (success, data, error) = await _programacionService.ObtenerEntrevistadosDisponiblesAsync(trabajoId);

            if (!success)
            {
                return Json(new { success = false, message = error });
            }

            return Json(new { success = true, data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo entrevistados disponibles trabajo {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error obteniendo entrevistados" });
        }
    }

    /// <summary>
    /// Validar participantes seleccionados (AJAX)
    /// Ref: Sprint 4 - Validación Participantes
    /// </summary>
    [HttpPost("ValidateParticipants")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ValidateParticipants(long trabajoId, [FromBody] ValidateParticipantsRequest req)
    {
        try
        {
            if (req == null || req.Ids == null || !req.Ids.Any())
                return Json(new { success = false, message = "Debe seleccionar al menos un participante" });

            var (success, data, error) = await _programacionService.ValidarParticipantesAsync(
                trabajoId, req.Ids, req.FechaProgramada);

            if (!success)
                return Json(new { success = false, message = error });

            return Json(new { success = true, data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando participantes trabajo {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error validando participantes" });
        }
    }

    public class ValidateParticipantsRequest
    {
        public List<long> Ids { get; set; } = new();
        public DateTime? FechaProgramada { get; set; }
    }

    /// <summary>
    /// Exportar programaciones a archivo ICS (iCalendar)
    /// Ref: CampoCualitativo.aspx.vb (Crear_Archivo_ICS)
    /// </summary>
    [HttpGet("ExportIcs")]
    public async Task<IActionResult> ExportIcs(long trabajoId, string? estado = null)
    {
        try
        {
            var (success, programaciones, error) = await _programacionService.ObtenerProgramacionesPorTrabajoAsync(trabajoId, estado);

            if (!success || programaciones == null || !programaciones.Any())
            {
                TempData["Warning"] = "No hay sesiones programadas para exportar";
                return RedirectToAction("Index", new { trabajoId, estado });
            }

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("PRODID:-//MatrixNext//OP_Programacion//EN");
            sb.AppendLine("CALSCALE:GREGORIAN");

            foreach (var prog in programaciones.Where(p => p.FechaProgramada.HasValue))
            {
                var fechaInicio = prog.FechaProgramada!.Value;
                var duracion = prog.DuracionEstimada.HasValue && prog.DuracionEstimada.Value > 0
                    ? TimeSpan.FromMinutes(prog.DuracionEstimada.Value)
                    : TimeSpan.FromHours(2);
                var fechaFin = fechaInicio.Add(duracion);

                sb.AppendLine("BEGIN:VEVENT");
                sb.AppendLine($"UID:{Guid.NewGuid()}");
                sb.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMdd\\THHmmss\\Z}");
                sb.AppendLine($"DTSTART:{fechaInicio.ToUniversalTime():yyyyMMdd\\THHmmss\\Z}");
                sb.AppendLine($"DTEND:{fechaFin.ToUniversalTime():yyyyMMdd\\THHmmss\\Z}");
                var resumen = string.IsNullOrEmpty(prog.NombreEntrevistado) ? prog.EntrevistadoNombre : prog.NombreEntrevistado;
                sb.AppendLine($"SUMMARY:Programación - {resumen}");
                var estadoDesc = string.IsNullOrEmpty(prog.NombreEstado) ? prog.EstadoDescripcion : prog.NombreEstado;
                sb.AppendLine($"DESCRIPTION:Trabajo {trabajoId} - Estado: {estadoDesc}");
                if (!string.IsNullOrEmpty(prog.DireccionCita))
                    sb.AppendLine($"LOCATION:{prog.DireccionCita}");
                sb.AppendLine("CLASS:PUBLIC");
                sb.AppendLine("STATUS:CONFIRMED");
                sb.AppendLine("END:VEVENT");
            }

            sb.AppendLine("END:VCALENDAR");

            var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            var fileName = $"Programacion_Trabajo_{trabajoId}_{DateTime.Now:yyyyMMdd}.ics";
            return File(bytes, "text/calendar", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando ICS programaciones trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error generando archivo de calendario";
            return RedirectToAction("Index", new { trabajoId, estado });
        }
    }

    /// <summary>
    /// Vista de calendario simple de programaciones
    /// </summary>
    [HttpGet("Calendar")]
    public async Task<IActionResult> Calendar(long trabajoId, string? estado = null)
    {
        try
        {
            var (success, data, error) = await _programacionService.ObtenerProgramacionesPorTrabajoAsync(trabajoId, estado);
            if (!success)
            {
                TempData["Error"] = error;
                return RedirectToAction("Index", new { trabajoId, estado });
            }

            ViewBag.TrabajoId = trabajoId;
            ViewBag.EstadoFiltro = estado;
            return View(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando calendario trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error cargando calendario";
            return RedirectToAction("Index", new { trabajoId, estado });
        }
    }
}
