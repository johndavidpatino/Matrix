using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.OP.Controllers
{
    /// <summary>
    /// MVC Controller para vistas de Operational Traffic (OP_Trafico)
    /// Sprint 11B - Sirve las vistas HTML, el API está en OP_TraficoController
    /// </summary>
    [Area("OP")]
    [Authorize]
    public class OP_TraficoViewController : Controller
    {
        private readonly ILogger<OP_TraficoViewController> _logger;

        public OP_TraficoViewController(ILogger<OP_TraficoViewController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET: /OP/OP_Trafico/Index
        /// Vista principal del listado de eventos de tráfico
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("[OP_TraficoViewController] Acceso a vista Index");
            return View();
        }

        /// <summary>
        /// GET: /OP/OP_Trafico/Dashboard
        /// Vista del dashboard con estadísticas
        /// </summary>
        [HttpGet]
        public IActionResult Dashboard()
        {
            _logger.LogInformation("[OP_TraficoViewController] Acceso a vista Dashboard");
            return View();
        }

        /// <summary>
        /// GET: /OP/OP_Trafico/Detalle/{id}
        /// Vista de detalle de un evento específico
        /// </summary>
        [HttpGet]
        public IActionResult Detalle(int id)
        {
            _logger.LogInformation($"[OP_TraficoViewController] Acceso a vista Detalle (ID: {id})");
            ViewBag.EventoId = id;
            return View();
        }

        /// <summary>
        /// GET: /OP/OP_Trafico/Capturar
        /// Vista para capturar nuevo evento
        /// </summary>
        [HttpGet]
        public IActionResult Capturar()
        {
            _logger.LogInformation("[OP_TraficoViewController] Acceso a vista Capturar");
            return View();
        }
    }
}
