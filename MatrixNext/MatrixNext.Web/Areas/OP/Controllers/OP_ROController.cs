using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Data.Models.OP_RO;
using MatrixNext.Data.Services;
using MatrixNext.Data.Services.OP_RO;

namespace MatrixNext.Web.Areas.OP.Controllers
{
    /// <summary>
    /// Controller REST para Operational Review (OP_RO)
    /// Sprint 11 - Gestión de revisiones operacionales
    /// Workflow: Pendiente → Aprobado/Rechazado/Cancelado
    /// Responsable: Coordinar solicitudes, validar autorización, llamar servicio
    /// REGLA: Solo lógica de coordinación (sin BD directa ni reglas de negocio)
    /// </summary>
    [ApiController]
    [Route("api/[area]/[controller]")]
    [Area("OP")]
    public class OP_ROController : ControllerBase
    {
        private readonly IOP_ROService _service;
        private readonly ILogger<OP_ROController> _logger;

        public OP_ROController(IOP_ROService service, ILogger<OP_ROController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger;
        }

        // ============================================
        // REVISIONES - CONSULTAS
        // ============================================

        /// <summary>
        /// GET /api/op/op_ro
        /// Obtiene listado de revisiones con filtros
        /// </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OP_ROResultadoDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRevisiones([FromQuery] OP_ROFiltrosDTO filtros)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.Error("Datos de filtro inválidos"));

                _logger.LogInformation("[OP_ROController] GET: Obteniendo revisiones");

