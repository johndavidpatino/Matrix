using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MatrixNext.Data.Services.GD.Interfaces;
using MatrixNext.Web.Models.ViewModels.GD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.GD.Controllers
{
    [Area("GD")]
    [Authorize]
    public class AprobacionesController : Controller
    {
        private readonly IGdAprobacionesService _service;
        private readonly ILogger<AprobacionesController> _logger;

        public AprobacionesController(IGdAprobacionesService service, ILogger<AprobacionesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Aprobaciones de Documentos";

            var userId = GetUserId();
            if (userId <= 0)
            {
                return Forbid();
            }

            var (success, data, message) = await _service.ObtenerRevisionesPendientes(userId);
            var vm = new AprobacionesIndexVM
            {
                Revisiones = success
                    ? data.Select(d => new RevisionAprobacionVM
                    {
                        IdRevision = d.IdRevision,
                        DocumentoId = d.DocumentoId,
                        UsuarioId = d.UsuarioId,
                        TipoRevision = d.TipoRevision,
                        NombreDocumento = d.NombreDocumento,
                        FechaAprobacion = d.FechaAprobacion
                    }).ToList()
                    : new System.Collections.Generic.List<RevisionAprobacionVM>(),
                Error = success ? null : message
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aprobar(int idRevision, int documentoId)
        {
            var userId = GetUserId();
            if (userId <= 0)
            {
                return Json(new { success = false, message = "Usuario no autenticado" });
            }

            var (success, message) = await _service.AprobarRevision(idRevision, documentoId, userId);
            if (success)
            {
                _logger.LogInformation("Revisión aprobada {RevisionId} por usuario {UserId}", idRevision, userId);
            }

            return Json(new { success, message });
        }

        private int GetUserId()
        {
            var claimValue = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";
            return int.TryParse(claimValue, out var id) ? id : 0;
        }
    }
}
