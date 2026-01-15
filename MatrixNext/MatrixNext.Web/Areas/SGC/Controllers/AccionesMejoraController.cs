using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Data.Services.SGC;
using MatrixNext.Data.DTOs.SGC;

namespace MatrixNext.Web.Areas.SGC.Controllers
{
    /// <summary>
    /// Controller para gestión de Acciones de Mejora
    /// Endpoints REST para SGC_Calidad módulo
    /// Requiere autenticación
    /// </summary>
    [Area("SGC")]
    [Authorize]
    [ApiController]
    [Route("api/sgc/acciones-mejora")]
    public class AccionesMejoraController : ControllerBase
    {
        private readonly ISGCAccionMejoraService _accionMejoraService;
        private readonly ILogger<AccionesMejoraController> _logger;

        public AccionesMejoraController(
            ISGCAccionMejoraService accionMejoraService,
            ILogger<AccionesMejoraController> logger)
        {
            _accionMejoraService = accionMejoraService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener acciones de mejora con filtros
        /// GET: /api/sgc/acciones-mejora?procesoId=1&usuarioResponsable=123&pageSize=10&pageIndex=1
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? procesoId,
            [FromQuery] long? usuarioResponsable,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageIndex = 1)
        {
            try
            {
                var acciones = await _accionMejoraService.GetByFilterAsync(
                    procesoId, 
                    usuarioResponsable, 
                    pageSize, 
                    pageIndex);

                return Ok(new { success = true, data = acciones });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo acciones de mejora. ProcesoId: {ProcesoId}, Usuario: {Usuario}", 
                    procesoId, usuarioResponsable);
                return StatusCode(500, new { success = false, message = "Error al obtener acciones de mejora" });
            }
        }

        /// <summary>
        /// Obtener acción de mejora por ID
        /// GET: /api/sgc/acciones-mejora/5
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var accion = await _accionMejoraService.GetByIdAsync(id);

                if (accion == null)
                {
                    return NotFound(new { success = false, message = "Acción de mejora no encontrada" });
                }

