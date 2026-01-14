using MatrixNext.Data.Models.RP;
using System.Data;

namespace MatrixNext.Data.Adapters.RP
{
    /// <summary>
    /// Interfaz para Adapter de Reportes
    /// Responsable: Consultar StoredProcedures para datos de reportes
    /// REGLA 2: Mapeo exacto de SP/Tablas según CoreProject
    /// REGLA 4: Ejecutar SP correspondientes
    /// </summary>
    public interface IReportesAdapter
    {
        // ============================================
        // INDICADORES Y DASHBOARDS
        // ============================================
        
        /// <summary>
        /// Obtiene indicadores de calidad por rango de fechas
        /// SP: REP_IndicadoresCalidad_Get (a validar en CoreProject)
        /// </summary>
        Task<List<Dictionary<string, object>>> GetIndicadoresCalidadAsync(
            DateTime fechaDesde, DateTime fechaHasta, int? usuarioId = null);

        /// <summary>
        /// Obtiene indicadores de cumplimiento de tareas
        /// SP: REP_IndicadoresCumplimiento_Get (a validar)
        /// </summary>
        Task<List<Dictionary<string, object>>> GetIndicadoresCumplimientoAsync(
            DateTime fechaDesde, DateTime fechaHasta, int? usuarioId = null);

        /// <summary>
        /// Obtiene datos genéricos desde un SP
        /// Método genérico para reutilizar con múltiples reportes
        /// </summary>
        Task<List<Dictionary<string, object>>> GetReportDataAsync(
            string spName, Dictionary<string, object> parameters);

        // ============================================
        // REPORTES DE OPERACIÓN
        // ============================================

        /// <summary>
        /// Obtiene reporte de actividades
        /// SP: OP_ReporteActividades_Get
        /// </summary>
        Task<List<Dictionary<string, object>>> GetReporteActividadesAsync(
            DateTime fechaDesde, DateTime fechaHasta, int? usuarioId = null);

        /// <summary>
        /// Obtiene reporte de inconsistencias
        /// SP: OP_ReporteInconsistencias_Get
        /// </summary>
        Task<List<Dictionary<string, object>>> GetReporteInconsistenciasAsync(
            DateTime fechaDesde, DateTime fechaHasta, string? tipo = null);

        /// <summary>
        /// Obtiene listado de trabajos para reporte
        /// SP: OP_ReporteListadoTrabajos_Get
        /// </summary>
        Task<List<Dictionary<string, object>>> GetReporteListadoTrabajosAsync(
            DateTime fechaDesde, DateTime fechaHasta, int? proyectoId = null);

        // ============================================
        // REPORTES DE PLANEACIÓN
        // ============================================

        /// <summary>
        /// Obtiene datos de planeación por campo
        /// SP: PY_PlaneacionCampo_Get
        /// </summary>
        Task<List<Dictionary<string, object>>> GetPlaneacionCampoAsync(
            DateTime fechaDesde, DateTime fechaHasta, int? areaId = null);

        /// <summary>
        /// Obtiene datos de planeación de estudios
        /// SP: PY_PlaneacionEstudios_Get
        /// </summary>
        Task<List<Dictionary<string, object>>> GetPlaneacionEstudiosAsync(
            DateTime fechaDesde, DateTime fechaHasta);

        // ============================================
        // REPORTES DE RECURSOS
        // ============================================

        /// <summary>
        /// Obtiene listado de encuestadores
        /// SP: TH_ListadoEncuestadores_Get
        /// </summary>
        Task<List<Dictionary<string, object>>> GetListadoEncuestadoresAsync(
            int? areaId = null, string? estado = null);

        /// <summary>
        /// Obtiene ficha de encuestador detallada
        /// SP: TH_FichaEncuestador_Get
        /// </summary>
        Task<Dictionary<string, object>> GetFichaEncuestadorAsync(int idEncuestador);

        /// <summary>
        /// Obtiene personal sin producción
        /// SP: OP_PersonalSinProduccion_Get
        /// </summary>
        Task<List<Dictionary<string, object>>> GetPersonalSinProduccionAsync(
            DateTime fecha, int? areaId = null);

        // ============================================
        // VALIDACIONES Y UTILITARIAS
        // ============================================

        /// <summary>
        /// Valida parámetros de entrada según reglas de negocio
        /// REGLA 5: Validación en capa de datos
        /// </summary>
        void ValidarParametros(ReporteFiltrosDTO filtros);

        /// <summary>
        /// Obtiene lista de reportes disponibles (maestro)
        /// Para listar en UI
        /// </summary>
        Task<List<ReporteDTO>> GetReportesDisponiblesAsync();
    }
}
