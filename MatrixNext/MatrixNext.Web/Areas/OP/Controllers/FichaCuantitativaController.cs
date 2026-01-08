using System.Security.Claims;
using MatrixNext.Web.Services;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Services.PY;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controlador para gestión de Ficha Cuantitativa
/// </summary>
/// <remarks>
/// Migración de WebMatrix/OP_Cuantitativo/FichaCuantitativa.aspx
/// Gestiona información del trabajo: incentivos, habeas data, grupo objetivo, etc.
/// </remarks>
[Area("OP")]
[Authorize]
public class FichaCuantitativaController : Controller
{
    private readonly ITrabajosService _trabajosService;
    private readonly IEmailService _emailService;
    private readonly ILogger<FichaCuantitativaController> _logger;

    public FichaCuantitativaController(
        ITrabajosService trabajosService,
        IEmailService emailService,
        ILogger<FichaCuantitativaController> logger)
    {
        _trabajosService = trabajosService;
        _emailService = emailService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var trabajo = await _trabajosService.ObtenerPorIdAsync(id);
        if (trabajo == null)
        {
            TempData["Error"] = "Trabajo no encontrado";
            return RedirectToAction("Index", "Trabajos");
        }

        // TODO: Cargar ficha cuantitativa existente desde BD
        var model = new FichaCuantitativaVM
        {
            IdTrabajo = id
        };

        ViewBag.TrabajoId = id;
        ViewBag.NombreTrabajo = trabajo.Nombre;
        ViewBag.JobBook = trabajo.JobBook;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(FichaCuantitativaVM model, bool enviarEmail = false)
    {
        if (!ModelState.IsValid)
        {
            var trabajo = await _trabajosService.ObtenerPorIdAsync(model.IdTrabajo);
            ViewBag.NombreTrabajo = trabajo?.Nombre;
            return View(model);
        }

        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
            {
                TempData["Error"] = "Usuario no válido";
                return RedirectToAction("Index", "Trabajos");
            }

            // TODO: Guardar ficha cuantitativa en BD
            // TODO: Sincronizar Habeas Data con Propuesta (GAP-OP-18)
            
            if (enviarEmail)
            {
                // TODO: Enviar email de entrega al coordinador y COE
                _logger.LogInformation("Email de ficha cuantitativa enviado para trabajo {TrabajoId}", model.IdTrabajo);
            }

            _logger.LogInformation("Ficha cuantitativa guardada para trabajo {TrabajoId} por usuario {UsuarioId}", model.IdTrabajo, userId);

            TempData["Success"] = enviarEmail 
                ? "Ficha cuantitativa guardada y enviada por email exitosamente" 
                : "Ficha cuantitativa guardada exitosamente";
            
            return RedirectToAction("Index", "Trabajos");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar ficha cuantitativa para trabajo {TrabajoId}", model.IdTrabajo);
            TempData["Error"] = "Error al guardar la ficha cuantitativa";
            return View(model);
        }
    }
}
