/// <summary>
/// Controller para gestión de Instructivos (General y Cualitativo)
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.2.7
/// Refactorizado: AUDITORIA_MATRIXNEXT_ENERO_2026.md § Violación de Arquitectura
/// Webforms: InstructivoGeneral.aspx, InstructivoGeneralCuali.aspx
/// </summary>
namespace MatrixNext.Web.Areas.PY.Controllers
{
    using MatrixNext.Data.Services.PY.Interfaces;
    using MatrixNext.Web.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("PY")]
    [Route("PY/[controller]")]
    [Authorize]
    public class InstructivosController : Controller
    {
        private readonly IInstructivosService _instructivosService;
        private readonly ILogger<InstructivosController> _logger;

        public InstructivosController(
            IInstructivosService instructivosService,
            ILogger<InstructivosController> logger)
        {
            _instructivosService = instructivosService;
            _logger = logger;
        }

        private long ObtenerIdUsuarioActual()
        {
            return long.Parse(User.FindFirst("Id")?.Value ?? "0");
        }

        /// <summary>
        /// Listado de instructivos por trabajo
        /// GET /PY/Instructivos/Index/{idTrabajo}
        /// </summary>
        public async Task<IActionResult> Index(long idTrabajo)
        {
            try
            {
                // Obtener trabajo para contexto
                var trabajo = await _instructivosService.ObtenerInfoTrabajoAsync(idTrabajo);
                if (trabajo == null)
                {
                    return NotFound(new { success = false, message = "Trabajo no encontrado" });
                }

                // Validar permiso: solo Admin, GerenteProyectos o propietario
                var usuarioId = ObtenerIdUsuarioActual();
                if (!User.IsInRole("Administrador") && !User.IsInRole("GerenteProyectos"))
                {
                    _logger.LogWarning("Usuario {UserId} intentó acceder a instructivos de trabajo {TrabajoId} sin permisos",
                        usuarioId, idTrabajo);
                    return Forbid();
                }

                // Obtener instructivos actuales
                var instructivos = await _instructivosService.ObtenerInstructivosGeneralesAsync(idTrabajo);

                ViewBag.IdTrabajo = idTrabajo;
                ViewBag.NombreTrabajo = trabajo.NombreTrabajo;
                ViewBag.TipoTrabajo = trabajo.TipoTrabajo;

                _logger.LogInformation("Listado instructivos obtenido. Trabajo: {TrabajoId}, Instructivos: {Count}, Usuario: {UserId}",
                    idTrabajo, instructivos.Count, usuarioId);

                return View(instructivos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo listado de instructivos. TrabajoId: {TrabajoId}", idTrabajo);
                return StatusCode(500, new { success = false, message = "Error al obtener instructivos" });
            }
        }

        /// <summary>
        /// Modal de carga de instructivo
        /// GET /PY/Instructivos/UploadModal/{idTrabajo}
        /// </summary>
        [HttpGet("UploadModal/{idTrabajo}")]
        public async Task<IActionResult> UploadModal(long idTrabajo)
        {
            try
            {
                var trabajo = await _instructivosService.ObtenerInfoTrabajoAsync(idTrabajo);
                if (trabajo == null)
                {
                    return NotFound();
                }

                var model = new UploadFrameModel
                {
                    IdComponente = "uploadInstructivo_" + idTrabajo,
                    TituloSeccion = "Cargar Instructivo General",
                    ExtensionesPermitidas = ".pdf, .docx",
                    TamanoMaximoBytess = 5 * 1024 * 1024, // 5 MB
                    IdContenedor = idTrabajo,
                    TipoContenedor = "InstructivoGeneral",
                    UrlUpload = "/api/upload/UploadFile",
                    UrlDelete = "/api/upload/DeleteFile",
                    PermitirMultiple = false,
                    MostrarRestricciones = true,
                    CallbackJs = "location.reload();"
                };

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_UploadInstructivoModal", model);
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
        /// Listado de instructivos cualitativos (por segmento/sesión)
        /// GET /PY/Instructivos/Cualitativos/{idTrabajo}
        /// </summary>
        [HttpGet("Cualitativos/{idTrabajo}")]
        public async Task<IActionResult> Cualitativos(long idTrabajo)
        {
            try
            {
                var trabajo = await _instructivosService.ObtenerInfoTrabajoAsync(idTrabajo);
                if (trabajo == null)
                {
                    return NotFound(new { success = false, message = "Trabajo no encontrado" });
                }

                // Obtener instructivos cualitativos
                var instructivos = await _instructivosService.ObtenerInstructivosCualitativosAsync(idTrabajo);

                ViewBag.IdTrabajo = idTrabajo;
                ViewBag.NombreTrabajo = trabajo.NombreTrabajo;
                ViewBag.TipoTrabajo = trabajo.TipoTrabajo;

                var usuarioId = ObtenerIdUsuarioActual();
                _logger.LogInformation("Listado instructivos cualitativos obtenido. Trabajo: {TrabajoId}, Instructivos: {Count}, Usuario: {UserId}",
                    idTrabajo, instructivos.Count, usuarioId);

                return View(instructivos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo instructivos cualitativos. TrabajoId: {TrabajoId}", idTrabajo);
                return StatusCode(500, new { success = false, message = "Error al obtener instructivos" });
            }
        }

        /// <summary>
        /// Modal de carga de instructivo cualitativo
        /// GET /PY/Instructivos/UploadCualiModal/{idTrabajo}
        /// </summary>
        [HttpGet("UploadCualiModal/{idTrabajo}")]
        public async Task<IActionResult> UploadCualiModal(long idTrabajo)
        {
            try
            {
                var trabajo = await _instructivosService.ObtenerInfoTrabajoAsync(idTrabajo);
                if (trabajo == null)
                {
                    return NotFound();
                }

                var model = new UploadFrameModel
                {
                    IdComponente = "uploadInstructivoCuali_" + idTrabajo,
                    TituloSeccion = "Cargar Instructivo Cualitativo",
                    ExtensionesPermitidas = ".pdf, .docx, .pptx",
                    TamanoMaximoBytess = 10 * 1024 * 1024, // 10 MB
                    IdContenedor = idTrabajo,
                    TipoContenedor = "InstructivoCuali",
                    UrlUpload = "/api/upload/UploadFile",
                    UrlDelete = "/api/upload/DeleteFile",
                    PermitirMultiple = true,
                    MostrarRestricciones = true,
                    CallbackJs = "location.reload();"
                };

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_UploadInstructivoModal", model);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando modal de upload cualitativo. TrabajoId: {TrabajoId}", idTrabajo);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Descargar instructivo
        /// GET /PY/Instructivos/Download/{idArchivo}
        /// </summary>
        [HttpGet("Download/{idArchivo}")]
        public async Task<IActionResult> Download(long idArchivo)
        {
            try
            {
                var usuarioId = ObtenerIdUsuarioActual();
                var archivo = await _instructivosService.ObtenerArchivoAsync(idArchivo);

                if (archivo == null)
                {
                    _logger.LogWarning("Archivo no encontrado. IdArchivo: {IdArchivo}, Usuario: {UserId}",
                        idArchivo, usuarioId);
                    return NotFound();
                }

                // Validar permisos
                var stream = await _instructivosService.DescargarArchivoAsync(idArchivo, usuarioId);
                
                _logger.LogInformation("Instructivo descargado. IdArchivo: {IdArchivo}, Nombre: {Nombre}, Usuario: {UserId}",
                    idArchivo, archivo.Nombre, usuarioId);

                return File(stream, "application/octet-stream", archivo.Nombre);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error descargando instructivo. IdArchivo: {IdArchivo}", idArchivo);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Eliminar instructivo
        /// POST /PY/Instructivos/Delete/{idArchivo}
        /// </summary>
        [HttpPost("Delete/{idArchivo}")]
        public async Task<IActionResult> Delete(long idArchivo)
        {
            try
            {
                var usuarioId = ObtenerIdUsuarioActual();
                var (exitoso, mensaje, idContenedor) = await _instructivosService.EliminarInstructivoAsync(idArchivo, usuarioId);

                if (!exitoso && idContenedor == null)
                {
                    return NotFound(new { success = false, message = mensaje });
                }

                if (exitoso)
                {
                    _logger.LogInformation("Instructivo eliminado. IdArchivo: {IdArchivo}, Usuario: {UserId}",
                        idArchivo, usuarioId);

                    if (Request.IsAjaxRequest())
                    {
                        return Json(new { success = true, message = mensaje });
                    }

                    return RedirectToAction(nameof(Index), new { idTrabajo = idContenedor });
                }
                else
                {
                    return BadRequest(new { success = false, message = mensaje });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando instructivo. IdArchivo: {IdArchivo}", idArchivo);
                return StatusCode(500, new { success = false, message = "Error al eliminar" });
            }
        }

        /// <summary>
        /// API: Obtener versiones de instructivo
        /// GET /PY/Instructivos/GetVersiones/{idTrabajo}/{tipoInstructivo}
        /// </summary>
        [HttpGet("GetVersiones/{idTrabajo}/{tipoInstructivo}")]
        public async Task<IActionResult> GetVersiones(long idTrabajo, string tipoInstructivo = "InstructivoGeneral")
        {
            try
            {
                var versiones = await _instructivosService.ObtenerVersionesAsync(idTrabajo, tipoInstructivo);
                
                // Agregar URLs de descarga
                var datos = versiones.Select(v => new
                {
                    idArchivo = v.IdArchivo,
                    nombre = v.Nombre,
                    version = v.Version,
                    fechaSubida = v.FechaSubida,
                    usuario = v.Usuario,
                    urlDescarga = Url.Action("Download", "Instructivos", new { idArchivo = v.IdArchivo })
                }).ToList();

                return Json(new { exitoso = true, datos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo versiones. TrabajoId: {TrabajoId}, Tipo: {Tipo}",
                    idTrabajo, tipoInstructivo);
                return Json(new { exitoso = false, datos = new List<dynamic>() });
            }
        }
    }
}
