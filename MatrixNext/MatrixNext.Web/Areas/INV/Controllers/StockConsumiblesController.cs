using MatrixNext.Data.DTOs.INV;
using MatrixNext.Data.Services.INV;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.INV.Controllers
{
    /// <summary>
    /// Controller para gestión de movimientos de stock de consumibles
    /// Tipo Movimiento: 1=Entrada, 2=Salida
    /// </summary>
    [Area("INV")]
    [Authorize]
    public class StockConsumiblesController : Controller
    {
        private readonly IStockConsumiblesService _service;
        private readonly ILogger<StockConsumiblesController> _logger;

        public StockConsumiblesController(
            IStockConsumiblesService service,
            ILogger<StockConsumiblesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        private long GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return long.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        // GET: INV/StockConsumibles
        public async Task<IActionResult> Index(
            string? filtro,
            long? idConsumible,
            int? tipoMovimiento,
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int pagina = 1)
        {
            try
            {
                var movimientos = await _service.ObtenerListadoAsync(
                    filtro,
                    idConsumible,
                    tipoMovimiento.HasValue ? (short?)tipoMovimiento.Value : null,
                    fechaDesde,
                    fechaHasta,
                    false,  // legalizado - no filters
                    pagina,
                    20);  // pageSize

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_StockGrid", movimientos);
                }

                // Cargar dropdown de consumibles para filtro
                ViewBag.Consumibles = await ObtenerConsumiblesAsync();

                return View(movimientos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener movimientos de stock. Usuario: {UserId}", GetUserId());
                TempData["Error"] = "Error al cargar movimientos de stock";
                return View(new List<StockConsumibleListDto>());
            }
        }

        // GET: INV/StockConsumibles/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new StockConsumibleDto
                {
                    Fecha = DateTime.Now
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
                _logger.LogError(ex, "Error al cargar formulario de movimiento. Usuario: {UserId}", GetUserId());
                return BadRequest(new { success = false, message = "Error al cargar el formulario" });
            }
        }

        // POST: INV/StockConsumibles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockConsumibleDto dto)
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
                _logger.LogError(ex, "Error al crear movimiento de stock. Usuario: {UserId}, Dto: {@Dto}", GetUserId(), dto);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error al registrar el movimiento" });
                }

                TempData["Error"] = "Error al registrar el movimiento";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: INV/StockConsumibles/GetStockDisponible/5
        [HttpGet]
        public async Task<IActionResult> GetStockDisponible(long idConsumible)
        {
            try
            {
                var stock = await _service.CalcularStockDisponibleAsync(idConsumible);
                return Json(new { success = true, stock });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al calcular stock disponible para consumible {IdConsumible}. Usuario: {UserId}", idConsumible, GetUserId());
                return Json(new { success = false, message = "Error al calcular stock disponible" });
            }
        }

        // GET: INV/StockConsumibles/PorLegalizar
        public async Task<IActionResult> PorLegalizar()
        {
            try
            {
                var movimientos = await _service.ObtenerPorLegalizarAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_PorLegalizarGrid", movimientos);
                }

                return View(movimientos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener movimientos por legalizar. Usuario: {UserId}", GetUserId());
                TempData["Error"] = "Error al cargar movimientos por legalizar";
                return View(new List<StockConsumibleListDto>());
            }
        }

        #region Helpers

        private async Task CargarDropdownsAsync()
        {
            ViewBag.Consumibles = await ObtenerConsumiblesAsync();
            ViewBag.TiposMovimiento = new List<object>
            {
                new { Id = 1, Nombre = "Entrada" },
                new { Id = 2, Nombre = "Salida" }
            };
        }

        private async Task<List<object>> ObtenerConsumiblesAsync()
        {
            // TODO: Implementar llamada a servicio de artículos para obtener solo consumibles (IdTipoArticulo = 4)
            await Task.CompletedTask;
            return new List<object>();
        }

        #endregion
    }
}
