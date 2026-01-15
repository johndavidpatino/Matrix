using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.Services;

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
        private readonly MatrixDbContext _db;
        private readonly IGridService _grid;

        public TareasController(MatrixDbContext db, IGridService grid)
        {
            _db = db;
            _grid = grid;
        }

        /// <summary>
        /// Lookup para autocompletar tareas desde el catálogo CORE_Tareas
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Lookup(string q = "", int limit = 20)
        {
            var query = _db.Tareas
                .AsNoTracking()
                .Where(t => t.Visible == true);

            if (!string.IsNullOrWhiteSpace(q))
            {
                // Buscar por ID o por nombre
                if (long.TryParse(q, out var id))
                {
                    query = query.Where(t => t.Id == id || t.Nombre.Contains(q));
                }
                else
                {
                    query = query.Where(t => t.Nombre.Contains(q));
                }
            }

            var items = await query
                .OrderBy(t => t.Orden)
                .ThenBy(t => t.Nombre)
                .Take(limit)
                .Select(t => new
                {
                    id = t.Id,
                    text = t.Nombre
                })
                .ToListAsync();

            return Json(items);
        }

        /// <summary>
        /// Obtener tarea por ID desde catálogo CORE_Tareas
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var tarea = await _db.Tareas
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new { id = t.Id, text = t.Nombre })
                .FirstOrDefaultAsync();

            if (tarea == null)
            {
                return NotFound();
            }

            return Json(tarea);
        }
    }
}
