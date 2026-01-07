using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.CORE
{
    /// <summary>
    /// Servicio de dominio para WorkFlow (tareas de trabajos)
    /// Lógica de negocio: validaciones, cambios de estado, asignaciones
    /// </summary>
    public interface IWorkFlowService
    {
        Task<ResultVM<WorkFlow>> CrearAsync(WorkFlow entity);
        Task<ResultVM<WorkFlow>> ActualizarAsync(WorkFlow entity);
        Task<ResultVM<bool>> EliminarAsync(long id);
        Task<WorkFlow?> ObtenerPorIdAsync(long id);
        Task<IEnumerable<WorkFlow>> ObtenerPorTrabajoAsync(long idTrabajo);
    }

    public class WorkFlowService : IWorkFlowService
    {
        private readonly MatrixDbContext _db;
        private readonly WorkFlowDataAdapter _adapter;
        private readonly IAuditoriaService _auditoria;

        public WorkFlowService(
            MatrixDbContext db, 
            WorkFlowDataAdapter adapter,
            IAuditoriaService auditoria)
        {
            _db = db;
            _adapter = adapter;
            _auditoria = auditoria;
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
                return ResultVM<WorkFlow>.Fail($"Error al crear WorkFlow: {ex.Message}");
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
                return ResultVM<WorkFlow>.Fail($"Error al actualizar: {ex.Message}");
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
                return ResultVM<bool>.Fail($"Error al eliminar: {ex.Message}");
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
