using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MatrixNext.Data.Modules.TH.HWH.Models;
using MatrixNext.Data.Modules.TH.HWH.Services;

namespace MatrixNext.Web.Areas.TH.Controllers
{
    /// <summary>
    /// Controlador para gestión de Easy Work / Teletrabajo (HWH)
    /// </summary>
    [Area("TH")]
    [Authorize]
    public class HWHController : Controller
    {
        private readonly IHWHService _service;
        private readonly ILogger<HWHController> _logger;
        
        public HWHController(IHWHService service, ILogger<HWHController> logger)
        {
            _service = service;
            _logger = logger;
        }
        
        #region Vistas Principales
        
        /// <summary>
        /// Vista principal - Mis solicitudes de Easy Work
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// Vista de administración para jefes
        /// </summary>
        [HttpGet]
        public IActionResult Admin()
        {
            return View();
        }
        
        /// <summary>
        /// Vista para RRHH
        /// </summary>
        [HttpGet]
        public IActionResult RRHH()
        {
            return View();
        }
        
        #endregion
        
        #region APIs - Consultas
        
        /// <summary>
        /// Obtiene mis solicitudes de Easy Work
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> MisSolicitudes()
        {
            var userId = GetUserId();
            var solicitudes = await _service.ObtenerMisSolicitudesAsync(userId);
            return Json(new { success = true, data = solicitudes });
        }
        
        /// <summary>
        /// Obtiene las solicitudes del equipo (para jefes)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SolicitudesEquipo(int? estado, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var userId = GetUserId();
            var solicitudes = await _service.ObtenerSolicitudesEquipoAsync(userId, estado, fechaInicio, fechaFin);
            return Json(new { success = true, data = solicitudes });
        }
        
        /// <summary>
        /// Obtiene una solicitud por ID
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Obtener(long id)
        {
            var solicitud = await _service.ObtenerSolicitudAsync(id);
            if (solicitud == null)
            {
                return Json(new { success = false, message = "Solicitud no encontrada" });
            }
            return Json(new { success = true, data = solicitud });
        }
        
        /// <summary>
        /// Obtiene datos del Gantt para el usuario
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GanttUsuario(long usuario, DateTime fechaInicio, DateTime fechaFin)
        {
            var gantt = await _service.ObtenerGanttUsuarioAsync(usuario, fechaInicio, fechaFin);
            return Json(new { success = true, data = gantt });
        }
        
        /// <summary>
        /// Obtiene datos del Gantt para el equipo
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GanttEquipo(DateTime fechaInicio, DateTime fechaFin, int? estado)
        {
            var userId = GetUserId();
            var gantt = await _service.ObtenerGanttEquipoAsync(userId, fechaInicio, fechaFin, estado);
            return Json(new { success = true, data = gantt });
        }
        
        #endregion
        
        #region APIs - Operaciones
        
        /// <summary>
        /// Crea una nueva solicitud de Easy Work
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] HWHCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }
            
            var userId = GetUserId();
            var (success, message, id) = await _service.CrearSolicitudAsync(dto, userId);
            
            return Json(new { success, message, id });
        }
        
        /// <summary>
        /// Aprueba una solicitud de Easy Work
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Aprobar([FromBody] HWHGestionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }
            
            var userId = GetUserId();
            var (success, message) = await _service.AprobarSolicitudAsync(dto.Id, userId, dto.Observaciones);
            
            return Json(new { success, message });
        }
        
        /// <summary>
        /// Rechaza una solicitud de Easy Work
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Rechazar([FromBody] HWHGestionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }
            
            if (string.IsNullOrWhiteSpace(dto.Observaciones))
            {
                return Json(new { success = false, message = "Debe indicar el motivo del rechazo" });
            }
            
            var userId = GetUserId();
            var (success, message) = await _service.RechazarSolicitudAsync(dto.Id, userId, dto.Observaciones!);
            
            return Json(new { success, message });
        }
        
        /// <summary>
        /// Anula una solicitud de Easy Work
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Anular([FromBody] HWHGestionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }
            
            if (string.IsNullOrWhiteSpace(dto.Observaciones))
            {
                return Json(new { success = false, message = "Debe indicar el motivo de la anulación" });
            }
            
            var userId = GetUserId();
            var (success, message) = await _service.AnularSolicitudAsync(dto.Id, userId, dto.Observaciones!);
            
            return Json(new { success, message });
        }
        
        #endregion
        
        #region APIs - Catálogos
        
        /// <summary>
        /// Obtiene los estados disponibles
        /// </summary>
        [HttpGet]
        public IActionResult Estados()
        {
            var estados = new[]
            {
                new { id = 0, nombre = "Todos" },
                new { id = 1, nombre = "Pendiente" },
                new { id = 2, nombre = "Aprobado" },
                new { id = 3, nombre = "Rechazado" },
                new { id = 4, nombre = "Anulado" }
            };
            
            return Json(new { success = true, data = estados });
        }
        
        /// <summary>
        /// Obtiene los jefes aprobadores
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> JefesAprobadores()
        {
            var jefes = await _service.ObtenerJefesAprobadoresAsync();
            return Json(new { success = true, data = jefes });
        }
        
        #endregion
        
        #region Partials
        
        /// <summary>
        /// Modal para crear solicitud
        /// </summary>
        [HttpGet]
        public IActionResult _CreateModal()
        {
            return PartialView(new HWHCreateDto());
        }
        
        /// <summary>
        /// Modal para gestionar solicitud
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> _GestionModal(long id)
        {
            var solicitud = await _service.ObtenerSolicitudAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }
            return PartialView(solicitud);
        }
        
        /// <summary>
        /// Lista de solicitudes parcial
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> _Lista()
        {
            var userId = GetUserId();
            var solicitudes = await _service.ObtenerMisSolicitudesAsync(userId);
            return PartialView(solicitudes);
        }
        
        /// <summary>
        /// Lista de solicitudes del equipo parcial
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> _ListaEquipo(int? estado, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var userId = GetUserId();
            var solicitudes = await _service.ObtenerSolicitudesEquipoAsync(userId, estado, fechaInicio, fechaFin);
            return PartialView(solicitudes);
        }
        
        #endregion
        
        #region Helpers
        
        /// <summary>
        /// Obtiene el ID del usuario actual
        /// </summary>
        private long GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                           ?? User.FindFirst("sub")?.Value
                           ?? User.FindFirst("UserId")?.Value;
            
            if (long.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            
            return 0;
        }
        
        #endregion
    }
}
