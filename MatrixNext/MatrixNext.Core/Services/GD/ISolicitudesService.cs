/// <summary>
/// Interface para Service de Solicitudes de Documentos
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.3.1
/// </summary>
namespace MatrixNext.Core.Services.GD
{
    using MatrixNext.Core.DTOs.GD;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISolicitudesService
    {
        Task<IEnumerable<SolicitudDocumentoDto>> ObtenerSolicitudesAsync(long? idProyecto = null, long? idEstado = null, long? idSolicitante = null);
        Task<SolicitudDocumentoDto> ObtenerSolicitudAsync(long idSolicitud);
        Task<(bool exitoso, string mensaje, long idSolicitud)> CrearSolicitudAsync(SolicitudDocumentoDto solicitud, bool asignacionAutomatica = true);
        Task<(bool exitoso, string mensaje)> ActualizarSolicitudAsync(SolicitudDocumentoDto solicitud);
        Task<(bool exitoso, string mensaje)> AsignarRevisoresAsync(AsignacionRevisoresDto asignacion, long usuarioId);
        Task<ConfiguracionRevisionDto> ObtenerConfiguracionRevisionAsync(long idProceso);

        /// <summary>
        /// Aprobar una revisión y cambiar estado de solicitud automáticamente si todos aprobaron
        /// </summary>
        Task<(bool exitoso, string mensaje)> AprobarRevisionAsync(AprobacionRevisionDto aprobacion);

        /// <summary>
        /// Rechazar una revisión y cambiar estado de solicitud a Rechazado automáticamente
        /// </summary>
        Task<(bool exitoso, string mensaje)> RechazarRevisionAsync(AprobacionRevisionDto rechazo);

        /// <summary>
        /// Obtener resumen de aprobaciones de una solicitud
        /// </summary>
        Task<ResumenAprobacionDto> ObtenerResumenAprobacionAsync(long idSolicitud);

        /// <summary>
        /// Obtener historial de revisiones (Audit Trail) de una solicitud
        /// </summary>
        Task<IEnumerable<HistorialRevisionDto>> ObtenerHistorialRevisionesAsync(long idSolicitud);

        /// <summary>
        /// Obtener timeline completo de una solicitud
        /// </summary>
        Task<TimelineSolicitudDto> ObtenerTimelineSolicitudAsync(long idSolicitud);
    }
}
