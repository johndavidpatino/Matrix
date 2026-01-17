using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services;
using MatrixNext.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Web.Controllers
{
    /// <summary>
    /// API para gestión centralizada de carga/descarga/eliminación de archivos
    /// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.2.6
    /// Soporta: Componente _UploadFrame reutilizable + endpoints legacy (Sprint 6)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;
        private readonly ILogger<UploadController> _logger;
        private readonly string[] _extensionesPermitidas = { ".pdf", ".docx", ".xlsx", ".jpg", ".png", ".zip", ".txt" };
        private const long TAMAÑO_MAXIMO = 10 * 1024 * 1024; // 10 MB

        public UploadController(IUploadService uploadService, ILogger<UploadController> logger)
        {
            _uploadService = uploadService;
            _logger = logger;
        }

        private long ObtenerIdUsuarioActual()
        {
            return long.Parse(User.FindFirst("Id")?.Value ?? "0");
        }

        /// <summary>
        /// Sube un archivo a la entidad especificada
        /// </summary>
        /// <param name="moduleId">Módulo: PY, CORE, OP</param>
        /// <param name="entityId">ID de la entidad</param>
        /// <param name="file">Archivo a subir</param>
        [HttpPost("upload")]
        public async Task<IActionResult> SubirArchivo(
            [FromQuery] string moduleId,
            [FromQuery] long entityId,
            IFormFile file)
        {
            try
            {
                if (string.IsNullOrEmpty(moduleId) || entityId <= 0)
                {
                    return BadRequest(ResultVM<object>.Fail("moduleId y entityId son requeridos"));
                }

                var resultado = await _uploadService.SubirArchivoAsync(moduleId, entityId, file);

                _logger.LogInformation(
                    "Archivo subido correctamente: {NombreArchivo} para {ModuleId}/{EntityId}",
                    resultado.NombreArchivo, moduleId, entityId);

                return Ok(ResultVM<UploadResultVM>.Ok(resultado, "Archivo subido correctamente"));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Error validando archivo: {Mensaje}", ex.Message);
                return BadRequest(ResultVM<object>.Fail("Error al procesar la solicitud. Por favor intente nuevamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subiendo archivo para {ModuleId}/{EntityId}", moduleId, entityId);
                return StatusCode(500, ResultVM<object>.Fail("Error subiendo archivo"));
            }
        }

        /// <summary>
        /// Lista archivos de una entidad
        /// ISSUE RESUELTO: Sprint 6 GAP-6.4
        /// </summary>
        /// <param name="moduleId">Módulo: PY, CORE, OP</param>
        /// <param name="entityId">ID de la entidad</param>
        [HttpGet("list")]
        public async Task<IActionResult> ListarArchivos(
            [FromQuery] string moduleId,
            [FromQuery] long entityId)
        {
            try
            {
                if (string.IsNullOrEmpty(moduleId) || entityId <= 0)
                {
                    return BadRequest(ResultVM<object>.Fail("moduleId y entityId son requeridos"));
                }

                var archivos = await _uploadService.ListarArchivosAsync(moduleId, entityId);

                _logger.LogInformation(
                    "Listados {CantidadArchivos} archivos para {ModuleId}/{EntityId}",
                    archivos.Count, moduleId, entityId);

                return Ok(ResultVM<List<ArchivoVM>>.Ok(
                    archivos,
                    $"Encontrados {archivos.Count} archivo(s)"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listando archivos para {ModuleId}/{EntityId}", moduleId, entityId);
                return StatusCode(500, ResultVM<object>.Fail("Error listando archivos"));
            }
        }

        /// <summary>
        /// Descarga un archivo
        /// </summary>
        /// <param name="rutaRelativa">Ruta relativa del archivo (URL encoded)</param>
        [HttpGet("download")]
        public async Task<IActionResult> DescargarArchivo([FromQuery] string rutaRelativa)
        {
            try
            {
                if (string.IsNullOrEmpty(rutaRelativa))
                {
                    return BadRequest("Ruta del archivo requerida");
                }

                // Obtener ID del usuario actual (si está autenticado)
                var usuarioId = User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? "0";
                
                if (!long.TryParse(usuarioId, out var userId))
                {
                    userId = 0;
                }

                var fileStream = await _uploadService.DescargarArchivoAsync(rutaRelativa, userId);

                _logger.LogInformation("Usuario {UsuarioId} descargó archivo: {Ruta}", userId, rutaRelativa);

                return fileStream;
            }
            catch (FileNotFoundException)
            {
                _logger.LogWarning("Archivo no encontrado: {Ruta}", rutaRelativa);
                return NotFound(ResultVM<object>.Fail("Archivo no encontrado"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error descargando archivo: {Ruta}", rutaRelativa);
                return StatusCode(500, ResultVM<object>.Fail("Error descargando archivo"));
            }
        }

        /// <summary>
        /// Elimina un archivo
        /// </summary>
        /// <param name="rutaRelativa">Ruta relativa del archivo (URL encoded)</param>
        /// <param name="razon">Razón de eliminación (opcional)</param>
        [HttpDelete("delete")]
        public async Task<IActionResult> EliminarArchivo(
            [FromQuery] string rutaRelativa,
            [FromQuery] string? razon = null)
        {
            try
            {
                if (string.IsNullOrEmpty(rutaRelativa))
                {
                    return BadRequest(ResultVM<object>.Fail("Ruta del archivo requerida"));
                }

                // Obtener ID del usuario actual
                var usuarioId = User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? "0";
                
                if (!long.TryParse(usuarioId, out var userId))
                {
                    return Unauthorized(ResultVM<object>.Fail("Usuario no autenticado"));
                }

                var eliminado = await _uploadService.EliminarArchivoAsync(
                    rutaRelativa,
                    userId,
                    razon ?? "Sin especificar");

                if (!eliminado)
                {
                    return NotFound(ResultVM<object>.Fail("Archivo no encontrado"));
                }

                _logger.LogInformation(
                    "Usuario {UsuarioId} eliminó archivo: {Ruta}. Razón: {Razon}",
                    userId, rutaRelativa, razon);

                return Ok(ResultVM<bool>.Ok(true, "Archivo eliminado correctamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando archivo: {Ruta}", rutaRelativa);
                return StatusCode(500, ResultVM<object>.Fail("Error eliminando archivo"));
            }
        }

        /// <summary>
        /// Endpoint POST para cargar archivos desde _UploadFrame (Sprint 12.2.6)
        /// Soporta múltiples archivos con validación de extensión y tamaño
        /// </summary>
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(long containerId, string containerType)
        {
            try
            {
                var files = Request.Form.Files;

                // Validaciones
                if (files.Count == 0)
                {
                    return BadRequest(new { exitoso = false, mensaje = "No se seleccionaron archivos" });
                }

                var usuarioId = ObtenerIdUsuarioActual();
                var archivosSubidos = new List<dynamic>();

                foreach (var file in files)
                {
                    // Validar extensión
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    if (!_extensionesPermitidas.Contains(extension))
                    {
                        _logger.LogWarning("Extensión no permitida: {Extension} por usuario {UserId}", extension, usuarioId);
                        continue;
                    }

                    // Validar tamaño
                    if (file.Length > TAMAÑO_MAXIMO)
                    {
                        _logger.LogWarning("Archivo excede tamaño máximo: {Nombre} ({Tamaño}) por usuario {UserId}",
                            file.FileName, file.Length, usuarioId);
                        continue;
                    }

                    // Guardar archivo vía IUploadService
                    var resultado = await _uploadService.SubirArchivoAsync(containerType, containerId, file);

                    if (resultado != null)
                    {
                        archivosSubidos.Add(new
                        {
                            idArchivo = resultado.Id,
                            nombre = file.FileName,
                            tamaño = file.Length,
                            urlDescarga = resultado.UrlDescarga
                        });

                        _logger.LogInformation("Archivo cargado: {Nombre}, Contenedor: {ContainerType}:{ContainerId}, Usuario: {UserId}",
                            file.FileName, containerType, containerId, usuarioId);
                    }
                }

                if (archivosSubidos.Count == 0)
                {
                    return BadRequest(new { exitoso = false, mensaje = "No se pudieron cargar los archivos" });
                }

                return Ok(new
                {
                    exitoso = true,
                    mensaje = $"{archivosSubidos.Count} archivo(s) cargado(s) exitosamente",
                    datos = archivosSubidos
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en upload de archivos. ContainerId: {ContainerId}, ContainerType: {ContainerType}",
                    containerId, containerType);
                return StatusCode(500, new { exitoso = false, mensaje = "Error al cargar archivos" });
            }
        }

        /// <summary>
        /// Endpoint POST para eliminar archivo desde _UploadFrame (Sprint 12.2.6)
        /// </summary>
        [HttpPost("DeleteFile")]
        public async Task<IActionResult> DeleteFile(long fileId, long containerId)
        {
            try
            {
                var usuarioId = ObtenerIdUsuarioActual();

                // Validar permisos del archivo (debe pertenecer al usuario o ser admin)
                var eliminado = await _uploadService.EliminarArchivoAsync(
                    $"contenedor_{containerId}_{fileId}",
                    usuarioId,
                    "Eliminado desde _UploadFrame component");

                if (eliminado)
                {
                    _logger.LogInformation("Archivo eliminado: {FileId} por usuario {UserId}", fileId, usuarioId);
                    return Ok(new { exitoso = true, mensaje = "Archivo eliminado exitosamente" });
                }
                else
                {
                    return BadRequest(new { exitoso = false, mensaje = "No se pudo eliminar el archivo" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando archivo. FileId: {FileId}", fileId);
                return StatusCode(500, new { exitoso = false, mensaje = "Error al eliminar archivo" });
            }
        }

        /// <summary>
        /// API: Obtener archivos del contenedor (Sprint 12.2.6)
        /// </summary>
        [HttpGet("GetArchivos/{containerType}/{containerId}")]
        public async Task<IActionResult> GetArchivos(string containerType, long containerId)
        {
            try
            {
                // Nota: Requiere extensión del IUploadService para soportar búsqueda por contenedor
                var datos = new List<dynamic>();

                return Ok(new { exitoso = true, datos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo archivos. ContainerType: {ContainerType}, ContainerId: {ContainerId}",
                    containerType, containerId);
                return BadRequest(new { exitoso = false, datos = new List<dynamic>() });
            }
        }
    }
}
