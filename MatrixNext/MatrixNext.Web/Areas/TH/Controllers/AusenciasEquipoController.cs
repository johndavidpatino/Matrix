using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatrixNext.Data.Modules.TH.Ausencias.Models;
using MatrixNext.Data.Modules.TH.Ausencias.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.TH.Controllers
{
    /// <summary>
    /// Controlador para visualizar Ausencias del Equipo
    /// Proporciona vistas de ausencias de subordinados y equipo
    /// </summary>
    [Area("TH")]
    [Route("TH/AusenciasEquipo")]
    [Authorize]
    public class AusenciasEquipoController : Controller
    {
        private readonly AusenciaService _ausenciaService;
        private readonly ILogger<AusenciasEquipoController> _logger;

        public AusenciasEquipoController(AusenciaService ausenciaService, ILogger<AusenciasEquipoController> logger)
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

        // Request DTOs
        public class AgregarSubordinadoRequest
        {
            public long idJefe { get; set; }
            public long idSubordinado { get; set; }
        }

        public class RemoverSubordinadoRequest
        {
            public long idJefe { get; set; }
            public long idSubordinado { get; set; }
        }

        #region Views

        /// <summary>
        /// Index: Vista de calendario/timeline de ausencias del equipo
        /// </summary>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var idJefeActual = GetCurrentUserId();

                var inicio = DateTime.Today.AddMonths(-1);
                var fin = DateTime.Today.AddMonths(1);

                // Obtener ausencias del equipo
                var (success, message, ausenciasEquipo) = await Task.FromResult(
                    _ausenciaService.ObtenerAusenciasEquipo(idJefeActual, inicio, fin)
                );

                // Obtener subordinados
                var (success2, subordinados) = await Task.FromResult(
                    _ausenciaService.ObtenerSubordinados(idJefeActual)
                );

                ViewBag.AusenciasEquipo = ausenciasEquipo ?? new List<AusenciaEquipoViewModel>();
                ViewBag.Subordinados = subordinados ?? new List<SubordinadoViewModel>();
                ViewBag.CurrentUserId = idJefeActual;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading ausencias equipo");
                return View();
            }
        }

        #endregion

        #region AJAX Endpoints

        /// <summary>
        /// ObtenerAusenciasEquipo: Obtener ausencias del equipo
        /// </summary>
        [HttpGet("ObtenerAusenciasEquipo/{idJefe}")]
        public async Task<IActionResult> ObtenerAusenciasEquipo(long idJefe)
        {
            try
            {
                var inicio = DateTime.Today.AddMonths(-1);
                var fin = DateTime.Today.AddMonths(1);

                var (success, message, ausencias) = await Task.FromResult(
                    _ausenciaService.ObtenerAusenciasEquipo(idJefe, inicio, fin)
                );

                return Json(new { success, message, data = ausencias });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ausencias equipo");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        /// <summary>
        /// ObtenerSubordinados: Obtener lista de subordinados
        /// </summary>
        [HttpGet("ObtenerSubordinados/{idJefe}")]
        public async Task<IActionResult> ObtenerSubordinados(long idJefe)
        {
            try
            {
                var (success, subordinados) = await Task.FromResult(
                    _ausenciaService.ObtenerSubordinados(idJefe)
                );

                return Json(new { success, data = subordinados });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching subordinados");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        /// <summary>
        /// ObtenerPersonasConAusencias: Obtener personas que tienen ausencias activas
        /// </summary>
        [HttpGet("ObtenerPersonasConAusencias")]
        public async Task<IActionResult> ObtenerPersonasConAusencias(long? idJefe = null, string search = "")
        {
            try
            {
                var jefeId = idJefe ?? GetCurrentUserId();
                var (success, personas) = await Task.FromResult(
                    _ausenciaService.ObtenerPersonasConAusencias(jefeId, search)
                );

                return Json(new { success, data = personas });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching personas con ausencias");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        /// <summary>
        /// AgregarSubordinado: Agregar un nuevo subordinado al equipo
        /// </summary>
        [HttpPost("AgregarSubordinado")]
        public async Task<IActionResult> AgregarSubordinado([FromBody] AgregarSubordinadoRequest req)
        {
            try
            {
                var (success, message) = await Task.FromResult(
                    _ausenciaService.AgregarSubordinado(req.idJefe, req.idSubordinado)
                );

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding subordinado");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// RemoverSubordinado: Remover un subordinado del equipo
        /// </summary>
        [HttpPost("RemoverSubordinado")]
        public async Task<IActionResult> RemoverSubordinado([FromBody] RemoverSubordinadoRequest req)
        {
            try
            {
                var (success, message) = await Task.FromResult(
                    _ausenciaService.RemoverSubordinado(req.idSubordinado)
                );

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing subordinado");
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

        #endregion
    }
}
