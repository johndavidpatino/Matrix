using MatrixNext.Web.Services.OP.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MatrixNext.Web.Areas.OP.Controllers
{
    /// <summary>
    /// Controller para reportes y exportes de OP_Cualitativo
    /// Maneja: Sesiones, Entrevistas, Moderadores, Exportes Excel/PDF
    /// </summary>
    [Area("OP")]
    [Route("OP/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportesController : ControllerBase
    {
        private readonly IOpReportService _reportService;
        private readonly ILogger<ReportesController> _logger;

        public ReportesController(
            IOpReportService reportService,
            ILogger<ReportesController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        // ========== SESIONES ==========

        /// <summary>
        /// GET: OP/Reportes/Sesiones
        /// Obtiene listado de sesiones con filtros avanzados
        /// </summary>
        [HttpGet("sesiones")]
        public async Task<IActionResult> GetSesiones(
            [FromQuery] int? trabajoId,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] string estado,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 50)
        {
            try
            {
                var sessions = await _reportService.GetSessionsReportAsync(
                    trabajoId, fechaDesde, fechaHasta, estado, pageNumber, pageSize);

                return Ok(new { success = true, data = sessions });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reporte de sesiones");
                return BadRequest(new { success = false, message = "Error al obtener reporte de sesiones. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: OP/Reportes/ExportSesionesExcel
        /// Exporta sesiones a Excel
        /// </summary>
        [HttpGet("export-sesiones-excel")]
        public async Task<IActionResult> ExportSesionesExcel(
            [FromQuery] int? trabajoId,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] string estado)
        {
            try
            {
                var sessions = await _reportService.GetSessionsReportAsync(
                    trabajoId, fechaDesde, fechaHasta, estado, 1, 10000);

                var excelBytes = await _reportService.ExportSessionsToExcelAsync(sessions);

                return File(excelBytes, 
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Sesiones_Cualitativo_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar sesiones a Excel");
                return BadRequest(new { success = false, message = "Error al exportar sesiones a Excel. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: OP/Reportes/ExportSesionesPdf
        /// Exporta sesiones a PDF
        /// </summary>
        [HttpGet("export-sesiones-pdf")]
        public async Task<IActionResult> ExportSesionesPdf(
            [FromQuery] int? trabajoId,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] string estado)
        {
            try
            {
                var sessions = await _reportService.GetSessionsReportAsync(
                    trabajoId, fechaDesde, fechaHasta, estado, 1, 10000);

                var pdfBytes = await _reportService.ExportSessionsToPdfAsync(sessions);

                return File(pdfBytes,
                    "application/pdf",
                    $"Sesiones_Cualitativo_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar sesiones a PDF");
                return BadRequest(new { success = false, message = "Error al exportar sesiones a PDF. Por favor intente nuevamente." });
            }
        }


        // ========== ENTREVISTAS ==========

        /// <summary>
        /// GET: OP/Reportes/Entrevistas
        /// Obtiene listado de entrevistas con filtros
        /// </summary>
        [HttpGet("entrevistas")]
        public async Task<IActionResult> GetEntrevistas(
            [FromQuery] int? trabajoId,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] string estado,
            [FromQuery] string entrevistador,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 50)
        {
            try
            {
                var interviews = await _reportService.GetInterviewsReportAsync(
                    trabajoId, fechaDesde, fechaHasta, estado, entrevistador, pageNumber, pageSize);

                return Ok(new { success = true, data = interviews });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reporte de entrevistas");
                return BadRequest(new { success = false, message = "Error al obtener reporte de entrevistas. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: OP/Reportes/ExportEntrevistasExcel
        /// Exporta entrevistas a Excel
        /// </summary>
        [HttpGet("export-entrevistas-excel")]
        public async Task<IActionResult> ExportEntrevistasExcel(
            [FromQuery] int? trabajoId,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] string estado,
            [FromQuery] string entrevistador)
        {
            try
            {
                var interviews = await _reportService.GetInterviewsReportAsync(
                    trabajoId, fechaDesde, fechaHasta, estado, entrevistador, 1, 10000);

                var excelBytes = await _reportService.ExportInterviewsToExcelAsync(interviews);

                return File(excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Entrevistas_Cualitativo_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar entrevistas a Excel");
                return BadRequest(new { success = false, message = "Error al exportar entrevistas a Excel. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: OP/Reportes/ExportEntrevistasPdf
        /// Exporta entrevistas a PDF
        /// </summary>
        [HttpGet("export-entrevistas-pdf")]
        public async Task<IActionResult> ExportEntrevistasPdf(
            [FromQuery] int? trabajoId,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] string estado,
            [FromQuery] string entrevistador)
        {
            try
            {
                var interviews = await _reportService.GetInterviewsReportAsync(
                    trabajoId, fechaDesde, fechaHasta, estado, entrevistador, 1, 10000);

                var pdfBytes = await _reportService.ExportInterviewsToPdfAsync(interviews);

                return File(pdfBytes,
                    "application/pdf",
                    $"Entrevistas_Cualitativo_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar entrevistas a PDF");
                return BadRequest(new { success = false, message = "Error al exportar entrevistas a PDF. Por favor intente nuevamente." });
            }
        }


        // ========== MODERADORES ==========

        /// <summary>
        /// GET: OP/Reportes/Moderadores
        /// Obtiene estadísticas de moderadores
        /// </summary>
        [HttpGet("moderadores")]
        public async Task<IActionResult> GetModerators(
            [FromQuery] int? trabajoId,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta)
        {
            try
            {
                var moderators = await _reportService.GetModeratorsReportAsync(
                    trabajoId, fechaDesde, fechaHasta);

                return Ok(new { success = true, data = moderators });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reporte de moderadores");
                return BadRequest(new { success = false, message = "Error al obtener reporte de moderadores. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: OP/Reportes/ExportModeradoresExcel
        /// Exporta moderadores a Excel
        /// </summary>
        [HttpGet("export-moderadores-excel")]
        public async Task<IActionResult> ExportModeradoresExcel(
            [FromQuery] int? trabajoId,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta)
        {
            try
            {
                var moderators = await _reportService.GetModeratorsReportAsync(
                    trabajoId, fechaDesde, fechaHasta);

                var excelBytes = await _reportService.ExportModeratorsToExcelAsync(moderators);

                return File(excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Moderadores_Cualitativo_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar moderadores a Excel");
                return BadRequest(new { success = false, message = "Error al exportar moderadores a Excel. Por favor intente nuevamente." });
            }
        }


        // ========== VALIDACIONES DE CONCURRENCIA ==========

        /// <summary>
        /// POST: OP/Reportes/ValidarConcurrencia
        /// Valida si hay sesiones simultáneas conflictivas
        /// </summary>
        [HttpPost("validar-concurrencia")]
        public async Task<IActionResult> ValidateConcurrentSessions(
            [FromBody] ValidateConcurrencyRequest request)
        {
            try
            {
                var isValid = await _reportService.ValidateConcurrentSessionsAsync(
                    request.ModeradorId,
                    request.FechaInicio,
                    request.FechaFin,
                    request.SesionIdToExclude);

                if (!isValid)
                {
                    var conflicts = await _reportService.GetConcurrentSessionsAsync(
                        request.ModeradorId,
                        request.FechaInicio,
                        request.FechaFin);

                    return Ok(new
                    {
                        success = false,
                        valid = false,
                        message = "Hay sesiones simultáneas conflictivas",
                        conflicts
                    });
                }

                return Ok(new
                {
                    success = true,
                    valid = true,
                    message = "No hay conflictos de horarios"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar concurrencia");
                return BadRequest(new { success = false, message = "Error al validar concurrencia. Por favor intente nuevamente." });
            }
        }
    }

    public class ValidateConcurrencyRequest
    {
        public int ModeradorId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int? SesionIdToExclude { get; set; }
    }
}
