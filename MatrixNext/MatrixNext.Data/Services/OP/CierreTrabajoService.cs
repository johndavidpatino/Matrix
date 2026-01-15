using MatrixNext.Data.Adapters.OP;
using MatrixNext.Data.Models.OP;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.OP;

/// <summary>
/// Implementación del servicio de cierre de trabajos
/// Coordina validación de documentos, cambio de estado y notificaciones
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.6
/// </summary>
public class CierreTrabajoService : ICierreTrabajoService
{
    private readonly ICierreTrabajoAdapter _adapter;
    private readonly IOpNotificacionService _notificacionService;
    private readonly ILogger<CierreTrabajoService> _logger;

    public CierreTrabajoService(
        ICierreTrabajoAdapter adapter,
        IOpNotificacionService notificacionService,
        ILogger<CierreTrabajoService> logger)
    {
        _adapter = adapter;
        _notificacionService = notificacionService;
        _logger = logger;
    }

    /// <summary>
    /// Valida que se cumplan requisitos para cierre
    /// </summary>
    public async Task<(bool CanClose, string Message)> ValidarRequisitosParaCierreAsync(long idTrabajo)
    {
        try
        {
            // Obtener trabajo
            var trabajo = await _adapter.ObtenerTrabajoAsync(idTrabajo);
            if (trabajo == null)
            {
                return (false, "Trabajo no encontrado");
            }

            // Validar documentos
            var validacionDocs = await _adapter.ValidarDocumentosAsync(idTrabajo);
            if (!validacionDocs.EsValido)
            {
                return (false, validacionDocs.MensajeError ?? "Documentos no válidos");
            }

            _logger.LogInformation("Requisitos para cierre validados. IdTrabajo: {IdTrabajo}", idTrabajo);
            return (true, "Requisitos cumplidos");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando requisitos para cierre. IdTrabajo: {IdTrabajo}", idTrabajo);
            return (false, "Error al validar requisitos");
        }
    }

    /// <summary>
    /// Ejecuta el cierre completo del trabajo
    /// </summary>
    public async Task<(bool Success, string Message)> CerrarTrabajoAsync(
        long idTrabajo,
        string? observaciones,
        long usuarioId)
    {
        try
        {
            _logger.LogInformation("Iniciando cierre de trabajo. IdTrabajo: {IdTrabajo}, UsuarioId: {UsuarioId}",
                idTrabajo, usuarioId);

            // 1. Validar requisitos
            var (canClose, validationMsg) = await ValidarRequisitosParaCierreAsync(idTrabajo);
            if (!canClose)
            {
                return (false, validationMsg);
            }

            // 2. Cambiar estado
            var estadoActualizado = await _adapter.CambiarEstadoACerradoAsync(idTrabajo, observaciones, usuarioId);
            if (!estadoActualizado)
            {
                return (false, "Error al cambiar estado del trabajo");
            }

            _logger.LogInformation("Estado del trabajo cambiado a Cerrado. IdTrabajo: {IdTrabajo}", idTrabajo);

            // 3. Enviar notificación de cierre
            var (numeroTrabajo, codigoProyecto, nombreProyecto) = 
                await _adapter.ObtenerDatosTrabajoAsync(idTrabajo);

            _ = await _notificacionService.NotificarCierreTrabajoAsync(
                idTrabajo: idTrabajo,
                numeroTrabajo: numeroTrabajo,
                codigoProyecto: codigoProyecto,
                observaciones: observaciones,
                usuarioId: usuarioId);

            _logger.LogInformation(
                "Trabajo cerrado exitosamente. IdTrabajo: {IdTrabajo}, Coordinadores notificados",
                idTrabajo);

            return (true, $"Trabajo {numeroTrabajo} cerrado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cerrando trabajo. IdTrabajo: {IdTrabajo}", idTrabajo);
            return (false, "Error al cerrar el trabajo");
        }
    }

    /// <summary>
    /// Obtiene información de validación de documentos
    /// </summary>
    public async Task<ValidacionDocumentosDto> ObtenerValidacionDocumentosAsync(long idTrabajo)
    {
        try
        {
            return await _adapter.ValidarDocumentosAsync(idTrabajo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo validación de documentos. IdTrabajo: {IdTrabajo}", idTrabajo);
            return new ValidacionDocumentosDto
            {
                EsValido = false,
                MensajeError = "Error al obtener validación de documentos"
            };
        }
    }
}
