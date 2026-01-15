using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Data.Services.SGC;
using MatrixNext.Data.DTOs.SGC;

namespace MatrixNext.Web.Areas.SGC.Controllers
{
    /// <summary>
    /// Controller para gestión de Auditorías Internas
    /// Endpoints REST para SGC_Calidad módulo
    /// Requiere autenticación y validación de roles
    /// </summary>
    [Area("SGC")]
    [Authorize]
    [ApiController]
    [Route("api/sgc/[controller]")]
    public class AuditoriasController : ControllerBase
    {
        private readonly ISGCAuditoriaService _auditoriaService;
        private readonly ILogger<AuditoriasController> _logger;

        public AuditoriasController(
            ISGCAuditoriaService auditoriaService,
            ILogger<AuditoriasController> logger)
        {
            _auditoriaService = auditoriaService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener auditorías con filtros
        /// GET: /api/sgc/auditorias?estadoId=20&anoAuditoria=2024&pageSize=10&pageIndex=1
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] byte? estadoId,
            [FromQuery] int? anoAuditoria,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageIndex = 1)
        {
            try
            {
                var userId = long.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value ?? "0");
                var userRoleId = byte.Parse(User.Claims.FirstOrDefault(c => c.Type == "RoleId")?.Value ?? "0");

                if (userId == 0 || userRoleId == 0)
                {
                    return Unauthorized(new { success = false, message = "Usuario o rol no identificado" });
                }

                var auditorias = await _auditoriaService.GetByFilterAsync(
                    estadoId, 
                    anoAuditoria, 
                    pageSize, 
                    pageIndex, 
                    userId, 
                    userRoleId);

                return Ok(new { success = true, data = auditorias });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo auditorías. EstadoId: {EstadoId}, Año: {Ano}", estadoId, anoAuditoria);
                return StatusCode(500, new { success = false, message = "Error al obtener auditorías" });
            }
        }

        /// <summary>
        /// Obtener auditoría por ID
        /// GET: /api/sgc/auditorias/5
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var auditoria = await _auditoriaService.GetByIdAsync(id);

                if (auditoria == null)
                {
                    return NotFound(new { success = false, message = "Auditoría no encontrada" });
                }

                return Ok(new { success = true, data = auditoria });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo auditoría {AuditoriaId}", id);
                return StatusCode(500, new { success = false, message = "Error al obtener la auditoría" });
            }
        }

        /// <summary>
        /// Crear nueva auditoría
        /// POST: /api/sgc/auditorias
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SGCAuditoriaCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new { success = false, message = "Datos inválidos", errors });
                }

                var userId = long.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value ?? "0");

                if (userId == 0)
                {
                    return Unauthorized(new { success = false, message = "Usuario no identificado" });
                }

                var (success, message) = await _auditoriaService.CreateAsync(dto, userId);

                if (!success)
                {
                    return BadRequest(new { success = false, message });
                }

                return Ok(new { success = true, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando auditoría. Dto: {@Dto}", dto);
                return StatusCode(500, new { success = false, message = "Error al crear la auditoría" });
            }
        }

        /// <summary>
        /// Actualizar estado de auditoría
        /// PUT: /api/sgc/auditorias/5/estado
        /// </summary>
        [HttpPut("{id:int}/estado")]
        public async Task<IActionResult> UpdateEstado(
            int id, 
            [FromBody] UpdateEstadoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Datos inválidos" });
                }

                var userId = long.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value ?? "0");

                if (userId == 0)
                {
                    return Unauthorized(new { success = false, message = "Usuario no identificado" });
                }

                var (success, message) = await _auditoriaService.UpdateEstadoAsync(
                    id, 
                    request.NuevoEstadoId, 
                    userId);

                if (!success)
                {
                    return BadRequest(new { success = false, message });
                }

                return Ok(new { success = true, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando estado de auditoría {AuditoriaId} a {EstadoId}", id, request.NuevoEstadoId);
                return StatusCode(500, new { success = false, message = "Error al actualizar el estado" });
            }
        }

        /// <summary>
        /// Crear informe de auditoría
        /// POST: /api/sgc/auditorias/5/informe
        /// </summary>
        [HttpPost("{id:int}/informe")]
        public async Task<IActionResult> CreateInforme(
            int id,
            [FromBody] SGCAuditoriaInformeCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new { success = false, message = "Datos inválidos", errors });
                }

                var userId = long.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value ?? "0");

                if (userId == 0)
                {
                    return Unauthorized(new { success = false, message = "Usuario no identificado" });
                }

                // Asegurar que el ID de auditoría coincida
                if (dto.AuditoriaId != id)
                {
                    return BadRequest(new { success = false, message = "ID de auditoría no coincide" });
                }

                var (success, message) = await _auditoriaService.CreateInformeAsync(dto, userId);

                if (!success)
                {
                    return BadRequest(new { success = false, message });
                }

                return Ok(new { success = true, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando informe para auditoría {AuditoriaId}. Dto: {@Dto}", id, dto);
                return StatusCode(500, new { success = false, message = "Error al crear el informe" });
            }
        }

        /// <summary>
        /// Obtener informe de auditoría
        /// GET: /api/sgc/auditorias/5/informe
        /// </summary>
        [HttpGet("{id:int}/informe")]
        public async Task<IActionResult> GetInforme(int id)
        {
            try
            {
                var informe = await _auditoriaService.GetInformeByIdAsync(id);

                if (informe == null)
                {
                    return NotFound(new { success = false, message = "Informe no encontrado" });
                }

                return Ok(new { success = true, data = informe });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo informe de auditoría {AuditoriaId}", id);
                return StatusCode(500, new { success = false, message = "Error al obtener el informe" });
            }
        }

        /// <summary>
        /// Obtener catálogo de normativas
        /// GET: /api/sgc/auditorias/catalogos/normativas
        /// </summary>
        [HttpGet("catalogos/normativas")]
        public async Task<IActionResult> GetNormativas()
        {
            try
            {
                var normativas = await _auditoriaService.GetNormativasAsync();
                return Ok(new { success = true, data = normativas });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de normativas");
                return StatusCode(500, new { success = false, message = "Error al obtener normativas" });
            }
        }

        /// <summary>
        /// Obtener catálogo de tipos de auditoría
        /// GET: /api/sgc/auditorias/catalogos/tipos-auditoria
        /// </summary>
        [HttpGet("catalogos/tipos-auditoria")]
        public async Task<IActionResult> GetTiposAuditoria()
        {
            try
            {
                var tipos = await _auditoriaService.GetTiposAuditoriaAsync();
                return Ok(new { success = true, data = tipos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de tipos de auditoría");
                return StatusCode(500, new { success = false, message = "Error al obtener tipos de auditoría" });
            }
        }

        /// <summary>
        /// Obtener catálogo de tipos de hallazgo
        /// GET: /api/sgc/auditorias/catalogos/tipos-hallazgo
        /// </summary>
        [HttpGet("catalogos/tipos-hallazgo")]
        public async Task<IActionResult> GetTiposHallazgo()
        {
            try
            {
                var tipos = await _auditoriaService.GetTiposHallazgoAsync();
                return Ok(new { success = true, data = tipos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de tipos de hallazgo");
                return StatusCode(500, new { success = false, message = "Error al obtener tipos de hallazgo" });
            }
        }

        /// <summary>
        /// Obtener catálogo de estados de auditoría
        /// GET: /api/sgc/auditorias/catalogos/estados
        /// </summary>
        [HttpGet("catalogos/estados")]
        public async Task<IActionResult> GetEstados()
        {
            try
            {
                var estados = await _auditoriaService.GetEstadosAsync();
                return Ok(new { success = true, data = estados });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de estados");
                return StatusCode(500, new { success = false, message = "Error al obtener estados" });
            }
        }
    }

    /// <summary>
    /// Request para actualizar estado de auditoría
    /// </summary>
    public class UpdateEstadoRequest
    {
        public byte NuevoEstadoId { get; set; }
    }
}
