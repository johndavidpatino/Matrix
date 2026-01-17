using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.PY;
using MatrixNext.Web.Models.PY;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.PY.Controllers;

/// <summary>
/// T4.13: Controller para gestión de sesiones cualitativas
/// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 4.13
/// </summary>
[Area("PY")]
[Authorize(Roles = "Coordinador,Administrador")]
[Route("api/[area]/[controller]")]
[ApiController]
public class SesionesCualiController : ControllerBase
{
    private readonly ISesionesCualiService _sesionesService;
    private readonly ILogger<SesionesCualiController> _logger;

    public SesionesCualiController(
        ISesionesCualiService sesionesService,
        ILogger<SesionesCualiController> logger)
    {
        _sesionesService = sesionesService;
        _logger = logger;
    }

    private long ObtenerIdUsuarioActual()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    /// <summary>
    /// Obtiene todas las sesiones de un trabajo cualitativo.
    /// </summary>
    [HttpGet("obtener-por-trabajo/{idTrabajoCuali}")]
    public async Task<IActionResult> ObtenerPorTrabajo(long idTrabajoCuali)
    {
        try
        {
            var sesiones = await _sesionesService.ObtenerPorTrabajoAsync(idTrabajoCuali);
            return Ok(new
            {
                exitoso = true,
                datos = sesiones,
                mensaje = "Sesiones obtenidas correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener sesiones del trabajo {idTrabajoCuali}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al obtener sesiones del trabajo. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Obtiene sesiones por segmento.
    /// </summary>
    [HttpGet("obtener-por-segmento/{idSegmento}")]
    public async Task<IActionResult> ObtenerPorSegmento(long idSegmento)
    {
        try
        {
            var sesiones = await _sesionesService.ObtenerPorSegmentoAsync(idSegmento);
            return Ok(new
            {
                exitoso = true,
                datos = sesiones,
                mensaje = "Sesiones obtenidas correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener sesiones del segmento {idSegmento}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al obtener sesiones del segmento. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Obtiene una sesión por ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(long id)
    {
        try
        {
            var sesion = await _sesionesService.ObtenerPorIdAsync(id);
            if (sesion == null)
            {
                return NotFound(new
                {
                    exitoso = false,
                    mensaje = "Sesión no encontrada"
                });
            }

            return Ok(new
            {
                exitoso = true,
                datos = sesion,
                mensaje = "Sesión obtenida correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener sesión {id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al obtener sesión. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Crea una nueva sesión.
    /// </summary>
    [HttpPost("crear")]
    public async Task<IActionResult> Crear([FromBody] SesionesCuali sesion)
    {
        try
        {
            var resultado = await _sesionesService.CrearAsync(sesion, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear sesión");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al crear sesión. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Actualiza una sesión existente.
    /// </summary>
    [HttpPut("actualizar")]
    public async Task<IActionResult> Actualizar([FromBody] SesionesCuali sesion)
    {
        try
        {
            var resultado = await _sesionesService.ActualizarAsync(sesion, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar sesión {sesion.Id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al actualizar sesión. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Cambia el estado de una sesión.
    /// </summary>
    [HttpPost("cambiar-estado")]
    public async Task<IActionResult> CambiarEstado(
        [FromQuery] long idSesion,
        [FromQuery] string nuevoEstado,
        [FromQuery] string? observacion = null)
    {
        try
        {
            var resultado = await _sesionesService.CambiarEstadoAsync(idSesion, nuevoEstado, ObtenerIdUsuarioActual(), observacion);
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al cambiar estado de la sesión {idSesion}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al cambiar estado de la sesión. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Elimina lógicamente una sesión.
    /// </summary>
    [HttpDelete("eliminar/{id}")]
    public async Task<IActionResult> Eliminar(long id)
    {
        try
        {
            var resultado = await _sesionesService.EliminarAsync(id, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al eliminar sesión {id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al eliminar sesión. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Registra la asistencia de participantes a una sesión.
    /// </summary>
    [HttpPost("registrar-asistencia")]
    public async Task<IActionResult> RegistrarAsistencia(
        [FromQuery] long idSesion,
        [FromBody] List<long> idsParticipantes)
    {
        try
        {
            var resultado = await _sesionesService.RegistrarAsistenciaAsync(idSesion, idsParticipantes, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al registrar asistencia de la sesión {idSesion}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al registrar asistencia de la sesión. Por favor intente nuevamente."
            });
        }
    }
}
