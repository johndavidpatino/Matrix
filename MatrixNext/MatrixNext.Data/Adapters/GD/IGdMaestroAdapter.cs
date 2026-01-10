using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD.Models;

namespace MatrixNext.Data.Adapters.GD
{
    public interface IGdMaestroAdapter
    {
        // Lectura
        Task<List<MaestroDocumentoDto>> ObtenerMaestros();
        Task<MaestroDocumentoDto?> ObtenerMaestroById(int idMaestro);
        Task<DocumentoControlledDto?> ObtenerControlledDocById(int idMaestro);

        // Creación
        Task<int> CrearMaestroConControlled(MaestroDocumentoDto dto);

        // Actualización (por tipo)
        Task<bool> ActualizarMaestroConstitucion(int idMaestro, MaestroDocumentoDto dto);
        Task<bool> ActualizarMaestroActualizacion(int idMaestro, MaestroDocumentoDto dto);

        // Anulación
        Task<bool> AnularMaestro(int idMaestro);
        Task<bool> AnularControlado(int idMaestro);

        // Dropdowns
        Task<List<TipoSolicitudDto>> ObtenerTiposSolicitud();
        Task<List<ProcesoDto>> ObtenerProcesos();
        Task<List<UsuarioDto>> ObtenerUsuarios();
    }
}
