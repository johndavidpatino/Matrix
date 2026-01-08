using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Background service for cleaning up old export files
/// Runs daily to remove exports older than 30 days
/// Ref: S4-004
/// </summary>
public class ExportAuditoriaCleanupBackgroundService : BackgroundService
{
    private readonly ILogger<ExportAuditoriaCleanupBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TimeSpan _executionTime = TimeSpan.FromHours(1); // Run every hour

    public ExportAuditoriaCleanupBackgroundService(
        ILogger<ExportAuditoriaCleanupBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ExportAuditoriaCleanupBackgroundService iniciado");

        try
        {
            // Initial delay to avoid startup congestion
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var exportService = scope.ServiceProvider.GetRequiredService<IOpExportesAuditoriaService>();
                        
                        int cleaned = await exportService.LimpiarExportacionesAntiguasAsync(diasRetension: 30);
                        
                        if (cleaned > 0)
                        {
                            _logger.LogInformation("Limpieza de exportes completada: {Count} archivos eliminados", cleaned);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error ejecutando limpieza de exportes");
                }

                // Wait for next execution cycle
                await Task.Delay(_executionTime, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("ExportAuditoriaCleanupBackgroundService cancelado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fatal en ExportAuditoriaCleanupBackgroundService");
            throw;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("ExportAuditoriaCleanupBackgroundService detenido");
        await base.StopAsync(cancellationToken);
    }
}
