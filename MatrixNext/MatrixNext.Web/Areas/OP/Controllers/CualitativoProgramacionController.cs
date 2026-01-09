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
    private readonly ILogger<CualitativoProgramacionController> _logger;

    public CualitativoProgramacionController(
        IOpProgramacionService programacionService,
        ILogger<CualitativoProgramacionController> logger)
    {
        _programacionService = programacionService;
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

            return Json(new { success = true, message = "Estado actualizado exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cambiando estado programación {Id}", id);
            return Json(new { success = false, message = "Error cambiando estado" });
        }
    }

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
}
