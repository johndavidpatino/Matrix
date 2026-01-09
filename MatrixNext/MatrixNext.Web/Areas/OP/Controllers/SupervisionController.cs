using MatrixNext.Web.Services.OP;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controlador para supervisión telefónica de encuestas
/// </summary>
/// <remarks>
/// Migración de WebMatrix/OP_Cuantitativo/Supervision.aspx
/// GAP-OP-07: Se corrigió el uso de User.FindFirst(ClaimTypes.NameIdentifier)
/// Requiere permiso 157 (MySCall - Supervisión)
/// NOTA: Validación de permiso 157 se realizará mediante middleware o filtro global en futuras implementaciones
/// </remarks>
[Area("OP")]
[Authorize]
public class SupervisionController : Controller
{
    private readonly IOpSupervisionService _supervisionService;
    private readonly ILogger<SupervisionController> _logger;

    public SupervisionController(
        IOpSupervisionService supervisionService,
        ILogger<SupervisionController> logger)
    {
        _supervisionService = supervisionService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(long trabajoId, string? identificacion, long operadorId = 0, long supervisorId = 0)
    {
        if (trabajoId <= 0)
        {
            TempData["Error"] = "TrabajoId es requerido.";
            return View(new SupervisionViewModel());
        }

        var userId = GetCurrentUserId();
        if (userId == 0)
        {
            TempData["Error"] = "Usuario no autenticado correctamente.";
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        var usuarios = await _supervisionService.ObtenerUsuariosActivosAsync();
        var historico = await _supervisionService.ObtenerHistoricoAsync(trabajoId);
        var summary = await _supervisionService.ObtenerResumenAsync(trabajoId);

        var identificacionLong = long.TryParse(identificacion, out var parsedIdentificacion) ? parsedIdentificacion : 0;
        var model = new SupervisionViewModel
        {
            TrabajoId = trabajoId,
            Identificacion = identificacion ?? string.Empty,
            Operadores = usuarios,
            Supervisores = usuarios,
            Request = new GuardarSupervisionRequest
            {
                TrabajoId = trabajoId,
                Identificacion = identificacionLong,
                OperadorId = operadorId,
                SupervisorId = supervisorId,
                FechaSupervision = DateTime.UtcNow
            },
            Historico = historico,
            Summary = summary
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Guardar(GuardarSupervisionRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
        {
            return Forbid();
        }

        try
        {
            await _supervisionService.GuardarSupervisionAsync(request);
            
            _logger.LogInformation("Supervisión telefónica registrada: Trabajo {TrabajoId}, Identificación {Identificacion}, Usuario {UserId}",
                request.TrabajoId, request.Identificacion, userId);

            TempData["Success"] = "Supervisión telefónica registrada exitosamente.";
            return RedirectToAction(nameof(Index), new { trabajoId = request.TrabajoId, identificacion = request.Identificacion });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar supervisión para trabajo {TrabajoId}", request.TrabajoId);
            TempData["Error"] = "Error al registrar la supervisión telefónica.";
            return RedirectToAction(nameof(Index), new { trabajoId = request.TrabajoId, identificacion = request.Identificacion });
        }
    }

    /// <summary>
    /// Obtiene el ID del usuario actual desde los claims
    /// </summary>
    private long GetCurrentUserId()
    {
        var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(idClaim, out var id) ? id : 0;
    }
}
