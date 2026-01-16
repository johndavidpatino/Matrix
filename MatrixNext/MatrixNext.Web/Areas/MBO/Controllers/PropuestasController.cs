using MatrixNext.Data.Services.MBO;
using MatrixNext.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.MBO.Controllers;

/// <summary>
/// Controller para gestión de propuestas y métricas MBO
/// Incluye dashboards de estados, alta probabilidad, propuestas sin trabajo, gestión Matrix e índices manuales
/// </summary>
[Area("MBO")]
[Authorize]
public class PropuestasController : Controller
{
    private readonly IPropuestasService _propuestasService;
    private readonly IAOTService _aotService; // Para obtener unidades disponibles
    private readonly ILogger<PropuestasController> _logger;

    public PropuestasController(
        IPropuestasService propuestasService,
        IAOTService aotService,
        ILogger<PropuestasController> logger)
    {
        _propuestasService = propuestasService;
        _aotService = aotService;
        _logger = logger;
    }

    /// <summary>
    /// Dashboard de propuestas creadas/enviadas con gráficos por unidad y gerente
    /// Vista: EstadoTotal (PropuestasEstadoTotal.aspx)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> EstadoTotal(string? sigla)
    {
        try
        {
            var siglaFiltro = sigla ?? "9"; // '9' = todas las unidades
            var userId = GetUserId();
            
            // Obtener datos en paralelo
            var taskUnidades = _aotService.ObtenerUnidadesUsuarioAsync(userId);
            var taskPropuestas = _propuestasService.ObtenerPropuestasCreadasEnviadasAsync(siglaFiltro);
            var taskAltaProbabilidad = _propuestasService.ObtenerPropuestasAltaProbabilidadAsync(siglaFiltro);

            await Task.WhenAll(taskUnidades, taskPropuestas, taskAltaProbabilidad);

            var unidades = await taskUnidades;
            var (propuestasPorUnidad, propuestasPorGerente) = await taskPropuestas;
            var propuestasAltaProbabilidad = await taskAltaProbabilidad;

            // Calcular máximos para escala de gráficos
            var maximoPropuestas = _propuestasService.CalcularMaximoPropuestas(propuestasPorUnidad);
            var maximoAltaProbabilidad = _propuestasService.CalcularMaximoPropuestasAltaProbabilidad(propuestasAltaProbabilidad);

            var viewModel = new PropuestasEstadoViewModel
            {
                Sigla = siglaFiltro,
                UnidadesDisponibles = unidades,
                PropuestasPorUnidad = propuestasPorUnidad,
                PropuestasAltaProbabilidad = propuestasAltaProbabilidad,
                PropuestasPorGerente = propuestasPorGerente,
                MaximoPropuestas = maximoPropuestas,
                MaximoPropuestasAltaProbabilidad = maximoAltaProbabilidad
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar dashboard de estado de propuestas. Sigla: {Sigla}", sigla);
            TempData["Error"] = "Error al cargar los datos del dashboard";
            return View(new PropuestasEstadoViewModel());
        }
    }

    /// <summary>
    /// Dashboard de propuestas aprobadas sin trabajo asociado
    /// Vista: SinTrabajo (PropuestasSinTrabajo.aspx)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> SinTrabajo(string? unidad)
    {
        try
        {
            var (propuestasPorUnidad, propuestasPorMetodologia) = 
                await _propuestasService.ObtenerPropuestasSinTrabajoAsync(unidad);

            var viewModel = new PropuestasSinTrabajoViewModel
            {
                UnidadSeleccionada = unidad,
                PropuestasPorUnidad = propuestasPorUnidad,
                PropuestasPorMetodologia = propuestasPorMetodologia
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar propuestas sin trabajo. Unidad: {Unidad}", unidad);
            TempData["Error"] = "Error al cargar los datos de propuestas sin trabajo";
            return View(new PropuestasSinTrabajoViewModel());
        }
    }

    /// <summary>
    /// Dashboard de gestión Matrix (briefs, propuestas, presupuestos, estudios, proyectos, trabajos)
    /// Vista: GestionMatrix (MatrixGestion.aspx)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GestionMatrix()
    {
        try
        {
            var gestion = await _propuestasService.ObtenerGestionMatrixAsync();

            var viewModel = new GestionMatrixViewModel
            {
                GestionMatrix = gestion
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar dashboard de gestión Matrix");
            TempData["Error"] = "Error al cargar los datos de gestión Matrix";
            return View(new GestionMatrixViewModel());
        }
    }

    /// <summary>
    /// Dashboard de índices manuales de cuentas
    /// Vista: IndicesManuales (IndicesManualesCuentas.aspx)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> IndicesManuales()
    {
        try
        {
            var indices = await _propuestasService.ObtenerIndicesManualesAsync();

            var viewModel = new IndicesManualesViewModel
            {
                IndicesManuales = indices
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar índices manuales de cuentas");
            TempData["Error"] = "Error al cargar los índices manuales";
            return View(new IndicesManualesViewModel());
        }
    }

    /// <summary>
    /// Dashboard de propuestas por estado de unidad específica
    /// Vista: EstadoUnidad (PropuestasEstadoUnidad.aspx) - Similar a EstadoTotal pero para una unidad
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> EstadoUnidad(string sigla)
    {
        try
        {
            if (string.IsNullOrEmpty(sigla) || sigla == "9")
            {
                // Redirigir a EstadoTotal si no hay sigla específica
                return RedirectToAction(nameof(EstadoTotal));
            }

            // Obtener datos en paralelo
            var taskUnidades = _aotService.ObtenerUnidadesUsuarioAsync(GetUserId());
            var taskPropuestas = _propuestasService.ObtenerPropuestasCreadasEnviadasAsync(sigla);
            var taskAltaProbabilidad = _propuestasService.ObtenerPropuestasAltaProbabilidadAsync(sigla);

            await Task.WhenAll(taskUnidades, taskPropuestas, taskAltaProbabilidad);

            var unidades = await taskUnidades;
            var (propuestasPorUnidad, propuestasPorGerente) = await taskPropuestas;
            var propuestasAltaProbabilidad = await taskAltaProbabilidad;

            // Calcular máximos para escala de gráficos
            var maximoPropuestas = _propuestasService.CalcularMaximoPropuestas(propuestasPorUnidad);
            var maximoAltaProbabilidad = _propuestasService.CalcularMaximoPropuestasAltaProbabilidad(propuestasAltaProbabilidad);

            var viewModel = new PropuestasEstadoViewModel
            {
                Sigla = sigla,
                UnidadesDisponibles = unidades,
                PropuestasPorUnidad = propuestasPorUnidad,
                PropuestasAltaProbabilidad = propuestasAltaProbabilidad,
                PropuestasPorGerente = propuestasPorGerente,
                MaximoPropuestas = maximoPropuestas,
                MaximoPropuestasAltaProbabilidad = maximoAltaProbabilidad
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar dashboard de estado de unidad. Sigla: {Sigla}", sigla);
            TempData["Error"] = "Error al cargar los datos del dashboard";
            return RedirectToAction(nameof(EstadoTotal));
        }
    }

    #region Helpers

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    #endregion
}
