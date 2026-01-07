using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.PY;
using MatrixNext.Web.Services;
using MatrixNext.Web.Services.PY;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Areas.PY.Controllers
{
    [Area("PY")]
    [Authorize(Roles = "GerenteProyectos,Coordinador")]
    [Route("PY/[controller]/[action]")]
    public class TrabajosController : Controller
    {
        private readonly MatrixDbContext _db;
        private readonly ITrabajosService _service;
        private readonly ITrabajosWorkFlowService _workflow;
        private readonly IEmailService _email;

        public TrabajosController(MatrixDbContext db, ITrabajosService service, ITrabajosWorkFlowService workflow, IEmailService email)
        {
            _db = db;
            _service = service;
            _workflow = workflow;
            _email = email;
        }

        [HttpGet]
        public IActionResult Index(long? idProyecto = null)
        {
            ViewBag.IdProyecto = idProyecto;
            return View(new FiltrosVM());
        }

        [HttpGet]
        public async Task<IActionResult> Grid(FiltrosVM filtros, long? idProyecto = null)
        {
            var result = await _service.ListarAsync(filtros, idProyecto);
            return PartialView("_GridTable", result);
        }

        [HttpGet]
        public IActionResult CreateModal(long? idProyecto = null)
        {
            var model = new Trabajo
            {
                IdProyecto = idProyecto ?? 0,
                Estado = 1,
                Activo = true
            };
            return PartialView("_CreateEdit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModal(Trabajo model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateEdit", model);
            }

            var result = await _workflow.CrearTrabajoConWorkFlowAsync(model);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return PartialView("_CreateEdit", model);
            }

            // Notificación email (best-effort, no bloqueo)
            _ = _email.EnviarAsync(
                destinatario: Request.HttpContext.User.Identity?.Name ?? string.Empty,
                asunto: $"Trabajo creado: {model.Nombre}",
                cuerpo: $"Se ha creado el trabajo '{model.Nombre}' en el proyecto {model.IdProyecto}."
            );

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
        public async Task<IActionResult> EditModal(long id, Trabajo model)
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

        [HttpGet]
        public async Task<IActionResult> DuplicateModal(long id)
        {
            var entity = await _service.ObtenerPorIdAsync(id);
            if (entity == null) return NotFound();
            ViewBag.IdOriginal = id;
            return PartialView("_Duplicate", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duplicate(long id, string? nombre, string? jobBook)
        {
            var result = await _service.DuplicarAsync(id, nombre, jobBook);
            return Json(new { success = result.IsSuccess, message = result.Message });
        }

        /// <summary>
        /// Lookup para autocompletar trabajos (IdTrabajo)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Lookup(string q = "", int limit = 20)
        {
            var query = _db.Trabajos
                .AsNoTracking()
                .Where(t => t.Estado != 11); // Excluir anulados

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(t => 
                    (t.Nombre ?? string.Empty).Contains(q) || 
                    (t.JobBook ?? string.Empty).Contains(q) ||
                    t.Id.ToString().Contains(q));
            }

            var items = await query
                .OrderByDescending(t => t.Id)
                .Take(limit)
                .Select(t => new
                {
                    id = t.Id,
                    text = $"{t.JobBook ?? string.Empty} - {t.Nombre ?? string.Empty}",
                    jobBook = t.JobBook,
                    nombre = t.Nombre
                })
                .ToListAsync();

            return Json(items);
        }

        /// <summary>
        /// Obtener trabajo por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var trabajo = await _db.Trabajos
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    text = $"{t.JobBook ?? string.Empty} - {t.Nombre ?? string.Empty}",
                    jobBook = t.JobBook,
                    nombre = t.Nombre,
                    idProyecto = t.IdProyecto
                })
                .FirstOrDefaultAsync();

            return trabajo != null ? Json(trabajo) : NotFound();
        }
    }
}
