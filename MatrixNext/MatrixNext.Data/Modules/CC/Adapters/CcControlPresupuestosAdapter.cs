using Dapper;
using MatrixNext.Data.Modules.CC.DTOs.ControlPresupuestos;
using System.Data;

namespace MatrixNext.Data.Modules.CC.Adapters
{
    /// <summary>
    /// Adapter para Control de Presupuestos usando Stored Procedures
    /// </summary>
    public class CcControlPresupuestosAdapter
    {
        private readonly IDbConnection _dbConnection;

        public CcControlPresupuestosAdapter(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// Obtiene lista de presupuestos con filtros
        /// </summary>
        public async Task<IEnumerable<PresupuestoDto>> ObtenerPresupuestosAsync(
            int? periodo = null, long? idTrabajo = null, byte? estado = null)
        {
            const string sql = "CC_ObtenerPresupuestos";
            var parameters = new DynamicParameters();
            
            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (idTrabajo.HasValue)
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            if (estado.HasValue)
                parameters.Add("@Estado", estado.Value);

            var result = await _dbConnection.QueryAsync<PresupuestoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        /// <summary>
        /// Obtiene detalles de un presupuesto específico
        /// </summary>
        public async Task<IEnumerable<DetallePresupuestoDto>> ObtenerDetallePresupuestoAsync(
            long idPresupuesto)
        {
            const string sql = "CC_DetallePresupuesto";
            var parameters = new DynamicParameters();
            parameters.Add("@IdPresupuesto", idPresupuesto);

            var result = await _dbConnection.QueryAsync<DetallePresupuestoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        /// <summary>
        /// Guarda un presupuesto (insert/update)
        /// </summary>
        public async Task<long> GuardarPresupuestoAsync(PresupuestoDto presupuesto)
        {
            const string sql = "CC_GuardarPresupuesto";
            var parameters = new DynamicParameters();
            
            parameters.Add("@IdPresupuesto", presupuesto.IdPresupuesto);
            parameters.Add("@Periodo", presupuesto.Periodo);
            parameters.Add("@IdTrabajo", presupuesto.IdTrabajo);
            parameters.Add("@MontoPresupuesto", presupuesto.MontoPresupuesto);
            parameters.Add("@Estado", presupuesto.Estado);
            parameters.Add("@IdPresupuestoOutput", direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return parameters.Get<long>("@IdPresupuestoOutput");
        }

        /// <summary>
        /// Guarda detalles de presupuesto
        /// </summary>
        public async Task<long> GuardarDetallePresupuestoAsync(
            DetallePresupuestoDto detalle)
        {
            const string sql = "CC_GuardarDetallePresupuesto";
            var parameters = new DynamicParameters();
            
            parameters.Add("@IdDetallePresupuesto", detalle.IdDetallePresupuesto);
            parameters.Add("@IdPresupuesto", detalle.IdPresupuesto);
            parameters.Add("@IdActividad", detalle.IdActividad);
            parameters.Add("@Cantidad", detalle.Cantidad);
            parameters.Add("@ValorUnitario", detalle.ValorUnitario);
            parameters.Add("@IdDetalleOutput", direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return parameters.Get<long>("@IdDetalleOutput");
        }

        /// <summary>
        /// Elimina un presupuesto (soft delete)
        /// </summary>
        public async Task EliminarPresupuestoAsync(long idPresupuesto)
        {
            const string sql = "CC_EliminarPresupuesto";
            var parameters = new DynamicParameters();
            parameters.Add("@IdPresupuesto", idPresupuesto);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene verificación de presupuesto vs. realizado
        /// </summary>
        public async Task<IEnumerable<VerificacionPresupuestoDto>> 
            ObtenerVerificacionPresupuestosAsync(int? periodo = null)
        {
            const string sql = "CC_VerificacionPresupuestosRealizados";
            var parameters = new DynamicParameters();
            
            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);

            var result = await _dbConnection.QueryAsync<VerificacionPresupuestoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        /// <summary>
        /// Obtiene datos de nómina para distribución de costos
        /// </summary>
        public async Task<IEnumerable<NominaDistribucionDto>> 
            ObtenerNominaDistribucionAsync(int periodo, long? idEmpleado = null)
        {
            const string sql = "CC_LiquidarPlanillas";
            var parameters = new DynamicParameters();
            parameters.Add("@Periodo", periodo);
            
            if (idEmpleado.HasValue)
                parameters.Add("@IdEmpleado", idEmpleado.Value);

            var result = await _dbConnection.QueryAsync<NominaDistribucionDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        /// <summary>
        /// Guarda distribución de costos por centro
        /// </summary>
        public async Task<long> GuardarDistribucionCostoAsync(
            DistribucionPorCentroDto distribucion)
        {
            const string sql = "CC_GuardarDistribucionCostos";
            var parameters = new DynamicParameters();
            
            parameters.Add("@IdDistribucion", distribucion.IdDistribucion);
            parameters.Add("@IdEmpleado", distribucion.IdCentroCosto);
            parameters.Add("@IdCentroCosto", distribucion.IdCentroCosto);
            parameters.Add("@PorcentajeDistribucion", distribucion.PorcentajeDistribucion);
            parameters.Add("@IdDistribucionOutput", direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return parameters.Get<long>("@IdDistribucionOutput");
        }

        /// <summary>
        /// Obtiene actividades disponibles para asignación de presupuesto
        /// </summary>
        public async Task<IEnumerable<AsignacionPresupuestoDto>> 
            ObtenerActividadesPresupuestadasAsync(long idPresupuesto)
        {
            const string sql = "CC_ActividadesPresupuestadas";
            var parameters = new DynamicParameters();
            parameters.Add("@IdPresupuesto", idPresupuesto);

            var result = await _dbConnection.QueryAsync<AsignacionPresupuestoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        /// <summary>
        /// Guarda asignación de presupuesto a actividad
        /// </summary>
        public async Task<long> GuardarAsignacionPresupuestoAsync(
            AsignacionPresupuestoDto asignacion)
        {
            const string sql = "CC_GuardarAsignacionPresupuesto";
            var parameters = new DynamicParameters();
            
            parameters.Add("@IdAsignacion", asignacion.IdAsignacion);
            parameters.Add("@IdPresupuesto", asignacion.IdPresupuesto);
            parameters.Add("@IdActividad", asignacion.IdActividad);
            parameters.Add("@MontoAsignado", asignacion.MontoAsignado);
            parameters.Add("@IdAsignacionOutput", direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return parameters.Get<long>("@IdAsignacionOutput");
        }
    }
}
