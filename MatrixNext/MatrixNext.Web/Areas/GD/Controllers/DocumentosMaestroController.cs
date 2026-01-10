using System.Linq;
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
    public class DocumentosMaestroController : Controller
    {
        private readonly IGdMaestroService _service;
        private readonly ILogger<DocumentosMaestroController> _logger;

        public DocumentosMaestroController(IGdMaestroService service, ILogger<DocumentosMaestroController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: /GD/DocumentosMaestro
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Maestro de Documentos";
            var (success, data) = await _service.ObtenerMaestros();
            var vm = success
                ? data.Select(ToListVm).ToList()
                : new System.Collections.Generic.List<MaestroListVM>();
            return View(vm);
        }

        // GET: /GD/DocumentosMaestro/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var formData = await BuildFormDataVm();
            var model = new MaestroCreateVM
            {
                TiposSolicitud = formData.TiposSolicitud,
                Procesos = formData.Procesos,
                Usuarios = formData.Usuarios,
                ControlledDoc = new DocumentoControlledVM()
            };

            if (IsAjax())
            {
                return PartialView("~/Areas/GD/Views/DocumentosMaestro/_CreateMaestroModal.cshtml", model);
            }

            return View("~/Areas/GD/Views/DocumentosMaestro/_CreateMaestroModal.cshtml", model);
        }

        // POST: /GD/DocumentosMaestro/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaestroCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                var formData = await BuildFormDataVm();
                vm.TiposSolicitud = formData.TiposSolicitud;
                vm.Procesos = formData.Procesos;
                vm.Usuarios = formData.Usuarios;

                if (IsAjax())
                {
                    return PartialView("~/Areas/GD/Views/DocumentosMaestro/_CreateMaestroModal.cshtml", vm);
                }

                return View("~/Areas/GD/Views/DocumentosMaestro/_CreateMaestroModal.cshtml", vm);
            }

            var dto = ToDto(vm);
            var (success, id, message) = await _service.CrearMaestro(dto);

            if (success)
            {
                _logger.LogInformation("Maestro creado: {Id}", id);
                if (IsAjax())
                {
                    return Json(new { success = true, id, message });
                }

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, message);
            var formDataError = await BuildFormDataVm();
            vm.TiposSolicitud = formDataError.TiposSolicitud;
            vm.Procesos = formDataError.Procesos;
            vm.Usuarios = formDataError.Usuarios;

            if (IsAjax())
            {
                return PartialView("~/Areas/GD/Views/DocumentosMaestro/_CreateMaestroModal.cshtml", vm);
            }

            return View("~/Areas/GD/Views/DocumentosMaestro/_CreateMaestroModal.cshtml", vm);
        }

        // GET: /GD/DocumentosMaestro/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var (success, maestro, controlado) = await _service.ObtenerMaestroById(id);
            if (!success || maestro == null)
            {
                return NotFound();
            }

            var formData = await BuildFormDataVm();
            var model = ToUpdateVm(maestro, controlado, formData);

            if (IsAjax())
            {
                return PartialView("~/Areas/GD/Views/DocumentosMaestro/_EditMaestroModal.cshtml", model);
            }

            return View("~/Areas/GD/Views/DocumentosMaestro/_EditMaestroModal.cshtml", model);
        }

        // POST: /GD/DocumentosMaestro/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaestroUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                var formData = await BuildFormDataVm();
                vm.TiposSolicitud = formData.TiposSolicitud;
                vm.Procesos = formData.Procesos;
                vm.Usuarios = formData.Usuarios;

                if (IsAjax())
                {
                    return PartialView("~/Areas/GD/Views/DocumentosMaestro/_EditMaestroModal.cshtml", vm);
                }

                return View("~/Areas/GD/Views/DocumentosMaestro/_EditMaestroModal.cshtml", vm);
            }

            var dto = ToDto(vm);
            var (success, message) = await _service.ActualizarMaestro(id, dto);

            if (success)
            {
                _logger.LogInformation("Maestro actualizado: {Id}", id);
                if (IsAjax())
                {
                    return Json(new { success = true, message });
                }

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, message);
            var formDataError = await BuildFormDataVm();
            vm.TiposSolicitud = formDataError.TiposSolicitud;
            vm.Procesos = formDataError.Procesos;
            vm.Usuarios = formDataError.Usuarios;

            if (IsAjax())
            {
                return PartialView("~/Areas/GD/Views/DocumentosMaestro/_EditMaestroModal.cshtml", vm);
            }

            return View("~/Areas/GD/Views/DocumentosMaestro/_EditMaestroModal.cshtml", vm);
        }

        // POST: /GD/DocumentosMaestro/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _service.AnularMaestro(id);
            return Json(new { success, message });
        }

        // GET: /GD/DocumentosMaestro/GetFormData
        [HttpGet]
        public async Task<IActionResult> GetFormData()
        {
            var data = await BuildFormDataVm();
            return Json(new { success = true, data });
        }

        private bool IsAjax() => Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        private static MaestroListVM ToListVm(MaestroDocumentoDto dto)
        {
            return new MaestroListVM
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Codigo = dto.Codigo,
                Proceso = dto.ProcesoNombre,
                Responsable = dto.ResponsableNombre,
                Estado = dto.Activo ? "Activo" : "Inactivo",
                FechaRegistro = dto.ControlledDoc?.FechaRegistro ?? System.DateTime.Now
            };
        }

        private MaestroDocumentoDto ToDto(MaestroDocumentoVM vm)
        {
            return new MaestroDocumentoDto
            {
                Id = vm.Id,
                Nombre = vm.Nombre,
                Codigo = vm.Codigo,
                IdProceso = vm.IdProceso,
                IdResponsable = vm.IdResponsable,
                TipoSolicitud = vm.TipoSolicitud,
                Activo = vm.Activo,
                ControlledDoc = new DocumentoControlledDto
                {
                    Id = vm.ControlledDoc?.Id ?? 0,
                    IdMaestro = vm.Id,
                    Ubicacion = vm.ControlledDoc?.Ubicacion ?? string.Empty,
                    MetodoRecuperacion = vm.ControlledDoc?.MetodoRecuperacion ?? string.Empty,
                    TiempoRetencion = vm.ControlledDoc?.TiempoRetencion ?? 0,
                    DisposicionFinal = vm.ControlledDoc?.DisposicionFinal ?? string.Empty,
                    Activo = vm.ControlledDoc?.Activo ?? true
                }
            };
        }

        private async Task<MaestroCreateVM> BuildFormDataVm()
        {
            var (success, data) = await _service.ObtenerFormData();
            var vm = new MaestroCreateVM
            {
                TiposSolicitud = success ? data.TiposSolicitud.Select(t => new TipoSolicitudViewModel { Id = t.Id, Nombre = t.Nombre }).ToList() : new System.Collections.Generic.List<TipoSolicitudViewModel>(),
                Procesos = success ? data.Procesos.Select(p => new ProcesoViewModel { Id = p.Id, Nombre = p.Nombre }).ToList() : new System.Collections.Generic.List<ProcesoViewModel>(),
                Usuarios = success ? data.Usuarios.Select(u => new UsuarioViewModel { Id = u.Id, Usuario = u.Usuario, Nombres = u.Nombres, Apellidos = u.Apellidos, Email = u.Email, Activo = u.Activo }).ToList() : new System.Collections.Generic.List<UsuarioViewModel>()
            };
            return vm;
        }

        private MaestroUpdateVM ToUpdateVm(MaestroDocumentoDto maestro, DocumentoControlledDto? ctrl, MaestroCreateVM formData)
        {
            return new MaestroUpdateVM
            {
                Id = maestro.Id,
                Nombre = maestro.Nombre,
                Codigo = maestro.Codigo,
                IdProceso = maestro.IdProceso,
                IdResponsable = maestro.IdResponsable,
                TipoSolicitud = maestro.TipoSolicitud,
                Activo = maestro.Activo,
                ControlledDoc = ctrl == null ? new DocumentoControlledVM() : new DocumentoControlledVM
                {
                    Id = ctrl.Id,
                    IdMaestro = ctrl.IdMaestro,
                    Ubicacion = ctrl.Ubicacion,
                    MetodoRecuperacion = ctrl.MetodoRecuperacion,
                    TiempoRetencion = ctrl.TiempoRetencion,
                    DisposicionFinal = ctrl.DisposicionFinal,
                    Activo = ctrl.Activo,
                    FechaRegistro = ctrl.FechaRegistro
                },
                TiposSolicitud = formData.TiposSolicitud,
                Procesos = formData.Procesos,
                Usuarios = formData.Usuarios,
                RegistradoPor = maestro.IdResponsable,
                RegistradoPorNombre = maestro.ResponsableNombre,
                FechaRegistro = ctrl?.FechaRegistro ?? System.DateTime.Now,
                ModificadoPor = null,
                ModificadoPorNombre = string.Empty,
                FechaModificacion = null
            };
        }
    }
}
