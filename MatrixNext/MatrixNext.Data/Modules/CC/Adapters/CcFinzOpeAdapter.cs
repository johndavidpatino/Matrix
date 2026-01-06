using Dapper;
using MatrixNext.Data.Modules.CC.DTOs;
using System.Data;
using System.Data.SqlClient;

namespace MatrixNext.Data.Modules.CC.Adapters
{
    /// <summary>
    /// CC FinzOpe Adapter - Executes stored procedures from WebMatrix via Dapper
    /// </summary>
    public interface ICcFinzOpeAdapter
    {
        Task<CcLiquidacionDto> ObtenerLiquidacion(int idPeriodo, DateTime fechaInicio, DateTime fechaFin);
        Task<List<CcBonificacionDto>> ObtenerBonificaciones(int idPeriodo);
        Task<decimal> ObtenerProduccionTotal(DateTime fechaInicio, DateTime fechaFin, int? idTrabajo = null);
    }

    public class CcFinzOpeAdapter : ICcFinzOpeAdapter
    {
        private readonly string _webMatrixConnection;

        public CcFinzOpeAdapter(string webMatrixConnection)
        {
            _webMatrixConnection = webMatrixConnection ?? throw new ArgumentNullException(nameof(webMatrixConnection));
        }

        /// <summary>
        /// Obtener liquidación mensual completa de un período
        /// </summary>
        public async Task<CcLiquidacionDto> ObtenerLiquidacion(int idPeriodo, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                using (var connection = new SqlConnection(_webMatrixConnection))
                {
                    var result = await connection.QuerySingleOrDefaultAsync<CcLiquidacionDto>(
                        "CC_LiquidarPlanillas",
                        new { IdPeriodo = idPeriodo, FechaInicio = fechaInicio, FechaFin = fechaFin },
                        commandType: CommandType.StoredProcedure
                    );

                    return result ?? new CcLiquidacionDto { IdPeriodo = idPeriodo, FechaInicio = fechaInicio, FechaFin = fechaFin };
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing CC_LiquidarPlanillas for period {idPeriodo}", ex);
            }
        }

        /// <summary>
        /// Obtener bonificaciones para un período
        /// </summary>
        public async Task<List<CcBonificacionDto>> ObtenerBonificaciones(int idPeriodo)
        {
            try
            {
                using (var connection = new SqlConnection(_webMatrixConnection))
                {
                    var result = await connection.QueryAsync<CcBonificacionDto>(
                        "CC_GenerarBonificacion",
                        new { IdPeriodo = idPeriodo },
                        commandType: CommandType.StoredProcedure
                    );

                    return result?.ToList() ?? new List<CcBonificacionDto>();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing CC_GenerarBonificacion for period {idPeriodo}", ex);
            }
        }

        /// <summary>
        /// Obtener total de producción en un rango de fechas
        /// </summary>
        public async Task<decimal> ObtenerProduccionTotal(DateTime fechaInicio, DateTime fechaFin, int? idTrabajo = null)
        {
            try
            {
                using (var connection = new SqlConnection(_webMatrixConnection))
                {
                    var result = await connection.QuerySingleOrDefaultAsync<decimal?>(
                        "CC_ProduccionXFechas",
                        new { FechaInicio = fechaInicio, FechaFin = fechaFin, IdTrabajo = idTrabajo },
                        commandType: CommandType.StoredProcedure
                    );

                    return result ?? 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing CC_ProduccionXFechas", ex);
            }
        }
    }
}
