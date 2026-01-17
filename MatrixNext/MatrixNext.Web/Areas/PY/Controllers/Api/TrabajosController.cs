using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Services.PY.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.PY.Controllers.Api
{
    [Authorize]
    [Route("api/py/[controller]")]
    [ApiController]
    public class TrabajosController : ControllerBase
    {
        private readonly IPyTrabajosService _service;
        private readonly ILogger<TrabajosController> _logger;

        public TrabajosController(IPyTrabajosService service, ILogger<TrabajosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ========== DUPLICACIÓN ==========

        /// <summary>
        /// Duplicar trabajo completo con especificaciones, muestras, hilo y configuración
        /// POST: api/py/trabajos/duplicar
        /// </summary>
        [HttpPost("duplicar")]
        public async Task<ActionResult<DuplicarTrabajoResultDto>> DuplicarTrabajo([FromBody] DuplicarTrabajoInputDto input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState });

                var usuario = User.Identity?.Name ?? "Sistema";
                var resultado = await _service.DuplicarTrabajoCompleto(input, usuario);

                if (resultado.NuevoTrabajoId <= 0)
                    return BadRequest(new { success = false, message = resultado.ErrorMessage ?? "Error al duplicar trabajo" });

                return Ok(new 
                { 
                    success = true, 
                    data = resultado, 
                    message = $"Trabajo duplicado exitosamente. Nuevo ID: {resultado.NuevoTrabajoId}" 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error duplicando trabajo");
                return StatusCode(500, new { success = false, message = "Error al duplicar trabajo. Por favor intente nuevamente." });
            }
        }

        // ========== CONFIGURACIÓN ==========

        /// <summary>
        /// Obtener configuración de trabajo
        /// GET: api/py/trabajos/{trabajoId}/configuracion
        /// </summary>
        [HttpGet("{trabajoId}/configuracion")]
        public async Task<ActionResult<TrabajoConfiguracionDto>> ObtenerConfiguracion(int trabajoId)
        {
            try
            {
                var config = await _service.ObtenerConfiguracionTrabajo(trabajoId);
                return Ok(config);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo configuración trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Guardar configuración de trabajo
        /// POST: api/py/trabajos/{trabajoId}/configuracion
        /// </summary>
        [HttpPost("{trabajoId}/configuracion")]
        public async Task<ActionResult> GuardarConfiguracion(int trabajoId, [FromBody] TrabajoConfiguracionInputDto input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState });

                input.TrabajoId = trabajoId;
                var usuario = User.Identity?.Name ?? "Sistema";
                var resultado = await _service.GuardarConfiguracionTrabajo(input, usuario);
                
                return Ok(new { success = resultado, message = "Configuración guardada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando configuración trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error al guardar configuración. Por favor intente nuevamente." });
            }
        }

        // ========== VALIDACIÓN Y ESTADO ==========

        /// <summary>
        /// Validar que trabajo esté listo para iniciar
        /// GET: api/py/trabajos/{trabajoId}/validar
        /// </summary>
        [HttpGet("{trabajoId}/validar")]
        public async Task<ActionResult<bool>> ValidarTrabajo(int trabajoId)
        {
            try
            {
                var listo = await _service.ValidarTrabajoListo(trabajoId);
                return Ok(new 
                { 
                    success = listo, 
                    listo, 
                    message = listo ? "Trabajo listo para iniciar" : "Faltan configuraciones por completar" 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error al validar trabajo. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// Obtener estado actual del trabajo
        /// GET: api/py/trabajos/{trabajoId}/estado
        /// </summary>
        [HttpGet("{trabajoId}/estado")]
        public async Task<ActionResult<dynamic>> ObtenerEstado(int trabajoId)
        {
            try
            {
                var estado = await _service.ObtenerEstadoTrabajo(trabajoId);
                return Ok(estado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo estado trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        // ========== CIERRE ==========

        /// <summary>
        /// Cerrar trabajo (marcar como completado y archivar)
        /// POST: api/py/trabajos/{trabajoId}/cerrar
        /// </summary>
        [HttpPost("{trabajoId}/cerrar")]
        public async Task<ActionResult> CerrarTrabajo(int trabajoId, [FromBody] CerrarTrabajoInputDto input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState });

                var usuario = User.Identity?.Name ?? "Sistema";
                var resultado = await _service.CerrarTrabajo(trabajoId, input.Motivo, usuario);
                
                return Ok(new { success = resultado, message = "Trabajo cerrado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cerrando trabajo {TrabajoId}", trabajoId);
                return StatusCode(500, new { success = false, message = "Error al cerrar trabajo. Por favor intente nuevamente." });
            }
        }
    }

    public class CerrarTrabajoInputDto
    {
        public string Motivo { get; set; } = string.Empty;
    }
}
