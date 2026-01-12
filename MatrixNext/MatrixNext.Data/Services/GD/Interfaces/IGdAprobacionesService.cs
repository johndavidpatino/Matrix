using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.GD.Interfaces
{
    public interface IGdAprobacionesService
    {
        Task<(bool success, IEnumerable<Adapters.GD.Models.RevisionAprobacionDto> data, string message)> ObtenerRevisionesPendientes(int usuarioId);
        Task<(bool success, string message)> AprobarRevision(int revisionId, int documentoId, int usuarioId);
    }
}
