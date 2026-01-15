using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Data.Models.OP_Trafico;
using MatrixNext.Data.Services;
using MatrixNext.Data.Services.OP_Trafico;

namespace MatrixNext.Web.Areas.OP.Controllers
{
    /// <summary>
    /// Controller REST para Operational Traffic (OP_Trafico)
    /// Sprint 11 - Gestión de tráfico de datos / encuestas
    /// Workflow: Capturado → Criticado → Verificado → Anulado
    /// Responsable: Coordinar solicitudes, validar autorización, llamar servicio
    /// REGLA: Solo lógica de coordinación (sin BD directa ni reglas de negocio)
    /// </summary>
    [ApiController]
    [Route("api/[area]/[controller]")]
    [Area("OP")]
    public class OP_TraficoController : ControllerBase
    {
        private readonly IOP_TraficoService _service;
        private readonly ILogger<OP_TraficoController> _logger;

        public OP_TraficoController(IOP_TraficoService service, ILogger<OP_TraficoController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger;
        }

        // ============================================
        // EVENTOS - CONSULTAS
        // ============================================

        /// <summary>
        /// GET /api/op/op_trafico
        /// Obtiene listado de eventos de tráfico con filtros
        /// </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OP_TraficoResultadoDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventos([FromQuery] OP_TraficoFiltrosDTO filtros)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.Error("Datos de filtro inválidos"));

                _logger.LogInformation("[OP_TraficoController] GET: Obteniendo eventos con filtros");

