using System;
using System.Security.Claims;
using System.Text.Json;
using MatrixNext.Data.Modules.CU.Models;
using MatrixNext.Data.Services.CU;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.CU.Controllers
{
    [Area("CU")]
    [Route("CU/Presupuesto")]
    [Authorize]
    public class PresupuestoController : Controller
    {
        private readonly PresupuestoService _presupuestoService;
        private readonly ILogger<PresupuestoController> _logger;

        public PresupuestoController(PresupuestoService presupuestoService, ILogger<PresupuestoController> logger)
        {
            _presupuestoService = presupuestoService ?? throw new ArgumentNullException(nameof(presupuestoService));
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

        [HttpGet("{idPropuesta:long?}")]
        [HttpGet("")]
        public IActionResult Index(long? idPropuesta = null)
        {
            var contexto = GetJobBookContext();
            var propuestaId = idPropuesta ?? contexto?.IdPropuesta;
            if (propuestaId == null || propuestaId <= 0)
            {
                TempData["ErrorMessage"] = "Seleccione un JobBook desde Cuentas antes de ingresar a Presupuesto.";
                return RedirectToAction("Index", "Cuentas", new { area = "CU" });
            }

            var model = _presupuestoService.PrepararIndex(propuestaId.Value, contexto);
            return View(model);
        }

        [HttpGet("Alternativa")]
        public IActionResult Alternativa(long idPropuesta, int? alternativa = null)
        {
            var vm = _presupuestoService.PrepararDatosGenerales(idPropuesta, alternativa);
            return PartialView("_ModalAlternativa", vm);
        }

        [HttpPost("GuardarAlternativa")]
        public IActionResult GuardarAlternativa(EditarAlternativaViewModel model)
        {
            var (success, message, alternativa) = _presupuestoService.GuardarDatosGenerales(model);
            return Ok(new { success, message, alternativa });
        }
    }
}
