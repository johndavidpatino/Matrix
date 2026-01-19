using MatrixNext.Data.DTOs.PY;
using MatrixNext.Data.Services.PY;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.PY.Controllers;

[Area("PY")]
[Authorize]
public class VariablesControlController : Controller
{
    private readonly IVariablesControlService _service;
    private readonly ILogger<VariablesControlController> _logger;

    public VariablesControlController(
        IVariablesControlService service,
        ILogger<VariablesControlController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Vista principal de variables de control para un trabajo
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(long idTrabajo, string? tipoEvaluado = null)
    {
        try
        {
            var viewModel = await _service.PrepararViewModelAsync(idTrabajo, tipoEvaluado);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando variables de control para trabajo {IdTrabajo}", idTrabajo);
            TempData["Error"] = "Error al cargar las variables de control";
            return RedirectToAction("Index", "Trabajos", new { area = "PY" });
        }
    }

    /// <summary>
    /// Guardar nueva variable de control
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Guardar([FromBody] VariableControlDto dto)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Datos inválidos" });
        }

        try
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var (success, message, id) = await _service.CrearVariableControlAsync(dto, userId);

            if (success)
            {
                _logger.LogInformation("Variable de control {Id} creada para trabajo {IdTrabajo} por usuario {UserId}",
                    id, dto.IdTrabajo, userId);
            }

            return Json(new { success, message, id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error guardando variable de control para trabajo {IdTrabajo}", dto.IdTrabajo);
            return Json(new { success = false, message = "Error al guardar la variable de control" });
        }
    }

    /// <summary>
    /// Obtener lista de variables por trabajo (AJAX)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListarPorTrabajo(long idTrabajo, string? tipoEvaluado = null)
    {
        try
        {
            var variables = await _service.ObtenerVariablesControlPorTrabajoAsync(idTrabajo, tipoEvaluado);
            return Json(variables);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listando variables para trabajo {IdTrabajo}", idTrabajo);
            return Json(new { error = "Error al cargar las variables" });
        }
    }

    /// <summary>
    /// Modal para ver detalle de variable de control
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Detalle(long id)
    {
        try
        {
            var variable = await _service.ObtenerVariableControlAsync(id);
            
            if (variable == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Detalle", variable);
            }

            return View(variable);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo detalle de variable {Id}", id);
            return StatusCode(500, "Error al obtener el detalle");
        }
    }
}