                var response = await _service.ObtenerEventosAsync(filtros);
                return response.Success ? Ok(response) : StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoController] Error inesperado en GetEventos");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener eventos"));
            }
        }

        /// <summary>
        /// GET /api/op/op_trafico/{id}
        /// Obtiene detalle completo de un evento
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OP_TraficoEventoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEvento(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(ApiResponse<string>.Error("ID de evento inválido"));

                _logger.LogInformation($"[OP_TraficoController] GET: Obteniendo evento {id}");

                var response = await _service.ObtenerEventoDetalleAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[OP_TraficoController] Error obteniendo evento {id}");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener evento"));
            }
        }

        // ============================================
        // WORKFLOW - TRANSICIONES DE ESTADO
        // ============================================

        /// <summary>
        /// POST /api/op/op_trafico/capturar
        /// Inicia captura de datos
        /// </summary>
        [HttpPost("capturar")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CapturarDatos([FromBody] OP_TraficoCapturarDTO captura)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.Error("Datos de captura inválidos"));

                _logger.LogInformation("[OP_TraficoController] POST: Capturando datos");

                var response = await _service.CapturarAsync(captura);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoController] Error capturando datos");
                return StatusCode(500, ApiResponse<string>.Error("Error al capturar datos"));
            }
        }

        /// <summary>
        /// POST /api/op/op_trafico/criticar
        /// Realiza crítica de datos
        /// Transición: Capturado → Criticado
        /// </summary>
        [HttpPost("criticar")]
        [Authorize(Roles = "Admin,Supervisor,Criticador")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CriticarDatos([FromBody] OP_TraficoCriticarDTO critica)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.Error("Datos de crítica inválidos"));

                _logger.LogInformation("[OP_TraficoController] POST: Criticando datos");

                var response = await _service.CriticarAsync(critica);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoController] Error criticando datos");
                return StatusCode(500, ApiResponse<string>.Error("Error al criticar datos"));
            }
        }

        /// <summary>
        /// POST /api/op/op_trafico/verificar
        /// Realiza verificación final
        /// Transición: Criticado → Verificado
        /// </summary>
        [HttpPost("verificar")]
        [Authorize(Roles = "Admin,Supervisor,Verificador")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerificarDatos([FromBody] OP_TraficoVerificarDTO verificacion)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.Error("Datos de verificación inválidos"));

                _logger.LogInformation("[OP_TraficoController] POST: Verificando datos");

                var response = await _service.VerificarAsync(verificacion);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoController] Error verificando datos");
                return StatusCode(500, ApiResponse<string>.Error("Error al verificar datos"));
            }
        }

        /// <summary>
        /// POST /api/op/op_trafico/anular
        /// Anula un evento (desde cualquier estado)
        /// </summary>
        [HttpPost("anular")]
        [Authorize(Roles = "Admin,Supervisor")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AnularEvento([FromBody] OP_TraficoAnularDTO anulacion)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.Error("Datos de anulación inválidos"));

                _logger.LogInformation("[OP_TraficoController] POST: Anulando evento");

                var response = await _service.AnularAsync(anulacion);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoController] Error anulando evento");
                return StatusCode(500, ApiResponse<string>.Error("Error al anular evento"));
            }
        }

        // ============================================
        // CONSULTAS POR ESTADO
        // ============================================

        /// <summary>
        /// GET /api/op/op_trafico/capturados
        /// Obtiene eventos en estado Capturado
        /// </summary>
        [HttpGet("capturados/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OP_TraficoCapturadoDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCapturado(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(ApiResponse<string>.Error("ID inválido"));

                _logger.LogInformation($"[OP_TraficoController] GET: Obteniendo capturado {id}");

                var response = await _service.ObtenerCapturadoAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoController] Error en GetCapturado");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener capturado"));
            }
        }

        /// <summary>
        /// GET /api/op/op_trafico/criticados/{id}
        /// Obtiene evento en estado Criticado
        /// </summary>
        [HttpGet("criticados/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OP_TraficoCriticadoDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCriticado(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(ApiResponse<string>.Error("ID inválido"));

                _logger.LogInformation($"[OP_TraficoController] GET: Obteniendo criticado {id}");

                var response = await _service.ObtenerCriticadoAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoController] Error en GetCriticado");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener criticado"));
            }
        }

        /// <summary>
        /// GET /api/op/op_trafico/verificados/{id}
        /// Obtiene evento en estado Verificado
        /// </summary>
        [HttpGet("verificados/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OP_TraficoVerificadoDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVerificado(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(ApiResponse<string>.Error("ID inválido"));

                _logger.LogInformation($"[OP_TraficoController] GET: Obteniendo verificado {id}");

                var response = await _service.ObtenerVerificadoAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoController] Error en GetVerificado");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener verificado"));
            }
        }

        /// <summary>
        /// GET /api/op/op_trafico/anulados/{id}
        /// Obtiene evento en estado Anulado
        /// </summary>
        [HttpGet("anulados/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OP_TraficoAnuladoDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAnulado(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(ApiResponse<string>.Error("ID inválido"));

                _logger.LogInformation($"[OP_TraficoController] GET: Obteniendo anulado {id}");

                var response = await _service.ObtenerAnuladoAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoController] Error en GetAnulado");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener anulado"));
            }
        }

        // ============================================
        // DASHBOARD Y ESTADÍSTICAS
        // ============================================

        /// <summary>
        /// GET /api/op/op_trafico/dashboard
        /// Obtiene resumen de tráfico
        /// </summary>
        [HttpGet("dashboard")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OP_TraficoDashboardDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDashboard([FromQuery] DateTime? fechaDesde = null, [FromQuery] DateTime? fechaHasta = null)
        {
            try
            {
                _logger.LogInformation("[OP_TraficoController] GET: Obteniendo dashboard");

                var response = await _service.ObtenerDashboardAsync(fechaDesde, fechaHasta);
                return response.Success ? Ok(response) : StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoController] Error en GetDashboard");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener dashboard"));
            }
        }

        // ============================================
        // HISTORIAL Y AUDITORÍA
        // ============================================

        /// <summary>
        /// GET /api/op/op_trafico/{id}/historial
        /// Obtiene historial completo de un evento
        /// </summary>
        [HttpGet("{id}/historial")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<OP_TraficoHistorialDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHistorial(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(ApiResponse<string>.Error("ID de evento inválido"));

                _logger.LogInformation($"[OP_TraficoController] GET: Obteniendo historial para evento {id}");

                var response = await _service.ObtenerHistorialAsync(id);
                return response.Success ? Ok(response) : StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[OP_TraficoController] Error en GetHistorial {id}");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener historial"));
            }
        }
    }
}
