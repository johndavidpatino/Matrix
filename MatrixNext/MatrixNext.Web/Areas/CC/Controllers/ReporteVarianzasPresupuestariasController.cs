using MatrixNext.Data.Modules.CC.DTOs.Reportes;
using MatrixNext.Data.Modules.CC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.CC.Controllers
{
    [Area("CC")]
    [Route("CC/[controller]")]
    [Authorize]
    public class ReporteVarianzasPresupuestariasController : Controller
    {
        private readonly ICcReportesService _service;
        private readonly ILogger<ReporteVarianzasPresupuestariasController> _logger;

        public ReporteVarianzasPresupuestariasController(
            ICcReportesService service,
            ILogger<ReporteVarianzasPresupuestariasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerVarianzas([FromBody] FiltrosReporteVarianzaPresupuestariaDto filtros)
        {
            try
            {
                var data = await _service.ObtenerVarianzasPresupuestariasAsync(filtros);
                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo varianzas presupuestarias");
                return Json(new { success = false, message = "Error al obtener las varianzas presupuestarias. Por favor intente nuevamente." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Exportar([FromQuery] FiltrosReporteVarianzaPresupuestariaDto filtros)
        {
            try
            {
                var data = await _service.ObtenerVarianzasPresupuestariasAsync(filtros);
                var excelBytes = await _service.ExportarVarianzasPresupuestariasExcelAsync(data);
                var fileName = $"ReporteVarianzasPresupuestarias_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                return File(excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exportando varianzas presupuestarias");
                return BadRequest("Error al exportar las varianzas presupuestarias. Por favor intente nuevamente.");
            }
        }
    }
}
