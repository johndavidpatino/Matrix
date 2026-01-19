using MatrixNext.Data.Modules.CC.DTOs;
using MatrixNext.Data.Modules.CC.DTOs.ProcesosInternos;
using MatrixNext.Data.Modules.CC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.CC.Controllers
{
    /// <summary>
    /// Controller para Conteo de Trabajos (Preguntas Históricas de Cuestionarios)
    /// Migrado de: WebMatrix/CC_FinzOpe/ConteoTrabajos.aspx
    /// </summary>
    [Area("CC")]
    [Route("CC/[controller]")]
    [Authorize]
    public class ConteoTrabajosController : Controller
    {
        private readonly ICcProcesosInternosService _service;
        private readonly ILogger<ConteoTrabajosController> _logger;

        public ConteoTrabajosController(
            ICcProcesosInternosService service,
            ILogger<ConteoTrabajosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> ObtenerConteos(
            long? idTrabajo, DateTime? fechaInicio, DateTime? fechaFin)
        {
            try
            {
                var conteos = await _service.ObtenerConteosAsync(idTrabajo, fechaInicio, fechaFin);
                return Json(new { success = true, data = conteos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo conteos");
                return Json(new { success = false, message = "Error al obtener los conteos. Por favor intente nuevamente." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerActividadesPorTrabajo(long idTrabajo)
        {
            try
            {
                var actividades = await _service.ObtenerActividadesPorTrabajoAsync(idTrabajo);
                return Json(new { success = true, data = actividades });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo actividades para trabajo {TrabajoId}", idTrabajo);
                return Json(new { success = false, message = "Error al obtener las actividades. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// Guarda preguntas históricas de un trabajo (conteo de cuestionario)
        /// Migrado de: btnGuardarPreguntas_Click en ConteoTrabajos.aspx.vb
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GuardarPreguntas([FromBody] GuardarPreguntasHistoricoRequest request)
        {
            try
            {
                // Obtener usuario actual
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
                {
                    return Json(new { success = false, message = "Usuario no autenticado" });
                }

                request.UsuarioId = userId;
                await _service.GuardarPreguntasHistoricoAsync(request);
                return Json(new { success = true, message = "Datos guardados exitosamente" });
            }
            catch (ArgumentException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando preguntas históricas");
                return Json(new { success = false, message = "Error al guardar las preguntas. Por favor intente nuevamente." });
            }
        }
    }
}
