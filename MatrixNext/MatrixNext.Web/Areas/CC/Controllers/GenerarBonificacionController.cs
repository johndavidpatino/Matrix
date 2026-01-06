using MatrixNext.Data.Modules.CC.DTOs.Produccion;
using MatrixNext.Data.Modules.CC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MatrixNext.Web.Areas.CC.Controllers
{
    [Area("CC")]
    [Route("CC/[controller]")]
    [Authorize]
    public class GenerarBonificacionController : Controller
    {
        private readonly ICcProduccionService _service;
        private readonly ILogger<GenerarBonificacionController> _logger;

        public GenerarBonificacionController(ICcProduccionService service, 
            ILogger<GenerarBonificacionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Acceder a Generar Bonificación");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerBonificaciones([FromBody] FiltrosGenerarBonificacionDto filtros)
        {
            try
            {
                var bonificaciones = await _service.ObtenerBonificacionesAsync(filtros);
                return Json(new { success = true, data = bonificaciones });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener bonificaciones");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Exportar([FromQuery] FiltrosGenerarBonificacionDto filtros)
        {
            try
            {
                var bonificaciones = await _service.ObtenerBonificacionesAsync(filtros);
                var excelBytes = await _service.ExportarBonificacionesExcelAsync(bonificaciones);

                return File(excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Bonificaciones_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar bonificaciones");
                return BadRequest(ex.Message);
            }
        }
    }
}
