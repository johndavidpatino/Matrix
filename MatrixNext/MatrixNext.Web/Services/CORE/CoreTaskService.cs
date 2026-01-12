using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.CORE;

public class CoreTaskService : ICoreTaskService
{
    private readonly MatrixDbContext _context;
    private readonly ILogger<CoreTaskService> _logger;

    public CoreTaskService(MatrixDbContext context, ILogger<CoreTaskService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<WorkFlowDto> CreateTaskAsync(CreateTaskDto createDto)
    {
        try
        {
            if (createDto.IdTrabajo <= 0)
                throw new ArgumentException("El IdTrabajo es requerido");
            if (createDto.IdTarea <= 0)
                throw new ArgumentException("El IdTarea es requerido");
            if (createDto.IdTipoHilo <= 0)
                throw new ArgumentException("El IdTipoHilo es requerido");

            var workFlow = new WorkFlow
            {
                IdTrabajo = createDto.IdTrabajo,
                IdTarea = createDto.IdTarea,
                IdTipoHilo = createDto.IdTipoHilo,
                Estado = "Creada",
                Prioridad = createDto.Prioridad,
                FechaCreacion = DateTime.UtcNow,
                FechaVencimiento = createDto.FechaVencimiento,
                Observaciones = createDto.Observaciones
            };

            _context.WorkFlows.Add(workFlow);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Tarea creada: {TaskId}", workFlow.Id);

            return MapToDto(workFlow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear tarea");
            throw;
        }
    }

    public async Task<WorkFlowDto?> GetTaskByIdAsync(long taskId)
    {
        var workFlow = await _context.WorkFlows
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == taskId);

        return workFlow == null ? null : MapToDto(workFlow);
    }

    public async Task<PaginatedResponse<WorkFlowDto>> GetTasksByUserAsync(long userId, int pageNumber, int pageSize)
    {
        var query = _context.WorkFlowUsuariosAsignados
            .Where(a => a.IdUsuario == userId && a.Activo)
            .Select(a => a.WorkFlow!)
            .Where(w => w != null)
            .OrderByDescending(w => w!.FechaCreacion)
            .AsNoTracking();

        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponse<WorkFlowDto>
        {
            Items = items.ConvertAll(MapToDto),
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<WorkFlowDto?> UpdateTaskAsync(long taskId, UpdateTaskDto updateDto)
    {
        var workFlow = await _context.WorkFlows.FindAsync(taskId);
        if (workFlow == null)
            return null;

        if (!string.IsNullOrWhiteSpace(updateDto.Observaciones))
            workFlow.Observaciones = updateDto.Observaciones;

        if (updateDto.Prioridad.HasValue)
            workFlow.Prioridad = updateDto.Prioridad.Value;

        if (updateDto.FechaVencimiento.HasValue)
            workFlow.FechaVencimiento = updateDto.FechaVencimiento;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Tarea actualizada: {TaskId}", taskId);

        return MapToDto(workFlow);
    }

    public async Task<WorkFlowDto?> CancelTaskAsync(long taskId, string? motivo)
    {
        var workFlow = await _context.WorkFlows.FindAsync(taskId);
        if (workFlow == null)
            return null;

        if (workFlow.Estado == "Completada" || workFlow.Estado == "Anulada")
            throw new InvalidOperationException($"No se puede anular una tarea en estado {workFlow.Estado}");

        workFlow.Estado = "Anulada";

        await _context.SaveChangesAsync();
        _logger.LogInformation("Tarea anulada: {TaskId}, Motivo: {Reason}", taskId, motivo);

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
