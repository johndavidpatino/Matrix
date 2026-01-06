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
    public class AnulacionLiquidacionesController : Controller
    {
        private readonly ICcProduccionService _service;
        private readonly ILogger<AnulacionLiquidacionesController> _logger;

        public AnulacionLiquidacionesController(ICcProduccionService service, 
            ILogger<AnulacionLiquidacionesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Acceder a Anulación de Liquidaciones");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerAnulaciones([FromBody] FiltrosAnulacionLiquidacionesDto filtros)
        {
            try
            {
                var anulaciones = await _service.ObtenerAnulacionesAsync(filtros);
                return Json(new { success = true, data = anulaciones });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener anulaciones");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Exportar([FromQuery] FiltrosAnulacionLiquidacionesDto filtros)
        {
            try
            {
                var anulaciones = await _service.ObtenerAnulacionesAsync(filtros);
                var excelBytes = await _service.ExportarAnulacionesExcelAsync(anulaciones);

                return File(excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Anulaciones_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar anulaciones");
                return BadRequest(ex.Message);
            }
        }
    }
}
