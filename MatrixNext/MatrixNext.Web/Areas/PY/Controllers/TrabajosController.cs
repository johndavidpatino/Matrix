using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatrixNext.Web.Infrastructure.Data;

namespace MatrixNext.Web.Areas.PY.Controllers
{
    [Area("PY")]
    [Route("PY/[controller]/[action]")]
    public class TrabajosController : Controller
    {
        private readonly MatrixDbContext _db;

        public TrabajosController(MatrixDbContext db)
        {
            _db = db;
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
                    t.Nombre.Contains(q) || 
                    t.JobBook.Contains(q) ||
                    t.Id.ToString().Contains(q));
            }

            var items = await query
                .OrderByDescending(t => t.Id)
                .Take(limit)
                .Select(t => new
                {
                    id = t.Id,
                    text = $"{t.JobBook} - {t.Nombre}",
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
                    text = $"{t.JobBook} - {t.Nombre}",
                    jobBook = t.JobBook,
                    nombre = t.Nombre,
                    idProyecto = t.IdProyecto
                })
                .FirstOrDefaultAsync();

            return trabajo != null ? Json(trabajo) : NotFound();
        }
    }
}
