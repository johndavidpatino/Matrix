using Dapper;
using MatrixNext.Data.Modules.CC.DTOs.ControlPresupuestos;
using System.Data;

namespace MatrixNext.Data.Modules.CC.Adapters
{
    /// <summary>
    /// Adapter para Control de Presupuestos usando Stored Procedures existentes en BD
    /// SP disponibles: CC_PresupuestosInternosGet, CC_PresupuestosInternosGetXId, 
    /// CC_DetallePresupuestosSelect, CC_DetallePresupuestoGet, CC_PresupuestoInternoDelete,
    /// CC_ActividadesPresupuestadas
    /// </summary>
    public class CcControlPresupuestosAdapter
    {
        private readonly IDbConnection _dbConnection;

        public CcControlPresupuestosAdapter(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// Obtiene lista de presupuestos por trabajo
        /// SP: CC_PresupuestosInternosGet
        /// </summary>
        public async Task<IEnumerable<PresupuestoDto>> ObtenerPresupuestosAsync(
            int? periodo = null, long? idTrabajo = null, byte? estado = null)
        {
            // Usar SP existente CC_PresupuestosInternosGet (solo acepta @TrabajoId)
            const string sql = "CC_PresupuestosInternosGet";
            var parameters = new DynamicParameters();
            
            if (idTrabajo.HasValue)
                parameters.Add("@TrabajoId", idTrabajo.Value);

            var result = await _dbConnection.QueryAsync<PresupuestoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            // Filtrar por periodo y estado en memoria si se especificaron
            if (periodo.HasValue || estado.HasValue)
            {
                return result.Where(p => 
                    (!periodo.HasValue || p.Periodo == periodo.Value) &&
                    (!estado.HasValue || p.Estado == estado.Value));
            }
            
            return result;
        }

        /// <summary>
        /// Obtiene un presupuesto por ID
        /// SP: CC_PresupuestosInternosGetXId
        /// </summary>
        public async Task<PresupuestoDto?> ObtenerPresupuestoPorIdAsync(long idPresupuesto)
        {
            const string sql = "CC_PresupuestosInternosGetXId";
            var parameters = new DynamicParameters();
            parameters.Add("@IdPresupuesto", idPresupuesto);

            return await _dbConnection.QueryFirstOrDefaultAsync<PresupuestoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene detalles de un presupuesto específico
        /// SP: CC_DetallePresupuestoGet
        /// </summary>
        public async Task<IEnumerable<DetallePresupuestoDto>> ObtenerDetallePresupuestoAsync(
            long idPresupuesto)
        {
            const string sql = "CC_DetallePresupuestoGet";
            var parameters = new DynamicParameters();
            parameters.Add("@IdPresupuesto", idPresupuesto);

            var result = await _dbConnection.QueryAsync<DetallePresupuestoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        /// <summary>
        /// Obtiene detalles de presupuesto (alternativo)
        /// SP: CC_DetallePresupuestosSelect
        /// </summary>
        public async Task<IEnumerable<DetallePresupuestoDto>> ObtenerDetallesPresupuestoSelectAsync(
            long idPresupuesto)
        {
            const string sql = "CC_DetallePresupuestosSelect";
            var parameters = new DynamicParameters();
            parameters.Add("@idpresup", idPresupuesto);

            var result = await _dbConnection.QueryAsync<DetallePresupuestoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        /// <summary>
        /// Guarda un presupuesto (insert/update)
        /// NOTA: No existe SP para guardar - usar EF Core o implementar SP en BD
        /// </summary>
        public async Task<long> GuardarPresupuestoAsync(PresupuestoDto presupuesto)
        {
            // TODO: SP CC_GuardarPresupuesto no existe en BD legacy
            // Usar CC_SolicitudPresupuestoInternoAdd si aplica o implementar con EF
            throw new NotImplementedException(
                "SP CC_GuardarPresupuesto no existe. Usar CC_SolicitudPresupuestoInternoAdd o implementar con EF Core.");
        }

        /// <summary>
        /// Guarda detalles de presupuesto
        /// NOTA: No existe SP para guardar detalle - usar CC_DetallePresupuestosUpdate
        /// </summary>
        public async Task<long> GuardarDetallePresupuestoAsync(DetallePresupuestoDto detalle)
        {
            // TODO: SP CC_GuardarDetallePresupuesto no existe - usar CC_DetallePresupuestosUpdate
            throw new NotImplementedException(
                "SP CC_GuardarDetallePresupuesto no existe. Usar CC_DetallePresupuestosUpdate.");
        }

        /// <summary>
        /// Elimina un presupuesto
        /// SP: CC_PresupuestoInternoDelete
        /// </summary>
        public async Task EliminarPresupuestoAsync(long idPresupuesto)
        {
            const string sql = "CC_PresupuestoInternoDelete";
            var parameters = new DynamicParameters();
            parameters.Add("@PresupuestoId", idPresupuesto);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene verificación de presupuesto vs. realizado
        /// NOTA: SP no existe en BD legacy
        /// </summary>
        public async Task<IEnumerable<VerificacionPresupuestoDto>> 
            ObtenerVerificacionPresupuestosAsync(int? periodo = null)
        {
            // TODO: SP CC_VerificacionPresupuestosRealizados no existe
            throw new NotImplementedException(
                "SP CC_VerificacionPresupuestosRealizados no existe en BD legacy.");
        }

        /// <summary>
        /// Obtiene datos de nómina para distribución de costos
        /// NOTA: SP no existe - usar CC_NominaDistribucionCostos si aplica
        /// </summary>
        public async Task<IEnumerable<NominaDistribucionDto>> 
            ObtenerNominaDistribucionAsync(int periodo, long? idEmpleado = null)
        {
            // TODO: SP CC_LiquidarPlanillas no existe con esos parámetros
            throw new NotImplementedException(
                "SP CC_LiquidarPlanillas no existe. Revisar CC_NominaDistribucion* disponibles.");
        }

        /// <summary>
        /// Guarda distribución de costos por centro
        /// NOTA: SP no existe en BD legacy
        /// </summary>
        public async Task<long> GuardarDistribucionCostoAsync(DistribucionPorCentroDto distribucion)
        {
            // TODO: SP CC_GuardarDistribucionCostos no existe
            throw new NotImplementedException(
                "SP CC_GuardarDistribucionCostos no existe en BD legacy.");
        }

        /// <summary>
        /// Obtiene actividades disponibles para asignación de presupuesto
        /// SP: CC_ActividadesPresupuestadas
        /// </summary>
        public async Task<IEnumerable<AsignacionPresupuestoDto>> 
            ObtenerActividadesPresupuestadasAsync(long idPropuesta)
        {
            const string sql = "CC_ActividadesPresupuestadas";
            var parameters = new DynamicParameters();
            parameters.Add("@IdPropuesta", idPropuesta);

            var result = await _dbConnection.QueryAsync<AsignacionPresupuestoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        /// <summary>
        /// Guarda asignación de presupuesto a actividad
        /// NOTA: SP no existe en BD legacy
        /// </summary>
        public async Task<long> GuardarAsignacionPresupuestoAsync(AsignacionPresupuestoDto asignacion)
        {
            // TODO: SP CC_GuardarAsignacionPresupuesto no existe
            throw new NotImplementedException(
                "SP CC_GuardarAsignacionPresupuesto no existe en BD legacy.");
        }
    }
}
