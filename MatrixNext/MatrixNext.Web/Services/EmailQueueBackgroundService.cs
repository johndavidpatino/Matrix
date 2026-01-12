using System.Threading;
using System.Threading.Tasks;
using MatrixNext.Data.Services;

namespace MatrixNext.Web.Services
{
    /// <summary>
    /// Background service for processing email queue
    /// Ref: ESPECIFICACION_COMPONENTES_COMPARTIDOS.md § 4.2
    /// Processes queued emails asynchronously without external dependencies
    /// </summary>
    public class EmailQueueBackgroundService : BackgroundService
    {
        private readonly ILogger<EmailQueueBackgroundService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly TimeSpan _processingInterval = TimeSpan.FromSeconds(5); // Process every 5 seconds

        public EmailQueueBackgroundService(
            ILogger<EmailQueueBackgroundService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("EmailQueueBackgroundService iniciado");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var emailQueueService = scope.ServiceProvider.GetRequiredService<IEmailQueueService>();

                            if (emailQueueService is EmailQueueService queueService)
                            {
                                await queueService.ProcessQueueAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error procesando email queue");
                    }

                    // Wait before next processing cycle
                    await Task.Delay(_processingInterval, stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("EmailQueueBackgroundService cancelado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fatal en EmailQueueBackgroundService");
                throw;
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EmailQueueBackgroundService detenido");
            await base.StopAsync(cancellationToken);
        }
    }
}
