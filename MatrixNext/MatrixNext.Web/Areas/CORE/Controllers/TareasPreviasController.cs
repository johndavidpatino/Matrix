using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.Services.CORE;
using System.Linq;

namespace MatrixNext.Web.Areas.CORE.Controllers
{
    [Area("CORE")]
    [Authorize]
    [Route("CORE/[controller]/[action]")]
    public class TareasPreviasController : Controller
    {
        private readonly ITareasPreviasService _service;

        public TareasPreviasController(
            ITareasPreviasService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = (await _service.ObtenerTodasAsync())
                .OrderByDescending(x => x.Id)
                .ToList();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Grid()
        {
            var list = (await _service.ObtenerTodasAsync())
                .OrderByDescending(x => x.Id)
                .ToList();
            return PartialView("_GridTable", list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TareaPrevia());
        }

        [HttpGet]
        public IActionResult CreateModal()
        {
            return PartialView("_Create", new TareaPrevia());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TareaPrevia model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers.ContainsKey("X-Requested-With"))
                    return PartialView("_Create", model);
                return View(model);
            }

            var resultado = await _service.CrearAsync(model);
            if (!resultado.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, resultado.Message);
                if (Request.Headers.ContainsKey("X-Requested-With"))
                    return PartialView("_Create", model);
                return View(model);
            }

            if (Request.Headers.ContainsKey("X-Requested-With"))
                return Json(new { success = true, message = resultado.Message });
            return RedirectToAction(nameof(Index));
        }
    }
}
