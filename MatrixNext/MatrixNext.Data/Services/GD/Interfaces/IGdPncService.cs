using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.GD.Interfaces
{
    public interface IGdPncService
    {
        Task<(bool success, IEnumerable<object> data)> ObtenerPnc();
        Task<(bool success, int idCreado)> CrearPnc(object dto);
        Task<(bool success, string message)> ActualizarPnc(int id, object dto);
    }
}
