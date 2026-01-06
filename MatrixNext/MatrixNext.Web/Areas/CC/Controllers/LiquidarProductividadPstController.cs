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
    public class LiquidarProductividadPstController : Controller
    {
        private readonly ICcProduccionService _service;
        private readonly ILogger<LiquidarProductividadPstController> _logger;

        public LiquidarProductividadPstController(ICcProduccionService service, 
            ILogger<LiquidarProductividadPstController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Acceder a Liquidar Productividad PST");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerLiquidaciones([FromBody] FiltrosLiquidacionProductividadPstDto filtros)
        {
            try
            {
                var liquidaciones = await _service.ObtenerLiquidacionesPstAsync(filtros);
                return Json(new { success = true, data = liquidaciones });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener liquidaciones PST");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Exportar([FromQuery] FiltrosLiquidacionProductividadPstDto filtros)
        {
            try
            {
                var liquidaciones = await _service.ObtenerLiquidacionesPstAsync(filtros);
                var excelBytes = await _service.ExportarLiquidacionesPstExcelAsync(liquidaciones);

                return File(excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"LiquidacionPST_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar liquidaciones PST");
                return BadRequest(ex.Message);
            }
        }
    }
}
