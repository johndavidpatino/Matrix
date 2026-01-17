using MatrixNext.Data.DTOs.RE_GT;
using MatrixNext.Data.Adapters.RE_GT;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.RE_GT
{
    /// <summary>
    /// Servicio para gestión de asignación de trabajos a coordinadores de campo
    /// </summary>
    public class AsignacionCampoService : IAsignacionCampoService
    {
        private readonly IAsignacionCampoAdapter _adapter;
        private readonly ILogger<AsignacionCampoService> _logger;

        public AsignacionCampoService(IAsignacionCampoAdapter adapter, ILogger<AsignacionCampoService> logger)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene lista paginada de trabajos sin asignación
        /// </summary>
        public async Task<(IEnumerable<TrabajoAsignacionDto> trabajos, int totalRecords)> ObtenerTrabajosParaAsignacionAsync(
            BusquedaAsignacionDto busqueda)
        {
            try
            {
                if (busqueda == null)
                {
                    busqueda = new BusquedaAsignacionDto { PageIndex = 0, PageSize = 10 };
                }

                _logger.LogInformation("Obteniendo trabajos para asignación. PageIndex: {PageIndex}, PageSize: {PageSize}",
                    busqueda.PageIndex, busqueda.PageSize);

                var result = await _adapter.ObtenerTrabajosParaAsignacionAsync(busqueda);
                
                _logger.LogInformation("Se encontraron {Count} trabajos", result.trabajos.Count());
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo trabajos para asignación");
                throw;
            }
        }

        /// <summary>
        /// Obtiene información del trabajo por ID
        /// </summary>
        public async Task<TrabajoAsignacionDto?> ObtenerTrabajoAsync(int idTrabajo)
        {
            try
            {
                _logger.LogInformation("Obteniendo información de trabajo {IdTrabajo}", idTrabajo);

                var trabajo = await _adapter.ObtenerTrabajoAsync(idTrabajo);
                
                if (trabajo == null)
                {
                    _logger.LogWarning("Trabajo no encontrado: {IdTrabajo}", idTrabajo);
                }

                return trabajo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo información de trabajo {IdTrabajo}", idTrabajo);
                throw;
            }
        }

        /// <summary>
        /// Obtiene lista de usuarios COE disponibles
        /// </summary>
        public async Task<IEnumerable<UsuarioCOEDto>> ObtenerUsuariosCOEAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo usuarios COE");

                var usuarios = await _adapter.ObtenerUsuariosCOEAsync();
                
                _logger.LogInformation("Se encontraron {Count} usuarios COE", usuarios.Count());
                
                return usuarios;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo usuarios COE");
                throw;
            }
        }

        /// <summary>
        /// Valida que el trabajo exista y esté en estado válido
        /// </summary>
        public async Task<(bool valid, string message)> ValidarTrabajoAsync(int idTrabajo)
        {
            try
            {
                if (idTrabajo <= 0)
                {
                    return (false, "ID de Trabajo inválido");
                }

                var trabajo = await _adapter.ObtenerTrabajoAsync(idTrabajo);
                
                if (trabajo == null)
                {
                    _logger.LogWarning("Intento de validar trabajo inexistente: {IdTrabajo}", idTrabajo);
                    return (false, "El trabajo NO existe");
                }

                if (string.IsNullOrEmpty(trabajo.Estado))
                {
                    return (false, "El trabajo no tiene estado válido");
                }

                return (true, "Validación exitosa");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando trabajo {IdTrabajo}", idTrabajo);
                return (false, "Error al validar el trabajo");
            }
        }

        /// <summary>
        /// Realiza la asignación del trabajo a coordinador de campo
        /// </summary>
        public async Task<(bool success, string message)> AsignarTrabajoCampoAsync(
            AsignacionCampoDto dto, int usuarioId)
        {
            try
            {
                if (dto == null || dto.IdTrabajo <= 0 || dto.IdCOE <= 0)
                {
                    return (false, "Datos inválidos para asignación");
                }

                _logger.LogInformation("Iniciando asignación de trabajo {IdTrabajo} a COE {IdCOE}",
                    dto.IdTrabajo, dto.IdCOE);

                // Obtener información del trabajo para audit trail
                var trabajo = await _adapter.ObtenerTrabajoAsync(dto.IdTrabajo);
                if (trabajo == null)
                {
                    return (false, "El trabajo no existe");
                }

                // Obtener COE anterior para log
                int coeAnterior = trabajo.IdCOEActual;

                // Realizar asignación
                await _adapter.AsignarTrabajoCampoAsync(dto);

                // Registrar cambio
                var logDto = new LogAsignacionCampoDto
                {
                    IdTrabajo = dto.IdTrabajo,
                    COEAnterior = coeAnterior,
                    COENuevo = dto.IdCOE,
                    PersonaAnterior = null, // Opcional si se requiere
                    PersonaNueva = dto.IdPersona,
                    IdUsuario = usuarioId,
                    FechaCambio = DateTime.Now
                };

                await _adapter.GuardarLogAsignacionAsync(logDto);

                _logger.LogInformation("Asignación exitosa de trabajo {IdTrabajo} a COE {IdCOE}",
                    dto.IdTrabajo, dto.IdCOE);

                return (true, "Asignación realizada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al asignar trabajo {IdTrabajo}. Usuario: {UserId}",
                    dto?.IdTrabajo, usuarioId);
                return (false, "Error al realizar la asignación");
            }
        }

        /// <summary>
        /// Obtiene lista de COEs
        /// </summary>
        public async Task<IEnumerable<dynamic>> ObtenerCOEsAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo lista de COEs");

                var coes = await _adapter.ObtenerCOEsAsync();
                
                _logger.LogInformation("Se encontraron {Count} COEs", coes.Count());
                
                return coes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo COEs");
                throw;
            }
        }
    }
}
