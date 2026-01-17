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
    public class AsignacionCostosPstController : Controller
    {
        private readonly ICcProduccionService _service;
        private readonly ILogger<AsignacionCostosPstController> _logger;

        public AsignacionCostosPstController(ICcProduccionService service, 
            ILogger<AsignacionCostosPstController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Acceder a Asignación Costos PST");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerAsignaciones([FromBody] FiltrosAsignacionCostosPstDto filtros)
        {
            try
            {
                var asignaciones = await _service.ObtenerAsignacionesCostosAsync(filtros);
                return Json(new { success = true, data = asignaciones });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener asignaciones");
                return Json(new { success = false, message = "Error al obtener las asignaciones de costos. Por favor intente nuevamente." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Exportar([FromQuery] FiltrosAsignacionCostosPstDto filtros)
        {
            try
            {
                var asignaciones = await _service.ObtenerAsignacionesCostosAsync(filtros);
                var excelBytes = await _service.ExportarAsignacionesCostosExcelAsync(asignaciones);

                return File(excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"AsignacionCostos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar asignaciones");
                return BadRequest("Error al exportar las asignaciones de costos. Por favor intente nuevamente.");
            }
        }
    }
}
