using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Services.OP.Models;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controller para administración de planillas de moderación e informes cualitativos
/// Ref: AdministracionRegistroPlanillas.aspx + RegistroPlanillasCualitativo.aspx
/// WebForm JS: AdministracionRegistroPlanillas.js (~500 LOC)
/// Service JS: RegistroPlanillasCualitativoService.js
/// </summary>
[Area("OP")]
[Authorize]
[Route("OP/Cualitativo/Planillas")]
public class CualitativoPlanillasController : Controller
{
    private readonly IOpPlanillasModeracionService _planillasService;
    private readonly ILogger<CualitativoPlanillasController> _logger;

    public CualitativoPlanillasController(
        IOpPlanillasModeracionService planillasService,
        ILogger<CualitativoPlanillasController> logger)
    {
        _planillasService = planillasService;
        _logger = logger;
    }

    /// <summary>
    /// Index: Grid de planillas con filtros
    /// Ref: AdministracionRegistroPlanillas.aspx líneas 61-104
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(
        string? tipoPlantilla = null,
        short? statusRegistro = null,
        int pageIndex = 0,
        int pageSize = 25)
    {
        try
        {
            var (success, data, totalRecords, error) = await _planillasService.ObtenerPlanillasAsync(
                tipoPlantilla,
                statusRegistro,
                pageIndex,
                pageSize);

            if (!success)
            {
                TempData["Error"] = error;
                return View(new List<PlanillaListItemVm>());
            }

            ViewBag.TipoPlantilla = tipoPlantilla;
            ViewBag.StatusRegistro = statusRegistro;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            return View(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar planillas. TipoPlantilla: {TipoPlantilla}, Status: {Status}",
                tipoPlantilla, statusRegistro);
            TempData["Error"] = "Error al cargar planillas";
            return View(new List<PlanillaListItemVm>());
        }
    }

