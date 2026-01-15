using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.OP;
using MatrixNext.Data.Models.OP;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.OP
{
    /// <summary>
    /// Servicio para aprobación/rechazo de planillas cuantitativas
    /// </summary>
    public class PlanillasAprobacionService : IPlanillasAprobacionService
    {
        private readonly IPlanillasAprobacionAdapter _adapter;
        private readonly ILogger<PlanillasAprobacionService> _logger;

        public PlanillasAprobacionService(IPlanillasAprobacionAdapter adapter, ILogger<PlanillasAprobacionService> logger)
        {
            _adapter = adapter;
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
                return await _adapter.ObtenerPlanillasAprobadosAsync(revisado, pmoId, fechaInicio, fechaFin, trabajoId, coordinadorId);
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
                return await _adapter.ObtenerPlanillasRechazadosAsync(revisado, pmoId, fechaInicio, fechaFin, trabajoId, coordinadorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo planillas rechazadas");
                throw;
            }
        }

        public async Task<(bool Success, string Message)> AprobarPlanillaAsync(long planillaId, decimal montoAutorizado, string? observaciones, long usuarioId)
        {
            try
            {
                // Validar que el monto sea válido
                if (montoAutorizado <= 0)
                {
                    return (false, "El monto autorizado debe ser mayor a cero");
                }

                var success = await _adapter.AprobarPlanillaAsync(planillaId, montoAutorizado, observaciones, usuarioId);

                if (success)
                {
                    _logger.LogInformation("Planilla {PlanillaId} aprobada correctamente. Usuario: {UsuarioId}", planillaId, usuarioId);
                    return (true, "Planilla aprobada correctamente");
                }

                return (false, "No se pudo aprobar la planilla");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error aprobando planilla {PlanillaId}. Usuario: {UsuarioId}", planillaId, usuarioId);
                return (false, "Error al aprobar la planilla. Por favor intente nuevamente.");
            }
        }

        public async Task<(bool Success, string Message)> RechazarPlanillaAsync(long planillaId, string motivo, long usuarioId)
        {
            try
            {
                // Validar motivo
                if (string.IsNullOrWhiteSpace(motivo))
                {
                    return (false, "El motivo del rechazo es requerido");
                }

                var success = await _adapter.RechazarPlanillaAsync(planillaId, motivo, usuarioId);

                if (success)
                {
                    _logger.LogInformation("Planilla {PlanillaId} rechazada correctamente. Usuario: {UsuarioId}, Motivo: {Motivo}", 
                        planillaId, usuarioId, motivo);
                    return (true, "Planilla rechazada correctamente");
                }

                return (false, "No se pudo rechazar la planilla");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rechazando planilla {PlanillaId}. Usuario: {UsuarioId}", planillaId, usuarioId);
                return (false, "Error al rechazar la planilla. Por favor intente nuevamente.");
            }
        }

        public (DateTime Inicio, DateTime Fin) ObtenerVentanaNominaActual()
        {
            try
            {
                var inicio = _adapter.GetNominaWindowStart();
                var fin = _adapter.GetNominaWindowEnd();
                
                _logger.LogInformation("Ventana de nómina actual (corte 16-15): {Inicio} - {Fin}", inicio, fin);
                
                return (inicio, fin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculando ventana de nómina");
                throw;
            }
        }
    }
}
