using System.Collections.Concurrent;

namespace MatrixNext.Web.Services
{
    /// <summary>
    /// Email queue service for asynchronous email processing
    /// Ref: ESPECIFICACION_COMPONENTES_COMPARTIDOS.md § 4.2
    /// Wraps IEmailService to queue emails for background processing without external dependencies
    /// </summary>
    public interface IEmailQueueService
    {
        /// <summary>
        /// Queue a single email for asynchronous sending
        /// </summary>
        Task QueueEmailAsync(string destinatario, string asunto, string cuerpo, bool esHtml = true);

        /// <summary>
        /// Queue multiple emails for asynchronous sending
        /// </summary>
        Task QueueEmailMultipleAsync(List<string> destinatarios, string asunto, string cuerpo);

        /// <summary>
        /// Queue email with attachments for asynchronous sending
        /// </summary>
        Task QueueEmailConArchivosAsync(string destinatario, string asunto, string cuerpo, List<string> rutasArchivos);

        /// <summary>
        /// Get current queue depth
        /// </summary>
        int GetQueueDepth();

        /// <summary>
        /// Get processing statistics
        /// </summary>
        EmailQueueStats GetStats();
    }

    /// <summary>
    /// Email queue statistics
    /// </summary>
    public class EmailQueueStats
    {
        public int QueuedCount { get; set; }
        public int ProcessedCount { get; set; }
        public int FailedCount { get; set; }
        public DateTime LastProcessedTime { get; set; }
    }

    /// <summary>
    /// Email queue item for internal processing
    /// </summary>
    internal class EmailQueueItem
    {
        public required string Type { get; set; } // "single", "multiple", "withfiles"
        public string Destinatario { get; set; } = string.Empty;
        public List<string> Destinatarios { get; set; } = new();
        public string Asunto { get; set; } = string.Empty;
        public string Cuerpo { get; set; } = string.Empty;
        public bool EsHtml { get; set; } = true;
        public List<string> RutasArchivos { get; set; } = new();
        public int RetryCount { get; set; } = 0;
        public const int MaxRetries = 3;
    }

    /// <summary>
    /// Implementation of IEmailQueueService using in-memory queue
    /// Processes emails without external dependencies (no Hangfire, no database)
    /// </summary>
    public class EmailQueueService : IEmailQueueService
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailQueueService> _logger;
        private readonly ConcurrentQueue<EmailQueueItem> _queue;
        private int _processedCount = 0;
        private int _failedCount = 0;
        private DateTime _lastProcessedTime = DateTime.MinValue;

        public EmailQueueService(IEmailService emailService, ILogger<EmailQueueService> logger)
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queue = new ConcurrentQueue<EmailQueueItem>();
        }

        public async Task QueueEmailAsync(string destinatario, string asunto, string cuerpo, bool esHtml = true)
        {
            if (string.IsNullOrWhiteSpace(destinatario))
            {
                _logger.LogWarning("QueueEmailAsync: destinatario vacío");
                return;
            }

            var item = new EmailQueueItem
            {
                Type = "single",
                Destinatario = destinatario,
                Asunto = asunto,
                Cuerpo = cuerpo,
                EsHtml = esHtml
            };

            _queue.Enqueue(item);
            _logger.LogInformation($"Email encolado para {destinatario}: {asunto}");
        }

        public async Task QueueEmailMultipleAsync(List<string> destinatarios, string asunto, string cuerpo)
        {
            if (destinatarios == null || destinatarios.Count == 0)
            {
                _logger.LogWarning("QueueEmailMultipleAsync: lista de destinatarios vacía");
                return;
            }

            var item = new EmailQueueItem
            {
                Type = "multiple",
                Destinatarios = destinatarios,
                Asunto = asunto,
                Cuerpo = cuerpo
            };

            _queue.Enqueue(item);
            _logger.LogInformation($"Emails múltiples encolados para {destinatarios.Count} destinatarios: {asunto}");
        }

        public async Task QueueEmailConArchivosAsync(string destinatario, string asunto, string cuerpo, List<string> rutasArchivos)
        {
            if (string.IsNullOrWhiteSpace(destinatario))
            {
                _logger.LogWarning("QueueEmailConArchivosAsync: destinatario vacío");
                return;
            }

            var item = new EmailQueueItem
            {
                Type = "withfiles",
                Destinatario = destinatario,
                Asunto = asunto,
                Cuerpo = cuerpo,
                RutasArchivos = rutasArchivos ?? new()
            };

            _queue.Enqueue(item);
            _logger.LogInformation($"Email con {rutasArchivos?.Count ?? 0} archivos encolado para {destinatario}");
        }

        public int GetQueueDepth() => _queue.Count;

        public EmailQueueStats GetStats()
        {
            return new EmailQueueStats
            {
                QueuedCount = _queue.Count,
                ProcessedCount = _processedCount,
                FailedCount = _failedCount,
                LastProcessedTime = _lastProcessedTime
            };
        }

        /// <summary>
        /// Process queued emails (called by BackgroundService)
        /// Internal method for background processing
        /// </summary>
        internal async Task ProcessQueueAsync()
        {
            while (_queue.TryDequeue(out var item))
            {
                try
                {
                    bool success = await ProcessEmailItemAsync(item);
                    
                    if (success)
                    {
                        _processedCount++;
                        _lastProcessedTime = DateTime.UtcNow;
                        _logger.LogInformation($"Email procesado exitosamente: {item.Asunto}");
                    }
                    else if (item.RetryCount < EmailQueueItem.MaxRetries)
                    {
                        item.RetryCount++;
                        _queue.Enqueue(item);
                        _logger.LogWarning($"Email reintentado ({item.RetryCount}/{EmailQueueItem.MaxRetries}): {item.Asunto}");
                    }
                    else
                    {
                        _failedCount++;
                        _logger.LogError($"Email descartado después de {EmailQueueItem.MaxRetries} reintentos: {item.Asunto}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error procesando email de cola: {item.Asunto}");
                    _failedCount++;
                }
            }
        }

        /// <summary>
        /// Process individual email item
        /// </summary>
        private async Task<bool> ProcessEmailItemAsync(EmailQueueItem item)
        {
            return item.Type switch
            {
                "single" => await _emailService.EnviarAsync(item.Destinatario, item.Asunto, item.Cuerpo, item.EsHtml),
                "multiple" => await _emailService.EnviarMultipleAsync(item.Destinatarios, item.Asunto, item.Cuerpo),
                "withfiles" => await _emailService.EnviarConArchivosAsync(item.Destinatario, item.Asunto, item.Cuerpo, item.RutasArchivos),
                _ => throw new InvalidOperationException($"Tipo de email desconocido: {item.Type}")
            };
        }
    }
}
