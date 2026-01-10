using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Adapters.GD
{
    public interface IGdSolicitudesAdapter
    {
        Task<int> SolDocumentosAdd(object parameters);
        Task<int> RevisionesAdd(object parameters);
        Task<IEnumerable<dynamic>> UsuariosGet(object? parameters = null);
    }
}
