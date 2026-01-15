/// <summary>
/// Interface para Adapter de Solicitudes de Documentos
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.3.1
/// </summary>
namespace MatrixNext.Infrastructure.Adapters.GD
{
    using MatrixNext.Core.DTOs.GD;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISolicitudesAdapter
    {
        Task<IEnumerable<SolicitudDocumentoDto>> ObtenerSolicitudesAsync(long? idProyecto = null, long? idEstado = null, long? idSolicitante = null);
        Task<SolicitudDocumentoDto> ObtenerSolicitudAsync(long idSolicitud);
        Task<long> CrearSolicitudAsync(SolicitudDocumentoDto solicitud);
        Task<bool> ActualizarSolicitudAsync(SolicitudDocumentoDto solicitud);
        Task<bool> CambiarEstadoSolicitudAsync(long idSolicitud, long idEstado, long usuarioId, string observaciones = null);
        Task<IEnumerable<RevisorDto>> ObtenerRevisoresAsync(long idSolicitud);
        Task<bool> AsignarRevisoresAsync(long idSolicitud, List<long> idsRevisores, long usuarioId);
        Task<ConfiguracionRevisionDto> ObtenerConfiguracionRevisionAsync(long idProceso);
        Task<List<long>> ObtenerRevisoresPorDefectoAsync(long idProceso);
        Task<bool> EnviarNotificacionRevisoresAsync(long idSolicitud, string contenido);
    }
}
