using MatrixNext.Data.Adapters.PY.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY.Interfaces
{
    /// <summary>
    /// Servicio de dominio para gestión de distribución y seguimiento de entrevistas cualitativas.
    /// Orquesta asignación de entrevistas a moderadores, seguimiento de estado y logística.
    /// </summary>
    public interface IPyDistribucionEntrevistasService
    {
        /// <summary>
        /// Obtiene entrevistas pendientes por asignar para un trabajo.
        /// </summary>
        Task<List<EntrevistaCualiDto>> ObtenerEntrevistasPendientes(int trabajoId);

        /// <summary>
        /// Obtiene distribución de entrevistas asignadas con estado actual.
        /// </summary>
        Task<List<DistribucionEntrevistaDto>> ObtenerDistribucionAsignada(int trabajoId);

        /// <summary>
        /// Guarda nueva distribución (asigna entrevistas a moderadores).
        /// Valida disponibilidad de moderadores y agenda.
        /// </summary>
    Task<int> GuardarDistribucion(DistribucionEntrevistaInputDto input, string usuario);
        Task<bool> ActualizarEstadoDistribucion(int distribucionId, string nuevoEstado, string observaciones);

        /// <summary>
        /// Obtiene log de cambios en distribución (auditoría de cambios de estado).
        /// </summary>
        Task<List<LogEntrevistaCualiDto>> ObtenerLogDistribucion(int distribucionId);

        /// <summary>
        /// Registra evento en log de entrevista (cambios de estado, observaciones, incidencias).
        /// </summary>
        Task<int> GuardarLogEntrevista(int distribucionId, string evento, string descripcion, string usuario);

        /// <summary>
        /// Obtiene disponibilidad de moderadores para una fecha/zona específica.
        /// </summary>
        Task<List<ModeradorCualiDto>> ObtenerModeradoresDisponibles(DateTime fecha, string zona);

        /// <summary>
        /// Genera reporte de entrevistas programadas vs completadas.
        /// </summary>
        Task<dynamic> ObtenerAvanceEntrevistas(int trabajoId);

        /// <summary>
        /// Valida que todas las entrevistas requeridas estén distribuidas.
        /// </summary>
        Task<List<string>> ValidarDistribucionCompleta(int trabajoId);
    }
}
