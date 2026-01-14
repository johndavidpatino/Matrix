using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Data.Models.OP_Trafico;
using MatrixNext.Data.Services;

namespace MatrixNext.Web.Areas.OP.Controllers
{
    /// <summary>
    /// Controller REST para Operational Traffic (OP_Trafico)
    /// Sprint 11B - Gestión de tráfico de datos
    /// Workflow: Capturado → Criticado → Verificado → Anulado
    /// TODO: Implementar servicios IOP_TraficoService cuando existan
    /// </summary>
    [ApiController]
    [Route("api/[area]/[controller]")]
    [Area("OP")]
    public class OP_TraficoController : ControllerBase
    {
        private readonly ILogger<OP_TraficoController> _logger;

        public OP_TraficoController(ILogger<OP_TraficoController> logger)
        {
            _logger = logger;
        }

        // ============================================
        // LISTAR EVENTOS
        // ============================================

        /// <summary>
        /// GET /api/op/op_trafico
        /// Obtiene listado de eventos de tráfico
        /// </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<OP_TraficoEventoDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventos([FromQuery] OP_TraficoFiltrosDTO filtros)
        {
            try
            {
                _logger.LogInformation("[OP_TraficoController] GET: Obteniendo eventos");

                // TODO: Implementar cuando exista IOP_TraficoService
                var resultado = ApiResponse<List<OP_TraficoEventoDTO>>.Ok(new List<OP_TraficoEventoDTO>());

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoController] Error en GetEventos");
                return StatusCode(500, ApiResponse<string>.Error("Error interno"));
            }
        }

        // ============================================
        // OBTENER DETALLE DE EVENTO
        // ============================================

        /// <summary>
        /// GET /api/op/op_trafico/{id}
        /// Obtiene detalle completo de un evento
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OP_TraficoEventoDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEvento(int id)
        {
            try
            {
                _logger.LogInformation($"[OP_TraficoController] GET: Obteniendo evento {id}");

                // TODO: Implementar cuando exista IOP_TraficoService
                return NotFound(ApiResponse<string>.NotFound($"Evento {id} no encontrado"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[OP_TraficoController] Error obteniendo evento {id}");
                return StatusCode(500, ApiResponse<string>.Error("Error interno"));
            }
        }
    }
}
