using MatrixNext.Web.Models.OP;
using MatrixNext.Web.Services.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.OP.Controllers
{
    /// <summary>
    /// Controlador para gestión de estimación de producción por ciudad
    /// </summary>
    /// <remarks>
    /// Migrado desde WebMatrix/OP_Cuantitativo/EstimacionProduccion.aspx.vb
    /// Funcionalidades:
    /// - Listado de estimaciones por trabajo
    /// - Creación de estimación con planeación automática
    /// - Edición de cantidades diarias
    /// - Activación de estimación
    /// - Validación contra muestra
    /// Permiso: 100 (COE)
    /// </remarks>
    [Area("OP")]
    [Authorize]
    public class EstimacionProduccionController : Controller
    {
        private readonly IOpEstimacionService _estimacionService;
        private readonly IOpTrabajosService _opTrabajosService;
        private readonly ILogger<EstimacionProduccionController> _logger;

        public EstimacionProduccionController(
            IOpEstimacionService estimacionService,
            IOpTrabajosService opTrabajosService,
            ILogger<EstimacionProduccionController> logger)
        {
            _estimacionService = estimacionService;
            _opTrabajosService = opTrabajosService;
            _logger = logger;
        }

        /// <summary>
        /// Página principal: listado de estimaciones de un trabajo
        /// GET: /OP/EstimacionProduccion/Index?trabajoId=123
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(long trabajoId)
        {
            if (trabajoId <= 0)
            {
                TempData["ErrorMessage"] = "ID de trabajo inválido";
                return RedirectToAction("Index", "Trabajos");
            }

            try
            {
                // Verificar que el trabajo exista
                var trabajoConfig = await _opTrabajosService.ObtenerConfiguracionAsync(trabajoId);
                if (trabajoConfig == null)
                {
                    _logger.LogWarning("Trabajo {TrabajoId} no encontrado o sin configuración", trabajoId);
                }

                ViewBag.TrabajoId = trabajoId;
                ViewBag.TrabajoNombre = $"Trabajo {trabajoId}"; // TODO: Obtener nombre real de PY_Trabajo
                ViewBag.JobBook = ""; // TODO: Obtener JobBook real de PY_Trabajo

                // Cargar estimaciones existentes
                var estimaciones = await _estimacionService.ObtenerEstimacionesPorTrabajoAsync(trabajoId);

                return View(estimaciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar estimaciones del trabajo {TrabajoId}", trabajoId);
                TempData["ErrorMessage"] = "Error al cargar las estimaciones";
                return RedirectToAction("Index", "Trabajos");
            }
        }

        /// <summary>
        /// Obtener detalle de una estimación con planeación diaria (AJAX)
        /// GET: /OP/EstimacionProduccion/Detalle?id=456
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Detalle(long id)
        {
            try
            {
                var detalle = await _estimacionService.ObtenerEstimacionDetalleAsync(id);
                if (detalle == null)
                    return NotFound(new { error = "Estimación no encontrada" });

                return PartialView("_DetalleEstimacion", detalle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalle de estimación {EstimacionId}", id);
                return StatusCode(500, new { error = "Error al cargar el detalle" });
            }
        }

        /// <summary>
        /// Crear nueva estimación con planeación automática
        /// POST: /OP/EstimacionProduccion/Crear
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([FromForm] CrearEstimacionVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Datos de estimación inválidos";
                return RedirectToAction("Index", new { trabajoId = model.TrabajoId });
            }

            try
            {
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                {
                    TempData["ErrorMessage"] = "Usuario no identificado";
                    return RedirectToAction("Index", "Trabajos");
                }

                var estimacionId = await _estimacionService.CrearEstimacionAsync(model, userId.Value);

                TempData["SuccessMessage"] = "Estimación creada correctamente con planeación automática";
                return RedirectToAction("Index", new { trabajoId = model.TrabajoId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear estimación para trabajo {TrabajoId}", model.TrabajoId);
                TempData["ErrorMessage"] = "Error al crear la estimación";
                return RedirectToAction("Index", new { trabajoId = model.TrabajoId });
            }
        }

        /// <summary>
        /// Actualizar cantidades de planeación (AJAX batch)
        /// POST: /OP/EstimacionProduccion/ActualizarCantidades
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ActualizarCantidades([FromBody] List<PlaneacionDiaVM> actualizaciones)
        {
            if (actualizaciones == null || !actualizaciones.Any())
                return BadRequest(new { error = "No se enviaron actualizaciones" });

            try
            {
                await _estimacionService.ActualizarCantidadesBatchAsync(actualizaciones);
                return Json(new { success = true, message = "Cantidades actualizadas correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cantidades en batch");
                return StatusCode(500, new { error = "Error al actualizar las cantidades" });
            }
        }

        /// <summary>
        /// Validar estimación contra muestra (AJAX)
        /// GET: /OP/EstimacionProduccion/Validar?id=456
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Validar(long id)
        {
            try
            {
                var (esValido, sumaEstimada, muestraEsperada) = 
                    await _estimacionService.ValidarEstimacionVsMuestraAsync(id);

                return Json(new
                {
                    esValido,
                    sumaEstimada,
                    muestraEsperada,
                    mensaje = esValido
                        ? "La estimación coincide con la muestra"
                        : $"La muestra es de {muestraEsperada} y la planeación es {sumaEstimada}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar estimación {EstimacionId}", id);
                return StatusCode(500, new { error = "Error al validar la estimación" });
            }
        }

        /// <summary>
        /// Activar una estimación (desactiva otras de la misma ciudad)
        /// POST: /OP/EstimacionProduccion/Activar
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activar(long id, long trabajoId)
        {
            try
            {
                // Primero validar contra muestra
                var (esValido, sumaEstimada, muestraEsperada) = 
                    await _estimacionService.ValidarEstimacionVsMuestraAsync(id);

                if (!esValido)
                {
                    TempData["ErrorMessage"] = $"La muestra y la planeación no coinciden. La muestra es de {muestraEsperada} y la planeación es {sumaEstimada}";
                    return RedirectToAction("Index", new { trabajoId });
                }

                var activado = await _estimacionService.ActivarEstimacionAsync(id);

                if (activado)
                {
                    TempData["SuccessMessage"] = "Estimación activada correctamente";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al activar la estimación";
                }

                return RedirectToAction("Index", new { trabajoId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar estimación {EstimacionId}", id);
                TempData["ErrorMessage"] = "Error al activar la estimación";
                return RedirectToAction("Index", new { trabajoId });
            }
        }

        #region Helpers

        private long? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            return null;
        }

        #endregion
    }
}
