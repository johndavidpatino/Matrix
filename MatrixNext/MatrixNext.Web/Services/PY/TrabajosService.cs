using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.PY;
using MatrixNext.Web.Services;
using MatrixNext.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.PY
{
    /// <summary>
    /// Servicio de dominio para trabajos (PY_Trabajo)
    /// Listados paginados, CRUD y utilidades de duplicado simple
    /// </summary>
    public interface ITrabajosService
    {
        Task<PaginationResultVM<Trabajo>> ListarAsync(FiltrosVM filtros, long? idProyecto = null);
        Task<Trabajo?> ObtenerPorIdAsync(long id);
        Task<ResultVM<Trabajo>> CrearAsync(Trabajo entity);
        Task<ResultVM<Trabajo>> ActualizarAsync(Trabajo entity);
        Task<ResultVM<bool>> EliminarAsync(long id);
        Task<ResultVM<Trabajo>> DuplicarAsync(long idTrabajo, string? nuevoNombre = null, string? nuevoJobBook = null);
    }

    public class TrabajosService : ITrabajosService
    {
        private readonly MatrixDbContext _db;
        private readonly IGridService _grid;
        private readonly IAuditoriaService _auditoria;

        public TrabajosService(MatrixDbContext db, IGridService grid, IAuditoriaService auditoria)
        {
            _db = db;
            _grid = grid;
            _auditoria = auditoria;
        }

        public async Task<PaginationResultVM<Trabajo>> ListarAsync(FiltrosVM filtros, long? idProyecto = null)
        {
            var query = _db.Trabajos.AsNoTracking();

            if (idProyecto.HasValue)
            {
                query = query.Where(t => t.IdProyecto == idProyecto.Value);
            }

            if (!string.IsNullOrWhiteSpace(filtros.Busqueda))
            {
                var term = filtros.Busqueda;
                query = query.Where(t => (t.Nombre ?? string.Empty).Contains(term) || (t.JobBook ?? string.Empty).Contains(term));
            }

            if (filtros.Estado >= 0)
            {
                query = query.Where(t => t.Estado == filtros.Estado);
            }

            return await _grid.PaginarAsync(query, filtros.PageNumber, filtros.PageSize, filtros.SortBy ?? "Id", filtros.SortDescending);
        }

        public async Task<Trabajo?> ObtenerPorIdAsync(long id)
        {
            return await _db.Trabajos.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<ResultVM<Trabajo>> CrearAsync(Trabajo entity)
        {
            var proyectoExiste = await _db.Proyectos.AnyAsync(p => p.Id == entity.IdProyecto);
            if (!proyectoExiste)
            {
                return ResultVM<Trabajo>.Fail("El proyecto asociado no existe");
            }

            var nombreDuplicado = !string.IsNullOrWhiteSpace(entity.Nombre) && await _db.Trabajos.AnyAsync(t => t.IdProyecto == entity.IdProyecto && t.Nombre == entity.Nombre);
            if (nombreDuplicado)
            {
                return ResultVM<Trabajo>.Fail("Ya existe un trabajo con el mismo nombre en el proyecto");
            }

            entity.FechaCreacion = DateTime.UtcNow;
            entity.FechaModificacion = DateTime.UtcNow;

            _db.Trabajos.Add(entity);
            await _db.SaveChangesAsync();

            await _auditoria.LogearAsync(new AuditoriaVM
            {
                Entidad = "PY_Trabajo",
                EntidadId = entity.Id,
                Accion = "CREATE",
                Detalles = $"Proyecto={entity.IdProyecto}, Nombre={entity.Nombre ?? string.Empty}"
            });

            return ResultVM<Trabajo>.Ok(entity, "Trabajo creado exitosamente");
        }

        public async Task<ResultVM<Trabajo>> ActualizarAsync(Trabajo entity)
        {
            var existente = await _db.Trabajos.FindAsync(entity.Id);
            if (existente == null)
            {
                return ResultVM<Trabajo>.Fail("Trabajo no encontrado");
            }

            if (!string.IsNullOrWhiteSpace(entity.Nombre))
            {
                var duplicado = await _db.Trabajos.AnyAsync(t => t.Id != entity.Id && t.IdProyecto == existente.IdProyecto && t.Nombre == entity.Nombre);
                if (duplicado)
                {
                    return ResultVM<Trabajo>.Fail("Ya existe un trabajo con el mismo nombre en el proyecto");
                }
            }

            existente.Nombre = entity.Nombre;
            existente.Descripcion = entity.Descripcion;
            existente.IdMetodologia = entity.IdMetodologia;
            existente.IdTipoProyecto = entity.IdTipoProyecto;
            existente.JobBook = entity.JobBook;
            existente.Estado = entity.Estado;
            existente.IdCoordinador = entity.IdCoordinador;
            existente.FechaEnvio = entity.FechaEnvio;
            existente.FechaCierre = entity.FechaCierre;
            existente.FechaModificacion = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            await _auditoria.LogearAsync(new AuditoriaVM
            {
                Entidad = "PY_Trabajo",
                EntidadId = existente.Id,
                Accion = "UPDATE",
                Detalles = $"Proyecto={existente.IdProyecto}, Estado={existente.Estado}"
            });

            return ResultVM<Trabajo>.Ok(existente, "Trabajo actualizado");
        }

        public async Task<ResultVM<bool>> EliminarAsync(long id)
        {
            var entity = await _db.Trabajos.FindAsync(id);
            if (entity == null)
            {
                return ResultVM<bool>.Fail("Trabajo no encontrado");
            }

            entity.Activo = false;
            entity.FechaModificacion = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            await _auditoria.LogearAsync(new AuditoriaVM
            {
                Entidad = "PY_Trabajo",
                EntidadId = id,
                Accion = "DELETE",
                Detalles = $"JobBook={entity.JobBook ?? string.Empty}"
            });

            return ResultVM<bool>.Ok(true, "Trabajo eliminado");
        }

        public async Task<ResultVM<Trabajo>> DuplicarAsync(long idTrabajo, string? nuevoNombre = null, string? nuevoJobBook = null)
        {
            var original = await _db.Trabajos.AsNoTracking().FirstOrDefaultAsync(t => t.Id == idTrabajo);
            if (original == null)
            {
                return ResultVM<Trabajo>.Fail("Trabajo no encontrado para duplicar");
            }

            var copia = new Trabajo
            {
                IdProyecto = original.IdProyecto,
                Nombre = nuevoNombre ?? $"{original.Nombre ?? ""} (Copia)",
                Descripcion = original.Descripcion,
                IdMetodologia = original.IdMetodologia,
                IdTipoProyecto = original.IdTipoProyecto,
                JobBook = nuevoJobBook ?? original.JobBook,
                Estado = 1,
                IdCoordinador = original.IdCoordinador,
                FechaEnvio = null,
                FechaCierre = null,
                Activo = true,
                FechaCreacion = DateTime.UtcNow,
                FechaModificacion = DateTime.UtcNow
            };

            _db.Trabajos.Add(copia);
            await _db.SaveChangesAsync();

            await _auditoria.LogearAsync(new AuditoriaVM
            {
                Entidad = "PY_Trabajo",
                EntidadId = copia.Id,
                Accion = "CREATE",
                Detalles = $"Duplicado de={idTrabajo}, Nombre={copia.Nombre ?? string.Empty}"
            });

            return ResultVM<Trabajo>.Ok(copia, "Trabajo duplicado");
        }
    }
}
