using MatrixNext.Data.Adapters.PY.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY.Interfaces
{
    /// <summary>
    /// Servicio de dominio para gestión de planillas de moderación e informes UU.
    /// Orquesta creación, actualización y exportación de planillas cuantitativas.
    /// </summary>
    public interface IPyPlanillasService
    {
        /// <summary>
        /// Obtiene todas las técnicas disponibles para un tipo específico.
        /// </summary>
        Task<List<TecnicaDto>> ObtenerTecnicas(string tipoTecnica);

        /// <summary>
    /// Obtiene lista de moderadores disponibles para una fecha específica.
        Task<int> CrearPlanillaModeracion(PlanillaModeracionInputDto input);

        /// <summary>
        /// Actualiza planilla de moderación existente (cambios en asignaciones).
        /// </summary>
        Task<bool> ActualizarPlanillaModeracion(PlanillaModeracionActualizacionDto input);

        /// <summary>
        /// Obtiene planillas de informes para gestión y seguimiento.
        /// </summary>
        Task<List<PlanillaInformesDto>> ObtenerPlanillasInformes(DateTime fechaInicio, DateTime fechaFinal);

        /// <summary>
        /// Actualiza estado de planilla de informes (en progreso, completada, revisada).
        /// </summary>
        Task<bool> ActualizarEstadoPlanillaInformes(int idPlanilla, string nuevoEstado);

        /// <summary>
        /// Obtiene planillas listas para exportar a sistemas UU.
        /// </summary>
        Task<List<PlanillaListDto>> ObtenerPlanillasParaExportar(DateTime fechaInicio, DateTime fechaFinal);

        /// <summary>
        /// Marca planilla como exportada a UU (actualiza fecha y estado).
        /// </summary>
        Task<bool> MarcarExportada(int idPlanilla);

        /// <summary>
        /// Valida que planilla cumpla con requisitos antes de permitir moderación.
        /// </summary>
        Task<List<string>> ValidarPlanillaModeracion(int idPlanilla);

        /// <summary>
        /// Obtiene resumen estadístico de planillas (cantidad moderaciones, informes completados).
        /// </summary>
        Task<dynamic> ObtenerEstadisticasPlanillas(DateTime fechaInicio, DateTime fechaFinal);
    }
}
