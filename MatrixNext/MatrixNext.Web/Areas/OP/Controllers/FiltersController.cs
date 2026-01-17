using MatrixNext.Web.Services.OP.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Web.Areas.OP.Controllers
{
    /// <summary>
    /// API Controller para filtros avanzados de OP_Cualitativo
    /// Soporta: Autocomplete, Date ranges, Multi-select
    /// </summary>
    [Area("OP")]
    [Route("api/OP/[controller]")]
    [ApiController]
    [Authorize]
    public class FiltersController : ControllerBase
    {
        private readonly IOpAdvancedFiltersService _filtersService;
        private readonly ILogger<FiltersController> _logger;

        public FiltersController(
            IOpAdvancedFiltersService filtersService,
            ILogger<FiltersController> logger)
        {
            _filtersService = filtersService;
            _logger = logger;
        }

        // ========== AUTOCOMPLETE ==========

        /// <summary>
        /// GET: api/OP/filters/trabajos/autocomplete?search=XXX
        /// Obtiene trabajos para autocomplete
        /// </summary>
        [HttpGet("trabajos/autocomplete")]
        public async Task<IActionResult> GetTrabajosAutocomplete(
            [FromQuery] string search,
            [FromQuery] int maxResults = 20)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(search) || search.Length < 2)
                {
                    return Ok(new { data = new List<object>() });
                }

                var trabajos = await _filtersService.GetTrabajosAutocompleteAsync(search, maxResults);
                return Ok(new { success = true, data = trabajos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en autocomplete de trabajos");
                return BadRequest(new { success = false, message = "Error en autocomplete de trabajos. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: api/OP/filters/moderadores/autocomplete?search=XXX
        /// Obtiene moderadores para autocomplete
        /// </summary>
        [HttpGet("moderadores/autocomplete")]
        public async Task<IActionResult> GetModeradoresAutocomplete(
            [FromQuery] string search,
            [FromQuery] int maxResults = 20)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(search) || search.Length < 2)
                {
                    return Ok(new { data = new List<object>() });
                }

                var moderadores = await _filtersService.GetModeradoresAutocompleteAsync(search, maxResults);
                return Ok(new { success = true, data = moderadores });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en autocomplete de moderadores");
                return BadRequest(new { success = false, message = "Error en autocomplete de moderadores. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: api/OP/filters/entrevistadores/autocomplete?search=XXX
        /// Obtiene entrevistadores para autocomplete
        /// </summary>
        [HttpGet("entrevistadores/autocomplete")]
        public async Task<IActionResult> GetEntrevistadoresAutocomplete(
            [FromQuery] string search,
            [FromQuery] int maxResults = 20)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(search) || search.Length < 2)
                {
                    return Ok(new { data = new List<object>() });
                }

                var entrevistadores = await _filtersService.GetEntrevistadoresAutocompleteAsync(search, maxResults);
                return Ok(new { success = true, data = entrevistadores });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en autocomplete de entrevistadores");
                return BadRequest(new { success = false, message = "Error en autocomplete de entrevistadores. Por favor intente nuevamente." });
            }
        }

        // ========== FILTROS POR RANGO DE FECHAS ==========

        /// <summary>
        /// GET: api/OP/filters/sesiones/by-date-range
        /// Filtra sesiones por rango de fechas
        /// </summary>
        [HttpGet("sesiones/by-date-range")]
        public async Task<IActionResult> GetSessionsByDateRange(
            [FromQuery] DateTime fechaDesde,
            [FromQuery] DateTime fechaHasta,
            [FromQuery] string estado,
            [FromQuery] int moderadorId = 0,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 50)
        {
            try
            {
                var result = await _filtersService.GetSessionsByDateRangeAsync(
                    fechaDesde, fechaHasta, estado, moderadorId, pageNumber, pageSize);

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al filtrar sesiones por rango de fechas");
                return BadRequest(new { success = false, message = "Error al filtrar sesiones por rango de fechas. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: api/OP/filters/entrevistas/by-date-range
        /// Filtra entrevistas por rango de fechas
        /// </summary>
        [HttpGet("entrevistas/by-date-range")]
        public async Task<IActionResult> GetInterviewsByDateRange(
            [FromQuery] DateTime fechaDesde,
            [FromQuery] DateTime fechaHasta,
            [FromQuery] string estado,
            [FromQuery] string entrevistador,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 50)
        {
            try
            {
                var result = await _filtersService.GetInterviewsByDateRangeAsync(
                    fechaDesde, fechaHasta, estado, entrevistador, pageNumber, pageSize);

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al filtrar entrevistas por rango de fechas");
                return BadRequest(new { success = false, message = "Error al filtrar entrevistas por rango de fechas. Por favor intente nuevamente." });
            }
        }

        // ========== ESTADOS DISPONIBLES ==========

        /// <summary>
        /// GET: api/OP/filters/estados
        /// Obtiene listado de estados disponibles
        /// </summary>
        [HttpGet("estados")]
        public async Task<IActionResult> GetAvailableEstados()
        {
            try
            {
                var estados = await _filtersService.GetAvailableEstadosAsync();
                return Ok(new { success = true, data = estados });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estados disponibles");
                return BadRequest(new { success = false, message = "Error al obtener estados disponibles. Por favor intente nuevamente." });
            }
        }

        // ========== MULTI-SELECT ==========

        /// <summary>
        /// POST: api/OP/filters/sesiones/by-multiple-estados
        /// Filtra sesiones por múltiples estados
        /// </summary>
        [HttpPost("sesiones/by-multiple-estados")]
        public async Task<IActionResult> FilterSessionsByMultipleEstados(
            [FromBody] FilterByMultipleEstadosRequest request)
        {
            try
            {
                var result = await _filtersService.FilterSessionsByMultipleEstadosAsync(
                    request.Estados,
                    request.FechaDesde,
                    request.FechaHasta,
                    request.PageNumber,
                    request.PageSize);

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al filtrar sesiones por múltiples estados");
                return BadRequest(new { success = false, message = "Error al filtrar sesiones por múltiples estados. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// POST: api/OP/filters/entrevistas/by-multiple-estados
        /// Filtra entrevistas por múltiples estados
        /// </summary>
        [HttpPost("entrevistas/by-multiple-estados")]
        public async Task<IActionResult> FilterInterviewsByMultipleEstados(
            [FromBody] FilterByMultipleEstadosRequest request)
        {
            try
            {
                var result = await _filtersService.FilterInterviewsByMultipleEstadosAsync(
                    request.Estados,
                    request.FechaDesde,
                    request.FechaHasta,
                    request.PageNumber,
                    request.PageSize);

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al filtrar entrevistas por múltiples estados");
                return BadRequest(new { success = false, message = "Error al filtrar entrevistas por múltiples estados. Por favor intente nuevamente." });
            }
        }
    }

    public class FilterByMultipleEstadosRequest
    {
        public List<string> Estados { get; set; } = new();
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
