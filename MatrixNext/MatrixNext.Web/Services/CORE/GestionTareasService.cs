using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.ViewModels;
using MatrixNext.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.CORE;

/// <summary>
/// Interfaz para gestionar operaciones en tareas (cambio de estado, auditoría, etc).
/// T3.4: Service GestionTareas (CRÍTICO)
/// Ref: MATRIZ_PERMISOS_ROLES.md § 5.2 (precedencias)
/// Ref: MAPA_DEPENDENCIAS_PY_CORE.md § 2.3 (validar transaccionalidad)
/// </summary>
public interface IGestionTareasService
{
    /// <summary>
    /// Cambia el estado de una tarea validando precedencias.
    /// </summary>
    Task<ResultVM<bool>> CambiarEstado(long idWorkFlow, string nuevoEstado, long idUsuario, string? observacion = null);

    /// <summary>
    /// Obtiene las tareas asignadas a un usuario (filtradas por estado).
    /// </summary>
    Task<List<WorkFlow>> ObtenerMisTareas(long idUsuario, string? estado = null);

    /// <summary>
    /// Obtiene las tareas previas que deben completarse antes de poder cambiar estado.
    /// </summary>
    Task<List<WorkFlow>> ObtenerTareasPrevias(long idWorkFlow);

    /// <summary>
    /// Agrega un comentario/observación a una tarea.
    /// </summary>
    Task<ResultVM<bool>> AgregarObservacion(long idWorkFlow, long idUsuario, string observacion, string tipoOperacion = "Comentario");

    /// <summary>
    /// Anula una tarea (solo administrador).
    /// </summary>
    Task<ResultVM<bool>> AnularTarea(long idWorkFlow, long idUsuario, string motivo);

    /// <summary>
    /// Valida si todas las tareas previas de una tarea están completadas.
    /// </summary>
    Task<bool> ValidarPrecedenciasCompletadas(long idWorkFlow);
}

/// <summary>
/// Implementación de gestión de tareas con validación de precedencias.
/// </summary>
public class GestionTareasService : IGestionTareasService
{
    private readonly MatrixDbContext _db;
    private readonly IAuditoriaService _auditoria;

    public GestionTareasService(
        MatrixDbContext db,
        IAuditoriaService auditoria)
    {
        _db = db;
        _auditoria = auditoria;
    }

