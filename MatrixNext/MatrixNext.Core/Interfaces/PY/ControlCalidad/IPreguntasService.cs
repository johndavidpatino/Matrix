using MatrixNext.Web.DTOs.PY.ControlCalidad;

namespace MatrixNext.Core.Interfaces.PY.ControlCalidad
{
    /// <summary>
    /// Interfaz para lógica de negocio de Preguntas de evaluación
    /// </summary>
    public interface IPreguntasService
    {
        Task<List<PreguntaListDto>> ObtenerPorTipoAsync(int tipoProceso);
        
        Task<(bool success, string message, long id)> CrearAsync(PreguntaInputDto dto, int userId);
        
        Task<(bool success, string message)> EditarAsync(long id, PreguntaInputDto dto, int userId);
        
        Task<(bool success, string message)> ToggleActivoAsync(long id, int userId);
    }
}
