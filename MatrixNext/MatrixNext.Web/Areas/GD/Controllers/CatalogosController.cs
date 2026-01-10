using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MatrixNext.Data.Services.GD.Interfaces;
using MatrixNext.Web.Models.ViewModels.GD;

namespace MatrixNext.Web.Areas.GD.Controllers
{
    [Area("GD")]
    [Authorize]
    public class CatalogosController : Controller
    {
        private readonly IGdCatalogosService _service;
        private readonly ILogger<CatalogosController> _logger;

        public CatalogosController(IGdCatalogosService service, ILogger<CatalogosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Title"] = "Catálogos Gestión Documental";
            return View();
        }

        // GET: /GD/Catalogos/TiposSolicitud
        [HttpGet]
        public async Task<IActionResult> TiposSolicitud()
        {
            _logger.LogInformation("Accediendo a TiposSolicitud");
            var (success, data) = await _service.ObtenerTipoSolicitudes();
            var vm = success
                ? data.Select(d => new TipoSolicitudViewModel { Id = d.Id, Nombre = d.Nombre, Descripcion = d.Descripcion }).ToList()
                : new System.Collections.Generic.List<TipoSolicitudViewModel>();
            return View("~/Areas/GD/Views/Catalogos/TiposSolicitud.cshtml", vm);
        }

        // GET: /GD/Catalogos/CreateTipo
        [HttpGet]
        public IActionResult CreateTipo()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("~/Areas/GD/Views/Catalogos/_CreateTipoModal.cshtml", new TipoSolicitudViewModel());
            return View("~/Areas/GD/Views/Catalogos/_CreateTipoModal.cshtml", new TipoSolicitudViewModel());
        }

