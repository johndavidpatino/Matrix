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
    [Route("CU/Cuentas")]
    [Authorize]
    public class CuentasController : Controller
    {
        private readonly CuentaService _cuentaService;
        private readonly BriefService _briefService;
        private readonly ILogger<CuentasController> _logger;

        public CuentasController(CuentaService cuentaService, BriefService briefService, ILogger<CuentasController> logger)
        {
            _cuentaService = cuentaService ?? throw new ArgumentNullException(nameof(cuentaService));
            _briefService = briefService ?? throw new ArgumentNullException(nameof(briefService));
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

        [HttpGet("")]
        public IActionResult Index()
        {
            var model = new JobBookSearchViewModel { TypeSearch = 1 };
            return View(model);
        }

        [HttpPost("Buscar")]
        public IActionResult Buscar(JobBookSearchViewModel filtros)
        {
            var usuarioId = GetCurrentUserId();
            var model = _cuentaService.Buscar(filtros, usuarioId);
            return PartialView("_ResultadosGrid", model.Resultados);
        }

        [HttpGet("Abrir")]
        public IActionResult Abrir(long idBrief = 0, long idPropuesta = 0, long idEstudio = 0)
        {
            var usuarioId = GetCurrentUserId();
            var contexto = _cuentaService.ObtenerContexto(
                idBrief == 0 ? null : idBrief,
                idPropuesta == 0 ? null : idPropuesta,
                idEstudio == 0 ? null : idEstudio,
                usuarioId
            );

            if (contexto == null)
            {
                TempData["ErrorMessage"] = "No se encontro informacion del JobBook seleccionado.";
                return RedirectToAction("Index");
            }

            TempData["JobBookContext"] = JsonSerializer.Serialize(contexto);

            if (contexto.IdEstudio > 0)
            {
                return RedirectToAction("Index", "Estudios", new { area = "CU", idPropuesta = contexto.IdPropuesta, idEstudio = contexto.IdEstudio });
            }

            if (contexto.IdPropuesta > 0)
            {
                return RedirectToAction("Index", "Propuestas", new { area = "CU", idPropuesta = contexto.IdPropuesta });
            }

            return RedirectToAction("Index", "Brief", new { area = "CU", id = contexto.IdBrief });
        }

        // TODO-P0-03: Action para mostrar modal de clonación
        [HttpGet("MostrarModalClonar")]
        public IActionResult MostrarModalClonar(long idBrief, string? tituloOriginal)
        {
            var usuarioId = GetCurrentUserId();
            var unidades = _briefService.PrepararFormulario(null, usuarioId).Unidades;
            
            var model = new Data.Modules.CU.Models.ClonarBriefViewModel
            {
                IdBrief = idBrief,
                TituloOriginal = tituloOriginal ?? $"Brief ID: {idBrief}",
                Unidades = unidades
            };
            
            return PartialView("_ModalClonar", model);
        }

        [HttpPost("Clonar")]
        public IActionResult Clonar([FromBody] ClonarBriefRequest request)
        {
            if (request == null || request.IdBrief <= 0 || string.IsNullOrWhiteSpace(request.NuevoNombre) || request.IdUnidad <= 0)
            {
                return BadRequest(new { success = false, message = "Parametros incompletos para clonar el brief." });
            }

            var usuarioId = GetCurrentUserId();
            var (success, message) = _cuentaService.ClonarBrief(request.IdBrief, usuarioId, request.IdUnidad, request.NuevoNombre);

            return Ok(new { success, message });
        }

        public class ClonarBriefRequest
        {
            public long IdBrief { get; set; }
            public int IdUnidad { get; set; }
            public string? NuevoNombre { get; set; }
        }
    }
}
