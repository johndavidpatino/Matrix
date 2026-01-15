using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.ViewModels;
using MatrixNext.Web.Services.CORE;

namespace MatrixNext.Web.Areas.CORE.Controllers
{
    /// <summary>
    /// Configuración del catálogo de tipos de tareas (CORE_Tareas)
    /// Ref: MIGRACION_CORE.md § Fase 1 - Configuración
    /// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T1.3
    /// </summary>
    [Area("CORE")]
    [Authorize]
    [Route("CORE/Configuracion/[controller]/[action]")]
    public class TareasConfigController : Controller
    {
        private readonly ITareasService _tareasService;

        public TareasConfigController(
            ITareasService tareasService)
        {
            _tareasService = tareasService;
        }

        private long ObtenerUsuarioActualId()
        {
            var userIdClaim = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return long.TryParse(userIdClaim, out var id) ? id : 0L;
        }

        /// <summary>
        /// Lista de tipos de tareas (catálogo)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(FiltrosVM? filtros)
        {
            var resultado = await _tareasService.ObtenerPaginadoAsync(
                filtros ?? new FiltrosVM { PageSize = 20, PageNumber = 1 }
            );

            return View(resultado);
        }

        /// <summary>
        /// Grid parcial para refrescar después de cambios
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Grid(FiltrosVM? filtros)
        {
            var resultado = await _tareasService.ObtenerPaginadoAsync(
                filtros ?? new FiltrosVM { PageSize = 20, PageNumber = 1 }
            );

            return PartialView("_GridTable", resultado);
        }

        /// <summary>
        /// Modal para crear nueva tarea
        /// </summary>
        [HttpGet]
        public IActionResult CreateModal()
        {
            return PartialView("_CreateEdit", new Tarea());
        }

        /// <summary>
        /// Guardar nueva tarea
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModal(Tarea model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateEdit", model);
            }

            var resultado = await _tareasService.CrearAsync(model, ObtenerUsuarioActualId());

            if (!resultado.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, resultado.Message);
                return PartialView("_CreateEdit", model);
            }

            return Json(new { success = true, message = resultado.Message });
        }

        /// <summary>
        /// Modal para editar tarea existente
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> EditModal(long id)
        {
            var tarea = await _tareasService.ObtenerPorIdAsync(id);
            if (tarea == null)
            {
                return NotFound();
            }

            return PartialView("_CreateEdit", tarea);
        }

        /// <summary>
        /// Actualizar tarea existente
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditModal(long id, Tarea model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return PartialView("_CreateEdit", model);
            }

            var resultado = await _tareasService.ActualizarAsync(id, model, ObtenerUsuarioActualId());

            if (!resultado.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, resultado.Message);
                return PartialView("_CreateEdit", model);
            }

            return Json(new { success = true, message = resultado.Message });
        }

        /// <summary>
        /// Eliminar tarea del catálogo
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var resultado = await _tareasService.EliminarAsync(id, ObtenerUsuarioActualId());
            return Json(new { success = resultado.IsSuccess, message = resultado.Message });
        }
    }
}