        // POST: /GD/Catalogos/CreateTipo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTipo(TipoSolicitudViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return PartialView("~/Areas/GD/Views/Catalogos/_CreateTipoModal.cshtml", vm);
                return View("~/Areas/GD/Views/Catalogos/_CreateTipoModal.cshtml", vm);
            }

            var (success, idCreado) = await _service.CrearTipoSolicitud(new MatrixNext.Data.Models.GD.TipoSolicitudDto
            {
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion
            });

            if (success)
            {
                _logger.LogInformation("Tipo solicitud creado: {Id}", idCreado);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = true, id = idCreado, message = "Tipo creado exitosamente" });
                return RedirectToAction(nameof(TiposSolicitud));
            }

            ModelState.AddModelError("", "Error al crear tipo solicitud");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("~/Areas/GD/Views/Catalogos/_CreateTipoModal.cshtml", vm);
            return View("~/Areas/GD/Views/Catalogos/_CreateTipoModal.cshtml", vm);
        }

        // GET: /GD/Catalogos/UpdateTipo/{id}
        [HttpGet]
        public async Task<IActionResult> UpdateTipo(int id)
        {
            // Pending: fetch existing by id if available; for now, render empty form
            var vm = new TipoSolicitudViewModel { Id = id };
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("~/Areas/GD/Views/Catalogos/_UpdateTipoModal.cshtml", vm);
            return View("~/Areas/GD/Views/Catalogos/_UpdateTipoModal.cshtml", vm);
        }

        // POST: /GD/Catalogos/UpdateTipo/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTipo(int id, TipoSolicitudViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return PartialView("~/Areas/GD/Views/Catalogos/_UpdateTipoModal.cshtml", vm);
                return View("~/Areas/GD/Views/Catalogos/_UpdateTipoModal.cshtml", vm);
            }

            var (success, message) = await _service.ActualizarTipoSolicitud(id, new MatrixNext.Data.Models.GD.TipoSolicitudDto
            {
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion
            });

            if (success)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = true, message = "Actualizado" });
                return RedirectToAction(nameof(TiposSolicitud));
            }

            ModelState.AddModelError("", message);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("~/Areas/GD/Views/Catalogos/_UpdateTipoModal.cshtml", vm);
            return View("~/Areas/GD/Views/Catalogos/_UpdateTipoModal.cshtml", vm);
        }

        // POST: /GD/Catalogos/DeleteTipo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTipo(int id)
        {
            var (success, _) = await _service.EliminarTipoSolicitud(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success });
            return RedirectToAction(nameof(TiposSolicitud));
        }

        // ========== ESTADOS SOLICITUD ==========
        [HttpGet]
        public async Task<IActionResult> EstadosSolicitud()
        {
            _logger.LogInformation("Accediendo a EstadosSolicitud");
            var (success, data) = await _service.ObtenerEstadosSolicitud();
            var vm = success
                ? data.Select(d => new EstadoSolicitudViewModel { Id = d.Id, Nombre = d.Nombre, Descripcion = d.Descripcion }).ToList()
                : new System.Collections.Generic.List<EstadoSolicitudViewModel>();
            return View("~/Areas/GD/Views/Catalogos/EstadosSolicitud.cshtml", vm);
        }

        [HttpGet]
        public IActionResult CreateEstado()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("~/Areas/GD/Views/Catalogos/_CreateEstadoModal.cshtml", new EstadoSolicitudViewModel());
            return View("~/Areas/GD/Views/Catalogos/_CreateEstadoModal.cshtml", new EstadoSolicitudViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEstado(EstadoSolicitudViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return PartialView("~/Areas/GD/Views/Catalogos/_CreateEstadoModal.cshtml", vm);
                return View("~/Areas/GD/Views/Catalogos/_CreateEstadoModal.cshtml", vm);
            }

            var (success, idCreado) = await _service.CrearEstadoSolicitud(new MatrixNext.Data.Models.GD.EstadoSolicitudDto
            {
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion
            });

            if (success)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = true, id = idCreado, message = "Estado creado" });
                return RedirectToAction(nameof(EstadosSolicitud));
            }

            ModelState.AddModelError("", "Error al crear estado");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("~/Areas/GD/Views/Catalogos/_CreateEstadoModal.cshtml", vm);
            return View("~/Areas/GD/Views/Catalogos/_CreateEstadoModal.cshtml", vm);
        }

        [HttpGet]
        public IActionResult UpdateEstado(int id)
        {
            var vm = new EstadoSolicitudViewModel { Id = id };
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("~/Areas/GD/Views/Catalogos/_UpdateEstadoModal.cshtml", vm);
            return View("~/Areas/GD/Views/Catalogos/_UpdateEstadoModal.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEstado(int id, EstadoSolicitudViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return PartialView("~/Areas/GD/Views/Catalogos/_UpdateEstadoModal.cshtml", vm);
                return View("~/Areas/GD/Views/Catalogos/_UpdateEstadoModal.cshtml", vm);
            }

            var (success, message) = await _service.ActualizarEstadoSolicitud(id, new MatrixNext.Data.Models.GD.EstadoSolicitudDto
            {
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion
            });
            if (success)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = true, message = "Actualizado" });
                return RedirectToAction(nameof(EstadosSolicitud));
            }
            ModelState.AddModelError("", message);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("~/Areas/GD/Views/Catalogos/_UpdateEstadoModal.cshtml", vm);
            return View("~/Areas/GD/Views/Catalogos/_UpdateEstadoModal.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEstado(int id)
        {
            var (success, _) = await _service.EliminarEstadoSolicitud(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success });
            return RedirectToAction(nameof(EstadosSolicitud));
        }

        // ========== PROCESOS ==========
        [HttpGet]
        public async Task<IActionResult> Procesos()
        {
            _logger.LogInformation("Accediendo a Procesos");
            var (success, data) = await _service.ObtenerProcesos();
            var vm = success
                ? data.Select(d => new ProcesoViewModel { Id = d.Id, Nombre = d.Nombre, Descripcion = d.Descripcion }).ToList()
                : new System.Collections.Generic.List<ProcesoViewModel>();
            return View("~/Areas/GD/Views/Catalogos/Procesos.cshtml", vm);
        }

        [HttpGet]
        public IActionResult CreateProceso()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("~/Areas/GD/Views/Catalogos/_CreateProcesoModal.cshtml", new ProcesoViewModel());
            return View("~/Areas/GD/Views/Catalogos/_CreateProcesoModal.cshtml", new ProcesoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProceso(ProcesoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return PartialView("~/Areas/GD/Views/Catalogos/_CreateProcesoModal.cshtml", vm);
                return View("~/Areas/GD/Views/Catalogos/_CreateProcesoModal.cshtml", vm);
            }

            var (success, idCreado) = await _service.CrearProceso(new MatrixNext.Data.Models.GD.ProcesoDto
            {
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion
            });
            if (success)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = true, id = idCreado, message = "Proceso creado" });
                return RedirectToAction(nameof(Procesos));
            }
            ModelState.AddModelError("", "Error al crear proceso");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("~/Areas/GD/Views/Catalogos/_CreateProcesoModal.cshtml", vm);
            return View("~/Areas/GD/Views/Catalogos/_CreateProcesoModal.cshtml", vm);
        }

        [HttpGet]
        public IActionResult UpdateProceso(int id)
        {
            var vm = new ProcesoViewModel { Id = id };
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("~/Areas/GD/Views/Catalogos/_UpdateProcesoModal.cshtml", vm);
            return View("~/Areas/GD/Views/Catalogos/_UpdateProcesoModal.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProceso(int id, ProcesoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return PartialView("~/Areas/GD/Views/Catalogos/_UpdateProcesoModal.cshtml", vm);
                return View("~/Areas/GD/Views/Catalogos/_UpdateProcesoModal.cshtml", vm);
            }
            var (success, message) = await _service.ActualizarProceso(id, new MatrixNext.Data.Models.GD.ProcesoDto
            {
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion
            });
            if (success)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = true, message = "Actualizado" });
                return RedirectToAction(nameof(Procesos));
            }
            ModelState.AddModelError("", message);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("~/Areas/GD/Views/Catalogos/_UpdateProcesoModal.cshtml", vm);
            return View("~/Areas/GD/Views/Catalogos/_UpdateProcesoModal.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProceso(int id)
        {
            var (success, _) = await _service.EliminarProceso(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success });
            return RedirectToAction(nameof(Procesos));
        }
    }
}
