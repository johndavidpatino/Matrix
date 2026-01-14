using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Data.Models.RP;
using MatrixNext.Data.Services.RP;
using MatrixNext.Data.Services;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.RP.Controllers
{
    /// <summary>
    /// Controller REST para Reportes
    /// Endpoints: GET /api/rp/reportes
    /// REGLA 1: REST API standard
    /// REGLA 9: Validación permisos [Authorize]
    /// REGLA 10: Retorna ApiResponse<T>
    /// </summary>
    [ApiController]
    [Route("api/[area]/[controller]")]
    [Area("RP")]
    public class ReportesController : ControllerBase
    {
        private readonly IReportesService _service;
        private readonly ILogger<ReportesController> _logger;

        public ReportesController(IReportesService service, ILogger<ReportesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ============================================
        // LISTAR REPORTES
        // ============================================

        /// <summary>
        /// GET /api/rp/reportes
        /// Obtiene listado de reportes disponibles
        /// [Authorize] requerido
        /// </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<ReporteDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReportes()
        {
            try
            {
                _logger.LogInformation("[ReportesController] GET: Obteniendo reportes disponibles");

                var resultado = await _service.ObtenerReportesDisponiblesAsync();

                if (!resultado.Success)
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ReportesController] Error en GetReportes");
                return StatusCode(500, ApiResponse<string>.Error("Error interno del servidor"));
            }
        }

        // ============================================
        // GENERAR REPORTE CON FILTROS
        // ============================================

        /// <summary>
        /// POST /api/rp/reportes/{id}/generar
        /// Genera reporte con filtros aplicados
        /// Soporta: fechas, paginación, búsqueda
        /// </summary>
        [HttpPost("{id}/generar")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<ReporteResultadoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerarReporte(
            int id,
            [FromBody] ReporteFiltrosDTO filtros,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"[ReportesController] POST: Generando reporte {id}");

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.BadRequest("Parámetros inválidos"));

                // Obtener usuario del contexto (TODO: integrar con Identity)
                var usuarioId = 1; // Temporal

                var filtrosCompletos = filtros ?? new ReporteFiltrosDTO();
                filtrosCompletos.UsuarioId = usuarioId;

                var resultado = await _service.GenerarReporteAsync(id, filtrosCompletos, cancellationToken);

                if (!resultado.Success)
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning($"[ReportesController] Solicitud cancelada para reporte {id}");
                return StatusCode(408, ApiResponse<string>.Error("Solicitud cancelada"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[ReportesController] Error en GenerarReporte {id}");
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        // ============================================
        // OBTENER DETALLES DE REPORTE
        // ============================================

        /// <summary>
        /// GET /api/rp/reportes/{id}
        /// Obtiene detalles de un reporte específico
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<ReporteDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetReporte(int id)
        {
            try
            {
                _logger.LogInformation($"[ReportesController] GET: Obteniendo detalles reporte {id}");

                // Obtener usuario del contexto (TODO: integrar con Identity)
                var usuarioId = 1; // Temporal

                var resultado = await _service.ObtenerReporteAsync(id, usuarioId);

                if (!resultado.Success)
                    return NotFound(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[ReportesController] Error en GetReporte {id}");
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        // ============================================
        // EXPORTAR A EXCEL
        // ============================================

        /// <summary>
        /// GET /api/rp/reportes/{id}/export-excel?fechaDesde=...&fechaHasta=...
        /// Descarga reporte en formato Excel
        /// </summary>
        [HttpGet("{id}/export-excel")]
        [Authorize]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ExportExcel(
            int id,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 50)
        {
            try
            {
                _logger.LogInformation($"[ReportesController] GET: Exportando a Excel reporte {id}");

                // Obtener usuario del contexto (TODO: integrar con Identity)
                var usuarioId = 1; // Temporal

                var filtros = new ReporteFiltrosDTO
                {
                    FechaDesde = fechaDesde ?? DateTime.Now.AddDays(-30),
                    FechaHasta = fechaHasta ?? DateTime.Now,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    UsuarioId = usuarioId
                };

                var export = await _service.PrepararExportExcelAsync(id, filtros, usuarioId);

                if (export.Contenido == null || export.Contenido.Length == 0)
                    return BadRequest(ApiResponse<string>.Error("No hay datos para exportar"));

                return File(export.Contenido, export.ContentType, export.Nombre);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[ReportesController] Error en ExportExcel {id}");
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        // ============================================
        // EXPORTAR A PDF
        // ============================================

        /// <summary>
        /// GET /api/rp/reportes/{id}/export-pdf?fechaDesde=...&fechaHasta=...
        /// Descarga reporte en formato PDF
        /// </summary>
        [HttpGet("{id}/export-pdf")]
        [Authorize]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ExportPdf(
            int id,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 50)
        {
            try
            {
                _logger.LogInformation($"[ReportesController] GET: Exportando a PDF reporte {id}");

                // Obtener usuario del contexto (TODO: integrar con Identity)
                var usuarioId = 1; // Temporal

                var filtros = new ReporteFiltrosDTO
                {
                    FechaDesde = fechaDesde ?? DateTime.Now.AddDays(-30),
                    FechaHasta = fechaHasta ?? DateTime.Now,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    UsuarioId = usuarioId
                };

                var export = await _service.PrepararExportPdfAsync(id, filtros, usuarioId);

                if (export.Contenido == null || export.Contenido.Length == 0)
                    return BadRequest(ApiResponse<string>.Error("No hay datos para exportar"));

                return File(export.Contenido, export.ContentType, export.Nombre);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[ReportesController] Error en ExportPdf {id}");
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        // ============================================
        // INDICADORES Y DASHBOARDS
        // ============================================

        /// <summary>
        /// GET /api/rp/reportes/indicadores/calidad?fechaDesde=...&fechaHasta=...
        /// Obtiene indicadores de calidad para dashboard
        /// </summary>
        [HttpGet("indicadores/calidad")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<Dictionary<string, object>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetIndicadoresCalidad(
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta)
        {
            try
            {
                _logger.LogInformation("[ReportesController] GET: Indicadores de calidad");

                // Obtener usuario del contexto (TODO: integrar con Identity)
                var usuarioId = 1; // Temporal

                var resultado = await _service.ObtenerIndicadoresCalidadAsync(
                    fechaDesde ?? DateTime.Now.AddDays(-30),
                    fechaHasta ?? DateTime.Now,
                    usuarioId);

                if (!resultado.Success)
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ReportesController] Error en GetIndicadoresCalidad");
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        /// <summary>
        /// GET /api/rp/reportes/indicadores/cumplimiento?fechaDesde=...&fechaHasta=...
        /// Obtiene indicadores de cumplimiento para dashboard
        /// </summary>
        [HttpGet("indicadores/cumplimiento")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<Dictionary<string, object>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetIndicadoresCumplimiento(
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta)
        {
            try
            {
                _logger.LogInformation("[ReportesController] GET: Indicadores de cumplimiento");

                // Obtener usuario del contexto (TODO: integrar con Identity)
                var usuarioId = 1; // Temporal

                var resultado = await _service.ObtenerIndicadoresCumplimientoAsync(
                    fechaDesde ?? DateTime.Now.AddDays(-30),
                    fechaHasta ?? DateTime.Now,
                    usuarioId);

                if (!resultado.Success)
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ReportesController] Error en GetIndicadoresCumplimiento");
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
