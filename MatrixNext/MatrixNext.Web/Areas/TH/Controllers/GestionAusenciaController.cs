using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatrixNext.Data.Modules.TH.Ausencias.Models;
using MatrixNext.Data.Modules.TH.Ausencias.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.TH.Controllers
{
    /// <summary>
    /// Controlador para Gestión de Ausencias - Panel RRHH
    /// Proporciona operaciones de administración, reportes y análisis de ausencias
    /// </summary>
    [Area("TH")]
    [Route("TH/GestionAusencia")]
    [Authorize]
    public class GestionAusenciaController : Controller
    {
        private readonly AusenciaService _ausenciaService;
        private readonly ILogger<GestionAusenciaController> _logger;

        public GestionAusenciaController(AusenciaService ausenciaService, ILogger<GestionAusenciaController> logger)
        {
            _ausenciaService = ausenciaService ?? throw new ArgumentNullException(nameof(ausenciaService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Request DTOs
        public class FiltroReporteRequest
        {
            public DateTime? FechaInicio { get; set; }
            public DateTime? FechaFin { get; set; }
            public long? idEmpleado { get; set; }
            public byte? Tipo { get; set; }
            public int? Anno { get; set; }
            public int? Mes { get; set; }
        }

        #region Views

        /// <summary>
        /// Index: Panel principal de gestión de ausencias
        /// </summary>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Obtener datos iniciales
                var (success, message, solicitudesPendientes) = await Task.FromResult(
                    _ausenciaService.ObtenerSolicitudesPendientes()
                );

                var (success2, tipos) = await Task.FromResult(
                    _ausenciaService.ObtenerTiposAusencia()
                );

                ViewBag.SolicitudesPendientes = solicitudesPendientes ?? new List<ReporteSolicitudesPendientesViewModel>();
                ViewBag.TiposAusencia = tipos ?? new List<TipoAusenciaViewModel>();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading gestión ausencia index");
                return View();
            }
        }

        #endregion

        #region AJAX Endpoints

        /// <summary>
        /// GetSolicitudesPorAprobar: Obtener solicitudes pendientes de aprobación
        /// </summary>
        [HttpGet("GetSolicitudesPorAprobar")]
        public async Task<IActionResult> GetSolicitudesPorAprobar()
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
                _logger.LogError(ex, "Error fetching solicitudes por aprobar");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        /// <summary>
        /// AprobarSolicitud: Aprobar solicitud con visto bueno RRHH
        /// </summary>
        [HttpPost("AprobarSolicitud")]
        public async Task<IActionResult> AprobarSolicitud([FromBody] dynamic req)
        {
            try
            {
                long idSolicitud = req.idSolicitud;
                long idAprobador = req.idAprobador;
                string observaciones = req.observaciones;

                var (success, message) = await Task.FromResult(
                    _ausenciaService.AprobarSolicitud(idSolicitud, idAprobador, observaciones)
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
        /// RechazarSolicitud: Rechazar solicitud
        /// </summary>
        [HttpPost("RechazarSolicitud")]
        public async Task<IActionResult> RechazarSolicitud([FromBody] dynamic req)
        {
            try
            {
                long idSolicitud = req.idSolicitud;
                long idAprobador = req.idAprobador ?? 0;
                string motivo = req.motivo;

                var (success, message) = await Task.FromResult(
                    _ausenciaService.RechazarSolicitud(idSolicitud, idAprobador, motivo)
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
        /// ReporteVacaciones: Generar reporte de vacaciones
        /// </summary>
        [HttpPost("ReporteVacaciones")]
        public async Task<IActionResult> ReporteVacaciones([FromBody] FiltroReporteRequest filtro)
        {
            try
            {
                var (success, data) = await Task.FromResult(
                    _ausenciaService.ReporteVacaciones()
                );

                var reporte = data ?? new List<ReporteVacacionesViewModel>();

                if (filtro.FechaInicio.HasValue || filtro.FechaFin.HasValue)
                {
                    var fechaInicio = filtro.FechaInicio ?? DateTime.MinValue;
                    var fechaFin = filtro.FechaFin ?? DateTime.MaxValue;
                    reporte = reporte.Where(r => (!r.FechaIngreso.HasValue || (r.FechaIngreso.Value >= fechaInicio && r.FechaIngreso.Value <= fechaFin))).ToList();
                }

                return Json(new { success, message = success ? "Reporte generado" : "No fue posible generar el reporte", data = reporte });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating reporte vacaciones");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        /// <summary>
        /// ReporteAusentismo: Generar reporte de ausentismo
        /// </summary>
        [HttpPost("ReporteAusentismo")]
        public async Task<IActionResult> ReporteAusentismo([FromBody] FiltroReporteRequest filtro)
        {
            try
            {
                var anno = filtro.Anno ?? filtro.FechaInicio?.Year ?? DateTime.Now.Year;

                var (success, data) = await Task.FromResult(
                    _ausenciaService.ReporteAusentismo(anno)
                );

                var reporte = data ?? new List<ReporteAusentismoViewModel>();

                if (filtro.idEmpleado.HasValue)
                {
                    reporte = reporte.Where(r => r.Identificacion == filtro.idEmpleado.Value).ToList();
                }

                if (filtro.FechaInicio.HasValue || filtro.FechaFin.HasValue)
                {
                    var fechaInicio = filtro.FechaInicio ?? DateTime.MinValue;
                    var fechaFin = filtro.FechaFin ?? DateTime.MaxValue;
                    reporte = reporte.Where(r => (!r.FechaInicio.HasValue || r.FechaInicio.Value >= fechaInicio) && (!r.FechaFin.HasValue || r.FechaFin.Value <= fechaFin)).ToList();
                }

                return Json(new { success, message = success ? "Reporte generado" : "No fue posible generar el reporte", data = reporte });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating reporte ausentismo");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        /// <summary>
        /// ReporteIncapacidades: Generar reporte de incapacidades
        /// </summary>
        [HttpPost("ReporteIncapacidades")]
        public async Task<IActionResult> ReporteIncapacidades([FromBody] FiltroReporteRequest filtro)
        {
            try
            {
                var anno = filtro.Anno ?? filtro.FechaInicio?.Year ?? DateTime.Now.Year;

                var (success, data) = await Task.FromResult(
                    _ausenciaService.ReporteIncapacidades(anno)
                );

                var reporte = data ?? new List<ReporteIncapacidadesViewModel>();

                if (filtro.idEmpleado.HasValue)
                {
                    reporte = reporte.Where(r => r.Identificacion == filtro.idEmpleado.Value).ToList();
                }

                if (filtro.FechaInicio.HasValue || filtro.FechaFin.HasValue)
                {
                    var fechaInicio = filtro.FechaInicio ?? DateTime.MinValue;
                    var fechaFin = filtro.FechaFin ?? DateTime.MaxValue;
                    reporte = reporte.Where(r => (!r.FechaInicio.HasValue || r.FechaInicio.Value >= fechaInicio) && (!r.FechaFin.HasValue || r.FechaFin.Value <= fechaFin)).ToList();
                }

                return Json(new { success, message = success ? "Reporte generado" : "No fue posible generar el reporte", data = reporte });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating reporte incapacidades");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        /// <summary>
        /// ReporteBeneficios: Generar reporte de beneficios pendientes
        /// </summary>
        [HttpPost("ReporteBeneficios")]
        public async Task<IActionResult> ReporteBeneficios([FromBody] FiltroReporteRequest filtro)
        {
            try
            {
                var anno = filtro.Anno ?? DateTime.Now.Year;

                var (success, beneficios) = await Task.FromResult(
                    _ausenciaService.ReporteBeneficios(anno)
                );

                var data = beneficios ?? new List<ReporteBeneficiosViewModel>();

                if (filtro.idEmpleado.HasValue)
                {
                    data = data.Where(b => b.Identificacion == filtro.idEmpleado.Value).ToList();
                }

                return Json(new { success, message = success ? "Reporte generado" : "No fue posible generar el reporte", data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating reporte beneficios");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        #endregion
    }
}
