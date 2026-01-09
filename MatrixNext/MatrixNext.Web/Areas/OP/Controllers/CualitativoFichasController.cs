using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Services.OP.Models;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controller para gestión de fichas técnicas cualitativos
/// Ref: ANALISIS_OP_CUALITATIVO_FASE4_FLUJOS2Y3.md § 3.3
/// WebForm original: FichaEntrevista.aspx.vb (353 LOC), FichaSesion.aspx, FichaObservacion.aspx
/// Tareas: OP-F03
/// </summary>
[Area("OP")]
[Authorize]
[Route("OP/Cualitativo/Fichas")]
public class CualitativoFichasController : Controller
{
    private readonly IOpFichasTecnicasService _fichasService;
    private readonly ILogger<CualitativoFichasController> _logger;

    public CualitativoFichasController(
        IOpFichasTecnicasService fichasService,
        ILogger<CualitativoFichasController> logger)
    {
        _fichasService = fichasService;
        _logger = logger;
    }

    /// <summary>
    /// PASO 3.1: Editar ficha de entrevista
    /// Ref: FichaEntrevista.aspx.vb líneas 41-123 (Page_Load, cargarDatos)
    /// </summary>
    [HttpGet("EditInterview")]
    public async Task<IActionResult> EditInterview(long trabajoId)
    {
        try
        {
            var (success, data, error) = await _fichasService.ObtenerFichaEntrevistaAsync(trabajoId);

            if (!success)
            {
                TempData["Error"] = error;
                return RedirectToAction("Index", "CualitativoTrabajos");
            }

            return View(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando ficha entrevista trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error cargando ficha de entrevista";
            return RedirectToAction("Index", "CualitativoTrabajos");
        }
    }

    /// <summary>
    /// PASO 3.2-3.3: Guardar ficha de entrevista con validaciones
    /// Ref: FichaEntrevista.aspx.vb líneas 125-214 (btnGuardar_Click)
    /// 8 validaciones documentadas en FASE4 § 3.3 PASO 3.2
    /// </summary>
    [HttpPost("SaveInterview")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveInterview(FichaTecnicaVm ficha)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("EditInterview", ficha);
            }

            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (success, error) = await _fichasService.GuardarFichaEntrevistaAsync(ficha, usuarioId);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, error);
                return View("EditInterview", ficha);
            }

            TempData["Success"] = "Ficha guardada exitosamente";
            return RedirectToAction("EditInterview", new { trabajoId = ficha.TrabajoId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error guardando ficha entrevista trabajo {TrabajoId}", ficha.TrabajoId);
            ModelState.AddModelError(string.Empty, "Error guardando ficha");
            return View("EditInterview", ficha);
        }
    }

    /// <summary>
    /// PASO 3.4-3.5: Entregar ficha y enviar correo
    /// Ref: FichaEntrevista.aspx.vb líneas 216-267 (btnEntregar_Click)
    /// </summary>
    [HttpPost("SubmitInterview")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitInterview(long trabajoId)
    {
        try
        {
            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (success, error) = await _fichasService.EntregarFichaEntrevistaAsync(trabajoId, usuarioId);

            if (!success)
            {
                return Json(new { success = false, message = error });
            }

            return Json(new { success = true, message = "Ficha entregada exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error entregando ficha entrevista trabajo {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error entregando ficha" });
        }
    }

    /// <summary>
    /// Editar ficha de sesión
    /// Similar a EditInterview, tipo = 2
    /// </summary>
    [HttpGet("EditSession")]
    public async Task<IActionResult> EditSession(long trabajoId)
    {
        try
        {
            var (success, data, error) = await _fichasService.ObtenerFichaSesionAsync(trabajoId);

            if (!success)
            {
                TempData["Error"] = error;
                return RedirectToAction("Index", "CualitativoTrabajos");
            }

            return View("EditInterview", data); // Reutilizar misma vista
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando ficha sesión trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error cargando ficha de sesión";
            return RedirectToAction("Index", "CualitativoTrabajos");
        }
    }

    [HttpPost("SaveSession")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveSession(FichaTecnicaVm ficha)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("EditInterview", ficha);
            }

            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (success, error) = await _fichasService.GuardarFichaSesionAsync(ficha, usuarioId);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, error);
                return View("EditInterview", ficha);
            }

            TempData["Success"] = "Ficha de sesión guardada exitosamente";
            return RedirectToAction("EditSession", new { trabajoId = ficha.TrabajoId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error guardando ficha sesión trabajo {TrabajoId}", ficha.TrabajoId);
            ModelState.AddModelError(string.Empty, "Error guardando ficha");
            return View("EditInterview", ficha);
        }
    }

    /// <summary>
    /// Editar ficha de observación
    /// Similar a EditInterview, tipo = 3
    /// </summary>
    [HttpGet("EditObservation")]
    public async Task<IActionResult> EditObservation(long trabajoId)
    {
        try
        {
            var (success, data, error) = await _fichasService.ObtenerFichaObservacionAsync(trabajoId);

            if (!success)
            {
                TempData["Error"] = error;
                return RedirectToAction("Index", "CualitativoTrabajos");
            }

            return View("EditInterview", data); // Reutilizar misma vista
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando ficha observación trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error cargando ficha de observación";
            return RedirectToAction("Index", "CualitativoTrabajos");
        }
    }

    [HttpPost("SaveObservation")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveObservation(FichaTecnicaVm ficha)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("EditInterview", ficha);
            }

            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (success, error) = await _fichasService.GuardarFichaObservacionAsync(ficha, usuarioId);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, error);
                return View("EditInterview", ficha);
            }

            TempData["Success"] = "Ficha de observación guardada exitosamente";
            return RedirectToAction("EditObservation", new { trabajoId = ficha.TrabajoId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error guardando ficha observación trabajo {TrabajoId}", ficha.TrabajoId);
            ModelState.AddModelError(string.Empty, "Error guardando ficha");
            return View("EditInterview", ficha);
        }
    }

    /// <summary>
    /// Validar presupuesto disponible (AJAX)
    /// Ref: FichaEntrevista.aspx.vb líneas 269-305 (ValidarPresupuesto)
    /// </summary>
    [HttpGet("ValidateBudget")]
    public async Task<IActionResult> ValidateBudget(long trabajoId, decimal monto)
    {
        try
        {
            var (success, disponible, error) = await _fichasService.ValidarPresupuestoIncentivosAsync(
                trabajoId, monto);

            if (!success)
            {
                return Json(new { success = false, message = error });
            }

            var esValido = monto <= disponible;
            return Json(new
            {
                success = true,
                isValid = esValido,
                disponible,
                message = esValido
                    ? "Presupuesto disponible"
                    : $"Monto excede disponible. Disponible: {disponible:C}"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando presupuesto trabajo {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error validando presupuesto" });
        }
    }

    /// <summary>
    /// Actualizar estado de Habeas Data
    /// Ref: FichaEntrevista.aspx.vb líneas 307-332 (btnActualizarHabeasData_Click)
    /// </summary>
    [HttpPost("UpdateHabeasData")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateHabeasData(long trabajoId, bool firmado)
    {
        try
        {
            var (success, error) = await _fichasService.ActualizarHabeasDataAsync(trabajoId, firmado);

            if (!success)
            {
                return Json(new { success = false, message = error });
            }

            return Json(new { success = true, message = "Habeas Data actualizado" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando Habeas Data trabajo {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error actualizando Habeas Data" });
        }
    }
}
