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
    public class CargueDescuentosSsController : Controller
    {
        private readonly ICcProduccionService _service;
        private readonly ILogger<CargueDescuentosSsController> _logger;

        public CargueDescuentosSsController(ICcProduccionService service, 
            ILogger<CargueDescuentosSsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Acceder a Cargue Descuentos SS");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerDescuentos([FromBody] FiltrosCargueDescuentoSSDto filtros)
        {
            try
            {
                var descuentos = await _service.ObtenerDescuentosSsAsync(filtros);
                return Json(new { success = true, data = descuentos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener descuentos");
                return Json(new { success = false, message = "Error al obtener los descuentos. Por favor intente nuevamente." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Exportar([FromQuery] FiltrosCargueDescuentoSSDto filtros)
        {
            try
            {
                var descuentos = await _service.ObtenerDescuentosSsAsync(filtros);
                var excelBytes = await _service.ExportarDescuentosSsExcelAsync(descuentos);

                return File(excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"DescuentosSS_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar descuentos");
                return BadRequest("Error al exportar los descuentos. Por favor intente nuevamente.");
            }
        }
    }
}
