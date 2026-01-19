using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using MatrixNext.Data.Modules.CU.Proyectos.Models;
using MatrixNext.Data.Modules.CU.Proyectos.Services;

namespace MatrixNext.Web.Areas.CU.Controllers;

/// <summary>
/// Controller para gestión de Proyectos
/// Migrado de: WebMatrix/CU_Cuentas/Proyectos.aspx
/// SP: PY_Proyectos_Get, PY_Proyecto_Add, PY_Proyectos_Edit
/// </summary>
[Area("CU")]
[Authorize]
public class ProyectosController : Controller
{
    private readonly IProyectoService _service;
    private readonly ILogger<ProyectosController> _logger;

    public ProyectosController(IProyectoService service, ILogger<ProyectosController> logger)
    {
        _service = service;
        _logger = logger;
    }

    private long GetCurrentUserId()
    {
        var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User?.FindFirst("Id")?.Value;
        return long.TryParse(idClaim, out var id) ? id : 0;
    }

    #region Vistas

    /// <summary>
    /// GET: /CU/Proyectos?estudioId=123
    /// Página principal - Lista de proyectos por estudio
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(long? estudioId)
    {
        if (!estudioId.HasValue)
        {
            TempData["ErrorMessage"] = "Debe seleccionar un estudio primero";
            return RedirectToAction("Index", "Estudios", new { area = "CU" });
        }

        var viewModel = await _service.PrepararViewModelAsync(estudioId.Value);
        return View(viewModel);
    }

    /// <summary>
    /// GET: /CU/Proyectos/CreateEdit?id=0&estudioId=123
    /// Modal para crear/editar proyecto
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> CreateEdit(long id = 0, long? estudioId = null)
    {
        ProyectoCreateEditDto dto;

        if (id > 0)
        {
            var proyecto = await _service.ObtenerProyectoPorIdAsync(id);
            if (proyecto == null)
            {
                return NotFound(new { success = false, message = "Proyecto no encontrado" });
            }

            dto = new ProyectoCreateEditDto
            {
                Id = proyecto.Id,
                JobBook = proyecto.JobBook,
                Nombre = proyecto.Nombre,
                UnidadId = proyecto.UnidadId,
                GerenteProyectos = proyecto.GerenteProyectos,
                EstudioId = proyecto.EstudioId,
                TipoProyectoId = proyecto.TipoProyectoId,
                A1 = proyecto.A1,
                A2 = proyecto.A2,
                A3 = proyecto.A3,
                A4 = proyecto.A4,
                A5 = proyecto.A5,
                A6 = proyecto.A6,
                A7 = proyecto.A7
            };
            estudioId = proyecto.EstudioId;
        }
        else
        {
            dto = new ProyectoCreateEditDto
            {
                EstudioId = estudioId ?? 0
            };
        }

        // Cargar catálogos
        var viewModel = await _service.PrepararViewModelAsync(dto.EstudioId);
        ViewBag.TiposProyecto = viewModel.TiposProyecto;
        ViewBag.Unidades = viewModel.Unidades;

        return PartialView("_CreateEdit", dto);
    }

    #endregion

    #region API AJAX

    /// <summary>
    /// GET: /CU/Proyectos/Buscar
    /// Buscar proyectos con filtros
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Buscar([FromQuery] ProyectoBusquedaParams filtros)
    {
        var proyectos = await _service.ObtenerProyectosAsync(filtros);
        return PartialView("_ListaProyectos", proyectos);
    }

    /// <summary>
    /// GET: /CU/Proyectos/Obtener/5
    /// Obtener un proyecto por ID
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Obtener(long id)
    {
        var proyecto = await _service.ObtenerProyectoPorIdAsync(id);
        if (proyecto == null)
        {
            return NotFound(new { success = false, message = "Proyecto no encontrado" });
        }
        return Json(new { success = true, data = proyecto });
    }

    /// <summary>
    /// POST: /CU/Proyectos/Guardar
    /// Guardar proyecto (crear o editar)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Guardar([FromForm] ProyectoCreateEditDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errores = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return Json(new { success = false, message = string.Join(", ", errores) });
        }

        if (dto.Id == 0)
        {
            // Crear
            var (success, message, id) = await _service.CrearProyectoAsync(dto);
            return Json(new { success, message, id });
        }
        else
        {
            // Editar
            var (success, message) = await _service.ActualizarProyectoAsync(dto);
            return Json(new { success, message });
        }
    }

    /// <summary>
    /// POST: /CU/Proyectos/ActualizarGerente
    /// Actualizar gerente de proyecto
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ActualizarGerente([FromForm] long id, [FromForm] long gerenteProyectos)
    {
        var (success, message) = await _service.ActualizarGerenteProyectoAsync(id, gerenteProyectos);
        return Json(new { success, message });
    }

    /// <summary>
    /// GET: /CU/Proyectos/TiposProyecto
    /// Obtener tipos de proyecto para combo
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> TiposProyecto()
    {
        var viewModel = await _service.PrepararViewModelAsync(0);
        return Json(viewModel.TiposProyecto);
    }

    /// <summary>
    /// GET: /CU/Proyectos/Unidades
    /// Obtener unidades para combo
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Unidades()
    {
        var viewModel = await _service.PrepararViewModelAsync(0);
        return Json(viewModel.Unidades);
    }

    #endregion
}
