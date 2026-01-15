using MatrixNext.Data.Models.OP;
using MatrixNext.Data.Services.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MatrixNext.Web.Areas.OP.Controllers
{
    /// <summary>
    /// Controller para activación de encuestas
    /// Migrado desde WebMatrix/OP_Cuantitativo/ActivacionEncuestas.aspx
    /// </summary>
    [Area("OP")]
    [Authorize]
    public class ActivacionEncuestasController : Controller
    {
        private readonly IEncuestasService _service;
        private readonly ILogger<ActivacionEncuestasController> _logger;

        public ActivacionEncuestasController(IEncuestasService service, ILogger<ActivacionEncuestasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Vista principal de activación de encuestas
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(long? trabajoId)
        {
            try
            {
                if (!trabajoId.HasValue)
                {
                    return View(new EncuestaAnuladaDto[] { });
                }

                var encuestasAnuladas = await _service.ObtenerEncuestasAnuladasAsync(trabajoId.Value);
                ViewBag.TrabajoId = trabajoId.Value;
                
                return View(encuestasAnuladas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando vista de activación de encuestas. Trabajo: {TrabajoId}", trabajoId);
                TempData["Error"] = "Error al cargar las encuestas anuladas";
                return View(new EncuestaAnuladaDto[] { });
            }
        }

        /// <summary>
        /// Activa (elimina anulación) de una encuesta
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activar(ActivacionEncuestaDto dto)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }
                
                TempData["Error"] = "Datos inválidos";
                return RedirectToAction(nameof(Index), new { trabajoId = dto.TrabajoId });
            }

            try
            {
                var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                
                var (success, message) = await _service.ActivarEncuestaAsync(
                    dto.TrabajoId, 
                    dto.NumeroEncuesta, 
                    dto.Observacion, 
                    usuarioId
                );

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success, message });
                }

                if (success)
                {
                    TempData["Success"] = message;
                }
                else
                {
                    TempData["Error"] = message;
                }

                return RedirectToAction(nameof(Index), new { trabajoId = dto.TrabajoId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activando encuesta. Trabajo: {TrabajoId}, Encuesta: {NumeroEncuesta}", 
                    dto.TrabajoId, dto.NumeroEncuesta);

                var errorMessage = "Error al activar la encuesta. Por favor intente nuevamente.";

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = errorMessage });
                }

                TempData["Error"] = errorMessage;
                return RedirectToAction(nameof(Index), new { trabajoId = dto.TrabajoId });
            }
        }
    }
}
