using MatrixNext.Core.DTOs.PY.ControlCalidad;
using MatrixNext.Core.Interfaces.PY.ControlCalidad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.PY.Controllers;

/// <summary>
/// Controlador para gestión de Control de Calidad.
/// Responsable de recibir requests, validar datos y coordinar con el servicio de negocio.
/// </summary>
[Area("PY")]
[Route("api/py/[controller]")]
[ApiController]
[Authorize]
public class ControlCalidadController : ControllerBase
{
    private readonly IControlCalidadService _service;
    private readonly ILogger<ControlCalidadController> _logger;

    public ControlCalidadController(IControlCalidadService service, ILogger<ControlCalidadController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los registros de control de calidad para un tipo de proceso.
    /// GET: /api/py/controlcalidad/{tipoProceso}
    /// </summary>
    /// <param name="tipoProceso">Tipo de proceso (1=Campo, 2=Moderadora, 3=Entrevistadora, 4=Transcripciones, 5=Informe)</param>
    /// <returns>Lista de controles de calidad con detalles básicos</returns>
    [HttpGet("{tipoProceso}")]
    public async Task<IActionResult> ObtenerPorTipo(int tipoProceso)
    {
        try
        {
            _logger.LogInformation("Obteniendo controles de calidad para tipo {TipoProceso} - Usuario: {UserId}", 
                tipoProceso, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Validar tipo de proceso
            if (tipoProceso < 1 || tipoProceso > 5)
                return BadRequest(new { success = false, message = "Tipo de proceso inválido" });

            var controles = await _service.ObtenerTodosAsync(tipoProceso);

            return Ok(new { success = true, data = controles });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo controles de calidad para tipo {TipoProceso}", tipoProceso);
            return StatusCode(500, new { success = false, message = "Error al obtener los controles de calidad" });
        }
    }

    /// <summary>
    /// Obtiene los detalles completos de un registro de control de calidad.
    /// GET: /api/py/controlcalidad/details/{id}
    /// </summary>
    /// <param name="id">ID del control de calidad</param>
    /// <returns>Detalle completo del control de calidad con todas las preguntas respondidas</returns>
    [HttpGet("details/{id}")]
    public async Task<IActionResult> ObtenerDetalle(long id)
    {
        try
        {
            _logger.LogInformation("Obteniendo detalles de control {Id} - Usuario: {UserId}", 
                id, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var detalle = await _service.ObtenerPorIdAsync(id);
            if (detalle == null)
                return NotFound(new { success = false, message = "Control de calidad no encontrado" });

            return Ok(new { success = true, data = detalle });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo detalles del control {Id}", id);
            return StatusCode(500, new { success = false, message = "Error al obtener los detalles del control" });
        }
    }

    /// <summary>
    /// Crea un nuevo registro de control de calidad con sus respuestas.
    /// POST: /api/py/controlcalidad/create
    /// </summary>
    /// <param name="dto">Datos del control de calidad a crear</param>
    /// <returns>ID del nuevo control de calidad creado</returns>
    [HttpPost("create")]
    public async Task<IActionResult> Crear([FromBody] ControlCalidadInputDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos inválidos", errors = ModelState.Values.SelectMany(v => v.Errors) });

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0)
                return Unauthorized(new { success = false, message = "Usuario no identificado" });

            _logger.LogInformation("Creando control de calidad - Usuario: {UserId}, TrabajoProceso: {TrabajoProcesoId}", 
                userId, dto.TrabajoProcesoId);

            var (success, message, id) = await _service.CrearAsync(dto, userId);

            if (!success)
                return BadRequest(new { success = false, message });

            _logger.LogInformation("Control de calidad {Id} creado exitosamente", id);
            return Ok(new { success = true, message, data = new { id } });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando control de calidad - Dto: {@Dto}", dto);
            return StatusCode(500, new { success = false, message = "Error al crear el control de calidad" });
        }
    }

    /// <summary>
    /// Edita un registro existente de control de calidad y sus respuestas.
    /// POST: /api/py/controlcalidad/edit/{id}
    /// </summary>
    /// <param name="id">ID del control de calidad a editar</param>
    /// <param name="dto">Datos actualizados del control</param>
    /// <returns>Confirmación de éxito o error</returns>
    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Editar(long id, [FromBody] ControlCalidadInputDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos inválidos" });

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0)
                return Unauthorized(new { success = false, message = "Usuario no identificado" });

            _logger.LogInformation("Editando control de calidad {Id} - Usuario: {UserId}", id, userId);

            var (success, message) = await _service.EditarAsync(id, dto, userId);

            if (!success)
                return BadRequest(new { success = false, message });

            _logger.LogInformation("Control de calidad {Id} editado exitosamente", id);
            return Ok(new { success = true, message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error editando control de calidad {Id}", id);
            return StatusCode(500, new { success = false, message = "Error al editar el control de calidad" });
        }
    }

    /// <summary>
    /// Elimina un registro de control de calidad y todos sus detalles asociados.
    /// POST: /api/py/controlcalidad/delete/{id}
    /// </summary>
    /// <param name="id">ID del control de calidad a eliminar</param>
    /// <returns>Confirmación de éxito o error</returns>
    [HttpPost("delete/{id}")]
    public async Task<IActionResult> Eliminar(long id)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0)
                return Unauthorized(new { success = false, message = "Usuario no identificado" });

            _logger.LogInformation("Eliminando control de calidad {Id} - Usuario: {UserId}", id, userId);

            var (success, message) = await _service.EliminarAsync(id, userId);

            if (!success)
                return BadRequest(new { success = false, message });

            _logger.LogInformation("Control de calidad {Id} eliminado exitosamente", id);
            return Ok(new { success = true, message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando control de calidad {Id}", id);
            return StatusCode(500, new { success = false, message = "Error al eliminar el control de calidad" });
        }
    }

    /// <summary>
    /// Obtiene las preguntas activas para un tipo de proceso específico.
    /// GET: /api/py/controlcalidad/preguntas/{tipoProceso}
    /// </summary>
    /// <param name="tipoProceso">Tipo de proceso (1=Campo, 2=Moderadora, 3=Entrevistadora, 4=Transcripciones, 5=Informe)</param>
    /// <returns>Lista de preguntas activas para el proceso</returns>
    [HttpGet("preguntas/{tipoProceso}")]
    public async Task<IActionResult> ObtenerPreguntasParaTipo(int tipoProceso)
    {
        try
        {
            _logger.LogInformation("Obteniendo preguntas para tipo {TipoProceso}", tipoProceso);

            // Validar tipo de proceso
            if (tipoProceso < 1 || tipoProceso > 5)
                return BadRequest(new { success = false, message = "Tipo de proceso inválido" });

            var preguntas = await _service.ObtenerPreguntasActivasAsync(tipoProceso);

            return Ok(new { success = true, data = preguntas });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo preguntas para tipo {TipoProceso}", tipoProceso);
            return StatusCode(500, new { success = false, message = "Error al obtener las preguntas" });
        }
    }
}
