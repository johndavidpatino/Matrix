using Dapper;
using MatrixNext.Data.Modules.CC.DTOs.ProcesosInternos;
using System.Data;

namespace MatrixNext.Data.Modules.CC.Adapters
{
    /// <summary>
    /// Adapter para Reportes de Procesos Internos
    /// </summary>
    public class CcProcesosInternosAdapter
    {
        private readonly IDbConnection _dbConnection;

        public CcProcesosInternosAdapter(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        #region Reporte de Conteos

        /// <summary>
        /// Obtiene reporte de conteos de trabajos con filtros
        /// </summary>
        public async Task<IEnumerable<ReporteConteoDto>> ObtenerReporteConteosAsync(
            FiltrosReporteConteoDto filtros)
        {
            const string sql = "CC_ReporteConteoTrabajos";
            var parameters = new DynamicParameters();
            
            if (filtros.FechaInicio.HasValue)
                parameters.Add("@FechaInicio", filtros.FechaInicio.Value);
            if (filtros.FechaFin.HasValue)
                parameters.Add("@FechaFin", filtros.FechaFin.Value);
            if (filtros.IdTrabajo.HasValue)
                parameters.Add("@IdTrabajo", filtros.IdTrabajo.Value);
            if (filtros.IdActividad.HasValue)
                parameters.Add("@IdActividad", filtros.IdActividad.Value);
            if (!string.IsNullOrWhiteSpace(filtros.Categoria))
                parameters.Add("@Categoria", filtros.Categoria);
            if (filtros.Estado.HasValue)
                parameters.Add("@Estado", filtros.Estado.Value);

            var result = await _dbConnection.QueryAsync<ReporteConteoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        /// <summary>
        /// Obtiene totales agregados de conteos
        /// </summary>
        public async Task<dynamic> ObtenerTotalesConteosAsync(
            DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            const string sql = "CC_TotalesConteoTrabajos";
            var parameters = new DynamicParameters();
            
            if (fechaInicio.HasValue)
                parameters.Add("@FechaInicio", fechaInicio.Value);
            if (fechaFin.HasValue)
                parameters.Add("@FechaFin", fechaFin.Value);

            var result = await _dbConnection.QueryFirstOrDefaultAsync<dynamic>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        #endregion

        #region Resumen de Productividad

        /// <summary>
        /// Obtiene resumen de productividad con filtros
        /// </summary>
        public async Task<IEnumerable<ResumenProductividadDto>> ObtenerResumenProductividadAsync(
            FiltrosResumenProductividadDto filtros)
        {
            const string sql = "CC_ResumenesdeProduccion";
            var parameters = new DynamicParameters();
            
            if (filtros.Periodo.HasValue)
                parameters.Add("@Periodo", filtros.Periodo.Value);
            if (filtros.FechaInicio.HasValue)
                parameters.Add("@FechaInicio", filtros.FechaInicio.Value);
            if (filtros.FechaFin.HasValue)
                parameters.Add("@FechaFin", filtros.FechaFin.Value);
            if (filtros.IdTrabajo.HasValue)
                parameters.Add("@IdTrabajo", filtros.IdTrabajo.Value);
            if (!string.IsNullOrWhiteSpace(filtros.CodigoActividad))
                parameters.Add("@CodigoActividad", filtros.CodigoActividad);

            var result = await _dbConnection.QueryAsync<ResumenProductividadDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        /// <summary>
        /// Obtiene datos agregados de productividad global
        /// </summary>
        public async Task<ProductividadAgregadaDto?> ObtenerProductividadAgregadaAsync(
            int? periodo = null, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            const string sql = "CC_ProductividadAgregada";
            var parameters = new DynamicParameters();
            
            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (fechaInicio.HasValue)
                parameters.Add("@FechaInicio", fechaInicio.Value);
            if (fechaFin.HasValue)
                parameters.Add("@FechaFin", fechaFin.Value);

            var result = await _dbConnection.QueryFirstOrDefaultAsync<ProductividadAgregadaDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        #endregion
    }
}
