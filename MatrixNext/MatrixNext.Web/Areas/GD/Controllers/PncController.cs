using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MatrixNext.Data.Models.GD;
using MatrixNext.Data.Services.GD.Interfaces;
using MatrixNext.Web.Infrastructure;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MatrixNext.Web.Areas.GD.Controllers
{
    /// <summary>
    /// Controller para Productos No Conformes (PNC)
    /// Migración de: WebMatrix/GD_Documentos/ProductoNoConformeRegistrar.aspx, ProductosNoConformeRelacion.aspx
    /// </summary>
    [Area("GD")]
    [Authorize]
    public class PncController : Controller
    {
        private readonly IGdPncService _service;
        private readonly ILogger<PncController> _logger;

        public PncController(IGdPncService service, ILogger<PncController> logger)
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
            return long.TryParse(userIdClaim, out long userId) ? userId : 0;
        }

        /// <summary>
        /// Página principal de PNC - Lista y seguimiento
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Productos No Conformes";
            
            var usuarioId = GetUserId();
            var viewModel = await _service.PrepararViewModelAsync(null, usuarioId);
            
            return View(viewModel);
        }

        /// <summary>
        /// Buscar PNC con filtros
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Buscar([FromQuery] PncBusquedaParams filtros)
        {
            var usuarioId = GetUserId();
            var viewModel = await _service.PrepararViewModelAsync(filtros, usuarioId);
            
            if (Request.IsAjaxRequest())
                return PartialView("_ListaPnc", viewModel.Productos);
            
            return View("Index", viewModel);
        }

        /// <summary>
        /// Obtener seguimiento de PNC por estado
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Seguimiento(byte? estado)
        {
            var (success, data) = await _service.ObtenerSeguimientoAsync(estado);
            
            if (Request.IsAjaxRequest())
                return PartialView("_Seguimiento", data);
            
            return Json(new { success, data });
        }

        /// <summary>
        /// Formulario para crear nuevo PNC (modal)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new PncCrearDto();
            await CargarCatalogosParaCrear();
            
            if (Request.IsAjaxRequest())
                return PartialView("_CreateModal", viewModel);
            
            return View("_CreateModal", viewModel);
        }

        /// <summary>
        /// Crear nuevo PNC
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PncCrearDto dto)
        {
            if (!ModelState.IsValid)
            {
                await CargarCatalogosParaCrear();
                
                if (Request.IsAjaxRequest())
                    return PartialView("_CreateModal", dto);
                
                return View("_CreateModal", dto);
            }

            var usuarioId = GetUserId();
            var (success, idCreado, message) = await _service.CrearPncAsync(dto, usuarioId);

            if (Request.IsAjaxRequest())
                return Json(new { success, id = idCreado, message });

            if (success)
            {
                TempData["Success"] = message;
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = message;
            await CargarCatalogosParaCrear();
            return View("_CreateModal", dto);
        }

        /// <summary>
        /// Ver detalle de un PNC (modal)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Detalle(long id)
        {
            var (success, viewModel) = await _service.ObtenerDetalleCompletoAsync(id);
            
            if (!success || viewModel == null)
            {
                if (Request.IsAjaxRequest())
                    return Json(new { success = false, message = "PNC no encontrado" });
                
                TempData["Error"] = "Producto No Conforme no encontrado";
                return RedirectToAction(nameof(Index));
            }

            if (Request.IsAjaxRequest())
                return PartialView("_DetallesModal", viewModel);
            
            return View("Detalle", viewModel);
        }

        /// <summary>
        /// Ver seguimiento de un PNC específico (modal)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SeguimientoPnc(long id)
        {
            var (success, viewModel) = await _service.ObtenerDetalleCompletoAsync(id);
            
            if (!success || viewModel == null)
            {
                if (Request.IsAjaxRequest())
                    return Json(new { success = false, message = "PNC no encontrado" });
                
                return NotFound();
            }

            if (Request.IsAjaxRequest())
                return PartialView("_SeguimientoModal", viewModel);
            
            return View("Seguimiento", viewModel);
        }

        /// <summary>
        /// Agregar causa a un PNC
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarCausa(long pncId, PncCausaDto dto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Datos de causa inválidos" });

            var usuarioId = GetUserId();
            var (success, idCreado, message) = await _service.CrearCausaAsync(pncId, dto, usuarioId);

            return Json(new { success, id = idCreado, message });
        }

        /// <summary>
        /// Agregar acción a una causa de PNC
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarAccion(long pncId, long causaId, PncAccionDto dto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Datos de acción inválidos" });

            var usuarioId = GetUserId();
            var (success, idCreado, message) = await _service.CrearAccionAsync(pncId, causaId, dto, usuarioId);

            return Json(new { success, id = idCreado, message });
        }

        /// <summary>
        /// Actualizar estado de un PNC
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarEstado(long id, byte estado, string observacion)
        {
            var usuarioId = GetUserId();
            var (success, message) = await _service.ActualizarEstadoAsync(id, estado, observacion, usuarioId);

            return Json(new { success, message });
        }

        /// <summary>
        /// Obtener causas de un PNC (para cargar en modal)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerCausas(long pncId)
        {
            var (success, data) = await _service.ObtenerCausasAsync(pncId);
            return Json(new { success, data });
        }

        /// <summary>
        /// Obtener acciones de un PNC (para cargar en modal)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerAcciones(long pncId)
        {
            var (success, data) = await _service.ObtenerAccionesAsync(pncId);
            return Json(new { success, data });
        }

        /// <summary>
        /// Obtener historial de estados de un PNC
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerHistorial(long pncId)
        {
            var (success, data) = await _service.ObtenerHistorialEstadosAsync(pncId);
            return Json(new { success, data });
        }

        /// <summary>
        /// Obtener procedimientos por proceso (para cascada de combos)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerProcedimientos(byte procesoId)
        {
            var (success, data) = await _service.ObtenerProcedimientosAsync(procesoId);
            return Json(new { success, data });
        }

        /// <summary>
        /// Carga los catálogos necesarios para el formulario de creación
        /// </summary>
        private async Task CargarCatalogosParaCrear()
        {
            var (_, procesos) = await _service.ObtenerProcesosAsync();
            var (_, categorias) = await _service.ObtenerCategoriasAsync();
            var (_, fuentes) = await _service.ObtenerFuentesAsync();

            ViewBag.Procesos = procesos;
            ViewBag.Categorias = categorias;
            ViewBag.Fuentes = fuentes;
        }
    }
}
