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
    public class EstadoJobBooksController : Controller
    {
        private readonly ICcProduccionService _service;
        private readonly ILogger<EstadoJobBooksController> _logger;

        public EstadoJobBooksController(ICcProduccionService service, 
            ILogger<EstadoJobBooksController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Acceder a Estado JobBooks");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerJobBooks([FromBody] FiltrosEstadoJobBookDto filtros)
        {
            try
            {
                var jobbooks = await _service.ObtenerEstadoJobBooksAsync(filtros);
                return Json(new { success = true, data = jobbooks });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener jobbooks");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Exportar([FromQuery] FiltrosEstadoJobBookDto filtros)
        {
            try
            {
                var jobbooks = await _service.ObtenerEstadoJobBooksAsync(filtros);
                var excelBytes = await _service.ExportarEstadoJobBooksExcelAsync(jobbooks);

                return File(excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"JobBooks_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar jobbooks");
                return BadRequest(ex.Message);
            }
        }
    }
}
