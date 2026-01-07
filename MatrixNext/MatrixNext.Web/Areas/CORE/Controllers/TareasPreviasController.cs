using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.Services;

namespace MatrixNext.Web.Areas.CORE.Controllers
{
    [Area("CORE")]
    [Route("CORE/[controller]/[action]")]
    public class TareasPreviasController : Controller
    {
        private readonly MatrixDbContext _db;
        private readonly GrafoAciclicoService _grafo;

        public TareasPreviasController(MatrixDbContext db, GrafoAciclicoService grafo)
        {
            _db = db;
            _grafo = grafo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = await _db.TareasPrevias
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .ToListAsync();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TareaPrevía());
        }

        [HttpGet]
        public IActionResult CreateModal()
        {
            return PartialView("_Create", new TareaPrevía());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TareaPrevía model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers.ContainsKey("X-Requested-With"))
                    return PartialView("_Create", model);
                return View(model);
            }

            // Validar grafo acíclico simulando inserción
            var actuales = await _db.TareasPrevias.AsNoTracking().ToListAsync();
            actuales.Add(new TareaPrevía
            {
                IdTarea = model.IdTarea,
                IdTareaPreviaRequerida = model.IdTareaPreviaRequerida,
                Orden = model.Orden
            });

            // Grafo: aristas TareaPreviaRequerida -> Tarea
            var esAciclico = _grafo.ValidarNoCiclos(
                actuales,
                getId: x => x.IdTarea,
                getIdPrevia: x => x.IdTareaPreviaRequerida
            );

            if (!esAciclico)
            {
                ModelState.AddModelError(string.Empty, "La relación crea un ciclo de dependencias.");
                if (Request.Headers.ContainsKey("X-Requested-With"))
                    return PartialView("_Create", model);
                return View(model);
            }

            _db.TareasPrevias.Add(model);
            await _db.SaveChangesAsync();
            if (Request.Headers.ContainsKey("X-Requested-With"))
                return Json(new { success = true, message = "Relación creada" });
            return RedirectToAction(nameof(Index));
        }
    }
}
