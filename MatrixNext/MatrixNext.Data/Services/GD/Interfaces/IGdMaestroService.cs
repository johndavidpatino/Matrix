using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.GD.Interfaces
{
    public interface IGdMaestroService
    {
        Task<(bool success, IEnumerable<object> data)> ObtenerMaestros();
        Task<(bool success, object? data)> ObtenerMaestroById(int id);
        Task<(bool success, int idCreado)> CrearMaestro(object dto);
        Task<(bool success, string message)> ActualizarMaestro(int id, object dto);
        Task<(bool success, string message)> AnularMaestro(int id);
    }
}
