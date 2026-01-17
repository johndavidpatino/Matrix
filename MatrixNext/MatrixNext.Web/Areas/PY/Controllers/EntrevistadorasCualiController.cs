using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.PY;
using MatrixNext.Web.Models.PY;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.PY.Controllers;

/// <summary>
/// T4.15: Controller para gestión de entrevistadoras cualitativas
/// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 4.15
/// </summary>
[Area("PY")]
[Authorize(Roles = "Coordinador,Administrador")]
[Route("api/[area]/[controller]")]
[ApiController]
public class EntrevistadorasCualiController : ControllerBase
{
    private readonly IEntrevistadorasCualiService _entrevistadorasService;
    private readonly ILogger<EntrevistadorasCualiController> _logger;

    public EntrevistadorasCualiController(
        IEntrevistadorasCualiService entrevistadorasService,
        ILogger<EntrevistadorasCualiController> logger)
    {
        _entrevistadorasService = entrevistadorasService;
        _logger = logger;
    }

    private long ObtenerIdUsuarioActual()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    /// <summary>
    /// Obtiene todas las entrevistadoras de un trabajo cualitativo.
    /// </summary>
    [HttpGet("obtener-por-trabajo/{idTrabajoCuali}")]
    public async Task<IActionResult> ObtenerPorTrabajo(long idTrabajoCuali)
    {
        try
        {
            var entrevistadoras = await _entrevistadorasService.ObtenerPorTrabajoAsync(idTrabajoCuali);
            return Ok(new
            {
                exitoso = true,
                datos = entrevistadoras,
                mensaje = "Entrevistadoras obtenidas correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener entrevistadoras del trabajo {idTrabajoCuali}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al procesar la solicitud. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Obtiene entrevistadoras por segmento.
    /// </summary>
    [HttpGet("obtener-por-segmento/{idSegmento}")]
    public async Task<IActionResult> ObtenerPorSegmento(long idSegmento)
    {
        try
        {
            var entrevistadoras = await _entrevistadorasService.ObtenerPorSegmentoAsync(idSegmento);
            return Ok(new
            {
                exitoso = true,
                datos = entrevistadoras,
                mensaje = "Entrevistadoras obtenidas correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener entrevistadoras del segmento {idSegmento}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al procesar la solicitud. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Obtiene entrevistadoras disponibles.
    /// </summary>
    [HttpGet("obtener-disponibles")]
    public async Task<IActionResult> ObtenerDisponibles()
    {
        try
        {
            var entrevistadoras = await _entrevistadorasService.ObtenerDisponiblesAsync();
            return Ok(new
            {
                exitoso = true,
                datos = entrevistadoras,
                mensaje = "Entrevistadoras disponibles obtenidas correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener entrevistadoras disponibles");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al procesar la solicitud. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Obtiene una entrevistadora por ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(long id)
    {
        try
        {
            var entrevistadora = await _entrevistadorasService.ObtenerPorIdAsync(id);
            if (entrevistadora == null)
            {
                return NotFound(new
                {
                    exitoso = false,
                    mensaje = "Entrevistadora no encontrada"
                });
            }

            return Ok(new
            {
                exitoso = true,
                datos = entrevistadora,
                mensaje = "Entrevistadora obtenida correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener entrevistadora {id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al procesar la solicitud. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Crea una nueva entrevistadora.
    /// </summary>
    [HttpPost("crear")]
    public async Task<IActionResult> Crear([FromBody] EntrevistadorasCuali entrevistadora)
    {
        try
        {
            var resultado = await _entrevistadorasService.CrearAsync(entrevistadora, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear entrevistadora");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al procesar la solicitud. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Actualiza una entrevistadora existente.
    /// </summary>
    [HttpPut("actualizar")]
    public async Task<IActionResult> Actualizar([FromBody] EntrevistadorasCuali entrevistadora)
    {
        try
        {
            var resultado = await _entrevistadorasService.ActualizarAsync(entrevistadora, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar entrevistadora {entrevistadora.Id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al procesar la solicitud. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Cambia la disponibilidad de una entrevistadora.
    /// </summary>
    [HttpPost("cambiar-disponibilidad")]
    public async Task<IActionResult> CambiarDisponibilidad(
        [FromQuery] long idEntrevistadora,
        [FromQuery] string nuevaDisponibilidad)
    {
        try
        {
            var resultado = await _entrevistadorasService.CambiarDisponibilidadAsync(idEntrevistadora, nuevaDisponibilidad, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al cambiar disponibilidad de la entrevistadora {idEntrevistadora}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al procesar la solicitud. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Elimina lógicamente una entrevistadora.
    /// </summary>
    [HttpDelete("eliminar/{id}")]
    public async Task<IActionResult> Eliminar(long id)
    {
        try
        {
            var resultado = await _entrevistadorasService.EliminarAsync(id, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al eliminar entrevistadora {id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al procesar la solicitud. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Actualiza el porcentaje de cumplimiento de una entrevistadora.
    /// </summary>
    [HttpPost("actualizar-porcentaje-cumplimiento/{id}")]
    public async Task<IActionResult> ActualizarPorcentajeCumplimiento(long id)
    {
        try
        {
            var resultado = await _entrevistadorasService.ActualizarPorcentajeCumplimientoAsync(id);
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar porcentaje de cumplimiento de entrevistadora {id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al procesar la solicitud. Por favor intente nuevamente."
            });
        }
    }
}
