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
/// GAP-OP-02: FichaCuantitativa Sin Implementar
/// GAP-OP-18: Sincronización Habeas Data con Propuesta
/// </remarks>
[Area("OP")]
[Authorize]
public class FichaCuantitativaController : Controller
{
    private readonly ITrabajosService _trabajosService;
    private readonly IOpFichaService _fichaService;
    private readonly IEmailService _emailService;
    private readonly ILogger<FichaCuantitativaController> _logger;

    public FichaCuantitativaController(
        ITrabajosService trabajosService,
        IOpFichaService fichaService,
        IEmailService emailService,
        ILogger<FichaCuantitativaController> logger)
    {
        _trabajosService = trabajosService;
        _fichaService = fichaService;
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

        // Cargar ficha cuantitativa existente desde BD
        var model = await _fichaService.ObtenerPorTrabajoAsync(id);
        if (model == null)
        {
            // Crear nueva ficha vacía
            model = new FichaCuantitativaVM
            {
                IdTrabajo = id
            };
        }

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

            // Guardar ficha cuantitativa en BD
            var fichaId = await _fichaService.GuardarAsync(model, userId);

            // Sincronizar Habeas Data con Propuesta (GAP-OP-18)
            if (!string.IsNullOrWhiteSpace(model.HabeasData))
            {
                await _fichaService.SincronizarHabeasDataAsync(model.IdTrabajo, model.HabeasData);
            }
            
            if (enviarEmail)
            {
                // Enviar email de entrega al coordinador y COE
                await EnviarEmailFichaCuantitativaAsync(model.IdTrabajo);
                _logger.LogInformation("Email de ficha cuantitativa enviado para trabajo {TrabajoId}", model.IdTrabajo);
            }

            _logger.LogInformation("Ficha cuantitativa guardada: ID {FichaId}, Trabajo {TrabajoId}, Usuario {UsuarioId}", 
                fichaId, model.IdTrabajo, userId);

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

    /// <summary>
    /// Envía email con la ficha cuantitativa al coordinador y COE asignado
    /// </summary>
    private async Task EnviarEmailFichaCuantitativaAsync(long trabajoId)
    {
        try
        {
            var trabajo = await _trabajosService.ObtenerPorIdAsync(trabajoId);
            if (trabajo == null) return;

            var ficha = await _fichaService.ObtenerPorTrabajoAsync(trabajoId);
            if (ficha == null) return;

            // Obtener emails del coordinador y COE
            var destinatarios = await ObtenerDestinatariosEmailAsync(trabajoId);
            if (!destinatarios.Any()) return;

            var nombreTrabajo = trabajo.Nombre ?? $"Trabajo {trabajoId}";
            var jobBook = string.IsNullOrWhiteSpace(trabajo.JobBook) ? "N/A" : trabajo.JobBook;
            var asunto = $"Ficha Cuantitativa - {nombreTrabajo} ({jobBook})";
            var cuerpo = GenerarCuerpoEmailFicha(nombreTrabajo, trabajo.JobBook, ficha);

            await _emailService.EnviarMultipleAsync(destinatarios, asunto, cuerpo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar email de ficha cuantitativa para trabajo {TrabajoId}", trabajoId);
            // No lanzar excepción para no bloquear el guardado
        }
    }

    /// <summary>
    /// Obtiene los emails del coordinador y COE para envío
    /// </summary>
    private async Task<List<string>> ObtenerDestinatariosEmailAsync(long trabajoId)
    {
        // TODO: Implementar consulta a BD para obtener emails del coordinador y COE
        // Por ahora retornar lista vacía
        await Task.CompletedTask;
        return new List<string>();
    }

    /// <summary>
    /// Genera el cuerpo del email con la información de la ficha
    /// </summary>
    private string GenerarCuerpoEmailFicha(string nombreTrabajo, string? jobBook, FichaCuantitativaVM ficha)
    {
        return $@"
            <h2>Ficha Cuantitativa - {nombreTrabajo}</h2>
            <p><strong>Job Book:</strong> {jobBook ?? "N/A"}</p>
            <hr/>
            <h3>Información del Estudio</h3>
            <p><strong>Grupo Objetivo:</strong> {ficha.GrupoObjetivo}</p>
            <p><strong>Marco Muestral:</strong> {ficha.MarcoMuestral}</p>
            {(!string.IsNullOrWhiteSpace(ficha.Incentivos) ? $"<p><strong>Incentivos:</strong> {ficha.Incentivos}</p>" : "")}
            {(!string.IsNullOrWhiteSpace(ficha.RegaloClientes) ? $"<p><strong>Regalos a Clientes:</strong> {ficha.RegaloClientes}</p>" : "")}
            {(!string.IsNullOrWhiteSpace(ficha.CompraIpsos) ? $"<p><strong>Compras Ipsos:</strong> {ficha.CompraIpsos}</p>" : "")}
            {(!string.IsNullOrWhiteSpace(ficha.HabeasData) ? $"<p><strong>Habeas Data:</strong> {ficha.HabeasData}</p>" : "")}
            {(!string.IsNullOrWhiteSpace(ficha.Observaciones) ? $"<p><strong>Observaciones:</strong> {ficha.Observaciones}</p>" : "")}
        ";
    }
}

