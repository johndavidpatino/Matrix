using MatrixNext.Data.Modules.CC.DTOs.PresupuestosInternos;
using MatrixNext.Data.Modules.CC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.CC.Controllers
{
    /// <summary>
    /// Controller para Presupuestos Internos (Sprint 2)
    /// </summary>
    [Area("CC")]
    [Route("CC/[controller]")]
    [Authorize]
    public class PresupuestosInternosController : Controller
    {
        private readonly ICcPresupuestosInternosService _service;
        private readonly ILogger<PresupuestosInternosController> _logger;

        public PresupuestosInternosController(
            ICcPresupuestosInternosService service,
            ILogger<PresupuestosInternosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// GET: /CC/PresupuestosInternos - Página principal (Listado)
        /// </summary>
        public IActionResult Index()
        {
            _logger.LogInformation("Acceso a Presupuestos Internos");
            return View();
        }

        /// <summary>
        /// POST: /CC/PresupuestosInternos/ObtenerPresupuestos - API para listado
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ObtenerPresupuestos(
            int? periodo = null, string? codigoEmpresa = null, byte? estado = null)
        {
            try
            {
                var presupuestos = await _service.ObtenerPresupuestosInternosAsync(
                    periodo, codigoEmpresa, estado);

                return Json(new { success = true, data = presupuestos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ObtenerPresupuestos");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// GET: /CC/PresupuestosInternos/Detalles/{id} - Vista de detalles
        /// </summary>
        public async Task<IActionResult> Detalles(long id)
        {
            try
            {
                var presupuesto = await _service.ObtenerPresupuestoInternoDetalleAsync(id);
                
                if (presupuesto == null)
                    return NotFound();

                return View(presupuesto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo detalles {id}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// GET: /api/cc/presupuestosinternos/{id} - API para obtener detalle
        /// </summary>
        [HttpGet("/api/cc/presupuestosinternos/{id}")]
        public async Task<IActionResult> GetDetalle(long id)
        {
            try
            {
                var presupuesto = await _service.ObtenerPresupuestoInternoDetalleAsync(id);
                return Json(new { success = true, data = presupuesto });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo presupuesto {id}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// POST: /api/cc/presupuestosinternos/guardar - Guardar presupuesto interno
        /// </summary>
        [HttpPost("/api/cc/presupuestosinternos/guardar")]
        public async Task<IActionResult> GuardarPresupuesto(
            [FromBody] PresupuestoInternoDto modelo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { success = false, message = "Datos inválidos" });

                var id = await _service.GuardarPresupuestoInternoAsync(modelo);
                
                _logger.LogInformation($"Presupuesto interno {id} guardado exitosamente");
                return Json(new { success = true, id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando presupuesto interno");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// DELETE: /api/cc/presupuestosinternos/{id} - Eliminar presupuesto
        /// </summary>
        [HttpDelete("/api/cc/presupuestosinternos/{id}")]
        public async Task<IActionResult> EliminarPresupuesto(long id)
        {
            try
            {
                await _service.EliminarPresupuestoInternoAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error eliminando presupuesto {id}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// GET: /CC/PresupuestosInternos/Historico/{id} - Vista de histórico
        /// </summary>
        public async Task<IActionResult> Historico(long id)
        {
            try
            {
                var historico = await _service.ObtenerHistoricoPresupuestoInternoAsync(id);
                ViewBag.IdPresupuesto = id;
                return View(historico);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo histórico {id}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// GET: /api/cc/presupuestosinternos/{id}/historico - API para histórico
        /// </summary>
        [HttpGet("/api/cc/presupuestosinternos/{id}/historico")]
        public async Task<IActionResult> GetHistorico(long id)
        {
            try
            {
                var historico = await _service.ObtenerHistoricoPresupuestoInternoAsync(id);
                return Json(new { success = true, data = historico });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo histórico");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// POST: /api/cc/presupuestosinternos/{id}/aprobar - Aprobar presupuesto
        /// </summary>
        [HttpPost("/api/cc/presupuestosinternos/{id}/aprobar")]
        public async Task<IActionResult> AprobarPresupuesto(long id, [FromBody] string usuario)
        {
            try
            {
                await _service.AprobarPresupuestoInternoAsync(id, usuario);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error aprobando presupuesto {id}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// GET: /CC/PresupuestosInternos/Exportar - Exportar a Excel
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Exportar(
            int? periodo = null, string? codigoEmpresa = null, byte? estado = null)
        {
            try
            {
                var presupuestos = await _service.ObtenerPresupuestosInternosAsync(
                    periodo, codigoEmpresa, estado);
                var excelBytes = await _service.ExportarPresupuestosInternosExcelAsync(presupuestos);

                var fileName = $"PresupuestosInternos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                
                return File(
                    excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exportando presupuestos internos");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// GET: /CC/PresupuestosInternos/Resumen - Vista de resumen
        /// </summary>
        public async Task<IActionResult> Resumen(int? periodo = null)
        {
            try
            {
                var resumen = await _service.ObtenerResumenPresupuestosInternosAsync(periodo);
                return View(resumen);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo resumen");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// POST: /api/cc/presupuestosinternos/detalle/guardar - Guardar línea presupuestal
        /// </summary>
        [HttpPost("/api/cc/presupuestosinternos/detalle/guardar")]
        public async Task<IActionResult> GuardarDetalle(
            [FromBody] DetallePresupuestoInternoDto detalle)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { success = false, message = "Datos inválidos" });

                var id = await _service.GuardarDetallePresupuestoInternoAsync(detalle);
                
                _logger.LogInformation($"Detalle {id} guardado exitosamente");
                return Json(new { success = true, id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando detalle");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// DELETE: /api/cc/presupuestosinternos/detalle/{id} - Eliminar línea presupuestal
        /// </summary>
        [HttpDelete("/api/cc/presupuestosinternos/detalle/{id}")]
        public async Task<IActionResult> EliminarDetalle(long id)
        {
            try
            {
                await _service.EliminarDetallePresupuestoInternoAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error eliminando detalle {id}");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
