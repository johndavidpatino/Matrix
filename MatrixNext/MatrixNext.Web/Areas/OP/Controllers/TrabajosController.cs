using System.Security.Claims;
using MatrixNext.Web.Services;
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
    private readonly IOpGestionDocumentalService _gestionDocumentalService;
    private readonly IEmailService _emailService;
    private readonly ILogger<TrabajosController> _logger;

    public TrabajosController(
        ITrabajosService trabajosService,
        IOpTrabajosService opTrabajosService,
        IOpPermisosService permisosService,
        IOpGestionDocumentalService gestionDocumentalService,
        IEmailService emailService,
        ILogger<TrabajosController> logger)
    {
        _trabajosService = trabajosService;
        _opTrabajosService = opTrabajosService;
        _permisosService = permisosService;
        _gestionDocumentalService = gestionDocumentalService;
        _emailService = emailService;
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
    /// Modal de confirmación de cierre de trabajo con validación GD
    /// GET: /OP/Trabajos/ConfirmarCierre?trabajoId=123
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ConfirmarCierre(long trabajoId)
    {
        try
        {
            var trabajo = await _trabajosService.ObtenerPorIdAsync(trabajoId);
            if (trabajo == null)
            {
                return NotFound();
            }

            // Validar documentos de cierre (rol 10 = Gerente Operaciones para OP)
            const int rolResponsableCierre = 10;
            var (todosEncontrados, documentosFaltantes) = await _gestionDocumentalService
                .ValidarDocumentosEscaneadosAsync(trabajoId, rolResponsableCierre);

            var viewModel = new ConfirmarCierreVM
            {
                TrabajoId = trabajoId,
                NombreTrabajo = trabajo.Nombre ?? $"Trabajo {trabajoId}",
                JobBook = trabajo.JobBook,
                TodosDocumentosEncontrados = todosEncontrados,
                DocumentosFaltantes = documentosFaltantes,
                RolResponsableCierre = rolResponsableCierre
            };

            return PartialView("_ModalCerrarTrabajo", viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar modal de cierre para trabajo {TrabajoId}", trabajoId);
            return PartialView("_ModalCerrarTrabajo", new ConfirmarCierreVM
            {
                TrabajoId = trabajoId,
                NombreTrabajo = "Error",
                TodosDocumentosEncontrados = false,
                DocumentosFaltantes = new List<string> { "Error al validar documentos" }
            });
        }
    }

    /// <summary>
    /// Sincroniza documentos escaneados antes de validar cierre
    /// POST: /OP/Trabajos/SincronizarDocumentos
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SincronizarDocumentos(long trabajoId)
    {
        try
        {
            var trabajo = await _trabajosService.ObtenerPorIdAsync(trabajoId);
            if (trabajo == null)
            {
                return Json(new { success = false, message = "Trabajo no encontrado" });
            }

            // TODO: Obtener parámetros de red del trabajo (servidor, unidad, JBI, etc.)
            // Por ahora usamos valores de ejemplo
            const int rolResponsableCierre = 10;
            const string servidor = "co-file04";
            const string unidad = "D$";
            var jbi = trabajo.JobBook ?? "2025-01";
            var nombreTrabajo = trabajo.Nombre ?? $"Trabajo_{trabajoId}";

            var documentosActualizados = await _gestionDocumentalService.SincronizarDocumentosEscaneadosAsync(
                trabajoId,
                rolResponsableCierre,
                servidor,
                unidad,
                jbi,
                nombreTrabajo);

            _logger.LogInformation(
                "Sincronizados {Count} documentos para trabajo {TrabajoId}",
                documentosActualizados, trabajoId);

            // Volver a validar después de sincronizar
            var (todosEncontrados, documentosFaltantes) = await _gestionDocumentalService
                .ValidarDocumentosEscaneadosAsync(trabajoId, rolResponsableCierre);

            return Json(new
            {
                success = true,
                message = $"Sincronizados {documentosActualizados} documentos",
                todosEncontrados,
                documentosFaltantes
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al sincronizar documentos para trabajo {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error al sincronizar documentos" });
        }
    }

    /// <summary>
    /// Cierra el trabajo con validación de documentos GD
    /// POST: /OP/Trabajos/CerrarTrabajo
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CerrarTrabajo(long trabajoId, bool forzar = false, string? observaciones = null)
    {
        try
        {
            _logger.LogInformation("Cerrando trabajo {TrabajoId} (forzar: {Forzar})", trabajoId, forzar);

            var trabajo = await _trabajosService.ObtenerPorIdAsync(trabajoId);
            if (trabajo == null)
            {
                return Json(new { success = false, message = "Trabajo no encontrado" });
            }

            // Validar estado del trabajo
            // TODO: Verificar que el trabajo esté en estado "Activo" (EstadoId == X)
            // Por ahora omitimos validación de estado

            // Validar documentos GD si no se está forzando
            if (!forzar)
            {
                const int rolResponsableCierre = 10;
                var (todosEncontrados, documentosFaltantes) = await _gestionDocumentalService
                    .ValidarDocumentosEscaneadosAsync(trabajoId, rolResponsableCierre);

                if (!todosEncontrados)
                {
                    _logger.LogWarning(
                        "Intento de cerrar trabajo {TrabajoId} sin todos los documentos. Faltan: {Documentos}",
                        trabajoId, string.Join(", ", documentosFaltantes));

                    return Json(new
                    {
                        success = false,
                        requiereForzar = true,
                        message = $"Faltan {documentosFaltantes.Count} documento(s). Confirme si desea forzar el cierre.",
                        documentosFaltantes
                    });
                }
            }

            // Cambiar estado del trabajo a cerrado
            // TODO: Implementar IOpTrabajosService.CambiarEstadoAsync(trabajoId, estadoCerrado, observaciones)
            // Por ahora solo registramos el intento

            _logger.LogInformation(
                "Trabajo {TrabajoId} cerrado exitosamente (forzado: {Forzar})",
                trabajoId, forzar);

            // Enviar email de notificación de cierre
            // TODO: Implementar envío de email con plantilla de cierre
            try
            {
                var destinatarios = new List<string>(); // TODO: Obtener coordinadores del trabajo
                if (destinatarios.Any())
                {
                    await _emailService.EnviarMultipleAsync(
                        destinatarios,
                        $"Cierre de Trabajo: {trabajo.Nombre}",
                        GenerarCuerpoEmailCierre(trabajo.Nombre ?? "", trabajo.JobBook, forzar, observaciones));
                }
            }
            catch (Exception emailEx)
            {
                _logger.LogWarning(emailEx, "Error al enviar email de cierre para trabajo {TrabajoId}", trabajoId);
                // No fallar el cierre si el email falla
            }

            return Json(new
            {
                success = true,
                message = forzar
                    ? "Trabajo cerrado (cierre forzado)"
                    : "Trabajo cerrado exitosamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cerrar trabajo {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error al cerrar el trabajo" });
        }
    }

    /// <summary>
    /// Genera el cuerpo del email de notificación de cierre
    /// </summary>
    private string GenerarCuerpoEmailCierre(
        string nombreTrabajo,
        string? jobBook,
        bool forzado,
        string? observaciones)
    {
        var fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        var usuarioNombre = User.Identity?.Name ?? "Sistema";

        var html = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .header {{ background-color: #0066cc; color: white; padding: 20px; text-align: center; }}
        .content {{ padding: 20px; }}
        .info-box {{ background-color: #f4f4f4; border-left: 4px solid #0066cc; padding: 15px; margin: 15px 0; }}
        .warning {{ border-left-color: #ff9800; background-color: #fff3cd; }}
        .footer {{ font-size: 0.9em; color: #666; margin-top: 20px; padding-top: 20px; border-top: 1px solid #ddd; }}
    </style>
</head>
<body>
    <div class='header'>
        <h2>🔒 Notificación de Cierre de Trabajo</h2>
    </div>
    <div class='content'>
        <p>El siguiente trabajo ha sido cerrado en el sistema Matrix:</p>
        
        <div class='info-box'>
            <strong>Trabajo:</strong> {nombreTrabajo}<br/>
            <strong>JobBook:</strong> {jobBook ?? "N/A"}<br/>
            <strong>Fecha de Cierre:</strong> {fecha}<br/>
            <strong>Usuario:</strong> {usuarioNombre}
        </div>";

        if (forzado)
        {
            html += @"
        <div class='info-box warning'>
            <strong>⚠️ Cierre Forzado:</strong> Este trabajo fue cerrado sin completar todos los documentos de gestión documental.
        </div>";
        }

        if (!string.IsNullOrWhiteSpace(observaciones))
        {
            html += $@"
        <div class='info-box'>
            <strong>Observaciones:</strong><br/>
            {observaciones}
        </div>";
        }

        html += @"
        <p>Este es un correo automático generado por el sistema Matrix. No responda a este mensaje.</p>
        
        <div class='footer'>
            <p>Sistema de Gestión de Proyectos - Matrix<br/>
            Ipsos Colombia</p>
        </div>
    </div>
</body>
</html>";

        return html;
    }
}
