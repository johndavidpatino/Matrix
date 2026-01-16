using MatrixNext.Data.DTOs.INV;
using MatrixNext.Data.Services.INV;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.INV.Controllers
{
    /// <summary>
    /// Controller para gestión de legalizaciones de consumibles
    /// Incluye componentes: Firmas, Devoluciones, Notas de Crédito, Descuento por Nómina
    /// Estado verificado protege contra modificaciones
    /// </summary>
    [Area("INV")]
    [Authorize]
    public class LegalizacionesController : Controller
    {
        private readonly ILegalizacionesService _service;
        private readonly ILogger<LegalizacionesController> _logger;

        public LegalizacionesController(
            ILegalizacionesService service,
            ILogger<LegalizacionesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        private long GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return long.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        // GET: INV/Legalizaciones
        public async Task<IActionResult> Index(
            string? filtro,
            long? idUsuario,
            bool? verificado,
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int pagina = 1)
        {
            try
            {
                var legalizaciones = await _service.ObtenerListadoAsync(
                    filtro,
                    verificado,
                    fechaDesde,
                    fechaHasta,
                    pagina,
                    20);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_LegalizacionesGrid", legalizaciones);
                }

                return View(legalizaciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener listado de legalizaciones. Usuario: {UserId}", GetUserId());
                TempData["Error"] = "Error al cargar el listado de legalizaciones";
                return View(new List<LegalizacionDto>());
            }
        }

        // GET: INV/Legalizaciones/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new LegalizacionDto
                {
                    FechaLegalizacion = DateTime.Now,
                    Verificado = false
                };

                await CargarDropdownsAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_CreateEdit", model);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar formulario de legalización. Usuario: {UserId}", GetUserId());
                return BadRequest(new { success = false, message = "Error al cargar el formulario" });
            }
        }

        // POST: INV/Legalizaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LegalizacionDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await CargarDropdownsAsync();

                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Por favor corrija los errores en el formulario",
                            errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                        });
                    }

                    return View(dto);
                }

                var (success, message, id) = await _service.CrearAsync(dto, GetUserId());

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success, message, id });
                }

                if (success)
                {
                    TempData["Success"] = message;
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", message);
                await CargarDropdownsAsync();
                return View(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear legalización. Usuario: {UserId}, Dto: {@Dto}", GetUserId(), dto);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al crear la legalización" });
                }

                TempData["Error"] = "Error al crear la legalización";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: INV/Legalizaciones/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var legalizacion = await _service.ObtenerPorIdAsync(id);

                if (legalizacion == null)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Legalización no encontrada" });
                    }

                    TempData["Error"] = "Legalización no encontrada";
                    return RedirectToAction(nameof(Index));
                }

                // Validar que no esté verificada
                if (legalizacion.Verificado)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new
                        {
                            success = false,
                            message = "No se puede modificar una legalización verificada"
                        });
                    }

                    TempData["Warning"] = "No se puede modificar una legalización verificada";
                    return RedirectToAction(nameof(Index));
                }

                await CargarDropdownsAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_CreateEdit", legalizacion);
                }

                return View(legalizacion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar legalización {Id} para edición. Usuario: {UserId}", id, GetUserId());

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al cargar la legalización" });
                }

                TempData["Error"] = "Error al cargar la legalización";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: INV/Legalizaciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, LegalizacionDto dto)
        {
            try
            {
                if (id != dto.IdLegalizacion)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "ID de legalización no coincide" });
                    }

                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    await CargarDropdownsAsync();

                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Por favor corrija los errores en el formulario",
                            errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                        });
                    }

                    return View(dto);
                }

                var (success, message) = await _service.ActualizarAsync(dto, GetUserId());

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success, message });
                }

                if (success)
                {
                    TempData["Success"] = message;
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", message);
                await CargarDropdownsAsync();
                return View(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar legalización {Id}. Usuario: {UserId}, Dto: {@Dto}", id, GetUserId(), dto);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al actualizar la legalización" });
                }

                TempData["Error"] = "Error al actualizar la legalización";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: INV/Legalizaciones/Verificar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Verificar(long id)
        {
            try
            {
                // Obtener legalización y marcar como verificada
                var legalizacion = await _service.ObtenerPorIdAsync(id);
                if (legalizacion == null)
                {
                    return Json(new { success = false, message = "Legalización no encontrada" });
                }

                if (legalizacion.Verificado)
                {
                    return Json(new { success = false, message = "La legalización ya está verificada" });
                }

                legalizacion.Verificado = true;
                var (success, message) = await _service.ActualizarAsync(legalizacion, GetUserId());

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar legalización {Id}. Usuario: {UserId}", id, GetUserId());
                return Json(new { success = false, message = "Error al verificar la legalización" });
            }
        }

        // POST: INV/Legalizaciones/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                // Validar que no esté verificada antes de eliminar
                var legalizacion = await _service.ObtenerPorIdAsync(id);
                if (legalizacion == null)
                {
                    return Json(new { success = false, message = "Legalización no encontrada" });
                }

                if (legalizacion.Verificado)
                {
                    return Json(new
                    {
                        success = false,
                        message = "No se puede eliminar una legalización verificada"
                    });
                }

                var (success, message) = await _service.EliminarAsync(id, GetUserId());
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar legalización {Id}. Usuario: {UserId}", id, GetUserId());
                return Json(new { success = false, message = "Error al eliminar la legalización" });
            }
        }

        #region Helpers

        private async Task CargarDropdownsAsync()
        {
            // TODO: Implementar llamadas a servicios para obtener:
            // - Usuarios activos
            // - Trabajos activos

            ViewBag.Usuarios = new List<object>(); // TODO
            ViewBag.Trabajos = new List<object>(); // TODO

            await Task.CompletedTask;
        }

        #endregion
    }
}
