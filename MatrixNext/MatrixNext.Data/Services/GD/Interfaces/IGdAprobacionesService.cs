using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.GD.Interfaces
{
    public interface IGdAprobacionesService
    {
        Task<(bool success, IEnumerable<object> data)> ObtenerRevisionesPendientes();
        Task<(bool success, string message)> AprobarRevision(int revisionId, string? observacion = null);
        Task<(bool success, string message)> RechazarRevision(int revisionId, string? observacion = null);
    }
}
