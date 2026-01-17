using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Services.PY.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.PY.Controllers.Api
{
    [Authorize]
    [Route("api/py/[controller]")]
    [ApiController]
    public class DistribucionEntrevistasController : ControllerBase
    {
        private readonly IPyDistribucionEntrevistasService _service;
        private readonly ILogger<DistribucionEntrevistasController> _logger;

        public DistribucionEntrevistasController(IPyDistribucionEntrevistasService service, ILogger<DistribucionEntrevistasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ========== ENTREVISTAS ==========

        /// <summary>
        /// Obtener entrevistas pendientes por trabajo
        /// GET: api/py/distribucionentrevistas/pendientes/{trabajoId}
        /// </summary>
        [HttpGet("pendientes/{trabajoId}")]
        public async Task<ActionResult<List<EntrevistaCualiDto>>> ObtenerPendientes(int trabajoId)
        {
            try
            {
                var entrevistas = await _service.ObtenerEntrevistasPendientes(trabajoId);
                return Ok(entrevistas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo entrevistas pendientes trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener distribución asignada por trabajo
        /// GET: api/py/distribucionentrevistas/asignadas/{trabajoId}
        /// </summary>
        [HttpGet("asignadas/{trabajoId}")]
        public async Task<ActionResult<List<DistribucionEntrevistaDto>>> ObtenerAsignadas(int trabajoId)
        {
            try
            {
                var distribuciones = await _service.ObtenerDistribucionAsignada(trabajoId);
                return Ok(distribuciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo distribución asignada trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        // ========== DISTRIBUCIÓN ==========

        /// <summary>
        /// Guardar nueva distribución de entrevista
        /// POST: api/py/distribucionentrevistas
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> GuardarDistribucion([FromBody] DistribucionEntrevistaInputDto input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState });

                var usuario = User.Identity?.Name ?? "Sistema";
                var id = await _service.GuardarDistribucion(input, usuario);
                return Ok(new { success = true, id, message = "Distribución guardada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando distribución");
                return StatusCode(500, new { success = false, message = "Error al procesar la solicitud. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// Actualizar estado de distribución
        /// PUT: api/py/distribucionentrevistas/{id}/estado
        /// </summary>
        [HttpPut("{id}/estado")]
        public async Task<ActionResult> ActualizarEstado(int id, [FromBody] ActualizarEstadoDistribucionInputDto input)
        {
            try
            {
                var resultado = await _service.ActualizarEstadoDistribucion(id, input.Estado, input.Observaciones);
                return Ok(new { success = resultado, message = "Estado actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando estado distribución {Id}", id);
                return StatusCode(500, new { success = false, message = "Error al procesar la solicitud. Por favor intente nuevamente." });
            }
        }

        // ========== LOG ==========

        /// <summary>
        /// Obtener log de distribución
        /// GET: api/py/distribucionentrevistas/{id}/log
        /// </summary>
        [HttpGet("{id}/log")]
        public async Task<ActionResult<List<LogEntrevistaCualiDto>>> ObtenerLog(int id)
        {
            try
            {
                var logs = await _service.ObtenerLogDistribucion(id);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo log distribución {Id}", id);
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Guardar evento en log
        /// POST: api/py/distribucionentrevistas/{id}/log
        /// </summary>
        [HttpPost("{id}/log")]
        public async Task<ActionResult> GuardarLog(int id, [FromBody] GuardarLogInputDto input)
        {
            try
            {
                var usuario = User.Identity?.Name ?? "Sistema";
                var resultado = await _service.GuardarLogEntrevista(id, input.Evento, input.Descripcion, usuario);
                return Ok(new { success = true, id = resultado, message = "Log guardado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando log distribución {Id}", id);
                return StatusCode(500, new { success = false, message = "Error al procesar la solicitud. Por favor intente nuevamente." });
            }
        }

        // ========== MODERADORES ==========

        /// <summary>
        /// Obtener moderadores disponibles
        /// GET: api/py/distribucionentrevistas/moderadores
        /// </summary>
        [HttpGet("moderadores")]
        public async Task<ActionResult<List<ModeradorCualiDto>>> ObtenerModeradoresDisponibles([FromQuery] DateTime fecha, [FromQuery] string zona = "")
        {
            try
            {
                var moderadores = await _service.ObtenerModeradoresDisponibles(fecha, zona);
                return Ok(moderadores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo moderadores disponibles");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        // ========== REPORTES ==========

        /// <summary>
        /// Obtener avance de entrevistas
        /// GET: api/py/distribucionentrevistas/{trabajoId}/avance
        /// </summary>
        [HttpGet("{trabajoId}/avance")]
        public async Task<ActionResult<dynamic>> ObtenerAvance(int trabajoId)
        {
            try
            {
                var avance = await _service.ObtenerAvanceEntrevistas(trabajoId);
                return Ok(avance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo avance trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Validar distribución completa
        /// GET: api/py/distribucionentrevistas/{trabajoId}/validar
        /// </summary>
        [HttpGet("{trabajoId}/validar")]
        public async Task<ActionResult<List<string>>> ValidarDistribucion(int trabajoId)
        {
            try
            {
                var errores = await _service.ValidarDistribucionCompleta(trabajoId);
                return Ok(new { success = errores.Count == 0, errores, message = errores.Count == 0 ? "Distribución completa" : "Faltan asignaciones" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando distribución trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error al procesar la solicitud. Por favor intente nuevamente." });
            }
        }
    }

    public class ActualizarEstadoDistribucionInputDto
    {
        public string Estado { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
    }

    public class GuardarLogInputDto
    {
        public string Evento { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }
}
