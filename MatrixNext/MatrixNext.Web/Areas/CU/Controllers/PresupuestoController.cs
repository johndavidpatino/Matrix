using System;
using System.Collections.Generic;
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
        private readonly PresupuestoServiceExtended _presupuestoService;
        private readonly ILogger<PresupuestoController> _logger;

        public PresupuestoController(PresupuestoServiceExtended presupuestoService, ILogger<PresupuestoController> logger)
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
        public IActionResult Index(long? idPropuesta = null, int? alternativa = null, int? tecnica = null)
        {
            var contexto = GetJobBookContext();
            var propuestaId = idPropuesta ?? contexto?.IdPropuesta;
            if (propuestaId == null || propuestaId <= 0)
            {
                TempData["ErrorMessage"] = "Seleccione un JobBook desde Cuentas antes de ingresar a Presupuesto.";
                return RedirectToAction("Index", "Cuentas", new { area = "CU" });
            }

            var model = _presupuestoService.PrepararIndex(propuestaId.Value, contexto);
            model.AlternativaSeleccionada = alternativa;
            model.TecnicaSeleccionada = tecnica;

            if (alternativa.HasValue)
            {
                model.Presupuestos = _presupuestoService.ObtenerPresupuestos(propuestaId.Value, alternativa.Value, tecnica);
            }

            return View(model);
        }

        #region Alternativas

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

        #endregion

        #region Presupuestos

        [HttpGet("Presupuestos")]
        public IActionResult ObtenerPresupuestos(long idPropuesta, int alternativa, int? tecnica = null)
        {
            var presupuestos = _presupuestoService.ObtenerPresupuestos(idPropuesta, alternativa, tecnica);
            return PartialView("_GridPresupuestos", presupuestos);
        }

        [HttpGet("ModalPresupuesto")]
        public IActionResult ModalPresupuesto(long idPropuesta, int alternativa, int? metCodigo = null, int? nacional = null, int? tecCodigo = null)
        {
            EditarPresupuestoViewModel vm;

            if (metCodigo.HasValue && nacional.HasValue)
            {
                // Editar presupuesto existente
                vm = _presupuestoService.ObtenerPresupuesto(idPropuesta, alternativa, metCodigo.Value, nacional.Value) 
                    ?? new EditarPresupuestoViewModel();
            }
            else
            {
                // Nuevo presupuesto
                vm = _presupuestoService.PrepararNuevoPresupuesto(
                    idPropuesta, 
                    alternativa, 
                    tecCodigo ?? 100, 
                    metCodigo ?? 0, 
                    nacional ?? 1
                );
            }

            return PartialView("_ModalPresupuesto", vm);
        }

        [HttpPost("GuardarPresupuesto")]
        public IActionResult GuardarPresupuesto([FromBody] EditarPresupuestoViewModel model)
        {
            try
            {
                var usuarioId = GetCurrentUserId();
                var (success, message) = _presupuestoService.GuardarPresupuesto(model, usuarioId);
                return Ok(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GuardarPresupuesto");
                return Ok(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("EliminarPresupuesto")]
        public IActionResult EliminarPresupuesto([FromBody] EliminarPresupuestoRequest request)
        {
            var (success, message) = _presupuestoService.EliminarPresupuesto(
                request.IdPropuesta, 
                request.ParAlternativa, 
                request.MetCodigo, 
                request.ParNacional
            );
            return Ok(new { success, message });
        }

        #endregion

        #region Muestra

        [HttpPost("AgregarMuestra")]
        public IActionResult AgregarMuestra([FromBody] MuestraItemViewModel muestra)
        {
            var (success, message) = _presupuestoService.AgregarMuestra(muestra);
            return Ok(new { success, message });
        }

        [HttpPost("EliminarMuestra")]
        public IActionResult EliminarMuestra([FromBody] EliminarMuestraRequest request)
        {
            var (success, message) = _presupuestoService.EliminarMuestra(
                request.IdPropuesta,
                request.ParAlternativa,
                request.MetCodigo,
                request.CiuCodigo,
                request.MuIdentificador,
                request.ParNacional
            );
            return Ok(new { success, message });
        }

        #endregion
    }

    public class EliminarPresupuestoRequest
    {
        public long IdPropuesta { get; set; }
        public int ParAlternativa { get; set; }
        public int MetCodigo { get; set; }
        public int ParNacional { get; set; }
    }

    public class EliminarMuestraRequest
    {
        public long IdPropuesta { get; set; }
        public int ParAlternativa { get; set; }
        public int MetCodigo { get; set; }
        public int CiuCodigo { get; set; }
        public int MuIdentificador { get; set; }
        public int ParNacional { get; set; }
    }
}