    /// <summary>
    /// EditModeracion: Cargar form para planilla de moderación (create/edit)
    /// Ref: RegistroPlanillasCualitativo.aspx WebMethod GetPlanillaModeracionById
    /// </summary>
    [HttpGet("EditModeracion")]
    public async Task<IActionResult> EditModeracion(long? id)
    {
        try
        {
            // Cargar listas dropdown
            var tecnicas = await _planillasService.ObtenerTecnicasAsync("Moderacion");
            var moderadores = await _planillasService.ObtenerModeradoresAsync();

            ViewBag.Tecnicas = tecnicas;
            ViewBag.Moderadores = moderadores;

            if (id == null || id == 0) // CREATE
            {
                var model = new PlanillaModeracionVm
                {
                    FechaPlanilla = DateTime.Now,
                    IdEstadoAprobacion = 1 // En Espera
                };
                return View(model);
            }
            else // EDIT
            {
                var (success, data, error) = await _planillasService.ObtenerPlanillaModeracionAsync(id.Value);

                if (!success)
                {
                    TempData["Error"] = error;
                    return RedirectToAction(nameof(Index));
                }

                return View(data);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar planilla de moderación. ID: {Id}", id);
            TempData["Error"] = "Error al cargar planilla de moderación";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// SaveModeracion: Guardar planilla de moderación (INSERT o UPDATE)
    /// Ref: RegistroPlanillasCualitativo.aspx WebMethod SavePlanillaModeracion
    /// </summary>
    [HttpPost("SaveModeracion")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveModeracion(PlanillaModeracionVm model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                // Recargar dropdowns en caso de error
                ViewBag.Tecnicas = await _planillasService.ObtenerTecnicasAsync("Moderacion");
                ViewBag.Moderadores = await _planillasService.ObtenerModeradoresAsync();
                return View("EditModeracion", model);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var usuarioId))
            {
                TempData["Error"] = "Usuario no autenticado";
                return RedirectToAction(nameof(Index));
            }

            var (success, idPlanilla, error) = await _planillasService.GuardarPlanillaModeracionAsync(model, usuarioId);

            if (!success)
            {
                TempData["Error"] = error;
                ViewBag.Tecnicas = await _planillasService.ObtenerTecnicasAsync("Moderacion");
                ViewBag.Moderadores = await _planillasService.ObtenerModeradoresAsync();
                return View("EditModeracion", model);
            }

            TempData["Success"] = model.IdPlanilla == 0
                ? $"Planilla de moderación creada exitosamente (ID: {idPlanilla})"
                : "Planilla de moderación actualizada exitosamente";

            return RedirectToAction(nameof(Index), new { tipoPlantilla = "Moderacion" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar planilla de moderación. ID: {IdPlanilla}", model.IdPlanilla);
            TempData["Error"] = "Error al guardar planilla de moderación";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// EditInforme: Cargar form para planilla de informes (create/edit)
    /// Ref: RegistroPlanillasCualitativo.aspx WebMethod GetPlanillaInformesById
    /// </summary>
    [HttpGet("EditInforme")]
    public async Task<IActionResult> EditInforme(long? id)
    {
        try
        {
            if (id == null || id == 0) // CREATE
            {
                var model = new PlanillaInformeVm
                {
                    Fecha = DateTime.Now,
                    IdEstadoAprobacion = 1 // En Espera
                };
                return View(model);
            }
            else // EDIT
            {
                var (success, data, error) = await _planillasService.ObtenerPlanillaInformeAsync(id.Value);

                if (!success)
                {
                    TempData["Error"] = error;
                    return RedirectToAction(nameof(Index));
                }

                return View(data);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar planilla de informes. ID: {Id}", id);
            TempData["Error"] = "Error al cargar planilla de informes";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// SaveInforme: Guardar planilla de informes (INSERT o UPDATE)
    /// Ref: RegistroPlanillasCualitativo.aspx WebMethod SavePlanillaInformes
    /// </summary>
    [HttpPost("SaveInforme")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveInforme(PlanillaInformeVm model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("EditInforme", model);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var usuarioId))
            {
                TempData["Error"] = "Usuario no autenticado";
                return RedirectToAction(nameof(Index));
            }

            var (success, idPlanilla, error) = await _planillasService.GuardarPlanillaInformeAsync(model, usuarioId);

            if (!success)
            {
                TempData["Error"] = error;
                return View("EditInforme", model);
            }

            TempData["Success"] = model.IdPlanilla == 0
                ? $"Planilla de informes creada exitosamente (ID: {idPlanilla})"
                : "Planilla de informes actualizada exitosamente";

            return RedirectToAction(nameof(Index), new { tipoPlantilla = "Informes" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar planilla de informes. ID: {IdPlanilla}", model.IdPlanilla);
            TempData["Error"] = "Error al guardar planilla de informes";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// AprobarPlanilla: Aprobar planilla (AJAX)
    /// Ref: RegistroPlanillasCualitativo.aspx WebMethod SaveStatusAprobacionModeracion/Informes
    /// </summary>
    [HttpPost("AprobarPlanilla")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AprobarPlanilla(
        long idPlanilla,
        string tipoPlantilla,
        string? observaciones = null)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var usuarioId))
            {
                return Json(new { success = false, error = "Usuario no autenticado" });
            }

            var (success, error) = await _planillasService.AprobarPlanillaAsync(
                idPlanilla,
                tipoPlantilla,
                usuarioId,
                observaciones);

            if (!success)
            {
                return Json(new { success = false, error });
            }

            return Json(new { success = true, message = "Planilla aprobada exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al aprobar planilla. ID: {IdPlanilla}, Tipo: {TipoPlantilla}",
                idPlanilla, tipoPlantilla);
            return Json(new { success = false, error = "Error al aprobar planilla" });
        }
    }

    /// <summary>
    /// RechazarPlanilla: Rechazar planilla con observaciones requeridas (AJAX)
    /// Ref: RegistroPlanillasCualitativo.aspx WebMethod SaveStatusAprobacionModeracion/Informes
    /// </summary>
    [HttpPost("RechazarPlanilla")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RechazarPlanilla(
        long idPlanilla,
        string tipoPlantilla,
        string observaciones)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(observaciones))
            {
                return Json(new { success = false, error = "Observaciones son requeridas para rechazar" });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var usuarioId))
            {
                return Json(new { success = false, error = "Usuario no autenticado" });
            }

            var (success, error) = await _planillasService.RechazarPlanillaAsync(
                idPlanilla,
                tipoPlantilla,
                usuarioId,
                observaciones);

            if (!success)
            {
                return Json(new { success = false, error });
            }

            return Json(new { success = true, message = "Planilla rechazada exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al rechazar planilla. ID: {IdPlanilla}, Tipo: {TipoPlantilla}",
                idPlanilla, tipoPlantilla);
            return Json(new { success = false, error = "Error al rechazar planilla" });
        }
    }

    /// <summary>
    /// ExportExcel: Exportar planillas a Excel
    /// Ref: AdministracionRegistroPlanillas.js #ExportExcel
    /// </summary>
    [HttpGet("ExportExcel")]
    public async Task<IActionResult> ExportExcel(
        string? tipoPlantilla = null,
        short? statusRegistro = null)
    {
        try
        {
            var excelBytes = await _planillasService.ExportarPlanillasExcelAsync(tipoPlantilla, statusRegistro);

            if (excelBytes == null || excelBytes.Length == 0)
            {
                TempData["Warning"] = "No hay datos para exportar";
                return RedirectToAction(nameof(Index));
            }

            var nombreArchivo = $"Planillas_{tipoPlantilla ?? "Todas"}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al exportar planillas. Tipo: {TipoPlantilla}, Status: {Status}",
                tipoPlantilla, statusRegistro);
            TempData["Error"] = "Error al exportar planillas";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// BuscarJobBooks: Búsqueda de JobBooks por término (AJAX autocomplete)
    /// Ref: RegistroPlanillasCualitativo.aspx WebMethod GetJobsBy
    /// </summary>
    [HttpGet("BuscarJobBooks")]
    public async Task<IActionResult> BuscarJobBooks(string termino)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(termino) || termino.Length < 2)
            {
                return Json(new List<JobBookSearchVm>());
            }

            var results = await _planillasService.BuscarJobBooksAsync(termino);
            return Json(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar JobBooks. Término: {Termino}", termino);
            return Json(new List<JobBookSearchVm>());
        }
    }

    /// <summary>
    /// ObtenerModeradoresDisponibles: Dropdown de moderadores (AJAX)
    /// Ref: RegistroPlanillasCualitativo.aspx WebMethod GetModeradores
    /// </summary>
    [HttpGet("ObtenerModeradoresDisponibles")]
    public async Task<IActionResult> ObtenerModeradoresDisponibles()
    {
        try
        {
            var moderadores = await _planillasService.ObtenerModeradoresAsync();
            return Json(moderadores);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener moderadores disponibles");
            return Json(new List<ModeradorVm>());
        }
    }

    /// <summary>
    /// ObtenerTecnicas: Dropdown de técnicas cualitativas (AJAX)
    /// Ref: RegistroPlanillasCualitativo.aspx WebMethod GetTecnicas
    /// </summary>
    [HttpGet("ObtenerTecnicas")]
    public async Task<IActionResult> ObtenerTecnicas(string? tipoTecnica = null)
    {
        try
        {
            var tecnicas = await _planillasService.ObtenerTecnicasAsync(tipoTecnica);
            return Json(tecnicas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener técnicas. Tipo: {TipoTecnica}", tipoTecnica);
            return Json(new List<TecnicaVm>());
        }
    }
}
