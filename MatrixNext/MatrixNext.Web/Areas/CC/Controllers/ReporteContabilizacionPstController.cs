using MatrixNext.Data.Modules.CC.DTOs.Reportes;
using MatrixNext.Data.Modules.CC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.CC.Controllers
{
    [Area("CC")]
    [Route("CC/[controller]")]
    [Authorize]
    public class ReporteContabilizacionPstController : Controller
    {
        private readonly ICcReportesService _service;
        private readonly ILogger<ReporteContabilizacionPstController> _logger;

        public ReporteContabilizacionPstController(
            ICcReportesService service,
            ILogger<ReporteContabilizacionPstController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerContabilizaciones([FromBody] FiltrosReporteContabilizacionPstDto filtros)
        {
            try
            {
                var data = await _service.ObtenerContabilizacionPstAsync(filtros);
                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo contabilización PST");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Exportar([FromQuery] FiltrosReporteContabilizacionPstDto filtros)
        {
            try
            {
                var data = await _service.ObtenerContabilizacionPstAsync(filtros);
                var excelBytes = await _service.ExportarContabilizacionPstExcelAsync(data);
                var fileName = $"ReporteContabilizacionPST_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                return File(excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exportando contabilización PST");
                return BadRequest(ex.Message);
            }
        }
    }
}
