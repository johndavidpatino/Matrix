using Dapper;
using MatrixNext.Data.Models.RP;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MatrixNext.Data.Adapters.RP
{
    /// <summary>
    /// Implementación de Adapter para Reportes
    /// NOTA: Varios SP de este módulo NO EXISTEN en la BD legacy (CO_Matrix_Intranet)
    /// Los métodos afectados retornan valores vacíos/default hasta que se creen los SP correspondientes
    /// </summary>
    public class ReportesAdapter : IReportesAdapter
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<ReportesAdapter> _logger;

        public ReportesAdapter(IDbConnection dbConnection, ILogger<ReportesAdapter> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }

        // ============================================
        // INDICADORES Y DASHBOARDS
        // ============================================

        /// <summary>
        /// STUB: SP REP_IndicadoresCalidad_Get no existe en BD legacy
        /// </summary>
        public Task<List<Dictionary<string, object>>> GetIndicadoresCalidadAsync(
            DateTime fechaDesde, DateTime fechaHasta, int? usuarioId = null)
        {
            ValidarRangoFechas(fechaDesde, fechaHasta);
            _logger.LogWarning("[RP] GetIndicadoresCalidadAsync: SP 'REP_IndicadoresCalidad_Get' no existe en BD legacy. Retornando lista vacía. FechaDesde={FechaDesde}, FechaHasta={FechaHasta}", fechaDesde, fechaHasta);
            return Task.FromResult(new List<Dictionary<string, object>>());
        }

        /// <summary>
        /// STUB: SP REP_IndicadoresCumplimiento_Get no existe en BD legacy
        /// </summary>
        public Task<List<Dictionary<string, object>>> GetIndicadoresCumplimientoAsync(
            DateTime fechaDesde, DateTime fechaHasta, int? usuarioId = null)
        {
            ValidarRangoFechas(fechaDesde, fechaHasta);
            _logger.LogWarning("[RP] GetIndicadoresCumplimientoAsync: SP 'REP_IndicadoresCumplimiento_Get' no existe en BD legacy. Retornando lista vacía. FechaDesde={FechaDesde}, FechaHasta={FechaHasta}", fechaDesde, fechaHasta);
            return Task.FromResult(new List<Dictionary<string, object>>());
        }

        public async Task<List<Dictionary<string, object>>> GetReportDataAsync(
            string spName, Dictionary<string, object> parameters)
        {
            try
            {
                if (string.IsNullOrEmpty(spName))
                    throw new ArgumentException("spName no puede estar vacío");

                _logger.LogInformation("[RP] Ejecutando SP genérico: {SpName}", spName);

                var dynamicParams = new DynamicParameters();
                foreach (var param in parameters)
                {
                    dynamicParams.Add($"@{param.Key}", param.Value);
                }

                var result = await _dbConnection.QueryAsync<dynamic>(
                    spName,
                    dynamicParams,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                var dictList = ConvertirDynamicADictionary(result);
                _logger.LogInformation("[RP] SP {SpName} retornó {Count} registros", spName, dictList.Count);

                return dictList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RP] Error ejecutando SP genérico: {SpName}", spName);
                throw;
            }
        }

        // ============================================
        // REPORTES DE OPERACIÓN
        // ============================================

        /// <summary>
        /// STUB: SP OP_ReporteActividades_Get no existe en BD legacy
        /// </summary>
        public Task<List<Dictionary<string, object>>> GetReporteActividadesAsync(
            DateTime fechaDesde, DateTime fechaHasta, int? usuarioId = null)
        {
            ValidarRangoFechas(fechaDesde, fechaHasta);
            _logger.LogWarning("[RP-OP] GetReporteActividadesAsync: SP 'OP_ReporteActividades_Get' no existe en BD legacy. Retornando lista vacía. FechaDesde={FechaDesde}, FechaHasta={FechaHasta}", fechaDesde, fechaHasta);
            return Task.FromResult(new List<Dictionary<string, object>>());
        }

        /// <summary>
        /// STUB: SP OP_ReporteInconsistencias_Get no existe en BD legacy
        /// </summary>
        public Task<List<Dictionary<string, object>>> GetReporteInconsistenciasAsync(
            DateTime fechaDesde, DateTime fechaHasta, string? tipo = null)
        {
            ValidarRangoFechas(fechaDesde, fechaHasta);
            _logger.LogWarning("[RP-OP] GetReporteInconsistenciasAsync: SP 'OP_ReporteInconsistencias_Get' no existe en BD legacy. Retornando lista vacía. FechaDesde={FechaDesde}, FechaHasta={FechaHasta}", fechaDesde, fechaHasta);
            return Task.FromResult(new List<Dictionary<string, object>>());
        }

        /// <summary>
        /// STUB: SP OP_ReporteListadoTrabajos_Get no existe en BD legacy
        /// </summary>
        public Task<List<Dictionary<string, object>>> GetReporteListadoTrabajosAsync(
            DateTime fechaDesde, DateTime fechaHasta, int? proyectoId = null)
        {
            ValidarRangoFechas(fechaDesde, fechaHasta);
            _logger.LogWarning("[RP-OP] GetReporteListadoTrabajosAsync: SP 'OP_ReporteListadoTrabajos_Get' no existe en BD legacy. Retornando lista vacía. FechaDesde={FechaDesde}, FechaHasta={FechaHasta}", fechaDesde, fechaHasta);
            return Task.FromResult(new List<Dictionary<string, object>>());
        }

        // ============================================
        // REPORTES DE PLANEACIÓN
        // ============================================

        /// <summary>
        /// STUB: SP PY_PlaneacionCampo_Get no existe en BD legacy
        /// </summary>
        public Task<List<Dictionary<string, object>>> GetPlaneacionCampoAsync(
            DateTime fechaDesde, DateTime fechaHasta, int? areaId = null)
        {
            ValidarRangoFechas(fechaDesde, fechaHasta);
            _logger.LogWarning("[RP-PY] GetPlaneacionCampoAsync: SP 'PY_PlaneacionCampo_Get' no existe en BD legacy. Retornando lista vacía. FechaDesde={FechaDesde}, FechaHasta={FechaHasta}", fechaDesde, fechaHasta);
            return Task.FromResult(new List<Dictionary<string, object>>());
        }

        /// <summary>
        /// STUB: SP PY_PlaneacionEstudios_Get no existe en BD legacy
        /// </summary>
        public Task<List<Dictionary<string, object>>> GetPlaneacionEstudiosAsync(
            DateTime fechaDesde, DateTime fechaHasta)
        {
            ValidarRangoFechas(fechaDesde, fechaHasta);
            _logger.LogWarning("[RP-PY] GetPlaneacionEstudiosAsync: SP 'PY_PlaneacionEstudios_Get' no existe en BD legacy. Retornando lista vacía. FechaDesde={FechaDesde}, FechaHasta={FechaHasta}", fechaDesde, fechaHasta);
            return Task.FromResult(new List<Dictionary<string, object>>());
        }

        // ============================================
        // REPORTES DE RECURSOS
        // ============================================

        /// <summary>
        /// STUB: SP TH_ListadoEncuestadores_Get no existe en BD legacy
        /// </summary>
        public Task<List<Dictionary<string, object>>> GetListadoEncuestadoresAsync(
            int? areaId = null, string? estado = null)
        {
            _logger.LogWarning("[RP-TH] GetListadoEncuestadoresAsync: SP 'TH_ListadoEncuestadores_Get' no existe en BD legacy. Retornando lista vacía. AreaId={AreaId}, Estado={Estado}", areaId, estado);
            return Task.FromResult(new List<Dictionary<string, object>>());
        }

        /// <summary>
        /// STUB: SP TH_FichaEncuestador_Get no existe en BD legacy
        /// </summary>
        public Task<Dictionary<string, object>> GetFichaEncuestadorAsync(int idEncuestador)
        {
            _logger.LogWarning("[RP-TH] GetFichaEncuestadorAsync: SP 'TH_FichaEncuestador_Get' no existe en BD legacy. Retornando diccionario vacío. IdEncuestador={IdEncuestador}", idEncuestador);
            return Task.FromResult(new Dictionary<string, object>());
        }

        /// <summary>
        /// STUB: SP OP_PersonalSinProduccion_Get no existe en BD legacy
        /// </summary>
        public Task<List<Dictionary<string, object>>> GetPersonalSinProduccionAsync(
            DateTime fecha, int? areaId = null)
        {
            _logger.LogWarning("[RP-OP] GetPersonalSinProduccionAsync: SP 'OP_PersonalSinProduccion_Get' no existe en BD legacy. Retornando lista vacía. Fecha={Fecha}, AreaId={AreaId}", fecha, areaId);
            return Task.FromResult(new List<Dictionary<string, object>>());
        }

        // ============================================
        // VALIDACIONES Y UTILITARIAS
        // ============================================

        public void ValidarParametros(ReporteFiltrosDTO filtros)
        {
            if (filtros == null)
                throw new ArgumentNullException(nameof(filtros));

            var fechaDesde = filtros.FechaDesde ?? DateTime.Now.AddMonths(-1);
            var fechaHasta = filtros.FechaHasta ?? DateTime.Now;

            if (fechaDesde > fechaHasta)
                throw new InvalidOperationException("La fecha inicial no puede ser mayor a la fecha final");

            if ((fechaHasta - fechaDesde).TotalDays > 365)
                throw new InvalidOperationException("El rango de fechas no puede exceder 365 días");

            if (filtros.PageNumber < 1)
                throw new ArgumentException("PageNumber debe ser mayor a 0");

            if (filtros.PageSize < 1 || filtros.PageSize > 1000)
                throw new ArgumentException("PageSize debe estar entre 1 y 1000");
        }

        /// <summary>
        /// STUB: SP REP_ReportesDisponibles_Get no existe en BD legacy
        /// </summary>
        public Task<List<ReporteDTO>> GetReportesDisponiblesAsync()
        {
            _logger.LogWarning("[RP] GetReportesDisponiblesAsync: SP 'REP_ReportesDisponibles_Get' no existe en BD legacy. Retornando lista vacía.");
            return Task.FromResult(new List<ReporteDTO>());
        }

        // ============================================
        // HELPERS PRIVADOS
        // ============================================

        /// <summary>
        /// Convierte objeto dinámico de Dapper a Dictionary
        /// Facilita manejo flexible de columnas
        /// </summary>
        private List<Dictionary<string, object>> ConvertirDynamicADictionary(IEnumerable<dynamic> results)
        {
            return results
                .Select(r => new Dictionary<string, object>((IDictionary<string, object>)r))
                .ToList();
        }

        /// <summary>
        /// Valida rango de fechas
        /// </summary>
        private void ValidarRangoFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            if (fechaDesde > fechaHasta)
                throw new ArgumentException("FechaDesde no puede ser mayor a FechaHasta");

            if ((fechaHasta - fechaDesde).TotalDays > 365)
                throw new ArgumentException("Rango de fechas no puede exceder 365 días");
        }
    }
}
