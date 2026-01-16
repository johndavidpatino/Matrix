using MatrixNext.Data.DTOs.INV;
using MatrixNext.Data.Services.INV;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.INV.Controllers
{
    /// <summary>
    /// Controller para gestión de registro de artículos de inventario
    /// Soporta 8 tipos: Computadores, Tabletas, Celulares, Consumibles, Periféricos, Papelería
    /// </summary>
    [Area("INV")]
    [Authorize]
    public class RegistroArticulosController : Controller
    {
        private readonly IRegistroArticulosService _service;
        private readonly ILogger<RegistroArticulosController> _logger;

        public RegistroArticulosController(
            IRegistroArticulosService service,
            ILogger<RegistroArticulosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene el ID del usuario autenticado
        /// </summary>
        private long GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return long.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        // GET: INV/RegistroArticulos
        public async Task<IActionResult> Index(
            string? filtro,
            long? idTipoArticulo,
            bool? asignado,
            int pagina = 1)
        {
            try
            {
                var articulos = await _service.ObtenerListadoAsync(
                    filtro,
                    idTipoArticulo,
                    asignado,
                    pagina,
                    pageSize: 20);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_ArticulosGrid", articulos);
                }

                // Cargar dropdowns para filtros
                ViewBag.TiposArticulo = await ObtenerTiposArticuloAsync();

                return View(articulos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener listado de artículos. Usuario: {UserId}", GetUserId());
                TempData["Error"] = "Error al cargar el listado de artículos";
                return View(new List<RegistroArticuloListDto>());
            }
        }

        // GET: INV/RegistroArticulos/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new RegistroArticuloDto();
                await CargarDropdownsAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_CreateEdit", model);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar formulario de creación. Usuario: {UserId}", GetUserId());
                return BadRequest(new { success = false, message = "Error al cargar el formulario" });
            }
        }

        // POST: INV/RegistroArticulos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegistroArticuloDto dto)
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
                _logger.LogError(ex, "Error al crear artículo. Usuario: {UserId}, Dto: {@Dto}", GetUserId(), dto);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al crear el artículo" });
                }

                TempData["Error"] = "Error al crear el artículo";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: INV/RegistroArticulos/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var articulo = await _service.ObtenerPorIdAsync(id);

                if (articulo == null)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Artículo no encontrado" });
                    }

                    TempData["Error"] = "Artículo no encontrado";
                    return RedirectToAction(nameof(Index));
                }

                // Validar que no esté asignado
                if (articulo.Asignado)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new
                        {
                            success = false,
                            message = "No se puede modificar un artículo que está asignado. Primero debe desasignarlo."
                        });
                    }

                    TempData["Warning"] = "No se puede modificar un artículo asignado";
                    return RedirectToAction(nameof(Index));
                }

                await CargarDropdownsAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_CreateEdit", articulo);
                }

                return View(articulo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar artículo {Id} para edición. Usuario: {UserId}", id, GetUserId());

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al cargar el artículo" });
                }

                TempData["Error"] = "Error al cargar el artículo";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: INV/RegistroArticulos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, RegistroArticuloDto dto)
        {
            try
            {
                if (id != dto.IdArticulo)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "ID de artículo no coincide" });
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
                _logger.LogError(ex, "Error al actualizar artículo {Id}. Usuario: {UserId}, Dto: {@Dto}", id, GetUserId(), dto);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al actualizar el artículo" });
                }

                TempData["Error"] = "Error al actualizar el artículo";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: INV/RegistroArticulos/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            try
            {
                var articulo = await _service.ObtenerPorIdAsync(id);

                if (articulo == null)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Artículo no encontrado" });
                    }

                    TempData["Error"] = "Artículo no encontrado";
                    return RedirectToAction(nameof(Index));
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_Details", articulo);
                }

                return View(articulo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar detalles del artículo {Id}. Usuario: {UserId}", id, GetUserId());

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al cargar los detalles" });
                }

                TempData["Error"] = "Error al cargar los detalles";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: INV/RegistroArticulos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                // Validar que no esté asignado antes de eliminar
                var articulo = await _service.ObtenerPorIdAsync(id);
                if (articulo == null)
                {
                    return Json(new { success = false, message = "Artículo no encontrado" });
                }

                if (articulo.Asignado)
                {
                    return Json(new
                    {
                        success = false,
                        message = "No se puede eliminar un artículo asignado. Primero debe desasignarlo."
                    });
                }

                var (success, message) = await _service.EliminarAsync(id, GetUserId());

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar artículo {Id}. Usuario: {UserId}", id, GetUserId());
                return Json(new { success = false, message = "Error al eliminar el artículo" });
            }
        }

        #region Helpers

        /// <summary>
        /// Carga dropdowns necesarios para el formulario
        /// </summary>
        private async Task CargarDropdownsAsync()
        {
            // TODO: Implementar llamadas a adapters para obtener catálogos
            // Por ahora usar valores hardcoded basados en WebMatrix

            ViewBag.TiposArticulo = new List<object>
            {
                new { Id = 1, Nombre = "Computador" },
                new { Id = 2, Nombre = "Tableta" },
                new { Id = 3, Nombre = "Celular" },
                new { Id = 4, Nombre = "Consumible" },
                new { Id = 5, Nombre = "Periférico" },
                new { Id = 6, Nombre = "Papelería" }
            };

            ViewBag.Marcas = new List<string> { "HP", "Dell", "Lenovo", "Apple", "Samsung", "LG", "Asus", "Acer" };
            ViewBag.SistemasOperativos = new List<string> { "Windows 10", "Windows 11", "Ubuntu", "macOS" };
            ViewBag.Procesadores = new List<string> { "Intel Core i3", "Intel Core i5", "Intel Core i7", "AMD Ryzen 5", "AMD Ryzen 7" };

            await Task.CompletedTask;
        }

        /// <summary>
        /// Obtiene lista de tipos de artículo para filtros
        /// </summary>
        private async Task<List<object>> ObtenerTiposArticuloAsync()
        {
            // TODO: Reemplazar con llamada a servicio de catálogos
            await Task.CompletedTask;

            return new List<object>
            {
                new { Id = 1, Nombre = "Computador" },
                new { Id = 2, Nombre = "Tableta" },
                new { Id = 3, Nombre = "Celular" },
                new { Id = 4, Nombre = "Consumible" },
                new { Id = 5, Nombre = "Periférico" },
                new { Id = 6, Nombre = "Papelería" }
            };
        }

        #endregion
    }
}
