using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using MatrixNext.Data.Modules.TH.Ausencias.Models;
using MatrixNext.Data.Modules.TH.Ausencias.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.TH.Controllers
{
    /// <summary>
    /// Controlador para gestión de Solicitudes de Ausencias
    /// Proporciona operaciones para crear, consultar y aprobar solicitudes de ausencia de empleados
    /// </summary>
    [Area("TH")]
    [Route("TH/Ausencias")]
    [Authorize]
    public class AusenciasController : Controller
    {
        private readonly AusenciaService _ausenciaService;
        private readonly ILogger<AusenciasController> _logger;

        public AusenciasController(AusenciaService ausenciaService, ILogger<AusenciasController> logger)
        {
            _ausenciaService = ausenciaService ?? throw new ArgumentNullException(nameof(ausenciaService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private long GetCurrentUserId()
        {
            var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User?.FindFirst("Id")?.Value;
            if (long.TryParse(idClaim, out var id))
            {
                return id;
            }

            throw new InvalidOperationException("Id de usuario autenticado no disponible");
        }

        // Request DTOs for JSON binding
        public class CrearSolicitudRequest
        {
            public long idEmpleado { get; set; }
            public DateTime FiniCausacion { get; set; }
            public DateTime FFinCausacion { get; set; }
            public DateTime FInicio { get; set; }
            public DateTime FFin { get; set; }
            public short DiasCalendario { get; set; }
            public short DiasLaborales { get; set; }
            public byte Tipo { get; set; }
            public long AprobadoPor { get; set; }
            public string? ObservacionesSolicitud { get; set; }
        }

        public class AprobarSolicitudRequest
        {
            public long idSolicitud { get; set; }
            public long AprobadoPor { get; set; }
            public byte VoBo { get; set; }
            public string? ObservacionesAprobacion { get; set; }
        }

        public class RechazarSolicitudRequest
        {
            public long idSolicitud { get; set; }
            public long AprobadoPor { get; set; }
            public string? Motivo { get; set; }
        }

        public class CrearIncapacidadRequest
        {
            public long idSolicitudAusencia { get; set; }
            public byte? EntidadConsulta { get; set; }
            public string? IPS { get; set; }
            public string? RegistroMedico { get; set; }
            public byte? TipoIncapacidad { get; set; }
            public byte? ClaseAusencia { get; set; }
            public byte? SOAT { get; set; }
            public DateTime? FechaAccidenteTrabajo { get; set; }
            public string? Comentarios { get; set; }
            public string? CIE { get; set; }
        }

        #region Views

        /// <summary>
        /// Index: Lista de solicitudes de ausencia del empleado actual
        /// </summary>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var idEmpleadoActual = GetCurrentUserId();
                var (success, message, solicitudes) = await Task.FromResult(
                    _ausenciaService.ObtenerSolicitudesEmpleado(idEmpleadoActual)
                );

                ViewBag.CurrentUserId = idEmpleadoActual;

                if (!success)
                {
                    ModelState.AddModelError("", message);
                }

                return View(solicitudes ?? new List<AusenciaViewModel>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ausencias for employee");
                return View(new List<AusenciaViewModel>());
            }
        }

        /// <summary>
        /// Create GET: Formulario para crear nueva solicitud
        /// </summary>
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var idEmpleadoActual = GetCurrentUserId();
                var (success, tiposAusencia) = await Task.FromResult(
                    _ausenciaService.ObtenerTiposAusencia()
                );
                var (success2, aprobadores) = await Task.FromResult(
                    _ausenciaService.ObtenerAprobadores()
                );

                ViewBag.TiposAusencia = tiposAusencia ?? new List<TipoAusenciaViewModel>();
                ViewBag.Aprobadores = aprobadores ?? new List<AprobadorViewModel>();
                ViewBag.CurrentUserId = idEmpleadoActual;

                return View(new SolicitudAusenciaFormViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading create form");
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Create POST: Guardar nueva solicitud
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(SolicitudAusenciaFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var idEmpleadoActual = GetCurrentUserId();

                model.IdEmpleado = idEmpleadoActual;

                var (success, message, _) = await Task.FromResult(
                    _ausenciaService.CrearSolicitud(idEmpleadoActual, model)
                );

                if (success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", message);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating solicitud ausencia");
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        /// <summary>
        /// Details: Detalle de una solicitud
        /// </summary>
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(long id)
        {
            try
            {
                var (success, solicitud) = await Task.FromResult(
                    _ausenciaService.ObtenerPorId(id)
                );

                if (!success || solicitud == null)
                {
                    return NotFound();
                }

                return View(solicitud);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ausencia details");
                return NotFound();
            }
        }

        #endregion

        #region AJAX Endpoints

        /// <summary>
        /// GetSolicitudes: Obtener lista de solicitudes con filtros
        /// </summary>
        [HttpGet("GetSolicitudes")]
        public async Task<IActionResult> GetSolicitudes(long? idEmpleado = null, byte? estado = null)
        {
            try
            {
                if (idEmpleado.HasValue)
                {
                    var (ok, msg, data) = _ausenciaService.ObtenerSolicitudesEmpleado(idEmpleado.Value);
                    return Json(new { success = ok, message = msg, data });
                }

                var (success, message, pendientes) = _ausenciaService.ObtenerSolicitudesPendientes();
                return Json(new { success, message, data = pendientes });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching solicitudes");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        /// <summary>
        /// GetSolicitudesPendientes: Obtener solicitudes pendientes de aprobación
        /// </summary>
        [HttpGet("GetSolicitudesPendientes")]
        public async Task<IActionResult> GetSolicitudesPendientes()
        {
            try
            {
                var (success, message, solicitudes) = await Task.FromResult(
                    _ausenciaService.ObtenerSolicitudesPendientes()
                );

                return Json(new { success, message, data = solicitudes });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching solicitudes pendientes");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        /// <summary>
        /// AprobarSolicitud: Aprobar una solicitud
        /// </summary>
        [HttpPost("AprobarSolicitud")]
        public async Task<IActionResult> AprobarSolicitud([FromBody] AprobarSolicitudRequest req)
        {
            try
            {
                var (success, message) = await Task.FromResult(
                    _ausenciaService.AprobarSolicitud(req.idSolicitud, req.AprobadoPor, req.ObservacionesAprobacion)
                );

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving solicitud");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// RechazarSolicitud: Rechazar una solicitud
        /// </summary>
        [HttpPost("RechazarSolicitud")]
        public async Task<IActionResult> RechazarSolicitud([FromBody] RechazarSolicitudRequest req)
        {
            try
            {
                var (success, message) = await Task.FromResult(
                    _ausenciaService.RechazarSolicitud(req.idSolicitud, req.AprobadoPor, req.Motivo)
                );

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting solicitud");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// ObtenerBeneficiosPendientes: Obtener beneficios pendientes del empleado
        /// </summary>
        [HttpGet("ObtenerBeneficiosPendientes/{idEmpleado}")]
        public async Task<IActionResult> ObtenerBeneficiosPendientes(long idEmpleado)
        {
            try
            {
                var (success, beneficios) = await Task.FromResult(
                    _ausenciaService.ObtenerBeneficiosPendientes(idEmpleado)
                );

                return Json(new { success, data = beneficios });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching beneficios pendientes");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        /// <summary>
        /// CrearIncapacidad: Crear registro de incapacidad
        /// </summary>
        [HttpPost("CrearIncapacidad")]
        public async Task<IActionResult> CrearIncapacidad([FromBody] CrearIncapacidadRequest req)
        {
            try
            {
                var modelo = new IncapacidadViewModel
                {
                    EntidadConsulta = req.EntidadConsulta,
                    IPS = req.IPS,
                    RegistroMedico = req.RegistroMedico,
                    TipoIncapacidad = req.TipoIncapacidad,
                    ClaseAusencia = req.ClaseAusencia,
                    SOAT = req.SOAT,
                    FechaAccidenteTrabajo = req.FechaAccidenteTrabajo,
                    Comentarios = req.Comentarios,
                    CIE = req.CIE
                };

                var (success, message) = await Task.FromResult(
                    _ausenciaService.CrearIncapacidad(req.idSolicitudAusencia, modelo)
                );

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating incapacidad");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// ObtenerIncapacidad: Obtener datos de incapacidad
        /// </summary>
        [HttpGet("ObtenerIncapacidad/{idSolicitud}")]
        public async Task<IActionResult> ObtenerIncapacidad(long idSolicitud)
        {
            try
            {
                var (success, incapacidad) = await Task.FromResult(
                    _ausenciaService.ObtenerIncapacidad(idSolicitud)
                );

                return Json(new { success, data = incapacidad });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching incapacidad");
                return Json(new { success = false, message = ex.Message, data = new object() });
            }
        }

        /// <summary>
        /// ObtenerTiposAusencia: Catálogo de tipos de ausencia
        /// </summary>
        [HttpGet("ObtenerTiposAusencia")]
        public async Task<IActionResult> ObtenerTiposAusencia()
        {
            try
            {
                var (success, tipos) = await Task.FromResult(
                    _ausenciaService.ObtenerTiposAusencia()
                );

                return Json(new { success, data = tipos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tipos ausencia");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        #endregion
    }
}
