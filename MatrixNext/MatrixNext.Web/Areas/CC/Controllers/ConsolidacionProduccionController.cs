using MatrixNext.Data.Modules.CC.DTOs;
using MatrixNext.Data.Modules.CC.DTOs.ProcesosInternos;
using MatrixNext.Data.Modules.CC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.CC.Controllers
{
    [Area("CC")]
    [Route("CC/[controller]")]
    [Authorize]
    public class ConsolidacionProduccionController : Controller
    {
        private readonly ICcProcesosInternosService _service;
        private readonly ILogger<ConsolidacionProduccionController> _logger;

        public ConsolidacionProduccionController(
            ICcProcesosInternosService service,
            ILogger<ConsolidacionProduccionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> ObtenerProduccionPendiente(int? periodo, long? idTrabajo)
        {
            try
            {
                var produccion = await _service.ObtenerProduccionPendienteAsync(periodo, idTrabajo);
                return Json(new { success = true, data = produccion });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo producción pendiente");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConsolidarProduccion([FromBody] ConsolidarProduccionRequest request)
        {
            try
            {
                await _service.ConsolidarProduccionAsync(request);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consolidando producción");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerResumen(int periodo)
        {
            try
            {
                var resumen = await _service.ObtenerResumenConsolidacionAsync(periodo);
                return Json(new { success = true, data = resumen });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo resumen periodo {periodo}");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
