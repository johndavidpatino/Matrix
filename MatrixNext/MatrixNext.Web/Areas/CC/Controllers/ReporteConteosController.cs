using MatrixNext.Data.Modules.CC.DTOs.ProcesosInternos;
using MatrixNext.Data.Modules.CC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.CC.Controllers
{
    /// <summary>
    /// Controller para Reporte de Conteos de Trabajos (Sprint 3 - Fase 1)
    /// </summary>
    [Area("CC")]
    [Route("CC/[controller]")]
    [Authorize]
    public class ReporteConteosController : Controller
    {
        private readonly ICcProcesosInternosService _service;
        private readonly ILogger<ReporteConteosController> _logger;

        public ReporteConteosController(
            ICcProcesosInternosService service,
            ILogger<ReporteConteosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// GET: /CC/ReporteConteos - Página principal
        /// </summary>
        public IActionResult Index()
        {
            _logger.LogInformation("Acceso a Reporte de Conteos");
            return View();
        }

        /// <summary>
        /// POST: /CC/ReporteConteos/ObtenerReporte - API para listado
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ObtenerReporte([FromBody] FiltrosReporteConteoDto filtros)
        {
            try
            {
                var conteos = await _service.ObtenerReporteConteosAsync(filtros);
                return Json(new { success = true, data = conteos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reporte de conteos");
                return Json(new { success = false, message = "Error al obtener el reporte de conteos. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: /CC/ReporteConteos/ObtenerTotales - Obtener totales agregados
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerTotales(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                var totales = await _service.ObtenerTotalesConteosAsync(fechaInicio, fechaFin);
                return Json(new { success = true, data = totales });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo totales");
                return Json(new { success = false, message = "Error al obtener los totales. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: /CC/ReporteConteos/Exportar - Exportar a Excel
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Exportar([FromQuery] FiltrosReporteConteoDto filtros)
        {
            try
            {
                var conteos = await _service.ObtenerReporteConteosAsync(filtros);
                var excelBytes = await _service.ExportarReporteConteosExcelAsync(conteos);

                var fileName = $"ReporteConteos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                
                return File(
                    excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exportando reporte de conteos");
                return BadRequest("Error al exportar el reporte. Por favor intente nuevamente.");
            }
        }
    }
}
