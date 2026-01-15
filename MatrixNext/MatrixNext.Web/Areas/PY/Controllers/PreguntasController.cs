using MatrixNext.Data.DTOs.PY.ControlCalidad;
using MatrixNext.Data.Services.PY.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.PY.Controllers;

/// <summary>
/// Controlador para gestiÃ³n de Preguntas de Control de Calidad.
/// Responsable de recibir requests, validar datos y coordinar con el servicio de preguntas.
/// </summary>
[Area("PY")]
[Route("api/py/[controller]")]
[ApiController]
[Authorize]
public class PreguntasController : ControllerBase
{
    private readonly IPreguntasService _service;
    private readonly ILogger<PreguntasController> _logger;

    public PreguntasController(IPreguntasService service, ILogger<PreguntasController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las preguntas activas para un tipo de proceso especÃ­fico.
    /// GET: /api/py/preguntas/{tipoProceso}
    /// </summary>
    /// <param name="tipoProceso">Tipo de proceso (1=Campo, 2=Moderadora, 3=Entrevistadora, 4=Transcripciones, 5=Informe)</param>
    /// <returns>Lista de preguntas activas filtrads por tipo</returns>
    [HttpGet("{tipoProceso}")]
    [AllowAnonymous]  // Preguntas pÃºblicas para cargar en formularios
    public async Task<IActionResult> ObtenerPorTipo(int tipoProceso)
    {
        try
        {
            _logger.LogInformation("Obteniendo preguntas para tipo {TipoProceso}", tipoProceso);

            // Validar tipo de proceso
            if (tipoProceso < 1 || tipoProceso > 5)
                return BadRequest(new { success = false, message = "Tipo de proceso invÃ¡lido" });

            var preguntas = await _service.ObtenerPorTipoProcesoAsync(tipoProceso);

            return Ok(new { success = true, data = preguntas });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo preguntas para tipo {TipoProceso}", tipoProceso);
            return StatusCode(500, new { success = false, message = "Error al obtener las preguntas" });
        }
    }

    /// <summary>
    /// Crea una nueva pregunta para control de calidad.
    /// POST: /api/py/preguntas/create
    /// </summary>
    /// <param name="dto">Datos de la pregunta a crear</param>
    /// <returns>ID de la nueva pregunta creada</returns>
    [HttpPost("create")]
    public async Task<IActionResult> Crear([FromBody] PreguntaInputDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos invÃ¡lidos", errors = ModelState.Values.SelectMany(v => v.Errors) });

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0)
                return Unauthorized(new { success = false, message = "Usuario no identificado" });

            _logger.LogInformation("Creando pregunta - Usuario: {UserId}, TipoProceso: {TipoProceso}", 
                userId, dto.TipoProceso);

            var (success, message, id) = await _service.CrearAsync(dto, userId);

            if (!success)
                return BadRequest(new { success = false, message });

            _logger.LogInformation("Pregunta {Id} creada exitosamente", id);
            return Ok(new { success = true, message, data = new { id } });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando pregunta - Dto: {@Dto}", dto);
            return StatusCode(500, new { success = false, message = "Error al crear la pregunta" });
        }
    }

    /// <summary>
    /// Edita una pregunta existente de control de calidad.
    /// POST: /api/py/preguntas/edit/{id}
    /// </summary>
    /// <param name="id">ID de la pregunta a editar</param>
    /// <param name="dto">Datos actualizados de la pregunta</param>
    /// <returns>ConfirmaciÃ³n de Ã©xito o error</returns>
    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Editar(long id, [FromBody] PreguntaInputDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos invÃ¡lidos" });

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0)
                return Unauthorized(new { success = false, message = "Usuario no identificado" });

            _logger.LogInformation("Editando pregunta {Id} - Usuario: {UserId}", id, userId);

            var (success, message) = await _service.EditarAsync(id, dto, userId);

            if (!success)
                return BadRequest(new { success = false, message });

            _logger.LogInformation("Pregunta {Id} editada exitosamente", id);
            return Ok(new { success = true, message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error editando pregunta {Id}", id);
            return StatusCode(500, new { success = false, message = "Error al editar la pregunta" });
        }
    }

    /// <summary>
    /// Activa o desactiva una pregunta (toggle del estado Activo).
    /// POST: /api/py/preguntas/toggle/{id}
    /// </summary>
    /// <param name="id">ID de la pregunta a activar/desactivar</param>
    /// <returns>Nuevo estado y confirmaciÃ³n de Ã©xito</returns>
    [HttpPost("toggle/{id}")]
    public async Task<IActionResult> Toggle(long id)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0)
                return Unauthorized(new { success = false, message = "Usuario no identificado" });

            _logger.LogInformation("Toggling pregunta {Id} - Usuario: {UserId}", id, userId);

            var (success, message, nuevoEstado) = await _service.ToggleActivoAsync(id, userId);

            if (!success)
                return BadRequest(new { success = false, message });

            _logger.LogInformation("Pregunta {Id} toggleada a {NuevoEstado}", id, nuevoEstado ? "activa" : "inactiva");
            return Ok(new { success = true, message, data = new { esActiva = nuevoEstado } });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling pregunta {Id}", id);
            return StatusCode(500, new { success = false, message = "Error al cambiar el estado de la pregunta" });
        }
    }
}

