using System.Security.Claims;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Services.PY;
using MatrixNext.Web.ViewModels;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controlador para gestión de trabajos OP (Portal COE)
/// </summary>
/// <remarks>
/// Migración de WebMatrix/OP_Cuantitativo/Trabajos.aspx
/// Permiso requerido: 100 (COE)
/// </remarks>
[Area("OP")]
[Authorize]
public class TrabajosController : Controller
{
    private readonly ITrabajosService _trabajosService;
    private readonly IOpTrabajosService _opTrabajosService;
    private readonly IOpPermisosService _permisosService;
    private readonly ILogger<TrabajosController> _logger;

    public TrabajosController(
        ITrabajosService trabajosService,
        IOpTrabajosService opTrabajosService,
        IOpPermisosService permisosService,
        ILogger<TrabajosController> logger)
    {
        _trabajosService = trabajosService;
        _opTrabajosService = opTrabajosService;
        _permisosService = permisosService;
        _logger = logger;
    }

    /// <summary>
    /// Vista principal: listado de trabajos del COE
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(FiltrosVM filtros)
    {
        // Validar permiso 100 (COE)
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
        {
            _logger.LogWarning("Usuario sin ID válido intentó acceder a Trabajos");
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        var tienePermiso = await _permisosService.TienePermisoAsync(userId, 100);
        if (!tienePermiso)
        {
            _logger.LogWarning("Usuario {UserId} sin permiso 100 intentó acceder a Trabajos", userId);
            TempData["Error"] = "No tiene permisos para acceder a esta sección";
            return RedirectToAction("Index", "Home", new { area = "OP" });
        }

        // Obtener listado de trabajos (reutilizando servicio de PY)
        var trabajos = await _trabajosService.ListarAsync(filtros);

        var viewModel = new OpTrabajosViewModel
        {
            Filtros = filtros ?? new FiltrosVM(),
            Trabajos = trabajos,
            UserId = userId
        };

        return View(viewModel);
    }

    /// <summary>
    /// Selecciona un trabajo y carga su configuración
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SeleccionarTrabajo(long trabajoId)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Json(new { success = false, message = "Usuario no válido" });
            }

            // Verificar si el trabajo está bloqueado
            var estaBloqueado = await _opTrabajosService.EstaTrabajoBloquadoAsync(trabajoId);
            if (estaBloqueado)
            {
                return Json(new { success = false, message = "El trabajo está cerrado o anulado" });
            }

            // Obtener configuración del trabajo
            var config = await _opTrabajosService.ObtenerConfiguracionAsync(trabajoId);
            
            // Verificar si tiene estimación (para mostrar/ocultar botones)
            var tieneEstimacion = await _opTrabajosService.TieneEstimacionAsync(trabajoId);

            // Obtener ID de ficha cuantitativa si existe
            var idFicha = await _opTrabajosService.ObtenerIdFichaCuantitativaAsync(trabajoId);

            // Guardar en TempData para mantener estado entre requests
            TempData["TrabajoId"] = trabajoId;

            return Json(new
            {
                success = true,
                trabajoId,
                tipoRecoleccionId = config?.TipoRecoleccionId,
                tieneEstimacion,
                idFicha,
                message = "Trabajo seleccionado"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al seleccionar trabajo {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error al seleccionar el trabajo" });
        }
    }

    /// <summary>
    /// Guarda la configuración del trabajo (tipo de recolección)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GuardarConfiguracion(long trabajoId, short tipoRecoleccionId)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Json(new { success = false, message = "Usuario no válido" });
            }

            var resultado = await _opTrabajosService.GuardarConfiguracionAsync(trabajoId, tipoRecoleccionId, userId);

            if (resultado)
            {
                return Json(new { success = true, message = "Configuración guardada exitosamente" });
            }
            else
            {
                return Json(new { success = false, message = "Error al guardar la configuración" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar configuración de trabajo {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error al guardar la configuración" });
        }
    }

    /// <summary>
    /// Navega a la vista de muestra del trabajo
    /// </summary>
    [HttpGet]
    public IActionResult Muestra(long trabajoId)
    {
        TempData["TrabajoId"] = trabajoId;
        return RedirectToAction("Index", "MuestraTrabajos", new { area = "OP", trabajoId });
    }

    /// <summary>
    /// Navega a la vista de estimaciones
    /// </summary>
    [HttpGet]
    public IActionResult Estimaciones(long trabajoId)
    {
        TempData["TrabajoId"] = trabajoId;
        return RedirectToAction("Index", "EstimacionProduccion", new { area = "OP", trabajoId });
    }

    /// <summary>
    /// Navega a la vista de ficha cuantitativa
    /// </summary>
    [HttpGet]
    public IActionResult FichaCuantitativa(long trabajoId)
    {
        TempData["TrabajoId"] = trabajoId;
        return RedirectToAction("Edit", "FichaCuantitativa", new { area = "OP", id = trabajoId });
    }

    /// <summary>
    /// Navega a la vista de presupuestos internos
    /// </summary>
    [HttpGet]
    public IActionResult Presupuestos(long trabajoId)
    {
        TempData["TrabajoId"] = trabajoId;
        return RedirectToAction("Index", "Presupuestos", new { area = "OP", trabajoId });
    }

    /// <summary>
    /// Navega a la vista de avance de campo
    /// </summary>
    [HttpGet]
    public IActionResult Avance(long trabajoId)
    {
        TempData["TrabajoId"] = trabajoId;
        return RedirectToAction("Index", "Avances", new { area = "OP", trabajoId });
    }

    /// <summary>
    /// Navega a la vista de importación de datos
    /// </summary>
    [HttpGet]
    public IActionResult ImportarDatos(long trabajoId)
    {
        TempData["TrabajoId"] = trabajoId;
        return RedirectToAction("Index", "ImportacionMasiva", new { area = "OP", trabajoId });
    }

    /// <summary>
    /// Modal de confirmación de cierre de trabajo (placeholder para Sprint 2)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ConfirmarCierre(long trabajoId)
    {
        // TODO: Implementar en Sprint 2 (GAP-OP-08)
        // - Validar documentos escaneados en GD
        // - Mostrar modal con documentos faltantes
        // - Permitir forzar cierre

        var trabajo = await _trabajosService.ObtenerPorIdAsync(trabajoId);
        if (trabajo == null)
        {
            return NotFound();
        }

        return PartialView("_ModalCerrarTrabajo", new { TrabajoId = trabajoId, NombreTrabajo = trabajo.Nombre });
    }

    /// <summary>
    /// Cierra el trabajo (placeholder para Sprint 2)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CerrarTrabajo(long trabajoId, bool forzar = false)
    {
        // TODO: Implementar en Sprint 2 (GAP-OP-08)
        // - Validar estado del trabajo
        // - Validar documentos GD
        // - Cambiar estado a cerrado
        // - Enviar email de notificación

        try
        {
            _logger.LogInformation("Cerrando trabajo {TrabajoId} (forzar: {Forzar})", trabajoId, forzar);
            
            return Json(new { success = false, message = "Funcionalidad en desarrollo (Sprint 2)" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cerrar trabajo {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error al cerrar el trabajo" });
        }
    }
}
