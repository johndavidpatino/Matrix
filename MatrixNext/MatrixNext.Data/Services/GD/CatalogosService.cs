using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatrixNext.Data.DTOs.GD;
using MatrixNext.Data.Adapters.GD;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.GD
{
    /// <summary>
    /// Interfaz para servicio de Catálogos
    /// NOTA: Solo GD_TipoSolicitud implementado. Estados y Procesos removidos (tablas no existen en BD).
    /// </summary>
    public interface ICatalogosService
    {
        // ========== Tipos de Solicitud (FUNCIONAL) ==========
        Task<IEnumerable<TipoSolicitudDto>> ObtenerTiposSolicitudAsync(bool soloActivos = false);
        Task<TipoSolicitudDto> ObtenerTipoSolicitudAsync(long idTipoSolicitud);
        Task<(bool exitoso, string mensaje)> ActualizarTipoSolicitudAsync(TipoSolicitudDto tipoSolicitud, long usuarioModifica);
        Task<(bool exitoso, string mensaje)> EliminarTipoSolicitudAsync(long idTipoSolicitud, long usuarioModifica);

        // ========== Resumen ==========
        Task<CatalogosResumenDto> ObtenerResumenAsync();
    }

    /// <summary>
    /// Servicio de Catálogos GD
    /// NOTA: Solo GD_TipoSolicitud implementado. Estados y Procesos no existen en BD.
    /// </summary>
    public class CatalogosService : ICatalogosService
    {
        private readonly ICatalogosAdapter _adapter;
        private readonly ILogger<CatalogosService> _logger;

        public CatalogosService(ICatalogosAdapter adapter, ILogger<CatalogosService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        #region Tipos de Solicitud

        public async Task<IEnumerable<TipoSolicitudDto>> ObtenerTiposSolicitudAsync(bool soloActivos = false)
        {
            try
            {
                _logger.LogInformation("Obteniendo tipos de solicitud (soloActivos={SoloActivos})", soloActivos);
                return await _adapter.ObtenerTiposSolicitudAsync(soloActivos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo tipos de solicitud");
                return Enumerable.Empty<TipoSolicitudDto>();
            }
        }

        public async Task<TipoSolicitudDto> ObtenerTipoSolicitudAsync(long idTipoSolicitud)
        {
            try
            {
                if (idTipoSolicitud <= 0)
                {
                    _logger.LogWarning("ID de tipo de solicitud inválido: {IdTipoSolicitud}", idTipoSolicitud);
                    return null;
                }

                _logger.LogInformation("Obteniendo tipo de solicitud: {IdTipoSolicitud}", idTipoSolicitud);
                return await _adapter.ObtenerTipoSolicitudAsync(idTipoSolicitud);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo tipo de solicitud {IdTipoSolicitud}", idTipoSolicitud);
                return null;
            }
        }

        public async Task<(bool exitoso, string mensaje)> ActualizarTipoSolicitudAsync(
            TipoSolicitudDto tipoSolicitud, long usuarioModifica)
        {
            try
            {
                if (tipoSolicitud == null || tipoSolicitud.IdTipoSolicitud <= 0)
                {
                    _logger.LogWarning("Intento de actualizar tipo de solicitud con ID inválido");
                    return (false, "El ID del tipo de solicitud es inválido");
                }

                if (string.IsNullOrWhiteSpace(tipoSolicitud.Nombre))
                {
                    _logger.LogWarning("Tipo de solicitud sin nombre");
                    return (false, "El nombre es obligatorio");
                }

                // Verificar que existe
                var existe = await _adapter.ObtenerTipoSolicitudAsync(tipoSolicitud.IdTipoSolicitud);
                if (existe == null)
                {
                    _logger.LogWarning("Intento de actualizar tipo de solicitud inexistente: {IdTipoSolicitud}",
                        tipoSolicitud.IdTipoSolicitud);
                    return (false, "El tipo de solicitud no existe");
                }

                _logger.LogInformation("Actualizando tipo de solicitud: {IdTipoSolicitud}, Usuario: {UsuarioId}",
                    tipoSolicitud.IdTipoSolicitud, usuarioModifica);

                var resultado = await _adapter.ActualizarTipoSolicitudAsync(tipoSolicitud, usuarioModifica);

                if (resultado)
                {
                    _logger.LogInformation("Tipo de solicitud {IdTipoSolicitud} actualizado exitosamente",
                        tipoSolicitud.IdTipoSolicitud);
                    return (true, "Tipo de solicitud actualizado exitosamente");
                }

                return (false, "No se pudo actualizar el tipo de solicitud");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando tipo de solicitud {IdTipoSolicitud}",
                    tipoSolicitud?.IdTipoSolicitud);
                return (false, "Error al actualizar. Por favor intente nuevamente");
            }
        }

        public async Task<(bool exitoso, string mensaje)> EliminarTipoSolicitudAsync(
            long idTipoSolicitud, long usuarioModifica)
        {
            try
            {
                if (idTipoSolicitud <= 0)
                {
                    _logger.LogWarning("ID de tipo de solicitud inválido para eliminar: {IdTipoSolicitud}",
                        idTipoSolicitud);
                    return (false, "El ID es inválido");
                }

                // Verificar que existe
                var existe = await _adapter.ObtenerTipoSolicitudAsync(idTipoSolicitud);
                if (existe == null)
                {
                    _logger.LogWarning("Intento de eliminar tipo de solicitud inexistente: {IdTipoSolicitud}",
                        idTipoSolicitud);
                    return (false, "El tipo de solicitud no existe");
                }

                _logger.LogInformation("Desactivando tipo de solicitud: {IdTipoSolicitud}, Usuario: {UsuarioId}",
                    idTipoSolicitud, usuarioModifica);

                var resultado = await _adapter.DesactivarTipoSolicitudAsync(idTipoSolicitud, usuarioModifica);

                if (resultado)
                {
                    _logger.LogInformation("Tipo de solicitud {IdTipoSolicitud} desactivado exitosamente",
                        idTipoSolicitud);
                    return (true, "Tipo de solicitud desactivado exitosamente");
                }

                return (false, "No se pudo desactivar el tipo de solicitud");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error desactivando tipo de solicitud {IdTipoSolicitud}", idTipoSolicitud);
                return (false, "Error al desactivar. Por favor intente nuevamente");
            }
        }

        #endregion

        #region Resumen

        public async Task<CatalogosResumenDto> ObtenerResumenAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo resumen de catálogos");
                return await _adapter.ObtenerResumenAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo resumen de catálogos");
                return new CatalogosResumenDto();
            }
        }

        #endregion
    }
}

