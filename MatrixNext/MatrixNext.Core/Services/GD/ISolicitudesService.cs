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
    }
}
