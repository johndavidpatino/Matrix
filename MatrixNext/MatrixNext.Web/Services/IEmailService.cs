using System.Net;
using System.Net.Mail;

namespace MatrixNext.Web.Services
{
    /// <summary>
    /// Interface para envío de emails
    /// Ref: ESPECIFICACION_COMPONENTES_COMPARTIDOS.md § 4
    /// </summary>
    public interface IEmailService
    {
        Task<bool> EnviarAsync(string destinatario, string asunto, string cuerpo, bool esHtml = true);
        Task<bool> EnviarMultipleAsync(List<string> destinatarios, string asunto, string cuerpo);
        Task<bool> EnviarConArchivosAsync(string destinatario, string asunto, string cuerpo, List<string> rutasArchivos);
    }
}
