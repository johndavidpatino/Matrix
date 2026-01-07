namespace MatrixNext.Web.Services
{
    /// <summary>
    /// Implementación de IAuditoriaService
    /// Por ahora log a archivo; puede extenderse a BD
    /// </summary>
    public class AuditoriaService : IAuditoriaService
    {
        private readonly ILogger<AuditoriaService> _logger;
        private readonly string _auditLogPath;

        public AuditoriaService(ILogger<AuditoriaService> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _auditLogPath = Path.Combine(env.ContentRootPath, "logs", "audit.log");
            
            // Crear directorio de logs si no existe
            var logDir = Path.GetDirectoryName(_auditLogPath);
            if (!string.IsNullOrEmpty(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
        }

        public async Task LogearAsync(AuditoriaVM auditoria)
        {
            try
            {
                var logEntry = $"[{auditoria.Timestamp:yyyy-MM-dd HH:mm:ss}] " +
                    $"Acción: {auditoria.Accion} | " +
                    $"Entidad: {auditoria.Entidad} (ID: {auditoria.EntidadId}) | " +
                    $"Detalles: {auditoria.Detalles} | " +
                    $"Usuario: {auditoria.IdUsuario}";

                // Log a archivo
                await File.AppendAllTextAsync(_auditLogPath, logEntry + Environment.NewLine);

                // Log a ILogger
                _logger.LogInformation(logEntry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando auditoría");
            }
        }
    }
}
