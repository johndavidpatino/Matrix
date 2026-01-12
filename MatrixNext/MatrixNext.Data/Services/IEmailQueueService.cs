namespace MatrixNext.Data.Services
{
    /// <summary>
    /// Email queue service interface for asynchronous email processing
    /// Ref: ESPECIFICACION_COMPONENTES_COMPARTIDOS.md § 4.2
    /// This interface is in MatrixNext.Data to avoid circular dependencies
    /// Implementation is in MatrixNext.Web.Services
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
}
