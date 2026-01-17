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
    public class ConteoTrabajosController : Controller
    {
        private readonly ICcProcesosInternosService _service;
        private readonly ILogger<ConteoTrabajosController> _logger;

        public ConteoTrabajosController(
            ICcProcesosInternosService service,
            ILogger<ConteoTrabajosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> ObtenerConteos(
            long? idTrabajo, DateTime? fechaInicio, DateTime? fechaFin)
        {
            try
            {
                var conteos = await _service.ObtenerConteosAsync(idTrabajo, fechaInicio, fechaFin);
                return Json(new { success = true, data = conteos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo conteos");
                return Json(new { success = false, message = "Error al obtener los conteos. Por favor intente nuevamente." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerActividadesPorTrabajo(long idTrabajo)
        {
            try
            {
                var actividades = await _service.ObtenerActividadesPorTrabajoAsync(idTrabajo);
                return Json(new { success = true, data = actividades });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo actividades para trabajo {idTrabajo}");
                return Json(new { success = false, message = "Error al obtener las actividades. Por favor intente nuevamente." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarConteo([FromBody] GuardarConteoRequest request)
        {
            try
            {
                var id = await _service.GuardarConteoAsync(request);
                return Json(new { success = true, id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando conteo");
                return Json(new { success = false, message = "Error al guardar el conteo. Por favor intente nuevamente." });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarConteo(long id)
        {
            try
            {
                await _service.EliminarConteoAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error eliminando conteo {id}");
                return Json(new { success = false, message = "Error al eliminar el conteo. Por favor intente nuevamente." });
            }
        }
    }
}
