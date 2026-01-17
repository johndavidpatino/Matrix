using MatrixNext.Web.DTOs;
using MatrixNext.Web.Services.CORE;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Controllers;

/// <summary>
/// API Controller para gestión de flujos de trabajo (tareas) del sistema CORE.
/// Proporciona endpoints para CRUD de tareas, asignación, escalada y cierre.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoreController : ControllerBase
{
    private readonly ICoreTaskService _taskService;
    private readonly ICoreWorkflowService _workflowService;
    private readonly ICoreAssignmentService _assignmentService;
    private readonly ILogger<CoreController> _logger;

    public CoreController(
        ICoreTaskService taskService,
        ICoreWorkflowService workflowService,
        ICoreAssignmentService assignmentService,
        ILogger<CoreController> logger)
    {
        _taskService = taskService;
        _workflowService = workflowService;
        _assignmentService = assignmentService;
        _logger = logger;
    }

    /// <summary>
    /// Crear una nueva tarea (WorkFlow).
    /// </summary>
    /// <param name="createTaskDto">Datos de la tarea a crear</param>
    /// <returns>Tarea creada con ID</returns>
    [HttpPost("tareas/crear")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<WorkFlowDto>>> CreateTask([FromBody] CreateTaskDto createTaskDto)
    {
        try
        {
            var task = await _taskService.CreateTaskAsync(createTaskDto);
            _logger.LogInformation("Tarea creada: {TaskId}", task.Id);
            
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, 
                new ApiResponse<WorkFlowDto> { Success = true, Data = task, Message = "Tarea creada exitosamente" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Validación de tarea fallida: {Message}", ex.Message);
            return BadRequest(new ApiResponse<object> { Success = false, Message = "Error de validación al crear la tarea. Por favor verifique los datos." });
        }
    }

    /// <summary>
    /// Obtener una tarea por ID.
    /// </summary>
    /// <param name="id">ID de la tarea</param>
    /// <returns>Datos de la tarea</returns>
    [HttpGet("tareas/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<WorkFlowDto>>> GetTask(long id)
    {
        try
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound(new ApiResponse<object> { Success = false, Message = "Tarea no encontrada" });

            return Ok(new ApiResponse<WorkFlowDto> { Success = true, Data = task });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tarea: {TaskId}", id);
            return StatusCode(500, new ApiResponse<object> { Success = false, Message = "Error interno del servidor" });
        }
    }

    /// <summary>
    /// Obtener todas las tareas asignadas a un usuario.
    /// </summary>
    /// <param name="idUsuario">ID del usuario</param>
    /// <param name="pageNumber">Número de página (default: 1)</param>
    /// <param name="pageSize">Tamaño de página (default: 20)</param>
    /// <returns>Lista paginada de tareas del usuario</returns>
    [HttpGet("tareas/usuario/{idUsuario}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<WorkFlowDto>>>> GetUserTasks(
        long idUsuario, 
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var tasks = await _taskService.GetTasksByUserAsync(idUsuario, pageNumber, pageSize);
            return Ok(new ApiResponse<PaginatedResponse<WorkFlowDto>> { Success = true, Data = tasks });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tareas del usuario: {UserId}", idUsuario);
            return StatusCode(500, new ApiResponse<object> { Success = false, Message = "Error interno del servidor" });
        }
    }

    /// <summary>
    /// Actualizar los datos de una tarea (descripción, vencimiento, prioridad).
    /// </summary>
    /// <param name="id">ID de la tarea a actualizar</param>
    /// <param name="updateTaskDto">Datos a actualizar</param>
    /// <returns>Tarea actualizada</returns>
    [HttpPut("tareas/{id}/actualizar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<WorkFlowDto>>> UpdateTask(
        long id, 
        [FromBody] UpdateTaskDto updateTaskDto)
    {
        try
        {
            var task = await _taskService.UpdateTaskAsync(id, updateTaskDto);
            if (task == null)
                return NotFound(new ApiResponse<object> { Success = false, Message = "Tarea no encontrada" });

            _logger.LogInformation("Tarea actualizada: {TaskId}", id);
            return Ok(new ApiResponse<WorkFlowDto> { Success = true, Data = task, Message = "Tarea actualizada exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Error al actualizar tarea: {Message}", ex.Message);
            return BadRequest(new ApiResponse<object> { Success = false, Message = "No se puede actualizar la tarea. Por favor verifique los datos." });
        }
    }

    /// <summary>
    /// Anular una tarea (cambiar estado a Anulada).
    /// </summary>
    /// <param name="id">ID de la tarea a anular</param>
    /// <param name="motivo">Motivo de la anulación (opcional)</param>
    /// <returns>Tarea anulada</returns>
    [HttpDelete("tareas/{id}/anular")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<WorkFlowDto>>> CancelTask(
        long id, 
        [FromQuery] string? motivo = null)
    {
        try
        {
            var task = await _taskService.CancelTaskAsync(id, motivo);
            if (task == null)
                return NotFound(new ApiResponse<object> { Success = false, Message = "Tarea no encontrada" });

            _logger.LogInformation("Tarea anulada: {TaskId}, Motivo: {Reason}", id, motivo);
            return Ok(new ApiResponse<WorkFlowDto> { Success = true, Data = task, Message = "Tarea anulada exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("No se puede anular tarea: {Message}", ex.Message);
            return BadRequest(new ApiResponse<object> { Success = false, Message = "No se puede anular la tarea en su estado actual." });
        }
    }

    /// <summary>
    /// Asignar una tarea a uno o más usuarios con roles específicos.
    /// </summary>
    /// <param name="id">ID de la tarea</param>
    /// <param name="assignDto">Datos de asignación (usuario(s) y rol(es))</param>
    /// <returns>Tarea asignada</returns>
    [HttpPost("tareas/{id}/asignar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<WorkFlowDto>>> AssignTask(
        long id, 
        [FromBody] AssignTaskDto assignDto)
    {
        try
        {
            var task = await _assignmentService.AssignTaskAsync(id, assignDto);
            if (task == null)
                return NotFound(new ApiResponse<object> { Success = false, Message = "Tarea no encontrada" });

            _logger.LogInformation("Tarea asignada: {TaskId} a usuario(s): {Users}", 
                id, string.Join(", ", assignDto.IdUsuarios));

            return Ok(new ApiResponse<WorkFlowDto> { Success = true, Data = task, Message = "Tarea asignada exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Error en asignación: {Message}", ex.Message);
            return BadRequest(new ApiResponse<object> { Success = false, Message = "No se puede asignar la tarea. Verifique los datos de asignación." });
        }
    }

    /// <summary>
    /// Escalar una tarea a un nivel superior en la cadena de mando.
    /// </summary>
    /// <param name="id">ID de la tarea a escalar</param>
    /// <param name="escalateDto">Datos de escalada (usuario destino y motivo)</param>
    /// <returns>Tarea escalada</returns>
    [HttpPost("tareas/{id}/escalar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<WorkFlowDto>>> EscalateTask(
        long id, 
        [FromBody] EscalateTaskDto escalateDto)
    {
        try
        {
            var task = await _assignmentService.EscalateTaskAsync(id, escalateDto);
            if (task == null)
                return NotFound(new ApiResponse<object> { Success = false, Message = "Tarea no encontrada" });

            _logger.LogInformation("Tarea escalada: {TaskId} a usuario: {UserId}, Motivo: {Reason}", 
                id, escalateDto.IdUsuarioDestino, escalateDto.Motivo);

            return Ok(new ApiResponse<WorkFlowDto> { Success = true, Data = task, Message = "Tarea escalada exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Error en escalada: {Message}", ex.Message);
            return BadRequest(new ApiResponse<object> { Success = false, Message = "No se puede escalar la tarea. Verifique los datos." });
        }
    }

    /// <summary>
    /// Cerrar una tarea (cambiar estado a Completada o Cerrada).
    /// </summary>
    /// <param name="id">ID de la tarea a cerrar</param>
    /// <param name="resultado">Resultado/comentario final (opcional)</param>
    /// <returns>Tarea cerrada</returns>
    [HttpPost("tareas/{id}/cerrar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<WorkFlowDto>>> CompleteTask(
        long id, 
        [FromQuery] string? resultado = null)
    {
        try
        {
            var task = await _workflowService.CompleteTaskAsync(id, resultado);
            if (task == null)
                return NotFound(new ApiResponse<object> { Success = false, Message = "Tarea no encontrada" });

            _logger.LogInformation("Tarea cerrada: {TaskId}", id);
            return Ok(new ApiResponse<WorkFlowDto> { Success = true, Data = task, Message = "Tarea cerrada exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("No se puede cerrar tarea: {Message}", ex.Message);
            return BadRequest(new ApiResponse<object> { Success = false, Message = "No se puede cerrar la tarea en su estado actual." });
        }
    }
}
