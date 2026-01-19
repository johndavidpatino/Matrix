using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MatrixNext.Data.Modules.US.GruposPermisos.Models;
using MatrixNext.Data.Modules.US.GruposPermisos.Services;

namespace MatrixNext.Web.Areas.US.Controllers
{
    /// <summary>
    /// Controlador para Grupos de Permisos
    /// Ref: WebMatrix/US_Usuarios/GruposPermisos.aspx - Permiso 90
    /// SP: US_GruposPermisos_Get, US_GruposPermisos_Add, US_GruposPermisos_Edit, US_GruposPermisos_Del
    /// </summary>
    [Area("US")]
    [Route("Usuarios/GruposPermisos")]
    [Authorize]
    public class GruposPermisosController : Controller
    {
        private readonly IGrupoPermisoService _service;
        private readonly ILogger<GruposPermisosController> _logger;

        public GruposPermisosController(IGrupoPermisoService service, ILogger<GruposPermisosController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Vista principal con listado de grupos de permisos
        /// </summary>
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var grupos = await _service.ObtenerTodosAsync();
            return View(grupos);
        }

        /// <summary>
        /// Obtiene la lista de grupos filtrada (AJAX)
        /// </summary>
        [HttpGet("Lista")]
        public async Task<IActionResult> Lista(string? filtro)
        {
            var grupos = await _service.ObtenerTodosAsync(filtro);
            return PartialView("_Lista", grupos);
        }

        /// <summary>
        /// Modal para crear nuevo grupo de permisos
        /// </summary>
        [HttpGet("CreateModal")]
        public IActionResult CreateModal()
        {
            return PartialView("_CreateEditModal", new GrupoPermisoDto());
        }

        /// <summary>
        /// Modal para editar grupo de permisos
        /// </summary>
        [HttpGet("EditModal/{id}")]
        public async Task<IActionResult> EditModal(int id)
        {
            var grupo = await _service.ObtenerPorIdAsync(id);
            if (grupo == null)
            {
                return NotFound(new { success = false, message = "Grupo de permisos no encontrado" });
            }
            return PartialView("_CreateEditModal", grupo);
        }

        /// <summary>
        /// Guarda un nuevo grupo de permisos
        /// </summary>
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] GrupoPermisoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            var (success, message) = await _service.GuardarAsync(dto);
            return Json(new { success, message });
        }

        /// <summary>
        /// Actualiza un grupo de permisos existente
        /// </summary>
        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] GrupoPermisoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            var (success, message) = await _service.EditarAsync(dto);
            return Json(new { success, message });
        }

        /// <summary>
        /// Modal de confirmación para eliminar
        /// </summary>
        [HttpGet("DeleteModal/{id}")]
        public async Task<IActionResult> DeleteModal(int id)
        {
            var grupo = await _service.ObtenerPorIdAsync(id);
            if (grupo == null)
            {
                return NotFound(new { success = false, message = "Grupo de permisos no encontrado" });
            }
            return PartialView("_DeleteModal", grupo);
        }

        /// <summary>
        /// Elimina un grupo de permisos
        /// </summary>
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _service.EliminarAsync(id);
            return Json(new { success, message });
        }
    }
}
