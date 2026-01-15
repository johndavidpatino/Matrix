using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.GD
{
    /// <summary>
    /// Servicio de validaciÃ³n para uploads de documentos
    /// Sprint 12.3.7: Repositorio Validaciones y Versionamiento
    /// </summary>
    public interface IRepositorioValidadorService
    {
        /// <summary>
        /// Valida extensiÃ³n de archivo
        /// </summary>
        Task<(bool valido, string mensaje)> ValidarExtensionAsync(string nombreArchivo);

        /// <summary>
        /// Valida tamano de archivo
        /// </summary>
        Task<(bool valido, string mensaje)> ValidartamanoAsync(long tamanoBytes);

        /// <summary>
        /// Valida archivo completo (extensiÃ³n + tamano)
        /// </summary>
        Task<(bool valido, string mensaje)> ValidarArchivoAsync(string nombreArchivo, long tamanoBytes);

        /// <summary>
        /// Obtiene versiÃ³n siguiente para un documento
        /// </summary>
        Task<decimal> ObtenerVersionSiguienteAsync(long idDocumento);

        /// <summary>
        /// Genera nombre de archivo con versiÃ³n
        /// </summary>
        Task<string> GenerarNombreArchivoConVersionAsync(string nombreOriginal, long idDocumento);
    }

    /// <summary>
    /// ImplementaciÃ³n del servicio de validaciÃ³n
    /// </summary>
    public class RepositorioValidadorService : IRepositorioValidadorService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RepositorioValidadorService> _logger;
        private readonly List<string> _extensionesPermitidas;
        private readonly long _tamanoMaximoBytes;

        public RepositorioValidadorService(
            IConfiguration configuration,
            ILogger<RepositorioValidadorService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            // Leer configuraciÃ³n desde appsettings.json
            var extensionesConfig = _configuration["Repositorio:ExtensionesPermitidas"] 
                                   ?? ".pdf,.docx,.xlsx,.doc,.xls,.txt,.jpg,.jpeg,.png";
            
            _extensionesPermitidas = extensionesConfig
                .Split(',')
                .Select(e => e.Trim().ToLower())
                .ToList();

            var tamanoConfig = _configuration["Repositorio:tamanoMaximoMB"];
            if (long.TryParse(tamanoConfig ?? "50", out long tamanoMB))
            {
                _tamanoMaximoBytes = tamanoMB * 1024 * 1024; // Convertir MB a bytes
            }
            else
            {
                _tamanoMaximoBytes = 50 * 1024 * 1024; // Default: 50 MB
            }

            _logger.LogInformation(
                "RepositorioValidadorService inicializado. Extensiones permitidas: {Extensiones}, tamano mÃ¡ximo: {tamanoMB}MB",
                string.Join(", ", _extensionesPermitidas),
                _tamanoMaximoBytes / (1024 * 1024));
        }

        public async Task<(bool valido, string mensaje)> ValidarExtensionAsync(string nombreArchivo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreArchivo))
                {
                    _logger.LogWarning("Intento de validar archivo con nombre vacÃ­o");
                    return (false, "El nombre del archivo es requerido");
                }

                // Obtener extensiÃ³n
                var extension = Path.GetExtension(nombreArchivo).ToLower();

                if (string.IsNullOrWhiteSpace(extension))
                {
                    _logger.LogWarning("Archivo sin extensiÃ³n: {NombreArchivo}", nombreArchivo);
                    return (false, "El archivo debe tener una extensiÃ³n vÃ¡lida");
                }

                // Validar contra lista permitida
                if (!_extensionesPermitidas.Contains(extension))
                {
                    var extensionesTexto = string.Join(", ", _extensionesPermitidas);
                    _logger.LogWarning(
                        "ExtensiÃ³n no permitida: {ExtensiÃ³n}. Permitidas: {Permitidas}",
                        extension, extensionesTexto);
                    
                    return (false, 
                        $"ExtensiÃ³n '{extension}' no permitida. Extensiones vÃ¡lidas: {extensionesTexto}");
                }

                _logger.LogInformation("ExtensiÃ³n vÃ¡lida: {ExtensiÃ³n}", extension);
                return (true, "ExtensiÃ³n vÃ¡lida");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando extensiÃ³n del archivo: {NombreArchivo}", nombreArchivo);
                return (false, "Error al validar la extensiÃ³n");
            }
        }

        public async Task<(bool valido, string mensaje)> ValidartamanoAsync(long tamanoBytes)
        {
            try
            {
                if (tamanoBytes <= 0)
                {
                    _logger.LogWarning("Intento de validar archivo con tamano cero o negativo");
                    return (false, "El archivo no puede estar vacÃ­o");
                }

                if (tamanoBytes > _tamanoMaximoBytes)
                {
                    var tamanoMaximoMB = _tamanoMaximoBytes / (1024 * 1024);
                    var tamanoActualMB = Math.Round(tamanoBytes / (1024.0 * 1024), 2);

                    _logger.LogWarning(
                        "tamano excedido. MÃ¡ximo: {tamanoMaximoMB}MB, Enviado: {tamanoActualMB}MB",
                        tamanoMaximoMB, tamanoActualMB);

                    return (false, 
                        $"El archivo es muy grande. tamano mÃ¡ximo: {tamanoMaximoMB}MB (actual: {tamanoActualMB}MB)");
                }

                var tamanoMB = Math.Round(tamanoBytes / (1024.0 * 1024), 2);
                _logger.LogInformation("tamano vÃ¡lido: {tamanoMB}MB", tamanoMB);
                return (true, "tamano vÃ¡lido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando tamano del archivo: {tamanoBytes} bytes", tamanoBytes);
                return (false, "Error al validar el tamano");
            }
        }

        public async Task<(bool valido, string mensaje)> ValidarArchivoAsync(string nombreArchivo, long tamanoBytes)
        {
            try
            {
                // Validar extensiÃ³n
                var (extensionValida, mensajeExtension) = await ValidarExtensionAsync(nombreArchivo);
                if (!extensionValida)
                {
                    return (false, mensajeExtension);
                }

                // Validar tamano
                var (tamanoValido, mensajetamano) = await ValidartamanoAsync(tamanoBytes);
                if (!tamanoValido)
                {
                    return (false, mensajetamano);
                }

                _logger.LogInformation(
                    "Archivo validado exitosamente: {NombreArchivo} ({tamanoMB}MB)",
                    nombreArchivo, Math.Round(tamanoBytes / (1024.0 * 1024), 2));

                return (true, "Archivo vÃ¡lido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando archivo: {NombreArchivo}", nombreArchivo);
                return (false, "Error al validar el archivo");
            }
        }

        public async Task<decimal> ObtenerVersionSiguienteAsync(long idDocumento)
        {
            try
            {
                if (idDocumento <= 0)
                {
                    _logger.LogWarning("ID de documento invÃ¡lido para obtener versiÃ³n: {IdDocumento}", idDocumento);
                    return 1.0m; // Primera versiÃ³n por defecto
                }

                // Esta operaciÃ³n deberÃ­a consultarse con la BD
                // Por ahora, retornamos 1.0 como default
                // La implementaciÃ³n real consulta: SELECT MAX(Version) FROM GD_RepositorioDocumentos WHERE IdDocumento = @IdDocumento
                
                _logger.LogInformation("VersiÃ³n siguiente para documento {IdDocumento}: 1.0 (default)", idDocumento);
                return 1.0m;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo versiÃ³n siguiente para documento {IdDocumento}", idDocumento);
                return 1.0m;
            }
        }

        public async Task<string> GenerarNombreArchivoConVersionAsync(string nombreOriginal, long idDocumento)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreOriginal))
                {
                    _logger.LogWarning("Nombre original vacÃ­o para generar nombre con versiÃ³n");
                    throw new ArgumentException("El nombre original del archivo es requerido", nameof(nombreOriginal));
                }

                // Obtener versiÃ³n siguiente
                var versionSiguiente = await ObtenerVersionSiguienteAsync(idDocumento);

                // Separar nombre de extensiÃ³n
                var nombreSinExtension = Path.GetFileNameWithoutExtension(nombreOriginal);
                var extension = Path.GetExtension(nombreOriginal);

                // Generar nombre con versiÃ³n: Nombre_v1.0.ext
                var nombreConVersion = $"{nombreSinExtension}_v{versionSiguiente:F1}{extension}";

                _logger.LogInformation(
                    "Nombre con versiÃ³n generado: {NombreConVersion} (original: {NombreOriginal}, documento: {IdDocumento}, versiÃ³n: {Version})",
                    nombreConVersion, nombreOriginal, idDocumento, versionSiguiente);

                return nombreConVersion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Error generando nombre con versiÃ³n. Original: {NombreOriginal}, Documento: {IdDocumento}",
                    nombreOriginal, idDocumento);
                
                // En caso de error, retornar nombre original
                return nombreOriginal;
            }
        }
    }
}


