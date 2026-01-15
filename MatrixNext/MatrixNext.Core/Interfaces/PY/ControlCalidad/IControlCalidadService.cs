using MatrixNext.Web.DTOs.PY.ControlCalidad;

namespace MatrixNext.Core.Interfaces.PY.ControlCalidad
{
    /// <summary>
    /// Interfaz para lógica de negocio de Control de Calidad
    /// </summary>
    public interface IControlCalidadService
    {
        Task<List<ControlCalidadListDto>> ObtenerTodosAsync(int tipoProceso);
        
        Task<List<ControlCalidadListDto>> ObtenerPorTrabajoAsync(long trabajoId, int tipoProceso);
        
        Task<ControlCalidadDetailDto> ObtenerPorIdAsync(long id);
        
        Task<(bool success, string message, long id)> CrearAsync(ControlCalidadInputDto dto, int userId);
        
        Task<(bool success, string message)> EditarAsync(long id, ControlCalidadInputDto dto, int userId);
        
        Task<(bool success, string message)> EliminarAsync(long id, int userId);
        
        Task<List<PreguntaListDto>> ObtenerPreguntasActivasAsync(int tipoProceso);
    }
}
