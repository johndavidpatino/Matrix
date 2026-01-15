using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Models.OP;

namespace MatrixNext.Data.Services.OP
{
    /// <summary>
    /// Interfaz para servicio de aprobación/rechazo de planillas
    /// </summary>
    public interface IPlanillasAprobacionService
    {
        /// <summary>
        /// Obtiene planillas aprobadas con filtros
        /// </summary>
        Task<IEnumerable<PlanillaAprobacionDto>> ObtenerPlanillasAprobadosAsync(
            bool? revisado = null,
            long? pmoId = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            long? trabajoId = null,
            long? coordinadorId = null
        );

        /// <summary>
        /// Obtiene planillas rechazadas con filtros
        /// </summary>
        Task<IEnumerable<PlanillaAprobacionDto>> ObtenerPlanillasRechazadosAsync(
            bool? revisado = null,
            long? pmoId = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            long? trabajoId = null,
            long? coordinadorId = null
        );

        /// <summary>
        /// Aprueba una planilla
        /// </summary>
        Task<(bool Success, string Message)> AprobarPlanillaAsync(long planillaId, decimal montoAutorizado, string? observaciones, long usuarioId);

        /// <summary>
        /// Rechaza una planilla
        /// </summary>
        Task<(bool Success, string Message)> RechazarPlanillaAsync(long planillaId, string motivo, long usuarioId);

        /// <summary>
        /// Obtiene información de ventana de nómina actual (corte 16-15)
        /// </summary>
        (DateTime Inicio, DateTime Fin) ObtenerVentanaNominaActual();
    }
}
