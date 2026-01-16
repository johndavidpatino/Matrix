using MatrixNext.Data.DTOs.INV;
using MatrixNext.Data.Services.INV;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.INV.Controllers
{
    /// <summary>
    /// Controller para gestión de historial de mantenimientos de equipos
    /// Registra reparaciones, actualizaciones y servicios técnicos
    /// </summary>
    [Area("INV")]
    [Authorize]
    public class MantenimientoEquiposController : Controller
    {
        private readonly IMantenimientoEquiposService _service;
        private readonly ILogger<MantenimientoEquiposController> _logger;

        public MantenimientoEquiposController(
            IMantenimientoEquiposService service,
            ILogger<MantenimientoEquiposController> logger)
        {
            _service = service;
            _logger = logger;
        }

        private long GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return long.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        // GET: INV/MantenimientoEquipos
        public async Task<IActionResult> Index(
            string? filtro,
            long? idActivo,
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int pagina = 1)
        {
            try
            {
                var mantenimientos = await _service.ObtenerListadoAsync(
                    filtro,
                    idActivo,
                    fechaDesde,
                    fechaHasta,
                    pagina,
                    pageSize: 20);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_MantenimientosGrid", mantenimientos);
                }

                return View(mantenimientos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener listado de mantenimientos. Usuario: {UserId}", GetUserId());
                TempData["Error"] = "Error al cargar el listado de mantenimientos";
                return View(new List<MantenimientoEquipoDto>());
            }
        }

        // GET: INV/MantenimientoEquipos/PorActivo/5
        public async Task<IActionResult> PorActivo(long idActivo)
        {
            try
            {
                var mantenimientos = await _service.ObtenerPorActivoAsync(idActivo);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_HistorialMantenimiento", mantenimientos);
                }

                ViewBag.IdActivo = idActivo;
                return View(mantenimientos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener historial de mantenimientos para activo {IdActivo}. Usuario: {UserId}", idActivo, GetUserId());
                TempData["Error"] = "Error al cargar el historial de mantenimientos";
                return View(new List<MantenimientoEquipoDto>());
            }
        }

        // GET: INV/MantenimientoEquipos/Create
        [HttpGet]
        public async Task<IActionResult> Create(long? idActivo)
        {
            try
            {
                var model = new MantenimientoEquipoDto
                {
                    Fecha = DateTime.Now,
                    IdActivoFijo = idActivo ?? 0
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
                _logger.LogError(ex, "Error al cargar formulario de mantenimiento. Usuario: {UserId}", GetUserId());
                return BadRequest(new { success = false, message = "Error al cargar el formulario" });
            }
        }

        // POST: INV/MantenimientoEquipos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MantenimientoEquipoDto dto)
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

                    // Si vino de un activo específico, redirigir al historial de ese activo
                    if (dto.IdActivoFijo > 0)
                    {
                        return RedirectToAction(nameof(PorActivo), new { idActivo = dto.IdActivoFijo });
                    }

                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", message);
                await CargarDropdownsAsync();
                return View(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear mantenimiento. Usuario: {UserId}, Dto: {@Dto}", GetUserId(), dto);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al registrar el mantenimiento" });
                }

                TempData["Error"] = "Error al registrar el mantenimiento";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: INV/MantenimientoEquipos/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var mantenimiento = await _service.ObtenerPorIdAsync(id);

                if (mantenimiento == null)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Mantenimiento no encontrado" });
                    }

                    TempData["Error"] = "Mantenimiento no encontrado";
                    return RedirectToAction(nameof(Index));
                }

                await CargarDropdownsAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_CreateEdit", mantenimiento);
                }

                return View(mantenimiento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar mantenimiento {Id} para edición. Usuario: {UserId}", id, GetUserId());

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al cargar el mantenimiento" });
                }

                TempData["Error"] = "Error al cargar el mantenimiento";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: INV/MantenimientoEquipos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, MantenimientoEquipoDto dto)
        {
            try
            {
                if (id != dto.IdMantenimiento)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "ID de mantenimiento no coincide" });
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
                _logger.LogError(ex, "Error al actualizar mantenimiento {Id}. Usuario: {UserId}, Dto: {@Dto}", id, GetUserId(), dto);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al actualizar el mantenimiento" });
                }

                TempData["Error"] = "Error al actualizar el mantenimiento";
                return RedirectToAction(nameof(Index));
            }
        }

        #region Helpers

        private async Task CargarDropdownsAsync()
        {
            // TODO: Implementar llamada a servicio de artículos para obtener activos fijos
            ViewBag.ActivosFijos = new List<object>(); // TODO

            await Task.CompletedTask;
        }

        #endregion
    }
}
