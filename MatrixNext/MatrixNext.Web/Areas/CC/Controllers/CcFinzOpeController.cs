using MatrixNext.Data.Modules.CC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.CC.Controllers
{
    [Area("CC")]
    [Authorize]
    public class CcFinzOpeController : Controller
    {
        private readonly ICcFinzOpeService _service;
        private readonly ILogger<CcFinzOpeController> _logger;

        public CcFinzOpeController(ICcFinzOpeService service, ILogger<CcFinzOpeController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Vista web de FinzOpe
        /// GET /cc/ccfinzope
        /// </summary>
        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Index()
        {
            _logger.LogInformation("Accediendo a vista de FinzOpe");
            return View();
        }

        // ============================================
        // API ENDPOINTS (REST)
        // ============================================

        /// <summary>
        /// Calcular liquidación mensual de un período
        /// GET /api/cc/finzope/liquidacion?idPeriodo=202501&fechaInicio=2025-01-01&fechaFin=2025-01-31
        /// </summary>
        [HttpGet("/api/cc/finzope/liquidacion")]
        public async Task<IActionResult> ObtenerLiquidacion(
            [FromQuery] int idPeriodo,
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            try
            {
                _logger.LogInformation("Obteniendo liquidación para período {IdPeriodo}", idPeriodo);
                
                var result = await _service.CalcularLiquidacionMensual(idPeriodo, fechaInicio, fechaFin);
                
                return Ok(new { success = true, data = result, timestamp = DateTime.UtcNow });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo liquidación");
                return StatusCode(500, new { success = false, error = "Error al obtener la liquidación. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// Obtener bonificaciones de un período
        /// GET /api/cc/finzope/bonificaciones?idPeriodo=202501
        /// </summary>
        [HttpGet("/api/cc/finzope/bonificaciones")]
        public async Task<IActionResult> ObtenerBonificaciones([FromQuery] int idPeriodo)
        {
            try
            {
                _logger.LogInformation("Obteniendo bonificaciones para período {IdPeriodo}", idPeriodo);
                
                var result = await _service.ObtenerBonificaciones(idPeriodo);
                
                return Ok(new { success = true, data = result, timestamp = DateTime.UtcNow });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo bonificaciones");
                return StatusCode(500, new { success = false, error = "Error al obtener las bonificaciones. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// Obtener total de producción en un rango de fechas
        /// GET /api/cc/finzope/produccion/total?fechaInicio=2025-01-01&fechaFin=2025-01-31
        /// </summary>
        [HttpGet("/api/cc/finzope/produccion/total")]
        public async Task<IActionResult> ObtenerProduccionTotal(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin,
            [FromQuery] int? idTrabajo = null)
        {
            try
            {
                _logger.LogInformation("Obteniendo total de producción desde {FechaInicio} hasta {FechaFin}", fechaInicio, fechaFin);
                
                var result = await _service.ObtenerProduccionTotal(fechaInicio, fechaFin, idTrabajo);
                
                return Ok(new { success = true, data = result, timestamp = DateTime.UtcNow });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo producción total");
                return StatusCode(500, new { success = false, error = "Error al obtener la producción total. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// Health check del servicio
        /// </summary>
        [HttpGet("/api/cc/finzope/health")]
        public IActionResult Health()
        {
            return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
        }
    }
}
