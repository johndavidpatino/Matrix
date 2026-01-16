using MatrixNext.Data.DTOs.INV;
using MatrixNext.Data.Services.INV;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.INV.Controllers
{
    /// <summary>
    /// Controller para gestión de asignaciones de activos fijos a usuarios
    /// Incluye workflow: crear asignación → actualizar estado artículo → crear log auditoría
    /// </summary>
    [Area("INV")]
    [Authorize]
    public class AsignacionesController : Controller
    {
        private readonly IAsignacionesService _service;
        private readonly ILogger<AsignacionesController> _logger;

        public AsignacionesController(
            IAsignacionesService service,
            ILogger<AsignacionesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        private long GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return long.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        // GET: INV/Asignaciones
        public async Task<IActionResult> Index(
            string? filtro,
            long? idUsuarioAsignado,
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int pagina = 1)
        {
            try
            {
                var asignaciones = await _service.ObtenerListadoAsync(
                    filtro,
                    idUsuarioAsignado,
                    fechaDesde,
                    fechaHasta,
                    pagina,
                    pageSize: 20);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_AsignacionesGrid", asignaciones);
                }

                return View(asignaciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener listado de asignaciones. Usuario: {UserId}", GetUserId());
                TempData["Error"] = "Error al cargar el listado de asignaciones";
                return View(new List<AsignacionListDto>());
            }
        }

        // GET: INV/Asignaciones/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new AsignacionActivoDto
                {
                    FechaAsignacion = DateTime.Now
                };

                // Cargar dropdowns de activos disponibles y usuarios
                await CargarDropdownsAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_CreateEdit", model);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar formulario de asignación. Usuario: {UserId}", GetUserId());
                return BadRequest(new { success = false, message = "Error al cargar el formulario" });
            }
        }

        // POST: INV/Asignaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AsignacionActivoDto dto)
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
                _logger.LogError(ex, "Error al crear asignación. Usuario: {UserId}, Dto: {@Dto}", GetUserId(), dto);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al crear la asignación" });
                }

                TempData["Error"] = "Error al crear la asignación";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: INV/Asignaciones/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var asignacion = await _service.ObtenerPorIdAsync(id);

                if (asignacion == null)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Asignación no encontrada" });
                    }

                    TempData["Error"] = "Asignación no encontrada";
                    return RedirectToAction(nameof(Index));
                }

                await CargarDropdownsAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_CreateEdit", asignacion);
                }

                return View(asignacion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar asignación {Id} para edición. Usuario: {UserId}", id, GetUserId());

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al cargar la asignación" });
                }

                TempData["Error"] = "Error al cargar la asignación";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: INV/Asignaciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, AsignacionActivoDto dto)
        {
            try
            {
                if (id != dto.IdAsignacion)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "ID de asignación no coincide" });
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
                _logger.LogError(ex, "Error al actualizar asignación {Id}. Usuario: {UserId}, Dto: {@Dto}", id, GetUserId(), dto);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al actualizar la asignación" });
                }

                TempData["Error"] = "Error al actualizar la asignación";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: INV/Asignaciones/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            try
            {
                var asignacion = await _service.ObtenerPorIdAsync(id);

                if (asignacion == null)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Asignación no encontrada" });
                    }

                    TempData["Error"] = "Asignación no encontrada";
                    return RedirectToAction(nameof(Index));
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_Details", asignacion);
                }

                return View(asignacion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar detalles de asignación {Id}. Usuario: {UserId}", id, GetUserId());

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al cargar los detalles" });
                }

                TempData["Error"] = "Error al cargar los detalles";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: INV/Asignaciones/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var (success, message) = await _service.EliminarAsync(id, GetUserId());
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar asignación {Id}. Usuario: {UserId}", id, GetUserId());
                return Json(new { success = false, message = "Error al eliminar la asignación" });
            }
        }

        #region Helpers

        private async Task CargarDropdownsAsync()
        {
            // TODO: Implementar llamadas a servicios para obtener:
            // - Activos disponibles (no asignados)
            // - Usuarios activos

            ViewBag.ActivosDisponibles = new List<object>(); // TODO
            ViewBag.Usuarios = new List<object>(); // TODO

            await Task.CompletedTask;
        }

        #endregion
    }
}
