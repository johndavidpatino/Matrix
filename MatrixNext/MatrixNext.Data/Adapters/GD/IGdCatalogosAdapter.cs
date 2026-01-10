using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Models.GD;

namespace MatrixNext.Data.Adapters.GD
{
    public interface IGdCatalogosAdapter
    {
        Task<List<TipoSolicitudDto>> ObtenerTipoSolicitudes();
        Task<int> CrearTipoSolicitud(string nombre, string? descripcion);
        Task<bool> ActualizarTipoSolicitud(int id, string nombre, string? descripcion);
        Task<bool> EliminarTipoSolicitud(int id);

        Task<List<EstadoSolicitudDto>> ObtenerEstadosSolicitud();
        Task<int> CrearEstadoSolicitud(string nombre, string? descripcion);
        Task<bool> ActualizarEstadoSolicitud(int id, string nombre, string? descripcion);
        Task<bool> EliminarEstadoSolicitud(int id);

        Task<List<ProcesoDto>> ObtenerProcesos();
        Task<int> CrearProceso(string nombre, string? descripcion);
        Task<bool> ActualizarProceso(int id, string nombre, string? descripcion);
        Task<bool> EliminarProceso(int id);
    }
}
