/// <summary>
/// Interface para Adapter de Solicitudes de Documentos
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md Â§ Sprint 12.3.1
/// </summary>
namespace MatrixNext.Data.Adapters.GD
{
    using MatrixNext.Data.DTOs.GD;
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

        /// <summary>
        /// Aprobar una revisiÃ³n (TipoRevision = 2)
        /// </summary>
        Task<bool> AprobarRevisionAsync(AprobacionRevisionDto aprobacion);

        /// <summary>
        /// Rechazar una revisiÃ³n (TipoRevision = 3)
        /// </summary>
        Task<bool> RechazarRevisionAsync(AprobacionRevisionDto rechazo);

        /// <summary>
        /// Obtener resumen de aprobaciones de una solicitud
        /// </summary>
        Task<ResumenAprobacionDto> ObtenerResumenAprobacionAsync(long idSolicitud);

        /// <summary>
        /// Obtener historial de revisiones (Audit Trail) de una solicitud
        /// </summary>
        Task<IEnumerable<HistorialRevisionDto>> ObtenerHistorialRevisionesAsync(long idSolicitud);

        /// <summary>
        /// Obtener timeline completo de una solicitud (solicitud + revisiones)
        /// </summary>
        Task<TimelineSolicitudDto> ObtenerTimelineSolicitudAsync(long idSolicitud);
    }
}

