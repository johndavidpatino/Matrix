using MatrixNext.Data.Services.IT;
using MatrixNext.Data.Models.IT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.IT.Controllers;

[Area("IT")]
[Authorize] // TODO: Agregar política específica para permisos 133/134
public class SyncIssuesController : Controller
{
    private readonly IITSyncService _service;
    private readonly ILogger<SyncIssuesController> _logger;

    public SyncIssuesController(IITSyncService service, ILogger<SyncIssuesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Vista principal de resolución de problemas de sincronización
    /// Migrado desde: WebMatrix/IT/SyncIssues.aspx
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }

    #region Acordeón 0: Ajustar trabajos

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> QuitarEntrenamiento([FromForm] long trabajoId)
    {
        if (trabajoId <= 0)
        {
            return Json(new { success = false, message = "ID de trabajo inválido" });
        }

        var userId = GetUserId();
        var (success, message) = await _service.QuitarPreguntasEntrenamientoAsync(trabajoId, userId);

        return Json(new { success, message });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> QuitarSupervision([FromForm] long trabajoId)
    {
        if (trabajoId <= 0)
        {
            return Json(new { success = false, message = "ID de trabajo inválido" });
        }

        var userId = GetUserId();
        var (success, message) = await _service.ErrorTrabajoEspecializadoAsync(trabajoId, userId);

        return Json(new { success, message });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HabilitarSincronizacion([FromForm] long trabajoId)
    {
        if (trabajoId <= 0)
        {
            return Json(new { success = false, message = "ID de trabajo inválido" });
        }

        var userId = GetUserId();
        var (success, message) = await _service.HabilitarSincronizacionAsync(trabajoId, userId);

        return Json(new { success, message });
    }

    #endregion

    #region Acordeón 1: Actualizar preguntas

    [HttpPost]
    public async Task<IActionResult> BuscarPreguntas([FromForm] long trabajoId)
    {
        if (trabajoId <= 0)
        {
            return Json(new { success = false, message = "ID de trabajo inválido", preguntas = Array.Empty<object>() });
        }

        try
        {
            var preguntas = await _service.ObtenerPreguntasAsync(trabajoId);
            var preguntasDto = preguntas.Select(p => new
            {
                value = p.DCPDescripcion,
                text = p.PrNombre
            });

            return Json(new { success = true, preguntas = preguntasDto });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error buscando preguntas para trabajo {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error al buscar preguntas", preguntas = Array.Empty<object>() });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActualizarRespuesta([FromForm] SyncActualizarRespuestaDto dto)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Datos inválidos" });
        }

        if (dto.SbjNum <= 0)
        {
            return Json(new { success = false, message = "Digite el SbjNum correctamente" });
        }

        if (string.IsNullOrWhiteSpace(dto.NuevoValor))
        {
            return Json(new { success = false, message = "Debe escribir el nuevo valor" });
        }

        var userId = GetUserId();
        var (success, message) = await _service.ActualizarRespuestaAsync(dto, userId);

        return Json(new { success, message });
    }

    #endregion

    #region Acordeón 2 y 3: Encuestas piloto

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HabilitarPiloto([FromForm] decimal sbjNum)
    {
        if (sbjNum <= 0)
        {
            return Json(new { success = false, message = "SbjNum inválido" });
        }

        var userId = GetUserId();
        var (success, message) = await _service.HabilitarEncuestaPilotoAsync(sbjNum, userId);

        return Json(new { success, message });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EncuestaPiloto([FromForm] decimal sbjNum)
    {
        if (sbjNum <= 0)
        {
            return Json(new { success = false, message = "SbjNum inválido" });
        }

        var userId = GetUserId();
        var (success, message) = await _service.EncuestaPilotoAsync(sbjNum, userId);

        return Json(new { success, message });
    }

    #endregion

    #region Helpers

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    #endregion
}
