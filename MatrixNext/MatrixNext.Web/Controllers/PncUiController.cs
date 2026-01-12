using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MatrixNext.Data.Services.Pnc; // IPncService real
using MatrixNext.Data.Models.ViewModels.Pnc; // ViewModels reales

namespace MatrixNext.Web.Controllers
{
    [Authorize]
    public class PncUiController : Controller
    {
        private readonly IPncService _pncService;
        private readonly ILogger<PncUiController> _logger;

        public PncUiController(IPncService pncService, ILogger<PncUiController> logger)
        {
            _pncService = pncService;
            _logger = logger;
        }

        // GET: /Pnc
        [HttpGet]
        [Route("Pnc")] 
        public IActionResult Index()
        {
            return View("~/MatrixNext.Web/Views/Pnc/Index.cshtml", new PncFiltrosVM());
        }

        // GET: /Pnc/Crear
        [HttpGet]
        [Route("Pnc/Crear")] 
        public IActionResult Crear()
        {
            return View("~/MatrixNext.Web/Views/Pnc/Crear.cshtml");
        }

        // GET: /Pnc/Seguimiento
        [HttpGet]
        [Route("Pnc/Seguimiento")] 
        public IActionResult Seguimiento()
        {
            return View("~/MatrixNext.Web/Views/Pnc/Seguimiento.cshtml");
        }

        // GET: /Pnc/Detalle/{id}
        [HttpGet]
        [Route("Pnc/Detalle/{id:int}")] 
        public async Task<IActionResult> Detalle(int id)
        {
            try
            {
                var resultado = await _pncService.ObtenerPncById(id);
                if (!resultado.success || resultado.data == null)
                {
                    TempData["ErrorMessage"] = resultado.message ?? "PNC no encontrado";
                    return RedirectToAction(nameof(Index));
                }

                return View("~/MatrixNext.Web/Views/Pnc/Detalle.cshtml", resultado.data);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error cargando detalle PNC {Id}", id);
                TempData["ErrorMessage"] = "Ocurrió un error cargando el detalle";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
