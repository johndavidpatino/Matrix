using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Web.DTOs;

namespace MatrixNext.Web.Services.EQ
{
    /// <summary>
    /// Interfaz para CRUD y operaciones de cotizaciones EasyQuote
    /// </summary>
    public interface IEasyQuoteService
    {
        /// <summary>
        /// Crear nueva cotización
        /// </summary>
        Task<ApiResponse<EasyQuoteHeaderDto>> CreateAsync(EasyQuoteCreateDto dto);

        /// <summary>
        /// Obtener cotización completa por Id
        /// </summary>
        Task<ApiResponse<EasyQuoteDetailDto>> GetAsync(int id);

        /// <summary>
        /// Listar cotizaciones con paginación
        /// </summary>
        Task<ApiResponse<PaginatedResult<EasyQuoteListDto>>> ListAsync(int page = 1, int pageSize = 20);

        /// <summary>
        /// Actualizar cotización
        /// </summary>
        Task<ApiResponse<EasyQuoteHeaderDto>> UpdateAsync(int id, EasyQuoteUpdateDto dto);

        /// <summary>
        /// Eliminar cotización
        /// </summary>
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }

    /// <summary>
    /// Interfaz para cálculos de costos
    /// </summary>
    public interface IEasyCostService
    {
        /// <summary>
        /// Calcular costos para una cotización
        /// </summary>
        Task<ApiResponse<EasyCostResultDto>> CalculateAsync(int quoteHeaderId);

        /// <summary>
        /// Obtener ultimo calculo de costos
        /// </summary>
        Task<ApiResponse<EasyCostResultDto>> GetLastCalculationAsync(int quoteHeaderId);

        /// <summary>
        /// Validar datos antes de calcular
        /// </summary>
        Task<ApiResponse<bool>> ValidateQuoteAsync(int quoteHeaderId);
    }

    /// <summary>
    /// Interfaz para gestión de tablas maestras
    /// </summary>
    public interface IEasyMasterService
    {
        /// <summary>
        /// Obtener matriz de precios por metodologia
        /// </summary>
        Task<ApiResponse<List<EasyMasterPrecioDto>>> GetPrecioMatrizAsync(string tipoMetodologia);

        /// <summary>
        /// Obtener horas por duracion
        /// </summary>
        Task<ApiResponse<EasyMasterScriptProcDto>> GetHorasByDuracionAsync(int duracionMin);

        /// <summary>
        /// Obtener tarifas por nivel OPS
        /// </summary>
        Task<ApiResponse<List<EasyMasterValorHoraDto>>> GetValorHoraOpsAsync();

        /// <summary>
        /// Obtener costos de insumos por NSE
        /// </summary>
        Task<ApiResponse<EasyMasterCostInsumosDto>> GetCostInsumosAsync(int nse);

        /// <summary>
        /// Obtener catalogo estadistica
        /// </summary>
        Task<ApiResponse<List<EasyMasterRateEstadisticaDto>>> GetRateEstadisticaAsync();

        /// <summary>
        /// Obtener tarifas locaciones
        /// </summary>
        Task<ApiResponse<List<EasyMasterLocacionesDto>>> GetLocacionesAsync();
    }
}
