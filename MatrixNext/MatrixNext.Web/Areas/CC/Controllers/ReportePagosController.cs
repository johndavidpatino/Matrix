using MatrixNext.Data.Modules.CC.DTOs.Reportes;
using MatrixNext.Data.Modules.CC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.CC.Controllers
{
    [Area("CC")]
    [Route("CC/[controller]")]
    [Authorize]
    public class ReportePagosController : Controller
    {
        private readonly ICcReportesService _service;
        private readonly ILogger<ReportePagosController> _logger;

        public ReportePagosController(ICcReportesService service, ILogger<ReportePagosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerPagos([FromBody] FiltrosReportePagosDto filtros)
        {
            try
            {
                var pagos = await _service.ObtenerPagosAsync(filtros);
                return Json(new { success = true, data = pagos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reporte de pagos");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Exportar([FromQuery] FiltrosReportePagosDto filtros)
        {
            try
            {
                var pagos = await _service.ObtenerPagosAsync(filtros);
                var excelBytes = await _service.ExportarPagosExcelAsync(pagos);
                var fileName = $"ReportePagos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                return File(excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exportando reporte de pagos");
                return BadRequest(ex.Message);
            }
        }
    }
}
