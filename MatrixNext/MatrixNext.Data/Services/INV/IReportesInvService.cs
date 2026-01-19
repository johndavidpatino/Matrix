using MatrixNext.Data.DTOs.INV;

namespace MatrixNext.Data.Services.INV;

/// <summary>
/// Interfaz para reportes de legalizaciones de inventario
/// </summary>
public interface IReportesInvService
{
    /// <summary>
    /// Obtiene reporte de legalizaciones con filtros
    /// SP: INV_ReporteLegalizaciones
    /// </summary>
    Task<IEnumerable<ReporteLegalizacionDto>> ObtenerReporteLegalizacionesAsync(ReporteLegalizacionFiltrosDto filtros);
    
    /// <summary>
    /// Obtiene reporte de remanente con filtros
    /// SP: INV_ReporteRemanente
    /// </summary>
    Task<IEnumerable<ReporteRemanenteDto>> ObtenerReporteRemanenteAsync(ReporteRemanenteFiltrosDto filtros);
    
    /// <summary>
    /// Obtiene lista de Business Units para dropdown
    /// </summary>
    Task<IEnumerable<(int Id, string Nombre)>> ObtenerBUsAsync();
    
    /// <summary>
    /// Obtiene tipos de artículo para dropdown
    /// </summary>
    Task<IEnumerable<(long Id, string Nombre)>> ObtenerTiposArticuloAsync();
    
    /// <summary>
    /// Obtiene tipos de producto para dropdown
    /// </summary>
    Task<IEnumerable<(long Id, string Nombre)>> ObtenerTiposProductoAsync();
    
    /// <summary>
    /// Exporta reporte de legalizaciones a Excel
    /// </summary>
    Task<byte[]> ExportarLegalizacionesExcelAsync(ReporteLegalizacionFiltrosDto filtros);
    
    /// <summary>
    /// Exporta reporte de remanente a Excel
    /// </summary>
    Task<byte[]> ExportarRemanenteExcelAsync(ReporteRemanenteFiltrosDto filtros);
}