                var response = await _service.ObtenerRevisionesAsync(filtros);
                return response.Success ? Ok(response) : StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROController] Error en GetRevisiones");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener revisiones"));
            }
        }

        /// <summary>
        /// GET /api/op/op_ro/{id}
        /// Obtiene detalle completo de una revisión
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OP_ROSolicitudRevisionDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRevision(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(ApiResponse<string>.Error("ID de revisión inválido"));

                _logger.LogInformation($"[OP_ROController] GET: Obteniendo revisión {id}");

                var response = await _service.ObtenerRevisionDetalleAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[OP_ROController] Error en GetRevision {id}");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener revisión"));
            }
        }

        // ============================================
        // WORKFLOW - APROBACIÓN/RECHAZO
        // ============================================

        /// <summary>
        /// POST /api/op/op_ro/aprobar
        /// Aprueba una revisión
        /// </summary>
        [HttpPost("aprobar")]
        [Authorize(Roles = "Admin,Supervisor,Jefe")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AprobarRevision([FromBody] OP_ROAprobarDTO aprobacion)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.Error("Datos de aprobación inválidos"));

                _logger.LogInformation("[OP_ROController] POST: Aprobando revisión");

                var response = await _service.AprobarRevisionAsync(aprobacion);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROController] Error en AprobarRevision");
                return StatusCode(500, ApiResponse<string>.Error("Error al aprobar revisión"));
            }
        }

        /// <summary>
        /// POST /api/op/op_ro/rechazar
        /// Rechaza una revisión
        /// </summary>
        [HttpPost("rechazar")]
        [Authorize(Roles = "Admin,Supervisor,Jefe")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RechazarRevision([FromBody] OP_RORechazarDTO rechazo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.Error("Datos de rechazo inválidos"));

                _logger.LogInformation("[OP_ROController] POST: Rechazando revisión");

                var response = await _service.RechazarRevisionAsync(rechazo);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROController] Error en RechazarRevision");
                return StatusCode(500, ApiResponse<string>.Error("Error al rechazar revisión"));
            }
        }

        // ============================================
        // CUESTIONARIOS
        // ============================================

        /// <summary>
        /// GET /api/op/op_ro/cuestionarios
        /// Obtiene listado de cuestionarios
        /// </summary>
        [HttpGet("cuestionarios")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<OP_ROCuestionarioDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCuestionarios([FromQuery] OP_ROFiltrosDTO filtros)
        {
            try
            {
                _logger.LogInformation("[OP_ROController] GET: Obteniendo cuestionarios");

                var response = await _service.ObtenerCuestionariosAsync(filtros);
                return response.Success ? Ok(response) : StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROController] Error en GetCuestionarios");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener cuestionarios"));
            }
        }

        /// <summary>
        /// GET /api/op/op_ro/cuestionarios/{id}
        /// Obtiene detalle de cuestionario
        /// </summary>
        [HttpGet("cuestionarios/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OP_ROCuestionarioDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCuestionario(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(ApiResponse<string>.Error("ID inválido"));

                _logger.LogInformation($"[OP_ROController] GET: Obteniendo cuestionario {id}");

                var response = await _service.ObtenerCuestionarioAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROController] Error en GetCuestionario");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener cuestionario"));
            }
        }

        /// <summary>
        /// POST /api/op/op_ro/cuestionarios
        /// Guarda cuestionario
        /// </summary>
        [HttpPost("cuestionarios")]
        [Authorize(Roles = "Admin,Supervisor")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SaveCuestionario([FromBody] OP_ROCuestionarioDTO cuestionario, [FromQuery] int usuarioId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.Error("Datos inválidos"));

                _logger.LogInformation("[OP_ROController] POST: Guardando cuestionario");

                var response = await _service.GuardarCuestionarioAsync(cuestionario, usuarioId);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROController] Error en SaveCuestionario");
                return StatusCode(500, ApiResponse<string>.Error("Error al guardar"));
            }
        }

        // ============================================
        // INSTRUCTIVOS
        // ============================================

        /// <summary>
        /// GET /api/op/op_ro/instructivos
        /// Obtiene listado de instructivos
        /// </summary>
        [HttpGet("instructivos")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<OP_ROInstructivoDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInstructivos([FromQuery] OP_ROFiltrosDTO filtros)
        {
            try
            {
                _logger.LogInformation("[OP_ROController] GET: Obteniendo instructivos");

                var response = await _service.ObtenerInstructivosAsync(filtros);
                return response.Success ? Ok(response) : StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROController] Error en GetInstructivos");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener instructivos"));
            }
        }

        /// <summary>
        /// GET /api/op/op_ro/instructivos/{id}
        /// Obtiene detalle de instructivo
        /// </summary>
        [HttpGet("instructivos/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OP_ROInstructivoDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInstructivo(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(ApiResponse<string>.Error("ID inválido"));

                _logger.LogInformation($"[OP_ROController] GET: Obteniendo instructivo {id}");

                var response = await _service.ObtenerInstructivoAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROController] Error en GetInstructivo");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener instructivo"));
            }
        }

        /// <summary>
        /// POST /api/op/op_ro/instructivos
        /// Guarda instructivo
        /// </summary>
        [HttpPost("instructivos")]
        [Authorize(Roles = "Admin,Supervisor")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SaveInstructivo([FromBody] OP_ROInstructivoDTO instructivo, [FromQuery] int usuarioId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.Error("Datos inválidos"));

                _logger.LogInformation("[OP_ROController] POST: Guardando instructivo");

                var response = await _service.GuardarInstructivoAsync(instructivo, usuarioId);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROController] Error en SaveInstructivo");
                return StatusCode(500, ApiResponse<string>.Error("Error al guardar"));
            }
        }

        // ============================================
        // MATERIALES DE AYUDA
        // ============================================

        /// <summary>
        /// GET /api/op/op_ro/materiales
        /// Obtiene listado de materiales
        /// </summary>
        [HttpGet("materiales")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<OP_ROMaterialAyudaDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMateriales([FromQuery] OP_ROFiltrosDTO filtros)
        {
            try
            {
                _logger.LogInformation("[OP_ROController] GET: Obteniendo materiales");

                var response = await _service.ObtenerMaterialesAsync(filtros);
                return response.Success ? Ok(response) : StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROController] Error en GetMateriales");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener materiales"));
            }
        }

        /// <summary>
        /// GET /api/op/op_ro/materiales/{id}
        /// Obtiene detalle de material
        /// </summary>
        [HttpGet("materiales/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OP_ROMaterialAyudaDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMaterial(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(ApiResponse<string>.Error("ID inválido"));

                _logger.LogInformation($"[OP_ROController] GET: Obteniendo material {id}");

                var response = await _service.ObtenerMaterialAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROController] Error en GetMaterial");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener material"));
            }
        }

        /// <summary>
        /// POST /api/op/op_ro/materiales
        /// Guarda material
        /// </summary>
        [HttpPost("materiales")]
        [Authorize(Roles = "Admin,Supervisor")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SaveMaterial([FromBody] OP_ROMaterialAyudaDTO material, [FromQuery] int usuarioId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.Error("Datos inválidos"));

                _logger.LogInformation("[OP_ROController] POST: Guardando material");

                var response = await _service.GuardarMaterialAsync(material, usuarioId);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROController] Error en SaveMaterial");
                return StatusCode(500, ApiResponse<string>.Error("Error al guardar"));
            }
        }

        // ============================================
        // METODOLOGÍAS
        // ============================================

        /// <summary>
        /// GET /api/op/op_ro/metodologias
        /// Obtiene listado de metodologías
        /// </summary>
        [HttpGet("metodologias")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<OP_ROMetodologiaDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMetodologias([FromQuery] OP_ROFiltrosDTO filtros)
        {
            try
            {
                _logger.LogInformation("[OP_ROController] GET: Obteniendo metodologías");

                var response = await _service.ObtenerMetodologiasAsync(filtros);
                return response.Success ? Ok(response) : StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROController] Error en GetMetodologias");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener metodologías"));
            }
        }

        /// <summary>
        /// GET /api/op/op_ro/metodologias/{id}
        /// Obtiene detalle de metodología
        /// </summary>
        [HttpGet("metodologias/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<OP_ROMetodologiaDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMetodologia(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(ApiResponse<string>.Error("ID inválido"));

                _logger.LogInformation($"[OP_ROController] GET: Obteniendo metodología {id}");

                var response = await _service.ObtenerMetodologiaAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROController] Error en GetMetodologia");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener metodología"));
            }
        }

        /// <summary>
        /// POST /api/op/op_ro/metodologias
        /// Guarda metodología
        /// </summary>
        [HttpPost("metodologias")]
        [Authorize(Roles = "Admin,Supervisor")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SaveMetodologia([FromBody] OP_ROMetodologiaDTO metodologia, [FromQuery] int usuarioId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.Error("Datos inválidos"));

                _logger.LogInformation("[OP_ROController] POST: Guardando metodología");

                var response = await _service.GuardarMetodologiaAsync(metodologia, usuarioId);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROController] Error en SaveMetodologia");
                return StatusCode(500, ApiResponse<string>.Error("Error al guardar"));
            }
        }

    }
}
