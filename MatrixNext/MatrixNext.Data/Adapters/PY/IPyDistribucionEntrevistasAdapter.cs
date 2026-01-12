using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.PY.Models;

namespace MatrixNext.Data.Adapters.PY
{
    public interface IPyDistribucionEntrevistasAdapter
    {
        Task<List<EntrevistaCualiDto>> ObtenerEntrevistasPorTrabajo(long trabajoId);
        Task<EntrevistaCualiDto?> ObtenerEntrevistaPorId(long id);
        Task<List<DistribucionEntrevistaDto>> ObtenerDistribucionesPorEntrevista(long entrevistaId);
        Task<DistribucionEntrevistaDto?> ObtenerDistribucionPorId(long id);
        Task<List<ModeradorCualiDto>> ObtenerModeradores();
        Task<List<LogEntrevistaCualiDto>> ObtenerLogEntrevistas(long distribucionId);
        Task<long> GuardarDistribucion(DistribucionEntrevistaInputDto input);
        Task ActualizarEstadoDistribucion(long distribucionId, short estado);
        Task GuardarLogEntrevista(long distribucionId, long entrevistaId, string usuario, short estado, string observacion);
    }
}
