using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.OP.Interfaces
{
    /// <summary>
    /// Servicio para reportes y exportes de OP_Cualitativo (Sesiones, Entrevistas, Moderadores)
    /// </summary>
    public interface IOpReportService
    {
        // ========== REPORTES DE SESIONES ==========
        
        /// <summary>
        /// Obtiene listado de sesiones para reporte con filtros avanzados
        /// </summary>
        Task<List<ReportSessionDto>> GetSessionsReportAsync(
            int? trabajoId = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            string estado = null,
            int pageNumber = 1,
            int pageSize = 50);

        /// <summary>
        /// Exporta sesiones a Excel
        /// </summary>
        Task<byte[]> ExportSessionsToExcelAsync(List<ReportSessionDto> sessions);

        /// <summary>
        /// Exporta sesiones a PDF
        /// </summary>
        Task<byte[]> ExportSessionsToPdfAsync(List<ReportSessionDto> sessions);


        // ========== REPORTES DE ENTREVISTAS ==========

        /// <summary>
        /// Obtiene listado de entrevistas para reporte
        /// </summary>
        Task<List<ReportInterviewDto>> GetInterviewsReportAsync(
            int? trabajoId = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            string estado = null,
            string entrevistador = null,
            int pageNumber = 1,
            int pageSize = 50);

        /// <summary>
        /// Exporta entrevistas a Excel
        /// </summary>
        Task<byte[]> ExportInterviewsToExcelAsync(List<ReportInterviewDto> interviews);

        /// <summary>
        /// Exporta entrevistas a PDF
        /// </summary>
        Task<byte[]> ExportInterviewsToPdfAsync(List<ReportInterviewDto> interviews);


        // ========== REPORTES DE MODERADORES ==========

        /// <summary>
        /// Obtiene listado de moderadores con estadísticas
        /// </summary>
        Task<List<ReportModeratorDto>> GetModeratorsReportAsync(
            int? trabajoId = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null);

        /// <summary>
        /// Exporta moderadores a Excel
        /// </summary>
        Task<byte[]> ExportModeratorsToExcelAsync(List<ReportModeratorDto> moderators);


        // ========== VALIDACIONES DE CONCURRENCIA ==========

        /// <summary>
        /// Verifica si hay sesiones simultáneas conflictivas para un moderador
        /// </summary>
        Task<bool> ValidateConcurrentSessionsAsync(int moderadorId, DateTime fechaInicio, DateTime fechaFin, int? sessionIdToExclude = null);

        /// <summary>
        /// Obtiene todas las sesiones simultáneas para un moderador
        /// </summary>
        Task<List<ConcurrentSessionDto>> GetConcurrentSessionsAsync(int moderadorId, DateTime fechaInicio, DateTime fechaFin);
    }


    // ========== DTOs PARA REPORTES ==========

    public class ReportSessionDto
    {
        public int SesionId { get; set; }
        public int TrabajoId { get; set; }
        public string TrabajoCodigo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Duracion { get; set; } // minutos
        public string Ubicacion { get; set; }
        public string Estado { get; set; }
        public int NumeroParticipantes { get; set; }
        public string Moderador { get; set; }
        public string Observaciones { get; set; }
    }

    public class ReportInterviewDto
    {
        public int EntrevistaId { get; set; }
        public int TrabajoId { get; set; }
        public string TrabajoCodigo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaEjecucion { get; set; }
        public string Entrevistador { get; set; }
        public string Encuestado { get; set; }
        public int Duracion { get; set; } // minutos
        public int Preguntas { get; set; }
        public int PreguntasRespondidas { get; set; }
        public string Estado { get; set; }
        public decimal Completitud { get; set; } // %
        public string Observaciones { get; set; }
    }

    public class ReportModeratorDto
    {
        public int ModeradorId { get; set; }
        public string Nombre { get; set; }
        public int TotalSesiones { get; set; }
        public int SesionesCompletadas { get; set; }
        public int HorasTotal { get; set; }
        public decimal PromedioParticipantes { get; set; }
        public DateTime UltimaSesion { get; set; }
        public string UltimoTrabajo { get; set; }
    }

    public class ConcurrentSessionDto
    {
        public int SesionId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Ubicacion { get; set; }
        public string EstadoConflicto { get; set; } // Conflicto/Advertencia/Ok
    }
}
