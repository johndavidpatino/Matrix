using Dapper;
using MatrixNext.Data.Modules.CC.DTOs.Produccion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Data.Modules.CC.Adapters
{
    /// <summary>
    /// Adapter para consultas de Producción usando Dapper
    /// </summary>
    public class CcProduccionAdapter
    {
        private readonly IDbConnection _connection;

        public CcProduccionAdapter(IDbConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Obtiene registros de producción con filtros opcionales
        /// </summary>
        public async Task<List<RegistroProduccionDto>> ObtenerRegistrosProduccionAsync(
            int? periodo, long? idTrabajo, long? idEmpleado, 
            long? idActividad, DateTime? fechaInicio, DateTime? fechaFin, byte? estado)
        {
            var parameters = new DynamicParameters();

            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (idTrabajo.HasValue)
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            if (idEmpleado.HasValue)
                parameters.Add("@IdEmpleado", idEmpleado.Value);
            if (idActividad.HasValue)
                parameters.Add("@IdActividad", idActividad.Value);
            if (fechaInicio.HasValue)
                parameters.Add("@FechaInicio", fechaInicio.Value);
            if (fechaFin.HasValue)
                parameters.Add("@FechaFin", fechaFin.Value);
            if (estado.HasValue)
                parameters.Add("@Estado", estado.Value);

            var result = await _connection.QueryAsync<RegistroProduccionDto>(
                "CC_RegistrosProduccion",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        /// <summary>
        /// Obtiene liquidaciones de planillas con filtros opcionales
        /// </summary>
        public async Task<List<LiquidacionPlanillaDto>> ObtenerLiquidacionesAsync(
            int? periodo, long? idTrabajo, long? idEmpleado, byte? estado)
        {
            var parameters = new DynamicParameters();

            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (idTrabajo.HasValue)
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            if (idEmpleado.HasValue)
                parameters.Add("@IdEmpleado", idEmpleado.Value);
            if (estado.HasValue)
                parameters.Add("@Estado", estado.Value);

            var result = await _connection.QueryAsync<LiquidacionPlanillaDto>(
                "CC_LiquidarPlanillas",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        /// <summary>
        /// Obtiene bonificaciones generadas con filtros opcionales
        /// </summary>
        public async Task<List<GenerarBonificacionDto>> ObtenerBonificacionesAsync(
            int? periodo, long? idTrabajo, long? idEmpleado, byte? estado)
        {
            var parameters = new DynamicParameters();

            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (idTrabajo.HasValue)
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            if (idEmpleado.HasValue)
                parameters.Add("@IdEmpleado", idEmpleado.Value);
            if (estado.HasValue)
                parameters.Add("@Estado", estado.Value);

            var result = await _connection.QueryAsync<GenerarBonificacionDto>(
                "CC_GenerarBonificacion",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        /// <summary>
        /// Obtiene descuentos de seguridad social con filtros opcionales
        /// </summary>
        public async Task<List<CargueDescuentoSSDto>> ObtenerDescuentosSsAsync(
            int? periodo, long? idEmpleado, string tipoDescuento, byte? estado)
        {
            var parameters = new DynamicParameters();

            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (idEmpleado.HasValue)
                parameters.Add("@IdEmpleado", idEmpleado.Value);
            if (!string.IsNullOrEmpty(tipoDescuento))
                parameters.Add("@TipoDescuento", tipoDescuento);
            if (estado.HasValue)
                parameters.Add("@Estado", estado.Value);

            var result = await _connection.QueryAsync<CargueDescuentoSSDto>(
                "CC_CargueDescuentosSS",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        /// <summary>
        /// Obtiene liquidaciones de productividad PST con filtros opcionales
        /// </summary>
        public async Task<List<LiquidacionProductividadPstDto>> ObtenerLiquidacionesPstAsync(
            int? periodo, long? idTrabajo, long? idEmpleado, byte? estado)
        {
            var parameters = new DynamicParameters();

            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (idTrabajo.HasValue)
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            if (idEmpleado.HasValue)
                parameters.Add("@IdEmpleado", idEmpleado.Value);
            if (estado.HasValue)
                parameters.Add("@Estado", estado.Value);

            var result = await _connection.QueryAsync<LiquidacionProductividadPstDto>(
                "CC_LiquidarProductividadPST",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        /// <summary>
        /// Obtiene asignaciones de costos a PST con filtros opcionales
        /// </summary>
        public async Task<List<AsignacionCostosPstDto>> ObtenerAsignacionesCostosAsync(
            int? periodo, long? idTrabajo, long? idConcepto, byte? estado)
        {
            var parameters = new DynamicParameters();

            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (idTrabajo.HasValue)
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            if (idConcepto.HasValue)
                parameters.Add("@IdConcepto", idConcepto.Value);
            if (estado.HasValue)
                parameters.Add("@Estado", estado.Value);

            var result = await _connection.QueryAsync<AsignacionCostosPstDto>(
                "CC_AsignacionCostosPST",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        /// <summary>
        /// Obtiene estado de jobbooks con filtros opcionales
        /// </summary>
        public async Task<List<EstadoJobBookDto>> ObtenerEstadoJobBooksAsync(
            long? idTrabajo, byte? estadoActual)
        {
            var parameters = new DynamicParameters();

            if (idTrabajo.HasValue)
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            if (estadoActual.HasValue)
                parameters.Add("@EstadoActual", estadoActual.Value);

            var result = await _connection.QueryAsync<EstadoJobBookDto>(
                "CC_EstadoJobBooks",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        /// <summary>
        /// Obtiene bonificaciones para revisión con filtros opcionales
        /// </summary>
        public async Task<List<RevisarGeneracionBonificacionDto>> ObtenerRevisarBonificacionesAsync(
            int? periodo, long? idEmpleado, long? idTrabajo, bool? aprobada)
        {
            var parameters = new DynamicParameters();

            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (idEmpleado.HasValue)
                parameters.Add("@IdEmpleado", idEmpleado.Value);
            if (idTrabajo.HasValue)
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            if (aprobada.HasValue)
                parameters.Add("@Aprobada", aprobada.Value);

            var result = await _connection.QueryAsync<RevisarGeneracionBonificacionDto>(
                "CC_RevisarGeneracionBonificacion",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        /// <summary>
        /// Obtiene anulaciones de liquidaciones con filtros opcionales
        /// </summary>
        public async Task<List<AnulacionLiquidacionesDto>> ObtenerAnulacionesAsync(
            int? periodo, long? idEmpleado, long? idTrabajo, 
            DateTime? fechaInicio, DateTime? fechaFin)
        {
            var parameters = new DynamicParameters();

            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (idEmpleado.HasValue)
                parameters.Add("@IdEmpleado", idEmpleado.Value);
            if (idTrabajo.HasValue)
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            if (fechaInicio.HasValue)
                parameters.Add("@FechaInicio", fechaInicio.Value);
            if (fechaFin.HasValue)
                parameters.Add("@FechaFin", fechaFin.Value);

            var result = await _connection.QueryAsync<AnulacionLiquidacionesDto>(
                "CC_AnulacionLiquidaciones",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }
    }
}
