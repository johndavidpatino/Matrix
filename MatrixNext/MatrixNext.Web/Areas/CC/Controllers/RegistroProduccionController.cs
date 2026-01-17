using MatrixNext.Data.Modules.CC.DTOs.Produccion;
using MatrixNext.Data.Modules.CC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Web.Areas.CC.Controllers
{
    /// <summary>
    /// Controller para Registro de Producción
    /// </summary>
    [Area("CC")]
    [Route("CC/[controller]")]
    [Authorize]
    public class RegistroProduccionController : Controller
    {
        private readonly ICcProduccionService _service;
        private readonly ILogger<RegistroProduccionController> _logger;

        public RegistroProduccionController(ICcProduccionService service, 
            ILogger<RegistroProduccionController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// GET: Página principal de Registro de Producción
        /// </summary>
        public IActionResult Index()
        {
            _logger.LogInformation("Acceder a Registro de Producción");
            return View();
        }

        /// <summary>
        /// POST: Obtener registros de producción con filtros
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ObtenerRegistros([FromBody] FiltrosRegistroProduccionDto filtros)
        {
            try
            {
                _logger.LogInformation("Obtener registros de producción");
                var registros = await _service.ObtenerRegistrosProduccionAsync(filtros);
                
                return Json(new
                {
                    success = true,
                    data = registros
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener registros de producción");
                return Json(new
                {
                    success = false,
                    message = "Error al obtener los registros de producción. Por favor intente nuevamente."
                });
            }
        }

        /// <summary>
        /// GET: Exportar registros de producción a Excel
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Exportar([FromQuery] FiltrosRegistroProduccionDto filtros)
        {
            try
            {
                _logger.LogInformation("Exportar registros de producción");
                var registros = await _service.ObtenerRegistrosProduccionAsync(filtros);
                var excelBytes = await _service.ExportarRegistrosProduccionExcelAsync(registros);

                return File(excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"RegistroProduccion_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar registros de producción");
                return BadRequest("Error al exportar los registros de producción. Por favor intente nuevamente.");
            }
        }
    }
}
