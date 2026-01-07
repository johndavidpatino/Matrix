using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.CORE;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Areas.CORE.Controllers;

/// <summary>
/// T3.6: Controller para gestionar operaciones en tareas (CRÍTICO)
/// - Cambio de estado con validación de precedencias
/// - Agregar observaciones/comentarios
/// - Anular tareas
/// 
/// Ref: MATRIZ_PERMISOS_ROLES.md § 4.4
/// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T3.6
/// </summary>
[Area("CORE")]
[Authorize(Roles = "Coordinador,Ejecutor,Administrador")]
[Route("api/[area]/[controller]")]
[ApiController]
public class GestionTareasController : ControllerBase
{
    private readonly IGestionTareasService _gestionService;
    private readonly ILogger<GestionTareasController> _logger;

    public GestionTareasController(
        IGestionTareasService gestionService,
        ILogger<GestionTareasController> logger)
    {
        _gestionService = gestionService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene las tareas asignadas al usuario actual.
    /// Opcionalmente filtradas por estado.
    /// </summary>
    /// <param name="idUsuario">ID del usuario</param>
    /// <param name="estado">Estado a filtrar (opcional)</param>
    /// <returns>Lista de tareas asignadas</returns>
    [HttpGet("mis-tareas/{idUsuario}")]
    public async Task<IActionResult> MisTrabajos(long idUsuario, [FromQuery] string? estado = null)
    {
        try
        {
            var tareas = await _gestionService.ObtenerMisTareas(idUsuario, estado);
            return Ok(new
            {
                exitoso = true,
                datos = tareas,
                mensaje = $"{tareas.Count} tarea(s) encontrada(s)",
                filtro = new { idUsuario, estado }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener tareas del usuario {idUsuario}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Cambia el estado de una tarea validando:
    /// 1. Que todas las tareas previas estén completadas
    /// 2. Que el usuario esté asignado a la tarea
    /// 3. Que el cambio de estado sea válido
    /// </summary>
    /// <param name="idWorkFlow">ID de la tarea</param>
    /// <param name="nuevoEstado">Nuevo estado (ej: "EnProgreso", "Completada", "Anulada")</param>
    /// <param name="idUsuario">ID del usuario que realiza el cambio</param>
    /// <param name="observacion">Observación opcional del cambio</param>
    /// <returns>Resultado del cambio de estado</returns>
    [HttpPost("cambiar-estado")]
    public async Task<IActionResult> CambiarEstado(
        [FromQuery] long idWorkFlow,
        [FromQuery] string nuevoEstado,
        [FromQuery] long idUsuario,
        [FromQuery] string? observacion = null)
    {
        try
        {
            // Validación de entrada
            if (string.IsNullOrWhiteSpace(nuevoEstado))
            {
                return BadRequest(new
                {
                    exitoso = false,
                    mensaje = "El nuevo estado es requerido"
                });
            }

            var resultado = await _gestionService.CambiarEstado(
                idWorkFlow,
                nuevoEstado,
                idUsuario,
                observacion
            );

            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                mensaje = resultado.Message,
                datos = resultado.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al cambiar estado de tarea {idWorkFlow} a {nuevoEstado}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Agrega un comentario/observación a una tarea.
    /// No requiere validación de precedencias.
    /// </summary>
    /// <param name="idWorkFlow">ID de la tarea</param>
    /// <param name="idUsuario">ID del usuario que comenta</param>
    /// <param name="observacion">Texto del comentario</param>
    /// <returns>Resultado de la operación</returns>
    [HttpPost("agregar-observacion")]
    public async Task<IActionResult> AgregarObservacion(
        [FromQuery] long idWorkFlow,
        [FromQuery] long idUsuario,
        [FromBody] string observacion)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(observacion))
            {
                return BadRequest(new
                {
                    exitoso = false,
                    mensaje = "La observación no puede estar vacía"
                });
            }

            var resultado = await _gestionService.AgregarObservacion(
                idWorkFlow,
                idUsuario,
                observacion,
                "Comentario"
            );

            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                mensaje = resultado.Message,
                datos = resultado.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al agregar observación a tarea {idWorkFlow}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Anula una tarea (solo administrador).
    /// Requiere motivo obligatorio.
    /// </summary>
    /// <param name="idWorkFlow">ID de la tarea</param>
    /// <param name="idUsuario">ID del usuario que anula (debe ser admin)</param>
    /// <param name="motivo">Motivo de la anulación</param>
    /// <returns>Resultado de la anulación</returns>
    [HttpPost("anular")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AnularTarea(
        [FromQuery] long idWorkFlow,
        [FromQuery] long idUsuario,
        [FromBody] string motivo)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(motivo))
            {
                return BadRequest(new
                {
                    exitoso = false,
                    mensaje = "El motivo de anulación es requerido"
                });
            }

            var resultado = await _gestionService.AnularTarea(
                idWorkFlow,
                idUsuario,
                motivo
            );

            return Ok(new
            {
                exitoso = resultado.IsSuccess,
                mensaje = resultado.Message,
                datos = resultado.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al anular tarea {idWorkFlow}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Obtiene las tareas previas que bloquean el cambio de estado.
    /// Útil para mostrar mensajes de validación al usuario.
    /// </summary>
    /// <param name="idWorkFlow">ID de la tarea</param>
    /// <returns>Lista de tareas previas</returns>
    [HttpGet("tareas-previas/{idWorkFlow}")]
    public async Task<IActionResult> ObtenerTareasPrevias(long idWorkFlow)
    {
        try
        {
            var tareasPrevias = await _gestionService.ObtenerTareasPrevias(idWorkFlow);
            var tareasPendientes = tareasPrevias
                .Where(t => t.Estado != "Completada" && t.Estado != "Anulada")
                .ToList();

            return Ok(new
            {
                exitoso = true,
                datos = new
                {
                    tareasPrevias = tareasPrevias.Count,
                    tareasPendientes = tareasPendientes.Count,
                    tareas = tareasPendientes,
                    puedeAvanzar = tareasPendientes.Count == 0
                },
                mensaje = tareasPendientes.Count == 0
                    ? "Todas las tareas previas están completadas"
                    : $"Faltan {tareasPendientes.Count} tarea(s) previa(s) por completar"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener tareas previas de {idWorkFlow}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Valida si todas las tareas previas de una tarea están completadas.
    /// Retorna true/false sin cambiar estado.
    /// </summary>
    /// <param name="idWorkFlow">ID de la tarea</param>
    /// <returns>True si se pueden completar todas las precedencias</returns>
    [HttpGet("validar-precedencias/{idWorkFlow}")]
    public async Task<IActionResult> ValidarPrecedencias(long idWorkFlow)
    {
        try
        {
            var validas = await _gestionService.ValidarPrecedenciasCompletadas(idWorkFlow);
            return Ok(new
            {
                exitoso = true,
                datos = validas,
                mensaje = validas
                    ? "Todas las precedencias están satisfechas"
                    : "Existen precedencias pendientes"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al validar precedencias de tarea {idWorkFlow}");
            return BadRequest(new
            {
                exitoso = false,
                mensaje = $"Error: {ex.Message}"
            });
        }
    }
}
