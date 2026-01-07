using MatrixNext.Web.Services.PY;
using MatrixNext.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.PY.Controllers
{
    /// <summary>
    /// API Controller para gestión de asignaciones y reasignaciones de proyectos a gerentes
    /// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T5 (Asignaciones & Reasignaciones)
    /// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 1.2 (AsignacionProyectos.aspx)
    /// </summary>
    [Area("PY")]
    [Route("api/py/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador,Gerente")]
    public class AsignacionesProyectosController : ControllerBase
    {
        private readonly IAsignacionesProyectosService _asignacionesService;
        private readonly ILogger<AsignacionesProyectosController> _logger;

        public AsignacionesProyectosController(
            IAsignacionesProyectosService asignacionesService,
            ILogger<AsignacionesProyectosController> logger)
        {
            _asignacionesService = asignacionesService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene el ID del usuario autenticado desde los claims
        /// </summary>
        private long ObtenerIdUsuarioActual()
        {
            var idUsuarioStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (long.TryParse(idUsuarioStr, out long idUsuario))
                return idUsuario;

            throw new InvalidOperationException("No se pudo obtener el ID del usuario actual");
        }

        /// <summary>
        /// [GET] /api/py/asignacionesproyectos/obtener-para-asignar?idUnidad=1
        /// 
        /// Obtiene proyectos sin gerente asignado para una unidad específica
        /// 
        /// **Query Parameters:**
        /// - idUnidad (int, required): ID de la unidad
        /// 
        /// **Response (200 OK):**
        /// ```json
        /// {
        ///   "IsSuccess": true,
        ///   "Data": [
        ///     {
        ///       "id": 123,
        ///       "nombre": "Estudio Market Research 2024",
        ///       "jobBook": "JB-2024-001",
        ///       "unidadId": 1,
        ///       "estado": 1,
        ///       "gerenteProyectosActual": null,
        ///       "nombreGerente": null
        ///     }
        ///   ],
        ///   "Message": "Se encontraron 5 proyectos sin asignar"
        /// }
        /// ```
        /// 
        /// **Validaciones:**
        /// - Usuario debe estar autenticado
        /// - Usuario debe tener rol Administrador o Gerente
        /// - idUnidad debe ser válido
        /// </summary>
        [HttpGet("obtener-para-asignar")]
        public async Task<ActionResult<ResultVM<List<dynamic>>>> ObtenerProyectosParaAsignar([FromQuery] int idUnidad)
        {
            try
            {
                _logger.LogInformation($"[AsignacionesProyectos] GET obtener-para-asignar?idUnidad={idUnidad}");

                var idUsuario = ObtenerIdUsuarioActual();
                var result = await _asignacionesService.ObtenerProyectosXAsignarAsync(idUnidad, idUsuario);

                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AsignacionesProyectos] Error: {ex.Message}");
                return BadRequest(new ResultVM<List<dynamic>>
                {
                    IsSuccess = false,
                    Data = new List<dynamic>(),
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// [GET] /api/py/asignacionesproyectos/obtener-para-reasignar?idUnidad=1&filtroNombre=
        /// 
        /// Obtiene proyectos con gerente asignado para reasignación
        /// 
        /// **Query Parameters:**
        /// - idUnidad (int, required): ID de la unidad
        /// - filtroNombre (string, optional): Filtro por nombre o JobBook del proyecto
        /// 
        /// **Response (200 OK):**
        /// ```json
        /// {
        ///   "IsSuccess": true,
        ///   "Data": [
        ///     {
        ///       "id": 125,
        ///       "nombre": "Estudio Behavioral Analysis",
        ///       "jobBook": "JB-2024-003",
        ///       "unidadId": 1,
        ///       "estado": 1,
        ///       "gerenteProyectosActual": 456,
        ///       "nombreGerente": "Usuario 456"
        ///     }
        ///   ],
        ///   "Message": "Se encontraron 3 proyectos para reasignar"
        /// }
        /// ```
        /// 
        /// **Validaciones:**
        /// - Usuario debe estar autenticado
        /// - Usuario debe tener rol Administrador o Gerente
        /// </summary>
        [HttpGet("obtener-para-reasignar")]
        public async Task<ActionResult<ResultVM<List<dynamic>>>> ObtenerProyectosParaReasignar([FromQuery] int idUnidad, [FromQuery] string? filtroNombre = null)
        {
            try
            {
                _logger.LogInformation($"[AsignacionesProyectos] GET obtener-para-reasignar?idUnidad={idUnidad}&filtroNombre={filtroNombre}");

                var idUsuario = ObtenerIdUsuarioActual();
                var result = await _asignacionesService.ObtenerProyectosXReasignarAsync(idUnidad, filtroNombre, idUsuario);

                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AsignacionesProyectos] Error: {ex.Message}");
                return BadRequest(new ResultVM<List<dynamic>>
                {
                    IsSuccess = false,
                    Data = new List<dynamic>(),
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// [GET] /api/py/asignacionesproyectos/obtener-gerentes?idUnidad=1
        /// 
        /// Obtiene lista de gerentes disponibles para asignación en una unidad
        /// 
        /// **Query Parameters:**
        /// - idUnidad (int, required): ID de la unidad
        /// 
        /// **Response (200 OK):**
        /// ```json
        /// {
        ///   "IsSuccess": true,
        ///   "Data": [
        ///     {
        ///       "id": 456,
        ///       "nombre": "Gerente Demo 1",
        ///       "activo": true
        ///     }
        ///   ],
        ///   "Message": "Se encontraron 5 gerentes disponibles"
        /// }
        /// ```
        /// </summary>
        [HttpGet("obtener-gerentes")]
        public async Task<ActionResult<ResultVM<List<dynamic>>>> ObtenerGerentesDisponibles([FromQuery] int idUnidad)
        {
            try
            {
                _logger.LogInformation($"[AsignacionesProyectos] GET obtener-gerentes?idUnidad={idUnidad}");

                var idUsuario = ObtenerIdUsuarioActual();
                var result = await _asignacionesService.ObtenerGerentesDisponiblesAsync(idUnidad, idUsuario);

                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AsignacionesProyectos] Error: {ex.Message}");
                return BadRequest(new ResultVM<List<dynamic>>
                {
                    IsSuccess = false,
                    Data = new List<dynamic>(),
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// [POST] /api/py/asignacionesproyectos/asignar
        /// 
        /// Asigna un gerente a un proyecto (asignación inicial)
        /// 
        /// **Body (JSON):**
        /// ```json
        /// {
        ///   "idProyecto": 123,
        ///   "idGerenteProyecto": 456,
        ///   "observaciones": "Asignación por cambio organizacional"
        /// }
        /// ```
        /// 
        /// **Response (200 OK):**
        /// ```json
        /// {
        ///   "IsSuccess": true,
        ///   "Data": true,
        ///   "Message": "Gerente asignado exitosamente"
        /// }
        /// ```
        /// 
        /// **Response (400 Bad Request):**
        /// ```json
        /// {
        ///   "IsSuccess": false,
        ///   "Data": false,
        ///   "Message": "Proyecto no encontrado"
        /// }
        /// ```
        /// 
        /// **Validaciones:**
        /// - Usuario debe estar autenticado
        /// - Usuario debe tener rol Administrador o Gerente
        /// - Proyecto debe existir
        /// - Proyecto no debe tener gerente asignado
        /// - Gerente debe ser válido
        /// 
        /// **Auditoría:**
        /// - Registra CreatedBy = ID usuario actual
        /// - Registra CreatedAt = fecha/hora actual
        /// - Crea registro en AsignacionProyecto con tipo "Inicial"
        /// </summary>
        [HttpPost("asignar")]
        public async Task<ActionResult<ResultVM<bool>>> AsignarGerente([FromBody] AsignarGerenteRequest request)
        {
            try
            {
                _logger.LogInformation($"[AsignacionesProyectos] POST asignar (IdProyecto={request.IdProyecto}, IdGerenteProyecto={request.IdGerenteProyecto})");

                if (request == null || request.IdProyecto <= 0 || request.IdGerenteProyecto <= 0)
                {
                    return BadRequest(new ResultVM<bool>
                    {
                        IsSuccess = false,
                        Data = false,
                        Message = "ID Proyecto e ID Gerente son requeridos y deben ser mayores a 0"
                    });
                }

                var idUsuario = ObtenerIdUsuarioActual();
                var result = await _asignacionesService.AsignarGerenteAsync(
                    request.IdProyecto,
                    request.IdGerenteProyecto,
                    idUsuario,
                    request.Observaciones);

                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AsignacionesProyectos] Error: {ex.Message}");
                return BadRequest(new ResultVM<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// [POST] /api/py/asignacionesproyectos/reasignar
        /// 
        /// Reasigna un gerente a un proyecto (cambio de gerente)
        /// 
        /// **Body (JSON):**
        /// ```json
        /// {
        ///   "idProyecto": 123,
        ///   "idGerenteNuevo": 789,
        ///   "observaciones": "Reasignación debido a disponibilidad"
        /// }
        /// ```
        /// 
        /// **Response (200 OK):**
        /// ```json
        /// {
        ///   "IsSuccess": true,
        ///   "Data": true,
        ///   "Message": "Gerente reasignado exitosamente"
        /// }
        /// ```
        /// 
        /// **Response (400 Bad Request):**
        /// ```json
        /// {
        ///   "IsSuccess": false,
        ///   "Data": false,
        ///   "Message": "Proyecto no encontrado"
        /// }
        /// ```
        /// 
        /// **Validaciones:**
        /// - Usuario debe estar autenticado
        /// - Usuario debe tener rol Administrador o Gerente
        /// - Proyecto debe existir
        /// - Proyecto debe tener gerente asignado
        /// - Nuevo gerente debe ser diferente al actual
        /// 
        /// **Auditoría:**
        /// - Registra CreatedBy = ID usuario actual
        /// - Registra CreatedAt = fecha/hora actual
        /// - Crea registro en AsignacionProyecto con tipo "Reasignación"
        /// - Preserva ID gerente anterior (IdGerentePrevio)
        /// </summary>
        [HttpPost("reasignar")]
        public async Task<ActionResult<ResultVM<bool>>> ReasignarGerente([FromBody] ReasignarGerenteRequest request)
        {
            try
            {
                _logger.LogInformation($"[AsignacionesProyectos] POST reasignar (IdProyecto={request.IdProyecto}, IdGerenteNuevo={request.IdGerenteNuevo})");

                if (request == null || request.IdProyecto <= 0 || request.IdGerenteNuevo <= 0)
                {
                    return BadRequest(new ResultVM<bool>
                    {
                        IsSuccess = false,
                        Data = false,
                        Message = "ID Proyecto e ID Gerente Nuevo son requeridos y deben ser mayores a 0"
                    });
                }

                var idUsuario = ObtenerIdUsuarioActual();
                var result = await _asignacionesService.ReasignarGerenteAsync(
                    request.IdProyecto,
                    request.IdGerenteNuevo,
                    idUsuario,
                    request.Observaciones);

                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AsignacionesProyectos] Error: {ex.Message}");
                return BadRequest(new ResultVM<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// [GET] /api/py/asignacionesproyectos/historial/{idProyecto}
        /// 
        /// Obtiene historial de asignaciones de un proyecto
        /// 
        /// **Path Parameters:**
        /// - idProyecto (long): ID del proyecto
        /// 
        /// **Response (200 OK):**
        /// ```json
        /// {
        ///   "IsSuccess": true,
        ///   "Data": [
        ///     {
        ///       "id": 1,
        ///       "idProyecto": 123,
        ///       "idGerenteProyecto": 456,
        ///       "nombreGerenteProyecto": "Usuario 456",
        ///       "fechaAsignacion": "2024-01-15T10:30:00Z",
        ///       "tipoAsignacion": "Inicial",
        ///       "observaciones": "Primera asignación",
        ///       "idGerentePrevio": null,
        ///       "nombreGerentePrevio": null
        ///     }
        ///   ],
        ///   "Message": "Se encontraron 2 registros de asignación"
        /// }
        /// ```
        /// </summary>
        [HttpGet("historial/{idProyecto}")]
        public async Task<ActionResult<ResultVM<List<Models.PY.AsignacionProyecto>>>> ObtenerHistorial(long idProyecto)
        {
            try
            {
                _logger.LogInformation($"[AsignacionesProyectos] GET historial/{idProyecto}");

                var idUsuario = ObtenerIdUsuarioActual();
                var result = await _asignacionesService.ObtenerHistorialAsync(idProyecto, idUsuario);

                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AsignacionesProyectos] Error: {ex.Message}");
                return BadRequest(new ResultVM<List<Models.PY.AsignacionProyecto>>
                {
                    IsSuccess = false,
                    Data = new List<Models.PY.AsignacionProyecto>(),
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// [GET] /api/py/asignacionesproyectos/validar-permisos
        /// 
        /// Valida si el usuario actual tiene permisos para asignar/reasignar
        /// 
        /// **Response (200 OK):**
        /// ```json
        /// {
        ///   "IsSuccess": true,
        ///   "Data": true,
        ///   "Message": "Usuario tiene permisos suficientes"
        /// }
        /// ```
        /// </summary>
        [HttpGet("validar-permisos")]
        public async Task<ActionResult<ResultVM<bool>>> ValidarPermisos()
        {
            try
            {
                _logger.LogInformation($"[AsignacionesProyectos] GET validar-permisos");

                var idUsuario = ObtenerIdUsuarioActual();
                var result = await _asignacionesService.ValidarPermisosAsync(idUsuario);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AsignacionesProyectos] Error: {ex.Message}");
                return BadRequest(new ResultVM<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }
    }

    // ===== Request DTOs =====
    public class AsignarGerenteRequest
    {
        public long IdProyecto { get; set; }
        public long IdGerenteProyecto { get; set; }
        public string? Observaciones { get; set; }
    }

    public class ReasignarGerenteRequest
    {
        public long IdProyecto { get; set; }
        public long IdGerenteNuevo { get; set; }
        public string? Observaciones { get; set; }
    }
}
