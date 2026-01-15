using MatrixNext.Web.DTOs.PY.ControlCalidad;

namespace MatrixNext.Infrastructure.Adapters.PY.ControlCalidad
{
    /// <summary>
    /// Interfaz para acceso a datos de Preguntas de evaluación
    /// </summary>
    public interface IPreguntasAdapter
    {
        Task<List<PreguntaListDto>> ObtenerTodasAsync();
        
        Task<List<PreguntaListDto>> ObtenerPorTipoAsync(int tipoProceso);
        
        Task<long> CrearAsync(PreguntaInputDto dto, int userId);
        
        Task EditarAsync(long id, PreguntaInputDto dto, int userId);
        
        Task ToggleActivoAsync(long id, int userId);
    }
}
