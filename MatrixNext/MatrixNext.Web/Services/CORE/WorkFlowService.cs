using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.ViewModels;
using MatrixNext.Data.DTOs.CORE;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.CORE
{
    /// <summary>
    /// Servicio de dominio para WorkFlow (tareas de trabajos)
    /// Lógica de negocio: validaciones, cambios de estado, asignaciones
    /// </summary>
    public interface IWorkFlowService
    {
        Task<PaginationResultVM<WorkFlow>> ObtenerPaginadoAsync(int page, int size, string sortBy, bool desc, long? idTrabajo = null, long? idTarea = null, int? estado = null);
        Task<ResultVM<bool>> CrearHiloInicialAsync(long idTrabajo, long idProyecto);
        Task<ResultVM<WorkFlow>> CrearAsync(WorkFlow entity);
        Task<ResultVM<WorkFlow>> ActualizarAsync(WorkFlow entity);
        Task<ResultVM<bool>> EliminarAsync(long id);
        Task<WorkFlow?> ObtenerPorIdAsync(long id);
        Task<IEnumerable<WorkFlow>> ObtenerPorTrabajoAsync(long idTrabajo);

        // Sprint 17 - TraficoTareas
        Task<(List<TareasPorUnidadDto> Tareas, int Total)> ObtenerTareasPorUnidadAsync(
            int idUnidad,
            string? estado = null,
            int? prioridad = null,
            string? busqueda = null,
            int page = 1,
            int pageSize = 20);

        Task<List<UnidadTraficoDto>> ObtenerUnidadesTraficoAsync();

        Task<TrabajoTraficoInfoDto?> ObtenerInformacionTrabajoAsync(long idTrabajo);
    }

    public partial class WorkFlowService : IWorkFlowService
    {
        private readonly MatrixDbContext _db;
        private readonly WorkFlowDataAdapter _adapter;
        private readonly IAuditoriaService _auditoria;
        private readonly IGridService _grid;
        private readonly ILogger<WorkFlowService> _logger;

        public WorkFlowService(
            MatrixDbContext db, 
            WorkFlowDataAdapter adapter,
            IAuditoriaService auditoria,
            IGridService grid,
            ILogger<WorkFlowService> logger)
        {
            _db = db;
            _adapter = adapter;
            _auditoria = auditoria;
            _grid = grid;
            _logger = logger;
        }

        public async Task<PaginationResultVM<WorkFlow>> ObtenerPaginadoAsync(int page, int size, string sortBy, bool desc, long? idTrabajo = null, long? idTarea = null, int? estado = null)
        {
            var query = _db.WorkFlows.AsNoTracking();

            if (idTrabajo.HasValue)
                query = query.Where(x => x.IdTrabajo == idTrabajo.Value);

            if (idTarea.HasValue)
                query = query.Where(x => x.IdTarea == idTarea.Value);

            if (estado.HasValue)
            {
                var estadoTexto = estado.Value.ToString();
                query = query.Where(x => x.Estado == estadoTexto);
            }

            return await _grid.PaginarAsync(query, page, size, sortBy, desc);
        }

        public async Task<ResultVM<bool>> CrearHiloInicialAsync(long idTrabajo, long idProyecto)
        {
            try
            {
                var ok = await _adapter.CrearHiloCrearTareasAsync(idTrabajo, idProyecto);
                if (!ok)
                {
                    return ResultVM<bool>.Fail("No se generaron tareas CORE para el trabajo");
                }

                // Best-effort: el log no bloquea la respuesta principal
                _ = _adapter.RegistrarLogCreacionAsync(idTrabajo);

                return ResultVM<bool>.Ok(true, "Tareas CORE generadas para el trabajo");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail("Error al crear tareas CORE. Por favor intente nuevamente.");
            }
        }

        public async Task<ResultVM<WorkFlow>> CrearAsync(WorkFlow entity)
        {
            // Validar que no exista duplicado
            var existente = await _adapter.ObtenerPorTrabajoYTareaAsync(entity.IdTrabajo, entity.IdTarea);
            if (existente != null)
            {
                return ResultVM<WorkFlow>.Fail("Ya existe un WorkFlow para este Trabajo y Tarea");
            }

            try
            {
                _db.WorkFlows.Add(entity);
                await _db.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "CORE_WorkFlow",
                    EntidadId = entity.Id,
                    Accion = "CREATE",
                    Detalles = $"IdTrabajo={entity.IdTrabajo}, IdTarea={entity.IdTarea}"
                });

                return ResultVM<WorkFlow>.Ok(entity, "WorkFlow creado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<WorkFlow>.Fail("Error al crear WorkFlow. Por favor intente nuevamente.");
            }
        }

        public async Task<ResultVM<WorkFlow>> ActualizarAsync(WorkFlow entity)
        {
            var existente = await _db.WorkFlows.FindAsync(entity.Id);
            if (existente == null)
            {
                return ResultVM<WorkFlow>.Fail("WorkFlow no encontrado");
            }

            try
            {
                existente.IdTrabajo = entity.IdTrabajo;
                existente.IdTarea = entity.IdTarea;
                existente.Estado = entity.Estado;
                existente.Prioridad = entity.Prioridad;
                existente.FechaVencimiento = entity.FechaVencimiento;
                existente.Observaciones = entity.Observaciones;

                await _db.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "CORE_WorkFlow",
                    EntidadId = entity.Id,
                    Accion = "UPDATE",
                    Detalles = $"Estado={entity.Estado}, Prioridad={entity.Prioridad}"
                });

                return ResultVM<WorkFlow>.Ok(existente, "WorkFlow actualizado");
            }
            catch (Exception ex)
            {
                return ResultVM<WorkFlow>.Fail("Error al actualizar WorkFlow. Por favor intente nuevamente.");
            }
        }

        public async Task<ResultVM<bool>> EliminarAsync(long id)
        {
            var entity = await _db.WorkFlows.FindAsync(id);
            if (entity == null)
            {
                return ResultVM<bool>.Fail("WorkFlow no encontrado");
            }

            try
            {
                _db.WorkFlows.Remove(entity);
                await _db.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "CORE_WorkFlow",
                    EntidadId = id,
                    Accion = "DELETE",
                    Detalles = $"IdTrabajo={entity.IdTrabajo}, IdTarea={entity.IdTarea}"
                });

                return ResultVM<bool>.Ok(true, "WorkFlow eliminado");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail("Error al eliminar WorkFlow. Por favor intente nuevamente.");
            }
        }

        public async Task<WorkFlow?> ObtenerPorIdAsync(long id)
        {
            return await _db.WorkFlows
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<WorkFlow>> ObtenerPorTrabajoAsync(long idTrabajo)
        {
            return await _adapter.ObtenerListaAsync(idTrabajo: idTrabajo);
        }
    }
}
