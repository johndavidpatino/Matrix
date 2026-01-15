using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.CORE;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Areas.CORE.Controllers;

/// <summary>
/// Controller para reportes e indicadores del WorkFlow
/// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T3 (reportes)
/// </summary>
[Area("CORE")]
[Authorize(Roles = "Coordinador,Ejecutor,Administrador")]
[Route("api/[area]/[controller]")]
[ApiController]
public class ReportesWorkFlowController : ControllerBase
{
    private readonly IWorkFlowReportesService _reportesService;
    private readonly ILogger<ReportesWorkFlowController> _logger;

    public ReportesWorkFlowController(
        IWorkFlowReportesService reportesService,
        ILogger<ReportesWorkFlowController> logger)
    {
        _reportesService = reportesService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene indicadores de cumplimiento de tareas
    /// </summary>
    [HttpGet("cumplimiento")]
    public async Task<IActionResult> ObtenerIndicadoresCumplimiento(
        [FromQuery] int? mes = null,
        [FromQuery] int? año = null)
    {
        try
        {
            var indicadores = await _reportesService.ObtenerIndicadoresCumplimiento(mes, año);
            return Ok(new
            {
                exitoso = true,
                datos = indicadores,
                cantidad = indicadores.Count
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo indicadores de cumplimiento");
            return BadRequest(new { exitoso = false, mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene tareas vencidas del usuario actual
    /// </summary>
    [HttpGet("tareas-vencidas/{idUsuario}")]
    public async Task<IActionResult> ObtenerTareasVencidas(long idUsuario)
    {
        try
        {
            var tareasVencidas = await _reportesService.ObtenerTareasVencidas(idUsuario);
            return Ok(new
            {
                exitoso = true,
                datos = tareasVencidas,
                cantidad = tareasVencidas.Count
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error obteniendo tareas vencidas del usuario {idUsuario}");
            return BadRequest(new { exitoso = false, mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene estadísticas generales del WorkFlow
    /// </summary>
    [HttpGet("estadisticas")]
    public async Task<IActionResult> ObtenerEstadisticas()
    {
        try
        {
            var estadisticas = await _reportesService.ObtenerEstadisticas();
            return Ok(new
            {
                exitoso = true,
                datos = estadisticas
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo estadísticas");
            return BadRequest(new { exitoso = false, mensaje = ex.Message });
        }
    }
}
