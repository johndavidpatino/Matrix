using MatrixNext.Data.DTOs.PY.ControlCalidad;

namespace MatrixNext.Data.Services.PY.Interfaces
{
    /// <summary>
    /// Interfaz para lógica de negocio de Preguntas de evaluación
    /// </summary>
    public interface IPreguntasService
    {
        Task<List<PreguntaListDto>> ObtenerPorTipoAsync(int tipoProceso);

        Task<List<PreguntaListDto>> ObtenerPorTipoProcesoAsync(int tipoProceso);
        
        Task<(bool success, string message, long id)> CrearAsync(PreguntaInputDto dto, int userId);
        
        Task<(bool success, string message)> EditarAsync(long id, PreguntaInputDto dto, int userId);
        
        Task<(bool success, string message, bool nuevoEstado)> ToggleActivoAsync(long id, int userId);
    }
}
