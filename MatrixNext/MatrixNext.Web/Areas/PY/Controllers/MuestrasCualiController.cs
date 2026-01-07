using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.PY;
using MatrixNext.Web.Models.PY;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.PY.Controllers;

/// <summary>
/// T4.14: Controller para gestión de muestras cualitativas
/// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 4.14
/// </summary>
[Area("PY")]
[Authorize(Roles = "Coordinador,Administrador")]
[Route("api/[area]/[controller]")]
[ApiController]
public class MuestrasCualiController : ControllerBase
{
    private readonly IMuestrasCualiService _muestrasService;
    private readonly ILogger<MuestrasCualiController> _logger;

    public MuestrasCualiController(
        IMuestrasCualiService muestrasService,
        ILogger<MuestrasCualiController> logger)
    {
        _muestrasService = muestrasService;
        _logger = logger;
    }

    private long ObtenerIdUsuarioActual()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    /// <summary>
    /// Obtiene todas las muestras de un trabajo cualitativo.
    /// </summary>
    [HttpGet("obtener-por-trabajo/{idTrabajoCuali}")]
    public async Task<IActionResult> ObtenerPorTrabajo(long idTrabajoCuali)
    {
        try
        {
            var muestras = await _muestrasService.ObtenerPorTrabajoAsync(idTrabajoCuali);
            return Ok(new
            {
                exitoso = true,
                datos = muestras,
                mensaje = "Muestras obtenidas correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener muestras del trabajo {idTrabajoCuali}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Obtiene muestras por segmento.
    /// </summary>
    [HttpGet("obtener-por-segmento/{idSegmento}")]
    public async Task<IActionResult> ObtenerPorSegmento(long idSegmento)
    {
        try
        {
            var muestras = await _muestrasService.ObtenerPorSegmentoAsync(idSegmento);
            return Ok(new
            {
                exitoso = true,
                datos = muestras,
                mensaje = "Muestras obtenidas correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener muestras del segmento {idSegmento}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Obtiene muestras por estado.
    /// </summary>
    [HttpGet("obtener-por-estado/{estado}")]
    public async Task<IActionResult> ObtenerPorEstado(string estado)
    {
        try
        {
            var muestras = await _muestrasService.ObtenerPorEstadoAsync(estado);
            return Ok(new
            {
                exitoso = true,
                datos = muestras,
                mensaje = "Muestras obtenidas correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener muestras por estado {estado}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Obtiene una muestra por ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(long id)
    {
        try
        {
            var muestra = await _muestrasService.ObtenerPorIdAsync(id);
            if (muestra == null)
            {
                return NotFound(new
                {
                    exitoso = false,
                    mensaje = "Muestra no encontrada"
                });
            }

            return Ok(new
            {
                exitoso = true,
                datos = muestra,
                mensaje = "Muestra obtenida correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener muestra {id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Crea una nueva muestra.
    /// </summary>
    [HttpPost("crear")]
    public async Task<IActionResult> Crear([FromBody] MuestrasCuali muestra)
    {
        try
        {
            var resultado = await _muestrasService.CrearAsync(muestra, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear muestra");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Actualiza una muestra existente.
    /// </summary>
    [HttpPut("actualizar")]
    public async Task<IActionResult> Actualizar([FromBody] MuestrasCuali muestra)
    {
        try
        {
            var resultado = await _muestrasService.ActualizarAsync(muestra, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar muestra {muestra.Id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Cambia el estado de una muestra.
    /// </summary>
    [HttpPost("cambiar-estado")]
    public async Task<IActionResult> CambiarEstado(
        [FromQuery] long idMuestra,
        [FromQuery] string nuevoEstado)
    {
        try
        {
            var resultado = await _muestrasService.CambiarEstadoAsync(idMuestra, nuevoEstado, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al cambiar estado de la muestra {idMuestra}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Elimina lógicamente una muestra.
    /// </summary>
    [HttpDelete("eliminar/{id}")]
    public async Task<IActionResult> Eliminar(long id)
    {
        try
        {
            var resultado = await _muestrasService.EliminarAsync(id, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al eliminar muestra {id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Asigna un entrevistador a una muestra.
    /// </summary>
    [HttpPost("asignar-entrevistador")]
    public async Task<IActionResult> AsignarEntrevistador(
        [FromQuery] long idMuestra,
        [FromQuery] long idEntrevistador)
    {
        try
        {
            var resultado = await _muestrasService.AsignarEntrevistadorAsync(idMuestra, idEntrevistador, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al asignar entrevistador {idEntrevistador} a muestra {idMuestra}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }
}
