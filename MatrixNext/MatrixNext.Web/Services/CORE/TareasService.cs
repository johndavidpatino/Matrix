using Microsoft.EntityFrameworkCore;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services.CORE
{
    /// <summary>
    /// Servicio de dominio para el catálogo CORE_Tareas
    /// Responsable de validaciones, auditoría y paginación
    /// </summary>
    public interface ITareasService
    {
        Task<PaginationResultVM<Tarea>> ObtenerPaginadoAsync(FiltrosVM filtros);
        Task<IEnumerable<Tarea>> BuscarLookupAsync(string? termino, int limit = 20);
        Task<Tarea?> ObtenerPorIdAsync(long id);
        Task<ResultVM<Tarea>> CrearAsync(Tarea model, long usuarioId);
        Task<ResultVM<Tarea>> ActualizarAsync(long id, Tarea model, long usuarioId);
        Task<ResultVM<bool>> EliminarAsync(long id, long usuarioId);
    }

    public class TareasService : ITareasService
    {
        private readonly MatrixDbContext _db;
        private readonly IGridService _grid;
        private readonly IAuditoriaService _auditoria;
        private readonly ILogger<TareasService> _logger;

        public TareasService(
            MatrixDbContext db,
            IGridService grid,
            IAuditoriaService auditoria,
            ILogger<TareasService> logger)
        {
            _db = db;
            _grid = grid;
            _auditoria = auditoria;
            _logger = logger;
        }

        public async Task<PaginationResultVM<Tarea>> ObtenerPaginadoAsync(FiltrosVM filtros)
        {
            filtros ??= new FiltrosVM();
            var query = _db.Tareas.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filtros.Busqueda))
            {
                query = query.Where(t => t.Nombre.Contains(filtros.Busqueda));
            }

            // Asegurar orden consistente por Orden (luego por Id para estabilidad)
            query = query.OrderBy(t => t.Orden).ThenBy(t => t.Id);

            var sortBy = string.IsNullOrWhiteSpace(filtros.SortBy) || filtros.SortBy.Equals("FechaCreacion", StringComparison.OrdinalIgnoreCase)
                ? nameof(Tarea.Orden)
                : filtros.SortBy;

            var sortDescending = sortBy.Equals(nameof(Tarea.Orden), StringComparison.OrdinalIgnoreCase)
                ? false
                : filtros.SortDescending;

            return await _grid.PaginarAsync(
                query,
                filtros.PageNumber,
                filtros.PageSize,
                sortBy: sortBy,
                sortDescending: sortDescending
            );
        }

        public async Task<IEnumerable<Tarea>> BuscarLookupAsync(string? termino, int limit = 20)
        {
            limit = Math.Clamp(limit, 1, 100);

            var query = _db.Tareas
                .AsNoTracking()
                .Where(t => t.Visible == true);

            if (!string.IsNullOrWhiteSpace(termino))
            {
                if (long.TryParse(termino, out var id))
                {
                    query = query.Where(t => t.Id == id || t.Nombre.Contains(termino));
                }
                else
                {
                    query = query.Where(t => t.Nombre.Contains(termino));
                }
            }

            return await query
                .OrderBy(t => t.Orden)
                .ThenBy(t => t.Nombre)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<Tarea?> ObtenerPorIdAsync(long id)
        {
            return await _db.Tareas
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<ResultVM<Tarea>> CrearAsync(Tarea model, long usuarioId)
        {
            try
            {
                var existe = await _db.Tareas.AnyAsync(t => t.Nombre == model.Nombre);
                if (existe)
                {
                    return ResultVM<Tarea>.Fail("Ya existe una tarea con este nombre");
                }

                model.FechaCreacion = DateTime.UtcNow;
                model.FechaModificacion = DateTime.UtcNow;
                model.UsuarioCreacion = usuarioId;
                model.UsuarioModificacion = usuarioId;

                _db.Tareas.Add(model);
                await _db.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "CORE_Tareas",
                    EntidadId = model.Id,
                    Accion = "CREATE",
                    Detalles = $"Tarea creada: {model.Nombre}",
                    RutaArchivo = string.Empty,
                    IdUsuario = usuarioId
                });

                return ResultVM<Tarea>.Ok(model, "Tarea creada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando tarea {Nombre}", model.Nombre);
                return ResultVM<Tarea>.Fail("Error al crear la tarea. Por favor intente nuevamente.");
            }
        }

        public async Task<ResultVM<Tarea>> ActualizarAsync(long id, Tarea model, long usuarioId)
        {
            var tarea = await _db.Tareas.FirstOrDefaultAsync(t => t.Id == id);
            if (tarea == null)
            {
                return ResultVM<Tarea>.Fail("Tarea no encontrada");
            }

            var existe = await _db.Tareas.AnyAsync(t => t.Nombre == model.Nombre && t.Id != id);
            if (existe)
            {
                return ResultVM<Tarea>.Fail("Ya existe una tarea con este nombre");
            }

            try
            {
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
                tarea.FechaModificacion = DateTime.UtcNow;
                tarea.UsuarioModificacion = usuarioId;

                await _db.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "CORE_Tareas",
                    EntidadId = tarea.Id,
                    Accion = "UPDATE",
                    Detalles = $"Tarea actualizada: {tarea.Nombre}",
                    RutaArchivo = string.Empty,
                    IdUsuario = usuarioId
                });

                return ResultVM<Tarea>.Ok(tarea, "Tarea actualizada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando tarea {Id}", id);
                return ResultVM<Tarea>.Fail("Error al actualizar la tarea. Por favor intente nuevamente.");
            }
        }

        public async Task<ResultVM<bool>> EliminarAsync(long id, long usuarioId)
        {
            var tarea = await _db.Tareas.FindAsync(id);
            if (tarea == null)
            {
                return ResultVM<bool>.Fail("Tarea no encontrada");
            }

            var enUso = await _db.WorkFlows.AnyAsync(w => w.IdTarea == id);
            if (enUso)
            {
                return ResultVM<bool>.Fail("No se puede eliminar la tarea porque está siendo usada en WorkFlows");
            }

            try
            {
                _db.Tareas.Remove(tarea);
                await _db.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "CORE_Tareas",
                    EntidadId = id,
                    Accion = "DELETE",
                    Detalles = $"Tarea eliminada: {tarea.Nombre}",
                    RutaArchivo = string.Empty,
                    IdUsuario = usuarioId
                });

                return ResultVM<bool>.Ok(true, "Tarea eliminada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando tarea {Id}", id);
                return ResultVM<bool>.Fail("Error al eliminar la tarea. Por favor intente nuevamente.");
            }
        }
    }
}
