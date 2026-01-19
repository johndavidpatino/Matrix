using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MatrixNext.Data.Modules.US.TipoGrupoUnidad.Models;
using MatrixNext.Data.Modules.US.TipoGrupoUnidad.Services;

namespace MatrixNext.Web.Areas.US.Controllers
{
    /// <summary>
    /// Controlador para Tipos de Grupo de Unidad
    /// Ref: WebMatrix/US_Usuarios/TipoGrupoUnidad.aspx - Permiso 89
    /// SP: US_TipoGrupoUnidad_Get, US_TipoGrupoUnidad_Add, US_TipoGrupoUnidad_Edit, US_TipoGrupoUnidad_Del
    /// </summary>
    [Area("US")]
    [Route("Usuarios/TipoGrupoUnidad")]
    [Authorize]
    public class TipoGrupoUnidadController : Controller
    {
        private readonly ITipoGrupoUnidadService _service;
        private readonly ILogger<TipoGrupoUnidadController> _logger;

        public TipoGrupoUnidadController(ITipoGrupoUnidadService service, ILogger<TipoGrupoUnidadController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var tipos = await _service.ObtenerTodosAsync();
            return View(tipos);
        }

        [HttpGet("Lista")]
        public async Task<IActionResult> Lista(string? filtro)
        {
            var tipos = await _service.ObtenerTodosAsync(filtro);
            return PartialView("_Lista", tipos);
        }

        [HttpGet("CreateModal")]
        public IActionResult CreateModal()
        {
            return PartialView("_CreateEditModal", new TipoGrupoUnidadDto());
        }

        [HttpGet("EditModal/{id}")]
        public async Task<IActionResult> EditModal(int id)
        {
            var tipo = await _service.ObtenerPorIdAsync(id);
            if (tipo == null)
            {
                return NotFound(new { success = false, message = "Tipo de grupo de unidad no encontrado" });
            }
            return PartialView("_CreateEditModal", tipo);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] TipoGrupoUnidadDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            var (success, message) = await _service.GuardarAsync(dto);
            return Json(new { success, message });
        }

        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] TipoGrupoUnidadDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            var (success, message) = await _service.EditarAsync(dto);
            return Json(new { success, message });
        }

        [HttpGet("DeleteModal/{id}")]
        public async Task<IActionResult> DeleteModal(int id)
        {
            var tipo = await _service.ObtenerPorIdAsync(id);
            if (tipo == null)
            {
                return NotFound(new { success = false, message = "Tipo de grupo de unidad no encontrado" });
            }
            return PartialView("_DeleteModal", tipo);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _service.EliminarAsync(id);
            return Json(new { success, message });
        }
    }
}
