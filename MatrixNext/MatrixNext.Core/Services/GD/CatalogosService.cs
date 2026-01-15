using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatrixNext.Core.DTOs.GD;
using MatrixNext.Infrastructure.Adapters.GD;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Core.Services.GD
{
    /// <summary>
    /// Interfaz para servicio de Catálogos
    /// </summary>
    public interface ICatalogosService
    {
        // ========== Tipos de Solicitud ==========
        Task<IEnumerable<TipoSolicitudDto>> ObtenerTiposSolicitudAsync(bool soloActivos = false);
        Task<TipoSolicitudDto> ObtenerTipoSolicitudAsync(long idTipoSolicitud);
        Task<(bool exitoso, string mensaje)> ActualizarTipoSolicitudAsync(TipoSolicitudDto tipoSolicitud, long usuarioModifica);
        Task<(bool exitoso, string mensaje)> EliminarTipoSolicitudAsync(long idTipoSolicitud, long usuarioModifica);

        // ========== Estados ==========
        Task<IEnumerable<EstadoDto>> ObtenerEstadosAsync(bool soloActivos = false);
        Task<IEnumerable<EstadoDto>> ObtenerEstadosPorModuloAsync(string modulo, bool soloActivos = false);
        Task<EstadoDto> ObtenerEstadoAsync(long idEstado);
        Task<(bool exitoso, string mensaje)> ActualizarEstadoAsync(EstadoDto estado, long usuarioModifica);
        Task<(bool exitoso, string mensaje)> EliminarEstadoAsync(long idEstado, long usuarioModifica);

        // ========== Procesos ==========
        Task<IEnumerable<ProcesoDto>> ObtenerProcesosAsync(bool soloActivos = false);
        Task<ProcesoDto> ObtenerProcesoAsync(long idProceso);
        Task<(bool exitoso, string mensaje)> ActualizarProcesoAsync(ProcesoDto proceso, long usuarioModifica);
        Task<(bool exitoso, string mensaje)> EliminarProcesoAsync(long idProceso, long usuarioModifica);

        // ========== Resumen ==========
        Task<CatalogosResumenDto> ObtenerResumenAsync();
    }

    /// <summary>
    /// Servicio de Catálogos
    /// Sprint 12.3.8: Catálogos Edición con Datos
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

        #region Estados

        public async Task<IEnumerable<EstadoDto>> ObtenerEstadosAsync(bool soloActivos = false)
        {
            try
            {
                _logger.LogInformation("Obteniendo estados (soloActivos={SoloActivos})", soloActivos);
                return await _adapter.ObtenerEstadosAsync(soloActivos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo estados");
                return Enumerable.Empty<EstadoDto>();
            }
        }

        public async Task<IEnumerable<EstadoDto>> ObtenerEstadosPorModuloAsync(string modulo, bool soloActivos = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(modulo))
                {
                    _logger.LogWarning("Módulo vacío para obtener estados");
                    return Enumerable.Empty<EstadoDto>();
                }

                _logger.LogInformation("Obteniendo estados para módulo: {Modulo}", modulo);
                return await _adapter.ObtenerEstadosPorModuloAsync(modulo, soloActivos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo estados para módulo {Modulo}", modulo);
                return Enumerable.Empty<EstadoDto>();
            }
        }

        public async Task<EstadoDto> ObtenerEstadoAsync(long idEstado)
        {
            try
            {
                if (idEstado <= 0)
                {
                    _logger.LogWarning("ID de estado inválido: {IdEstado}", idEstado);
                    return null;
                }

                _logger.LogInformation("Obteniendo estado: {IdEstado}", idEstado);
                return await _adapter.ObtenerEstadoAsync(idEstado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo estado {IdEstado}", idEstado);
                return null;
            }
        }

        public async Task<(bool exitoso, string mensaje)> ActualizarEstadoAsync(
            EstadoDto estado, long usuarioModifica)
        {
            try
            {
                if (estado == null || estado.IdEstado <= 0)
                {
                    _logger.LogWarning("Intento de actualizar estado con ID inválido");
                    return (false, "El ID del estado es inválido");
                }

                if (string.IsNullOrWhiteSpace(estado.Nombre))
                {
                    _logger.LogWarning("Estado sin nombre");
                    return (false, "El nombre es obligatorio");
                }

                // Verificar que existe
                var existe = await _adapter.ObtenerEstadoAsync(estado.IdEstado);
                if (existe == null)
                {
                    _logger.LogWarning("Intento de actualizar estado inexistente: {IdEstado}", estado.IdEstado);
                    return (false, "El estado no existe");
                }

                _logger.LogInformation("Actualizando estado: {IdEstado}, Usuario: {UsuarioId}",
                    estado.IdEstado, usuarioModifica);

                var resultado = await _adapter.ActualizarEstadoAsync(estado, usuarioModifica);

                if (resultado)
                {
                    _logger.LogInformation("Estado {IdEstado} actualizado exitosamente", estado.IdEstado);
                    return (true, "Estado actualizado exitosamente");
                }

                return (false, "No se pudo actualizar el estado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando estado {IdEstado}", estado?.IdEstado);
                return (false, "Error al actualizar. Por favor intente nuevamente");
            }
        }

        public async Task<(bool exitoso, string mensaje)> EliminarEstadoAsync(
            long idEstado, long usuarioModifica)
        {
            try
            {
                if (idEstado <= 0)
                {
                    _logger.LogWarning("ID de estado inválido para eliminar: {IdEstado}", idEstado);
                    return (false, "El ID es inválido");
                }

                // Verificar que existe
                var existe = await _adapter.ObtenerEstadoAsync(idEstado);
                if (existe == null)
                {
                    _logger.LogWarning("Intento de eliminar estado inexistente: {IdEstado}", idEstado);
                    return (false, "El estado no existe");
                }

                _logger.LogInformation("Desactivando estado: {IdEstado}, Usuario: {UsuarioId}",
                    idEstado, usuarioModifica);

                var resultado = await _adapter.DesactivarEstadoAsync(idEstado, usuarioModifica);

                if (resultado)
                {
                    _logger.LogInformation("Estado {IdEstado} desactivado exitosamente", idEstado);
                    return (true, "Estado desactivado exitosamente");
                }

                return (false, "No se pudo desactivar el estado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error desactivando estado {IdEstado}", idEstado);
                return (false, "Error al desactivar. Por favor intente nuevamente");
            }
        }

        #endregion

        #region Procesos

        public async Task<IEnumerable<ProcesoDto>> ObtenerProcesosAsync(bool soloActivos = false)
        {
            try
            {
                _logger.LogInformation("Obteniendo procesos (soloActivos={SoloActivos})", soloActivos);
                return await _adapter.ObtenerProcesosAsync(soloActivos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo procesos");
                return Enumerable.Empty<ProcesoDto>();
            }
        }

        public async Task<ProcesoDto> ObtenerProcesoAsync(long idProceso)
        {
            try
            {
                if (idProceso <= 0)
                {
                    _logger.LogWarning("ID de proceso inválido: {IdProceso}", idProceso);
                    return null;
                }

                _logger.LogInformation("Obteniendo proceso: {IdProceso}", idProceso);
                return await _adapter.ObtenerProcesoAsync(idProceso);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo proceso {IdProceso}", idProceso);
                return null;
            }
        }

        public async Task<(bool exitoso, string mensaje)> ActualizarProcesoAsync(
            ProcesoDto proceso, long usuarioModifica)
        {
            try
            {
                if (proceso == null || proceso.IdProceso <= 0)
                {
                    _logger.LogWarning("Intento de actualizar proceso con ID inválido");
                    return (false, "El ID del proceso es inválido");
                }

                if (string.IsNullOrWhiteSpace(proceso.Nombre))
                {
                    _logger.LogWarning("Proceso sin nombre");
                    return (false, "El nombre es obligatorio");
                }

                // Verificar que existe
                var existe = await _adapter.ObtenerProcesoAsync(proceso.IdProceso);
                if (existe == null)
                {
                    _logger.LogWarning("Intento de actualizar proceso inexistente: {IdProceso}",
                        proceso.IdProceso);
                    return (false, "El proceso no existe");
                }

                _logger.LogInformation("Actualizando proceso: {IdProceso}, Usuario: {UsuarioId}",
                    proceso.IdProceso, usuarioModifica);

                var resultado = await _adapter.ActualizarProcesoAsync(proceso, usuarioModifica);

                if (resultado)
                {
                    _logger.LogInformation("Proceso {IdProceso} actualizado exitosamente", proceso.IdProceso);
                    return (true, "Proceso actualizado exitosamente");
                }

                return (false, "No se pudo actualizar el proceso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando proceso {IdProceso}", proceso?.IdProceso);
                return (false, "Error al actualizar. Por favor intente nuevamente");
            }
        }

        public async Task<(bool exitoso, string mensaje)> EliminarProcesoAsync(
            long idProceso, long usuarioModifica)
        {
            try
            {
                if (idProceso <= 0)
                {
                    _logger.LogWarning("ID de proceso inválido para eliminar: {IdProceso}", idProceso);
                    return (false, "El ID es inválido");
                }

                // Verificar que existe
                var existe = await _adapter.ObtenerProcesoAsync(idProceso);
                if (existe == null)
                {
                    _logger.LogWarning("Intento de eliminar proceso inexistente: {IdProceso}", idProceso);
                    return (false, "El proceso no existe");
                }

                _logger.LogInformation("Desactivando proceso: {IdProceso}, Usuario: {UsuarioId}",
                    idProceso, usuarioModifica);

                var resultado = await _adapter.DesactivarProcesoAsync(idProceso, usuarioModifica);

                if (resultado)
                {
                    _logger.LogInformation("Proceso {IdProceso} desactivado exitosamente", idProceso);
                    return (true, "Proceso desactivado exitosamente");
                }

                return (false, "No se pudo desactivar el proceso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error desactivando proceso {IdProceso}", idProceso);
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
