using MatrixNext.Web.Infrastructure.Data;
using Dapper;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Servicio de background para recordatorios de sesiones (24h antes)
/// Ejecuta cada 6 horas para verificar sesiones programadas
/// Ref: Sprint 6 Fase 5 - Email/Notifications
/// </summary>
public class OpReminderBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OpReminderBackgroundService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(6);

    public OpReminderBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<OpReminderBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Servicio OpReminderBackgroundService iniciado");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await EnviarRecordatorios();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en OpReminderBackgroundService");
            }

            try
            {
                await Task.Delay(_checkInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        _logger.LogInformation("Servicio OpReminderBackgroundService detenido");
    }

    private async Task EnviarRecordatorios()
    {
        using var scope = _serviceProvider.CreateScope();
        var notificacionService = scope.ServiceProvider.GetRequiredService<IOpNotificacionService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<MatrixNext.Web.Infrastructure.Data.MatrixDbContext>();

        try
        {
            // Obtener programaciones para 24-48h en el futuro
            var ahora = DateTime.Now;
            var ventanaFin = ahora.AddHours(48);
            var ventanaInicio = ahora.AddHours(23);

            var connection = dbContext.Database.GetDbConnection();
            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            var programacionesProximas = await connection.QueryAsync<long>(@"
                SELECT Id
                FROM OP_ProgramacionCampo
                WHERE FechaProgramada BETWEEN @VentanaInicio AND @VentanaFin
                AND Estado IN (0, 1) -- Pendiente, Confirmado
                AND ReminderEnviado = 0",
                new { VentanaInicio = ventanaInicio, VentanaFin = ventanaFin });

            foreach (var programacionId in programacionesProximas)
            {
                var result = await notificacionService.EnviarRecordatorioSesionAsync(programacionId);
                
                if (result.Success)
                {
                    // Marcar como recordatorio enviado
                    await connection.ExecuteAsync(
                        "UPDATE OP_ProgramacionCampo SET ReminderEnviado = 1 WHERE Id = @Id",
                        new { Id = programacionId });
                    
                    _logger.LogInformation("Recordatorio enviado para programación {Id}", programacionId);
                }
                else
                {
                    _logger.LogWarning("Error enviando recordatorio {Id}: {Error}", programacionId, result.Error);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en ciclo de recordatorios");
        }
    }
}
