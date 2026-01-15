using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.CORE;
using System.Linq;

namespace MatrixNext.Web.Areas.CORE.Controllers
{
    /// <summary>
    /// Catálogo de tareas CORE (tipo de tareas configurables)
    /// Ref: MIGRACION_CORE.md Fase 1 – Configuración
    /// </summary>
    [Area("CORE")]
    [Authorize]
    [Route("CORE/[controller]/[action]")]
    public class TareasController : Controller
    {
        private readonly ITareasService _tareasService;

        public TareasController(ITareasService tareasService)
        {
            _tareasService = tareasService;
        }

        /// <summary>
        /// Lookup para autocompletar tareas desde el catálogo CORE_Tareas
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Lookup(string q = "", int limit = 20)
        {
            var items = await _tareasService.BuscarLookupAsync(q, limit);
            var response = items.Select(t => new { id = t.Id, text = t.Nombre });

            return Json(response);
        }

        /// <summary>
        /// Obtener tarea por ID desde catálogo CORE_Tareas
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var tarea = await _tareasService.ObtenerPorIdAsync(id);

            if (tarea == null)
            {
                return NotFound();
            }

            return Json(new { id = tarea.Id, text = tarea.Nombre });
        }
    }
}
