using MatrixNext.Web.Services.PY;
using MatrixNext.Web.Services.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.PY.Controllers
{
    [Area("PY")]
    [Route("api/py/dashboard")]
    [Authorize(Roles = "Administrador,Gerente,Coordinador")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IExportService _exportService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IDashboardService dashboardService,
            IExportService exportService,
            ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _exportService = exportService;
            _logger = logger;
        }

        /// <summary>
        /// GET /api/py/dashboard/resumen-general?idUnidad=1
        /// Resumen general de proyectos y trabajos
        /// </summary>
        [HttpGet("resumen-general")]
        public async Task<IActionResult> ObtenerResumenGeneral([FromQuery] int? idUnidad = null)
        {
            var resultado = await _dashboardService.ObtenerResumenGeneralAsync(idUnidad);
            return Ok(resultado);
        }

        /// <summary>
        /// GET /api/py/dashboard/trabajos-por-gerente?idUnidad=1&idGerente=5&fechaInicio=2025-01-01
        /// Trabajos agrupados por gerente de proyectos
        /// </summary>
        [HttpGet("trabajos-por-gerente")]
        public async Task<IActionResult> ObtenerTrabajosPorGerente(
            [FromQuery] int? idUnidad = null,
            [FromQuery] long? idGerente = null,
            [FromQuery] DateTime? fechaInicio = null,
            [FromQuery] DateTime? fechaFin = null)
        {
            var resultado = await _dashboardService.ObtenerTrabajosPorGerenteAsync(
                idUnidad, idGerente, fechaInicio, fechaFin);
            return Ok(resultado);
        }

        /// <summary>
        /// GET /api/py/dashboard/trabajos-por-estado?idUnidad=1&fechaInicio=2025-01-01
        /// Trabajos agrupados por estado
        /// </summary>
        [HttpGet("trabajos-por-estado")]
        public async Task<IActionResult> ObtenerTrabajosPorEstado(
            [FromQuery] int? idUnidad = null,
            [FromQuery] DateTime? fechaInicio = null,
            [FromQuery] DateTime? fechaFin = null)
        {
            var resultado = await _dashboardService.ObtenerTrabajosPorEstadoAsync(
                idUnidad, fechaInicio, fechaFin);
            return Ok(resultado);
        }

        /// <summary>
        /// GET /api/py/dashboard/detalle-trabajos?page=1&pageSize=20&busqueda=Job123
        /// Detalle de trabajos con filtros y paginación
        /// </summary>
        [HttpGet("detalle-trabajos")]
        public async Task<IActionResult> ObtenerDetalleTrabajos(
            [FromQuery] int? idUnidad = null,
            [FromQuery] long? idGerente = null,
            [FromQuery] int? estado = null,
            [FromQuery] string? busqueda = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var resultado = await _dashboardService.ObtenerDetalleTrabajosAsync(
                idUnidad, idGerente, estado, busqueda, page, pageSize);
            return Ok(resultado);
        }

        /// <summary>
        /// GET /api/py/dashboard/export-excel?idUnidad=1&fechaInicio=2025-01-01
        /// Exporta el detalle de trabajos a Excel
        /// </summary>
        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportarExcel(
            [FromQuery] int? idUnidad = null,
            [FromQuery] long? idGerente = null,
            [FromQuery] int? estado = null,
            [FromQuery] string? busqueda = null)
        {
            try
            {
                // Obtener todos los datos sin paginación para export
                var resultado = await _dashboardService.ObtenerDetalleTrabajosAsync(
                    idUnidad, idGerente, estado, busqueda, 1, 10000);

                if (!resultado.IsSuccess || resultado.Data == null || !resultado.Data.Any())
                {
                    return BadRequest(new { mensaje = "No hay datos para exportar" });
                }

                // Configurar columnas personalizadas
                var configuracionColumnas = new Dictionary<string, string>
                {
                    { "IdTrabajo", "ID Trabajo" },
                    { "NombreTrabajo", "Nombre del Trabajo" },
                    { "IdProyecto", "ID Proyecto" },
                    { "NombreProyecto", "Proyecto" },
                    { "NombreGerente", "Gerente" },
                    { "NombreUnidad", "Unidad" },
                    { "FechaInicio", "Fecha Inicio" },
                    { "FechaCierre", "Fecha Cierre" },
                    { "Estado", "Estado" },
                    { "DiasAtrasado", "Días Atrasado" }
                };

                var excelBytes = await _exportService.ExportarExcelPersonalizadoAsync(
                    resultado.Data.ToList(),
                    "Dashboard_Trabajos",
                    configuracionColumnas,
                    "Trabajos",
                    $"Reporte de Trabajos - {DateTime.Now:dd/MM/yyyy HH:mm}");

                return File(
                    excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Dashboard_Trabajos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar dashboard PY a Excel");
                return StatusCode(500, new { mensaje = "Error al generar el archivo Excel" });
            }
        }

        /// <summary>
        /// GET /py/dashboard
        /// Vista principal del dashboard
        /// </summary>
        [HttpGet("/py/dashboard")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}
