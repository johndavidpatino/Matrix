using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Services.PY.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.PY.Controllers.Api
{
    [Authorize]
    [Route("api/py/[controller]")]
    [ApiController]
    public class InHomeVisitController : ControllerBase
    {
        private readonly IPyInHomeVisitService _service;
        private readonly ILogger<InHomeVisitController> _logger;

        public InHomeVisitController(IPyInHomeVisitService service, ILogger<InHomeVisitController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtener visitas InHome por trabajo
        /// GET: api/py/inhomevisit/{trabajoId}
        /// </summary>
        [HttpGet("{trabajoId}")]
        public async Task<ActionResult<List<InHomeVisitDto>>> ObtenerInHomes(int trabajoId)
        {
            try
            {
                var inHomes = await _service.ObtenerInHomesPorTrabajo(trabajoId);
                return Ok(inHomes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo InHomes para trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener log de una visita InHome
        /// GET: api/py/inhomevisit/{idInHome}/log
        /// </summary>
        [HttpGet("{idInHome}/log")]
        public async Task<ActionResult<List<LogInHomeDto>>> ObtenerLog(int idInHome)
        {
            try
            {
                var logs = await _service.ObtenerLogInHome(idInHome);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo log InHome {IdInHome}", idInHome);
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Guardar nueva visita InHome
        /// POST: api/py/inhomevisit
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> GuardarInHome([FromBody] InHomeVisitInputDto input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState });

                var id = await _service.GuardarInHome(input);
                return Ok(new { success = true, id, message = "Visita InHome guardada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando InHome");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Actualizar visita InHome existente
        /// PUT: api/py/inhomevisit/{id}
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarInHome(int id, [FromBody] InHomeVisitInputDto input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState });

                input.Id = id;
                var resultado = await _service.ActualizarInHome(input);
                return Ok(new { success = resultado, message = "Visita InHome actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando InHome {Id}", id);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Registrar evento en log de InHome
        /// POST: api/py/inhomevisit/{idInHome}/log
        /// </summary>
        [HttpPost("{idInHome}/log")]
        public async Task<ActionResult> GuardarLog(int idInHome, [FromBody] LogInHomeInputDto input)
        {
            try
            {
                var usuario = User.Identity?.Name ?? "Sistema";
                var id = await _service.GuardarLogInHome(idInHome, input.Descripcion, usuario);
                return Ok(new { success = true, id, message = "Log registrado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando log InHome {IdInHome}", idInHome);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }

    public class LogInHomeInputDto
    {
        public string Descripcion { get; set; } = string.Empty;
    }
}
