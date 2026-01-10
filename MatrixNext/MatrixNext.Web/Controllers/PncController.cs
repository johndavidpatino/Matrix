using MatrixNext.Data.Services.Pnc;
using MatrixNext.ViewModels.Pnc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Controllers
{
    /// <summary>
    /// Controller para Gestión de Productos No Conformes (PNC)
    /// Sistema de Gestión de Calidad ISO 9001
    /// Endpoints para CRUD, validaciones y seguimiento
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PncController : ControllerBase
    {
        private readonly IPncService _service;
        private readonly ILogger<PncController> _logger;

        public PncController(IPncService service, ILogger<PncController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ============= CONSULTAS =============

        /// <summary>
        /// Obtener listado de PNC con filtros
        /// GET: api/pnc/listado
        /// </summary>
        [HttpGet("listado")]
        public async Task<ActionResult<ApiResponse<PncFiltrosVM>>> Listado([FromQuery] PncFiltrosVM filtros)
        {
            try
            {
                var (success, data, message) = await _service.ObtenerPnc(filtros);
                
                if (!success)
                    return BadRequest(new ApiResponse<PncFiltrosVM> { Success = false, Message = message });

                return Ok(new ApiResponse<PncFiltrosVM> 
                { 
                    Success = true, 
                    Data = data, 
                    Message = message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GET Listado");
                return StatusCode(500, new ApiResponse<PncFiltrosVM> 
                { 
                    Success = false, 
                    Message = "Error interno del servidor" 
                });
            }
        }

        /// <summary>
        /// Obtener detalle de un PNC con causas y acciones
        /// GET: api/pnc/{id}
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductoNoConformeDetalleVM>>> Detalle(int id)
        {
            try
            {
                var (success, data, message) = await _service.ObtenerPncById(id);
                
                if (!success)
                    return NotFound(new ApiResponse<ProductoNoConformeDetalleVM> 
                    { 
                        Success = false, 
                        Message = message 
                    });

                return Ok(new ApiResponse<ProductoNoConformeDetalleVM> 
                { 
                    Success = true, 
                    Data = data, 
                    Message = message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GET Detalle {Id}", id);
                return StatusCode(500, new ApiResponse<ProductoNoConformeDetalleVM> 
                { 
                    Success = false, 
                    Message = "Error interno del servidor" 
                });
            }
        }

        /// <summary>
        /// Obtener dashboard/seguimiento con KPIs
        /// GET: api/pnc/seguimiento
        /// </summary>
        [HttpGet("seguimiento/dashboard")]
        public async Task<ActionResult<ApiResponse<PncSeguimientoVM>>> Seguimiento()
        {
            try
            {
                var (success, data, message) = await _service.ObtenerSeguimiento();
                
                return Ok(new ApiResponse<PncSeguimientoVM> 
                { 
                    Success = success, 
                    Data = data, 
                    Message = message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GET Seguimiento");
                return StatusCode(500, new ApiResponse<PncSeguimientoVM> 
                { 
                    Success = false, 
                    Message = "Error interno del servidor" 
                });
            }
        }

        /// <summary>
        /// Obtener catálogos para formularios
        /// GET: api/pnc/catalogos
        /// </summary>
        [HttpGet("catalogos")]
        public async Task<ActionResult<ApiResponse<PncCatalogosDto>>> Catalogos()
        {
            try
            {
                var (success, data, message) = await _service.ObtenerCatalogos();
                
                return Ok(new ApiResponse<PncCatalogosDto> 
                { 
                    Success = success, 
                    Data = data, 
                    Message = message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GET Catalogos");
                return StatusCode(500, new ApiResponse<PncCatalogosDto> 
                { 
                    Success = false, 
                    Message = "Error interno del servidor" 
                });
            }
        }

        // ============= CRUD PNC =============

        /// <summary>
        /// Crear nuevo PNC
        /// POST: api/pnc
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Crear([FromBody] CrearPncVM modelo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse<int> 
                    { 
                        Success = false, 
                        Message = "Datos inválidos", 
                        Data = 0 
                    });

                var idUsuario = ObtenerIdUsuarioActual();
                var (success, id, message) = await _service.CrearPnc(modelo, idUsuario);

                if (!success)
                    return BadRequest(new ApiResponse<int> 
                    { 
                        Success = false, 
                        Message = message, 
                        Data = 0 
                    });

                return CreatedAtAction(nameof(Detalle), new { id }, 
                    new ApiResponse<int> 
                    { 
                        Success = true, 
                        Data = id, 
                        Message = message 
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en POST Crear");
                return StatusCode(500, new ApiResponse<int> 
                { 
                    Success = false, 
                    Message = "Error interno del servidor", 
                    Data = 0 
                });
            }
        }

        /// <summary>
        /// Actualizar PNC existente
        /// PUT: api/pnc/{id}
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Actualizar(int id, [FromBody] ProductoNoConformeVM modelo)
        {
            try
            {
                if (!ModelState.IsValid || modelo.Id != id)
                    return BadRequest(new ApiResponse<bool> 
                    { 
                        Success = false, 
                        Message = "Datos inválidos" 
                    });

                var idUsuario = ObtenerIdUsuarioActual();
                var (success, message) = await _service.ActualizarPnc(modelo, idUsuario);

                if (!success)
                    return BadRequest(new ApiResponse<bool> 
                    { 
                        Success = false, 
                        Message = message 
                    });

                return Ok(new ApiResponse<bool> 
                { 
                    Success = true, 
                    Data = true, 
                    Message = message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PUT Actualizar {Id}", id);
                return StatusCode(500, new ApiResponse<bool> 
                { 
                    Success = false, 
                    Message = "Error interno del servidor" 
                });
            }
        }

        /// <summary>
        /// Cerrar PNC (marcar como completado)
        /// POST: api/pnc/{id}/cerrar
        /// </summary>
        [HttpPost("{id}/cerrar")]
        public async Task<ActionResult<ApiResponse<bool>>> Cerrar(int id)
        {
            try
            {
                var idUsuario = ObtenerIdUsuarioActual();
                var (success, message) = await _service.CerrarPnc(id, idUsuario);

                if (!success)
                    return BadRequest(new ApiResponse<bool> 
                    { 
                        Success = false, 
                        Message = message 
                    });

                return Ok(new ApiResponse<bool> 
                { 
                    Success = true, 
                    Data = true, 
                    Message = message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en POST Cerrar {Id}", id);
                return StatusCode(500, new ApiResponse<bool> 
                { 
                    Success = false, 
                    Message = "Error interno del servidor" 
                });
            }
        }

        // ============= CRUD CAUSAS =============

        /// <summary>
        /// Agregar causa a un PNC
        /// POST: api/pnc/{id}/causas
        /// </summary>
        [HttpPost("{id}/causas")]
        public async Task<ActionResult<ApiResponse<int>>> AgregarCausa(int id, [FromBody] AgregarCausaPncVM modelo)
        {
            try
            {
                if (!ModelState.IsValid || modelo.IdPNC != id)
                    return BadRequest(new ApiResponse<int> 
                    { 
                        Success = false, 
                        Message = "Datos inválidos", 
                        Data = 0 
                    });

                var idUsuario = ObtenerIdUsuarioActual();
                var (success, idCausa, message) = await _service.AgregarCausa(modelo, idUsuario);

                if (!success)
                    return BadRequest(new ApiResponse<int> 
                    { 
                        Success = false, 
                        Message = message, 
                        Data = 0 
                    });

                return CreatedAtAction(nameof(Detalle), new { id }, 
                    new ApiResponse<int> 
                    { 
                        Success = true, 
                        Data = idCausa, 
                        Message = message 
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en POST AgregarCausa {Id}", id);
                return StatusCode(500, new ApiResponse<int> 
                { 
                    Success = false, 
                    Message = "Error interno del servidor", 
                    Data = 0 
                });
            }
        }

        /// <summary>
        /// Actualizar causa existente
        /// PUT: api/pnc/causas/{id}
        /// </summary>
        [HttpPut("causas/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> ActualizarCausa(int id, [FromBody] ProductoNoConformeCausaVM modelo)
        {
            try
            {
                if (!ModelState.IsValid || modelo.Id != id)
                    return BadRequest(new ApiResponse<bool> 
                    { 
                        Success = false, 
                        Message = "Datos inválidos" 
                    });

                var (success, message) = await _service.ActualizarCausa(modelo);

                if (!success)
                    return BadRequest(new ApiResponse<bool> 
                    { 
                        Success = false, 
                        Message = message 
                    });

                return Ok(new ApiResponse<bool> 
                { 
                    Success = true, 
                    Data = true, 
                    Message = message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PUT ActualizarCausa {Id}", id);
                return StatusCode(500, new ApiResponse<bool> 
                { 
                    Success = false, 
                    Message = "Error interno del servidor" 
                });
            }
        }

        /// <summary>
        /// Eliminar causa
        /// DELETE: api/pnc/causas/{id}
        /// </summary>
        [HttpDelete("causas/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> EliminarCausa(int id)
        {
            try
            {
                var (success, message) = await _service.EliminarCausa(id);

                if (!success)
                    return BadRequest(new ApiResponse<bool> 
                    { 
                        Success = false, 
                        Message = message 
                    });

                return Ok(new ApiResponse<bool> 
                { 
                    Success = true, 
                    Data = true, 
                    Message = message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en DELETE EliminarCausa {Id}", id);
                return StatusCode(500, new ApiResponse<bool> 
                { 
                    Success = false, 
                    Message = "Error interno del servidor" 
                });
            }
        }

        // ============= CRUD ACCIONES =============

        /// <summary>
        /// Agregar acción a una causa
        /// POST: api/pnc/{id}/acciones
        /// </summary>
        [HttpPost("{id}/acciones")]
        public async Task<ActionResult<ApiResponse<int>>> AgregarAccion(int id, [FromBody] AgregarAccionPncVM modelo)
        {
            try
            {
                if (!ModelState.IsValid || modelo.IdPNC != id)
                    return BadRequest(new ApiResponse<int> 
                    { 
                        Success = false, 
                        Message = "Datos inválidos", 
                        Data = 0 
                    });

                var idUsuario = ObtenerIdUsuarioActual();
                var (success, idAccion, message) = await _service.AgregarAccion(modelo, idUsuario);

                if (!success)
                    return BadRequest(new ApiResponse<int> 
                    { 
                        Success = false, 
                        Message = message, 
                        Data = 0 
                    });

                return CreatedAtAction(nameof(Detalle), new { id }, 
                    new ApiResponse<int> 
                    { 
                        Success = true, 
                        Data = idAccion, 
                        Message = message 
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en POST AgregarAccion {Id}", id);
                return StatusCode(500, new ApiResponse<int> 
                { 
                    Success = false, 
                    Message = "Error interno del servidor", 
                    Data = 0 
                });
            }
        }

        /// <summary>
        /// Actualizar acción existente
        /// PUT: api/pnc/acciones/{id}
        /// </summary>
        [HttpPut("acciones/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> ActualizarAccion(int id, [FromBody] ProductoNoConformeAccionVM modelo)
        {
            try
            {
                if (!ModelState.IsValid || modelo.Id != id)
                    return BadRequest(new ApiResponse<bool> 
                    { 
                        Success = false, 
                        Message = "Datos inválidos" 
                    });

                var (success, message) = await _service.ActualizarAccion(modelo);

                if (!success)
                    return BadRequest(new ApiResponse<bool> 
                    { 
                        Success = false, 
                        Message = message 
                    });

                return Ok(new ApiResponse<bool> 
                { 
                    Success = true, 
                    Data = true, 
                    Message = message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PUT ActualizarAccion {Id}", id);
                return StatusCode(500, new ApiResponse<bool> 
                { 
                    Success = false, 
                    Message = "Error interno del servidor" 
                });
            }
        }

        /// <summary>
        /// Ejecutar/cerrar acción (marcar como completada)
        /// POST: api/pnc/acciones/{id}/ejecutar
        /// </summary>
        [HttpPost("acciones/{id}/ejecutar")]
        public async Task<ActionResult<ApiResponse<bool>>> EjecutarAccion(int id, [FromBody] CerrarAccionPncVM modelo)
        {
            try
            {
                if (!ModelState.IsValid || modelo.IdAccion != id)
                    return BadRequest(new ApiResponse<bool> 
                    { 
                        Success = false, 
                        Message = "Datos inválidos" 
                    });

                var idUsuario = ObtenerIdUsuarioActual();
                var (success, message) = await _service.EjecutarAccion(modelo, idUsuario);

                if (!success)
                    return BadRequest(new ApiResponse<bool> 
                    { 
                        Success = false, 
                        Message = message 
                    });

                return Ok(new ApiResponse<bool> 
                { 
                    Success = true, 
                    Data = true, 
                    Message = message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en POST EjecutarAccion {Id}", id);
                return StatusCode(500, new ApiResponse<bool> 
                { 
                    Success = false, 
                    Message = "Error interno del servidor" 
                });
            }
        }

        /// <summary>
        /// Eliminar acción
        /// DELETE: api/pnc/acciones/{id}
        /// </summary>
        [HttpDelete("acciones/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> EliminarAccion(int id)
        {
            try
            {
                var (success, message) = await _service.EliminarAccion(id);

                if (!success)
                    return BadRequest(new ApiResponse<bool> 
                    { 
                        Success = false, 
                        Message = message 
                    });

                return Ok(new ApiResponse<bool> 
                { 
                    Success = true, 
                    Data = true, 
                    Message = message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en DELETE EliminarAccion {Id}", id);
                return StatusCode(500, new ApiResponse<bool> 
                { 
                    Success = false, 
                    Message = "Error interno del servidor" 
                });
            }
        }

        // ============= VALIDACIONES =============

        /// <summary>
        /// Validar si un PNC puede ser cerrado
        /// GET: api/pnc/{id}/validar-cierre
        /// </summary>
        [HttpGet("{id}/validar-cierre")]
        public async Task<ActionResult<ApiResponse<bool>>> ValidarCierre(int id)
        {
            try
            {
                var (canClose, reason) = await _service.ValidarCierrePnc(id);
                
                return Ok(new ApiResponse<bool> 
                { 
                    Success = canClose, 
                    Data = canClose, 
                    Message = reason 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GET ValidarCierre {Id}", id);
                return StatusCode(500, new ApiResponse<bool> 
                { 
                    Success = false, 
                    Message = "Error interno del servidor" 
                });
            }
        }

        /// <summary>
        /// Validar si una causa tiene acción inmediata
        /// GET: api/pnc/{pncId}/causas/{causaId}/validar-accion-inmediata
        /// </summary>
        [HttpGet("{pncId}/causas/{causaId}/validar-accion-inmediata")]
        public async Task<ActionResult<ApiResponse<bool>>> ValidarAccionInmediata(int pncId, int causaId)
        {
            try
            {
                var (hasImmediate, message) = await _service.ValidarAccionInmediata(pncId, causaId);
                
                return Ok(new ApiResponse<bool> 
                { 
                    Success = hasImmediate, 
                    Data = hasImmediate, 
                    Message = message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GET ValidarAccionInmediata");
                return StatusCode(500, new ApiResponse<bool> 
                { 
                    Success = false, 
                    Message = "Error interno del servidor" 
                });
            }
        }

        // ============= HELPERS =============

        /// <summary>
        /// Obtener ID del usuario actual desde el contexto de autorización
        /// </summary>
        private long ObtenerIdUsuarioActual()
        {
            // TODO: Obtener del token JWT o sesión
            // Por ahora retorna 1 como placeholder
            var claim = User.FindFirst("sub") ?? User.FindFirst("nameidentifier");
            if (long.TryParse(claim?.Value, out var id))
                return id;
            return 1; // Default para testing
        }
    }

    /// <summary>
    /// Response genérico para todas las API
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
