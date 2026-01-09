using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Services.OP.Models;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controller para gestión de filtros de reclutamiento y asistencia
/// Ref: ANALISIS_OP_CUALITATIVO_FASE4_FLUJOS2Y3.md § 3.2
/// WebForm original: DisenarFiltros.aspx.vb (1,062 LOC), AprobacionesFiltros.aspx.vb (270 LOC)
/// Tareas: OP-F01, OP-F02
/// </summary>
[Area("OP")]
[Authorize]
[Route("OP/Cualitativo/Filtros")]
public class CualitativoFiltrosController : Controller
{
    private readonly IOpFiltrosService _filtrosService;
    private readonly ILogger<CualitativoFiltrosController> _logger;

    public CualitativoFiltrosController(
        IOpFiltrosService filtrosService,
        ILogger<CualitativoFiltrosController> logger)
    {
        _filtrosService = filtrosService;
        _logger = logger;
    }

    /// <summary>
    /// PASO 2.1-2.2: Configurar filtro (diseño de preguntas dinámicas)
    /// Ref: DisenarFiltros.aspx.vb líneas 45-89 (cargarPreguntasFiltro)
    /// </summary>
    [HttpGet("Configure")]
    public async Task<IActionResult> Configure(long trabajoId, int tipo)
    {
        try
        {
            var (success, data, error) = await _filtrosService.ObtenerConfiguracionFiltroAsync(trabajoId, tipo);

            if (!success)
            {
                TempData["Error"] = error;
                return RedirectToAction("Index", "CualitativoTrabajos");
            }

            ViewBag.TipoFiltro = tipo == 1 ? "Reclutamiento" : "Asistencia";
            return View(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando configuración filtro trabajo {TrabajoId}, tipo {Tipo}", 
                trabajoId, tipo);
            TempData["Error"] = "Error cargando configuración de filtro";
            return RedirectToAction("Index", "CualitativoTrabajos");
        }
    }

    /// <summary>
    /// PASO 2.3: Agregar pregunta a filtro
    /// Ref: DisenarFiltros.aspx.vb líneas 321-459 (btnAgregarPregunta_Click)
    /// Generación dinámica según tipoPregunta
    /// </summary>
    [HttpPost("AddQuestion")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddQuestion([FromBody] PreguntaFiltroVm pregunta)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (success, preguntaId, error) = await _filtrosService.AgregarPreguntaFiltroAsync(
                pregunta.TrabajoId, pregunta.TipoFiltro, pregunta, usuarioId);

            if (!success)
            {
                return Json(new { success = false, message = error });
            }

            return Json(new { success = true, preguntaId, message = "Pregunta agregada exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error agregando pregunta filtro");
            return Json(new { success = false, message = "Error agregando pregunta" });
        }
    }

