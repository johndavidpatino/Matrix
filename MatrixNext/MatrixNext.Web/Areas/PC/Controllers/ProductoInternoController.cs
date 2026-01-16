using MatrixNext.Data.DTOs.PC;
using MatrixNext.Data.Services.PC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.PC.Controllers
{
    /// <summary>
    /// Controller para gestión de productos internos (inventario entre unidades)
    /// </summary>
    [Area("PC")]
    [Authorize]
    public class ProductoInternoController : Controller
    {
        private readonly IProductoInternoService _service;
        private readonly ILogger<ProductoInternoController> _logger;

        public ProductoInternoController(
            IProductoInternoService service,
            ILogger<ProductoInternoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene el ID del usuario actual
        /// </summary>
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        // GET: PC/ProductoInterno
        public async Task<IActionResult> Index(int? unidadId = null, int? proyectoId = null, bool soloEnviados = false, bool soloPendientes = false)
        {
            try
            {
                var productos = await _service.ObtenerFiltradosAsync(unidadId, proyectoId, soloEnviados, soloPendientes);
                
                ViewBag.UnidadId = unidadId;
                ViewBag.ProyectoId = proyectoId;
                ViewBag.SoloEnviados = soloEnviados;
                ViewBag.SoloPendientes = soloPendientes;

                return View(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando listado de productos internos");
                TempData["Error"] = "Error al cargar los productos. Por favor intente nuevamente.";
                return View(new List<ProductoInternoListDto>());
            }
        }

        // GET: PC/ProductoInterno/Create
        [HttpGet]
        public IActionResult Create()
        {
            var model = new ProductoInternoDto
            {
                FechaEnvio = DateTime.Now,
                Envia = GetUserId()
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreateEdit", model);
            }

            return View("_CreateEdit", model);
        }

        // POST: PC/ProductoInterno/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoInternoDto dto)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_CreateEdit", dto);
                }
                return View("_CreateEdit", dto);
            }

            var userId = GetUserId();
            dto.Envia = userId;

            var (success, message, id) = await _service.CrearAsync(dto, userId);

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
            return View("_CreateEdit", dto);
        }

        // GET: PC/ProductoInterno/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var producto = await _service.ObtenerPorIdAsync(id);
                if (producto == null)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Producto no encontrado" });
                    }
                    TempData["Error"] = "Producto no encontrado";
                    return RedirectToAction(nameof(Index));
                }

                // Validar permisos
                var userId = GetUserId();
                var puedeEditar = await _service.PuedeEditarAsync(id, userId);
                if (!puedeEditar)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "No tiene permisos para editar este producto" });
                    }
                    TempData["Error"] = "No tiene permisos para editar este producto";
                    return RedirectToAction(nameof(Index));
                }

                var dto = new ProductoInternoDto
                {
                    Id = producto.Id,
                    ProyectoId = producto.ProyectoId,
                    FechaEnvio = producto.FechaEnvio,
                    UnidadEnvia = producto.UnidadEnvia,
                    UnidadRecibe = producto.UnidadRecibe,
                    Tipo = producto.Tipo,
                    Producto = producto.Producto,
                    Descripcion = producto.Descripcion,
                    Cantidad = producto.Cantidad,
                    Envia = producto.Envia,
                    Recibe = producto.Recibe,
                    FechaRecepcion = producto.FechaRecepcion,
                    Observaciones = producto.Observaciones
                };

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_CreateEdit", dto);
                }

                return View("_CreateEdit", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando producto {Id} para editar", id);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al cargar el producto" });
                }
                TempData["Error"] = "Error al cargar el producto";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: PC/ProductoInterno/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductoInternoDto dto)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_CreateEdit", dto);
                }
                return View("_CreateEdit", dto);
            }

            var userId = GetUserId();
            var (success, message) = await _service.ActualizarAsync(dto, userId);

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
            return View("_CreateEdit", dto);
        }

        // GET: PC/ProductoInterno/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var producto = await _service.ObtenerPorIdAsync(id);
                if (producto == null)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Producto no encontrado" });
                    }
                    return NotFound();
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_Details", producto);
                }

                return View("_Details", producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando detalles del producto {Id}", id);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al cargar los detalles" });
                }
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: PC/ProductoInterno/Recibir/5
        [HttpGet]
        public async Task<IActionResult> Recibir(int id)
        {
            try
            {
                var producto = await _service.ObtenerPorIdAsync(id);
                if (producto == null)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Producto no encontrado" });
                    }
                    return NotFound();
                }

                var dto = new EnvioRecepcionDto
                {
                    Id = producto.Id,
                    Producto = producto.Producto,
                    Descripcion = producto.Descripcion,
                    Cantidad = producto.Cantidad,
                    UnidadOrigen = producto.UnidadEnviaNombre,
                    UnidadDestino = producto.UnidadRecibeNombre,
                    FechaEnvio = producto.FechaEnvio,
                    FechaRecepcion = DateTime.Now,
                    RecibeUsuarioId = GetUserId()
                };

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_Recibir", dto);
                }

                return View("_Recibir", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando formulario de recepción para producto {Id}", id);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al cargar el formulario" });
                }
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: PC/ProductoInterno/Recibir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recibir(EnvioRecepcionDto dto)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_Recibir", dto);
                }
                return View("_Recibir", dto);
            }

            var userId = GetUserId();
            var (success, message) = await _service.RegistrarRecepcionAsync(dto.Id, userId, dto.Observaciones);

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
            return View("_Recibir", dto);
        }

        // POST: PC/ProductoInterno/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var (success, message) = await _service.EliminarAsync(id, userId);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success, message });
            }

            if (success)
            {
                TempData["Success"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
