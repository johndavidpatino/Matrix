using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Services.PY.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.PY.Controllers.Api
{
    [Authorize]
    [Route("api/py/[controller]")]
    [ApiController]
    public class InstructivosController : ControllerBase
    {
        private readonly IPyInstructivosService _service;
        private readonly ILogger<InstructivosController> _logger;

        public InstructivosController(IPyInstructivosService service, ILogger<InstructivosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ========== CUANTITATIVO ==========

        /// <summary>
        /// Obtener especificación técnica cuantitativa
        /// GET: api/py/instructivos/cuanti/{trabajoId}
        /// </summary>
        [HttpGet("cuanti/{trabajoId}")]
        public async Task<ActionResult<EspecificacionTecnicaDto>> ObtenerEspecificacionCuanti(int trabajoId)
        {
            try
            {
                var espec = await _service.ObtenerEspecificacionCuanti(trabajoId);
                return Ok(espec);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo especificación cuanti trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Guardar especificación técnica cuantitativa
        /// POST: api/py/instructivos/cuanti
        /// </summary>
        [HttpPost("cuanti")]
        public async Task<ActionResult<int>> GuardarEspecificacionCuanti([FromBody] EspecificacionTecnicaInputDto input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState });

                var usuario = User.Identity?.Name ?? "Sistema";
                var id = await _service.GuardarEspecificacionCuanti(input, usuario);
                return Ok(new { success = true, id, message = "Especificación cuantitativa guardada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando especificación cuanti");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ========== CUALITATIVO ==========

        /// <summary>
        /// Obtener especificación técnica cualitativa
        /// GET: api/py/instructivos/cuali/{trabajoId}
        /// </summary>
        [HttpGet("cuali/{trabajoId}")]
        public async Task<ActionResult<EspecificacionTecnicaCualiDto>> ObtenerEspecificacionCuali(int trabajoId)
        {
            try
            {
                var espec = await _service.ObtenerEspecificacionCuali(trabajoId);
                return Ok(espec);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo especificación cuali trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Guardar especificación técnica cualitativa
        /// POST: api/py/instructivos/cuali
        /// </summary>
        [HttpPost("cuali")]
        public async Task<ActionResult<int>> GuardarEspecificacionCuali([FromBody] EspecificacionTecnicaCualiInputDto input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState });

                var usuario = User.Identity?.Name ?? "Sistema";
                var id = await _service.GuardarEspecificacionCuali(input, usuario);
                return Ok(new { success = true, id, message = "Especificación cualitativa guardada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando especificación cuali");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ========== AYUDAS CUALI ==========

        /// <summary>
        /// Obtener ayudas cualitativas
        /// GET: api/py/instructivos/ayudas/{trabajoId}
        /// </summary>
        [HttpGet("ayudas/{trabajoId}")]
        public async Task<ActionResult<List<AyudaCualiDto>>> ObtenerAyudas(int trabajoId)
        {
            try
            {
                var ayudas = await _service.ObtenerAyudasCuali(trabajoId);
                return Ok(ayudas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo ayudas cuali trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Guardar ayuda cualitativa
        /// POST: api/py/instructivos/ayudas
        /// </summary>
        [HttpPost("ayudas")]
        public async Task<ActionResult<int>> GuardarAyuda([FromBody] AyudaCualiInputDto input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState });

                var id = await _service.GuardarAyudaCuali(input);
                return Ok(new { success = true, id, message = "Ayuda cualitativa guardada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando ayuda cuali");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ========== TIPOS RECLUTAMIENTO ==========

        /// <summary>
        /// Obtener tipos de reclutamiento cualitativos
        /// GET: api/py/instructivos/reclutamiento/{trabajoId}
        /// </summary>
        [HttpGet("reclutamiento/{trabajoId}")]
        public async Task<ActionResult<List<TipoReclutamientoCualiDto>>> ObtenerTiposReclutamiento(int trabajoId)
        {
            try
            {
                var tipos = await _service.ObtenerTiposReclutamientoCuali(trabajoId);
                return Ok(tipos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo tipos reclutamiento trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Guardar tipo de reclutamiento cualitativo
        /// POST: api/py/instructivos/reclutamiento
        /// </summary>
        [HttpPost("reclutamiento")]
        public async Task<ActionResult<int>> GuardarTipoReclutamiento([FromBody] TipoReclutamientoCualiInputDto input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState });

                var id = await _service.GuardarTipoReclutamientoCuali(input);
                return Ok(new { success = true, id, message = "Tipo reclutamiento guardado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando tipo reclutamiento");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ========== VERSIONES ==========

        /// <summary>
        /// Obtener historial de versiones
        /// GET: api/py/instructivos/versiones/{trabajoId}
        /// </summary>
        [HttpGet("versiones/{trabajoId}")]
        public async Task<ActionResult<List<dynamic>>> ObtenerHistorial(int trabajoId, [FromQuery] string tipo = "Cuantitativo")
        {
            try
            {
                var historial = await _service.ObtenerHistorialVersiones(trabajoId, tipo);
                return Ok(historial);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo historial versiones trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }
    }
}