    /// <summary>
    /// Cambia el estado de una tarea validando:
    /// 1. Que todas las tareas previas estén completadas
    /// 2. Que el usuario esté asignado a la tarea
    /// 3. Que el cambio de estado sea válido
    /// </summary>
    public async Task<ResultVM<bool>> CambiarEstado(long idWorkFlow, string nuevoEstado, long idUsuario, string? observacion = null)
    {
        try
        {
            // 1. Obtener la tarea
            var workFlow = await _db.WorkFlows.FindAsync(idWorkFlow);
            if (workFlow == null)
            {
                return ResultVM<bool>.Fail("Tarea no encontrada");
            }

            // 2. Validar que el usuario esté asignado
            var asignado = await _db.WorkFlowUsuariosAsignados
                .AnyAsync(x => x.IdWorkFlow == idWorkFlow && 
                               x.IdUsuario == idUsuario && 
                               x.Activo);
            if (!asignado)
            {
                return ResultVM<bool>.Fail("No tienes permiso para cambiar el estado de esta tarea");
            }

            // 3. Validar precedencias
            var validPrecedencias = await ValidarPrecedenciasCompletadas(idWorkFlow);
            if (!validPrecedencias && nuevoEstado != "Anulada")
            {
                var tareasPrevias = await ObtenerTareasPrevias(idWorkFlow);
                var tareasPendientes = tareasPrevias
                    .Where(t => t.Estado != "Completada" && t.Estado != "Anulada")
                    .ToList();
                
                return ResultVM<bool>.Fail($"No puedes cambiar el estado. Tienes {tareasPendientes.Count} tarea(s) previa(s) pendiente(s)");
            }

            // 4. Cambiar estado
            var estadoAnterior = workFlow.Estado;
            workFlow.Estado = nuevoEstado;
            _db.WorkFlows.Update(workFlow);
            await _db.SaveChangesAsync();

            // 5. Registrar en ObservacionesTareas
            var observacionTarea = new ObservacionTarea
            {
                IdWorkFlow = idWorkFlow,
                IdUsuario = idUsuario,
                Observacion = observacion ?? $"Estado cambió de {estadoAnterior} a {nuevoEstado}",
                TipoOperacion = "CambioEstado",
                FechaCreacion = DateTime.UtcNow
            };

            _db.ObservacionesTareas.Add(observacionTarea);
            await _db.SaveChangesAsync();

            // 6. Registrar en auditoría general
            _ = _auditoria.LogearAsync(new AuditoriaVM
            {
                Entidad = "WorkFlow",
                EntidadId = idWorkFlow,
                Accion = "CambioEstado",
                Detalles = $"Estado cambió de {estadoAnterior} a {nuevoEstado}"
            });

            return ResultVM<bool>.Ok(true, $"Estado cambió a {nuevoEstado} correctamente");
        }
        catch (Exception ex)
        {
            return ResultVM<bool>.Fail($"Error al cambiar estado: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtiene las tareas asignadas a un usuario.
    /// Opcionalmente filtradas por estado.
    /// </summary>
    public async Task<List<WorkFlow>> ObtenerMisTareas(long idUsuario, string? estado = null)
    {
        var query = _db.WorkFlows
            .AsNoTracking()
            .Where(w => w.UsuariosAsignados.Any(ua => ua.IdUsuario == idUsuario && ua.Activo));

        if (!string.IsNullOrEmpty(estado))
        {
            query = query.Where(w => w.Estado == estado);
        }

        return await query
            .OrderByDescending(w => w.FechaVencimiento)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene las tareas previas directas de una tarea.
    /// </summary>
    public async Task<List<WorkFlow>> ObtenerTareasPrevias(long idWorkFlow)
    {
        var tarea = await _db.WorkFlows
            .Include(w => w.TareasPrevias)
            .FirstOrDefaultAsync(w => w.Id == idWorkFlow);

        if (tarea == null || tarea.TareasPrevias.Count == 0)
        {
            return new List<WorkFlow>();
        }

        var idsTareasPrevias = tarea.TareasPrevias
            .Select(tp => tp.IdTareaPreviaRequerida)
            .ToList();

        return await _db.WorkFlows
            .AsNoTracking()
            .Where(w => idsTareasPrevias.Contains(w.Id))
            .ToListAsync();
    }

    public async Task<ResultVM<bool>> AgregarObservacion(long idWorkFlow, long idUsuario, string observacion, string tipoOperacion = "Comentario")
    {
        try
        {
            var workFlow = await _db.WorkFlows.FindAsync(idWorkFlow);
            if (workFlow == null)
            {
                return ResultVM<bool>.Fail("Tarea no encontrada");
            }

            var obs = new ObservacionTarea
            {
                IdWorkFlow = idWorkFlow,
                IdUsuario = idUsuario,
                Observacion = observacion,
                TipoOperacion = tipoOperacion,
                FechaCreacion = DateTime.UtcNow
            };

            _db.ObservacionesTareas.Add(obs);
            await _db.SaveChangesAsync();

            return ResultVM<bool>.Ok(true, "Observación agregada correctamente");
        }
        catch (Exception ex)
        {
            return ResultVM<bool>.Fail($"Error al agregar observación: {ex.Message}");
        }
    }

    /// <summary>
    /// Anula una tarea (solo administrador).
    /// </summary>
    public async Task<ResultVM<bool>> AnularTarea(long idWorkFlow, long idUsuario, string motivo)
    {
        try
        {
            var workFlow = await _db.WorkFlows.FindAsync(idWorkFlow);
            if (workFlow == null)
            {
                return ResultVM<bool>.Fail("Tarea no encontrada");
            }

            workFlow.Estado = "Anulada";
            _db.WorkFlows.Update(workFlow);

            var obs = new ObservacionTarea
            {
                IdWorkFlow = idWorkFlow,
                IdUsuario = idUsuario,
                Observacion = motivo,
                TipoOperacion = "Anulacion",
                FechaCreacion = DateTime.UtcNow
            };

            _db.ObservacionesTareas.Add(obs);
            await _db.SaveChangesAsync();

            _ = _auditoria.LogearAsync(new AuditoriaVM
            {
                Entidad = "WorkFlow",
                EntidadId = idWorkFlow,
                Accion = "Anulacion",
                Detalles = $"Tarea anulada. Motivo: {motivo}"
            });

            return ResultVM<bool>.Ok(true, "Tarea anulada correctamente");
        }
        catch (Exception ex)
        {
            return ResultVM<bool>.Fail($"Error al anular tarea: {ex.Message}");
        }
    }

    /// <summary>
    /// Valida si todas las tareas previas están completadas.
    /// </summary>
    public async Task<bool> ValidarPrecedenciasCompletadas(long idWorkFlow)
    {
        var tarea = await _db.WorkFlows
            .Include(w => w.TareasPrevias)
            .FirstOrDefaultAsync(w => w.Id == idWorkFlow);

        if (tarea == null || tarea.TareasPrevias.Count == 0)
        {
            return true;
        }

        var idsTareasPrevias = tarea.TareasPrevias
            .Select(tp => tp.IdTareaPreviaRequerida)
            .ToList();

        var tareasPendientes = await _db.WorkFlows
            .AnyAsync(w => idsTareasPrevias.Contains(w.Id) && 
                           w.Estado != "Completada" && 
                           w.Estado != "Anulada");

        return !tareasPendientes;
    }
}
