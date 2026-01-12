using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.PY.Models;

namespace MatrixNext.Data.Adapters.PY
{
    public interface IPyInHomeVisitAdapter
    {
        Task<List<InHomeVisitDto>> ObtenerInHomesPorTrabajo(long trabajoId);
        Task<InHomeVisitDto?> ObtenerInHomePorId(long id);
        Task<List<LogInHomeDto>> ObtenerLogInHome(long idInHome);
        Task<long> GuardarInHome(InHomeVisitInputDto input);
        Task GuardarLogInHome(long idInHome, long trabajoId, string usuario, string estado, string observacion);
    }
}
