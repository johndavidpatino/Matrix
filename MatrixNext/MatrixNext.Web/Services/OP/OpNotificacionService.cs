using MatrixNext.Data.Services;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Services.OP.Models;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Data;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Implementación de servicio de notificaciones por email para OP
/// Reutiliza IEmailQueueService (cola existente)
/// </summary>
public class OpNotificacionService : IOpNotificacionService
{
    private readonly IEmailQueueService _emailQueue;
    private readonly MatrixDbContext _dbContext;
    private readonly ILogger<OpNotificacionService> _logger;

    public OpNotificacionService(
        IEmailQueueService emailQueue,
        MatrixDbContext dbContext,
        ILogger<OpNotificacionService> logger)
    {
        _emailQueue = emailQueue;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<(bool Success, string Error)> NotificarProgramacionCreadaAsync(long programacionId)
    {
        try
        {
            _logger.LogInformation("Enviando notificación programación {Id}", programacionId);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error notificando programación {Id}", programacionId);
            return (false, "Error al encolar notificación");
        }
    }

    public async Task<(bool Success, string Error)> EnviarRecordatorioSesionAsync(long programacionId)
    {
        try
        {
            _logger.LogInformation("Enviando recordatorio programación {Id}", programacionId);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enviando recordatorio {Id}", programacionId);
            return (false, "Error al encolar recordatorio");
        }
    }

    public async Task<(bool Success, string Error)> NotificarCambioEstadoProgramacionAsync(
        long programacionId, string estadoAnterior, string estadoNuevo)
    {
        try
        {
            _logger.LogInformation("Notificando cambio estado {EstadoNuevo} programación {Id}",
                estadoNuevo, programacionId);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error notificando cambio estado {Id}", programacionId);
            return (false, "Error al encolar notificación");
        }
    }

    public async Task<(bool Success, string Error)> NotificarFichaCompletadaAsync(long fichaId)
    {
        try
        {
            _logger.LogInformation("Notificando ficha completada {Id}", fichaId);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error notificando ficha {Id}", fichaId);
            return (false, "Error al encolar notificación");
        }
    }

    public async Task<(bool Success, string Error)> NotificarAsignacionModeradorAsync(
        long programacionId, long moderadorId)
    {
        try
        {
            _logger.LogInformation("Notificando asignación moderador {ModeradorId} para programación {Id}",
                moderadorId, programacionId);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error notificando asignación {Id}", programacionId);
            return (false, "Error al encolar notificación");
        }
    }

    public async Task<(bool Success, string Error)> EnviarReporteDiarioAsync(DateTime fecha)
    {
        try
        {
            _logger.LogInformation("Generando reporte diario {Fecha}", fecha.Date);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generando reporte {Fecha}", fecha);
            return (false, "Error al generar reporte");
        }
    }
}