                return Ok(new { success = true, data = accion });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo acción de mejora {AccionMejoraId}", id);
                return StatusCode(500, new { success = false, message = "Error al obtener la acción de mejora" });
            }
        }

        /// <summary>
        /// Crear nueva acción de mejora
        /// POST: /api/sgc/acciones-mejora
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SGCAccionMejoraCreateDto dto)
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

                var (success, message) = await _accionMejoraService.CreateAsync(dto, userId);

                if (!success)
                {
                    return BadRequest(new { success = false, message });
                }

                return Ok(new { success = true, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando acción de mejora. Dto: {@Dto}", dto);
                return StatusCode(500, new { success = false, message = "Error al crear la acción de mejora" });
            }
        }

        /// <summary>
        /// Actualizar acción de mejora existente
        /// PUT: /api/sgc/acciones-mejora/5
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] SGCAccionMejoraUpdateDto dto)
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

                // Asegurar que el ID de acción coincida
                if (dto.AccionMejoraId != id)
                {
                    return BadRequest(new { success = false, message = "ID de acción de mejora no coincide" });
                }

                var (success, message) = await _accionMejoraService.UpdateAsync(dto, userId);

                if (!success)
                {
                    return BadRequest(new { success = false, message });
                }

                return Ok(new { success = true, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando acción de mejora {AccionMejoraId}. Dto: {@Dto}", id, dto);
                return StatusCode(500, new { success = false, message = "Error al actualizar la acción de mejora" });
            }
        }

        /// <summary>
        /// Eliminar acción de mejora (soft delete)
        /// DELETE: /api/sgc/acciones-mejora/5
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = long.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value ?? "0");

                if (userId == 0)
                {
                    return Unauthorized(new { success = false, message = "Usuario no identificado" });
                }

                var (success, message) = await _accionMejoraService.DeleteAsync(id, userId);

                if (!success)
                {
                    return BadRequest(new { success = false, message });
                }

                return Ok(new { success = true, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando acción de mejora {AccionMejoraId}", id);
                return StatusCode(500, new { success = false, message = "Error al eliminar la acción de mejora" });
            }
        }

        /// <summary>
        /// Agregar causas a una acción de mejora
        /// POST: /api/sgc/acciones-mejora/5/causas
        /// </summary>
        [HttpPost("{id:int}/causas")]
        public async Task<IActionResult> AddCausas(
            int id,
            [FromBody] List<SGCCausaCreateDto> causas)
        {
            try
            {
                if (!ModelState.IsValid || causas == null || !causas.Any())
                {
                    return BadRequest(new { success = false, message = "Debe proporcionar al menos una causa" });
                }

                var userId = long.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value ?? "0");

                if (userId == 0)
                {
                    return Unauthorized(new { success = false, message = "Usuario no identificado" });
                }

                var (success, message) = await _accionMejoraService.AddCausasAsync(id, causas, userId);

                if (!success)
                {
                    return BadRequest(new { success = false, message });
                }

                return Ok(new { success = true, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error agregando causas a acción de mejora {AccionMejoraId}. Causas: {@Causas}", id, causas);
                return StatusCode(500, new { success = false, message = "Error al agregar causas" });
            }
        }

        /// <summary>
        /// Agregar planes de acción a una acción de mejora
        /// POST: /api/sgc/acciones-mejora/5/planes-accion
        /// </summary>
        [HttpPost("{id:int}/planes-accion")]
        public async Task<IActionResult> AddPlanesAccion(
            int id,
            [FromBody] List<SGCPlanAccionCreateDto> planes)
        {
            try
            {
                if (!ModelState.IsValid || planes == null || !planes.Any())
                {
                    return BadRequest(new { success = false, message = "Debe proporcionar al menos un plan de acción" });
                }

                var userId = long.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value ?? "0");

                if (userId == 0)
                {
                    return Unauthorized(new { success = false, message = "Usuario no identificado" });
                }

                var (success, message) = await _accionMejoraService.AddPlanesAccionAsync(id, planes, userId);

                if (!success)
                {
                    return BadRequest(new { success = false, message });
                }

                return Ok(new { success = true, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error agregando planes de acción a acción de mejora {AccionMejoraId}. Planes: {@Planes}", id, planes);
                return StatusCode(500, new { success = false, message = "Error al agregar planes de acción" });
            }
        }

        /// <summary>
        /// Actualizar plan de acción específico
        /// PUT: /api/sgc/acciones-mejora/planes-accion/5
        /// </summary>
        [HttpPut("planes-accion/{planId:int}")]
        public async Task<IActionResult> UpdatePlanAccion(
            int planId,
            [FromBody] SGCPlanAccionUpdateDto dto)
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

                // Asegurar que el ID del plan coincida
                if (dto.PlanAccionId != planId)
                {
                    return BadRequest(new { success = false, message = "ID de plan de acción no coincide" });
                }

                var (success, message) = await _accionMejoraService.UpdatePlanAccionAsync(dto, userId);

                if (!success)
                {
                    return BadRequest(new { success = false, message });
                }

                return Ok(new { success = true, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando plan de acción {PlanAccionId}. Dto: {@Dto}", planId, dto);
                return StatusCode(500, new { success = false, message = "Error al actualizar el plan de acción" });
            }
        }

        /// <summary>
        /// Obtener catálogo de procesos
        /// GET: /api/sgc/acciones-mejora/catalogos/procesos
        /// </summary>
        [HttpGet("catalogos/procesos")]
        public async Task<IActionResult> GetProcesos()
        {
            try
            {
                var procesos = await _accionMejoraService.GetProcesosAsync();
                return Ok(new { success = true, data = procesos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de procesos");
                return StatusCode(500, new { success = false, message = "Error al obtener procesos" });
            }
        }

        /// <summary>
        /// Obtener catálogo de fuentes de no conformidad
        /// GET: /api/sgc/acciones-mejora/catalogos/fuentes-no-conformidad
        /// </summary>
        [HttpGet("catalogos/fuentes-no-conformidad")]
        public async Task<IActionResult> GetFuentesNoConformidad()
        {
            try
            {
                var fuentes = await _accionMejoraService.GetFuentesNoConformidadAsync();
                return Ok(new { success = true, data = fuentes });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de fuentes de no conformidad");
                return StatusCode(500, new { success = false, message = "Error al obtener fuentes de no conformidad" });
            }
        }

        /// <summary>
        /// Obtener fuentes específicas por tipo de fuente de no conformidad
        /// GET: /api/sgc/acciones-mejora/catalogos/fuentes/1
        /// </summary>
        [HttpGet("catalogos/fuentes/{fuenteNoConformidadId:int}")]
        public async Task<IActionResult> GetFuentesByType(int fuenteNoConformidadId)
        {
            try
            {
                var fuentes = await _accionMejoraService.GetFuentesByTypeAsync(fuenteNoConformidadId);
                return Ok(new { success = true, data = fuentes });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo fuentes por tipo {FuenteNoConformidadId}", fuenteNoConformidadId);
                return StatusCode(500, new { success = false, message = "Error al obtener fuentes" });
            }
        }

        /// <summary>
        /// Obtener planes de acción vencidos o próximos a vencer
        /// GET: /api/sgc/acciones-mejora/planes-accion/vencidos
        /// </summary>
        [HttpGet("planes-accion/vencidos")]
        public async Task<IActionResult> GetPlanesVencidos()
        {
            try
            {
                var planes = await _accionMejoraService.GetPlanesAccionVencidosAsync();
                return Ok(new { success = true, data = planes });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo planes de acción vencidos");
                return StatusCode(500, new { success = false, message = "Error al obtener planes vencidos" });
            }
        }
    }
}
