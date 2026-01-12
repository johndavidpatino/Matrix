using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.CORE;

public class CoreWorkflowService : ICoreWorkflowService
{
    private readonly MatrixDbContext _context;
    private readonly ICoreNotificationService _notificationService;
    private readonly ICoreAuditService _auditService;
    private readonly ILogger<CoreWorkflowService> _logger;

    public CoreWorkflowService(
        MatrixDbContext context,
        ICoreNotificationService notificationService,
        ICoreAuditService auditService,
        ILogger<CoreWorkflowService> logger)
    {
        _context = context;
        _notificationService = notificationService;
        _auditService = auditService;
        _logger = logger;
    }

    public async Task<bool> IsValidStateTransitionAsync(long taskId, string newState)
    {
        var workFlow = await _context.WorkFlows.FindAsync(taskId);
        if (workFlow == null)
            return false;

        var validTransitions = new Dictionary<string, List<string>>
        {
            { "Creada", new List<string> { "EnProgreso", "Anulada" } },
            { "EnProgreso", new List<string> { "Completada", "Creada", "Anulada" } },
            { "Completada", new List<string> { } }, // Final state
            { "Anulada", new List<string> { } } // Final state
        };

        return validTransitions.ContainsKey(workFlow.Estado) && 
               validTransitions[workFlow.Estado].Contains(newState);
    }

    public async Task<WorkFlowDto?> CompleteTaskAsync(long taskId, string? resultado)
    {
        var workFlow = await _context.WorkFlows.FindAsync(taskId);
        if (workFlow == null)
            return null;

        if (!await IsValidStateTransitionAsync(taskId, "Completada"))
            throw new InvalidOperationException($"No se puede completar una tarea en estado {workFlow.Estado}");

        workFlow.Estado = "Completada";

        await _context.SaveChangesAsync();
        _logger.LogInformation("Tarea completada: {TaskId}", taskId);

        // Notificar cambio de estado
        await _notificationService.NotifyTaskClosedAsync(taskId);

        // Registrar en auditoría
        var asignado = await _context.WorkFlowUsuariosAsignados
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.IdWorkFlow == taskId && a.Activo);
        await _auditService.LogTaskChangeAsync(taskId, asignado?.IdUsuario ?? 0, 
            "CambiarEstado", $"Completada. {resultado}");

        return MapToDto(workFlow);
    }

    public async Task<WorkFlowDto?> ChangeTaskStateAsync(long taskId, string newState, string? razon)
    {
        var workFlow = await _context.WorkFlows.FindAsync(taskId);
        if (workFlow == null)
            return null;

        if (!await IsValidStateTransitionAsync(taskId, newState))
            throw new InvalidOperationException(
                $"Transición no permitida de {workFlow.Estado} a {newState}");

        var oldState = workFlow.Estado;
        workFlow.Estado = newState;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Estado de tarea cambió de {OldState} a {NewState} para TaskId: {TaskId}", 
            oldState, newState, taskId);

        // Notificar cambio de estado
        await _notificationService.NotifyTaskStateChangedAsync(taskId, newState);

        // Registrar en auditoría
        var asignado = await _context.WorkFlowUsuariosAsignados
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.IdWorkFlow == taskId && a.Activo);
        await _auditService.LogTaskChangeAsync(taskId, asignado?.IdUsuario ?? 0, 
            "CambiarEstado", $"De {oldState} a {newState}. {razon}");

        return MapToDto(workFlow);
    }

    private static WorkFlowDto MapToDto(WorkFlow workFlow)
    {
        return new WorkFlowDto
        {
            Id = workFlow.Id,
            IdTrabajo = workFlow.IdTrabajo,
            IdTarea = workFlow.IdTarea,
            IdTipoHilo = workFlow.IdTipoHilo,
            Estado = workFlow.Estado,
            Prioridad = workFlow.Prioridad,
            Observaciones = workFlow.Observaciones,
            FechaCreacion = workFlow.FechaCreacion,
            FechaVencimiento = workFlow.FechaVencimiento
        };
    }
}
