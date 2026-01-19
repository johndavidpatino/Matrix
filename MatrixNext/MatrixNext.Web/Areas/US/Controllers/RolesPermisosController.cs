using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MatrixNext.Data.Modules.US.RolesPermisos.Models;
using MatrixNext.Data.Modules.US.RolesPermisos.Services;

namespace MatrixNext.Web.Areas.US.Controllers
{
    /// <summary>
    /// Controlador para asignación de Roles a Permisos
    /// Ref: WebMatrix/US_Usuarios/RolesPermisos.aspx
    /// SP: US_RolesPermisos_Get, US_RolesPermisos_Add, US_RolesPermisos_Del
    /// </summary>
    [Area("US")]
    [Route("Usuarios/RolesPermisos")]
    [Authorize]
    public class RolesPermisosController : Controller
    {
        private readonly IRolPermisoService _service;
        private readonly ILogger<RolesPermisosController> _logger;

        public RolesPermisosController(IRolPermisoService service, ILogger<RolesPermisosController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Vista principal - requiere IdPermiso
        /// </summary>
        [HttpGet("{permisoId:int}")]
        public async Task<IActionResult> Index(int permisoId)
        {
            if (permisoId <= 0)
            {
                TempData["ErrorMessage"] = "El Id del permiso es requerido";
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            await CargarRoles();
            ViewBag.PermisoId = permisoId;
            
            var rolesPermisos = await _service.ObtenerRolesPermisosAsync(permisoId);
            return View(rolesPermisos);
        }

        /// <summary>
        /// Lista parcial para AJAX
        /// </summary>
        [HttpGet("{permisoId:int}/Lista")]
        public async Task<IActionResult> Lista(int permisoId)
        {
            var rolesPermisos = await _service.ObtenerRolesPermisosAsync(permisoId);
            return PartialView("_Lista", rolesPermisos);
        }

        /// <summary>
        /// Modal para agregar rol
        /// </summary>
        [HttpGet("{permisoId:int}/Agregar")]
        public async Task<IActionResult> Agregar(int permisoId)
        {
            await CargarRoles();
            var dto = new RolPermisoCreateDto { PermisoId = permisoId };
            return PartialView("_AgregarModal", dto);
        }

        /// <summary>
        /// Guarda la asignación rol-permiso
        /// </summary>
        [HttpPost("Guardar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar([FromForm] RolPermisoCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            var (success, message) = await _service.GuardarRolPermisoAsync(dto);

            return Json(new { success, message });
        }

        /// <summary>
        /// Modal de confirmación de eliminación
        /// </summary>
        [HttpGet("{permisoId:int}/Eliminar/{rolId:int}")]
        public async Task<IActionResult> Eliminar(int permisoId, int rolId)
        {
            var rolesPermisos = await _service.ObtenerRolesPermisosAsync(permisoId);
            var rolPermiso = rolesPermisos.FirstOrDefault(rp => rp.RolId == rolId);
            
            if (rolPermiso == null)
            {
                return NotFound();
            }

            return PartialView("_EliminarModal", rolPermiso);
        }

        /// <summary>
        /// Elimina la asignación rol-permiso
        /// </summary>
        [HttpPost("EliminarConfirmado")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado([FromForm] int permisoId, [FromForm] int rolId)
        {
            var (success, message) = await _service.EliminarRolPermisoAsync(permisoId, rolId);

            return Json(new { success, message });
        }

        private async Task CargarRoles()
        {
            var roles = await _service.ObtenerRolesComboAsync();
            ViewBag.Roles = roles.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Rol
            }).ToList();
        }
    }
}
