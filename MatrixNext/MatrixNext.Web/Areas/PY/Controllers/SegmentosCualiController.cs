using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.PY;
using MatrixNext.Web.Models.PY;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.PY.Controllers;

/// <summary>
/// T4.12: Controller para gestión de segmentos cualitativos
/// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 4.12
/// </summary>
[Area("PY")]
[Authorize(Roles = "Coordinador,Administrador")]
[Route("api/[area]/[controller]")]
[ApiController]
public class SegmentosCualiController : ControllerBase
{
    private readonly ISegmentosCualiService _segmentosService;
    private readonly ILogger<SegmentosCualiController> _logger;

    public SegmentosCualiController(
        ISegmentosCualiService segmentosService,
        ILogger<SegmentosCualiController> logger)
    {
        _segmentosService = segmentosService;
        _logger = logger;
    }

    private long ObtenerIdUsuarioActual()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    /// <summary>
    /// Obtiene todos los segmentos de un trabajo cualitativo.
    /// </summary>
    [HttpGet("obtener-por-trabajo/{idTrabajoCuali}")]
    public async Task<IActionResult> ObtenerPorTrabajo(long idTrabajoCuali)
    {
        try
        {
            var segmentos = await _segmentosService.ObtenerPorTrabajoAsync(idTrabajoCuali);
            return Ok(new
            {
                exitoso = true,
                datos = segmentos,
                mensaje = "Segmentos cualitativos obtenidos correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener segmentos del trabajo {idTrabajoCuali}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Obtiene un segmento por ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(long id)
    {
        try
        {
            var segmento = await _segmentosService.ObtenerPorIdAsync(id);
            if (segmento == null)
            {
                return NotFound(new
                {
                    exitoso = false,
                    mensaje = "Segmento no encontrado"
                });
            }

            return Ok(new
            {
                exitoso = true,
                datos = segmento,
                mensaje = "Segmento obtenido correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener segmento {id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Obtiene el total de participantes por trabajo.
    /// </summary>
    [HttpGet("total-participantes/{idTrabajoCuali}")]
    public async Task<IActionResult> ObtenerTotalParticipantes(long idTrabajoCuali)
    {
        try
        {
            var total = await _segmentosService.ObtenerTotalParticipantesPorTrabajoAsync(idTrabajoCuali);
            return Ok(new
            {
                exitoso = true,
                datos = total,
                mensaje = "Total de participantes calculado correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al calcular total de participantes del trabajo {idTrabajoCuali}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Crea un nuevo segmento.
    /// </summary>
    [HttpPost("crear")]
    public async Task<IActionResult> Crear([FromBody] SegmentosCuali segmento)
    {
        try
        {
            var resultado = await _segmentosService.CrearAsync(segmento, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear segmento");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Actualiza un segmento existente.
    /// </summary>
    [HttpPut("actualizar")]
    public async Task<IActionResult> Actualizar([FromBody] SegmentosCuali segmento)
    {
        try
        {
            var resultado = await _segmentosService.ActualizarAsync(segmento, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar segmento {segmento.Id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Elimina lógicamente un segmento.
    /// </summary>
    [HttpDelete("eliminar/{id}")]
    public async Task<IActionResult> Eliminar(long id)
    {
        try
        {
            var resultado = await _segmentosService.EliminarAsync(id, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al eliminar segmento {id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Duplica un segmento.
    /// </summary>
    [HttpPost("duplicar/{id}")]
    public async Task<IActionResult> Duplicar(long id)
    {
        try
        {
            var resultado = await _segmentosService.DuplicarAsync(id, ObtenerIdUsuarioActual());
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                datos = resultado.Data,
                mensaje = resultado.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al duplicar segmento {id}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }
}
