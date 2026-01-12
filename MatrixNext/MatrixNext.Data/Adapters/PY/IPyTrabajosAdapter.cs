using System.Threading.Tasks;
using MatrixNext.Data.Adapters.PY.Models;

namespace MatrixNext.Data.Adapters.PY
{
    public interface IPyTrabajosAdapter
    {
        Task<TrabajoConfiguracionDto?> ObtenerConfiguracionTrabajo(long trabajoId);
        Task GuardarConfiguracionTrabajo(long trabajoId, string configuracion, long usuarioId);
        Task<DuplicarTrabajoResultDto> DuplicarTrabajoCompleto(DuplicarTrabajoInputDto input);
    }
}
