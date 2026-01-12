using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.OP.Interfaces
{
    /// <summary>
    /// Servicio para filtros avanzados de OP_Cualitativo
    /// Soporta: Autocomplete de trabajos, date ranges, multi-select de estados
    /// </summary>
    public interface IOpAdvancedFiltersService
    {
        // ========== AUTOCOMPLETE ==========

        /// <summary>
        /// Obtiene lista de trabajos para autocomplete (búsqueda por texto)
        /// </summary>
        Task<List<TrabajoAutocompleteDto>> GetTrabajosAutocompleteAsync(string searchText, int maxResults = 20);

        /// <summary>
        /// Obtiene lista de moderadores para autocomplete
        /// </summary>
        Task<List<ModeradorAutocompleteDto>> GetModeradoresAutocompleteAsync(string searchText, int maxResults = 20);

        /// <summary>
        /// Obtiene lista de entrevistadores para autocomplete
        /// </summary>
        Task<List<EntrevistadorAutocompleteDto>> GetEntrevistadoresAutocompleteAsync(string searchText, int maxResults = 20);


        // ========== FILTROS CON RANGO DE FECHAS ==========

        /// <summary>
        /// Obtiene sesiones dentro de un rango de fechas con filtros adicionales
        /// </summary>
        Task<FilteredResultDto<SessionFilteredDto>> GetSessionsByDateRangeAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            string estado = null,
            int moderadorId = 0,
            int pageNumber = 1,
            int pageSize = 50);

        /// <summary>
        /// Obtiene entrevistas dentro de un rango de fechas
        /// </summary>
        Task<FilteredResultDto<InterviewFilteredDto>> GetInterviewsByDateRangeAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            string estado = null,
            string entrevistador = null,
            int pageNumber = 1,
            int pageSize = 50);


        // ========== MULTI-SELECT ==========

        /// <summary>
        /// Obtiene listado de estados disponibles para filtro
        /// </summary>
        Task<List<EstadoFilterDto>> GetAvailableEstadosAsync();

        /// <summary>
        /// Filtra sesiones por múltiples estados
        /// </summary>
        Task<FilteredResultDto<SessionFilteredDto>> FilterSessionsByMultipleEstadosAsync(
            List<string> estados,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            int pageNumber = 1,
            int pageSize = 50);

        /// <summary>
        /// Filtra entrevistas por múltiples estados
        /// </summary>
        Task<FilteredResultDto<InterviewFilteredDto>> FilterInterviewsByMultipleEstadosAsync(
            List<string> estados,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            int pageNumber = 1,
            int pageSize = 50);
    }

    // ========== DTOs PARA AUTOCOMPLETE ==========

    public class TrabajoAutocompleteDto
    {
        public int TrabajoId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Area { get; set; }
    }

    public class ModeradorAutocompleteDto
    {
        public int ModeradorId { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
    }

    public class EntrevistadorAutocompleteDto
    {
        public int EntrevistadorId { get; set; }
        public string Nombre { get; set; }
        public string Especialidad { get; set; }
    }

    // ========== DTOs PARA FILTROS ==========

    public class SessionFilteredDto
    {
        public int SesionId { get; set; }
        public int TrabajoId { get; set; }
        public string TrabajoCodigo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Ubicacion { get; set; }
        public string Estado { get; set; }
        public int NumeroParticipantes { get; set; }
        public string Moderador { get; set; }
    }

    public class InterviewFilteredDto
    {
        public int EntrevistaId { get; set; }
        public int TrabajoId { get; set; }
        public string TrabajoCodigo { get; set; }
        public DateTime FechaEjecucion { get; set; }
        public string Entrevistador { get; set; }
        public string Encuestado { get; set; }
        public int Duracion { get; set; }
        public decimal Completitud { get; set; }
        public string Estado { get; set; }
    }

    public class EstadoFilterDto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
    }

    public class FilteredResultDto<T>
    {
        public List<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages => (TotalRecords + PageSize - 1) / PageSize;
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;
    }
}
