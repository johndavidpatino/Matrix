using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.PY;
using MatrixNext.Web.Models.PY;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.PY.Controllers;

/// <summary>
/// T4.11: Controller para gestión de trabajos cualitativos
/// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 4.11
/// </summary>
[Area("PY")]
[Authorize(Roles = "Coordinador,Administrador")]
[Route("api/[area]/[controller]")]
[ApiController]
public class TrabajosCualiController : ControllerBase
{
    private readonly ITrabajosCualiService _trabajosService;
    private readonly ILogger<TrabajosCualiController> _logger;

    public TrabajosCualiController(
        ITrabajosCualiService trabajosService,
        ILogger<TrabajosCualiController> logger)
    {
        _trabajosService = trabajosService;
        _logger = logger;
    }

    private long ObtenerIdUsuarioActual()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    /// <summary>
    /// Obtiene todos los trabajos cualitativos de un proyecto.
    /// </summary>
    [HttpGet("obtener-por-proyecto/{idProyecto}")]
    public async Task<IActionResult> ObtenerPorProyecto(long idProyecto)
    {
        try
        {
            var trabajos = await _trabajosService.ObtenerPorProyectoAsync(idProyecto);
            return Ok(new
            {
                exitoso = true,
                datos = trabajos,
                mensaje = "Trabajos cualitativos obtenidos correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener trabajos cualitativos del proyecto {idProyecto}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al obtener trabajos cualitativos. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Obtiene un trabajo cualitativo por ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(long id)
    {
        try
        {
            var trabajo = await _trabajosService.ObtenerPorIdAsync(id);
            if (trabajo == null)
            {
                return NotFound(new
                {
                    exitoso = false,
                    mensaje = "Trabajo cualitativo no encontrado"
                });
            }

            return Ok(new
            {
                exitoso = true,
                datos = trabajo,
                mensaje = "Trabajo cualitativo obtenido correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener trabajo cualitativo {id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al obtener trabajo cualitativo. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Obtiene trabajos cualitativos por estado.
    /// </summary>
    [HttpGet("obtener-por-estado/{estado}")]
    public async Task<IActionResult> ObtenerPorEstado(string estado)
    {
        try
        {
            var trabajos = await _trabajosService.ObtenerPorEstadoAsync(estado);
            return Ok(new
            {
                exitoso = true,
                datos = trabajos,
                mensaje = "Trabajos cualitativos obtenidos correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener trabajos cualitativos por estado {estado}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al obtener trabajos cualitativos por estado. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Obtiene trabajos cualitativos por coordinador.
    /// </summary>
    [HttpGet("obtener-por-coordinador/{idCoordinador}")]
    public async Task<IActionResult> ObtenerPorCoordinador(long idCoordinador)
    {
        try
        {
            var trabajos = await _trabajosService.ObtenerPorCoordinadorAsync(idCoordinador);
            return Ok(new
            {
                exitoso = true,
                datos = trabajos,
                mensaje = "Trabajos cualitativos obtenidos correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener trabajos cualitativos del coordinador {idCoordinador}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al obtener trabajos cualitativos del coordinador. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Crea un nuevo trabajo cualitativo.
    /// </summary>
    [HttpPost("crear")]
    public async Task<IActionResult> Crear([FromBody] TrabajosCuali trabajo)
    {
        try
        {
            var resultado = await _trabajosService.CrearAsync(trabajo, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear trabajo cualitativo");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al crear trabajo cualitativo. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Actualiza un trabajo cualitativo existente.
    /// </summary>
    [HttpPut("actualizar")]
    public async Task<IActionResult> Actualizar([FromBody] TrabajosCuali trabajo)
    {
        try
        {
            var resultado = await _trabajosService.ActualizarAsync(trabajo, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar trabajo cualitativo {trabajo.Id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al actualizar trabajo cualitativo. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Cambia el estado de un trabajo cualitativo.
    /// </summary>
    [HttpPost("cambiar-estado")]
    public async Task<IActionResult> CambiarEstado(
        [FromQuery] long idTrabajo,
        [FromQuery] string nuevoEstado,
        [FromQuery] string? observacion = null)
    {
        try
        {
            var resultado = await _trabajosService.CambiarEstadoAsync(idTrabajo, nuevoEstado, ObtenerIdUsuarioActual(), observacion);
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al cambiar estado del trabajo {idTrabajo}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al cambiar estado del trabajo. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Elimina lógicamente un trabajo cualitativo.
    /// </summary>
    [HttpDelete("eliminar/{id}")]
    public async Task<IActionResult> Eliminar(long id)
    {
        try
        {
            var resultado = await _trabajosService.EliminarAsync(id, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al eliminar trabajo cualitativo {id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al eliminar trabajo cualitativo. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Duplica un trabajo cualitativo con sus segmentos.
    /// </summary>
    [HttpPost("duplicar/{id}")]
    public async Task<IActionResult> Duplicar(long id, [FromQuery] string nuevoNombre)
    {
        try
        {
            var resultado = await _trabajosService.DuplicarAsync(id, nuevoNombre, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al duplicar trabajo cualitativo {id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al duplicar trabajo cualitativo. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Valida si un trabajo puede ser eliminado.
    /// </summary>
    [HttpGet("validar-eliminacion/{id}")]
    public async Task<IActionResult> ValidarEliminacion(long id)
    {
        try
        {
            var resultado = await _trabajosService.ValidarEliminacionAsync(id);
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al validar eliminación del trabajo {id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al validar eliminación del trabajo. Por favor intente nuevamente."
            });
        }
    }
}