    /// <summary>
    /// Actualizar pregunta existente
    /// Ref: DisenarFiltros.aspx.vb líneas 461-491 (btnActualizar_Click)
    /// </summary>
    [HttpPost("UpdateQuestion")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuestion([FromBody] PreguntaFiltroVm pregunta)
    {
        try
        {
            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (success, error) = await _filtrosService.ActualizarPreguntaFiltroAsync(
                pregunta.Id, pregunta, usuarioId);

            if (!success)
            {
                return Json(new { success = false, message = error });
            }

            return Json(new { success = true, message = "Pregunta actualizada exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando pregunta {PreguntaId}", pregunta.Id);
            return Json(new { success = false, message = "Error actualizando pregunta" });
        }
    }

    /// <summary>
    /// Eliminar pregunta
    /// Ref: DisenarFiltros.aspx.vb líneas 493-517 (btnEliminar_Click)
    /// </summary>
    [HttpPost("DeleteQuestion")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteQuestion(long preguntaId)
    {
        try
        {
            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (success, error) = await _filtrosService.EliminarPreguntaFiltroAsync(
                preguntaId, usuarioId);

            if (!success)
            {
                return Json(new { success = false, message = error });
            }

            return Json(new { success = true, message = "Pregunta eliminada exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando pregunta {PreguntaId}", preguntaId);
            return Json(new { success = false, message = "Error eliminando pregunta" });
        }
    }

    /// <summary>
    /// PASO 2.4: Generar link de visualización
    /// Ref: DisenarFiltros.aspx.vb líneas 519-546 (GenerarLink)
    /// </summary>
    [HttpGet("GenerateLink")]
    public async Task<IActionResult> GenerateLink(long trabajoId, int tipo)
    {
        try
        {
            var (success, link, error) = await _filtrosService.GenerarLinkVisualizacionAsync(trabajoId, tipo);

            if (!success)
            {
                return Json(new { success = false, message = error });
            }

            return Json(new { success = true, link });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generando link visualización trabajo {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error generando link" });
        }
    }

    /// <summary>
    /// PASO 2.5-2.6: Aprobación de respuestas de filtro
    /// Ref: AprobacionesFiltros.aspx.vb líneas 28-91 (Page_Load, cargarRespuestas)
    /// </summary>
    [HttpGet("Approve")]
    public async Task<IActionResult> Approve(long trabajoId, int tipo, string estado = null)
    {
        try
        {
            var (success, data, error) = await _filtrosService.ObtenerRespuestasFiltroAsync(
                trabajoId, tipo, estado);

            if (!success)
            {
                TempData["Error"] = error;
                return RedirectToAction("Index", "CualitativoTrabajos");
            }

            ViewBag.TrabajoId = trabajoId;
            ViewBag.TipoFiltro = tipo;
            ViewBag.TipoFiltroNombre = tipo == 1 ? "Reclutamiento" : "Asistencia";

            return View(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando aprobaciones filtro trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error cargando aprobaciones";
            return RedirectToAction("Index", "CualitativoTrabajos");
        }
    }

    /// <summary>
    /// PASO 2.7: Aprobar respuestas seleccionadas
    /// Ref: AprobacionesFiltros.aspx.vb líneas 143-188 (btnAprobar_Click)
    /// Registra en OP_LogRespuestas_Filtro
    /// </summary>
    [HttpPost("ApproveResponses")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveResponses([FromBody] AprobacionRequestVm request)
    {
        try
        {
            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (success, error) = await _filtrosService.AprobarRespuestasFiltroAsync(
                request.RespuestasIds, usuarioId, request.Observaciones);

            if (!success)
            {
                return Json(new { success = false, message = error });
            }

            return Json(new { success = true, message = "Respuestas aprobadas exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error aprobando respuestas");
            return Json(new { success = false, message = "Error aprobando respuestas" });
        }
    }

    /// <summary>
    /// Rechazar respuestas seleccionadas
    /// Ref: AprobacionesFiltros.aspx.vb líneas 190-235 (btnRechazar_Click)
    /// </summary>
    [HttpPost("RejectResponses")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectResponses([FromBody] AprobacionRequestVm request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Observaciones))
            {
                return Json(new { success = false, message = "Observaciones requeridas para rechazo" });
            }

            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (success, error) = await _filtrosService.RechazarRespuestasFiltroAsync(
                request.RespuestasIds, usuarioId, request.Observaciones);

            if (!success)
            {
                return Json(new { success = false, message = error });
            }

            return Json(new { success = true, message = "Respuestas rechazadas exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rechazando respuestas");
            return Json(new { success = false, message = "Error rechazando respuestas" });
        }
    }

    /// <summary>
    /// Exportar respuestas a Excel
    /// Ref: AprobacionesFiltros.aspx.vb líneas 237-270 (btnExportarExcel_Click)
    /// SP: REP_OP_Respuestas_Filtro
    /// </summary>
    [HttpGet("ExportExcel")]
    public async Task<IActionResult> ExportExcel(long trabajoId, int tipo)
    {
        try
        {
            var (success, data, error) = await _filtrosService.ExportarRespuestasExcelAsync(trabajoId, tipo);

            if (!success)
            {
                TempData["Error"] = error;
                return RedirectToAction("Approve", new { trabajoId, tipo });
            }

            var fileName = $"Respuestas_Filtro_{trabajoId}_{tipo}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando respuestas Excel trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error exportando respuestas";
            return RedirectToAction("Approve", new { trabajoId, tipo });
        }
    }
}

/// <summary>
/// ViewModel para request de aprobación/rechazo
/// </summary>
public class AprobacionRequestVm
{
    public List<long> RespuestasIds { get; set; } = new();
    public string Observaciones { get; set; } = string.Empty;
}
