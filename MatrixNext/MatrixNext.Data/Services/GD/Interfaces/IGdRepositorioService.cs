using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.GD.Interfaces
{
    public interface IGdRepositorioService
    {
        Task<(bool success, IEnumerable<object> data)> ObtenerDocumentos(int? trabajoId = null);
        Task<(bool success, int idCreado)> UploadDocumento(object dto);
        Task<(bool success, string message)> EliminarDocumento(int id);
    }
}
