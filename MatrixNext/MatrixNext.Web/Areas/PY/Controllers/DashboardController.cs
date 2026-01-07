using MatrixNext.Web.Services.PY;
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
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IDashboardService dashboardService,
            ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
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
