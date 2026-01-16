// MatrixNext.Web/Areas/RE_GT/Controllers/GestionTratamientoController.cs

using MatrixNext.Web.Services.RE_GT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.RE_GT.Controllers
{
    /// <summary>
    /// Controller para Gestión y Tratamiento de Datos
    /// Landing page con acceso a operaciones cualitativas, cuantitativas, y reportes
    /// Sprint 17 - Fase 3
    /// </summary>
    [Area("RE_GT")]
    [Authorize]
    [Route("[area]/[controller]/[action]")]
    public class GestionTratamientoController : Controller
    {
        private readonly IRecoleccionDatosService _service;
        private readonly ILogger<GestionTratamientoController> _logger;

        public GestionTratamientoController(IRecoleccionDatosService service, ILogger<GestionTratamientoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// GET /RE_GT/GestionTratamiento/Index
        /// Landing page de Gestión y Tratamiento de Datos
        /// Acceso a operaciones cualitativas, cuantitativas, calidad y tratamiento
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("[GestionTratamiento] Index - Usuario: {User}", User.Identity?.Name);

                var menu = await _service.ObtenerMenuGestionTratamientoAsync();

                if (!menu.TieneAcceso)
                {
                    _logger.LogWarning("[GestionTratamiento] Acceso denegado a usuario {User}", User.Identity?.Name);
                    return Forbid();
                }

                return View(menu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GestionTratamiento] Error en Index para usuario {User}", User.Identity?.Name);
                return BadRequest(new { message = "Error al cargar la página de gestión y tratamiento" });
            }
        }
    }
}
