using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.ViewModels;
using MatrixNext.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.CORE;

/// <summary>
/// Servicio de máquina de estados para transiciones de tareas (WorkFlow).
/// Implementa reglas de negocio:
/// - Estados válidos: Creada → EnProgreso → Completada / Anulada
/// - Validación de roles: solo responsables pueden cambiar estado
/// - Escalación: cambio a alta prioridad requiere supervisor
/// Ref: MATRIZ_PERMISOS_ROLES.md § 5.2
/// Ref: SPRINT_10_11_IMPLEMENTACION_COMPLETADA.md (estados)
/// </summary>
public interface IWorkFlowStateTransitionService
{
    /// <summary>
    /// Obtiene los estados válidos a los que puede transitar desde el estado actual.
    /// </summary>
    Task<List<string>> ObtenerEstadosPermitidos(long idWorkFlow, long idUsuario);

    /// <summary>
    /// Valida si la transición de estado es permitida según reglas de negocio.
    /// </summary>
    Task<ResultVM<bool>> ValidarTransicion(long idWorkFlow, string estadoActual, string nuevoEstado, long idUsuario);

    /// <summary>
    /// Obtiene el rol del usuario en la tarea.
    /// </summary>
    Task<string?> ObtenerRolUsuarioEnTarea(long idWorkFlow, long idUsuario);
}

public class WorkFlowStateTransitionService : IWorkFlowStateTransitionService
{
    private readonly MatrixDbContext _db;

    // Definición de máquina de estados: transiciones permitidas
    private static readonly Dictionary<string, List<string>> EstadosPermitidos = new()
    {
        { "Creada", new List<string> { "EnProgreso", "Anulada" } },
        { "EnProgreso", new List<string> { "Completada", "Anulada" } },
        { "Completada", new List<string> { "Anulada" } },
        { "Anulada", new List<string>() }  // Terminal, no hay transiciones
    };

    // Roles que pueden ejecutar cada transición
    private static readonly Dictionary<string, List<string>> RolesPermitidosPorTransicion = new()
    {
        { "Creada->EnProgreso", new List<string> { "Responsable", "Supervisor" } },
        { "EnProgreso->Completada", new List<string> { "Responsable", "Supervisor" } },
        { "Creada->Anulada", new List<string> { "Supervisor", "Administrador" } },
        { "EnProgreso->Anulada", new List<string> { "Supervisor", "Administrador" } },
        { "Completada->Anulada", new List<string> { "Administrador" } }
    };

    public WorkFlowStateTransitionService(MatrixDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Obtiene los estados válidos a los que puede transitar desde el estado actual.
    /// Filtra según el rol del usuario.
    /// </summary>
    public async Task<List<string>> ObtenerEstadosPermitidos(long idWorkFlow, long idUsuario)
    {
        var workflow = await _db.WorkFlows.FindAsync(idWorkFlow);
        if (workflow == null)
            return new List<string>();

        var estadosValidos = new List<string>();

        // Obtener estados válidos desde la máquina de estados
        if (EstadosPermitidos.TryGetValue(workflow.Estado ?? "Creada", out var estadosPosibles))
        {
            var rolUsuario = await ObtenerRolUsuarioEnTarea(idWorkFlow, idUsuario);

            foreach (var estado in estadosPosibles)
            {
                var transicion = $"{workflow.Estado}->{estado}";
                if (RolesPermitidosPorTransicion.TryGetValue(transicion, out var rolesRequeridos))
                {
                    if (rolesRequeridos.Contains(rolUsuario ?? ""))
                    {
                        estadosValidos.Add(estado);
                    }
                }
            }
        }

        return estadosValidos;
    }

    /// <summary>
    /// Valida si la transición es permitida:
    /// 1. Estado nuevo existe en los permitidos
    /// 2. Rol del usuario permite la transición
    /// 3. Precedencias están completas (si aplica)
    /// </summary>
    public async Task<ResultVM<bool>> ValidarTransicion(long idWorkFlow, string estadoActual, string nuevoEstado, long idUsuario)
    {
        // 1. Validar que el estado nuevo esté en los permitidos
        if (!EstadosPermitidos.TryGetValue(estadoActual, out var estadosValidos) || !estadosValidos.Contains(nuevoEstado))
        {
            return ResultVM<bool>.Fail($"Transición inválida: {estadoActual} → {nuevoEstado} no es permitida");
        }

        // 2. Validar rol del usuario
        var transicion = $"{estadoActual}->{nuevoEstado}";
        if (!RolesPermitidosPorTransicion.TryGetValue(transicion, out var rolesRequeridos))
        {
            return ResultVM<bool>.Fail("Regla de transición no configurada");
        }

        var rolUsuario = await ObtenerRolUsuarioEnTarea(idWorkFlow, idUsuario);
        if (!rolesRequeridos.Contains(rolUsuario ?? ""))
        {
            return ResultVM<bool>.Fail($"Tu rol '{rolUsuario}' no tiene permiso para cambiar de {estadoActual} a {nuevoEstado}. Roles requeridos: {string.Join(", ", rolesRequeridos)}");
        }

        // 3. Validar precedencias (solo para cambios de estado a "EnProgreso" o "Completada")
        if (nuevoEstado is "EnProgreso" or "Completada")
        {
            var tarea = await _db.WorkFlows
                .Include(w => w.TareasPrevias)
                .FirstOrDefaultAsync(w => w.Id == idWorkFlow);

            if (tarea?.TareasPrevias.Count > 0)
            {
                var idsTareasPrevias = tarea.TareasPrevias.Select(tp => tp.IdTareaPreviaRequerida).ToList();
                var tareasPendientes = await _db.WorkFlows
                    .Where(w => idsTareasPrevias.Contains(w.Id) && w.Estado != "Completada" && w.Estado != "Anulada")
                    .ToListAsync();

                if (tareasPendientes.Any())
                {
                    return ResultVM<bool>.Fail($"No puedes avanzar. Quedan {tareasPendientes.Count} tarea(s) previa(s) pendiente(s): {string.Join(", ", tareasPendientes.Select(t => t.IdTarea))}");
                }
            }
        }

        return ResultVM<bool>.Ok(true);
    }

    /// <summary>
    /// Obtiene el rol del usuario en la tarea.
    /// Retorna "Responsable", "Supervisor", "Observador" o null si no está asignado.
    /// </summary>
    public async Task<string?> ObtenerRolUsuarioEnTarea(long idWorkFlow, long idUsuario)
    {
        var asignacion = await _db.WorkFlowUsuariosAsignados
            .Where(x => x.IdWorkFlow == idWorkFlow && x.IdUsuario == idUsuario && x.Activo)
            .Select(x => x.Rol)
            .FirstOrDefaultAsync();

        return asignacion;
    }
}
