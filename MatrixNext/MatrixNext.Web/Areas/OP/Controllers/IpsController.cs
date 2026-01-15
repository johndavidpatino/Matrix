using MatrixNext.Data.Models.OP;
using MatrixNext.Data.Services.OP;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MatrixNext.Web.Areas.OP.Controllers;

[Area("OP")]
[Authorize]
public class IpsController : Controller
{
    private readonly IOpIpsService _ipsService;
    private readonly IIpsRevisionService _revisionService;
    private readonly ILogger<IpsController> _logger;

    public IpsController(
        IOpIpsService ipsService,
        IIpsRevisionService revisionService,
        ILogger<IpsController> logger)
    {
        _ipsService = ipsService;
        _revisionService = revisionService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(long? trabajoId)
    {
        var model = await _ipsService.ObtenerPlanillasAsync(trabajoId);
        return View(model);
    }

    /// <summary>
    /// Detalle IPS por tarea con grid de revisiones
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> DetallesPorTarea(long trabajoId)
    {
        try
        {
            if (trabajoId <= 0)
            {
                TempData["Error"] = "ID de trabajo inválido";
                return RedirectToAction(nameof(Index));
            }

            var revisiones = await _revisionService.ObtenerRevisionesAsync(trabajoId);
            ViewBag.TrabajoId = trabajoId;

            return View(revisiones);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando detalles IPS para trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error al cargar los detalles de IPS";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Modal para crear nueva revisión IPS
    /// </summary>
    [HttpGet]
    public IActionResult CrearRevision(long trabajoId)
    {
        var dto = new IpsRevisionCreateUpdateDto
        {
            TrabajoId = trabajoId
        };

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("_CrearEditarRevision", dto);
        }

        return View(dto);
    }

    /// <summary>
    /// Crea una nueva revisión IPS
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CrearRevision(IpsRevisionCreateUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            return View(dto);
        }

        try
        {
            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            var (success, message, id) = await _revisionService.CrearRevisionAsync(dto, usuarioId);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success, message });
            }

            if (success)
            {
                TempData["Success"] = message;
                return RedirectToAction(nameof(DetallesPorTarea), new { trabajoId = dto.TrabajoId });
            }

            ModelState.AddModelError("", message);
            return View(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando revisión IPS. Trabajo: {TrabajoId}", dto.TrabajoId);

            var errorMessage = "Error al crear la revisión IPS. Por favor intente nuevamente.";

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = errorMessage });
            }

            TempData["Error"] = errorMessage;
            return RedirectToAction(nameof(DetallesPorTarea), new { trabajoId = dto.TrabajoId });
        }
    }

    /// <summary>
    /// Modal para editar revisión IPS
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> EditarRevision(long revisionId, long trabajoId)
    {
        try
        {
            var revision = await _revisionService.ObtenerRevisionAsync(revisionId);

            if (revision == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Revisión no encontrada" });
                }

                TempData["Error"] = "Revisión no encontrada";
                return RedirectToAction(nameof(DetallesPorTarea), new { trabajoId });
            }

            var dto = new IpsRevisionCreateUpdateDto
            {
                Id = revision.Id,
                TrabajoId = revision.TrabajoId,
                Pregunta = revision.Pregunta,
                Observacion = revision.Observacion,
                DescripcionObservacion = revision.DescripcionObservacion,
                RespuestaProgramador = revision.RespuestaProgramador,
                TipoTarea = revision.TipoTarea
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CrearEditarRevision", dto);
            }

            return View(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando revisión IPS {RevisionId}", revisionId);
            TempData["Error"] = "Error al cargar la revisión";
            return RedirectToAction(nameof(DetallesPorTarea), new { trabajoId });
        }
    }

    /// <summary>
    /// Actualiza una revisión IPS
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActualizarRevision(IpsRevisionCreateUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            return View(dto);
        }

        try
        {
            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            var (success, message) = await _revisionService.ActualizarRevisionAsync(dto, usuarioId);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success, message });
            }

            if (success)
            {
                TempData["Success"] = message;
                return RedirectToAction(nameof(DetallesPorTarea), new { trabajoId = dto.TrabajoId });
            }

            ModelState.AddModelError("", message);
            return View(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando revisión IPS. ID: {RevisionId}", dto.Id);

            var errorMessage = "Error al actualizar la revisión IPS. Por favor intente nuevamente.";

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = errorMessage });
            }

            TempData["Error"] = errorMessage;
            return RedirectToAction(nameof(DetallesPorTarea), new { trabajoId = dto.TrabajoId });
        }
    }

    /// <summary>
    /// Elimina una revisión IPS
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarRevision(long revisionId, long trabajoId)
    {
        try
        {
            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            var (success, message) = await _revisionService.EliminarRevisionAsync(revisionId, usuarioId);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success, message });
            }

            if (success)
            {
                TempData["Success"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction(nameof(DetallesPorTarea), new { trabajoId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando revisión IPS {RevisionId}", revisionId);

            var errorMessage = "Error al eliminar la revisión IPS. Por favor intente nuevamente.";

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = errorMessage });
            }

            TempData["Error"] = errorMessage;
            return RedirectToAction(nameof(DetallesPorTarea), new { trabajoId });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Guardar(IpsRevisionUpdateModel model)
    {
        var success = await _ipsService.GuardarRevisionAsync(model);
        TempData["IpsMessage"] = success ? "Revisión guardada" : "No se pudo guardar";
        return RedirectToAction(nameof(Index), new { trabajoId = model.TrabajoId });
    }

    [HttpGet]
    public async Task<IActionResult> Exportar(long? trabajoId)
    {
        var result = await _ipsService.ExportarRevisionesAsync(trabajoId);
        return PhysicalFile(result.PhysicalPath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(result.PhysicalPath));
    }
}
