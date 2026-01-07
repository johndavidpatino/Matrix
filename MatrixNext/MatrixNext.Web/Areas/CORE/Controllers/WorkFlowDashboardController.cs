using MatrixNext.Web.Services.CORE;
using MatrixNext.Web.Services.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.CORE.Controllers
{
    [Area("CORE")]
    [Route("api/core/workflow-dashboard")]
    [Authorize(Roles = "Administrador,Gerente,Coordinador")]
    public class WorkFlowDashboardController : Controller
    {
        private readonly IWorkFlowDashboardService _workFlowDashboardService;
        private readonly IExportService _exportService;
        private readonly ILogger<WorkFlowDashboardController> _logger;

        public WorkFlowDashboardController(
            IWorkFlowDashboardService workFlowDashboardService,
            IExportService exportService,
            ILogger<WorkFlowDashboardController> logger)
        {
            _workFlowDashboardService = workFlowDashboardService;
            _exportService = exportService;
            _logger = logger;
        }

        /// <summary>
        /// GET /api/core/workflow-dashboard/resumen-general
        /// Resumen general de tareas y estado del WorkFlow
        /// </summary>
        [HttpGet("resumen-general")]
        public async Task<IActionResult> ObtenerResumenGeneral()
        {
            var resultado = await _workFlowDashboardService.ObtenerResumenGeneralAsync();
            return Ok(resultado);
        }

        /// <summary>
        /// GET /api/core/workflow-dashboard/tareas-por-estado?idTipoHilo=1
        /// Tareas agrupadas por estado con información de atraso
        /// </summary>
        [HttpGet("tareas-por-estado")]
        public async Task<IActionResult> ObtenerTareasPorEstado(
            [FromQuery] int? idTipoHilo = null,
            [FromQuery] DateTime? fechaInicio = null,
            [FromQuery] DateTime? fechaFin = null)
        {
            var resultado = await _workFlowDashboardService.ObtenerTareasPorEstadoAsync(
                idTipoHilo, fechaInicio, fechaFin);
            return Ok(resultado);
        }

        /// <summary>
        /// GET /api/core/workflow-dashboard/tareas-por-prioridad?idTipoHilo=1
        /// Tareas agrupadas por prioridad (crítica, alta, normal, baja)
        /// </summary>
        [HttpGet("tareas-por-prioridad")]
        public async Task<IActionResult> ObtenerTareasPorPrioridad(
            [FromQuery] int? idTipoHilo = null)
        {
            var resultado = await _workFlowDashboardService.ObtenerTareasPorPrioridadAsync(idTipoHilo);
            return Ok(resultado);
        }

        /// <summary>
        /// GET /api/core/workflow-dashboard/tareas-proximas-avencer?diasAlerta=3
        /// Tareas próximas a vencer (alarma)
        /// </summary>
        [HttpGet("tareas-proximas-avencer")]
        public async Task<IActionResult> ObtenerTareasProximasAvencer(
            [FromQuery] int diasAlerta = 3)
        {
            var resultado = await _workFlowDashboardService.ObtenerTareasProximasAVencerAsync(diasAlerta);
            return Ok(resultado);
        }

        /// <summary>
        /// GET /api/core/workflow-dashboard/detalle-tareas?page=1&pageSize=20
        /// Detalle de tareas con filtros y paginación
        /// </summary>
        [HttpGet("detalle-tareas")]
        public async Task<IActionResult> ObtenerDetalleTareas(
            [FromQuery] int? idTipoHilo = null,
            [FromQuery] string? estado = null,
            [FromQuery] int? prioridad = null,
            [FromQuery] string? busqueda = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var resultado = await _workFlowDashboardService.ObtenerDetalleTareasAsync(
                idTipoHilo, estado, prioridad, busqueda, page, pageSize);
            return Ok(resultado);
        }

        /// <summary>
        /// GET /api/core/workflow-dashboard/tareas-por-usuario
        /// Tareas asignadas agrupadas por usuario (carga de trabajo)
        /// </summary>
        [HttpGet("tareas-por-usuario")]
        public async Task<IActionResult> ObtenerTareasPorUsuario()
        {
            var resultado = await _workFlowDashboardService.ObtenerTareasPorUsuarioAsync();
            return Ok(resultado);
        }

        /// <summary>
        /// GET /api/core/workflow-dashboard/export-excel?estado=1&prioridad=Alta
        /// Exporta el detalle de tareas a Excel
        /// </summary>
        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportarExcel(
            [FromQuery] int? estado = null,
            [FromQuery] string? prioridad = null,
            [FromQuery] long? idUsuario = null,
            [FromQuery] string? busqueda = null)
        {
            try
            {
                // Obtener todos los datos sin paginación
                var resultado = await _workFlowDashboardService.ObtenerDetalleTareasAsync(
                    estado, prioridad, null, busqueda, 1, 10000);

                if (!resultado.IsSuccess || resultado.Data == null || !resultado.Data.Any())
                {
                    return BadRequest(new { mensaje = "No hay datos para exportar" });
                }

                // Configurar columnas personalizadas
                var configuracionColumnas = new Dictionary<string, string>
                {
                    { "IdWorkFlow", "ID Tarea" },
                    { "Titulo", "Título" },
                    { "Descripcion", "Descripción" },
                    { "Estado", "Estado" },
                    { "Prioridad", "Prioridad" },
                    { "FechaCreacion", "Fecha Creación" },
                    { "FechaVencimiento", "Fecha Vencimiento" },
                    { "DiasRestantes", "Días Restantes" },
                    { "NombreUsuarioAsignado", "Usuario Asignado" },
                    { "Observaciones", "Observaciones" }
                };

                var excelBytes = await _exportService.ExportarExcelPersonalizadoAsync(
                    resultado.Data.ToList(),
                    "Dashboard_Tareas",
                    configuracionColumnas,
                    "Tareas WorkFlow",
                    $"Reporte de Tareas WorkFlow - {DateTime.Now:dd/MM/yyyy HH:mm}");

                return File(
                    excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Dashboard_Tareas_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar dashboard WorkFlow a Excel");
                return StatusCode(500, new { mensaje = "Error al generar el archivo Excel" });
            }
        }

        /// <summary>
        /// GET /core/workflow-dashboard
        /// Vista principal del dashboard de WorkFlow
        /// </summary>
        [HttpGet("/core/workflow-dashboard")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}
