// MatrixNext.Web/Areas/RE_GT/Controllers/RecoleccionController.cs

using MatrixNext.Web.Services.RE_GT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.RE_GT.Controllers
{
    /// <summary>
    /// Controller para navegación de Recolección de Datos
    /// Landing page con menú de acceso a operaciones de recolección
    /// Sprint 17 - Fase 3
    /// </summary>
    [Area("RE_GT")]
    [Authorize]
    [Route("[area]/[controller]/[action]")]
    public class RecoleccionController : Controller
    {
        private readonly IRecoleccionDatosService _service;
        private readonly ILogger<RecoleccionController> _logger;

        public RecoleccionController(IRecoleccionDatosService service, ILogger<RecoleccionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// GET /RE_GT/Recoleccion/Index
        /// Landing page de Recolección de Datos con menú de navegación
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("[Recoleccion] Index - Usuario: {User}", User.Identity?.Name);

                var menu = await _service.ObtenerMenuRecoleccionAsync();

                if (!menu.TieneAcceso)
                {
                    _logger.LogWarning("[Recoleccion] Acceso denegado a usuario {User}", User.Identity?.Name);
                    return Forbid();
                }

                return View(menu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Recoleccion] Error en Index para usuario {User}", User.Identity?.Name);
                return BadRequest(new { message = "Error al cargar la página de recolección de datos" });
            }
        }
    }
}
