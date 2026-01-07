using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.PY;
using MatrixNext.Web.Services;
using MatrixNext.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.PY
{
    /// <summary>
    /// Servicio de dominio para proyectos (PY_Proyectos)
    /// Lecturas con paginación vía GridService, escrituras con EF Core
    /// </summary>
    public interface IProyectosService
    {
        Task<PaginationResultVM<Proyecto>> ListarAsync(FiltrosVM filtros);
        Task<Proyecto?> ObtenerPorIdAsync(long id);
        Task<ResultVM<Proyecto>> CrearAsync(Proyecto entity);
        Task<ResultVM<Proyecto>> ActualizarAsync(Proyecto entity);
        Task<ResultVM<bool>> EliminarAsync(long id);
    }

    public class ProyectosService : IProyectosService
    {
        private readonly MatrixDbContext _db;
        private readonly IGridService _grid;
        private readonly IAuditoriaService _auditoria;

        public ProyectosService(MatrixDbContext db, IGridService grid, IAuditoriaService auditoria)
        {
            _db = db;
            _grid = grid;
            _auditoria = auditoria;
        }

        public async Task<PaginationResultVM<Proyecto>> ListarAsync(FiltrosVM filtros)
        {
            var query = _db.Proyectos.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filtros.Busqueda))
            {
                var term = filtros.Busqueda;
                query = query.Where(p => (p.Nombre ?? string.Empty).Contains(term) || (p.JobBook ?? string.Empty).Contains(term));
            }

            if (filtros.Estado >= 0)
            {
                query = query.Where(p => p.Estado == filtros.Estado);
            }

            return await _grid.PaginarAsync(query, filtros.PageNumber, filtros.PageSize, filtros.SortBy, filtros.SortDescending);
        }

        public async Task<Proyecto?> ObtenerPorIdAsync(long id)
        {
            return await _db.Proyectos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ResultVM<Proyecto>> CrearAsync(Proyecto entity)
        {
            var existeJobBook = !string.IsNullOrWhiteSpace(entity.JobBook) && await _db.Proyectos.AnyAsync(p => p.JobBook == entity.JobBook);
            if (existeJobBook)
            {
                return ResultVM<Proyecto>.Fail("Ya existe un proyecto con el mismo JobBook");
            }

            entity.FechaCreacion = DateTime.UtcNow;
            entity.FechaModificacion = DateTime.UtcNow;

            _db.Proyectos.Add(entity);
            await _db.SaveChangesAsync();

            await _auditoria.LogearAsync(new AuditoriaVM
            {
                Entidad = "PY_Proyectos",
                EntidadId = entity.Id,
                Accion = "CREATE",
                Detalles = $"Nombre={entity.Nombre ?? string.Empty}, JobBook={entity.JobBook ?? string.Empty}"
            });

            return ResultVM<Proyecto>.Ok(entity, "Proyecto creado exitosamente");
        }

        public async Task<ResultVM<Proyecto>> ActualizarAsync(Proyecto entity)
        {
            var existente = await _db.Proyectos.FindAsync(entity.Id);
            if (existente == null)
            {
                return ResultVM<Proyecto>.Fail("Proyecto no encontrado");
            }

            if (!string.IsNullOrWhiteSpace(entity.JobBook))
            {
                var repetido = await _db.Proyectos.AnyAsync(p => p.Id != entity.Id && p.JobBook == entity.JobBook);
                if (repetido)
                {
                    return ResultVM<Proyecto>.Fail("Ya existe un proyecto con el mismo JobBook");
                }
            }

            existente.Nombre = entity.Nombre;
            existente.Descripcion = entity.Descripcion;
            existente.IdGerenteProyectos = entity.IdGerenteProyectos;
            existente.IdUnidad = entity.IdUnidad;
            existente.Estado = entity.Estado;
            existente.JobBook = entity.JobBook;
            existente.FechaModificacion = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            await _auditoria.LogearAsync(new AuditoriaVM
            {
                Entidad = "PY_Proyectos",
                EntidadId = existente.Id,
                Accion = "UPDATE",
                Detalles = $"Nombre={existente.Nombre ?? string.Empty}, Estado={existente.Estado}"
            });

            return ResultVM<Proyecto>.Ok(existente, "Proyecto actualizado");
        }

        public async Task<ResultVM<bool>> EliminarAsync(long id)
        {
            var entity = await _db.Proyectos.FindAsync(id);
            if (entity == null)
            {
                return ResultVM<bool>.Fail("Proyecto no encontrado");
            }

            // Soft delete para conservar historial
            entity.Activo = false;
            entity.FechaModificacion = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            await _auditoria.LogearAsync(new AuditoriaVM
            {
                Entidad = "PY_Proyectos",
                EntidadId = id,
                Accion = "DELETE",
                Detalles = $"JobBook={entity.JobBook ?? string.Empty}"
            });

            return ResultVM<bool>.Ok(true, "Proyecto eliminado");
        }
    }
}
