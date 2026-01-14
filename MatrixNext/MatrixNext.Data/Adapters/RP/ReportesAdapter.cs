using Dapper;
using MatrixNext.Data.Models.RP;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MatrixNext.Data.Adapters.RP
{
    /// <summary>
    /// Implementación de Adapter para Reportes
    /// Utiliza Dapper para ejecutar StoredProcedures contra BD
    /// REGLA 2: Mapeo exacto de nombres SP / parámetros desde CoreProject
    /// REGLA 3: Validación de respuestas
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

        public async Task<List<Dictionary<string, object>>> GetIndicadoresCalidadAsync(
            DateTime fechaDesde, DateTime fechaHasta, int? usuarioId = null)
        {
            try
            {
                ValidarRangoFechas(fechaDesde, fechaHasta);
                _logger.LogInformation($"[RP] Iniciando GetIndicadoresCalidad: {fechaDesde:yyyy-MM-dd} a {fechaHasta:yyyy-MM-dd}");

                var parameters = new DynamicParameters();
                parameters.Add("@FechaDesde", fechaDesde);
                parameters.Add("@FechaHasta", fechaHasta);
                if (usuarioId.HasValue)
                    parameters.Add("@UsuarioId", usuarioId.Value);

                // REGLA 2: SP exacto del CoreProject (a confirmar nombre)
                var result = await _dbConnection.QueryAsync<dynamic>(
                    "REP_IndicadoresCalidad_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                var dictList = ConvertirDynamicADictionary(result);
                _logger.LogInformation($"[RP] GetIndicadoresCalidad retornó {dictList.Count} registros");

                return dictList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RP] Error en GetIndicadoresCalidad");
                throw;
            }
        }

        public async Task<List<Dictionary<string, object>>> GetIndicadoresCumplimientoAsync(
            DateTime fechaDesde, DateTime fechaHasta, int? usuarioId = null)
        {
            try
            {
                ValidarRangoFechas(fechaDesde, fechaHasta);
                _logger.LogInformation($"[RP] Iniciando GetIndicadoresCumplimiento: {fechaDesde:yyyy-MM-dd} a {fechaHasta:yyyy-MM-dd}");

                var parameters = new DynamicParameters();
                parameters.Add("@FechaDesde", fechaDesde);
                parameters.Add("@FechaHasta", fechaHasta);
                if (usuarioId.HasValue)
                    parameters.Add("@UsuarioId", usuarioId.Value);

                // REGLA 2: SP exacto del CoreProject
                var result = await _dbConnection.QueryAsync<dynamic>(
                    "REP_IndicadoresCumplimiento_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                var dictList = ConvertirDynamicADictionary(result);
                _logger.LogInformation($"[RP] GetIndicadoresCumplimiento retornó {dictList.Count} registros");

                return dictList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RP] Error en GetIndicadoresCumplimiento");
                throw;
            }
        }

        public async Task<List<Dictionary<string, object>>> GetReportDataAsync(
            string spName, Dictionary<string, object> parameters)
        {
            try
            {
                if (string.IsNullOrEmpty(spName))
                    throw new ArgumentException("spName no puede estar vacío");

                _logger.LogInformation($"[RP] Ejecutando SP genérico: {spName}");

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
                _logger.LogInformation($"[RP] SP {spName} retornó {dictList.Count} registros");

                return dictList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[RP] Error ejecutando SP genérico: {spName}");
                throw;
            }
        }

        // ============================================
        // REPORTES DE OPERACIÓN
        // ============================================

        public async Task<List<Dictionary<string, object>>> GetReporteActividadesAsync(
            DateTime fechaDesde, DateTime fechaHasta, int? usuarioId = null)
        {
            try
            {
                ValidarRangoFechas(fechaDesde, fechaHasta);
                _logger.LogInformation($"[RP-OP] Iniciando GetReporteActividades");

                var parameters = new DynamicParameters();
                parameters.Add("@FechaDesde", fechaDesde);
                parameters.Add("@FechaHasta", fechaHasta);
                if (usuarioId.HasValue)
                    parameters.Add("@UsuarioId", usuarioId.Value);

                var result = await _dbConnection.QueryAsync<dynamic>(
                    "OP_ReporteActividades_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                return ConvertirDynamicADictionary(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RP-OP] Error en GetReporteActividades");
                throw;
            }
        }

        public async Task<List<Dictionary<string, object>>> GetReporteInconsistenciasAsync(
            DateTime fechaDesde, DateTime fechaHasta, string? tipo = null)
        {
            try
            {
                ValidarRangoFechas(fechaDesde, fechaHasta);
                _logger.LogInformation($"[RP-OP] Iniciando GetReporteInconsistencias");

                var parameters = new DynamicParameters();
                parameters.Add("@FechaDesde", fechaDesde);
                parameters.Add("@FechaHasta", fechaHasta);
                if (!string.IsNullOrEmpty(tipo))
                    parameters.Add("@Tipo", tipo);

                var result = await _dbConnection.QueryAsync<dynamic>(
                    "OP_ReporteInconsistencias_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                return ConvertirDynamicADictionary(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RP-OP] Error en GetReporteInconsistencias");
                throw;
            }
        }

        public async Task<List<Dictionary<string, object>>> GetReporteListadoTrabajosAsync(
            DateTime fechaDesde, DateTime fechaHasta, int? proyectoId = null)
        {
            try
            {
                ValidarRangoFechas(fechaDesde, fechaHasta);
                _logger.LogInformation($"[RP-OP] Iniciando GetReporteListadoTrabajos");

                var parameters = new DynamicParameters();
                parameters.Add("@FechaDesde", fechaDesde);
                parameters.Add("@FechaHasta", fechaHasta);
                if (proyectoId.HasValue)
                    parameters.Add("@ProyectoId", proyectoId.Value);

                var result = await _dbConnection.QueryAsync<dynamic>(
                    "OP_ReporteListadoTrabajos_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                return ConvertirDynamicADictionary(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RP-OP] Error en GetReporteListadoTrabajos");
                throw;
            }
        }

        // ============================================
        // REPORTES DE PLANEACIÓN
        // ============================================

        public async Task<List<Dictionary<string, object>>> GetPlaneacionCampoAsync(
            DateTime fechaDesde, DateTime fechaHasta, int? areaId = null)
        {
            try
            {
                ValidarRangoFechas(fechaDesde, fechaHasta);
                _logger.LogInformation($"[RP-PY] Iniciando GetPlaneacionCampo");

                var parameters = new DynamicParameters();
                parameters.Add("@FechaDesde", fechaDesde);
                parameters.Add("@FechaHasta", fechaHasta);
                if (areaId.HasValue)
                    parameters.Add("@AreaId", areaId.Value);

                var result = await _dbConnection.QueryAsync<dynamic>(
                    "PY_PlaneacionCampo_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                return ConvertirDynamicADictionary(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RP-PY] Error en GetPlaneacionCampo");
                throw;
            }
        }

        public async Task<List<Dictionary<string, object>>> GetPlaneacionEstudiosAsync(
            DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                ValidarRangoFechas(fechaDesde, fechaHasta);
                _logger.LogInformation($"[RP-PY] Iniciando GetPlaneacionEstudios");

                var parameters = new DynamicParameters();
                parameters.Add("@FechaDesde", fechaDesde);
                parameters.Add("@FechaHasta", fechaHasta);

                var result = await _dbConnection.QueryAsync<dynamic>(
                    "PY_PlaneacionEstudios_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                return ConvertirDynamicADictionary(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RP-PY] Error en GetPlaneacionEstudios");
                throw;
            }
        }

        // ============================================
        // REPORTES DE RECURSOS
        // ============================================

        public async Task<List<Dictionary<string, object>>> GetListadoEncuestadoresAsync(
            int? areaId = null, string? estado = null)
        {
            try
            {
                _logger.LogInformation($"[RP-TH] Iniciando GetListadoEncuestadores");

                var parameters = new DynamicParameters();
                if (areaId.HasValue)
                    parameters.Add("@AreaId", areaId.Value);
                if (!string.IsNullOrEmpty(estado))
                    parameters.Add("@Estado", estado);

                var result = await _dbConnection.QueryAsync<dynamic>(
                    "TH_ListadoEncuestadores_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                return ConvertirDynamicADictionary(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RP-TH] Error en GetListadoEncuestadores");
                throw;
            }
        }

        public async Task<Dictionary<string, object>> GetFichaEncuestadorAsync(int idEncuestador)
        {
            try
            {
                _logger.LogInformation($"[RP-TH] Iniciando GetFichaEncuestador: {idEncuestador}");

                var parameters = new DynamicParameters();
                parameters.Add("@IdEncuestador", idEncuestador);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<dynamic>(
                    "TH_FichaEncuestador_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                if (result == null)
                    return new Dictionary<string, object>();

                return ConvertirDynamicADictionary(new[] { result })[0];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[RP-TH] Error en GetFichaEncuestador: {idEncuestador}");
                throw;
            }
        }

        public async Task<List<Dictionary<string, object>>> GetPersonalSinProduccionAsync(
            DateTime fecha, int? areaId = null)
        {
            try
            {
                _logger.LogInformation($"[RP-OP] Iniciando GetPersonalSinProduccion: {fecha:yyyy-MM-dd}");

                var parameters = new DynamicParameters();
                parameters.Add("@Fecha", fecha);
                if (areaId.HasValue)
                    parameters.Add("@AreaId", areaId.Value);

                var result = await _dbConnection.QueryAsync<dynamic>(
                    "OP_PersonalSinProduccion_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                return ConvertirDynamicADictionary(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RP-OP] Error en GetPersonalSinProduccion");
                throw;
            }
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

        public async Task<List<ReporteDTO>> GetReportesDisponiblesAsync()
        {
            try
            {
                _logger.LogInformation("[RP] Obteniendo reportes disponibles");

                var result = await _dbConnection.QueryAsync<dynamic>(
                    "REP_ReportesDisponibles_Get",
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                return result
                    .Select(r => new ReporteDTO
                    {
                        ReporteId = (int)r.ReporteId,
                        Nombre = (string)r.Nombre,
                        Descripcion = (string)r.Descripcion,
                        Categoria = (string)r.Categoria,
                        UltimaGeneracion = (DateTime?)r.UltimaGeneracion,
                        Disponible = (bool)r.Disponible
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RP] Error en GetReportesDisponibles");
                throw;
            }
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
