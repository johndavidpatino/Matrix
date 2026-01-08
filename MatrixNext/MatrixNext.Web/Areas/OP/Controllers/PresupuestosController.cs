using MatrixNext.Web.Options;
using MatrixNext.Web.Services;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.OP.Controllers;

[Area("OP")]
[Authorize]
public class PresupuestosController : Controller
{
    private readonly IOpPresupuestosService _presupuestosService;
    private readonly IEmailService _emailService;
    private readonly ILogger<PresupuestosController> _logger;
    private readonly PresupuestoNotificationOptions _notificationOptions;

    public PresupuestosController(
        IOpPresupuestosService presupuestosService,
        IEmailService emailService,
        IOptions<PresupuestoNotificationOptions> notificationOptions,
        ILogger<PresupuestosController> logger)
    {
        _presupuestosService = presupuestosService;
        _emailService = emailService;
        _logger = logger;
        _notificationOptions = notificationOptions.Value;
    }

    [HttpGet]
    public async Task<IActionResult> Index(long trabajoId)
    {
        if (trabajoId <= 0)
        {
            TempData["PresupuestoMessage"] = "Proporcione un TrabajoId válido.";
            return View(new PresupuestoViewModel());
        }

        var estado = await _presupuestosService.ObtenerEstadoAsync(trabajoId);

        var model = new PresupuestoViewModel
        {
            TrabajoId = trabajoId,
            TieneSolicitud = estado?.TieneSolicitud ?? false,
            ObservacionActual = estado?.Observacion ?? string.Empty,
            Completo = new SolicitudPresupuestoCompletoRequest
            {
                TrabajoId = trabajoId,
                UsuarioId = GetCurrentUserId()
            },
            Simplificado = new SolicitudPresupuestoSimplificadoRequest
            {
                TrabajoId = trabajoId,
                UsuarioId = GetCurrentUserId()
            }
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GuardarCompleta(SolicitudPresupuestoCompletoRequest request)
    {
        var success = await _presupuestosService.GuardarSolicitudCompletaAsync(request);
        if (success)
        {
            await EnviarNotificacionPresupuestoAsync(request.TrabajoId, "completa", request.Observacion);
        }
        TempData["PresupuestoMessage"] = success ? "Solicitud de presupuesto completo guardada." : "No fue posible guardar la solicitud.";

        return RedirectToAction(nameof(Index), new { trabajoId = request.TrabajoId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GuardarSimplificada(SolicitudPresupuestoSimplificadoRequest request)
    {
        var success = await _presupuestosService.GuardarSolicitudSimplificadaAsync(request);
        if (success)
        {
            await EnviarNotificacionPresupuestoAsync(request.TrabajoId, "simplificada", request.Observacion);
        }
        TempData["PresupuestoMessage"] = success ? "Solicitud simplificada registrada." : "No fue posible guardar la observación.";

        return RedirectToAction(nameof(Index), new { trabajoId = request.TrabajoId });
    }

    private async Task EnviarNotificacionPresupuestoAsync(long trabajoId, string tipo, string observacion)
    {
        var recipients = _notificationOptions.Recipients?
            .Where(email => !string.IsNullOrWhiteSpace(email))
            .Select(email => email!.Trim())
            .ToArray();

        if (recipients == null || recipients.Length == 0)
        {
            _logger.LogDebug("No se enviará notificación de presupuesto ({Tipo}) para el trabajo {TrabajoId}: no hay destinatarios configurados.", tipo, trabajoId);
            return;
        }

        var subject = _notificationOptions.SubjectTemplate
            .Replace("{Tipo}", tipo, StringComparison.InvariantCultureIgnoreCase)
            .Replace("{TrabajoId}", trabajoId.ToString());

        var body = _notificationOptions.BodyTemplate
            .Replace("{Tipo}", tipo, StringComparison.InvariantCultureIgnoreCase)
            .Replace("{TrabajoId}", trabajoId.ToString())
            .Replace("{FechaUtc}", DateTime.UtcNow.ToString("s"))
            .Replace("{Observacion}", string.IsNullOrWhiteSpace(observacion) ? "Sin observación" : observacion);

        var userName = User?.Identity?.Name ?? GetCurrentUserId().ToString();
        body += "\r\nRegistrado por: " + userName;

        await _emailService.EnviarMultipleAsync(recipients.ToList(), subject, body);
        _logger.LogInformation("Notificación de presupuesto {Tipo} enviada para el trabajo {TrabajoId}", tipo, trabajoId);
    }

    private long GetCurrentUserId()
    {
        var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (long.TryParse(idClaim, out var id))
        {
            return id;
        }
        return 0;
    }
}
