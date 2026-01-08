using Dapper;
using MatrixNext.Data.Modules.CC.DTOs.ProcesosInternos;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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

            IEnumerable<ReporteConteoDto>? result = await _dbConnection.QueryAsync<ReporteConteoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);

            if (result is null)
            {
                return Enumerable.Empty<ReporteConteoDto>();
            }

            return result;
        }

        /// <summary>
        /// Obtiene totales agregados de conteos
        /// </summary>
        public async Task<dynamic?> ObtenerTotalesConteosAsync(
            DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            const string sql = "CC_TotalesConteoTrabajos";
            var parameters = new DynamicParameters();
            
            if (fechaInicio.HasValue)
                parameters.Add("@FechaInicio", fechaInicio.Value);
            if (fechaFin.HasValue)
                parameters.Add("@FechaFin", fechaFin.Value);

            var result = await _dbConnection.QueryFirstOrDefaultAsync<dynamic?>(
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

            IEnumerable<ResumenProductividadDto>? result = await _dbConnection.QueryAsync<ResumenProductividadDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return (result ?? Enumerable.Empty<ResumenProductividadDto>())!;
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

        #region Conteo Trabajos (CRUD)

        /// <summary>
        /// Obtiene conteos con filtros
        /// </summary>
        public async Task<IEnumerable<ConteoTrabajoDto>> ObtenerConteosAsync(
            long? idTrabajo = null, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            const string sql = "CC_ConteosXIdGet";
            var parameters = new DynamicParameters();
            
            if (idTrabajo.HasValue)
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            if (fechaInicio.HasValue)
                parameters.Add("@FechaInicio", fechaInicio.Value);
            if (fechaFin.HasValue)
                parameters.Add("@FechaFin", fechaFin.Value);

            return await _dbConnection.QueryAsync<ConteoTrabajoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene actividades por trabajo
        /// </summary>
        public async Task<IEnumerable<ActividadTrabajoDto>> ObtenerActividadesPorTrabajoAsync(
            long idTrabajo)
        {
            const string sql = "CC_ActividadesXTrabajo";
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", idTrabajo);

            return await _dbConnection.QueryAsync<ActividadTrabajoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Guarda un conteo
        /// </summary>
        public async Task<long> GuardarConteoAsync(GuardarConteoRequest request)
        {
            const string sql = "CC_Conteos_Insert";
            var parameters = new DynamicParameters();
            
            parameters.Add("@IdConteo", request.IdConteo);
            parameters.Add("@IdTrabajo", request.IdTrabajo);
            parameters.Add("@IdActividad", request.IdActividad);
            parameters.Add("@Categoria", request.Categoria);
            parameters.Add("@Cantidad", request.Cantidad);
            parameters.Add("@FechaConteo", request.FechaConteo);
            parameters.Add("@Observaciones", request.Observaciones);
            parameters.Add("@IdConteoOutput", direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return parameters.Get<long>("@IdConteoOutput");
        }

        /// <summary>
        /// Elimina un conteo
        /// </summary>
        public async Task EliminarConteoAsync(long idConteo)
        {
            const string sql = "CC_Conteos_Delete";
            var parameters = new DynamicParameters();
            parameters.Add("@IdConteo", idConteo);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Requerimientos Equipo (CRUD)

        /// <summary>
        /// Obtiene requerimientos de equipo
        /// </summary>
        public async Task<IEnumerable<RequerimientoEquipoDto>> ObtenerRequerimientosAsync(
            long? idTrabajo = null, byte? estado = null)
        {
            const string sql = "CC_Requerimientos_Get";
            var parameters = new DynamicParameters();
            
            if (idTrabajo.HasValue)
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            if (estado.HasValue)
                parameters.Add("@Estado", estado.Value);

            return await _dbConnection.QueryAsync<RequerimientoEquipoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Genera muestra de requerimientos
        /// </summary>
        public async Task<IEnumerable<MuestraRequerimientoDto>> GenerarMuestraRequerimientosAsync(
            long idTrabajo)
        {
            const string sql = "CC_MuestraGenerarRequerimiento";
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", idTrabajo);

            return await _dbConnection.QueryAsync<MuestraRequerimientoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Guarda un requerimiento
        /// </summary>
        public async Task<long> GuardarRequerimientoAsync(GuardarRequerimientoRequest request)
        {
            const string sql = "CC_GenerarRequerimientos";
            var parameters = new DynamicParameters();
            
            parameters.Add("@IdRequerimiento", request.IdRequerimiento);
            parameters.Add("@IdTrabajo", request.IdTrabajo);
            parameters.Add("@FechaRequerimiento", request.FechaRequerimiento);
            parameters.Add("@TipoEquipo", request.TipoEquipo);
            parameters.Add("@CantidadRequerida", request.CantidadRequerida);
            parameters.Add("@Justificacion", request.Justificacion);
            parameters.Add("@IdRequerimientoOutput", direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return parameters.Get<long>("@IdRequerimientoOutput");
        }

        /// <summary>
        /// Elimina un requerimiento
        /// </summary>
        public async Task EliminarRequerimientoAsync(long idRequerimiento)
        {
            const string sql = "CC_Requerimientos_Delete";
            var parameters = new DynamicParameters();
            parameters.Add("@IdRequerimiento", idRequerimiento);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Consolidación Producción

        /// <summary>
        /// Obtiene producción pendiente de consolidar
        /// </summary>
        public async Task<IEnumerable<ProduccionDto>> ObtenerProduccionPendienteAsync(
            int? periodo = null, long? idTrabajo = null)
        {
            const string sql = "CC_Produccion_PendienteConsolidar";
            var parameters = new DynamicParameters();
            
            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (idTrabajo.HasValue)
                parameters.Add("@IdTrabajo", idTrabajo.Value);

            return await _dbConnection.QueryAsync<ProduccionDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Consolida producción
        /// </summary>
        public async Task ConsolidarProduccionAsync(ConsolidarProduccionRequest request)
        {
            const string sql = "CC_ConsolidacionProduccion";
            var parameters = new DynamicParameters();
            
            parameters.Add("@IdProduccion", request.IdProduccion);
            parameters.Add("@CantidadConsolidada", request.CantidadConsolidada);
            parameters.Add("@UsuarioConsolida", request.UsuarioConsolida);
            parameters.Add("@Observaciones", request.Observaciones);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene resumen de consolidación
        /// </summary>
        public async Task<ResumenConsolidacionDto?> ObtenerResumenConsolidacionAsync(
            int periodo)
        {
            const string sql = "CC_ResumenConsolidacion";
            var parameters = new DynamicParameters();
            parameters.Add("@Periodo", periodo);

            return await _dbConnection.QueryFirstOrDefaultAsync<ResumenConsolidacionDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Cálculo Jornada Laboral

        /// <summary>
        /// Obtiene jornadas laborales
        /// </summary>
        public async Task<IEnumerable<JornadaLaboralDto>> ObtenerJornadasAsync(
            int? periodo = null, long? idEmpleado = null)
        {
            const string sql = "CC_CalculoJornada_Get";
            var parameters = new DynamicParameters();
            
            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (idEmpleado.HasValue)
                parameters.Add("@IdEmpleado", idEmpleado.Value);

            return await _dbConnection.QueryAsync<JornadaLaboralDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene ausencias del empleado (integración TH)
        /// </summary>
        public async Task<IEnumerable<AusenciaEmpleadoDto>> ObtenerAusenciasEmpleadoAsync(
            long idEmpleado, DateTime fechaInicio, DateTime fechaFin)
        {
            const string sql = "TH_Ausencia_CalculoDias";
            var parameters = new DynamicParameters();
            
            parameters.Add("@IdEmpleado", idEmpleado);
            parameters.Add("@FechaInicio", fechaInicio);
            parameters.Add("@FechaFin", fechaFin);

            return await _dbConnection.QueryAsync<AusenciaEmpleadoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Calcula y guarda jornada laboral
        /// </summary>
        public async Task<long> CalcularJornadaAsync(CalcularJornadaRequest request)
        {
            const string sql = "CC_CalculoJornada_Insert";
            var parameters = new DynamicParameters();
            
            parameters.Add("@IdEmpleado", request.IdEmpleado);
            parameters.Add("@Periodo", request.Periodo);
            parameters.Add("@FechaInicio", request.FechaInicio);
            parameters.Add("@FechaFin", request.FechaFin);
            parameters.Add("@HorasBase", request.HorasBase);
            parameters.Add("@HorasExtras", request.HorasExtras);
            parameters.Add("@UsuarioCalcula", request.UsuarioCalcula);
            parameters.Add("@IdJornadaOutput", direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return parameters.Get<long>("@IdJornadaOutput");
        }

        /// <summary>
        /// Obtiene resumen de jornadas
        /// </summary>
        public async Task<ResumenJornadasDto?> ObtenerResumenJornadasAsync(int periodo)
        {
            const string sql = "CC_ResumenJornadas";
            var parameters = new DynamicParameters();
            parameters.Add("@Periodo", periodo);

            return await _dbConnection.QueryFirstOrDefaultAsync<ResumenJornadasDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        #endregion
    }
}
