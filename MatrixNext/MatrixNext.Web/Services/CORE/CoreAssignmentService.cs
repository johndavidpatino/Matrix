using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.CORE;

public class CoreAssignmentService : ICoreAssignmentService
{
    private readonly MatrixDbContext _context;
    private readonly ICoreNotificationService _notificationService;
    private readonly ICoreAuditService _auditService;
    private readonly ILogger<CoreAssignmentService> _logger;

    public CoreAssignmentService(
        MatrixDbContext context,
        ICoreNotificationService notificationService,
        ICoreAuditService auditService,
        ILogger<CoreAssignmentService> logger)
    {
        _context = context;
        _notificationService = notificationService;
        _auditService = auditService;
        _logger = logger;
    }

    public async Task<WorkFlowDto?> AssignTaskAsync(long taskId, AssignTaskDto assignDto)
    {
        var workFlow = await _context.WorkFlows.FindAsync(taskId);
        if (workFlow == null)
            return null;

        if (assignDto.IdUsuarios == null || assignDto.IdUsuarios.Count == 0)
            throw new ArgumentException("Se requiere al menos un usuario para asignar");

        // Limpiar asignaciones previas
        var prevAssignments = await _context.WorkFlowUsuariosAsignados
            .Where(x => x.IdWorkFlow == taskId && x.Activo)
            .ToListAsync();

        foreach (var assignment in prevAssignments)
        {
            assignment.Activo = false;
        }

        // Crear nuevas asignaciones
        foreach (var idUsuario in assignDto.IdUsuarios)
        {
            var assignment = new WorkFlowUsuarioAsignado
            {
                IdWorkFlow = taskId,
                IdUsuario = idUsuario,
                Rol = assignDto.Rol ?? "Ejecutor",
                FechaAsignacion = DateTime.UtcNow,
                Activo = true
            };

            _context.WorkFlowUsuariosAsignados.Add(assignment);

            // Notificar asignación
            await _notificationService.NotifyTaskAssignedAsync(taskId, idUsuario, 
                assignDto.Rol ?? "Ejecutor");
        }

        if (workFlow.Estado == "Creada")
            workFlow.Estado = "EnProgreso";

        await _context.SaveChangesAsync();
        _logger.LogInformation("Tarea asignada: {TaskId} a usuarios: {Users}", taskId, 
            string.Join(", ", assignDto.IdUsuarios));

        // Registrar en auditoría
        await _auditService.LogTaskChangeAsync(taskId, assignDto.IdUsuarios.First(), 
            "Asignar", assignDto.Comentario);

        return MapToDto(workFlow);
    }

    public async Task<WorkFlowDto?> EscalateTaskAsync(long taskId, EscalateTaskDto escalateDto)
    {
        var workFlow = await _context.WorkFlows.FindAsync(taskId);
        if (workFlow == null)
            return null;

        if (workFlow.Estado == "Completada" || workFlow.Estado == "Anulada")
            throw new InvalidOperationException(
                $"No se puede escalar una tarea en estado {workFlow.Estado}");

        // Crear nueva asignación para usuario destino
        var escalationAssignment = new WorkFlowUsuarioAsignado
        {
            IdWorkFlow = taskId,
            IdUsuario = escalateDto.IdUsuarioDestino,
            Rol = "Supervisor",
            FechaAsignacion = DateTime.UtcNow,
            Activo = true
        };

        _context.WorkFlowUsuariosAsignados.Add(escalationAssignment);

        // Mantener estado en EnProgreso si ya estaba trabajando
        if (workFlow.Estado == "Creada")
            workFlow.Estado = "EnProgreso";

        await _context.SaveChangesAsync();
        _logger.LogInformation("Tarea escalada: {TaskId} a usuario: {UserId}, Motivo: {Reason}", 
            taskId, escalateDto.IdUsuarioDestino, escalateDto.Motivo);

        // Notificar escalada
        await _notificationService.NotifyTaskEscalatedAsync(taskId, escalateDto.IdUsuarioDestino, 
            escalateDto.Motivo);

        // Registrar en auditoría
        await _auditService.LogTaskChangeAsync(taskId, escalateDto.IdUsuarioDestino, 
            "Escalar", $"Escalada a supervisor. Motivo: {escalateDto.Motivo}");

        return MapToDto(workFlow);
    }

    public async Task<WorkFlowDto?> ReassignTaskAsync(long taskId, long newUserId, string? motivo)
    {
        var workFlow = await _context.WorkFlows.FindAsync(taskId);
        if (workFlow == null)
            return null;

        // Desactivar asignación anterior
        var currentAssignments = await _context.WorkFlowUsuariosAsignados
            .Where(x => x.IdWorkFlow == taskId && x.Activo)
            .ToListAsync();
        foreach (var ca in currentAssignments)
            ca.Activo = false;

        // Crear nueva asignación
        var newAssignment = new WorkFlowUsuarioAsignado
        {
            IdWorkFlow = taskId,
            IdUsuario = newUserId,
            Rol = "Ejecutor",
            FechaAsignacion = DateTime.UtcNow,
            Activo = true
        };

        _context.WorkFlowUsuariosAsignados.Add(newAssignment);

        await _context.SaveChangesAsync();
        _logger.LogInformation("Tarea reasignada: {TaskId} a usuario: {UserId}", taskId, newUserId);

        // Notificar nueva asignación
        await _notificationService.NotifyTaskAssignedAsync(taskId, newUserId, "Ejecutor");

        // Registrar en auditoría
        await _auditService.LogTaskChangeAsync(taskId, newUserId, "Reasignar", motivo);

        return MapToDto(workFlow);
    }

    public async Task<List<object>> GetAvailableUsersAsync(string modulo)
    {
        // TODO: Integrar con TH_Usuario para obtener usuarios disponibles del módulo
        // Por ahora, retornar lista vacía
        return new List<object>();
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
