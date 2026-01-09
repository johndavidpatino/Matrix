using Dapper;
using MatrixNext.Data.Modules.CC.DTOs.PresupuestosInternos;
using System.Data;

namespace MatrixNext.Data.Modules.CC.Adapters
{
    /// <summary>
    /// Adapter para Presupuestos Internos usando Stored Procedures
    /// </summary>
    public class CcPresupuestosInternosAdapter
    {
        private readonly IDbConnection _dbConnection;

        public CcPresupuestosInternosAdapter(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// Obtiene lista de presupuestos internos con filtros
        /// </summary>
        public async Task<IEnumerable<PresupuestoInternoDto>> ObtenerPresupuestosInternosAsync(
            int? periodo = null, string? codigoEmpresa = null, byte? estado = null)
        {
            const string sql = "CC_PresupuestosInternosGet";
            var parameters = new DynamicParameters();
            
            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);
            if (!string.IsNullOrWhiteSpace(codigoEmpresa))
                parameters.Add("@CodigoEmpresa", codigoEmpresa);
            if (estado.HasValue)
                parameters.Add("@Estado", estado.Value);

            var result = await _dbConnection.QueryAsync<PresupuestoInternoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        /// <summary>
        /// Obtiene detalle de un presupuesto interno específico
        /// </summary>
        public async Task<PresupuestoInternoDto?> ObtenerPresupuestoInternoDetalleAsync(
            long idPresupuestoInterno)
        {
            const string sql = "CC_PresupuestosInternosGetXId";
            var parameters = new DynamicParameters();
            parameters.Add("@IdPresupuesto", idPresupuestoInterno);

            var result = await _dbConnection.QueryFirstOrDefaultAsync<PresupuestoInternoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        /// <summary>
        /// Obtiene detalles (líneas) de un presupuesto interno
        /// </summary>
        public async Task<IEnumerable<DetallePresupuestoInternoDto>> 
            ObtenerDetallesPresupuestoInternoAsync(long idPresupuestoInterno)
        {
            const string sql = "CC_DetallePresupuestosSelect";
            var parameters = new DynamicParameters();
            parameters.Add("@idpresup", idPresupuestoInterno);

            var result = await _dbConnection.QueryAsync<DetallePresupuestoInternoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        /// <summary>
        /// Guarda un presupuesto interno (insert/update)
        /// </summary>
        public async Task<long> GuardarPresupuestoInternoAsync(
            PresupuestoInternoDto presupuesto)
        {
            const string sql = "CC_GuardarPresupuestoInterno";
            var parameters = new DynamicParameters();
            
            parameters.Add("@IdPresupuestoInterno", presupuesto.IdPresupuestoInterno);
            parameters.Add("@Periodo", presupuesto.Periodo);
            parameters.Add("@CodigoEmpresa", presupuesto.CodigoEmpresa);
            parameters.Add("@Division", presupuesto.Division);
            parameters.Add("@MontoTotal", presupuesto.MontoTotal);
            parameters.Add("@Estado", presupuesto.Estado);
            parameters.Add("@IdPresupuestoOutput", direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return parameters.Get<long>("@IdPresupuestoOutput");
        }

        /// <summary>
        /// Guarda un detalle de presupuesto interno
        /// </summary>
        public async Task<long> GuardarDetallePresupuestoInternoAsync(
            DetallePresupuestoInternoDto detalle)
        {
            const string sql = "CC_GuardarDetallePresupuestoInterno";
            var parameters = new DynamicParameters();
            
            parameters.Add("@IdDetalle", detalle.IdDetalle);
            parameters.Add("@IdPresupuestoInterno", detalle.IdPresupuestoInterno);
            parameters.Add("@CodigoLinea", detalle.CodigoLinea);
            parameters.Add("@DescripcionLinea", detalle.DescripcionLinea);
            parameters.Add("@MontoAsignado", detalle.MontoAsignado);
            parameters.Add("@CentroCosto", detalle.CentroCosto);
            parameters.Add("@CuentaContable", detalle.CuentaContable);
            parameters.Add("@Observaciones", detalle.Observaciones);
            parameters.Add("@IdDetalleOutput", direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return parameters.Get<long>("@IdDetalleOutput");
        }

        /// <summary>
        /// Elimina un presupuesto interno (soft delete)
        /// </summary>
        public async Task EliminarPresupuestoInternoAsync(long idPresupuestoInterno)
        {
            const string sql = "CC_PresupuestoInternoDelete";
            var parameters = new DynamicParameters();
            parameters.Add("@PresupuestoId", idPresupuestoInterno);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Elimina un detalle de presupuesto interno
        /// </summary>
        public async Task EliminarDetallePresupuestoInternoAsync(long idDetalle)
        {
            const string sql = "CC_EliminarDetallePresupuestoInterno";
            var parameters = new DynamicParameters();
            parameters.Add("@IdDetalle", idDetalle);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene histórico de cambios de un presupuesto interno
        /// </summary>
        public async Task<IEnumerable<HistoricoPresupuestoInternoDto>> 
            ObtenerHistoricoPresupuestoInternoAsync(long idPresupuestoInterno)
        {
            const string sql = "CC_HistoricoPresupuestosInterno";
            var parameters = new DynamicParameters();
            parameters.Add("@IdPresupuestoInterno", idPresupuestoInterno);

            var result = await _dbConnection.QueryAsync<HistoricoPresupuestoInternoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        /// <summary>
        /// Obtiene resumen de presupuestos internos por período
        /// </summary>
        public async Task<IEnumerable<ResumenPresupuestoInternoDto>> 
            ObtenerResumenPresupuestosInternosAsync(int? periodo = null)
        {
            const string sql = "CC_ResumenPresupuestosInternos";
            var parameters = new DynamicParameters();
            
            if (periodo.HasValue)
                parameters.Add("@Periodo", periodo.Value);

            var result = await _dbConnection.QueryAsync<ResumenPresupuestoInternoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return result;
        }

        /// <summary>
        /// Aprueba un presupuesto interno
        /// </summary>
        public async Task AprobarPresupuestoInternoAsync(
            long idPresupuestoInterno, string usuarioAprobacion)
        {
            const string sql = "CC_AprobarPresupuestoInterno";
            var parameters = new DynamicParameters();
            parameters.Add("@IdPresupuestoInterno", idPresupuestoInterno);
            parameters.Add("@UsuarioAprobacion", usuarioAprobacion);

            await _dbConnection.ExecuteAsync(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
