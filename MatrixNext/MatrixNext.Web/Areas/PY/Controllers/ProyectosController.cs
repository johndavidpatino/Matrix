using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Models.PY;
using MatrixNext.Web.Services.PY;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Areas.PY.Controllers
{
    [Area("PY")]
    [Authorize(Roles = "GerenteProyectos")]
    [Route("PY/[controller]/[action]")]
    public class ProyectosController : Controller
    {
        private readonly IProyectosService _service;

        public ProyectosController(IProyectosService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new FiltrosVM());
        }

        [HttpGet]
        public async Task<IActionResult> Grid(FiltrosVM filtros)
        {
            var result = await _service.ListarAsync(filtros);
            return PartialView("_GridTable", result);
        }

        [HttpGet]
        public IActionResult CreateModal()
        {
            return PartialView("_CreateEdit", new Proyecto { Activo = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModal(Proyecto model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateEdit", model);
            }

            var result = await _service.CrearAsync(model);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return PartialView("_CreateEdit", model);
            }

            return Json(new { success = true, message = result.Message });
        }

        [HttpGet]
        public async Task<IActionResult> EditModal(long id)
        {
            var entity = await _service.ObtenerPorIdAsync(id);
            if (entity == null) return NotFound();
            return PartialView("_CreateEdit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditModal(long id, Proyecto model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateEdit", model);
            }

            var result = await _service.ActualizarAsync(model);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return PartialView("_CreateEdit", model);
            }

            return Json(new { success = true, message = result.Message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.EliminarAsync(id);
            return Json(new { success = result.IsSuccess, message = result.Message });
        }
    }
}
