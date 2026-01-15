using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Core.Services.GD
{
    /// <summary>
    /// Servicio de validación para uploads de documentos
    /// Sprint 12.3.7: Repositorio Validaciones y Versionamiento
    /// </summary>
    public interface IRepositorioValidadorService
    {
        /// <summary>
        /// Valida extensión de archivo
        /// </summary>
        Task<(bool valido, string mensaje)> ValidarExtensionAsync(string nombreArchivo);

        /// <summary>
        /// Valida tamaño de archivo
        /// </summary>
        Task<(bool valido, string mensaje)> ValidarTamañoAsync(long tamañoBytes);

        /// <summary>
        /// Valida archivo completo (extensión + tamaño)
        /// </summary>
        Task<(bool valido, string mensaje)> ValidarArchivoAsync(string nombreArchivo, long tamañoBytes);

        /// <summary>
        /// Obtiene versión siguiente para un documento
        /// </summary>
        Task<decimal> ObtenerVersionSiguienteAsync(long idDocumento);

        /// <summary>
        /// Genera nombre de archivo con versión
        /// </summary>
        Task<string> GenerarNombreArchivoConVersionAsync(string nombreOriginal, long idDocumento);
    }

    /// <summary>
    /// Implementación del servicio de validación
    /// </summary>
    public class RepositorioValidadorService : IRepositorioValidadorService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RepositorioValidadorService> _logger;
        private readonly List<string> _extensionesPermitidas;
        private readonly long _tamañoMaximoBytes;

        public RepositorioValidadorService(
            IConfiguration configuration,
            ILogger<RepositorioValidadorService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            // Leer configuración desde appsettings.json
            var extensionesConfig = _configuration["Repositorio:ExtensionesPermitidas"] 
                                   ?? ".pdf,.docx,.xlsx,.doc,.xls,.txt,.jpg,.jpeg,.png";
            
            _extensionesPermitidas = extensionesConfig
                .Split(',')
                .Select(e => e.Trim().ToLower())
                .ToList();

            var tamañoConfig = _configuration["Repositorio:TamañoMaximoMB"];
            if (long.TryParse(tamañoConfig ?? "50", out long tamañoMB))
            {
                _tamañoMaximoBytes = tamañoMB * 1024 * 1024; // Convertir MB a bytes
            }
            else
            {
                _tamañoMaximoBytes = 50 * 1024 * 1024; // Default: 50 MB
            }

            _logger.LogInformation(
                "RepositorioValidadorService inicializado. Extensiones permitidas: {Extensiones}, Tamaño máximo: {TamañoMB}MB",
                string.Join(", ", _extensionesPermitidas),
                _tamañoMaximoBytes / (1024 * 1024));
        }

        public async Task<(bool valido, string mensaje)> ValidarExtensionAsync(string nombreArchivo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreArchivo))
                {
                    _logger.LogWarning("Intento de validar archivo con nombre vacío");
                    return (false, "El nombre del archivo es requerido");
                }

                // Obtener extensión
                var extension = Path.GetExtension(nombreArchivo).ToLower();

                if (string.IsNullOrWhiteSpace(extension))
                {
                    _logger.LogWarning("Archivo sin extensión: {NombreArchivo}", nombreArchivo);
                    return (false, "El archivo debe tener una extensión válida");
                }

                // Validar contra lista permitida
                if (!_extensionesPermitidas.Contains(extension))
                {
                    var extensionesTexto = string.Join(", ", _extensionesPermitidas);
                    _logger.LogWarning(
                        "Extensión no permitida: {Extensión}. Permitidas: {Permitidas}",
                        extension, extensionesTexto);
                    
                    return (false, 
                        $"Extensión '{extension}' no permitida. Extensiones válidas: {extensionesTexto}");
                }

                _logger.LogInformation("Extensión válida: {Extensión}", extension);
                return (true, "Extensión válida");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando extensión del archivo: {NombreArchivo}", nombreArchivo);
                return (false, "Error al validar la extensión");
            }
        }

        public async Task<(bool valido, string mensaje)> ValidarTamañoAsync(long tamañoBytes)
        {
            try
            {
                if (tamañoBytes <= 0)
                {
                    _logger.LogWarning("Intento de validar archivo con tamaño cero o negativo");
                    return (false, "El archivo no puede estar vacío");
                }

                if (tamañoBytes > _tamañoMaximoBytes)
                {
                    var tamañoMaximoMB = _tamañoMaximoBytes / (1024 * 1024);
                    var tamañoActualMB = Math.Round(tamañoBytes / (1024.0 * 1024), 2);

                    _logger.LogWarning(
                        "Tamaño excedido. Máximo: {TamañoMaximoMB}MB, Enviado: {TamañoActualMB}MB",
                        tamañoMaximoMB, tamañoActualMB);

                    return (false, 
                        $"El archivo es muy grande. Tamaño máximo: {tamañoMaximoMB}MB (actual: {tamañoActualMB}MB)");
                }

                var tamañoMB = Math.Round(tamañoBytes / (1024.0 * 1024), 2);
                _logger.LogInformation("Tamaño válido: {TamañoMB}MB", tamañoMB);
                return (true, "Tamaño válido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando tamaño del archivo: {TamañoBytes} bytes", tamañoBytes);
                return (false, "Error al validar el tamaño");
            }
        }

        public async Task<(bool valido, string mensaje)> ValidarArchivoAsync(string nombreArchivo, long tamañoBytes)
        {
            try
            {
                // Validar extensión
                var (extensionValida, mensajeExtension) = await ValidarExtensionAsync(nombreArchivo);
                if (!extensionValida)
                {
                    return (false, mensajeExtension);
                }

                // Validar tamaño
                var (tamañoValido, mensajeTamaño) = await ValidarTamañoAsync(tamañoBytes);
                if (!tamañoValido)
                {
                    return (false, mensajeTamaño);
                }

                _logger.LogInformation(
                    "Archivo validado exitosamente: {NombreArchivo} ({TamañoMB}MB)",
                    nombreArchivo, Math.Round(tamañoBytes / (1024.0 * 1024), 2));

                return (true, "Archivo válido");
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
                    _logger.LogWarning("ID de documento inválido para obtener versión: {IdDocumento}", idDocumento);
                    return 1.0m; // Primera versión por defecto
                }

                // Esta operación debería consultarse con la BD
                // Por ahora, retornamos 1.0 como default
                // La implementación real consulta: SELECT MAX(Version) FROM GD_RepositorioDocumentos WHERE IdDocumento = @IdDocumento
                
                _logger.LogInformation("Versión siguiente para documento {IdDocumento}: 1.0 (default)", idDocumento);
                return 1.0m;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo versión siguiente para documento {IdDocumento}", idDocumento);
                return 1.0m;
            }
        }

        public async Task<string> GenerarNombreArchivoConVersionAsync(string nombreOriginal, long idDocumento)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreOriginal))
                {
                    _logger.LogWarning("Nombre original vacío para generar nombre con versión");
                    throw new ArgumentException("El nombre original del archivo es requerido", nameof(nombreOriginal));
                }

                // Obtener versión siguiente
                var versionSiguiente = await ObtenerVersionSiguienteAsync(idDocumento);

                // Separar nombre de extensión
                var nombreSinExtension = Path.GetFileNameWithoutExtension(nombreOriginal);
                var extension = Path.GetExtension(nombreOriginal);

                // Generar nombre con versión: Nombre_v1.0.ext
                var nombreConVersion = $"{nombreSinExtension}_v{versionSiguiente:F1}{extension}";

                _logger.LogInformation(
                    "Nombre con versión generado: {NombreConVersion} (original: {NombreOriginal}, documento: {IdDocumento}, versión: {Version})",
                    nombreConVersion, nombreOriginal, idDocumento, versionSiguiente);

                return nombreConVersion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Error generando nombre con versión. Original: {NombreOriginal}, Documento: {IdDocumento}",
                    nombreOriginal, idDocumento);
                
                // En caso de error, retornar nombre original
                return nombreOriginal;
            }
        }
    }
}
