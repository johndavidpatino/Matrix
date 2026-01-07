using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.ViewModels;
using MatrixNext.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.CORE;

/// <summary>
/// Interfaz para gestionar asignaciones de tareas a usuarios.
/// T3.3: Service Asignaciones
/// Ref: MATRIZ_PERMISOS_ROLES.md § 3.3 (Coordinador + Administrador)
/// </summary>
public interface IAsignacionesService
{
    /// <summary>
    /// Obtiene todos los usuarios asignados a una tarea.
    /// </summary>
    Task<List<WorkFlowUsuarioAsignado>> ObtenerUsuariosAsignados(long idWorkFlow);

    /// <summary>
    /// Asigna un usuario a una tarea con rol específico.
    /// </summary>
    Task<ResultVM<bool>> AsignarUsuario(long idWorkFlow, long idUsuario, string? rol = null);

    /// <summary>
    /// Desasigna un usuario de una tarea.
    /// </summary>
    Task<ResultVM<bool>> DesasignarUsuario(long idWorkFlow, long idUsuario);

    /// <summary>
    /// Obtiene asignaciones activas de un usuario.
    /// </summary>
    Task<List<WorkFlowUsuarioAsignado>> ObtenerAsignacionesActivas(long idUsuario);

    /// <summary>
    /// Verifica si un usuario está asignado a una tarea.
    /// </summary>
    Task<bool> EstaAsignado(long idWorkFlow, long idUsuario);
}

/// <summary>
/// Implementación de gestión de asignaciones.
/// </summary>
public class AsignacionesService : IAsignacionesService
{
    private readonly MatrixDbContext _db;
    private readonly IAuditoriaService _auditoria;

    public AsignacionesService(MatrixDbContext db, IAuditoriaService auditoria)
    {
        _db = db;
        _auditoria = auditoria;
    }

    /// <summary>
    /// Obtiene todos los usuarios asignados a una tarea.
    /// </summary>
    public async Task<List<WorkFlowUsuarioAsignado>> ObtenerUsuariosAsignados(long idWorkFlow)
    {
        return await _db.WorkFlowUsuariosAsignados
            .AsNoTracking()
            .Where(x => x.IdWorkFlow == idWorkFlow && x.Activo)
            .ToListAsync();
    }

    /// <summary>
    /// Asigna un usuario a una tarea con rol específico.
    /// Valida que no esté ya asignado.
    /// </summary>
    public async Task<ResultVM<bool>> AsignarUsuario(long idWorkFlow, long idUsuario, string? rol = null)
    {
        try
        {
            // Validar que la tarea existe
            var workFlow = await _db.WorkFlows.FindAsync(idWorkFlow);
            if (workFlow == null)
            {
                return ResultVM<bool>.Fail("Tarea no encontrada");
            }

            // Validar que no esté ya asignado
            var existente = await _db.WorkFlowUsuariosAsignados
                .FirstOrDefaultAsync(x => x.IdWorkFlow == idWorkFlow && 
                                          x.IdUsuario == idUsuario && 
                                          x.Activo);
            if (existente != null)
            {
                return ResultVM<bool>.Fail("El usuario ya está asignado a esta tarea");
            }

            // Crear asignación
            var asignacion = new WorkFlowUsuarioAsignado
            {
                IdWorkFlow = idWorkFlow,
                IdUsuario = idUsuario,
                Rol = rol ?? "Ejecutor",
                FechaAsignacion = DateTime.UtcNow,
                Activo = true
            };

            _db.WorkFlowUsuariosAsignados.Add(asignacion);
            await _db.SaveChangesAsync();

            // Registrar en auditoría
            _ = _auditoria.LogearAsync(new AuditoriaVM
            {
                Entidad = "WorkFlowUsuarioAsignado",
                EntidadId = idWorkFlow,
                Accion = "Asignar",
                Detalles = $"Usuario {idUsuario} asignado a tarea {idWorkFlow} con rol {rol ?? "Ejecutor"}"
            });

            return ResultVM<bool>.Ok(true, "Usuario asignado correctamente");
        }
        catch (Exception ex)
        {
            return ResultVM<bool>.Fail($"Error al asignar usuario: {ex.Message}");
        }
    }

    /// <summary>
    /// Desasigna un usuario de una tarea (soft delete).
    /// </summary>
    public async Task<ResultVM<bool>> DesasignarUsuario(long idWorkFlow, long idUsuario)
    {
        try
        {
            var asignacion = await _db.WorkFlowUsuariosAsignados
                .FirstOrDefaultAsync(x => x.IdWorkFlow == idWorkFlow && 
                                          x.IdUsuario == idUsuario && 
                                          x.Activo);
            if (asignacion == null)
            {
                return ResultVM<bool>.Fail("Asignación no encontrada");
            }

            asignacion.Activo = false;
            _db.WorkFlowUsuariosAsignados.Update(asignacion);
            await _db.SaveChangesAsync();

            // Registrar en auditoría
            _ = _auditoria.LogearAsync(new AuditoriaVM
            {
                Entidad = "WorkFlowUsuarioAsignado",
                EntidadId = idWorkFlow,
                Accion = "Desasignar",
                Detalles = $"Usuario {idUsuario} desasignado de tarea {idWorkFlow}"
            });

            return ResultVM<bool>.Ok(true, "Usuario desasignado correctamente");
        }
        catch (Exception ex)
        {
            return ResultVM<bool>.Fail($"Error al desasignar usuario: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtiene todas las asignaciones activas de un usuario.
    /// </summary>
    public async Task<List<WorkFlowUsuarioAsignado>> ObtenerAsignacionesActivas(long idUsuario)
    {
        return await _db.WorkFlowUsuariosAsignados
            .AsNoTracking()
            .Where(x => x.IdUsuario == idUsuario && x.Activo)
            .Include(x => x.WorkFlow)
            .ToListAsync();
    }

    /// <summary>
    /// Verifica si un usuario está asignado a una tarea.
    /// </summary>
    public async Task<bool> EstaAsignado(long idWorkFlow, long idUsuario)
    {
        return await _db.WorkFlowUsuariosAsignados
            .AnyAsync(x => x.IdWorkFlow == idWorkFlow && 
                           x.IdUsuario == idUsuario && 
                           x.Activo);
    }
}
