using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.CORE;
using MatrixNext.Web.ViewModels;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.CORE.Controllers
{
    /// <summary>
    /// Configuración: asignar tareas a tipos de hilo (equivalente a Configuracion_TareasXHilo en WebForms)
    /// </summary>
    [Area("CORE")]
    [Authorize]
    [Route("CORE/[controller]/[action]")]
    public class TareasXHiloController : Controller
    {
        private readonly ITareasPorTipoHiloService _service;

        public TareasXHiloController(ITareasPorTipoHiloService service)
        {
            _service = service;
        }

        private long ObtenerUsuarioActualId()
        {
            var userIdClaim = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return long.TryParse(userIdClaim, out var id) ? id : 0L;
        }

        /// <summary>
        /// Lista tareas asignadas/no asignadas a un tipo de hilo
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> List(long tipoHiloId, bool? asignada = null)
        {
            var result = await _service.ObtenerAsync(tipoHiloId, asignada);
            return Json(new { success = result.IsSuccess, message = result.Message, data = result.Data });
        }

        /// <summary>
        /// Asigna una tarea al tipo de hilo
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Asignar(long tipoHiloId, long tareaId)
        {
            var result = await _service.AsignarAsync(tipoHiloId, tareaId, ObtenerUsuarioActualId());
            return Json(new { success = result.IsSuccess, message = result.Message });
        }

        /// <summary>
        /// Quita la tarea del tipo de hilo
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desasignar(long tipoHiloId, long tareaId)
        {
            var result = await _service.DesasignarAsync(tipoHiloId, tareaId, ObtenerUsuarioActualId());
            return Json(new { success = result.IsSuccess, message = result.Message });
        }
    }
}
