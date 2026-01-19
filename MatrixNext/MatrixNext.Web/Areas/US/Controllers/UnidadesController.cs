using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MatrixNext.Data.Modules.US.Unidades.Models;
using MatrixNext.Data.Modules.US.Unidades.Services;
using MatrixNext.Data.Services.Usuarios;

namespace MatrixNext.Web.Areas.US.Controllers
{
    /// <summary>
    /// Controlador para Unidades
    /// Ref: WebMatrix/US_Usuarios/Unidades.aspx
    /// SP: US_Unidades_Get, US_Unidades_Add, US_Unidades_Edit, US_Unidades_Del
    /// </summary>
    [Area("US")]
    [Route("Usuarios/Unidades")]
    [Authorize]
    public class UnidadesController : Controller
    {
        private readonly IUnidadService _service;
        private readonly GrupoUnidadService _grupoUnidadService;
        private readonly ILogger<UnidadesController> _logger;

        public UnidadesController(
            IUnidadService service,
            GrupoUnidadService grupoUnidadService,
            ILogger<UnidadesController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _grupoUnidadService = grupoUnidadService ?? throw new ArgumentNullException(nameof(grupoUnidadService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var unidades = await _service.ObtenerTodosAsync();
            await CargarGruposUnidad();
            return View(unidades);
        }

        [HttpGet("Lista")]
        public async Task<IActionResult> Lista(string? filtro, int? idGrupoUnidad)
        {
            var unidades = await _service.ObtenerTodosAsync(filtro, idGrupoUnidad);
            return PartialView("_Lista", unidades);
        }

        [HttpGet("CreateModal")]
        public async Task<IActionResult> CreateModal()
        {
            await CargarGruposUnidad();
            return PartialView("_CreateEditModal", new UnidadDto());
        }

        [HttpGet("EditModal/{id}")]
        public async Task<IActionResult> EditModal(int id)
        {
            var unidad = await _service.ObtenerPorIdAsync(id);
            if (unidad == null)
            {
                return NotFound(new { success = false, message = "Unidad no encontrada" });
            }
            await CargarGruposUnidad();
            return PartialView("_CreateEditModal", unidad);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UnidadDto dto)
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
        public async Task<IActionResult> Edit([FromForm] UnidadDto dto)
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
            var unidad = await _service.ObtenerPorIdAsync(id);
            if (unidad == null)
            {
                return NotFound(new { success = false, message = "Unidad no encontrada" });
            }
            return PartialView("_DeleteModal", unidad);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _service.EliminarAsync(id);
            return Json(new { success, message });
        }

        private async Task CargarGruposUnidad()
        {
            var (success, _, grupos) = _grupoUnidadService.ObtenerTodos();
            if (success && grupos != null)
            {
                ViewBag.GruposUnidad = grupos.Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Nombre
                }).ToList();
            }
            else
            {
                ViewBag.GruposUnidad = new List<SelectListItem>();
            }
        }
    }
}
