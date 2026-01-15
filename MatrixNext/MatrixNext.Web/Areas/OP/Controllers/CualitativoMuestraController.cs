using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Models.OP;
using MatrixNext.Web.Services;
using MatrixNext.Web.Services.OP;
using MatrixNext.Data.Services;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controller para gestión de muestra por ciudad (versión Cualitativo)
/// Reutiliza IOpMuestraService y ofrece vista unificada
/// </summary>
[Area("OP")]
[Authorize]
[Route("OP/Cualitativo/Muestra")]
public class CualitativoMuestraController : Controller
{
    private readonly IOpMuestraService _muestraService;
    private readonly IOpBulkImportService _bulkImportService;
    private readonly IEmailQueueService _emailQueueService;
    private readonly ILogger<CualitativoMuestraController> _logger;

    public CualitativoMuestraController(
        IOpMuestraService muestraService,
        IOpBulkImportService bulkImportService,
        IEmailQueueService emailQueueService,
        ILogger<CualitativoMuestraController> logger)
    {
        _muestraService = muestraService;
        _bulkImportService = bulkImportService;
        _emailQueueService = emailQueueService;
        _logger = logger;
    }

    /// <summary>
    /// Listado de muestra por trabajo
    /// </summary>
    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(long trabajoId)
    {
        if (trabajoId <= 0)
        {
            TempData["Error"] = "TrabajoId inválido";
            return RedirectToAction("Index", "CualitativoTrabajos", new { area = "OP" });
        }

        try
        {
            var muestras = await _muestraService.ObtenerMuestraPorTrabajoAsync(trabajoId);
            var historial = await _bulkImportService.ObtenerHistorialImportsAsync(trabajoId);
            
            ViewBag.TrabajoId = trabajoId;
            ViewBag.Historial = historial;
            
            return View(muestras);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando muestra trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error cargando la muestra";
            return RedirectToAction("Index", "CualitativoTrabajos", new { area = "OP" });
        }
    }

