using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using MatrixNext.Data.Models.TH;
using MatrixNext.Data.Services.TH;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.TH.Controllers
{
    /// <summary>
    /// Controlador para gestión de desvinculaciones de empleados
    /// Equivalente a DesvinculacionesEmpleadosGestionRRHH.aspx en WebMatrix
    /// Área: TH, Ruta base: /TH/Desvinculaciones
    /// </summary>
    [Area("TH")]
    [Route("TH/[controller]")]
    [Authorize] // Permiso 154 en legacy
    public class DesvinculacionesController : Controller
    {
        private readonly DesvinculacionService _service;
        private readonly ILogger<DesvinculacionesController> _logger;
        private readonly IConfiguration _configuration;

        public DesvinculacionesController(
            DesvinculacionService service,
            ILogger<DesvinculacionesController> logger,
            IConfiguration configuration)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Helper para obtener ID del usuario actual desde Claims
        /// </summary>
        private long GetCurrentUserId()
        {
            var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User?.FindFirst("sub")?.Value
                          ?? User?.FindFirst("userId")?.Value;

            if (long.TryParse(idClaim, out long userId))
            {
                return userId;
            }

            throw new InvalidOperationException("ID de usuario autenticado no disponible");
        }

        #region Vistas

        /// <summary>
        /// GET: /TH/Desvinculaciones
        /// Vista principal de gestión de desvinculaciones
        /// </summary>
        [HttpGet("")]
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = "Gestión de Desvinculaciones RRHH";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar vista Index de desvinculaciones");
                return View("Error");
            }
        }

        #endregion

        #region API - Consulta de Desvinculaciones

        /// <summary>
        /// POST: /TH/Desvinculaciones/Buscar
        /// Busca procesos de desvinculación con paginación
        /// Equivalente a WebMethod DesvinculacionesEmpleadosEstatus
        /// </summary>
        [HttpPost("Buscar")]
        public async Task<IActionResult> BuscarDesvinculaciones([FromBody] DesvinculacionFiltroDTO filtro)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Filtros inválidos" });
                }

                var (success, message, data) = await _service.ObtenerDesvinculacionesPaginadas(filtro);

                if (!success)
                {
                    return Json(new { success = false, message });
                }

                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar desvinculaciones");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        #endregion

        #region API - Empleados Activos

        /// <summary>
        /// GET: /TH/Desvinculaciones/EmpleadosActivos
        /// Obtiene empleados activos disponibles para desvinculación
        /// Equivalente a WebMethod EmpleadosActivos
        /// </summary>
        [HttpGet("EmpleadosActivos")]
        public async Task<IActionResult> ObtenerEmpleadosActivos()
        {
            try
            {
                var (success, message, data) = await _service.ObtenerEmpleadosActivos();

                if (!success)
                {
                    return Json(new { success = false, message });
                }

                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empleados activos");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        #endregion

        #region API - Iniciar Proceso

        /// <summary>
        /// POST: /TH/Desvinculaciones/Iniciar
        /// Inicia proceso de desvinculación de empleado
        /// Equivalente a WebMethod IniciarProcesoDesvinculacion
        /// </summary>
        [HttpPost("Iniciar")]
        public async Task<IActionResult> IniciarProcesoDesvinculacion([FromBody] IniciarDesvinculacionDTO datos)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var usuarioId = GetCurrentUserId();
                var (success, message, desvinculacionId) = await _service.IniciarProcesoDesvinculacion(datos, usuarioId);

                if (!success)
                {
                    return Json(new { success = false, message });
                }

                return Json(new { success = true, message, desvinculacionId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar proceso de desvinculación");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        #endregion

        #region API - Gestión Área (legacy DesvinculacionesEmpleadosGestionArea.aspx)

        [HttpGet("Pendientes/Area/{areaId:int}")]
        public async Task<IActionResult> ProcesosPendientesPorArea(int areaId)
        {
            try
            {
                var (success, message, data) = await _service.ProcesosPendientesPorArea(areaId);
                return Json(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar pendientes por área");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        [HttpGet("Pendientes/UsuarioActual")]
        public async Task<IActionResult> ProcesosPendientesPorEvaluarUsuarioActual()
        {
            try
            {
                var usuarioId = GetCurrentUserId();
                var (success, message, data) = await _service.ProcesosPendientesPorEvaluarUsuarioActual(usuarioId);
                return Json(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar pendientes del usuario actual");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        [HttpGet("ItemsVerificar/Area/{areaId:int}")]
        public async Task<IActionResult> ItemsVerificarPorArea(int areaId)
        {
            try
            {
                var (success, message, data) = await _service.ItemsVerificarPorArea(areaId);
                return Json(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar items por área");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        [HttpGet("EmpleadoInfo/{desvinculacionId:int}")]
        public async Task<IActionResult> InformacionEmpleadoPor(int desvinculacionId)
        {
            try
            {
                var (success, message, data) = await _service.InformacionEmpleadoPor(desvinculacionId);
                return Json(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar información empleado");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        [HttpPost("GuardarEvaluacion")]
        public async Task<IActionResult> GuardarEvaluacion([FromBody] GuardarEvaluacionRequestDTO request)
        {
            try
            {
                var usuarioId = GetCurrentUserId();
                var (success, message) = await _service.GuardarEvaluacion(request, usuarioId);
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar evaluación");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        [HttpGet("EvaluacionesRealizadas/UsuarioActual")]
        public async Task<IActionResult> EvaluacionesRealizadasUsuarioActual()
        {
            try
            {
                var usuarioId = GetCurrentUserId();
                var (success, message, data) = await _service.EvaluacionesRealizadasPorUsuarioActual(usuarioId);
                return Json(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar evaluaciones realizadas");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        #endregion

        #region API - Evaluaciones

        /// <summary>
        /// GET: /TH/Desvinculaciones/{desvinculacionId}/Evaluaciones
        /// Obtiene evaluaciones de áreas para una desvinculación
        /// Equivalente a WebMethod DesvinculacionEmpleadosEstatusEvaluacionesPor
        /// </summary>
        [HttpGet("{desvinculacionId}/Evaluaciones")]
        public async Task<IActionResult> ObtenerEvaluaciones(int desvinculacionId)
        {
            try
            {
                var (success, message, data) = await _service.ObtenerEvaluacionesPorDesvinculacion(desvinculacionId);

                if (!success)
                {
                    return Json(new { success = false, message });
                }

                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener evaluaciones de desvinculación {desvinculacionId}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        #endregion

        #region API - Generación de PDF

        /// <summary>
        /// GET: /TH/Desvinculaciones/{desvinculacionId}/PDF
        /// Genera PDF de formato de desvinculación
        /// Equivalente a WebMethod PDFFormato
        /// </summary>
        [HttpGet("{desvinculacionId}/PDF")]
        public async Task<IActionResult> GenerarPDF(int desvinculacionId)
        {
            try
            {
                // Obtener ruta de plantilla desde configuración
                var templatePath = _configuration["DesvinculacionPDF:TemplatePath"] 
                    ?? Path.Combine(Directory.GetCurrentDirectory(), "Resources", "TH_DesvinculacionEmpleados", "TemplateFormatoDesvinculacion.html");

                var (success, message, pdfBase64) = await _service.GenerarPDFFormato(desvinculacionId, templatePath);

                if (!success)
                {
                    return Json(new { success = false, message });
                }

                return Json(new { success = true, pdfBase64 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al generar PDF de desvinculación {desvinculacionId}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        #endregion
    }
}
