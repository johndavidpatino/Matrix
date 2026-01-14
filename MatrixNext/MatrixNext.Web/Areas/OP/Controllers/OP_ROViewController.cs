using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.OP.Controllers
{
    /// <summary>
    /// MVC Controller para vistas de Operational Review (OP_RO)
    /// Sprint 11A - Sirve las vistas HTML, el API está en OP_ROController
    /// </summary>
    [Area("OP")]
    [Authorize]
    public class OP_ROViewController : Controller
    {
        private readonly ILogger<OP_ROViewController> _logger;

        public OP_ROViewController(ILogger<OP_ROViewController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET: /OP/OP_RO/Index
        /// Vista principal del listado de revisiones
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("[OP_ROViewController] Acceso a vista Index");
            return View();
        }

        /// <summary>
        /// GET: /OP/OP_RO/Detalle/{id}
        /// Vista de detalle de una revisión específica
        /// </summary>
        [HttpGet]
        public IActionResult Detalle(int id)
        {
            _logger.LogInformation($"[OP_ROViewController] Acceso a vista Detalle (ID: {id})");
            ViewBag.ReviewId = id;
            return View();
        }

        /// <summary>
        /// GET: /OP/OP_RO/Crear
        /// Vista para crear nueva revisión
        /// </summary>
        [HttpGet]
        public IActionResult Crear()
        {
            _logger.LogInformation("[OP_ROViewController] Acceso a vista Crear");
            return View();
        }
    }
}
