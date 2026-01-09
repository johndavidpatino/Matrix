using MatrixNext.Web.Services.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MatrixNext.Web.Models.OP.Dtos;

namespace MatrixNext.Web.Areas.OP.Controllers
{
    /// <summary>
    /// Controlador para la revisión de productividad desde rol PMO.
    /// Permite aprobar/rechazar planillas de todas las líneas de producción.
    /// Requiere permiso 100 (PMO).
    /// </summary>
    [Area("OP")]
    [Authorize]
    [Route("OP/[controller]")]
    public class RevisionProductividadPMOController : Controller
    {
        private readonly IOpRevisionProductividadService _revisionService;
        private readonly ILogger<RevisionProductividadPMOController> _logger;

        public RevisionProductividadPMOController(
            IOpRevisionProductividadService revisionService,
            ILogger<RevisionProductividadPMOController> logger)
        {
            _revisionService = revisionService;
            _logger = logger;
        }

        /// <summary>
        /// Muestra el listado de planillas de productividad pendientes de aprobación por PMO.
        /// </summary>
        /// <param name="trabajoId">ID del trabajo a revisar (opcional)</param>
        /// <returns>Vista con grid de planillas</returns>
        [HttpGet("Index")]
        public async Task<IActionResult> Index(int? trabajoId = null)
        {
            try
            {
                // TODO: Validar permiso 100 (PMO)
                // if (!User.HasClaim("Permiso", "100"))
                //     return Forbid();

                if (!trabajoId.HasValue)
                {
                    // TODO: Obtener lista de trabajos activos para que PMO seleccione uno
                    return View("Index", new List<PlanillaProductividadDto>());
                }

                // Obtener planillas del trabajo para rol PMO
                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var planillas = await _revisionService.ObtenerPlanillasPorRolAsync(trabajoId.Value, "PMO", usuarioId);

                _logger.LogInformation("Usuario {User} accedió a revisión PMO para trabajo {TrabajoId}", 
                    User.FindFirst(ClaimTypes.NameIdentifier)?.Value, trabajoId);

                return View("Index", planillas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Index de RevisionProductividadPMO");
                TempData["Error"] = "Error al cargar las planillas";
                return RedirectToAction("Index", "Trabajos");
            }
        }

        /// <summary>
        /// Aprueba una planilla de productividad con monto autorizado.
        /// </summary>
        /// <param name="planillaId">ID de la planilla a aprobar</param>
        /// <param name="montoAutorizado">Monto que el PMO autoriza</param>
        /// <returns>JSON resultado de la operación</returns>
        [HttpPost("Aprobar")]
        public async Task<IActionResult> Aprobar(int planillaId, decimal montoAutorizado)
        {
            try
            {
                if (planillaId <= 0)
                    return Json(new { success = false, message = "ID de planilla inválido" });

                if (montoAutorizado < 0)
                    return Json(new { success = false, message = "El monto no puede ser negativo" });

                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var resultado = await _revisionService.AprobarPlanillaAsync(planillaId, montoAutorizado, usuarioId, "PMO");

                if (resultado)
                {
                    _logger.LogInformation("Planilla {PlanillaId} aprobada por PMO {Usuario} con monto {Monto}", 
                        planillaId, usuarioId, montoAutorizado);
                    return Json(new { success = true, message = "Planilla aprobada exitosamente" });
                }

                return Json(new { success = false, message = "No se pudo aprobar la planilla" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error aprobando planilla {PlanillaId}", planillaId);
                return Json(new { success = false, message = "Error al aprobar la planilla" });
            }
        }

        /// <summary>
        /// Rechaza una planilla de productividad con observaciones.
        /// </summary>
        /// <param name="planillaId">ID de la planilla a rechazar</param>
        /// <param name="observacion">Motivo del rechazo</param>
        /// <returns>JSON resultado de la operación</returns>
        [HttpPost("Rechazar")]
        public async Task<IActionResult> Rechazar(int planillaId, string observacion)
        {
            try
            {
                if (planillaId <= 0)
                    return Json(new { success = false, message = "ID de planilla inválido" });

                if (string.IsNullOrWhiteSpace(observacion))
                    return Json(new { success = false, message = "Debe proporcionar una observación" });

                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var resultado = await _revisionService.RechazarPlanillaAsync(planillaId, observacion.Trim(), usuarioId, "PMO");

                if (resultado)
                {
                    _logger.LogWarning("Planilla {PlanillaId} rechazada por PMO {Usuario}. Observación: {Obs}", 
                        planillaId, usuarioId, observacion);
                    return Json(new { success = true, message = "Planilla rechazada. Será devuelta para corrección" });
                }

                return Json(new { success = false, message = "No se pudo rechazar la planilla" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rechazando planilla {PlanillaId}", planillaId);
                return Json(new { success = false, message = "Error al rechazar la planilla" });
            }
        }

        /// <summary>
        /// Obtiene el detalleModal de una planilla para mostrar en confirmación.
        /// </summary>
        /// <param name="planillaId">ID de la planilla</param>
        /// <returns>JSON con detalles de la planilla</returns>
        [HttpGet("Detalles/{planillaId}")]
        public async Task<IActionResult> Detalles(int planillaId)
        {
            try
            {
                // TODO: Obtener detalles completos de la planilla
                return Json(new { success = false, message = "No implementado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo detalles de planilla {PlanillaId}", planillaId);
                return Json(new { success = false, message = "Error al obtener detalles" });
            }
        }
    }
}
