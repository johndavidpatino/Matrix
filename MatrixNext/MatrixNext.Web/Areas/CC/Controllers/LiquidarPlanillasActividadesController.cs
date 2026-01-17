using MatrixNext.Data.Modules.CC.DTOs.Produccion;
using MatrixNext.Data.Modules.CC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MatrixNext.Web.Areas.CC.Controllers
{
    /// <summary>
    /// Controller para Liquidación de Planillas de Actividades
    /// </summary>
    [Area("CC")]
    [Route("CC/[controller]")]
    [Authorize]
    public class LiquidarPlanillasActividadesController : Controller
    {
        private readonly ICcProduccionService _service;
        private readonly ILogger<LiquidarPlanillasActividadesController> _logger;

        public LiquidarPlanillasActividadesController(ICcProduccionService service, 
            ILogger<LiquidarPlanillasActividadesController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// GET: Página principal
        /// </summary>
        public IActionResult Index()
        {
            _logger.LogInformation("Acceder a Liquidación de Planillas");
            return View();
        }

        /// <summary>
        /// POST: Obtener liquidaciones
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ObtenerLiquidaciones([FromBody] FiltrosLiquidacionPlanillaDto filtros)
        {
            try
            {
                var liquidaciones = await _service.ObtenerLiquidacionesAsync(filtros);
                return Json(new { success = true, data = liquidaciones });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener liquidaciones");
                return Json(new { success = false, message = "Error al obtener las liquidaciones. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: Exportar liquidaciones
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Exportar([FromQuery] FiltrosLiquidacionPlanillaDto filtros)
        {
            try
            {
                var liquidaciones = await _service.ObtenerLiquidacionesAsync(filtros);
                var excelBytes = await _service.ExportarLiquidacionesExcelAsync(liquidaciones);

                return File(excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"LiquidacionPlanillas_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar liquidaciones");
                return BadRequest("Error al exportar las liquidaciones. Por favor intente nuevamente.");
            }
        }
    }
}
