using MatrixNext.Data.Modules.CC.DTOs.Reportes;
using MatrixNext.Data.Modules.CC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.CC.Controllers
{
    [Area("CC")]
    [Route("CC/[controller]")]
    [Authorize]
    public class ReporteActividadesProduccionController : Controller
    {
        private readonly ICcReportesService _service;
        private readonly ILogger<ReporteActividadesProduccionController> _logger;

        public ReporteActividadesProduccionController(
            ICcReportesService service,
            ILogger<ReporteActividadesProduccionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerActividades([FromBody] FiltrosReporteActividadProduccionDto filtros)
        {
            try
            {
                var actividades = await _service.ObtenerActividadesProduccionAsync(filtros);
                return Json(new { success = true, data = actividades });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo actividades de producción");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Exportar([FromQuery] FiltrosReporteActividadProduccionDto filtros)
        {
            try
            {
                var actividades = await _service.ObtenerActividadesProduccionAsync(filtros);
                var excelBytes = await _service.ExportarActividadesProduccionExcelAsync(actividades);
                var fileName = $"ReporteActividadesProduccion_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                return File(excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exportando actividades de producción");
                return BadRequest(ex.Message);
            }
        }
    }
}
