using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.TH.Models;
using MatrixNext.Data.Services.TH.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.TH.Controllers.Api
{
    /// <summary>
    /// Controller para gestión de Desvinculaciones de Empleados
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/th/[controller]")]
    public class DesvinculacionesController : ControllerBase
    {
        private readonly IThDesvinculacionService _service;
        private readonly ILogger<DesvinculacionesController> _logger;

        public DesvinculacionesController(IThDesvinculacionService service, ILogger<DesvinculacionesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// GET /api/th/desvinculaciones - Obtiene desvinculaciones con paginación
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<DesvinculacionDto>>>> Get(
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageIndex = 1,
            [FromQuery] string? textoBuscado = null)
        {
            try
            {
                var resultado = await _service.ObtenerDesvinculaciones(pageSize, pageIndex, textoBuscado);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Get Desvinculaciones");
                return StatusCode(500, ApiResponse<List<DesvinculacionDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// POST /api/th/desvinculaciones - Inicia proceso de desvinculación
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<long>>> Post([FromBody] DesvinculacionInputDto input)
        {
            try
            {
                var resultado = await _service.IniciarProcesoDesvinculacion(input);
                return resultado.Success ? Created($"api/th/desvinculaciones/{resultado.Data}", resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Post Desvinculación");
                return StatusCode(500, ApiResponse<long>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// GET /api/th/desvinculaciones/{id}/evaluaciones - Obtiene evaluaciones pendientes
        /// </summary>
        [HttpGet("{id}/evaluaciones")]
        public async Task<ActionResult<ApiResponse<List<dynamic>>>> GetEvaluaciones(long id)
        {
            try
            {
                var resultado = await _service.ObtenerEvaluacionesDesvinculacion(id);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetEvaluaciones {id}");
                return StatusCode(500, ApiResponse<List<dynamic>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// POST /api/th/desvinculaciones/{id}/evaluaciones - Guarda evaluación
        /// </summary>
        [HttpPost("{id}/evaluaciones")]
        public async Task<ActionResult<ApiResponse<bool>>> PostEvaluacion(long id, [FromBody] DesvinculacionEvaluacionInputDto input)
        {
            try
            {
                input.DesvinculacionEmpleadoId = id;
                var usuario = User?.Identity?.Name ?? "Sistema";
                var resultado = await _service.GuardarEvaluacionDesvinculacion(input, usuario);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PostEvaluacion {id}");
                return StatusCode(500, ApiResponse<bool>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// PUT /api/th/desvinculaciones/{id}/finalizar - Finaliza el proceso
        /// </summary>
        [HttpPut("{id}/finalizar")]
        public async Task<ActionResult<ApiResponse<bool>>> PutFinalizar(long id)
        {
            try
            {
                var resultado = await _service.FinalizarProcesoDesvinculacion(id);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PutFinalizar {id}");
                return StatusCode(500, ApiResponse<bool>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// GET /api/th/desvinculaciones/{id}/pdf - Genera PDF de desvinculación
        /// </summary>
        [HttpGet("{id}/pdf")]
        public async Task<ActionResult> GetPDF(long id)
        {
            try
            {
                var resultado = await _service.GenerarPDFDesvinculacion(id);
                if (!resultado.Success || string.IsNullOrEmpty(resultado.Data))
                    return BadRequest(ApiResponse<string>.Error("No se pudo generar el PDF"));

                var pdfBytes = Convert.FromBase64String(resultado.Data);
                return File(pdfBytes, "application/pdf", $"desvinculacion_{id}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetPDF {id}");
                return StatusCode(500, ApiResponse<string>.Error("Error interno del servidor"));
            }
        }
    }
}
