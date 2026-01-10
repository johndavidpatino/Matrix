using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD.Models;

namespace MatrixNext.Data.Adapters.GD
{
    public interface IGdRepositorioAdapter
    {
        Task<List<RepositorioListDto>> ObtenerDocumentos(int idContenedor, int tipoContenedor);
        Task<RepositorioDocumentoDto?> ObtenerDocumentoById(int id);
        Task<decimal> ObtenerProximaVersion(int idContenedor, int idDocumento);
        Task<int> GuardarDocumento(UploadDocumentoDto dto, decimal version);
        Task<bool> EliminarDocumento(int id);
        Task<List<RepositorioDocumentoDto>> ObtenerDocumentosContenedor(int idContenedor);
    }
}
