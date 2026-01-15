using MatrixNext.Data.Services.MBO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.MBO.Controllers;

/// <summary>
/// Controller para dashboards de AOT (Achievement of Tasks)
/// Gestión por Objetivos para Dirección, Gerencia y Gerentes
/// </summary>
[Area("MBO")]
[Authorize]
public class AOTController : Controller
{
    private readonly IAOTService _service;
    private readonly ILogger<AOTController> _logger;

    public AOTController(IAOTService service, ILogger<AOTController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Dashboard AOT para Dirección
    /// Muestra Budget vs Ejecución AOT para todas las unidades
    /// Requiere permiso 23 (acceso MBO)
    /// GET: /MBO/AOT/Direccion
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Direccion(int? año, int? mes, string? sigla)
    {
        try
        {
            var usuarioId = GetUsuarioId();
            
            // Valores por defecto: año y mes actuales
            var añoSeleccionado = año ?? DateTime.Now.Year;
            var mesSeleccionado = mes ?? DateTime.Now.Month;
            
            // Obtener unidades disponibles para el usuario
            var unidades = await _service.ObtenerUnidadesUsuarioAsync(usuarioId);
            
            // Si no hay sigla seleccionada, usar la primera unidad disponible
            var siglaSeleccionada = sigla ?? unidades.FirstOrDefault()?.Sigla ?? string.Empty;
            
            // Obtener datos completos del dashboard
            var viewModel = await _service.ObtenerDatosDireccionAsync(
                añoSeleccionado, 
                mesSeleccionado, 
                siglaSeleccionada, 
                usuarioId
            );
            
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando dashboard AOT Dirección");
            TempData["Error"] = "Error al cargar el dashboard AOT Dirección";
            return RedirectToAction("Index", "Home", new { area = "MBO" });
        }
    }

    /// <summary>
    /// Dashboard AOT para Gerencia
    /// Similar a Dirección pero filtrado por gerencia específica
    /// GET: /MBO/AOT/Gerencia
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Gerencia(int? año, int? mes, string? sigla)
    {
        try
        {
            var usuarioId = GetUsuarioId();
            
            var añoSeleccionado = año ?? DateTime.Now.Year;
            var mesSeleccionado = mes ?? DateTime.Now.Month;
            
            var unidades = await _service.ObtenerUnidadesUsuarioAsync(usuarioId);
            var siglaSeleccionada = sigla ?? unidades.FirstOrDefault()?.Sigla ?? string.Empty;
            
            var viewModel = await _service.ObtenerDatosGerenciaAsync(
                añoSeleccionado, 
                mesSeleccionado, 
                siglaSeleccionada, 
                usuarioId
            );
            
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando dashboard AOT Gerencia");
            TempData["Error"] = "Error al cargar el dashboard AOT Gerencia";
            return RedirectToAction("Index", "Home", new { area = "MBO" });
        }
    }

    /// <summary>
    /// Dashboard AOT desagregado por Gerentes de Cuenta
    /// Muestra rendimiento individual de cada gerente
    /// GET: /MBO/AOT/PorGerentes
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> PorGerentes(int? año, int? mes, string? sigla)
    {
        try
        {
            var usuarioId = GetUsuarioId();
            
            var añoSeleccionado = año ?? DateTime.Now.Year;
            var mesSeleccionado = mes ?? DateTime.Now.Month;
            
            var unidades = await _service.ObtenerUnidadesUsuarioAsync(usuarioId);
            var siglaSeleccionada = sigla ?? unidades.FirstOrDefault()?.Sigla ?? string.Empty;
            
            var viewModel = await _service.ObtenerDatosPorGerentesAsync(
                añoSeleccionado, 
                mesSeleccionado, 
                siglaSeleccionada, 
                usuarioId
            );
            
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando dashboard AOT por Gerentes");
            TempData["Error"] = "Error al cargar el dashboard AOT por Gerentes";
            return RedirectToAction("Index", "Home", new { area = "MBO" });
        }
    }

    /// <summary>
    /// Dashboard AOT para una unidad específica
    /// GET: /MBO/AOT/GerenciaAOT/{sigla}
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GerenciaAOT(string sigla, int? año, int? mes)
    {
        try
        {
            if (string.IsNullOrEmpty(sigla))
            {
                TempData["Error"] = "Debe especificar una unidad";
                return RedirectToAction("Direccion");
            }
            
            var usuarioId = GetUsuarioId();
            
            var añoSeleccionado = año ?? DateTime.Now.Year;
            var mesSeleccionado = mes ?? DateTime.Now.Month;
            
            var viewModel = await _service.ObtenerDatosUnidadAsync(
                añoSeleccionado, 
                mesSeleccionado, 
                sigla, 
                usuarioId
            );
            
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando dashboard AOT para unidad {Sigla}", sigla);
            TempData["Error"] = $"Error al cargar el dashboard AOT para la unidad {sigla}";
            return RedirectToAction("Direccion");
        }
    }

    /// <summary>
    /// Obtiene el ID del usuario logueado desde Claims
    /// </summary>
    private int GetUsuarioId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            _logger.LogWarning("No se pudo obtener el ID del usuario logueado");
            throw new UnauthorizedAccessException("Usuario no autenticado");
        }
        
        return userId;
    }
}
