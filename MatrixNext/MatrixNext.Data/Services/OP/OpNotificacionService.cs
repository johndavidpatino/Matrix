using MatrixNext.Data.Adapters.OP;
using MatrixNext.Data.Models.OP;
using MatrixNext.Web.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.StringBuilder;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.OP;

/// <summary>
/// Implementación del servicio de notificaciones para OP
/// Envía emails de notificación sobre cambios en FichaCuantitativa y estados de trabajos
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.5
/// </summary>
public class OpNotificacionService : IOpNotificacionService
{
    private readonly INotificacionesOpAdapter _adapter;
    private readonly IEmailService _emailService;
    private readonly ILogger<OpNotificacionService> _logger;

    public OpNotificacionService(
        INotificacionesOpAdapter adapter,
        IEmailService emailService,
        ILogger<OpNotificacionService> logger)
    {
        _adapter = adapter;
        _emailService = emailService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene destinatarios para notificación de trabajo
    /// </summary>
    public async Task<IEnumerable<DestinatarioEmailDto>> ObtenerDestinatariosAsync(long idTrabajo)
    {
        try
        {
            var destinatarios = await _adapter.ObtenerDestinatariosAsync(idTrabajo);
            return destinatarios;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo destinatarios. IdTrabajo: {IdTrabajo}", idTrabajo);
            return Enumerable.Empty<DestinatarioEmailDto>();
        }
    }

    /// <summary>
    /// Notifica creación de FichaCuantitativa
    /// </summary>
    public async Task<(bool Success, string Message)> NotificarCreacionFichaAsync(
        long idTrabajo,
        string numeroTrabajo,
        string codigoProyecto,
        string nombreProyecto,
        long usuarioId)
    {
        try
        {
            var destinatarios = await ObtenerDestinatariosAsync(idTrabajo);

            if (!destinatarios.Any())
            {
                _logger.LogWarning(
                    "No hay destinatarios para notificar creación de ficha. IdTrabajo: {IdTrabajo}",
                    idTrabajo);
                return (true, "No hay destinatarios para notificar");
            }

            var asunto = $"[MATRIX] Ficha Cuantitativa Creada - {numeroTrabajo}";
            var cuerpo = GenerarCuerpoCreacionFicha(numeroTrabajo, codigoProyecto, nombreProyecto);

            var emails = destinatarios.Select(d => d.EmailOrigen).Distinct().ToList();

            await _emailService.EnviarAsync(
                asunto: asunto,
                cuerpo: cuerpo,
                destinatarios: emails,
                esHtml: true);

            _logger.LogInformation(
                "Notificación de creación de ficha enviada. IdTrabajo: {IdTrabajo}, Destinatarios: {Count}",
                idTrabajo, destinatarios.Count());

            return (true, $"Notificación enviada a {destinatarios.Count()} destinatarios");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enviando notificación creación ficha. IdTrabajo: {IdTrabajo}", idTrabajo);
            return (false, "Error al enviar notificación");
        }
    }

    /// <summary>
    /// Notifica cambio de estado de trabajo
    /// </summary>
    public async Task<(bool Success, string Message)> NotificarCambioEstadoAsync(
        long idTrabajo,
        string numeroTrabajo,
        string estadoAnterior,
        string estadoNuevo,
        string? observaciones = null,
        long? usuarioId = null)
    {
        try
        {
            var destinatarios = await ObtenerDestinatariosAsync(idTrabajo);

            if (!destinatarios.Any())
            {
                _logger.LogWarning(
                    "No hay destinatarios para notificar cambio de estado. IdTrabajo: {IdTrabajo}",
                    idTrabajo);
                return (true, "No hay destinatarios para notificar");
            }

            var asunto = $"[MATRIX] Cambio de Estado - {numeroTrabajo}: {estadoAnterior} → {estadoNuevo}";
            var cuerpo = GenerarCuerpoCambioEstado(numeroTrabajo, estadoAnterior, estadoNuevo, observaciones);

            var emails = destinatarios.Select(d => d.EmailOrigen).Distinct().ToList();

            await _emailService.EnviarAsync(
                asunto: asunto,
                cuerpo: cuerpo,
                destinatarios: emails,
                esHtml: true);

            _logger.LogInformation(
                "Notificación de cambio de estado enviada. IdTrabajo: {IdTrabajo}, {Anterior} → {Nuevo}",
                idTrabajo, estadoAnterior, estadoNuevo);

            return (true, "Notificación enviada");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error enviando notificación cambio estado. IdTrabajo: {IdTrabajo}",
                idTrabajo);
            return (false, "Error al enviar notificación");
        }
    }

