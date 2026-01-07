using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Controllers
{
    /// <summary>
    /// API para gestión de carga/descarga/eliminación de archivos
    /// ISSUE RESUELTO: Sprint 6 GAP-6.4 - Listado de archivos implementado
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;
        private readonly ILogger<UploadController> _logger;

        public UploadController(IUploadService uploadService, ILogger<UploadController> logger)
        {
            _uploadService = uploadService;
            _logger = logger;
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
                return BadRequest(ResultVM<object>.Fail(ex.Message));
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
    }
}
