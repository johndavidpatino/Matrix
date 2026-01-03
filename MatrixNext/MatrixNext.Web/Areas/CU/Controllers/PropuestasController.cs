using System;
using System.Security.Claims;
using MatrixNext.Data.Modules.CU.Models;
using MatrixNext.Data.Services.CU;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MatrixNext.Web.Areas.CU.Controllers
{
    [Area("CU")]
    [Route("CU/Propuestas")]
    [Authorize]
    public class PropuestasController : Controller
    {
        private readonly PropuestaService _propuestaService;
        private readonly ILogger<PropuestasController> _logger;

        public PropuestasController(PropuestaService propuestaService, ILogger<PropuestasController> logger)
        {
            _propuestaService = propuestaService ?? throw new ArgumentNullException(nameof(propuestaService));
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

        private JobBookContextViewModel? GetJobBookContext()
        {
            if (TempData.TryGetValue("JobBookContext", out var raw) && raw is string json)
            {
                try
                {
                    var ctx = JsonSerializer.Deserialize<JobBookContextViewModel>(json);
                    TempData.Keep("JobBookContext");
                    return ctx;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "No se pudo deserializar JobBookContext");
                }
            }

            return null;
        }

        [HttpGet("")]
        public IActionResult Index(long? idPropuesta = null, long? idBrief = null, byte? estadoId = null)
        {
            var userId = GetCurrentUserId();
            var listado = _propuestaService.ObtenerListado(userId, estadoId);
            var contexto = GetJobBookContext();
            var vm = new PropuestasIndexViewModel
            {
                Propuestas = listado,
                Estados = _propuestaService.ObtenerEstados(),
                EstadoFiltro = estadoId,
                IdBriefContext = idBrief ?? contexto?.IdBrief,
                IdPropuestaContext = idPropuesta ?? contexto?.IdPropuesta
            };
            ViewBag.JobBookContext = contexto;
            return View(vm);
        }

        [HttpGet("Crear")]
        public IActionResult Crear(long? idBrief = null)
        {
            var form = _propuestaService.PrepararFormulario(null, idBrief);
            return PartialView("_ModalCrear", form);
        }

        [HttpGet("Editar/{id:long}")]
        public IActionResult Editar(long id)
        {
            var form = _propuestaService.PrepararFormulario(id, null);
            return PartialView("_ModalCrear", form);
        }

        [HttpPost("Guardar")]
        public IActionResult Guardar(PropuestaViewModel model)
        {
            var (success, message, id) = _propuestaService.Guardar(model);
            return Ok(new { success, message, id });
        }

        [HttpPost("Eliminar/{id:long}")]
        public IActionResult Eliminar(long id)
        {
            var (success, message) = _propuestaService.Eliminar(id);
            return Ok(new { success, message });
        }

        [HttpGet("Observaciones/{id:long}")]
        public IActionResult Observaciones(long id)
        {
            var data = _propuestaService.ObtenerObservaciones(id);
            ViewBag.IdPropuesta = id;
            return PartialView("_ModalObservaciones", data);
        }

        [HttpPost("AgregarObservacion")]
        public IActionResult AgregarObservacion(long idPropuesta, string observacion)
        {
            var userId = GetCurrentUserId();
            var (success, message) = _propuestaService.AgregarObservacion(idPropuesta, userId, observacion);
            return Ok(new { success, message });
        }
    }
}
