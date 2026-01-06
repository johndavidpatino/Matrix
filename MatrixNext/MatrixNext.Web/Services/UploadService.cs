using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services
{
    /// <summary>
    /// Implementación de IUploadService
    /// Ref: ESPECIFICACION_COMPONENTES_COMPARTIDOS.md § 1.2
    /// </summary>
    public class UploadService : IUploadService
    {
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IAuditoriaService _auditoria;
        private readonly ILogger<UploadService> _logger;
        private readonly string _basePath = "uploads";

        public UploadService(IWebHostEnvironment hostEnv, IAuditoriaService auditoria, ILogger<UploadService> logger)
        {
            _hostEnv = hostEnv;
            _auditoria = auditoria;
            _logger = logger;
        }

        public async Task<UploadResultVM> SubirArchivoAsync(string moduleId, long entityId, IFormFile file)
        {
            // Validar
            if (file == null || file.Length == 0)
                throw new ArgumentException("Archivo vacío");

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".csv", ".txt", ".jpg", ".png" };
            var fileExt = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExt))
                throw new ArgumentException($"Extensión no permitida: {fileExt}");

            if (file.Length > 20 * 1024 * 1024) // 20 MB
                throw new ArgumentException("Archivo demasiado grande (máx 20 MB)");

            try
            {
                // Crear ruta
                var carpetaModulo = Path.Combine(_hostEnv.WebRootPath, _basePath, moduleId);
                var carpetaEntidad = Path.Combine(carpetaModulo, entityId.ToString());
                Directory.CreateDirectory(carpetaEntidad);

                // Generar nombre único
                var nombreUnico = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var rutaFisica = Path.Combine(carpetaEntidad, nombreUnico);
                var rutaRelativa = Path.Combine(_basePath, moduleId, entityId.ToString(), nombreUnico)
                    .Replace("\\", "/");

                // Guardar archivo
                using (var fileStream = new FileStream(rutaFisica, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                _logger.LogInformation($"Archivo subido: {rutaRelativa} ({file.Length} bytes)");

                return new UploadResultVM
                {
                    RutaRelativa = rutaRelativa,
                    RutaAbsoluta = Path.Combine(_basePath, rutaRelativa),
                    NombreArchivo = file.FileName,
                    TamañoBytes = file.Length,
                    FechaSubida = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error subiendo archivo: {file.FileName}");
                throw;
            }
        }

        public async Task<FileStreamResult> DescargarArchivoAsync(string rutaRelativa, long usuarioId)
        {
            var rutaFisica = Path.Combine(_hostEnv.WebRootPath, rutaRelativa);

            if (!File.Exists(rutaFisica))
                throw new FileNotFoundException("Archivo no encontrado");

            try
            {
                var fileStream = new FileStream(rutaFisica, FileMode.Open, FileAccess.Read);
                var nombreArchivo = Path.GetFileName(rutaFisica);
                var mimeType = ObtenerMimeType(rutaFisica);

                _logger.LogInformation($"Usuario {usuarioId} descargó {rutaRelativa}");

                return new FileStreamResult(fileStream, mimeType) { FileDownloadName = nombreArchivo };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error descargando archivo: {rutaRelativa}");
                throw;
            }
        }

        public async Task<bool> EliminarArchivoAsync(string rutaRelativa, long usuarioId, string razon)
        {
            var rutaFisica = Path.Combine(_hostEnv.WebRootPath, rutaRelativa);

            if (!File.Exists(rutaFisica))
                return false;

            try
            {
                File.Delete(rutaFisica);
                _logger.LogInformation($"Usuario {usuarioId} eliminó {rutaRelativa}. Razón: {razon}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error eliminando archivo: {rutaRelativa}");
                return false;
            }
        }

        public async Task<List<ArchivoVM>> ListarArchivosAsync(string moduleId, long entityId)
        {
            var carpetaEntidad = Path.Combine(_hostEnv.WebRootPath, _basePath, moduleId, entityId.ToString());

            if (!Directory.Exists(carpetaEntidad))
                return new List<ArchivoVM>();

            var archivos = new List<ArchivoVM>();
            try
            {
                foreach (var archivo in Directory.GetFiles(carpetaEntidad))
                {
                    var info = new FileInfo(archivo);
                    archivos.Add(new ArchivoVM
                    {
                        NombreArchivo = Path.GetFileName(archivo),
                        RutaRelativa = $"/{_basePath}/{moduleId}/{entityId}/{Path.GetFileName(archivo)}".Replace("\\", "/"),
                        TamañoKB = Math.Round((decimal)info.Length / 1024, 2),
                        FechaCreacion = info.CreationTime
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error listando archivos en {moduleId}/{entityId}");
            }

            return archivos;
        }

        private string ObtenerMimeType(string rutaArchivo)
        {
            var ext = Path.GetExtension(rutaArchivo).ToLower();
            return ext switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".csv" => "text/csv",
                ".txt" => "text/plain",
                ".jpg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
        }
    }
}
