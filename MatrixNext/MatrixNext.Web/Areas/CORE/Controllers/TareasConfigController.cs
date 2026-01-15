using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatrixNext.Web.Infrastructure.Data;
using System.Security.Claims;
using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.ViewModels;
using MatrixNext.Web.Services;

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
        private readonly MatrixDbContext _db;
        private readonly IGridService _grid;
        private readonly IAuditoriaService _auditoria;

        public TareasConfigController(
            MatrixDbContext db, 
            IGridService grid,
            IAuditoriaService auditoria)
        {
            _db = db;
            _grid = grid;
            _auditoria = auditoria;
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
            var query = _db.Tareas.AsNoTracking();

            // Filtro por búsqueda
            if (!string.IsNullOrWhiteSpace(filtros?.Busqueda))
            {
                query = query.Where(t => t.Nombre.Contains(filtros.Busqueda));
            }

            // Ordenar por Orden, luego por Nombre
            query = query.OrderBy(t => t.Orden).ThenBy(t => t.Nombre);

            var resultado = await _grid.PaginarAsync(
                query,
                filtros?.PageNumber ?? 1,
                filtros?.PageSize ?? 20
            );

            return View(resultado);
        }

        /// <summary>
        /// Grid parcial para refrescar después de cambios
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Grid(FiltrosVM? filtros)
        {
            var query = _db.Tareas.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filtros?.Busqueda))
            {
                query = query.Where(t => t.Nombre.Contains(filtros.Busqueda));
            }

            query = query.OrderBy(t => t.Orden).ThenBy(t => t.Nombre);

            var resultado = await _grid.PaginarAsync(
                query,
                filtros?.PageNumber ?? 1,
                filtros?.PageSize ?? 20
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

            try
            {
                // Validar nombre único
                var existe = await _db.Tareas.AnyAsync(t => t.Nombre == model.Nombre);
                if (existe)
                {
                    ModelState.AddModelError("Nombre", "Ya existe una tarea con este nombre");
                    return PartialView("_CreateEdit", model);
                }

                model.FechaCreacion = DateTime.Now;
                model.UsuarioCreacion = ObtenerUsuarioActualId();

                _db.Tareas.Add(model);
                await _db.SaveChangesAsync();

                // Auditoría
                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "CORE_Tareas",
                    EntidadId = model.Id,
                    Accion = "CREATE",
                    Detalles = $"Tarea creada: {model.Nombre}",
                    RutaArchivo = ""
                });

                return Json(new { success = true, message = "Tarea creada exitosamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al crear tarea: {ex.Message}" });
            }
        }

        /// <summary>
        /// Modal para editar tarea existente
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> EditModal(long id)
        {
            var tarea = await _db.Tareas.FindAsync(id);
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

            try
            {
                var tarea = await _db.Tareas.FindAsync(id);
                if (tarea == null)
                {
                    return Json(new { success = false, message = "Tarea no encontrada" });
                }

                // Validar nombre único (excepto el actual)
                var existe = await _db.Tareas.AnyAsync(t => t.Nombre == model.Nombre && t.Id != id);
                if (existe)
                {
                    ModelState.AddModelError("Nombre", "Ya existe una tarea con este nombre");
                    return PartialView("_CreateEdit", model);
                }

                // Actualizar campos
                tarea.Nombre = model.Nombre;
                tarea.NoEmpiezaAntesDe = model.NoEmpiezaAntesDe;
                tarea.NoTerminaAntesDe = model.NoTerminaAntesDe;
                tarea.TiempoPromedioDias = model.TiempoPromedioDias;
                tarea.RequiereEstimacion = model.RequiereEstimacion;
                tarea.RolEstima = model.RolEstima;
                tarea.UnidadEjecuta = model.UnidadEjecuta;
                tarea.UnidadRecibe = model.UnidadRecibe;
                tarea.RolEjecuta = model.RolEjecuta;
                tarea.Visible = model.Visible;
                tarea.Orden = model.Orden;
                tarea.FechaModificacion = DateTime.Now;
                tarea.UsuarioModificacion = ObtenerUsuarioActualId();

                await _db.SaveChangesAsync();

                // Auditoría
                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "CORE_Tareas",
                    EntidadId = tarea.Id,
                    Accion = "UPDATE",
                    Detalles = $"Tarea actualizada: {tarea.Nombre}",
                    RutaArchivo = ""
                });

                return Json(new { success = true, message = "Tarea actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al actualizar tarea: {ex.Message}" });
            }
        }

        /// <summary>
        /// Eliminar tarea del catálogo
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var tarea = await _db.Tareas.FindAsync(id);
                if (tarea == null)
                {
                    return Json(new { success = false, message = "Tarea no encontrada" });
                }

                // Validar que no esté siendo usada en WorkFlows
                var enUso = await _db.WorkFlows.AnyAsync(w => w.IdTarea == id);
                if (enUso)
                {
                    return Json(new { success = false, message = "No se puede eliminar la tarea porque está siendo usada en WorkFlows" });
                }

                _db.Tareas.Remove(tarea);
                await _db.SaveChangesAsync();

                // Auditoría
                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "CORE_Tareas",
                    EntidadId = id,
                    Accion = "DELETE",
                    Detalles = $"Tarea eliminada: {tarea.Nombre}",
                    RutaArchivo = ""
                });

                return Json(new { success = true, message = "Tarea eliminada exitosamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al eliminar tarea: {ex.Message}" });
            }
        }
    }
}
