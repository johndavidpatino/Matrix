using MatrixNext.Data.DTOs.RP;

namespace MatrixNext.Data.Services.RP;

/// <summary>
/// Interfaz para reportes de indicadores de calidad
/// </summary>
public interface IIndicadoresCalidadService
{
    /// <summary>
    /// Obtiene indicadores de esquema de análisis
    /// SP: REP_Diligenciamiento_Esquema_Analisis
    /// </summary>
    Task<(List<EsquemaAnalisisResumenDto> Resumen, List<EsquemaAnalisisDto> Detalle)> 
        ObtenerEsquemaAnalisisAsync(short? año, short? mes, short? estado, string? usuario);
    
    /// <summary>
    /// Obtiene indicadores de diligenciamiento de Brief
    /// SP: REP_Porcentaje_Diligenciamiento_Brief
    /// </summary>
    Task<(List<DiligenciamientoBriefResumenDto> Resumen, List<DiligenciamientoBriefDto> Detalle)> 
        ObtenerDiligenciamientoBriefAsync(short? año, short? mes, string? usuario);
    
    /// <summary>
    /// Obtiene indicadores de envío propuestas 48 horas
    /// SP: REP_Envio_Propuestas_48Horas
    /// </summary>
    Task<(List<EnvioPropuestas48HorasResumenDto> Resumen, List<EnvioPropuestas48HorasDto> Detalle)> 
        ObtenerEnvioPropuestas48HorasAsync(short? año, short? mes, short? estado, string? usuario);
    
    /// <summary>
    /// Obtiene años disponibles para dropdown
    /// </summary>
    Task<List<int>> ObtenerAñosDisponiblesAsync();
    
    /// <summary>
    /// Obtiene usuarios (gerentes) disponibles para dropdown
    /// </summary>
    Task<List<string>> ObtenerUsuariosDisponiblesAsync(int tipoReporte);
    
    /// <summary>
    /// Prepara el ViewModel completo para la vista
    /// </summary>
    Task<IndicadoresCalidadViewModel> PrepararViewModelAsync(IndicadoresCalidadFiltrosDto filtros);
    
    /// <summary>
    /// Exporta indicadores a Excel
    /// </summary>
    Task<byte[]> ExportarExcelAsync(IndicadoresCalidadFiltrosDto filtros);
}
