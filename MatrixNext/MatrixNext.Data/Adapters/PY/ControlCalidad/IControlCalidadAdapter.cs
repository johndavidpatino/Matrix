using MatrixNext.Data.DTOs.PY.ControlCalidad;

namespace MatrixNext.Data.Adapters.PY.ControlCalidad
{
    /// <summary>
    /// Interfaz para acceso a datos de Control de Calidad
    /// </summary>
    public interface IControlCalidadAdapter
    {
        Task<List<ControlCalidadListDto>> ObtenerTodosAsync(int tipoProceso);
        
        Task<List<ControlCalidadListDto>> ObtenerPorTrabajoAsync(long trabajoId, int tipoProceso);
        
        Task<ControlCalidadDetailDto> ObtenerPorIdAsync(long id);
        
        Task<long> CrearAsync(ControlCalidadInputDto dto, int userId);
        
        Task EditarAsync(long id, ControlCalidadInputDto dto, int userId);
        
        Task EliminarAsync(long id);
        
        Task<List<DetalleControlCalidadDetailDto>> ObtenerDetallesAsync(long controlCalidadId);
        
        Task GuardarDetallesAsync(long controlCalidadId, List<DetalleControlCalidadInputDto> detalles, int userId);
    }
}