    /// <summary>
    /// Notifica cierre de trabajo
    /// </summary>
    public async Task<(bool Success, string Message)> NotificarCierreTrabajoAsync(
        long idTrabajo,
        string numeroTrabajo,
        string codigoProyecto,
        string? observaciones = null,
        long? usuarioId = null)
    {
        try
        {
            var destinatarios = await ObtenerDestinatariosAsync(idTrabajo);

            if (!destinatarios.Any())
            {
                _logger.LogWarning(
                    "No hay destinatarios para notificar cierre. IdTrabajo: {IdTrabajo}",
                    idTrabajo);
                return (true, "No hay destinatarios para notificar");
            }

            var asunto = $"[MATRIX] Trabajo Cerrado - {numeroTrabajo} ({codigoProyecto})";
            var cuerpo = GenerarCuerpoCierre(numeroTrabajo, codigoProyecto, observaciones);

            var emails = destinatarios.Select(d => d.EmailOrigen).Distinct().ToList();

            await _emailService.EnviarAsync(
                asunto: asunto,
                cuerpo: cuerpo,
                destinatarios: emails,
                esHtml: true);

            _logger.LogInformation(
                "Notificación de cierre enviada. IdTrabajo: {IdTrabajo}, Destinatarios: {Count}",
                idTrabajo, destinatarios.Count());

            return (true, "Notificación de cierre enviada");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enviando notificación cierre. IdTrabajo: {IdTrabajo}", idTrabajo);
            return (false, "Error al enviar notificación");
        }
    }

