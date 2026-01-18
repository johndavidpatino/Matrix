using Dapper;
using MatrixNext.Data.Modules.CC.DTOs;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CcFinzOpeAdapter> _logger;

        public CcFinzOpeAdapter(string webMatrixConnection, ILogger<CcFinzOpeAdapter> logger)
        {
            _webMatrixConnection = webMatrixConnection ?? throw new ArgumentNullException(nameof(webMatrixConnection));
            _logger = logger;
        }

        /// <summary>
        /// Obtener liquidación mensual completa de un período
        /// </summary>
        public async Task<CcLiquidacionDto> ObtenerLiquidacion(int idPeriodo, DateTime fechaInicio, DateTime fechaFin)
        {
            // STUB: SP CC_LiquidarPlanillas no existe en legacy
            _logger.LogWarning("[CC] ObtenerLiquidacion: SP CC_LiquidarPlanillas no existe en legacy. IdPeriodo: {IdPeriodo}", idPeriodo);
            return await Task.FromResult(new CcLiquidacionDto { IdPeriodo = idPeriodo, FechaInicio = fechaInicio, FechaFin = fechaFin });
        }

        /// <summary>
        /// Obtener bonificaciones para un período
        /// </summary>
        public async Task<List<CcBonificacionDto>> ObtenerBonificaciones(int idPeriodo)
        {
            // STUB: SP CC_GenerarBonificacion no existe en legacy
            _logger.LogWarning("[CC] ObtenerBonificaciones: SP CC_GenerarBonificacion no existe en legacy. IdPeriodo: {IdPeriodo}", idPeriodo);
            return await Task.FromResult(new List<CcBonificacionDto>());
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
