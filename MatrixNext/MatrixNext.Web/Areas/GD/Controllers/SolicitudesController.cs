using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD.Models;
using MatrixNext.Data.Services.GD.Interfaces;
using MatrixNext.Web.Models.ViewModels.GD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.GD.Controllers
{
    [Area("GD")]
    [Authorize]
    public class SolicitudesController : Controller
    {
        private readonly IGdSolicitudesService _service;
        private readonly ILogger<SolicitudesController> _logger;

        public SolicitudesController(IGdSolicitudesService service, ILogger<SolicitudesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Lista de solicitudes
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Solicitudes de Documentos";

            var (success, data, message) = await _service.ObtenerSolicitudes();
            var vm = new SolicitudListVM
            {
                Solicitudes = success ? data.Select(ToListVm).ToList() : new List<SolicitudListItemVM>()
            };

            if (!success)
                ViewData["Error"] = message;

            return View(vm);
        }

        /// <summary>
        /// GET: Formulario de creación de solicitud (modal)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var (success, formData) = await _service.ObtenerFormData();
            
            var vm = new SolicitudCreateVM();
            if (success)
            {
                vm.TiposSolicitud = formData.TiposSolicitud;
                vm.Documentos = formData.Documentos;
                vm.Usuarios = formData.Usuarios;
                vm.Estados = formData.Estados;
            }

            return PartialView("~/Areas/GD/Views/Solicitudes/_CreateModal.cshtml", vm);
        }

        /// <summary>
        /// POST: Crear solicitud
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SolicitudCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                var (successForm, formData) = await _service.ObtenerFormData();
                if (successForm)
                {
                    vm.TiposSolicitud = formData.TiposSolicitud;
                    vm.Documentos = formData.Documentos;
                    vm.Usuarios = formData.Usuarios;
                    vm.Estados = formData.Estados;
                }
                return PartialView("~/Areas/GD/Views/Solicitudes/_CreateModal.cshtml", vm);
            }

            var userId = GetUserId();
            if (userId <= 0)
                return Json(new { success = false, message = "Usuario no autenticado", idSolicitud = 0 });

            // Mapear VM a InputDto
            var inputDto = new SolicitudCreateInputDto
            {
                TipoSolicitud = vm.TipoSolicitud,
                IdDocumento = vm.IdDocumento,
                IdSolicitante = userId,
                Area = vm.Area,
                Cargo = vm.Cargo,
                Razon = vm.Razon,
                Descripcion = vm.Descripcion,
                IdEstado = vm.IdEstado ?? 1,
                Comentarios = vm.Comentarios,
                AreaUso = vm.AreaUso,
                SitioAcceso = vm.SitioAcceso,
                NombreDocumento = vm.NombreDocumento,
                Codigo = vm.Codigo
            };

            var (success, idSolicitud, message) = await _service.CrearSolicitud(inputDto);
            
            if (!success)
            {
                _logger.LogWarning("No se pudo crear solicitud para usuario {UserId}: {Message}", userId, message);
                return Json(new { success = false, message = message, idSolicitud = 0 });
            }

            _logger.LogInformation("Solicitud creada: ID {IdSolicitud} por usuario {UserId}", idSolicitud, userId);
            return Json(new { success = true, message = message, idSolicitud = idSolicitud });
        }

        /// <summary>
        /// GET: Asignar revisores (modal)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> AssignReviewers(int id)
        {
            if (id <= 0)
                return Json(new { success = false, message = "ID de solicitud inválido" });

            var (success, solicitud, message) = await _service.ObtenerSolicitudById(id);
            if (!success || solicitud == null)
                return Json(new { success = false, message = "Solicitud no encontrada" });

            var (successForm, formData) = await _service.ObtenerFormData();
            var vm = new AsignReviewersVM
            {
                IdSolicitud = id,
                RevisoresDisponibles = successForm ? formData.Usuarios : new List<SelectListItemDto>()
            };

            return PartialView("~/Areas/GD/Views/Solicitudes/_AssignReviewersModal.cshtml", vm);
        }

        /// <summary>
        /// POST: Asignar revisores a solicitud
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignReviewers(AsignReviewersVM vm)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Datos inválidos" });

            if (vm.IdRevisores == null || vm.IdRevisores.Count == 0)
                return Json(new { success = false, message = "Debe seleccionar al menos un revisor" });

            var (success, message) = await _service.AsignarRevisores(vm.IdSolicitud, vm.IdRevisores);
            
            if (!success)
            {
                _logger.LogWarning("No se pudieron asignar revisores a solicitud {IdSolicitud}: {Message}", vm.IdSolicitud, message);
                return Json(new { success = false, message = message });
            }

            _logger.LogInformation("Revisores asignados a solicitud {IdSolicitud}: {Message}", vm.IdSolicitud, message);
            return Json(new { success = true, message = message });
        }

        /// <summary>
        /// Obtiene el ID del usuario actual
        /// </summary>
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                return userId;
            return 0;
        }

        /// <summary>
        /// Mapea SolicitudListDto a SolicitudListItemVM
        /// </summary>
        private SolicitudListItemVM ToListVm(SolicitudListDto dto)
        {
            return new SolicitudListItemVM
            {
                Id = dto.Id,
                NombreDocumento = dto.NombreDocumento,
                TipoSolicitud = dto.TipoSolicitud,
                Solicitante = dto.Solicitante,
                Estado = dto.Estado,
                RevisoresPendientes = dto.RevisoresPendientes,
                RevisoresAprobados = dto.RevisoresAprobados,
                FechaRegistro = dto.FechaRegistro
            };
        }
    }
}
