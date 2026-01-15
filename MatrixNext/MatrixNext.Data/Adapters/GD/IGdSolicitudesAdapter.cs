using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD.Models;
using TipoSolicitudDtoGD = MatrixNext.Data.Models.GD.TipoSolicitudDto;
using EstadoSolicitudDtoGD = MatrixNext.Data.Models.GD.EstadoSolicitudDto;

namespace MatrixNext.Data.Adapters.GD
{
    public interface IGdSolicitudesAdapter
    {
        // Lectura
        Task<List<SolicitudListDto>> ObtenerSolicitudes();
        Task<SolicitudDocumentoDto?> ObtenerSolicitudById(int id);

        // CreaciÃ³n
        Task<int> CrearSolicitud(SolicitudDocumentoDto dto);

        // Revisores
        Task<bool> CrearRevision(int idSolicitud, int idDocumentoControlado, int idRevisor);
        Task<List<RevisionDto>> ObtenerRevisoresPendientes(int idSolicitud);
        Task<int> ObtenerRevisoresAprobados(int idSolicitud);
        Task<int> ObtenerTotalRevisores(int idSolicitud);

        // Dropdowns
        Task<List<MaestroListDto>> ObtenerDocumentos();
        Task<List<UsuarioDto>> ObtenerUsuarios();
        Task<List<EstadoSolicitudDto>> ObtenerEstados();
        Task<List<TipoSolicitudDto>> ObtenerTiposSolicitud();
    }
}

