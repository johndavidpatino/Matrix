using Dapper;
using MatrixNext.Data.Modules.CC.DTOs.Reportes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MatrixNext.Data.Modules.CC.Adapters
{
    public class CcReportesAdapter
    {
        private readonly IDbConnection _dbConnection;

        public CcReportesAdapter(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<ReportePagoDto>> ObtenerReportePagosAsync(
            int? periodo = null,
            long? idTrabajo = null,
            long? idEmpleado = null,
            byte? estado = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            const string sql = "CC_ReportePagos";
            var parameters = new DynamicParameters();
            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (idTrabajo.HasValue)
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            if (idEmpleado.HasValue)
                parameters.Add("@IdEmpleado", idEmpleado.Value);
            if (estado.HasValue)
                parameters.Add("@Estado", estado.Value);
            if (fechaInicio.HasValue)
                parameters.Add("@FechaInicio", fechaInicio.Value);
            if (fechaFin.HasValue)
                parameters.Add("@FechaFin", fechaFin.Value);

            return await _dbConnection.QueryAsync<ReportePagoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteActividadProduccionDto>> ObtenerActividadesProduccionAsync(
            int? periodo = null,
            long? idTrabajo = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            const string sql = "CC_ReporteActividadesProduccion";
            var parameters = new DynamicParameters();
            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (idTrabajo.HasValue)
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            if (fechaInicio.HasValue)
                parameters.Add("@FechaInicio", fechaInicio.Value);
            if (fechaFin.HasValue)
                parameters.Add("@FechaFin", fechaFin.Value);

            return await _dbConnection.QueryAsync<ReporteActividadProduccionDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteContabilizacionPstDto>> ObtenerContabilizacionPstAsync(
            int? periodo = null,
            long? idTrabajo = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            const string sql = "CC_ReporteContabilizacionPST";
            var parameters = new DynamicParameters();
            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (idTrabajo.HasValue)
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            if (fechaInicio.HasValue)
                parameters.Add("@FechaInicio", fechaInicio.Value);
            if (fechaFin.HasValue)
                parameters.Add("@FechaFin", fechaFin.Value);

            return await _dbConnection.QueryAsync<ReporteContabilizacionPstDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteVarianzaPresupuestariaDto>> ObtenerVarianzasPresupuestariasAsync(
            int? periodo = null,
            long? idTrabajo = null)
        {
            const string sql = "CC_ReporteVarianzasPresupuestarias";
            var parameters = new DynamicParameters();
            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (idTrabajo.HasValue)
                parameters.Add("@IdTrabajo", idTrabajo.Value);

            return await _dbConnection.QueryAsync<ReporteVarianzaPresupuestariaDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
