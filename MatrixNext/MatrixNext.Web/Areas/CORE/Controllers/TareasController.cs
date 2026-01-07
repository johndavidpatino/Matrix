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
        /// Lookup para autocompletar tareas (IdTarea)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Lookup(string q = "", int limit = 20)
        {
            // Placeholder: cuando exista tabla CORE_Tareas, consultar aquí
            // Por ahora devolver mock basado en WorkFlow.IdTarea (valores únicos)
            var query = _db.WorkFlows
                .AsNoTracking()
                .Select(w => w.IdTarea)
                .Distinct();

            if (!string.IsNullOrWhiteSpace(q) && long.TryParse(q, out var idTarea))
            {
                query = query.Where(id => id == idTarea);
            }

            var items = await query
                .OrderBy(id => id)
                .Take(limit)
                .Select(id => new
                {
                    id = id,
                    text = $"Tarea #{id}" // TODO: nombre real desde CORE_Tareas
                })
                .ToListAsync();

            return Json(items);
        }

        /// <summary>
        /// Obtener tarea por ID (placeholder hasta tener catálogo real)
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            // TODO: consultar CORE_Tareas cuando exista
            return Json(new { id = id, text = $"Tarea #{id}" });
        }
    }
}
