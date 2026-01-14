using MatrixNext.Data.Models.RP;
using MatrixNext.Data.Services;

namespace MatrixNext.Data.Services.RP
{
    /// <summary>
    /// Interfaz para Service de Reportes
    /// Responsable: Orquestar lógica de negocio para reportes
    /// REGLA 6: Validaciones complejas
    /// REGLA 7: Transformación datos
    /// REGLA 8: Gestión errores
    /// </summary>
    public interface IReportesService
    {
        // ============================================
        // GENERACIÓN DE REPORTES
        // ============================================

        /// <summary>
        /// Genera reporte con filtros aplicados
        /// Incluye paginación, filtrado, validación
        /// </summary>
        Task<ApiResponse<ReporteResultadoDTO>> GenerarReporteAsync(
            int reporteId,
            ReporteFiltrosDTO filtros,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene datos de un reporte específico con detalles
        /// </summary>
        Task<ApiResponse<ReporteDTO>> ObtenerReporteAsync(int reporteId, int usuarioId);

        /// <summary>
        /// Obtiene listado de reportes disponibles
        /// Para combo/dropdown en UI
        /// </summary>
        Task<ApiResponse<List<ReporteDTO>>> ObtenerReportesDisponiblesAsync();

        /// <summary>
        /// Valida acceso a reporte según permisos del usuario
        /// REGLA 9: Validación permisos
        /// </summary>
        Task<bool> ValidarAccesoReporteAsync(int reporteId, int usuarioId, string tipoAcceso = "Lectura");

        // ============================================
        // FILTROS Y BÚSQUEDA
        // ============================================

        /// <summary>
        /// Aplica filtros avanzados a datos del reporte
        /// Maneja: fechas, usuarios, estados, búsqueda texto, etc.
        /// </summary>
        Task<ApiResponse<ReporteResultadoDTO>> AplicarFiltrosAvanzadosAsync(
            ReporteFiltrosDTO filtros,
            List<Dictionary<string, object>> datos);

        /// <summary>
        /// Pagina resultados del reporte
        /// Implementa lógica de SKIP/TAKE
        /// </summary>
        ReporteResultadoDTO AplicarPaginacion(
            List<Dictionary<string, object>> datos,
            int pageNumber,
            int pageSize);

        // ============================================
        // EXPORTACIÓN DE DATOS
        // ============================================

        /// <summary>
        /// Prepara datos para exportar a Excel
        /// Incluye: formateo, cabeceras, validación
        /// </summary>
        Task<ReporteExportDTO> PrepararExportExcelAsync(
            int reporteId,
            ReporteFiltrosDTO filtros,
            int usuarioId);

        /// <summary>
        /// Prepara datos para exportar a PDF
        /// Incluye: formateo, estructura, validación
        /// </summary>
        Task<ReporteExportDTO> PrepararExportPdfAsync(
            int reporteId,
            ReporteFiltrosDTO filtros,
            int usuarioId);

        // ============================================
        // INDICADORES Y DASHBOARDS
        /// </summary>
        /// Obtiene indicadores de calidad para dashboard
        /// </summary>
        Task<ApiResponse<Dictionary<string, object>>> ObtenerIndicadoresCalidadAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            int usuarioId);

        /// <summary>
        /// Obtiene indicadores de cumplimiento
        /// </summary>
        Task<ApiResponse<Dictionary<string, object>>> ObtenerIndicadoresCumplimientoAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            int usuarioId);

        // ============================================
        // AUDITORÍA Y LOGGING
        // ============================================

        /// <summary>
        /// Registra generación de reporte en auditoría
        /// REGLA 8: Trazabilidad
        /// </summary>
        Task RegistrarAuditoriaAsync(int reporteId, int usuarioId, string accion, string detalles = null);
    }
}
