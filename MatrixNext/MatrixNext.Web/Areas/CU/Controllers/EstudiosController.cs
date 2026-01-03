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
    [Route("CU/Estudios")]
    [Authorize]
    public class EstudiosController : Controller
    {
        private readonly EstudioService _estudioService;
        private readonly ILogger<EstudiosController> _logger;

        public EstudiosController(EstudioService estudioService, ILogger<EstudiosController> logger)
        {
            _estudioService = estudioService ?? throw new ArgumentNullException(nameof(estudioService));
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
        public IActionResult Index(long? idPropuesta = null, long? idEstudio = null)
        {
            var contexto = GetJobBookContext();
            var propuestaId = idPropuesta ?? contexto?.IdPropuesta;
            if (propuestaId == null || propuestaId == 0)
            {
                TempData["ErrorMessage"] = "Seleccione un JobBook desde Cuentas antes de ver estudios.";
                return RedirectToAction("Index", "Cuentas", new { area = "CU" });
            }

            var userId = GetCurrentUserId();
            var listado = _estudioService.Listar(propuestaId.Value, userId);
            var vm = new EstudiosIndexViewModel
            {
                IdPropuesta = propuestaId,
                Estudios = listado,
                Contexto = contexto
            };
            return View(vm);
        }

        [HttpGet("Crear")]
        public IActionResult Crear(long idPropuesta)
        {
            var form = _estudioService.PrepararFormulario(null, idPropuesta);
            var contexto = GetJobBookContext();
            if (contexto != null)
            {
                form.Estudio.Nombre = contexto.Titulo;
                form.Estudio.JobBook = contexto.NumJobBook;
            }
            return PartialView("_ModalCrear", form);
        }

        [HttpGet("Editar/{id:long}")]
        public IActionResult Editar(long id)
        {
            var form = _estudioService.PrepararFormulario(id, null);
            return PartialView("_ModalCrear", form);
        }

        [HttpPost("Guardar")]
        public IActionResult Guardar(EstudioViewModel model)
        {
            var (success, message, id) = _estudioService.Guardar(model);
            return Ok(new { success, message, id });
        }
    }
}
