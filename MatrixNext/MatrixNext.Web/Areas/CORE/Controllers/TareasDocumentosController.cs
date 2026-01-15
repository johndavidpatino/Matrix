using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.CORE;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.CORE.Controllers
{
    /// <summary>
    /// Configuración: documentos requeridos por tarea (Configuracion_Tareas_Documentos)
    /// </summary>
    [Area("CORE")]
    [Authorize]
    [Route("CORE/[controller]/[action]")]
    public class TareasDocumentosController : Controller
    {
        private readonly ITareasDocumentosService _service;

        public TareasDocumentosController(ITareasDocumentosService service)
        {
            _service = service;
        }

        private long ObtenerUsuarioActualId()
        {
            var userIdClaim = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return long.TryParse(userIdClaim, out var id) ? id : 0L;
        }

        [HttpGet]
        public async Task<IActionResult> List(long tareaId, short tipoDocumentoTareaId, bool? asignado = null)
        {
            var result = await _service.ObtenerAsync(tareaId, tipoDocumentoTareaId, asignado);
            return Json(new { success = result.IsSuccess, message = result.Message, data = result.Data });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Asignar(long tareaId, long documentoId, short tipoDocumentoTareaId, bool esOpcional = false)
        {
            var result = await _service.AsignarAsync(tareaId, documentoId, tipoDocumentoTareaId, esOpcional, ObtenerUsuarioActualId());
            return Json(new { success = result.IsSuccess, message = result.Message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desasignar(long tareaId, long documentoId, short tipoDocumentoTareaId)
        {
            var result = await _service.DesasignarAsync(tareaId, documentoId, tipoDocumentoTareaId, ObtenerUsuarioActualId());
            return Json(new { success = result.IsSuccess, message = result.Message });
        }
    }
}
