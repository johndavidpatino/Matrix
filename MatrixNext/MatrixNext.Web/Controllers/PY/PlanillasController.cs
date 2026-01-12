using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Services.PY.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Controllers.PY
{
    [Authorize]
    [Route("api/py/[controller]")]
    [ApiController]
    public class PlanillasController : ControllerBase
    {
        private readonly IPyPlanillasService _service;
        private readonly ILogger<PlanillasController> _logger;

        public PlanillasController(IPyPlanillasService service, ILogger<PlanillasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ========== CATÁLOGOS ==========

        /// <summary>
        /// Obtener técnicas UU disponibles
        /// GET: api/py/planillas/tecnicas
        /// </summary>
        [HttpGet("tecnicas")]
        public async Task<ActionResult<List<TecnicaDto>>> ObtenerTecnicas([FromQuery] string tipo = "")
        {
            try
            {
                var tecnicas = await _service.ObtenerTecnicas(tipo);
                return Ok(tecnicas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo técnicas");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        // ========== PLANILLAS MODERACIÓN ==========

        /// <summary>
        /// Crear nueva planilla de moderación
        /// POST: api/py/planillas/moderacion
        /// </summary>
        [HttpPost("moderacion")]
        public async Task<ActionResult<int>> CrearPlanillaModeracion([FromBody] PlanillaModeracionInputDto input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState });

                var id = await _service.CrearPlanillaModeracion(input);
                return Ok(new { success = true, id, message = "Planilla de moderación creada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando planilla moderación");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Actualizar planilla de moderación
        /// PUT: api/py/planillas/moderacion/{id}
        /// </summary>
        [HttpPut("moderacion/{id}")]
        public async Task<ActionResult> ActualizarPlanillaModeracion(int id, [FromBody] PlanillaModeracionActualizacionDto input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState });

                input.IdPlanilla = id;
                var resultado = await _service.ActualizarPlanillaModeracion(input);
                return Ok(new { success = resultado, message = "Planilla actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando planilla moderación {Id}", id);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Validar planilla de moderación
        /// GET: api/py/planillas/moderacion/{id}/validar
        /// </summary>
        [HttpGet("moderacion/{id}/validar")]
        public async Task<ActionResult<List<string>>> ValidarPlanilla(int id)
        {
            try
            {
                var errores = await _service.ValidarPlanillaModeracion(id);
                return Ok(new { success = errores.Count == 0, errores, message = errores.Count == 0 ? "Planilla válida" : "Errores encontrados" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando planilla {Id}", id);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ========== PLANILLAS INFORMES ==========

        /// <summary>
        /// Obtener planillas de informes
        /// GET: api/py/planillas/informes
        /// </summary>
        [HttpGet("informes")]
        public async Task<ActionResult<List<PlanillaInformesDto>>> ObtenerPlanillasInformes([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFinal)
        {
            try
            {
                var planillas = await _service.ObtenerPlanillasInformes(fechaInicio, fechaFinal);
                return Ok(planillas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo planillas informes");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualizar estado de planilla de informes
        /// PUT: api/py/planillas/informes/{id}/estado
        /// </summary>
        [HttpPut("informes/{id}/estado")]
        public async Task<ActionResult> ActualizarEstadoInformes(int id, [FromBody] ActualizarEstadoInputDto input)
        {
            try
            {
                var resultado = await _service.ActualizarEstadoPlanillaInformes(id, input.Estado);
                return Ok(new { success = resultado, message = "Estado actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando estado planilla {Id}", id);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ========== EXPORTACIÓN ==========

        /// <summary>
        /// Obtener planillas para exportar a UU
        /// GET: api/py/planillas/exportar
        /// </summary>
        [HttpGet("exportar")]
        public async Task<ActionResult<List<PlanillaListDto>>> ObtenerPlanillasParaExportar([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFinal)
        {
            try
            {
                var planillas = await _service.ObtenerPlanillasParaExportar(fechaInicio, fechaFinal);
                return Ok(planillas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo planillas exportar");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Marcar planilla como exportada
        /// POST: api/py/planillas/{id}/marcar-exportada
        /// </summary>
        [HttpPost("{id}/marcar-exportada")]
        public async Task<ActionResult> MarcarExportada(int id)
        {
            try
            {
                var resultado = await _service.MarcarExportada(id);
                return Ok(new { success = resultado, message = "Planilla marcada como exportada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marcando planilla exportada {Id}", id);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ========== ESTADÍSTICAS ==========

        /// <summary>
        /// Obtener estadísticas de planillas
        /// GET: api/py/planillas/estadisticas
        /// </summary>
        [HttpGet("estadisticas")]
        public async Task<ActionResult<dynamic>> ObtenerEstadisticas([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFinal)
        {
            try
            {
                var stats = await _service.ObtenerEstadisticasPlanillas(fechaInicio, fechaFinal);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo estadísticas planillas");
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }
    }

    public class ActualizarEstadoInputDto
    {
        public string Estado { get; set; } = string.Empty;
    }
}
