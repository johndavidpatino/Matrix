using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MatrixNext.Data.Modules.US.Feedback.Models;
using MatrixNext.Data.Modules.US.Feedback.Services;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.US.Controllers
{
    /// <summary>
    /// Controlador para Seguimiento de Feedback (administradores)
    /// Ref: WebMatrix/US_Usuarios/SeguimientoFeedback.aspx
    /// SP: CORE_Retroalimentacion (consultas/updates)
    /// </summary>
    [Area("US")]
    [Route("Usuarios/SeguimientoFeedback")]
    [Authorize]
    public class SeguimientoFeedbackController : Controller
    {
        private readonly IFeedbackService _service;
        private readonly ILogger<SeguimientoFeedbackController> _logger;

        public SeguimientoFeedbackController(IFeedbackService service, ILogger<SeguimientoFeedbackController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Lista de feedback pendiente
        /// </summary>
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var pendientes = await _service.ObtenerPendientesAsync();
            return View(pendientes);
        }

        /// <summary>
        /// Lista de feedback resuelto
        /// </summary>
        [HttpGet("Resueltos")]
        public async Task<IActionResult> Resueltos()
        {
            var resueltos = await _service.ObtenerResueltosAsync();
            return View(resueltos);
        }

        /// <summary>
        /// Obtiene lista parcial para AJAX
        /// </summary>
        [HttpGet("ListaPendientes")]
        public async Task<IActionResult> ListaPendientes()
        {
            var pendientes = await _service.ObtenerPendientesAsync();
            return PartialView("_ListaFeedback", pendientes);
        }

        /// <summary>
        /// Obtiene lista de resueltos parcial para AJAX
        /// </summary>
        [HttpGet("ListaResueltos")]
        public async Task<IActionResult> ListaResueltos()
        {
            var resueltos = await _service.ObtenerResueltosAsync();
            return PartialView("_ListaFeedback", resueltos);
        }

        /// <summary>
        /// Modal para ver detalle y responder
        /// </summary>
        [HttpGet("Detalle/{id}")]
        public async Task<IActionResult> Detalle(long id)
        {
            var feedback = await _service.ObtenerPorIdAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            return PartialView("_DetalleModal", feedback);
        }

        /// <summary>
        /// Responde al feedback
        /// </summary>
        [HttpPost("Responder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Responder([FromForm] FeedbackUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            var userId = GetUserId();
            var (success, message) = await _service.ResponderFeedbackAsync(dto, userId);

            return Json(new { success, message });
        }

        private long GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                           ?? User.FindFirst("UserId")?.Value
                           ?? User.FindFirst("sub")?.Value;
            
            if (long.TryParse(userIdClaim, out var userId))
                return userId;
            
            return 0;
        }
    }
}