    /// <summary>
    /// Envía email customizado con parámetros específicos
    /// </summary>
    public async Task<(bool Success, string Message)> NotificarCustomizadoAsync(
        ParamsNotificacionFichaDto parametros)
    {
        try
        {
            if (!parametros.Destinatarios.Any())
            {
                _logger.LogWarning(
                    "No hay destinatarios en notificación customizada. IdTrabajo: {IdTrabajo}",
                    parametros.IdTrabajo);
                return (false, "No hay destinatarios especificados");
            }

            var asunto = $"[MATRIX] {parametros.TipoNotificacion} - {parametros.NumeroTrabajo}";
            var cuerpo = GenerarCuerpoCustomizado(parametros);

            var emails = parametros.Destinatarios
                .Where(d => !string.IsNullOrEmpty(d.EmailOrigen))
                .Select(d => d.EmailOrigen)
                .Distinct()
                .ToList();

            if (!emails.Any())
            {
                return (false, "No hay emails válidos para enviar");
            }

            await _emailService.EnviarAsync(
                asunto: asunto,
                cuerpo: cuerpo,
                destinatarios: emails,
                esHtml: true);

            _logger.LogInformation(
                "Notificación customizada enviada. IdTrabajo: {IdTrabajo}, Tipo: {Tipo}, Destinatarios: {Count}",
                parametros.IdTrabajo, parametros.TipoNotificacion, emails.Count);

            return (true, $"Notificación enviada a {emails.Count} destinatarios");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error enviando notificación customizada. IdTrabajo: {IdTrabajo}",
                parametros.IdTrabajo);
            return (false, "Error al enviar notificación");
        }
    }

    // Generadores de cuerpo de email

    private string GenerarCuerpoCreacionFicha(string numeroTrabajo, string codigoProyecto, string nombreProyecto)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<html>");
        sb.AppendLine("<head><meta charset='UTF-8'></head>");
        sb.AppendLine("<body style='font-family: Arial, sans-serif;'>");
        sb.AppendLine("<div style='background-color: #f5f5f5; padding: 20px;'>");
        sb.AppendLine("<h2 style='color: #0066cc;'>Ficha Cuantitativa Creada</h2>");
        sb.AppendLine("<p>Se ha creado una nueva ficha cuantitativa para su revisión:</p>");
        sb.AppendLine("<table style='border-collapse: collapse; width: 100%; max-width: 600px;'>");
        sb.AppendLine($"<tr><td style='padding: 10px; border: 1px solid #ddd;'><strong>Trabajo:</strong></td><td style='padding: 10px; border: 1px solid #ddd;'>{numeroTrabajo}</td></tr>");
        sb.AppendLine($"<tr><td style='padding: 10px; border: 1px solid #ddd;'><strong>Proyecto:</strong></td><td style='padding: 10px; border: 1px solid #ddd;'>{codigoProyecto} - {nombreProyecto}</td></tr>");
        sb.AppendLine($"<tr><td style='padding: 10px; border: 1px solid #ddd;'><strong>Fecha:</strong></td><td style='padding: 10px; border: 1px solid #ddd;'>{DateTime.Now:dd/MM/yyyy HH:mm}</td></tr>");
        sb.AppendLine("</table>");
        sb.AppendLine("<p style='margin-top: 20px;'><strong>Acción requerida:</strong> Revise y apruebe la ficha en MATRIX</p>");
        sb.AppendLine("</div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        return sb.ToString();
    }

    private string GenerarCuerpoCambioEstado(string numeroTrabajo, string estadoAnterior, string estadoNuevo, string? observaciones)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<html>");
        sb.AppendLine("<head><meta charset='UTF-8'></head>");
        sb.AppendLine("<body style='font-family: Arial, sans-serif;'>");
        sb.AppendLine("<div style='background-color: #f5f5f5; padding: 20px;'>");
        sb.AppendLine("<h2 style='color: #0066cc;'>Cambio de Estado del Trabajo</h2>");
        sb.AppendLine("<p>El estado del trabajo ha cambiado:</p>");
        sb.AppendLine("<table style='border-collapse: collapse; width: 100%; max-width: 600px;'>");
        sb.AppendLine($"<tr><td style='padding: 10px; border: 1px solid #ddd;'><strong>Trabajo:</strong></td><td style='padding: 10px; border: 1px solid #ddd;'>{numeroTrabajo}</td></tr>");
        sb.AppendLine($"<tr><td style='padding: 10px; border: 1px solid #ddd;'><strong>Estado Anterior:</strong></td><td style='padding: 10px; border: 1px solid #ddd;'>{estadoAnterior}</td></tr>");
        sb.AppendLine($"<tr><td style='padding: 10px; border: 1px solid #ddd;'><strong>Estado Nuevo:</strong></td><td style='padding: 10px; border: 1px solid #ddd;'><span style='background-color: #d4edda; padding: 5px;'>{estadoNuevo}</span></td></tr>");
        sb.AppendLine($"<tr><td style='padding: 10px; border: 1px solid #ddd;'><strong>Fecha:</strong></td><td style='padding: 10px; border: 1px solid #ddd;'>{DateTime.Now:dd/MM/yyyy HH:mm}</td></tr>");
        if (!string.IsNullOrEmpty(observaciones))
        {
            sb.AppendLine($"<tr><td style='padding: 10px; border: 1px solid #ddd;'><strong>Observaciones:</strong></td><td style='padding: 10px; border: 1px solid #ddd;'>{observaciones}</td></tr>");
        }
        sb.AppendLine("</table>");
        sb.AppendLine("</div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        return sb.ToString();
    }

    private string GenerarCuerpoCierre(string numeroTrabajo, string codigoProyecto, string? observaciones)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<html>");
        sb.AppendLine("<head><meta charset='UTF-8'></head>");
        sb.AppendLine("<body style='font-family: Arial, sans-serif;'>");
        sb.AppendLine("<div style='background-color: #d4edda; padding: 20px;'>");
        sb.AppendLine("<h2 style='color: #155724;'>Trabajo Cerrado</h2>");
        sb.AppendLine("<p>El trabajo ha sido cerrado exitosamente:</p>");
        sb.AppendLine("<table style='border-collapse: collapse; width: 100%; max-width: 600px;'>");
        sb.AppendLine($"<tr><td style='padding: 10px; border: 1px solid #ddd;'><strong>Trabajo:</strong></td><td style='padding: 10px; border: 1px solid #ddd;'>{numeroTrabajo}</td></tr>");
        sb.AppendLine($"<tr><td style='padding: 10px; border: 1px solid #ddd;'><strong>Proyecto:</strong></td><td style='padding: 10px; border: 1px solid #ddd;'>{codigoProyecto}</td></tr>");
        sb.AppendLine($"<tr><td style='padding: 10px; border: 1px solid #ddd;'><strong>Fecha Cierre:</strong></td><td style='padding: 10px; border: 1px solid #ddd;'>{DateTime.Now:dd/MM/yyyy HH:mm}</td></tr>");
        if (!string.IsNullOrEmpty(observaciones))
        {
            sb.AppendLine($"<tr><td style='padding: 10px; border: 1px solid #ddd;'><strong>Observaciones:</strong></td><td style='padding: 10px; border: 1px solid #ddd;'>{observaciones}</td></tr>");
        }
        sb.AppendLine("</table>");
        sb.AppendLine("<p style='margin-top: 20px;'><strong>Estado:</strong> ✓ Cerrado</p>");
        sb.AppendLine("</div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        return sb.ToString();
    }

    private string GenerarCuerpoCustomizado(ParamsNotificacionFichaDto parametros)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<html>");
        sb.AppendLine("<head><meta charset='UTF-8'></head>");
        sb.AppendLine("<body style='font-family: Arial, sans-serif;'>");
        sb.AppendLine("<div style='background-color: #f5f5f5; padding: 20px;'>");
        sb.AppendLine($"<h2 style='color: #0066cc;'>{parametros.TipoNotificacion}</h2>");
        sb.AppendLine("<table style='border-collapse: collapse; width: 100%; max-width: 600px;'>");
        sb.AppendLine($"<tr><td style='padding: 10px; border: 1px solid #ddd;'><strong>Trabajo:</strong></td><td style='padding: 10px; border: 1px solid #ddd;'>{parametros.NumeroTrabajo}</td></tr>");
        sb.AppendLine($"<tr><td style='padding: 10px; border: 1px solid #ddd;'><strong>Proyecto:</strong></td><td style='padding: 10px; border: 1px solid #ddd;'>{parametros.CodigoProyecto} - {parametros.NombreProyecto}</td></tr>");
        sb.AppendLine($"<tr><td style='padding: 10px; border: 1px solid #ddd;'><strong>Fecha:</strong></td><td style='padding: 10px; border: 1px solid #ddd;'>{parametros.FechaNotificacion:dd/MM/yyyy HH:mm}</td></tr>");
        if (!string.IsNullOrEmpty(parametros.Observaciones))
        {
            sb.AppendLine($"<tr><td style='padding: 10px; border: 1px solid #ddd;'><strong>Observaciones:</strong></td><td style='padding: 10px; border: 1px solid #ddd;'>{parametros.Observaciones}</td></tr>");
        }
        sb.AppendLine("</table>");
        sb.AppendLine("</div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        return sb.ToString();
    }
}
