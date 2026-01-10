using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD.Models;

namespace MatrixNext.Data.Services.GD.Interfaces
{
    public interface IGdRepositorioService
    {
        Task<(bool success, List<RepositorioListDto> data, string message)> ObtenerDocumentos(int idContenedor, int tipoContenedor);
        Task<(bool success, RepositorioDocumentoDto? data, string message)> ObtenerDocumento(int id);
        Task<(bool success, int idCreado, decimal version, string message)> SubirDocumento(UploadDocumentoDto dto);
        Task<(bool success, string message)> EliminarDocumento(int id);
    }
}
