using MatrixNext.Web.Models.OP;
using MatrixNext.Web.Services;
using MatrixNext.Web.Services.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.OP.Controllers
{
    /// <summary>
    /// Controlador para gestión de muestra por ciudad en trabajos OP
    /// </summary>
    /// <remarks>
    /// Migrado desde WebMatrix/OP_Cuantitativo/MuestraTrabajos.aspx.vb
    /// Funcionalidades:
    /// - Listado de muestra por ciudad
    /// - Agregar/editar/eliminar muestra
    /// - Actualización de fechas con auto-planeación
    /// - Cálculo de total de muestra
    /// Permiso: 100 (COE)
    /// </remarks>
    [Area("OP")]
    [Authorize]
    public class MuestraTrabajosController : Controller
    {
        private readonly IOpMuestraService _muestraService;
        private readonly IOpTrabajosService _trabajosService;
        private readonly IEmailService _emailService;
        private readonly ILogger<MuestraTrabajosController> _logger;

        public MuestraTrabajosController(
            IOpMuestraService muestraService,
            IOpTrabajosService trabajosService,
            IEmailService emailService,
            ILogger<MuestraTrabajosController> logger)
        {
            _muestraService = muestraService;
            _trabajosService = trabajosService;
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// Página principal: listado de muestra por ciudad
        /// GET: /OP/MuestraTrabajos/Index?trabajoId=123
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
                var trabajoConfig = await _trabajosService.ObtenerConfiguracionAsync(trabajoId);
                if (trabajoConfig == null)
                {
                    _logger.LogWarning("Trabajo {TrabajoId} no encontrado", trabajoId);
                }

                ViewBag.TrabajoId = trabajoId;
                ViewBag.TrabajoNombre = $"Trabajo {trabajoId}"; // TODO: Obtener nombre real

                // Cargar muestra existente
                var muestras = await _muestraService.ObtenerMuestraPorTrabajoAsync(trabajoId);
                var totalMuestra = await _muestraService.CalcularTotalMuestraAsync(trabajoId);

                ViewBag.TotalMuestra = totalMuestra;

                return View(muestras);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar muestra del trabajo {TrabajoId}", trabajoId);
                TempData["ErrorMessage"] = "Error al cargar la muestra";
                return RedirectToAction("Index", "Trabajos");
            }
        }

        /// <summary>
        /// Agregar nueva muestra de ciudad
        /// POST: /OP/MuestraTrabajos/Agregar
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar([FromForm] MuestraCiudadVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Datos de muestra inválidos";
                return RedirectToAction("Index", new { trabajoId = model.TrabajoId });
            }

            try
            {
                var id = await _muestraService.GuardarMuestraAsync(model);

                TempData["SuccessMessage"] = "Muestra agregada correctamente";

                // TODO: Enviar email a coordinadores
                // await EnviarEmailCoordinadoresAsync(model.TrabajoId);

                return RedirectToAction("Index", new { trabajoId = model.TrabajoId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar muestra para trabajo {TrabajoId}", model.TrabajoId);
                TempData["ErrorMessage"] = "Error al agregar la muestra";
                return RedirectToAction("Index", new { trabajoId = model.TrabajoId });
            }
        }

        /// <summary>
        /// Actualizar muestra existente
        /// POST: /OP/MuestraTrabajos/Actualizar
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Actualizar([FromForm] MuestraCiudadVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Datos de muestra inválidos";
                return RedirectToAction("Index", new { trabajoId = model.TrabajoId });
            }

            try
            {
                await _muestraService.GuardarMuestraAsync(model);

                TempData["SuccessMessage"] = "Muestra actualizada correctamente";
                return RedirectToAction("Index", new { trabajoId = model.TrabajoId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar muestra {Id}", model.Id);
                TempData["ErrorMessage"] = "Error al actualizar la muestra";
                return RedirectToAction("Index", new { trabajoId = model.TrabajoId });
            }
        }

        /// <summary>
        /// Eliminar muestra de ciudad
        /// POST: /OP/MuestraTrabajos/Eliminar
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(long id, long trabajoId)
        {
            try
            {
                var eliminado = await _muestraService.EliminarMuestraAsync(id);

                if (eliminado)
                {
                    TempData["SuccessMessage"] = "Muestra eliminada correctamente";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar la muestra";
                }

                return RedirectToAction("Index", new { trabajoId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar muestra {Id}", id);
                TempData["ErrorMessage"] = "Error al eliminar la muestra";
                return RedirectToAction("Index", new { trabajoId });
            }
        }

        /// <summary>
        /// Actualizar fechas con auto-planeación
        /// POST: /OP/MuestraTrabajos/ActualizarFechas
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarFechas([FromForm] ActualizarFechasMuestraVM model, long trabajoId)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Datos de fechas inválidos";
                return RedirectToAction("Index", new { trabajoId });
            }

            try
            {
                var actualizado = await _muestraService.ActualizarFechasConPlaneacionAsync(model);

                if (actualizado)
                {
                    TempData["SuccessMessage"] = "Fechas y planeación actualizadas correctamente";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al actualizar las fechas";
                }

                return RedirectToAction("Index", new { trabajoId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar fechas de muestra {IdMuestra}", model.IdMuestra);
                TempData["ErrorMessage"] = "Error al actualizar las fechas";
                return RedirectToAction("Index", new { trabajoId });
            }
        }

        /// <summary>
        /// Obtener datos de muestra para edición (AJAX)
        /// GET: /OP/MuestraTrabajos/ObtenerMuestra?id=123
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerMuestra(long id)
        {
            try
            {
                var muestra = await _muestraService.ObtenerMuestraPorIdAsync(id);
                if (muestra == null)
                    return NotFound(new { error = "Muestra no encontrada" });

                return Json(muestra);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener muestra {Id}", id);
                return StatusCode(500, new { error = "Error al obtener la muestra" });
            }
        }

        #region Helpers

        /// <summary>
        /// Enviar email de notificación a coordinadores
        /// </summary>
        /// <remarks>
        /// TODO: Implementar consulta de coordinadores del trabajo
        /// Ref: EnviarEmailCoordinadores() del legado
        /// </remarks>
        private async Task EnviarEmailCoordinadoresAsync(long trabajoId)
        {
            try
            {
                // TODO: Implementar lógica real de obtención de emails de coordinadores
                // var coordinadores = await ObtenerCoordinadoresEmailAsync(trabajoId);
                // if (coordinadores.Any())
                // {
                //     var asunto = $"Actualización de Muestra - Trabajo {trabajoId}";
                //     var cuerpo = GenerarCuerpoEmailMuestra(trabajoId);
                //     await _emailService.EnviarMultipleAsync(coordinadores, asunto, cuerpo);
                // }

                _logger.LogInformation("Email de coordinadores pendiente de implementar para trabajo {TrabajoId}", trabajoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar email a coordinadores del trabajo {TrabajoId}", trabajoId);
                // No lanzar excepción para no bloquear el guardado
            }
        }

        #endregion
    }
}
