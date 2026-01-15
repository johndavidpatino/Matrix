/// <summary>
/// Interface para servicio de tráfico de encuestas
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.9
/// </summary>
namespace MatrixNext.Data.Services.OP;

using MatrixNext.Data.Models.OP;

public interface ITraficoService
{
    Task<List<TraficoEncuestaDto>> ObtenerMovimientosAsync(FiltrosTraficoDto filtros, long usuarioId);
    Task<ResumenTraficoDto> ObtenerResumenUnidadAsync(int idUnidad, long? idTrabajo, long usuarioId);
    Task<(bool Success, string Message, long IdMovimiento)> EnviarEncuestasAsync(EnvioEncuestasDto envio, long usuarioId);
    Task<(bool Success, string Message)> RecibirEncuestasAsync(RecepcionEncuestasDto recepcion, long usuarioId);
    Task<(bool Success, string Message)> DevolverEncuestasAsync(DevolucionEncuestasDto devolucion, long usuarioId);
    Task<List<PersonalTraficoDto>> ObtenerPersonalAsignadoAsync(long idMovimiento, long usuarioId);
    Task<(bool Success, string Message)> AsignarPersonalAsync(AsignacionPersonalDto asignacion, long usuarioId);
}
