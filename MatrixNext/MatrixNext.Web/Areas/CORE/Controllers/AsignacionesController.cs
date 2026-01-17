using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.CORE;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Areas.CORE.Controllers;

/// <summary>
/// T3.5: Controller para gestionar asignaciones de tareas a usuarios
/// Ref: MATRIZ_PERMISOS_ROLES.md § 3.3 (Coordinador + Administrador)
/// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T3.5
/// </summary>
[Area("CORE")]
[Authorize(Roles = "Coordinador,Administrador")]
[Route("api/[area]/[controller]")]
[ApiController]
public class AsignacionesController : ControllerBase
{
    private readonly IAsignacionesService _asignacionesService;
    private readonly ILogger<AsignacionesController> _logger;

    public AsignacionesController(
        IAsignacionesService asignacionesService,
        ILogger<AsignacionesController> logger)
    {
        _asignacionesService = asignacionesService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los usuarios asignados a una tarea.
    /// </summary>
    /// <param name="idWorkFlow">ID de la tarea (WorkFlow)</param>
    /// <returns>Lista de usuarios asignados</returns>
    [HttpGet("obtener-usuarios/{idWorkFlow}")]
    public async Task<IActionResult> ObtenerUsuariosAsignados(long idWorkFlow)
    {
        try
        {
            var usuarios = await _asignacionesService.ObtenerUsuariosAsignados(idWorkFlow);
            return Ok(new
            {
                exitoso = true,
                datos = usuarios,
                mensaje = "Usuarios obtenidos correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener usuarios asignados para tarea {idWorkFlow}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al procesar la solicitud. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Asigna un usuario a una tarea.
    /// </summary>
    /// <param name="idWorkFlow">ID de la tarea</param>
    /// <param name="idUsuario">ID del usuario a asignar</param>
    /// <param name="rol">Rol del usuario en la tarea (opcional, default: "Ejecutor")</param>
    /// <returns>Resultado de la asignación</returns>
    [HttpPost("asignar")]
    public async Task<IActionResult> AsignarUsuario(
        [FromQuery] long idWorkFlow,
        [FromQuery] long idUsuario,
        [FromQuery] string? rol = null)
    {
        try
        {
            var resultado = await _asignacionesService.AsignarUsuario(idWorkFlow, idUsuario, rol);
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                mensaje = resultado.Message,
                datos = resultado.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al asignar usuario {idUsuario} a tarea {idWorkFlow}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al procesar la solicitud. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Desasigna un usuario de una tarea.
    /// </summary>
    /// <param name="idWorkFlow">ID de la tarea</param>
    /// <param name="idUsuario">ID del usuario a desasignar</param>
    /// <returns>Resultado de la desasignación</returns>
    [HttpPost("desasignar")]
    public async Task<IActionResult> DesasignarUsuario(
        [FromQuery] long idWorkFlow,
        [FromQuery] long idUsuario)
    {
        try
        {
            var resultado = await _asignacionesService.DesasignarUsuario(idWorkFlow, idUsuario);
            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                mensaje = resultado.Message,
                datos = resultado.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al desasignar usuario {idUsuario} de tarea {idWorkFlow}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al procesar la solicitud. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Obtiene todas las asignaciones activas de un usuario.
    /// </summary>
    /// <param name="idUsuario">ID del usuario</param>
    /// <returns>Lista de tareas asignadas</returns>
    [HttpGet("asignaciones-usuario/{idUsuario}")]
    public async Task<IActionResult> ObtenerAsignacionesActivas(long idUsuario)
    {
        try
        {
            var asignaciones = await _asignacionesService.ObtenerAsignacionesActivas(idUsuario);
            return Ok(new
            {
                exitoso = true,
                datos = asignaciones,
                mensaje = $"{asignaciones.Count} asignación(es) encontrada(s)"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener asignaciones del usuario {idUsuario}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al procesar la solicitud. Por favor intente nuevamente."
            });
        }
    }

    /// <summary>
    /// Verifica si un usuario está asignado a una tarea.
    /// </summary>
    /// <param name="idWorkFlow">ID de la tarea</param>
    /// <param name="idUsuario">ID del usuario</param>
    /// <returns>True si está asignado, false en caso contrario</returns>
    [HttpGet("esta-asignado/{idWorkFlow}/{idUsuario}")]
    public async Task<IActionResult> EstaAsignado(long idWorkFlow, long idUsuario)
    {
        try
        {
            var estaAsignado = await _asignacionesService.EstaAsignado(idWorkFlow, idUsuario);
            return Ok(new
            {
                exitoso = true,
                datos = estaAsignado,
                mensaje = estaAsignado ? "Usuario está asignado" : "Usuario no está asignado"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al verificar asignación de usuario {idUsuario} a tarea {idWorkFlow}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = "Error al procesar la solicitud. Por favor intente nuevamente."
            });
        }
    }
}
