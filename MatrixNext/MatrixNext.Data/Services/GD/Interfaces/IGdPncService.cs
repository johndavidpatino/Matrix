using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Models.GD;

namespace MatrixNext.Data.Services.GD.Interfaces
{
    /// <summary>
    /// Servicio para gestión de Productos No Conformes (PNC)
    /// </summary>
    public interface IGdPncService
    {
        // Obtener PNC
        Task<(bool success, IEnumerable<PncDto> data)> ObtenerPncAsync(PncBusquedaParams? filtros = null);
        Task<(bool success, PncDto? data)> ObtenerPncPorIdAsync(long id);
        Task<(bool success, PncDetalleViewModel? data)> ObtenerDetalleCompletoAsync(long id);
        
        // Seguimiento
        Task<(bool success, IEnumerable<PncSeguimientoDto> data)> ObtenerSeguimientoAsync(byte? estado = null);
        
        // CRUD PNC
        Task<(bool success, long idCreado, string message)> CrearPncAsync(PncCrearDto dto, long usuarioId);
        Task<(bool success, string message)> ActualizarEstadoAsync(long id, byte estado, string observacion, long usuarioId);
        
        // Causas
        Task<(bool success, IEnumerable<PncCausaDto> data)> ObtenerCausasAsync(long pncId);
        Task<(bool success, long idCreado, string message)> CrearCausaAsync(long pncId, PncCausaDto dto, long usuarioId);
        
        // Acciones
        Task<(bool success, IEnumerable<PncAccionDto> data)> ObtenerAccionesAsync(long pncId);
        Task<(bool success, long idCreado, string message)> CrearAccionAsync(long pncId, long causaId, PncAccionDto dto, long usuarioId);
        
        // Log de estados
        Task<(bool success, IEnumerable<PncLogEstadoDto> data)> ObtenerHistorialEstadosAsync(long pncId);
        
        // Catálogos
        Task<(bool success, IEnumerable<CatalogoItem> data)> ObtenerProcesosAsync();
        Task<(bool success, IEnumerable<CatalogoItem> data)> ObtenerCategoriasAsync();
        Task<(bool success, IEnumerable<CatalogoItem> data)> ObtenerFuentesAsync();
        Task<(bool success, IEnumerable<CatalogoItem> data)> ObtenerProcedimientosAsync(byte procesoId);
        
        // ViewModel para Index
        Task<PncIndexViewModel> PrepararViewModelAsync(PncBusquedaParams? filtros, long usuarioId);
    }
}
