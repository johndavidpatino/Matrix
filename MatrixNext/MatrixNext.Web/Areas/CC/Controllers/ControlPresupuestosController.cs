using MatrixNext.Data.Modules.CC.DTOs.ControlPresupuestos;
using MatrixNext.Data.Modules.CC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.CC.Controllers
{
    /// <summary>
    /// Controller para Control de Presupuestos (Sprint 1)
    /// </summary>
    [Area("CC")]
    [Route("CC/[controller]")]
    [Authorize]
    public class ControlPresupuestosController : Controller
    {
        private readonly ICcControlPresupuestosService _service;
        private readonly ILogger<ControlPresupuestosController> _logger;

        public ControlPresupuestosController(
            ICcControlPresupuestosService service,
            ILogger<ControlPresupuestosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// GET: /CC/ControlPresupuestos - Página principal
        /// </summary>
        public IActionResult Index()
        {
            _logger.LogInformation("Acceso a Control de Presupuestos");
            return View();
        }

        /// <summary>
        /// POST: /CC/ControlPresupuestos/ObtenerPresupuestos - API para DataTable
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ObtenerPresupuestos(
            int? periodo = null, long? idTrabajo = null, byte? estado = null)
        {
            try
            {
                _logger.LogInformation(
                    $"Obtener presupuestos - Período: {periodo}, Trabajo: {idTrabajo}");

                var presupuestos = await _service.ObtenerPresupuestosAsync(
                    periodo, idTrabajo, estado);

                return Json(new { success = true, data = presupuestos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ObtenerPresupuestos");
                return Json(new { success = false, message = "Error al obtener presupuestos. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: /api/cc/controlpresupuestos/{id}/detalles - Detalles de presupuesto
        /// </summary>
        [HttpGet("/api/cc/controlpresupuestos/{id}/detalles")]
        public async Task<IActionResult> GetDetalles(long id)
        {
            try
            {
                var detalles = await _service.ObtenerDetallePresupuestoAsync(id);
                return Json(new { success = true, data = detalles });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo detalles presupuesto {id}");
                return Json(new { success = false, message = "Error al obtener detalles del presupuesto. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// POST: /api/cc/controlpresupuestos/guardar - Guardar presupuesto
        /// </summary>
        [HttpPost("/api/cc/controlpresupuestos/guardar")]
        public async Task<IActionResult> GuardarPresupuesto([FromBody] PresupuestoDto modelo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { success = false, message = "Datos inválidos" });

                if (modelo.MontoPresupuesto < 0)
                    return Json(new 
                    { 
                        success = false, 
                        message = "El monto debe ser positivo" 
                    });

                var idPresupuesto = await _service.GuardarPresupuestoAsync(modelo);
                
                _logger.LogInformation($"Presupuesto {idPresupuesto} guardado exitosamente");
                return Json(new { success = true, id = idPresupuesto });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando presupuesto");
                return Json(new { success = false, message = "Error al guardar presupuesto. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// DELETE: /api/cc/controlpresupuestos/{id} - Eliminar presupuesto
        /// </summary>
        [HttpDelete("/api/cc/controlpresupuestos/{id}")]
        public async Task<IActionResult> EliminarPresupuesto(long id)
        {
            try
            {
                await _service.EliminarPresupuestoAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error eliminando presupuesto {id}");
                return Json(new { success = false, message = "Error al eliminar presupuesto. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: /CC/ControlPresupuestos/Verificacion - Verificación presupuesto vs realizado
        /// </summary>
        public async Task<IActionResult> Verificacion(int? periodo = null)
        {
            try
            {
                var datos = await _service.ObtenerVerificacionPresupuestosAsync(periodo);
                return View(datos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Verificacion");
                return BadRequest("Error al verificar presupuestos. Por favor intente nuevamente.");
            }
        }

        /// <summary>
        /// GET: /api/cc/controlpresupuestos/verificacion/datos - API para verificación
        /// </summary>
        [HttpGet("/api/cc/controlpresupuestos/verificacion/datos")]
        public async Task<IActionResult> ObtenerVerificacion(int? periodo = null)
        {
            try
            {
                var datos = await _service.ObtenerVerificacionPresupuestosAsync(periodo);
                return Json(new { success = true, data = datos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ObtenerVerificacion");
                return Json(new { success = false, message = "Error al obtener verificación. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: /CC/ControlPresupuestos/Nomina - Nómina y distribución de costos
        /// </summary>
        public async Task<IActionResult> Nomina(int periodo)
        {
            try
            {
                var datos = await _service.ObtenerNominaDistribucionAsync(periodo);
                return View(datos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Nomina");
                return BadRequest("Error al obtener nómina. Por favor intente nuevamente.");
            }
        }

        /// <summary>
        /// POST: /api/cc/controlpresupuestos/distribucion/guardar - Guardar distribución
        /// </summary>
        [HttpPost("/api/cc/controlpresupuestos/distribucion/guardar")]
        public async Task<IActionResult> GuardarDistribucion(
            [FromBody] DistribucionPorCentroDto modelo)
        {
            try
            {
                if (modelo.PorcentajeDistribucion < 0 || modelo.PorcentajeDistribucion > 100)
                    return Json(new 
                    { 
                        success = false, 
                        message = "Porcentaje debe estar entre 0 y 100" 
                    });

                var id = await _service.GuardarDistribucionCostoAsync(modelo);
                return Json(new { success = true, id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando distribución");
                return Json(new { success = false, message = "Error al guardar distribución. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: /CC/ControlPresupuestos/Asignacion - Asignación de presupuestos
        /// </summary>
        public async Task<IActionResult> Asignacion(long idPresupuesto)
        {
            try
            {
                var actividades = await _service.ObtenerActividadesPresupuestadasAsync(
                    idPresupuesto);
                return View(actividades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Asignacion");
                return BadRequest("Error al obtener asignación. Por favor intente nuevamente.");
            }
        }

        /// <summary>
        /// GET: /api/cc/controlpresupuestos/asignacion/{idPresupuesto} - API para actividades
        /// </summary>
        [HttpGet("/api/cc/controlpresupuestos/asignacion/{idPresupuesto}")]
        public async Task<IActionResult> ObtenerActividades(long idPresupuesto)
        {
            try
            {
                var actividades = await _service.ObtenerActividadesPresupuestadasAsync(
                    idPresupuesto);
                return Json(new { success = true, data = actividades });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo actividades");
                return Json(new { success = false, message = "Error al obtener actividades. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// POST: /api/cc/controlpresupuestos/asignacion/guardar - Guardar asignación
        /// </summary>
        [HttpPost("/api/cc/controlpresupuestos/asignacion/guardar")]
        public async Task<IActionResult> GuardarAsignacion(
            [FromBody] AsignacionPresupuestoDto modelo)
        {
            try
            {
                if (modelo.MontoAsignado < 0)
                    return Json(new 
                    { 
                        success = false, 
                        message = "Monto debe ser positivo" 
                    });

                var id = await _service.GuardarAsignacionPresupuestoAsync(modelo);
                return Json(new { success = true, id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando asignación");
                return Json(new { success = false, message = "Error al guardar asignación. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GET: /CC/ControlPresupuestos/Exportar - Exportar presupuestos a Excel
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Exportar(int? periodo = null, long? idTrabajo = null, byte? estado = null)
        {
            try
            {
                _logger.LogInformation($"Exportar presupuestos - Período: {periodo}");

                var presupuestos = await _service.ObtenerPresupuestosAsync(periodo, idTrabajo, estado);
                var excelBytes = await _service.ExportarPresupuestosExcelAsync(presupuestos);

                var fileName = $"Presupuestos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                
                return File(
                    excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exportando presupuestos");
                return BadRequest("Error al exportar presupuestos. Por favor intente nuevamente.");
            }
        }
    }
}
