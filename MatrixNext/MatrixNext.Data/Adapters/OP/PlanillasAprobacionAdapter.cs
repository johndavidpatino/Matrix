using Dapper;
using MatrixNext.Data.Context;
using MatrixNext.Data.Models.OP;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Data.Adapters.OP
{
    /// <summary>
    /// Adapter para acceso a datos de planillas con aprobación/rechazo
    /// </summary>
    public class PlanillasAprobacionAdapter : IPlanillasAprobacionAdapter
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlanillasAprobacionAdapter> _logger;

        public PlanillasAprobacionAdapter(ApplicationDbContext context, ILogger<PlanillasAprobacionAdapter> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<PlanillaAprobacionDto>> ObtenerPlanillasAprobadosAsync(
            bool? revisado = null,
            long? pmoId = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            long? trabajoId = null,
            long? coordinadorId = null
        )
        {
            try
            {
                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@Revisado", revisado);
                parameters.Add("@PMO", pmoId);
                parameters.Add("@Fini", fechaInicio);
                parameters.Add("@Ffin", fechaFin);
                parameters.Add("@TrabajoId", trabajoId);
                parameters.Add("@Coordinador", coordinadorId);

                // Ejecutar SP de CoreProject: OP_CuantiPlanillas_GET con estado = Aprobada
                var result = await connection.QueryAsync<PlanillaAprobacionDto>(
                    "OP_CuantiPlanillas_GET",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo planillas aprobadas");
                throw;
            }
        }

        public async Task<IEnumerable<PlanillaAprobacionDto>> ObtenerPlanillasRechazadosAsync(
            bool? revisado = null,
            long? pmoId = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            long? trabajoId = null,
            long? coordinadorId = null
        )
        {
            try
            {
                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@Revisado", revisado);
                parameters.Add("@PMO", pmoId);
                parameters.Add("@Fini", fechaInicio);
                parameters.Add("@Ffin", fechaFin);
                parameters.Add("@TrabajoId", trabajoId);
                parameters.Add("@Coordinador", coordinadorId);

                // Ejecutar SP de CoreProject: OP_CuantiPlanillas_GET con estado = Rechazada
                var result = await connection.QueryAsync<PlanillaAprobacionDto>(
                    "OP_CuantiPlanillas_GET",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo planillas rechazadas");
                throw;
            }
        }

        public async Task<bool> AprobarPlanillaAsync(long planillaId, decimal montoAutorizado, string? observaciones, long usuarioId)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@PlanillaId", planillaId);
                parameters.Add("@MontoAutorizado", montoAutorizado);
                parameters.Add("@Observaciones", observaciones ?? "");
                parameters.Add("@UsuarioId", usuarioId);
                parameters.Add("@FechaModificacion", DateTime.Now);

                // Ejecutar SP de CoreProject para aprobación
                var result = await connection.ExecuteAsync(
                    "OP_CuantiPlanillas_Update",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("Planilla {PlanillaId} aprobada. Usuario: {UsuarioId}, Monto: {Monto}", 
                    planillaId, usuarioId, montoAutorizado);

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error aprobando planilla {PlanillaId}", planillaId);
                throw;
            }
        }

        public async Task<bool> RechazarPlanillaAsync(long planillaId, string motivo, long usuarioId)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@PlanillaId", planillaId);
                parameters.Add("@Motivo", motivo);
                parameters.Add("@UsuarioId", usuarioId);
                parameters.Add("@FechaModificacion", DateTime.Now);

                // Ejecutar SP de CoreProject para rechazo
                var result = await connection.ExecuteAsync(
                    "OP_CuantiPlanillas_Remove",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("Planilla {PlanillaId} rechazada. Usuario: {UsuarioId}, Motivo: {Motivo}", 
                    planillaId, usuarioId, motivo);

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rechazando planilla {PlanillaId}", planillaId);
                throw;
            }
        }

        public DateTime GetNominaWindowStart()
        {
            // Corte 16-15: del 16 del mes actual al 15 del siguiente mes
            var today = DateTime.Now;
            
            if (today.Day >= 16)
            {
                // Estamos en la segunda quincena, la ventana empezó el 16 de este mes
                return new DateTime(today.Year, today.Month, 16);
            }
            else
            {
                // Estamos en la primera quincena, la ventana empezó el 16 del mes anterior
                var previousMonth = today.AddMonths(-1);
                return new DateTime(previousMonth.Year, previousMonth.Month, 16);
            }
        }

        public DateTime GetNominaWindowEnd()
        {
            // Corte 16-15: del 16 del mes actual al 15 del siguiente mes
            var today = DateTime.Now;
            
            if (today.Day >= 16)
            {
                // Estamos en la segunda quincena, la ventana termina el 15 del siguiente mes
                var nextMonth = today.AddMonths(1);
                return new DateTime(nextMonth.Year, nextMonth.Month, 15);
            }
            else
            {
                // Estamos en la primera quincena, la ventana termina el 15 de este mes
                return new DateTime(today.Year, today.Month, 15);
            }
        }
    }
}
