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
    /// Controlador para la revisión de productividad desde rol MyS/Call.
    /// Permite aprobar/rechazar planillas de CATI/CAWI y supervisión.
    /// Requiere permiso 157 (Supervisión/MyS).
    /// </summary>
    [Area("OP")]
    [Authorize]
    [Route("OP/[controller]")]
    public class RevisionProductividadMYSCallController : Controller
    {
        private readonly IOpRevisionProductividadService _revisionService;
        private readonly ILogger<RevisionProductividadMYSCallController> _logger;

        public RevisionProductividadMYSCallController(
            IOpRevisionProductividadService revisionService,
            ILogger<RevisionProductividadMYSCallController> logger)
        {
            _revisionService = revisionService;
            _logger = logger;
        }

        /// <summary>
        /// Muestra el listado de planillas de productividad pendientes de aprobación por MyS/Call.
        /// Filtra planillas de actividades CATI/CAWI (TipoActividad 21-23).
        /// </summary>
        /// <param name="trabajoId">ID del trabajo a revisar (opcional)</param>
        /// <returns>Vista con grid de planillas</returns>
        [HttpGet("Index")]
        public async Task<IActionResult> Index(int? trabajoId = null)
        {
            try
            {
                // TODO: Validar permiso 157 (Supervisión/MyS)
                // TODO: Filtrar solo planillas de CATI/CAWI

                if (!trabajoId.HasValue)
                {
                    return View("Index", new List<PlanillaProductividadDto>());
                }

                var planillas = await _revisionService.ObtenerPlanillasPorRolAsync(trabajoId.Value, "MyS/Call");

                _logger.LogInformation("Supervisor MyS/Call {User} accedió a revisión para trabajo {TrabajoId}", 
                    User.FindFirst(ClaimTypes.NameIdentifier)?.Value, trabajoId);

                return View("Index", planillas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Index de RevisionProductividadMYSCall");
                TempData["Error"] = "Error al cargar las planillas";
                return RedirectToAction("Index", "Trabajos");
            }
        }

        /// <summary>
        /// Aprueba una planilla de productividad con monto autorizado.
        /// </summary>
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
                var resultado = await _revisionService.AprobarPlanillaAsync(planillaId, montoAutorizado, usuarioId);

                if (resultado)
                {
                    _logger.LogInformation("Planilla {PlanillaId} aprobada por MyS/Call {Usuario}", planillaId, usuarioId);
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
                var resultado = await _revisionService.RechazarPlanillaAsync(planillaId, observacion.Trim(), usuarioId);

                if (resultado)
                {
                    _logger.LogWarning("Planilla {PlanillaId} rechazada por MyS/Call {Usuario}", planillaId, usuarioId);
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
    }
}
