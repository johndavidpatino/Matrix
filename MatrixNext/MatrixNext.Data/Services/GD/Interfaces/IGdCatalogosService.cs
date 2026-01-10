using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Models.GD;

namespace MatrixNext.Data.Services.GD.Interfaces
{
    public interface IGdCatalogosService
    {
        Task<(bool success, List<TipoSolicitudDto> data)> ObtenerTipoSolicitudes();
        Task<(bool success, int idCreado)> CrearTipoSolicitud(TipoSolicitudDto dto);
        Task<(bool success, string message)> ActualizarTipoSolicitud(int id, TipoSolicitudDto dto);
        Task<(bool success, string message)> EliminarTipoSolicitud(int id);

        Task<(bool success, List<EstadoSolicitudDto> data)> ObtenerEstadosSolicitud();
        Task<(bool success, int idCreado)> CrearEstadoSolicitud(EstadoSolicitudDto dto);
        Task<(bool success, string message)> ActualizarEstadoSolicitud(int id, EstadoSolicitudDto dto);
        Task<(bool success, string message)> EliminarEstadoSolicitud(int id);

        Task<(bool success, List<ProcesoDto> data)> ObtenerProcesos();
        Task<(bool success, int idCreado)> CrearProceso(ProcesoDto dto);
        Task<(bool success, string message)> ActualizarProceso(int id, ProcesoDto dto);
        Task<(bool success, string message)> EliminarProceso(int id);
    }
}
