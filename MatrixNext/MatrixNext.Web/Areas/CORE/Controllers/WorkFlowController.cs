using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.Services;
using MatrixNext.Web.Services.CORE;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Areas.CORE.Controllers
{
    [Area("CORE")]
    [Route("CORE/[controller]/[action]")]
    public class WorkFlowController : Controller
    {
        private readonly MatrixDbContext _db;
        private readonly IGridService _grid;
        private readonly IWorkFlowService _service;

        public WorkFlowController(
            MatrixDbContext db, 
            IGridService grid,
            IWorkFlowService service)
        {
            _db = db;
            _grid = grid;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int size = 10, string sortBy = "Id", bool desc = true)
        {
            var query = _db.WorkFlows.AsNoTracking();
            var result = await _grid.PaginarAsync(query, page, size, sortBy, desc);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Grid(int page = 1, int size = 10, string sortBy = "Id", bool desc = true)
        {
            var query = _db.WorkFlows.AsNoTracking();
            var result = await _grid.PaginarAsync(query, page, size, sortBy, desc);
            return PartialView("_GridTable", result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new WorkFlow());
        }

        [HttpGet]
        public IActionResult CreateModal()
        {
            return PartialView("_CreateEdit", new WorkFlow());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkFlow model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers.ContainsKey("X-Requested-With"))
                    return PartialView("_CreateEdit", model);
                return View(model);
            }

            var resultado = await _service.CrearAsync(model);
            if (!resultado.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, resultado.Message);
                if (Request.Headers.ContainsKey("X-Requested-With"))
                    return PartialView("_CreateEdit", model);
                return View(model);
            }

            if (Request.Headers.ContainsKey("X-Requested-With"))
                return Json(new { success = true, message = resultado.Message });
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var entity = await _service.ObtenerPorIdAsync(id);
            if (entity == null) return NotFound();
            return View(entity);
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
        public async Task<IActionResult> Edit(long id, WorkFlow model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                if (Request.Headers.ContainsKey("X-Requested-With"))
                    return PartialView("_CreateEdit", model);
                return View(model);
            }

            var resultado = await _service.ActualizarAsync(model);
            if (!resultado.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, resultado.Message);
                if (Request.Headers.ContainsKey("X-Requested-With"))
                    return PartialView("_CreateEdit", model);
                return View(model);
            }

            if (Request.Headers.ContainsKey("X-Requested-With"))
                return Json(new { success = true, message = resultado.Message });
            return RedirectToAction(nameof(Index));
        }
    }
}
