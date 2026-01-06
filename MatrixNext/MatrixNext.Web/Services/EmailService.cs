using System.Net;
using System.Net.Mail;

namespace MatrixNext.Web.Services
{
    /// <summary>
    /// Implementación de IEmailService
    /// Ref: ESPECIFICACION_COMPONENTES_COMPARTIDOS.md § 4.2
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<bool> EnviarAsync(string destinatario, string asunto, string cuerpo, bool esHtml = true)
        {
            try
            {
                using (var cliente = new SmtpClient())
                {
                    var smtpHost = _config["Email:SmtpHost"];
                    var smtpPort = int.Parse(_config["Email:SmtpPort"] ?? "587");
                    var enableSsl = bool.Parse(_config["Email:EnableSsl"] ?? "true");
                    var username = _config["Email:Username"];
                    var password = _config["Email:Password"];
                    var senderEmail = _config["Email:SenderEmail"];
                    var senderName = _config["Email:SenderName"] ?? "Matrix";

                    cliente.Host = smtpHost;
                    cliente.Port = smtpPort;
                    cliente.EnableSsl = enableSsl;
                    cliente.Credentials = new NetworkCredential(username, password);

                    var mensaje = new MailMessage()
                    {
                        From = new MailAddress(senderEmail, senderName),
                        Subject = asunto,
                        Body = cuerpo,
                        IsBodyHtml = esHtml
                    };

                    mensaje.To.Add(destinatario);

                    await cliente.SendMailAsync(mensaje);
                    _logger.LogInformation($"Email enviado a {destinatario}: {asunto}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando email a {destinatario}");
                return false;
            }
        }

        public async Task<bool> EnviarMultipleAsync(List<string> destinatarios, string asunto, string cuerpo)
        {
            try
            {
                foreach (var dest in destinatarios)
                {
                    await EnviarAsync(dest, asunto, cuerpo);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando emails múltiples");
                return false;
            }
        }

        public async Task<bool> EnviarConArchivosAsync(string destinatario, string asunto, string cuerpo, List<string> rutasArchivos)
        {
            try
            {
                using (var cliente = new SmtpClient())
                {
                    var smtpHost = _config["Email:SmtpHost"];
                    var smtpPort = int.Parse(_config["Email:SmtpPort"] ?? "587");
                    var enableSsl = bool.Parse(_config["Email:EnableSsl"] ?? "true");
                    var username = _config["Email:Username"];
                    var password = _config["Email:Password"];
                    var senderEmail = _config["Email:SenderEmail"];
                    var senderName = _config["Email:SenderName"] ?? "Matrix";

                    cliente.Host = smtpHost;
                    cliente.Port = smtpPort;
                    cliente.EnableSsl = enableSsl;
                    cliente.Credentials = new NetworkCredential(username, password);

                    var mensaje = new MailMessage()
                    {
                        From = new MailAddress(senderEmail, senderName),
                        Subject = asunto,
                        Body = cuerpo,
                        IsBodyHtml = true
                    };

                    mensaje.To.Add(destinatario);

                    foreach (var ruta in rutasArchivos)
                    {
                        if (File.Exists(ruta))
                        {
                            mensaje.Attachments.Add(new Attachment(ruta));
                        }
                    }

                    await cliente.SendMailAsync(mensaje);
                    _logger.LogInformation($"Email con {rutasArchivos.Count} archivos enviado a {destinatario}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando email con archivos");
                return false;
            }
        }
    }
}
