using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.GD.Interfaces
{
    public interface IGdSolicitudesService
    {
        Task<(bool success, IEnumerable<object> data)> ObtenerSolicitudes();
        Task<(bool success, int idCreado)> CrearSolicitud(object dto);
        Task<(bool success, string message)> AsignarRevisores(int solicitudId, IEnumerable<int> usuariosIds);
    }
}
