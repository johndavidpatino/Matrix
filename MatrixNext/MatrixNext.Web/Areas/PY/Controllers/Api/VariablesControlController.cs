using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Services.PY.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.PY.Controllers.Api
{
    [Authorize]
    [Route("api/py/[controller]")]
    [ApiController]
    public class VariablesControlController : ControllerBase
    {
        private readonly IPyVariablesControlService _service;
        private readonly ILogger<VariablesControlController> _logger;

        public VariablesControlController(IPyVariablesControlService service, ILogger<VariablesControlController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtener variables de control por trabajo
        /// GET: api/py/variablescontrol/{trabajoId}
        /// </summary>
        [HttpGet("{trabajoId}")]
        public async Task<ActionResult<List<VariableControlDto>>> ObtenerVariables(int trabajoId)
        {
            try
            {
                var variables = await _service.ObtenerVariablesPorTrabajo(trabajoId);
                return Ok(variables);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo variables control para trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Guardar nueva variable de control
        /// POST: api/py/variablescontrol
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> GuardarVariable([FromBody] VariableControlInputDto input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState });

                var id = await _service.GuardarVariableControl(input);
                return Ok(new { success = true, id, message = "Variable de control guardada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando variable de control");
                return StatusCode(500, new { success = false, message = "Error al guardar variable de control. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// Validar completitud de variables de control
        /// GET: api/py/variablescontrol/{trabajoId}/validar
        /// </summary>
        [HttpGet("{trabajoId}/validar")]
        public async Task<ActionResult<bool>> ValidarVariables(int trabajoId)
        {
            try
            {
                var completadas = await _service.ValidarVariablesCompletadas(trabajoId);
                return Ok(new { success = true, completadas, message = completadas ? "Variables completas" : "Faltan variables por completar" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando variables trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error al validar variables. Por favor intente nuevamente." });
            }
        }
    }
}