    /// <summary>
    /// Crear nueva muestra
    /// </summary>
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MuestraCiudadVM model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Datos inválidos";
            return RedirectToAction("Index", new { trabajoId = model.TrabajoId });
        }

        try
        {
            var id = await _muestraService.GuardarMuestraAsync(model);
            TempData["Success"] = "Muestra creada";

            // Notificación por email (encolada)
            var detalle = await _muestraService.ObtenerDetalleMuestraParaEmailAsync(id);
            if (detalle != null && !string.IsNullOrWhiteSpace(detalle.CoordinadorEmail))
            {
                var asunto = $"Nueva Muestra - {detalle.Ciudad}";
                var cuerpo = $@"<p>Se ha creado una nueva muestra.</p>
                    <ul>
                        <li><strong>Trabajo:</strong> {detalle.TrabajoId}</li>
                        <li><strong>Ciudad:</strong> {detalle.Ciudad}, {detalle.Departamento}</li>
                        <li><strong>Cantidad:</strong> {detalle.Cantidad}</li>
                        <li><strong>Fecha Inicio:</strong> {detalle.FechaInicio:dd/MM/yyyy}</li>
                        <li><strong>Fecha Fin:</strong> {detalle.FechaFin:dd/MM/yyyy}</li>
                    </ul>";
                await _emailQueueService.QueueEmailAsync(detalle.CoordinadorEmail!, asunto, cuerpo, esHtml: true);
            }

            return RedirectToAction("Index", new { trabajoId = model.TrabajoId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando muestra trabajo {TrabajoId}", model.TrabajoId);
            TempData["Error"] = "Error creando muestra";
            return RedirectToAction("Index", new { trabajoId = model.TrabajoId });
        }
    }

    /// <summary>
    /// Actualizar muestra
    /// </summary>
    [HttpPost("Update")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(MuestraCiudadVM model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Datos inválidos";
            return RedirectToAction("Index", new { trabajoId = model.TrabajoId });
        }

        try
        {
            await _muestraService.GuardarMuestraAsync(model);
            TempData["Success"] = "Muestra actualizada";

            // Notificación por email (encolada)
            if (model.Id.HasValue)
            {
                var detalle = await _muestraService.ObtenerDetalleMuestraParaEmailAsync(model.Id.Value);
                if (detalle != null && !string.IsNullOrWhiteSpace(detalle.CoordinadorEmail))
                {
                    var asunto = $"Actualización de Muestra - {detalle.Ciudad}";
                    var cuerpo = $@"<p>La muestra ha sido actualizada.</p>
                        <ul>
                            <li><strong>Trabajo:</strong> {detalle.TrabajoId}</li>
                            <li><strong>Ciudad:</strong> {detalle.Ciudad}, {detalle.Departamento}</li>
                            <li><strong>Cantidad:</strong> {detalle.Cantidad}</li>
                            <li><strong>Fecha Inicio:</strong> {detalle.FechaInicio:dd/MM/yyyy}</li>
                            <li><strong>Fecha Fin:</strong> {detalle.FechaFin:dd/MM/yyyy}</li>
                        </ul>";
                    await _emailQueueService.QueueEmailAsync(detalle.CoordinadorEmail!, asunto, cuerpo, esHtml: true);
                }
            }

            return RedirectToAction("Index", new { trabajoId = model.TrabajoId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando muestra {Id}", model.Id);
            TempData["Error"] = "Error actualizando muestra";
            return RedirectToAction("Index", new { trabajoId = model.TrabajoId });
        }
    }

    /// <summary>
    /// Eliminar muestra
    /// </summary>
    [HttpPost("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id, long trabajoId)
    {
        try
        {
            var ok = await _muestraService.EliminarMuestraAsync(id);
            TempData[ok ? "Success" : "Error"] = ok ? "Muestra eliminada" : "No se pudo eliminar";
            return RedirectToAction("Index", new { trabajoId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando muestra {Id}", id);
            TempData["Error"] = "Error eliminando muestra";
            return RedirectToAction("Index", new { trabajoId });
        }
    }

    /// <summary>
    /// Actualizar fechas y auto-planeación
    /// </summary>
    [HttpPost("UpdateDates")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateDates(ActualizarFechasMuestraVM model, long trabajoId)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Datos de fechas inválidos";
            return RedirectToAction("Index", new { trabajoId });
        }

        try
        {
            var ok = await _muestraService.ActualizarFechasConPlaneacionAsync(model);
            TempData[ok ? "Success" : "Error"] = ok ? "Fechas actualizadas" : "Error al actualizar";
            return RedirectToAction("Index", new { trabajoId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en auto-planeación muestra {IdMuestra}", model.IdMuestra);
            TempData["Error"] = "Error actualizando fechas";
            return RedirectToAction("Index", new { trabajoId });
        }
    }

    /// <summary>
    /// Importar CSV de muestras
    /// Formato: CiudadId,Cantidad,FechaInicio,FechaFin,CoordinadorId
    /// </summary>
    [HttpPost("ImportCsv")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ImportCsv(long trabajoId)
    {
        var file = Request.Form.Files.FirstOrDefault();
        if (file == null || file.Length == 0)
        {
            TempData["Error"] = "Archivo CSV inválido";
            return RedirectToAction("Index", new { trabajoId });
        }

        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            int line = 0, imported = 0;
            while (!reader.EndOfStream)
            {
                var row = await reader.ReadLineAsync();
                line++;
                if (string.IsNullOrWhiteSpace(row)) continue;
                // Skip header if present
                if (line == 1 && row.Contains("CiudadId") && row.Contains("Cantidad")) continue;

                var cols = row.Split(',');
                if (cols.Length < 2) continue;

                int ciudadId = int.Parse(cols[0].Trim());
                double cantidad = double.Parse(cols[1].Trim());
                DateTime? fechaInicio = cols.Length > 2 && DateTime.TryParse(cols[2].Trim(), out var fi) ? fi : null;
                DateTime? fechaFin = cols.Length > 3 && DateTime.TryParse(cols[3].Trim(), out var ff) ? ff : null;
                long? coordinadorId = cols.Length > 4 && long.TryParse(cols[4].Trim(), out var cid) ? cid : null;

                var vm = new MuestraCiudadVM
                {
                    TrabajoId = trabajoId,
                    CiudadId = ciudadId,
                    Cantidad = cantidad,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    CoordinadorId = coordinadorId
                };

                await _muestraService.GuardarMuestraAsync(vm);
                imported++;
            }

            TempData["Success"] = $"Importación completada: {imported} registros";
            return RedirectToAction("Index", new { trabajoId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importando CSV de muestra trabajo {TrabajoId}", trabajoId);
            TempData["Error"] = "Error importando CSV";
            return RedirectToAction("Index", new { trabajoId });
        }
    }

    /// <summary>
    /// Mostrar modal de Bulk Import
    /// Sprint 6 Fase 6: Bulk Import
    /// </summary>
    [HttpGet("BulkImport")]
    public async Task<IActionResult> BulkImport(long trabajoId)
    {
        ViewBag.TrabajoId = trabajoId;
        return PartialView("_BulkImportModal");
    }

    /// <summary>
    /// Descargar plantilla Excel para import
    /// </summary>
    [HttpGet("DescargarPlantilla")]
    public async Task<IActionResult> DescargarPlantilla()
    {
        try
        {
            var datos = await _bulkImportService.GenerarPlantillaExcelAsync();
            return File(datos, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Plantilla_Muestras_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error descargando plantilla");
            return BadRequest("Error descargando plantilla");
        }
    }

    /// <summary>
    /// Procesar upload bulk de muestras (Excel/CSV)
    /// </summary>
    [HttpPost("ProcesarBulkImport")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProcesarBulkImport(long trabajoId, IFormFile archivo)
    {
        try
        {
            if (archivo == null || archivo.Length == 0)
                return Json(new { success = false, message = "Debe seleccionar un archivo" });

            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            using var stream = archivo.OpenReadStream();
            var (success, insertados, errores, mensajes) = await _bulkImportService.ImportarMuestrasAsync(
                stream, archivo.FileName, trabajoId, usuarioId);

            return Json(new
            {
                success = success,
                insertados,
                errores,
                mensajes = string.Join("<br>", mensajes)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando bulk import trabajo {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error procesando archivo" });
        }
    }

    /// <summary>
    /// Obtener historial de imports del trabajo
    /// </summary>
    [HttpGet("HistorialImports")]
    public async Task<IActionResult> HistorialImports(long trabajoId)
    {
        try
        {
            var historial = await _bulkImportService.ObtenerHistorialImportsAsync(trabajoId);
            return PartialView("_ImportHistorial", historial);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo historial imports {TrabajoId}", trabajoId);
            return BadRequest("Error obteniendo historial");
        }
    }
}