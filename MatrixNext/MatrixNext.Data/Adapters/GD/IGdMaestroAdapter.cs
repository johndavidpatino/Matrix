using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Adapters.GD
{
    public interface IGdMaestroAdapter
    {
        Task<IEnumerable<dynamic>> MaestroDocumentosGet();
        Task<int> MaestroDocumentosAdd(object parameters);
        Task<int> DocumentosControladosAdd(object parameters);
        Task<bool> DocumentosMaestrosUpdate(object parameters);
        Task<bool> DocumentosControladosActivo(int documentoId, bool activo);
    }
}
