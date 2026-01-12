using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.CORE;

public class CoreAuditService : ICoreAuditService
{
    private readonly MatrixDbContext _context;
    private readonly ILogger<CoreAuditService> _logger;

    public CoreAuditService(MatrixDbContext context, ILogger<CoreAuditService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task LogTaskChangeAsync(long taskId, long idUsuario, string tipoOperacion, string? detalles)
    {
        try
        {
            // TODO: Crear tabla CoreTaskAudit si no existe
            // Por ahora, solo registrar en log
            _logger.LogInformation(
                "Auditoría - Tarea: {TaskId}, Usuario: {UserId}, Operación: {Operation}, Detalles: {Details}",
                taskId, idUsuario, tipoOperacion, detalles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al registrar cambio de tarea en auditoría");
        }
    }

    public async Task<object> AddTaskCommentAsync(long taskId, long idUsuario, string comentario)
    {
        try
        {
            // TODO: Implementar tabla CoreTaskComment
            // Por ahora, retornar objeto vacío
            _logger.LogInformation("Comentario agregado a tarea {TaskId} por usuario {UserId}", 
                taskId, idUsuario);

            return new
            {
                TaskId = taskId,
                UserId = idUsuario,
                Comment = comentario,
                CreatedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar comentario a tarea {TaskId}", taskId);
            throw;
        }
    }

    public async Task<List<object>> GetTaskHistoryAsync(long taskId)
    {
        // TODO: Consultar tabla CoreTaskAudit
        // Por ahora, retornar lista vacía
        _logger.LogInformation("Obteniendo historial de tarea {TaskId}", taskId);
        return new List<object>();
    }

    public async Task<List<object>> GetTaskCommentsAsync(long taskId)
    {
        // TODO: Consultar tabla CoreTaskComment
        // Por ahora, retornar lista vacía
        _logger.LogInformation("Obteniendo comentarios de tarea {TaskId}", taskId);
        return new List<object>();
    }
}
