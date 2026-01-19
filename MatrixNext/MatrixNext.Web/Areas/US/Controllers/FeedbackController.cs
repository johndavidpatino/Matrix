using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MatrixNext.Data.Modules.US.Feedback.Models;
using MatrixNext.Data.Modules.US.Feedback.Services;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.US.Controllers
{
    /// <summary>
    /// Controlador para Feedback
    /// Ref: WebMatrix/US_Usuarios/Feedback.aspx - Permiso 30
    /// SP: CORE_Asunto_Get, CORE_Feedback_Add
    /// </summary>
    [Area("US")]
    [Route("Usuarios/Feedback")]
    [Authorize]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _service;
        private readonly ILogger<FeedbackController> _logger;

        public FeedbackController(IFeedbackService service, ILogger<FeedbackController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Vista para enviar feedback
        /// </summary>
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            await CargarAsuntos();
            return View(new FeedbackCreateDto());
        }

        /// <summary>
        /// Envía el feedback
        /// </summary>
        [HttpPost("Enviar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enviar([FromForm] FeedbackCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                await CargarAsuntos();
                return View("Index", dto);
            }

            var userId = GetUserId();
            var (success, message) = await _service.EnviarFeedbackAsync(dto, userId);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success, message });
            }

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return View("Enviado");
            }

            TempData["ErrorMessage"] = message;
            await CargarAsuntos();
            return View("Index", dto);
        }

        /// <summary>
        /// Vista de confirmación de envío
        /// </summary>
        [HttpGet("Enviado")]
        public IActionResult Enviado()
        {
            return View();
        }

        private async Task CargarAsuntos()
        {
            var asuntos = await _service.ObtenerAsuntosAsync();
            ViewBag.Asuntos = asuntos.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Tipo
            }).ToList();
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
