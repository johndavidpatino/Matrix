/// <summary>
/// Interface para adapter de tráfico de encuestas
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.9
/// </summary>
namespace MatrixNext.Data.Adapters.OP;

using MatrixNext.Data.Models.OP;

public interface ITraficoAdapter
{
    Task<List<TraficoEncuestaDto>> ObtenerMovimientosAsync(FiltrosTraficoDto filtros);
    Task<ResumenTraficoDto> ObtenerResumenPorUnidadAsync(int idUnidad, long? idTrabajo = null);
    Task<long> RegistrarEnvioAsync(EnvioEncuestasDto envio);
    Task<bool> RegistrarRecepcionAsync(RecepcionEncuestasDto recepcion);
    Task<bool> RegistrarDevolucionAsync(DevolucionEncuestasDto devolucion);
    Task<List<PersonalTraficoDto>> ObtenerPersonalAsignadoAsync(long idMovimiento);
    Task<bool> AsignarPersonalAsync(AsignacionPersonalDto asignacion);
    Task<bool> ValidarCantidadDisponibleAsync(long idTrabajo, int idUnidadOrigen, int cantidad);
    Task<bool> ValidarPermisoUnidadAsync(long usuarioId, int idUnidad);
}
