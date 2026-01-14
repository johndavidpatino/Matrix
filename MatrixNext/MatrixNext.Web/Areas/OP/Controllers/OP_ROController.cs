using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Data.Models.OP_RO;
using MatrixNext.Data.Services;

namespace MatrixNext.Web.Areas.OP.Controllers
{
    /// <summary>
    /// Controller REST para Operational Review (OP_RO)
    /// Sprint 11A - Gestión de revisiones operacionales
    /// Workflow: Pendiente → Aprobado/Rechazado
    /// TODO: Implementar servicios IOP_ROService cuando existan
    /// </summary>
    [ApiController]
    [Route("api/[area]/[controller]")]
    [Area("OP")]
    public class OP_ROController : ControllerBase
    {
        private readonly ILogger<OP_ROController> _logger;

        public OP_ROController(ILogger<OP_ROController> logger)
        {
            _logger = logger;
        }

        // ============================================
        // LISTAR REVISIONES
        // ============================================

        /// <summary>
        /// GET /api/op/op_ro
        /// Obtiene listado de revisiones con filtros
        /// </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<OP_ROReviewDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRevisiones([FromQuery] OP_ROFiltrosDTO filtros)
        {
            try
            {
                _logger.LogInformation("[OP_ROController] GET: Obteniendo revisiones");

                // TODO: Implementar cuando exista IOP_ROService
                var resultado = ApiResponse<List<OP_ROReviewDTO>>.Ok(new List<OP_ROReviewDTO>());
                
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROController] Error en GetRevisiones");
                return StatusCode(500, ApiResponse<string>.Error("Error interno"));
            }
        }

        // ============================================
        // OBTENER DETALLE DE REVISIÓN
        // ============================================

        /// <summary>
        /// GET /api/op/op_ro/{id}
        /// Obtiene detalle completo de una revisión
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OP_ROReviewDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRevision(int id)
        {
            try
            {
                _logger.LogInformation($"[OP_ROController] GET: Obteniendo revisión {id}");

                // TODO: Implementar cuando exista IOP_ROService
                return NotFound(ApiResponse<string>.NotFound($"Revisión {id} no encontrada"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[OP_ROController] Error obteniendo revisión {id}");
                return StatusCode(500, ApiResponse<string>.Error("Error interno"));
            }
        }
    }
}
