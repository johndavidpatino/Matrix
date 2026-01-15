using MatrixNext.Data.Adapters.OP;
using MatrixNext.Data.Models.OP;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.OP;

/// <summary>
/// Implementación del servicio de notificaciones para OP
/// Envía logs de notificaciones sobre cambios en FichaCuantitativa y estados de trabajos
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.5
/// NOTE: Integración con IEmailService pendiente (evitar referencia circular MatrixNext.Data → MatrixNext.Web)
/// </summary>
public class OpNotificacionService : IOpNotificacionService
{
    private readonly INotificacionesOpAdapter _adapter;
    private readonly ILogger<OpNotificacionService> _logger;

    public OpNotificacionService(
        INotificacionesOpAdapter adapter,
        ILogger<OpNotificacionService> logger)
    {
        _adapter = adapter;
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
    /// Registra notificación de creación de FichaCuantitativa
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

            var emails = destinatarios.Select(d => d.EmailOrigen).Distinct().ToList();

            _logger.LogInformation(
                "Notificación de creación de ficha preparada. IdTrabajo: {IdTrabajo}, Trabajo: {NumeroTrabajo}, Proyecto: {CodigoProyecto}, Destinatarios: {Count}",
                idTrabajo, numeroTrabajo, codigoProyecto, destinatarios.Count());

            return (true, $"Notificación preparada para {destinatarios.Count()} destinatarios");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando notificación creación ficha. IdTrabajo: {IdTrabajo}", idTrabajo);
            return (false, "Error al procesar notificación");
        }
    }

    /// <summary>
    /// Registra notificación de cambio de estado
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

            var emails = destinatarios.Select(d => d.EmailOrigen).Distinct().ToList();

            _logger.LogInformation(
                "Notificación de cambio de estado preparada. IdTrabajo: {IdTrabajo}, Trabajo: {NumeroTrabajo}, {Anterior} → {Nuevo}, Destinatarios: {Count}",
                idTrabajo, numeroTrabajo, estadoAnterior, estadoNuevo, destinatarios.Count());

            return (true, "Notificación preparada");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error procesando notificación cambio estado. IdTrabajo: {IdTrabajo}",
                idTrabajo);
            return (false, "Error al procesar notificación");
        }
    }

    /// <summary>
    /// Registra notificación de cierre de trabajo
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

            var emails = destinatarios.Select(d => d.EmailOrigen).Distinct().ToList();

            _logger.LogInformation(
                "Notificación de cierre preparada. IdTrabajo: {IdTrabajo}, Trabajo: {NumeroTrabajo}, Proyecto: {CodigoProyecto}, Destinatarios: {Count}",
                idTrabajo, numeroTrabajo, codigoProyecto, destinatarios.Count());

            return (true, "Notificación de cierre preparada");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando notificación cierre. IdTrabajo: {IdTrabajo}", idTrabajo);
            return (false, "Error al procesar notificación");
        }
    }

    /// <summary>
    /// Envía notificación customizada
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

            var emails = parametros.Destinatarios
                .Where(d => !string.IsNullOrEmpty(d.EmailOrigen))
                .Select(d => d.EmailOrigen)
                .Distinct()
                .ToList();

            if (!emails.Any())
            {
                return (false, "No hay emails válidos para enviar");
            }

            _logger.LogInformation(
                "Notificación customizada preparada. IdTrabajo: {IdTrabajo}, Tipo: {Tipo}, Destinatarios: {Count}",
                parametros.IdTrabajo, parametros.TipoNotificacion, emails.Count);

            return (true, $"Notificación preparada para {emails.Count} destinatarios");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error procesando notificación customizada. IdTrabajo: {IdTrabajo}",
                parametros.IdTrabajo);
            return (false, "Error al procesar notificación");
        }
    }
}
