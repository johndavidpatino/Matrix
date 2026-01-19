using MatrixNext.Data.DTOs.PY;
using MatrixNext.Data.Services.PY;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.PY.Controllers;

[Area("PY")]
[Authorize]
public class DuplicarTrabajosController : Controller
{
    private readonly IDuplicarTrabajoService _service;
    private readonly ILogger<DuplicarTrabajosController> _logger;

    public DuplicarTrabajosController(
        IDuplicarTrabajoService service,
        ILogger<DuplicarTrabajosController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Abrir modal para duplicar trabajo
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(long idTrabajo, long idProyecto)
    {
        try
        {
            var viewModel = await _service.PrepararViewModelAsync(idTrabajo);
            viewModel.IdProyecto = idProyecto;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DuplicarModal", viewModel);
            }

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando vista para duplicar trabajo {IdTrabajo}", idTrabajo);
            return StatusCode(500, "Error al cargar el formulario");
        }
    }

    /// <summary>
    /// Ejecutar duplicación del trabajo
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Duplicar([FromBody] DuplicarTrabajoDto dto)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Datos inválidos" });
        }

        try
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var (success, message, idNuevo) = await _service.DuplicarTrabajoAsync(dto, userId);

            if (success)
            {
                _logger.LogInformation(
                    "Trabajo {IdOrigen} duplicado a {IdNuevo} por usuario {UserId}. Opciones: Docs={Docs}, Specs={Specs}, SumarMes={Mes}",
                    dto.IdTrabajoOrigen, idNuevo, userId, 
                    dto.DuplicarDocumentos, dto.DuplicarEspecificaciones, dto.SumarUnMes);
            }

            return Json(new { success, message, idNuevo });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error duplicando trabajo {IdTrabajo}", dto.IdTrabajoOrigen);
            return Json(new { success = false, message = "Error inesperado al duplicar el trabajo" });
        }
    }
}
