using MatrixNext.Data.Modules.US.Feedback.Adapters;
using MatrixNext.Data.Modules.US.Feedback.Models;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Modules.US.Feedback.Services;

/// <summary>
/// Servicio para Feedback
/// Ref: CoreProject/Clases/CORE/Feedback.vb
/// </summary>
public interface IFeedbackService
{
    Task<IEnumerable<AsuntoDto>> ObtenerAsuntosAsync();
    Task<(bool Success, string Message)> EnviarFeedbackAsync(FeedbackCreateDto dto, long idUsuario);
    Task<IEnumerable<FeedbackDto>> ObtenerPendientesAsync();
    Task<IEnumerable<FeedbackDto>> ObtenerResueltosAsync();
    Task<FeedbackDto?> ObtenerPorIdAsync(long id);
    Task<(bool Success, string Message)> ResponderFeedbackAsync(FeedbackUpdateDto dto, long idUsuarioResponde);
}

public class FeedbackService : IFeedbackService
{
    private readonly IFeedbackAdapter _adapter;
    private readonly ILogger<FeedbackService> _logger;

    public FeedbackService(IFeedbackAdapter adapter, ILogger<FeedbackService> logger)
    {
        _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<AsuntoDto>> ObtenerAsuntosAsync()
    {
        try
        {
            return await _adapter.ObtenerAsuntosAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener asuntos de feedback");
            return Enumerable.Empty<AsuntoDto>();
        }
    }

    public async Task<(bool Success, string Message)> EnviarFeedbackAsync(FeedbackCreateDto dto, long idUsuario)
    {
        try
        {
            if (dto.IdAsunto <= 0)
                return (false, "Debe seleccionar un asunto");

            if (string.IsNullOrWhiteSpace(dto.Mensaje))
                return (false, "El mensaje es requerido");

            await _adapter.EnviarFeedbackAsync(idUsuario, dto.IdAsunto, dto.Mensaje.Trim());

            _logger.LogInformation("Feedback enviado. Usuario: {Usuario}, Asunto: {Asunto}", idUsuario, dto.IdAsunto);
            return (true, "Mensaje enviado correctamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar feedback. Usuario: {Usuario}", idUsuario);
            return (false, "Error al enviar el mensaje. Por favor intente nuevamente.");
        }
    }

    public async Task<IEnumerable<FeedbackDto>> ObtenerPendientesAsync()
    {
        try
        {
            return await _adapter.ObtenerFeedbackPendientesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener feedback pendientes");
            return Enumerable.Empty<FeedbackDto>();
        }
    }

    public async Task<IEnumerable<FeedbackDto>> ObtenerResueltosAsync()
    {
        try
        {
            return await _adapter.ObtenerFeedbackResueltosAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener feedback resueltos");
            return Enumerable.Empty<FeedbackDto>();
        }
    }

    public async Task<FeedbackDto?> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await _adapter.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener feedback por Id: {Id}", id);
            return null;
        }
    }

    public async Task<(bool Success, string Message)> ResponderFeedbackAsync(FeedbackUpdateDto dto, long idUsuarioResponde)
    {
        try
        {
            if (dto.Id <= 0)
                return (false, "El Id del feedback es requerido");

            var existente = await _adapter.ObtenerPorIdAsync(dto.Id);
            if (existente == null)
                return (false, "El feedback no existe");

            await _adapter.ActualizarFeedbackAsync(dto.Id, dto.Respuesta, dto.Solucionado, idUsuarioResponde);

            var mensaje = dto.Solucionado ? "Feedback marcado como solucionado" : "Respuesta guardada";
            _logger.LogInformation("Feedback respondido. Id: {Id}, Solucionado: {Solucionado}", dto.Id, dto.Solucionado);
            return (true, mensaje);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al responder feedback. Id: {Id}", dto.Id);
            return (false, "Error al responder el feedback. Por favor intente nuevamente.");
        }
    }
}
