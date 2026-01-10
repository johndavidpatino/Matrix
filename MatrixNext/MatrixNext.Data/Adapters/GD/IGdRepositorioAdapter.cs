using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Adapters.GD
{
    public interface IGdRepositorioAdapter
    {
        Task<IEnumerable<dynamic>> RepositorioDocumentosGetXTrabajo(int idTrabajo);
        Task<int> RepositorioDocumentosAdd(object parameters);
        Task<bool> EscanerDocumentosDel(int idContenedor);
    }
}
