using MatrixNext.Data.Modules.CC.DTOs;
using MatrixNext.Data.Modules.CC.DTOs.ProcesosInternos;
using MatrixNext.Data.Modules.CC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.CC.Controllers
{
    [Area("CC")]
    [Route("CC/[controller]")]
    [Authorize]
    public class CalculoJornadaLaboralController : Controller
    {
        private readonly ICcProcesosInternosService _service;
        private readonly ILogger<CalculoJornadaLaboralController> _logger;

        public CalculoJornadaLaboralController(
            ICcProcesosInternosService service,
            ILogger<CalculoJornadaLaboralController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> ObtenerJornadas(int? periodo, long? idEmpleado)
        {
            try
            {
                var jornadas = await _service.ObtenerJornadasAsync(periodo, idEmpleado);
                return Json(new { success = true, data = jornadas });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo jornadas");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerAusencias(
            long idEmpleado, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var ausencias = await _service.ObtenerAusenciasEmpleadoAsync(
                    idEmpleado, fechaInicio, fechaFin);
                return Json(new { success = true, data = ausencias });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo ausencias empleado {idEmpleado}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CalcularJornada([FromBody] CalcularJornadaRequest request)
        {
            try
            {
                var id = await _service.CalcularJornadaAsync(request);
                return Json(new { success = true, id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculando jornada");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerResumen(int periodo)
        {
            try
            {
                var resumen = await _service.ObtenerResumenJornadasAsync(periodo);
                return Json(new { success = true, data = resumen });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo resumen periodo {periodo}");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
