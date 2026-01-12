using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.PY.Models;

namespace MatrixNext.Data.Adapters.PY
{
    public interface IPyInstructivosAdapter
    {
        // Cuantitativo
        Task<EspecificacionTecnicaDto?> ObtenerEspecificacion(long trabajoId);
        Task<EspecificacionTecnicaDto?> ObtenerEspecificacionUltimaVersion(long trabajoId);
        Task<List<EspecificacionTecnicaDto>> ObtenerEspecificacionesLista(long trabajoId);
        Task<int> ContarVersionesEspecificacion(long trabajoId);
        Task<long> GuardarEspecificacion(EspecificacionTecnicaInputDto input);

        // Cualitativo
        Task<EspecificacionTecnicaCualiDto?> ObtenerEspecificacionCuali(long trabajoId);
        Task<EspecificacionTecnicaCualiDto?> ObtenerEspecificacionCualiUltimaVersion(long trabajoId);
        Task<List<EspecificacionTecnicaCualiDto>> ObtenerEspecificacionesCualiLista(long trabajoId);
        Task<int> ContarVersionesEspecificacionCuali(long trabajoId);
        Task<long> GuardarEspecificacionCuali(EspecificacionTecnicaCualiInputDto input);

        // Ayudas y reclutamiento cualitativos
        Task<List<AyudaCualiDto>> ObtenerAyudasCuali();
        Task<List<TipoReclutamientoCualiDto>> ObtenerTiposReclutamientoCuali();
        Task<List<int>> ObtenerAyudasRequeridasPorTrabajo(long trabajoId);
        Task<List<int>> ObtenerReclutamientoRequeridoPorTrabajo(long trabajoId);
        Task GuardarAyudasRequeridas(long trabajoId, List<int> ayudasSeleccionadas);
        Task GuardarReclutamientoRequerido(long trabajoId, List<int> tiposSeleccionados);
    }
}
