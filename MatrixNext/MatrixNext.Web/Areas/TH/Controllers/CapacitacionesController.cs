using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Data.Modules.TH.Capacitaciones.Models;
using MatrixNext.Data.Modules.TH.Capacitaciones.Services;

namespace MatrixNext.Web.Areas.TH.Controllers;

/// <summary>
/// Controller para gestión de capacitaciones
/// Equivalente a WebMatrix: TH_TalentoHumano/Capacitacion.aspx
/// Permiso: 86
/// </summary>
[Area("TH")]
[Authorize]
[Route("TH/[controller]")]
public class CapacitacionesController : Controller
{
    private readonly ICapacitacionService _service;
    private readonly ILogger<CapacitacionesController> _logger;

    public CapacitacionesController(ICapacitacionService service, ILogger<CapacitacionesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    #region Vistas principales

    /// <summary>
    /// Vista principal de capacitaciones
    /// </summary>
    [HttpGet]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(long? trabajoId = null, long? capacitacionRefId = null)
    {
        ViewBag.TrabajoId = trabajoId;
        ViewBag.CapacitacionRefId = capacitacionRefId;
        ViewBag.Responsables = await _service.ObtenerResponsablesAsync();
        
        var capacitaciones = await _service.ObtenerCapacitacionesAsync(trabajoId);
        return View(capacitaciones);
    }

    /// <summary>
    /// Obtener lista de capacitaciones (AJAX)
    /// </summary>
    [HttpGet("Lista")]
    public async Task<IActionResult> Lista(long? trabajoId = null)
    {
        var capacitaciones = await _service.ObtenerCapacitacionesAsync(trabajoId);
        return PartialView("_Lista", capacitaciones);
    }

    #endregion

    #region CRUD Capacitaciones

    /// <summary>
    /// Modal para crear capacitación
    /// </summary>
    [HttpGet("Create")]
    public async Task<IActionResult> Create(long? trabajoId = null)
    {
        ViewBag.Responsables = await _service.ObtenerResponsablesAsync();
        ViewBag.TrabajoId = trabajoId;
        
        return PartialView("_CreateEditModal", new CapacitacionCreateEditDto
        {
            Fecha = DateTime.Today,
            TrabajoId = trabajoId
        });
    }

    /// <summary>
    /// Modal para editar capacitación
    /// </summary>
    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(long id)
    {
        var capacitacion = await _service.ObtenerCapacitacionPorIdAsync(id);
        if (capacitacion == null)
            return NotFound();

        ViewBag.Responsables = await _service.ObtenerResponsablesAsync();

        var dto = new CapacitacionCreateEditDto
        {
            Id = capacitacion.Id,
            Ubicacion = capacitacion.Ubicacion ?? string.Empty,
            Fecha = capacitacion.Fecha ?? DateTime.Today,
            Duracion = capacitacion.Duracion ?? 1,
            Actividad = capacitacion.Actividad ?? string.Empty,
            ResponsableId = capacitacion.ResponsableId ?? 0,
            Capacitador = capacitacion.Capacitador,
            ObjetivoActividad = capacitacion.ObjetivoActividad,
            ModoEvaluacion = capacitacion.ModoEvaluacion,
            TrabajoId = capacitacion.TrabajoId
        };

        return PartialView("_CreateEditModal", dto);
    }

    /// <summary>
    /// Guardar capacitación (crear o editar)
    /// </summary>
    [HttpPost("Save")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save([FromForm] CapacitacionCreateEditDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return Json(new { success = false, message = string.Join(", ", errors) });
        }

        var (success, message, id) = await _service.GuardarCapacitacionAsync(dto);
        return Json(new { success, message, id });
    }

    /// <summary>
    /// Modal de confirmación para eliminar
    /// </summary>
    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var capacitacion = await _service.ObtenerCapacitacionPorIdAsync(id);
        if (capacitacion == null)
            return NotFound();

        return PartialView("_DeleteModal", capacitacion);
    }

    /// <summary>
    /// Confirmar eliminación
    /// </summary>
    [HttpPost("DeleteConfirm/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirm(long id)
    {
        var (success, message) = await _service.EliminarCapacitacionAsync(id);
        return Json(new { success, message });
    }

    /// <summary>
    /// Crear refuerzo de capacitación
    /// </summary>
    [HttpPost("CrearRefuerzo/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CrearRefuerzo(long id)
    {
        var (success, message, nuevoId) = await _service.CrearRefuerzoAsync(id);
        return Json(new { success, message, id = nuevoId });
    }

    #endregion

    #region Participantes

    /// <summary>
    /// Vista de participantes de una capacitación
    /// </summary>
    [HttpGet("Participantes/{capacitacionId}")]
    public async Task<IActionResult> Participantes(long capacitacionId)
    {
        var capacitacion = await _service.ObtenerCapacitacionPorIdAsync(capacitacionId);
        if (capacitacion == null)
            return NotFound();

        ViewBag.Capacitacion = capacitacion;
        var participantes = await _service.ObtenerParticipantesAsync(capacitacionId);
        return PartialView("_Participantes", participantes);
    }

    /// <summary>
    /// Modal para agregar participante
    /// </summary>
    [HttpGet("AgregarParticipante/{capacitacionId}")]
    public IActionResult AgregarParticipante(long capacitacionId)
    {
        return PartialView("_AgregarParticipanteModal", new CapacitacionParticipanteCreateDto
        {
            CapacitacionId = capacitacionId
        });
    }

    /// <summary>
    /// Buscar personas disponibles (AJAX)
    /// </summary>
    [HttpGet("BuscarPersonas")]
    public async Task<IActionResult> BuscarPersonas([FromQuery] BuscarPersonasCapacitacionParams parametros)
    {
        var personas = await _service.BuscarPersonasAsync(parametros);
        return Json(personas);
    }

    /// <summary>
    /// Guardar participante
    /// </summary>
    [HttpPost("GuardarParticipante")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GuardarParticipante([FromForm] CapacitacionParticipanteCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Datos inválidos" });
        }

        var (success, message) = await _service.AgregarParticipanteAsync(dto);
        return Json(new { success, message });
    }

    /// <summary>
    /// Actualizar participante (evaluación)
    /// </summary>
    [HttpPost("ActualizarParticipante")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActualizarParticipante([FromForm] CapacitacionParticipanteUpdateDto dto)
    {
        var (success, message) = await _service.ActualizarParticipanteAsync(dto);
        return Json(new { success, message });
    }

    /// <summary>
    /// Eliminar participante
    /// </summary>
    [HttpPost("EliminarParticipante/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarParticipante(long id)
    {
        var (success, message) = await _service.EliminarParticipanteAsync(id);
        return Json(new { success, message });
    }

    #endregion
}
