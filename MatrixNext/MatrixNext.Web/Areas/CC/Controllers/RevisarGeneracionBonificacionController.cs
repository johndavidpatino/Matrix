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
    public class RevisarGeneracionBonificacionController : Controller
    {
        private readonly ICcProduccionService _service;
        private readonly ILogger<RevisarGeneracionBonificacionController> _logger;

        public RevisarGeneracionBonificacionController(ICcProduccionService service, 
            ILogger<RevisarGeneracionBonificacionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Acceder a Revisar Bonificaciones");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerBonificaciones([FromBody] FiltrosRevisarGeneracionBonificacionDto filtros)
        {
            try
            {
                var bonificaciones = await _service.ObtenerRevisarBonificacionesAsync(filtros);
                return Json(new { success = true, data = bonificaciones });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener bonificaciones para revisión");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Exportar([FromQuery] FiltrosRevisarGeneracionBonificacionDto filtros)
        {
            try
            {
                var bonificaciones = await _service.ObtenerRevisarBonificacionesAsync(filtros);
                var excelBytes = await _service.ExportarRevisarBonificacionesExcelAsync(bonificaciones);

                return File(excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"RevisionBonificaciones_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar bonificaciones");
                return BadRequest(ex.Message);
            }
        }
    }
}
