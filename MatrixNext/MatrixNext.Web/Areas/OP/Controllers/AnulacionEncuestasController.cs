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
    /// Controller para anulación de encuestas
    /// Migrado desde WebMatrix/OP_Cuantitativo/AnulacionEncuestas.aspx
    /// </summary>
    [Area("OP")]
    [Authorize]
    public class AnulacionEncuestasController : Controller
    {
        private readonly IEncuestasService _service;
        private readonly ILogger<AnulacionEncuestasController> _logger;

        public AnulacionEncuestasController(IEncuestasService service, ILogger<AnulacionEncuestasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Vista principal de anulación de encuestas
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
                _logger.LogError(ex, "Error cargando vista de anulación de encuestas. Trabajo: {TrabajoId}", trabajoId);
                TempData["Error"] = "Error al cargar las encuestas anuladas";
                return View(new EncuestaAnuladaDto[] { });
            }
        }

        /// <summary>
        /// Muestra modal para crear nueva anulación
        /// </summary>
        [HttpGet]
        public IActionResult Create(long trabajoId)
        {
            var dto = new EncuestaAnuladaDto
            {
                TrabajoId = trabajoId,
                Fecha = DateTime.Now
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreateEdit", dto);
            }

            return View(dto);
        }

        /// <summary>
        /// Anula una encuesta
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EncuestaAnuladaDto dto)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_CreateEdit", dto);
                }
                
                return View(dto);
            }

            try
            {
                var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                
                // Obtener unidad del usuario (puede venir de claims o de configuración)
                // Por ahora usamos un valor por defecto, ajustar según implementación real
                var unidadId = long.Parse(User.FindFirstValue("UnidadId") ?? "1");
                
                var (success, message, id) = await _service.AnularEncuestaAsync(dto, usuarioId, unidadId);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success, message });
                }

                if (success)
                {
                    TempData["Success"] = message;
                    return RedirectToAction(nameof(Index), new { trabajoId = dto.TrabajoId });
                }

                ModelState.AddModelError("", message);
                return View(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error anulando encuesta. Trabajo: {TrabajoId}, Encuesta: {NumeroEncuesta}", 
                    dto.TrabajoId, dto.NumeroEncuesta);

                var errorMessage = "Error al anular la encuesta. Por favor intente nuevamente.";

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = errorMessage });
                }

                ModelState.AddModelError("", errorMessage);
                return View(dto);
            }
        }
    }
}
