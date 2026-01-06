using MatrixNext.Data.Modules.CC.DTOs;
using MatrixNext.Data.Modules.CC.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Areas.CC.Controllers
{
    [Area("CC")]
    public class RequerimientosEquipoController : Controller
    {
        private readonly ICcProcesosInternosService _service;
        private readonly ILogger<RequerimientosEquipoController> _logger;

        public RequerimientosEquipoController(
            ICcProcesosInternosService service,
            ILogger<RequerimientosEquipoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> ObtenerRequerimientos(long? idTrabajo, byte? estado)
        {
            try
            {
                var requerimientos = await _service.ObtenerRequerimientosAsync(idTrabajo, estado);
                return Json(new { success = true, data = requerimientos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo requerimientos");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GenerarMuestra(long idTrabajo)
        {
            try
            {
                var muestra = await _service.GenerarMuestraRequerimientosAsync(idTrabajo);
                return Json(new { success = true, data = muestra });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generando muestra para trabajo {idTrabajo}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarRequerimiento([FromBody] GuardarRequerimientoRequest request)
        {
            try
            {
                var id = await _service.GuardarRequerimientoAsync(request);
                return Json(new { success = true, id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando requerimiento");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarRequerimiento(long id)
        {
            try
            {
                await _service.EliminarRequerimientoAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error eliminando requerimiento {id}");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
