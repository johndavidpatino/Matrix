using MatrixNext.Data.Modules.CC.DTOs.ProcesosInternos;
using MatrixNext.Data.Modules.CC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.CC.Controllers
{
    /// <summary>
    /// Controller para Resumen de Productividad (Sprint 3 - Fase 1)
    /// </summary>
    [Area("CC")]
    [Route("CC/[controller]")]
    [Authorize]
    public class ResumenProductividadController : Controller
    {
        private readonly ICcProcesosInternosService _service;
        private readonly ILogger<ResumenProductividadController> _logger;

        public ResumenProductividadController(
            ICcProcesosInternosService service,
            ILogger<ResumenProductividadController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// GET: /CC/ResumenProductividad - Página principal
        /// </summary>
        public IActionResult Index()
        {
            _logger.LogInformation("Acceso a Resumen de Productividad");
            return View();
        }

        /// <summary>
        /// POST: /CC/ResumenProductividad/ObtenerResumen - API para listado
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ObtenerResumen([FromBody] FiltrosResumenProductividadDto filtros)
        {
            try
            {
                var resumen = await _service.ObtenerResumenProductividadAsync(filtros);
                return Json(new { success = true, data = resumen });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo resumen de productividad");
                return Json(new { success = false, message = "Error al obtener el resumen de productividad. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: /CC/ResumenProductividad/ObtenerAgregada - Obtener productividad agregada
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerAgregada(
            int? periodo = null, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                var agregada = await _service.ObtenerProductividadAgregadaAsync(periodo, fechaInicio, fechaFin);
                return Json(new { success = true, data = agregada });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo productividad agregada");
                return Json(new { success = false, message = "Error al obtener la productividad agregada. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: /CC/ResumenProductividad/Exportar - Exportar a Excel
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Exportar([FromQuery] FiltrosResumenProductividadDto filtros)
        {
            try
            {
                var resumen = await _service.ObtenerResumenProductividadAsync(filtros);
                var excelBytes = await _service.ExportarResumenProductividadExcelAsync(resumen);

                var fileName = $"ResumenProductividad_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                
                return File(
                    excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exportando resumen de productividad");
                return BadRequest("Error al exportar el resumen de productividad. Por favor intente nuevamente.");
            }
        }
    }
}
