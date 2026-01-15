/// <summary>
/// Controller para Registro de Planillas Cualitativo
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.2.8
/// Webform: RegistroPlanillasCualitativo.aspx
/// </summary>
namespace MatrixNext.Web.Areas.PY.Controllers
{
    using MatrixNext.Infrastructure.Adapters;
    using MatrixNext.Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("PY")]
    [Route("PY/[controller]")]
    [Authorize(Roles = "Administrador,GerenteProyectos,EntrevistadorCuali")]
    public class RegistroPlanillasCualiController : Controller
    {
        private readonly IPyTrabajosService _trabajosService;
        private readonly IUploadAdapter _uploadAdapter;
        private readonly ILogger<RegistroPlanillasCualiController> _logger;

        public RegistroPlanillasCualiController(
            IPyTrabajosService trabajosService,
            IUploadAdapter uploadAdapter,
            ILogger<RegistroPlanillasCualiController> logger)
        {
            _trabajosService = trabajosService;
            _uploadAdapter = uploadAdapter;
            _logger = logger;
        }

        private long ObtenerIdUsuarioActual()
        {
            return long.Parse(User.FindFirst("Id")?.Value ?? "0");
        }

        /// <summary>
        /// Listado de planillas cualitativas por trabajo
        /// GET /PY/RegistroPlanillasCuali/Index/{idTrabajo}
        /// </summary>
        public async Task<IActionResult> Index(long idTrabajo)
        {
            try
            {
                var trabajo = await _trabajosService.ObtenerAsync(idTrabajo);
                if (trabajo == null)
                {
                    return NotFound(new { success = false, message = "Trabajo no encontrado" });
                }

                // Obtener planillas cualitativas cargadas
                var planillas = await _uploadAdapter.ObtenerArchivosPorContenedorAsync("PlanillaCuali", idTrabajo);

                ViewBag.IdTrabajo = idTrabajo;
                ViewBag.NombreTrabajo = trabajo.NombreTrabajoPresupuesto;
                ViewBag.TipoTrabajo = trabajo.TipoTrabajo;

                var usuarioId = ObtenerIdUsuarioActual();
                _logger.LogInformation("Listado planillas cualitativo obtenido. Trabajo: {TrabajoId}, Planillas: {Count}, Usuario: {UserId}",
                    idTrabajo, planillas.Count, usuarioId);

                return View(planillas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo listado de planillas. TrabajoId: {TrabajoId}", idTrabajo);
                return StatusCode(500, new { success = false, message = "Error al obtener planillas" });
            }
        }

        /// <summary>
        /// Modal de carga de planilla cualitativa
        /// GET /PY/RegistroPlanillasCuali/UploadModal/{idTrabajo}
        /// </summary>
        [HttpGet("UploadModal/{idTrabajo}")]
        public async Task<IActionResult> UploadModal(long idTrabajo)
        {
            try
            {
                var trabajo = await _trabajosService.ObtenerAsync(idTrabajo);
                if (trabajo == null)
                {
                    return NotFound();
                }

                var model = new MatrixNext.Web.ViewModels.UploadFrameModel
                {
                    IdComponente = "uploadPlanillaCuali_" + idTrabajo,
                    TituloSeccion = "Cargar Planilla Cualitativa",
                    ExtensionesPermitidas = ".xlsx, .xls",
                    TamanoMaximoBytess = 5 * 1024 * 1024, // 5 MB
                    IdContenedor = idTrabajo,
                    TipoContenedor = "PlanillaCuali",
                    UrlUpload = "/api/upload/UploadFile",
                    UrlDelete = "/api/upload/DeleteFile",
                    PermitirMultiple = false,
                    PermitirEliminar = true,
                    MostrarRestricciones = true,
                    CallbackJs = "location.reload();"
                };

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_UploadPlanillaModal", model);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando modal de upload. TrabajoId: {TrabajoId}", idTrabajo);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Descargar planilla cualitativa
        /// GET /PY/RegistroPlanillasCuali/Download/{idArchivo}
        /// </summary>
        [HttpGet("Download/{idArchivo}")]
        public async Task<IActionResult> Download(long idArchivo)
        {
            try
            {
                var usuarioId = ObtenerIdUsuarioActual();
                var archivo = await _uploadAdapter.ObtenerArchivoAsync(idArchivo);

                if (archivo == null)
                {
                    _logger.LogWarning("Planilla no encontrada. IdArchivo: {IdArchivo}, Usuario: {UserId}",
                        idArchivo, usuarioId);
                    return NotFound();
                }

                var stream = await _uploadAdapter.DescargarArchivoAsync(idArchivo);

                _logger.LogInformation("Planilla descargada. IdArchivo: {IdArchivo}, Nombre: {Nombre}, Usuario: {UserId}",
                    idArchivo, archivo.Nombre, usuarioId);

                return File(stream, "application/vnd.ms-excel", archivo.Nombre);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error descargando planilla. IdArchivo: {IdArchivo}", idArchivo);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Eliminar planilla cualitativa
        /// POST /PY/RegistroPlanillasCuali/Delete/{idArchivo}
        /// </summary>
        [HttpPost("Delete/{idArchivo}")]
        public async Task<IActionResult> Delete(long idArchivo)
        {
            try
            {
                var usuarioId = ObtenerIdUsuarioActual();
                var archivo = await _uploadAdapter.ObtenerArchivoAsync(idArchivo);

                if (archivo == null)
                {
                    return NotFound(new { success = false, message = "Planilla no encontrada" });
                }

                var eliminado = await _uploadAdapter.EliminarArchivoAsync(idArchivo, usuarioId, "Eliminada desde Registro Planillas");

                if (eliminado)
                {
                    _logger.LogInformation("Planilla eliminada. IdArchivo: {IdArchivo}, Usuario: {UserId}",
                        idArchivo, usuarioId);

                    if (Request.IsAjaxRequest())
                    {
                        return Json(new { success = true, message = "Planilla eliminada exitosamente" });
                    }

                    return RedirectToAction(nameof(Index), new { idTrabajo = archivo.IdContenedor });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Error al eliminar planilla" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando planilla. IdArchivo: {IdArchivo}", idArchivo);
                return StatusCode(500, new { success = false, message = "Error al eliminar" });
            }
        }

        /// <summary>
        /// API: Obtener planillas cargadas
        /// GET /PY/RegistroPlanillasCuali/GetPlanillas/{idTrabajo}
        /// </summary>
        [HttpGet("GetPlanillas/{idTrabajo}")]
        public async Task<IActionResult> GetPlanillas(long idTrabajo)
        {
            try
            {
                var planillas = await _uploadAdapter.ObtenerArchivosPorContenedorAsync("PlanillaCuali", idTrabajo);

                var datos = planillas
                    .OrderByDescending(p => p.FechaSubida)
                    .Select(p => new
                    {
                        idArchivo = p.IdArchivo,
                        nombre = p.Nombre,
                        fechaSubida = p.FechaSubida.ToString("dd/MM/yyyy HH:mm"),
                        usuario = p.UsuarioSubida,
                        tamanoBytess = p.TamanoBytess,
                        urlDescarga = Url.Action("Download", "RegistroPlanillasCuali", new { idArchivo = p.IdArchivo })
                    }).ToList();

                return Json(new { exitoso = true, datos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo planillas. TrabajoId: {TrabajoId}", idTrabajo);
                return Json(new { exitoso = false, datos = new List<dynamic>() });
            }
        }
    }
}
