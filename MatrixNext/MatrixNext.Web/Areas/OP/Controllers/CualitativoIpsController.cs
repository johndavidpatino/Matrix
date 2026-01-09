using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Services.OP.Models;
using MatrixNext.Web.ViewModels.OP;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controller para gestión de IPS (Instrumentos y Procesos) cualitativo
/// Ref: IPSCuali.aspx.vb (682 LOC)
/// Tareas: OP-I01
/// Usa servicio existente OpIpsService con SPs OP_IPS_Revision_Get/Edit
/// </summary>
[Area("OP")]
[Authorize]
[Route("OP/Cualitativo/Ips")]
public class CualitativoIpsController : Controller
{
    private readonly IOpIpsService _ipsService;
    private readonly ILogger<CualitativoIpsController> _logger;

    public CualitativoIpsController(
        IOpIpsService ipsService,
        ILogger<CualitativoIpsController> logger)
    {
        _ipsService = ipsService;
        _logger = logger;
    }

    /// <summary>
    /// Index - Lista de revisiones IPS por trabajo
    /// Ref: IPSCuali.aspx.vb líneas 38-125 (Page_Load, CargarRevisiones)
    /// SP: OP_IPS_Revision_Get
    /// </summary>
    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(long? trabajoId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var viewModel = await _ipsService.ObtenerRevisionesAsync(trabajoId, cancellationToken);

            ViewBag.TrabajoId = trabajoId;

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando revisiones IPS trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error cargando revisiones IPS";
            return View(new IpsRevisionViewModel { TrabajoId = trabajoId, Revisiones = new List<IpsRevisionRowViewModel>() });
        }
    }

    /// <summary>
    /// Guardar/actualizar revisión IPS
    /// Ref: IPSCuali.aspx.vb líneas 245-382 (btnNotificar_Click, btnRechazar_Click)
    /// SP: OP_IPS_Revision_Edit
    /// </summary>
    [HttpPost("Save")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save([FromBody] IpsRevisionUpdateModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            var success = await _ipsService.GuardarRevisionAsync(model, cancellationToken);

            if (!success)
            {
                return Json(new { success = false, message = "Error guardando revisión IPS" });
            }

            return Json(new { success = true, message = "Revisión IPS actualizada exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error guardando revisión IPS {Id}", model.Id);
            return Json(new { success = false, message = "Error guardando revisión IPS" });
        }
    }

    /// <summary>
    /// Notificar revisión (aprobar)
    /// Ref: IPSCuali.aspx.vb líneas 245-312 (btnNotificar_Click)
    /// </summary>
    [HttpPost("Notify")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Notify(long id, string observaciones, CancellationToken cancellationToken = default)
    {
        try
        {
            // Aquí iría la lógica de notificación/aprobación
            // Por ahora usamos el servicio existente para actualizar estado
            var model = new IpsRevisionUpdateModel
            {
                Id = id,
                Observacion = observaciones,
                Estado = "Aprobado",
                Rechazar = string.Empty
            };

            var success = await _ipsService.GuardarRevisionAsync(model, cancellationToken);

            if (!success)
            {
                return Json(new { success = false, message = "Error notificando revisión" });
            }

            return Json(new { success = true, message = "Revisión notificada exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error notificando revisión IPS {Id}", id);
            return Json(new { success = false, message = "Error notificando revisión" });
        }
    }

    /// <summary>
    /// Rechazar revisión
    /// Ref: IPSCuali.aspx.vb líneas 315-382 (btnRechazar_Click)
    /// </summary>
    [HttpPost("Reject")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(long id, string observaciones, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(observaciones))
            {
                return Json(new { success = false, message = "Observaciones requeridas para rechazo" });
            }

            var model = new IpsRevisionUpdateModel
            {
                Id = id,
                Observacion = observaciones,
                DescripcionObservacion = observaciones,
                Estado = "Rechazado",
                Rechazar = "S"
            };

            var success = await _ipsService.GuardarRevisionAsync(model, cancellationToken);

            if (!success)
            {
                return Json(new { success = false, message = "Error rechazando revisión" });
            }

            return Json(new { success = true, message = "Revisión rechazada exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rechazando revisión IPS {Id}", id);
            return Json(new { success = false, message = "Error rechazando revisión" });
        }
    }

    /// <summary>
    /// Exportar revisiones a Excel
    /// Ref: IPSCuali.aspx.vb líneas 470-558 (btnExportar_Click con ClosedXML)
    /// Usa servicio existente que genera archivo físico y devuelve path relativo
    /// </summary>
    [HttpGet("ExportExcel")]
    public async Task<IActionResult> ExportExcel(long? trabajoId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var exportResult = await _ipsService.ExportarRevisionesAsync(trabajoId, cancellationToken);

            // El servicio existente devuelve un path físico y relativo
            // Redirigir al usuario al archivo generado
            return Json(new
            {
                success = true,
                message = "Exportación exitosa",
                downloadUrl = exportResult.PublicRelativePath,
                fileName = Path.GetFileName(exportResult.PhysicalPath)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando revisiones IPS trabajo {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error exportando revisiones" });
        }
    }

    // ==================== NUEVOS MÉTODOS SPRINT 3 ====================

    /// <summary>
    /// IndexV2 - Nueva versión con filtros extendidos (Sprint 3)
    /// Usa nuevos métodos ObtenerRevisionesAsync con filtros
    /// </summary>
    [HttpGet("IndexV2")]
    public async Task<IActionResult> IndexV2(long? trabajoId = null, int? procesoId = null, string? metodo = null)
    {
        try
            {
            var userRol = User.FindFirst(ClaimTypes.Role)?.Value;
            
                    var (success, data, error) = await _ipsService.ObtenerRevisionesAsync(
                        trabajoId, procesoId, metodo, userRol);

                    if (!success)
                    {
                        TempData["Error"] = error;
                        return View("Index", new IpsRevisionViewModel 
                        { 
                            TrabajoId = trabajoId, 
                            Revisiones = new List<IpsRevisionRowViewModel>() 
                        });
                    }

                    ViewBag.TrabajoId = trabajoId;
                    ViewBag.ProcesoId = procesoId;
                    ViewBag.Metodo = metodo;

                    // Cargar procesos para dropdown
                    var (successProcesos, procesos, _) = await _ipsService.ObtenerProcesosAsync();
                    ViewBag.Procesos = successProcesos ? procesos : new List<ProcesoIpsVm>();

                    // Convertir a ViewModel legacy para compatibilidad con vista existente
                    var viewModel = new IpsRevisionViewModel
                    {
                        TrabajoId = trabajoId,
                        Revisiones = data.Select(d => new IpsRevisionRowViewModel
                        {
                            Id = d.Id,
                            TrabajoId = d.TrabajoId,
                            Trabajo = d.TrabajoNombre,
                            Pregunta = d.TareaNombre ?? "",
                            Observacion = d.Observaciones ?? "",
                            DescripcionObservacion = d.Observaciones ?? "",
                            Instrumento = "",
                            Estado = d.EstadoWorkflowDescripcion,
                            FechaHoraObservacion = d.FechaRevision ?? DateTime.UtcNow
                        }).ToList()
                    };

                    return View("Index", viewModel);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error cargando revisiones IPS V2. Trabajo: {TrabajoId}", trabajoId);
                    TempData["Error"] = "Error cargando revisiones IPS";
                    return View("Index", new IpsRevisionViewModel { TrabajoId = trabajoId, Revisiones = new List<IpsRevisionRowViewModel>() });
                }
            }

            /// <summary>
            /// NotificarV2 - Notificar proceso IPS usando nuevo método Sprint 3
            /// </summary>
            [HttpPost("NotificarV2")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> NotificarV2(long id)
            {
                try
                {
                    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (!long.TryParse(userIdClaim, out var usuarioId))
                    {
                        return Json(new { success = false, error = "Usuario no autenticado" });
                    }

                    var (success, error) = await _ipsService.NotificarProcesoAsync(id, usuarioId);

                    if (!success)
                    {
                        return Json(new { success = false, error });
                    }

                    return Json(new { success = true, message = "Proceso notificado exitosamente" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al notificar proceso IPS V2. ID: {Id}", id);
                    return Json(new { success = false, error = "Error al notificar proceso" });
                }
            }

            /// <summary>
            /// RechazarV2 - Rechazar proceso IPS usando nuevo método Sprint 3
            /// </summary>
            [HttpPost("RechazarV2")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> RechazarV2(long id, string observaciones)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(observaciones))
                    {
                        return Json(new { success = false, error = "Las observaciones son requeridas para rechazar" });
                    }

                    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (!long.TryParse(userIdClaim, out var usuarioId))
                    {
                        return Json(new { success = false, error = "Usuario no autenticado" });
                    }

                    var (success, error) = await _ipsService.RechazarProcesoAsync(id, usuarioId, observaciones);

                    if (!success)
                    {
                        return Json(new { success = false, error });
                    }

                    return Json(new { success = true, message = "Proceso rechazado exitosamente" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al rechazar proceso IPS V2. ID: {Id}", id);
                    return Json(new { success = false, error = "Error al rechazar proceso" });
                }
            }

            /// <summary>
            /// ExportExcelV2 - Exportar con nuevos filtros Sprint 3
            /// </summary>
            [HttpGet("ExportExcelV2")]
            public async Task<IActionResult> ExportExcelV2(long? trabajoId = null, int? procesoId = null, string? metodo = null)
            {
                try
                {
                    var userRol = User.FindFirst(ClaimTypes.Role)?.Value;

                    var excelBytes = await _ipsService.ExportarRevisionesExcelAsync(
                        trabajoId, procesoId, metodo, userRol);

                    if (excelBytes == null || excelBytes.Length == 0)
                    {
                        TempData["Warning"] = "No hay datos para exportar";
                        return RedirectToAction("Index", new { trabajoId, procesoId, metodo });
                    }

                    var nombreArchivo = $"IPS_Revisiones_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al exportar revisiones IPS V2");
                    TempData["Error"] = "Error al exportar revisiones";
                    return RedirectToAction("Index");
        }
    }
}
