using MatrixNext.Web.Services.CORE;
using MatrixNext.Web.Services.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.CORE.Controllers
{
    [Area("CORE")]
    [Route("api/core/indicadores")]
    [Authorize(Roles = "Administrador,Gerente,Coordinador")]
    public class IndicadoresController : Controller
    {
        private readonly IIndicadoresCumplimientoService _indicadoresService;
        private readonly IExportService _exportService;
        private readonly ILogger<IndicadoresController> _logger;

        public IndicadoresController(
            IIndicadoresCumplimientoService indicadoresService,
            IExportService exportService,
            ILogger<IndicadoresController> logger)
        {
            _indicadoresService = indicadoresService;
            _exportService = exportService;
            _logger = logger;
        }

        [HttpGet("resumen")]
        public async Task<IActionResult> ObtenerResumen()
        {
            var resultado = await _indicadoresService.ObtenerResumenIndicadoresAsync();
            return Ok(resultado);
        }

        [HttpGet("por-gerente")]
        public async Task<IActionResult> ObtenerPorGerente()
        {
            var resultado = await _indicadoresService.ObtenerIndicadoresPorGerenteAsync();
            return Ok(resultado);
        }

        [HttpGet("por-tipo-hilo")]
        public async Task<IActionResult> ObtenerPorTipoHilo()
        {
            var resultado = await _indicadoresService.ObtenerIndicadoresPorTipoHiloAsync();
            return Ok(resultado);
        }

        /// <summary>
        /// GET /api/core/indicadores/export-excel
        /// Exporta indicadores de cumplimiento a Excel (multi-hojas)
        /// </summary>
        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportarExcel()
        {
            try
            {
                var resumenResult = await _indicadoresService.ObtenerResumenIndicadoresAsync();
                var porGerenteResult = await _indicadoresService.ObtenerIndicadoresPorGerenteAsync();
                var porTipoHiloResult = await _indicadoresService.ObtenerIndicadoresPorTipoHiloAsync();

                if (!resumenResult.IsSuccess)
                {
                    return BadRequest(new { mensaje = "No hay datos para exportar" });
                }

                var hojas = new Dictionary<string, object>
                {
                    { "Resumen General", new List<object?> { resumenResult.Data } },
                    { "Por Gerente", porGerenteResult.Data as object ?? new List<object?>() },
                    { "Por Tipo Hilo", porTipoHiloResult.Data as object ?? new List<object?>() }
                };

                var excelBytes = await _exportService.ExportarExcelMultiHojasAsync(
                    hojas,
                    "Indicadores_Cumplimiento");

                return File(
                    excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Indicadores_Cumplimiento_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar indicadores a Excel");
                return StatusCode(500, new { mensaje = "Error al generar el archivo Excel" });
            }
        }

        [HttpGet("/core/indicadores")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}
