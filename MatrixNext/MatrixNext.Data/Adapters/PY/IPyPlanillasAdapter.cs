using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.PY.Models;

namespace MatrixNext.Data.Adapters.PY
{
    public interface IPyPlanillasAdapter
    {
        // Catálogos
        Task<List<TecnicaDto>> ObtenerTecnicas(string tipoTecnica);
        Task<List<ModeradorDto>> ObtenerModeradores();

        // Planillas Moderación
        Task<int> CrearPlanillaModeracion(PlanillaModeracionInputDto input);
        Task ActualizarPlanillaModeracion(ActualizarEstadoPlanillaInputDto input);
        Task<PlanillaModeracionDto?> ObtenerPlanillaModeracionPorId(int idPlanilla);

        // Planillas Informes
        Task<int> CrearPlanillaInformes(PlanillaInformesInputDto input);
        Task ActualizarPlanillaInformes(ActualizarEstadoPlanillaInputDto input);
        Task<PlanillaInformesDto?> ObtenerPlanillaInformesPorId(int idPlanilla);

        // Listado y exportación
        Task<List<PlanillaListDto>> ObtenerPlanillasPaginadas(int pageSize, int pageIndex, string? filtro, short? idEstado);
        Task<List<PlanillaModeracionDto>> ObtenerPlanillasModeracionParaExportar(DateTime fechaInicio, DateTime fechaFinal);
        Task<List<PlanillaInformesDto>> ObtenerPlanillasInformesParaExportar(DateTime fechaInicio, DateTime fechaFinal);
    }
}
