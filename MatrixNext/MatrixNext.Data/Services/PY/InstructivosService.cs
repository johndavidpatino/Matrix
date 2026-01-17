using MatrixNext.Data.Adapters;
using MatrixNext.Data.Services.PY.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY
{
    /// <summary>
    /// Implementación del servicio de Instructivos
    /// Ref: AUDITORIA_MATRIXNEXT_ENERO_2026.md § Violación de Arquitectura
    /// Patrón: Controller → Service → Adapter → BD
    /// </summary>
    public class InstructivosService : IInstructivosService
    {
        private readonly IPyTrabajosService _trabajosService;
        private readonly IUploadAdapter _uploadAdapter;
        private readonly ILogger<InstructivosService> _logger;

        private const string TIPO_INSTRUCTIVO_GENERAL = "InstructivoGeneral";
        private const string TIPO_INSTRUCTIVO_CUALI = "InstructivoCuali";

        public InstructivosService(
            IPyTrabajosService trabajosService,
            IUploadAdapter uploadAdapter,
            ILogger<InstructivosService> logger)
        {
            _trabajosService = trabajosService;
            _uploadAdapter = uploadAdapter;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<InstructivoTrabajoInfoDto?> ObtenerInfoTrabajoAsync(long idTrabajo)
        {
            try
            {
                var trabajo = await _trabajosService.ObtenerAsync(idTrabajo);
                if (trabajo == null)
                {
                    _logger.LogWarning("Trabajo no encontrado. IdTrabajo: {IdTrabajo}", idTrabajo);
                    return null;
                }

                return new InstructivoTrabajoInfoDto
                {
                    IdTrabajo = trabajo.Id,
                    NombreTrabajo = trabajo.NombreTrabajoPresupuesto ?? string.Empty,
                    TipoTrabajo = trabajo.TipoTrabajo ?? string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo información de trabajo. IdTrabajo: {IdTrabajo}", idTrabajo);
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<List<UploadArchivoDto>> ObtenerInstructivosGeneralesAsync(long idTrabajo)
        {
            try
            {
                return await _uploadAdapter.ObtenerArchivosPorContenedorAsync(TIPO_INSTRUCTIVO_GENERAL, idTrabajo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo instructivos generales. IdTrabajo: {IdTrabajo}", idTrabajo);
                return new List<UploadArchivoDto>();
            }
        }

        /// <inheritdoc/>
        public async Task<List<UploadArchivoDto>> ObtenerInstructivosCualitativosAsync(long idTrabajo)
        {
            try
            {
                return await _uploadAdapter.ObtenerArchivosPorContenedorAsync(TIPO_INSTRUCTIVO_CUALI, idTrabajo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo instructivos cualitativos. IdTrabajo: {IdTrabajo}", idTrabajo);
                return new List<UploadArchivoDto>();
            }
        }

        /// <inheritdoc/>
        public async Task<UploadArchivoDto?> ObtenerArchivoAsync(long idArchivo)
        {
            try
            {
                return await _uploadAdapter.ObtenerArchivoAsync(idArchivo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo archivo. IdArchivo: {IdArchivo}", idArchivo);
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<Stream> DescargarArchivoAsync(long idArchivo, long usuarioId)
        {
            try
            {
                var archivo = await _uploadAdapter.ObtenerArchivoAsync(idArchivo);
                if (archivo == null)
                {
                    _logger.LogWarning("Archivo no encontrado para descarga. IdArchivo: {IdArchivo}, Usuario: {UserId}",
                        idArchivo, usuarioId);
                    throw new FileNotFoundException("Archivo no encontrado");
                }

                _logger.LogInformation("Iniciando descarga de instructivo. IdArchivo: {IdArchivo}, Nombre: {Nombre}, Usuario: {UserId}",
                    idArchivo, archivo.Nombre, usuarioId);

                return await _uploadAdapter.DescargarArchivoAsync(idArchivo);
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error descargando archivo. IdArchivo: {IdArchivo}", idArchivo);
                throw new InvalidOperationException("Error al descargar el archivo", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<(bool Exitoso, string Mensaje, long? IdContenedor)> EliminarInstructivoAsync(long idArchivo, long usuarioId)
        {
            try
            {
                var archivo = await _uploadAdapter.ObtenerArchivoAsync(idArchivo);
                if (archivo == null)
                {
                    return (false, "Archivo no encontrado", null);
                }

                var eliminado = await _uploadAdapter.EliminarArchivoAsync(
                    idArchivo, 
                    usuarioId, 
                    "Eliminado desde módulo Instructivos");

                if (eliminado)
                {
                    _logger.LogInformation("Instructivo eliminado. IdArchivo: {IdArchivo}, Nombre: {Nombre}, Usuario: {UserId}",
                        idArchivo, archivo.Nombre, usuarioId);
                    return (true, "Instructivo eliminado exitosamente", archivo.IdContenedor);
                }
                else
                {
                    _logger.LogWarning("No se pudo eliminar instructivo. IdArchivo: {IdArchivo}", idArchivo);
                    return (false, "Error al eliminar el instructivo", archivo.IdContenedor);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando instructivo. IdArchivo: {IdArchivo}", idArchivo);
                return (false, "Error al procesar la eliminación", null);
            }
        }

        /// <inheritdoc/>
        public async Task<List<InstructivoVersionDto>> ObtenerVersionesAsync(long idTrabajo, string tipoInstructivo)
        {
            try
            {
                var tipo = tipoInstructivo ?? TIPO_INSTRUCTIVO_GENERAL;
                var archivos = await _uploadAdapter.ObtenerArchivosPorContenedorAsync(tipo, idTrabajo);

                return archivos
                    .OrderByDescending(a => a.FechaSubida)
                    .Select(a => new InstructivoVersionDto
                    {
                        IdArchivo = a.IdArchivo,
                        Nombre = a.Nombre,
                        Version = a.Version,
                        FechaSubida = a.FechaSubida.ToString("dd/MM/yyyy HH:mm"),
                        Usuario = a.UsuarioSubida
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo versiones. IdTrabajo: {IdTrabajo}, Tipo: {Tipo}",
                    idTrabajo, tipoInstructivo);
                return new List<InstructivoVersionDto>();
            }
        }

        /// <inheritdoc/>
        public async Task<bool> ValidarPermisoAccesoAsync(long idTrabajo, long usuarioId, string[] rolesPermitidos)
        {
            try
            {
                var trabajo = await _trabajosService.ObtenerAsync(idTrabajo);
                if (trabajo == null)
                {
                    return false;
                }

                // Los usuarios con roles especiales siempre tienen acceso
                // La validación de roles se hace en el controller con User.IsInRole
                // Aquí solo validamos que el trabajo exista
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando permisos. IdTrabajo: {IdTrabajo}, Usuario: {UserId}",
                    idTrabajo, usuarioId);
                return false;
            }
        }
    }
}
