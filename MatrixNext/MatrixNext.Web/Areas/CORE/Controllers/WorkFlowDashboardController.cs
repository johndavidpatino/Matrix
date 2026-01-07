using MatrixNext.Web.Services.CORE;
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
        private readonly ILogger<WorkFlowDashboardController> _logger;

        public WorkFlowDashboardController(
            IWorkFlowDashboardService workFlowDashboardService,
            ILogger<WorkFlowDashboardController> logger)
        {
            _workFlowDashboardService = workFlowDashboardService;
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
