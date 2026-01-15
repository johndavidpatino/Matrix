using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Models.OP;

namespace MatrixNext.Data.Adapters.OP
{
    /// <summary>
    /// Interfaz para acceso a datos de planillas con aprobación/rechazo
    /// </summary>
    public interface IPlanillasAprobacionAdapter
    {
        /// <summary>
        /// Obtiene planillas aprobadas por filtros
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
        /// Obtiene planillas rechazadas por filtros
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
        /// Aprueba una planilla con monto autorizado
        /// </summary>
        Task<bool> AprobarPlanillaAsync(long planillaId, decimal montoAutorizado, string? observaciones, long usuarioId);

        /// <summary>
        /// Rechaza una planilla
        /// </summary>
        Task<bool> RechazarPlanillaAsync(long planillaId, string motivo, long usuarioId);

        /// <summary>
        /// Helper para calcular ventana de nómina (corte 16-15)
        /// </summary>
        DateTime GetNominaWindowStart();

        /// <summary>
        /// Helper para calcular fin de ventana de nómina (corte 16-15)
        /// </summary>
        DateTime GetNominaWindowEnd();
    }
}
