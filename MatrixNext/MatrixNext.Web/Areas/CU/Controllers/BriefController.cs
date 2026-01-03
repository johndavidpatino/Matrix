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
    [Route("CU/Brief")]
    [Authorize]
    public class BriefController : Controller
    {
        private readonly BriefService _briefService;
        private readonly CuentaService _cuentaService;
        private readonly ILogger<BriefController> _logger;

        public BriefController(BriefService briefService, CuentaService cuentaService, ILogger<BriefController> logger)
        {
            _briefService = briefService ?? throw new ArgumentNullException(nameof(briefService));
            _cuentaService = cuentaService ?? throw new ArgumentNullException(nameof(cuentaService));
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

        private void SetContextTempData(JobBookContextViewModel? ctx)
        {
            if (ctx == null) return;
            TempData["JobBookContext"] = JsonSerializer.Serialize(ctx);
        }

        [HttpGet("")]
        public IActionResult Index(long? id = null)
        {
            var usuarioId = GetCurrentUserId();
            var contexto = GetJobBookContext();
            var idBrief = id ?? contexto?.IdBrief;
            var form = _briefService.PrepararFormulario(idBrief, usuarioId);
            form.Contexto = contexto;
            return View(form);
        }

        [HttpPost("Guardar")]
        public IActionResult Guardar(BriefViewModel model)
        {
            var userId = GetCurrentUserId();
            var (success, message, id) = _briefService.Guardar(model, userId);
            JobBookContextViewModel? ctx = null;
            if (success)
            {
                ctx = _cuentaService.ObtenerContexto(id, null, null, userId);
                SetContextTempData(ctx);
            }
            return Ok(new { success, message, id, jobBook = ctx?.NumJobBook });
        }

        [HttpPost("Viabilidad")]
        public IActionResult MarcarViabilidad(long id, bool viable)
        {
            var userId = GetCurrentUserId();
            var (success, message) = _briefService.MarcarViabilidad(id, viable, userId);
            if (success)
            {
                var ctx = _cuentaService.ObtenerContexto(id, null, null, userId);
                SetContextTempData(ctx);
            }
            return Ok(new { success, message });
        }
    }
}
