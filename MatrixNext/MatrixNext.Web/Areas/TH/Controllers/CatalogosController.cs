using System;
using System.Threading.Tasks;
using MatrixNext.Data.Modules.TH.Empleados.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.TH.Controllers
{
    /// <summary>
    /// Controlador para catálogos y listas desplegables del módulo TH
    /// Equivalente a los métodos get* de EmpleadosAdmin.aspx en WebMatrix
    /// Área: TH, Ruta base: /TH/Catalogos
    /// </summary>
    [Area("TH")]
    [Route("TH/[controller]")]
    [Authorize] // REGLA 11: Siempre requerir autenticación
    public class CatalogosController : Controller
    {
        private readonly EmpleadoService _service;
        private readonly ILogger<CatalogosController> _logger;

        public CatalogosController(EmpleadoService service, ILogger<CatalogosController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Catálogos Críticos

        /// <summary>
        /// GET: /TH/Catalogos/AreasServiceLines
        /// Obtiene áreas/service lines (equivalente a getAreasServiceLines)
        /// </summary>
        [HttpGet("AreasServiceLines")]
        public async Task<IActionResult> GetAreasServiceLines()
        {
            try
            {
                var (success, message, data) = await _service.ObtenerAreasServiceLines();

                if (!success)
                {
                    return Json(new { success = false, message });
                }

                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener áreas/service lines");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        /// <summary>
        /// GET: /TH/Catalogos/GruposSanguineos
        /// Obtiene grupos sanguíneos (equivalente a getGruposSanguineos)
        /// </summary>
        [HttpGet("GruposSanguineos")]
        public async Task<IActionResult> GetGruposSanguineos()
        {
            try
            {
                var (success, message, data) = await _service.ObtenerGruposSanguineos();

                if (!success)
                {
                    return Json(new { success = false, message });
                }

                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener grupos sanguíneos");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        /// <summary>
        /// GET: /TH/Catalogos/Cargos
        /// Obtiene cargos (equivalente a getCargos)
        /// </summary>
        [HttpGet("Cargos")]
        public async Task<IActionResult> GetCargos()
        {
            try
            {
                var (success, message, data) = await _service.ObtenerCargos();

                if (!success)
                {
                    return Json(new { success = false, message });
                }

                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cargos");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        /// <summary>
        /// GET: /TH/Catalogos/EstadosCiviles
        /// Obtiene estados civiles (equivalente a getEstadosCiviles)
        /// </summary>
        [HttpGet("EstadosCiviles")]
        public async Task<IActionResult> GetEstadosCiviles()
        {
            try
            {
                var (success, message, data) = await _service.ObtenerEstadosCiviles();

                if (!success)
                {
                    return Json(new { success = false, message });
                }

                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estados civiles");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        #endregion

        #region Catálogos Complementarios

        /// <summary>
        /// GET: /TH/Catalogos/Todos
        /// Obtiene todos los catálogos en un solo request
        /// Optimización para carga inicial de formularios
        /// </summary>
        [HttpGet("Todos")]
        public async Task<IActionResult> GetTodosCatalogos()
        {
            try
            {
                var (success, message, data) = await _service.ObtenerTodosCatalogos();

                if (!success)
                {
                    return Json(new { success = false, message });
                }

                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener catálogos");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        #endregion
    }
}
