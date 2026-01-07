using MatrixNext.Data.Services.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.PY.Controllers
{
    /// <summary>
    /// API Controller para obtener catálogo de Unidades
    /// Utilizado por filtros y dropdowns en módulos PY
    /// </summary>
    [Area("PY")]
    [Route("api/py/[controller]")]
    [ApiController]
    [Authorize]
    public class UnidadesController : ControllerBase
    {
        private readonly GrupoUnidadService _grupoUnidadService;
        private readonly ILogger<UnidadesController> _logger;

        public UnidadesController(
            GrupoUnidadService grupoUnidadService,
            ILogger<UnidadesController> logger)
        {
            _grupoUnidadService = grupoUnidadService;
            _logger = logger;
        }

        /// <summary>
        /// [GET] /api/py/unidades
        /// 
        /// Obtiene todas las unidades disponibles en el sistema
        /// 
        /// **Response (200 OK):**
        /// ```json
        /// {
        ///   "success": true,
        ///   "message": "Unidades obtenidas",
        ///   "data": [
        ///     {
        ///       "id": 1,
        ///       "nombre": "Unidad Comercial 1",
        ///       "descripcion": ""
        ///     }
        ///   ]
        /// }
        /// ```
        /// 
        /// **Validaciones:**
        /// - Usuario debe estar autenticado
        /// </summary>
        [HttpGet]
        public IActionResult ObtenerTodas()
        {
            try
            {
                _logger.LogInformation("[Unidades] GET /api/py/unidades");

                var (success, message, data) = _grupoUnidadService.ObtenerTodos();

                return Ok(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Unidades] Error al obtener unidades");
                return StatusCode(500, new
                {
                    success = false,
                    message = $"Error al obtener unidades: {ex.Message}",
                    data = new List<object>()
                });
            }
        }
    }
}
