using MatrixNext.Data.DTOs.RE_GT;
using MatrixNext.Data.Adapters.RE_GT;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.RE_GT
{
    /// <summary>
    /// Implementación del servicio de cambios de JobBook Interno
    /// </summary>
    public class CambioJBIService : ICambioJBIService
    {
        private readonly ICambioJBIAdapter _adapter;
        private readonly ILogger<CambioJBIService> _logger;

        public CambioJBIService(ICambioJBIAdapter adapter, ILogger<CambioJBIService> logger)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene la lista de fases activas para el dropdown
        /// </summary>
        public async Task<IEnumerable<FaseDto>> ObtenerFasesAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo lista de fases activas");
                var fases = await _adapter.ObtenerFasesAsync();
                _logger.LogInformation("Se obtuvieron {Count} fases", fases.Count);
                return fases;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener fases");
                throw;
            }
        }

        /// <summary>
        /// Obtiene información de un trabajo para validar su existencia
        /// </summary>
        public async Task<TrabajoInfoDto?> ObtenerTrabajoAsync(int idTrabajo)
        {
            try
            {
                _logger.LogInformation("Obteniendo información de trabajo {IdTrabajo}", idTrabajo);
                var trabajo = await _adapter.ObtenerTrabajoAsync(idTrabajo);
                
                if (trabajo == null)
                {
                    _logger.LogWarning("Trabajo {IdTrabajo} no existe", idTrabajo);
                }
                
                return trabajo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener trabajo {IdTrabajo}", idTrabajo);
                throw;
            }
        }

        /// <summary>
        /// Valida que la fase existe en presupuestos del trabajo
        /// </summary>
        public async Task<bool> ValidarFaseCreadaAsync(int idPropuesta, int alternativa, int idFase, string metCodigo)
        {
            try
            {
                _logger.LogInformation("Validando fase {IdFase} para propuesta {IdPropuesta}", idFase, idPropuesta);
                var existe = await _adapter.ValidarFaseCreadaAsync(idPropuesta, alternativa, idFase, metCodigo);
                
                if (!existe)
                {
                    _logger.LogWarning("Fase {IdFase} no está creada en presupuestos", idFase);
                }
                
                return existe;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando fase {IdFase}", idFase);
                throw;
            }
        }

        /// <summary>
        /// Realiza el cambio de JBI y registra en auditoría
        /// </summary>
        public async Task<(bool success, string message)> CambiarJBIAsync(CambioJBIDto dto, int usuarioId)
        {
            try
            {
                // Validar formato del JBI (99-999999-99-99)
                if (string.IsNullOrWhiteSpace(dto.NuevoJBI))
                {
                    _logger.LogWarning("Intento de cambio con JBI vacío");
                    return (false, "El nuevo JBI no puede estar vacío");
                }

                if (!ValidarFormatoJBI(dto.NuevoJBI))
                {
                    _logger.LogWarning("Formato de JBI inválido: {JBI}", dto.NuevoJBI);
                    return (false, "El formato del JBI es inválido. Debe ser: 99-999999-99-99");
                }

                // Obtener JBI anterior para auditoría
                var trabajo = await _adapter.ObtenerTrabajoAsync(dto.IdTrabajo);
                if (trabajo == null)
                {
                    _logger.LogWarning("Trabajo {IdTrabajo} no existe al intentar cambiar JBI", dto.IdTrabajo);
                    return (false, "El trabajo no existe");
                }

                var jbIAnterior = trabajo.JobBook;

                // Ejecutar cambio de JBI
                _logger.LogInformation("Cambiando JBI para trabajo {IdTrabajo} de {JBIAnterior} a {JBINuevo}", 
                    dto.IdTrabajo, jbIAnterior, dto.NuevoJBI);
                
                await _adapter.CambiarJBIAsync(dto);

                // Registrar log de cambio
                var logDto = new LogCambioJBIDto
                {
                    IdTrabajo = dto.IdTrabajo,
                    JBIAnterior = jbIAnterior,
                    JBINuevo = dto.NuevoJBI,
                    IdUsuario = usuarioId,
                    FechaCambio = DateTime.UtcNow
                };

                await _adapter.GuardarLogCambioAsync(logDto);

                _logger.LogInformation("Cambio de JBI completado exitosamente para trabajo {IdTrabajo}", dto.IdTrabajo);
                return (true, "El JobBook Interno ha sido cambiado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar JBI para trabajo {IdTrabajo}", dto.IdTrabajo);
                return (false, "Error al realizar el cambio. Por favor intente nuevamente");
            }
        }

        /// <summary>
        /// Valida el formato del JBI (99-999999-99-99)
        /// </summary>
        private bool ValidarFormatoJBI(string jbi)
        {
            if (string.IsNullOrWhiteSpace(jbi))
                return false;

            // Remover guiones y validar que solo contiene números
            var cleanJBI = jbi.Replace("-", "");
            return cleanJBI.Length == 14 && System.Text.RegularExpressions.Regex.IsMatch(cleanJBI, @"^\d{14}$");
        }
    }
}
