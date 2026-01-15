using MatrixNext.Data.DTOs.PY.ControlCalidad;

namespace MatrixNext.Data.Adapters.PY.ControlCalidad
{
    /// <summary>
    /// Interfaz para acceso a datos de Preguntas de evaluaciÃ³n
    /// </summary>
    public interface IPreguntasAdapter
    {
        Task<List<PreguntaListDto>> ObtenerTodasAsync();
        
        Task<List<PreguntaListDto>> ObtenerPorTipoAsync(int tipoProceso);
        
        Task<long> CrearAsync(PreguntaInputDto dto, int userId);
        
        Task EditarAsync(long id, PreguntaInputDto dto, int userId);
        
        Task<bool> ToggleActivoAsync(long id, int userId);
    }
}

