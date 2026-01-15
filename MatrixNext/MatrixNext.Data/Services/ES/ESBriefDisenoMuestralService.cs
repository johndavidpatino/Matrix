using MatrixNext.Data.Adapters.ES;
using MatrixNext.Data.DTOs.ES;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.ES
{
    /// <summary>
    /// Servicio para lógica de negocio de Brief Diseño Muestral
    /// </summary>
    public class ESBriefDisenoMuestralService : IESBriefDisenoMuestralService
    {
        private readonly IESBriefDisenoMuestralAdapter _adapter;
        private readonly ILogger<ESBriefDisenoMuestralService> _logger;

        public ESBriefDisenoMuestralService(
            IESBriefDisenoMuestralAdapter adapter,
            ILogger<ESBriefDisenoMuestralService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<IEnumerable<ESBriefDisenoMuestralOutputDto>> ObtenerTodosAsync()
        {
            try
            {
                return await _adapter.ObtenerTodosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los briefs de diseño muestral");
                throw;
            }
        }

        public async Task<IEnumerable<ESBriefDisenoMuestralOutputDto>> ObtenerPorPropuestaAsync(long propuestaId)
        {
            try
            {
                return await _adapter.ObtenerPorPropuestaAsync(propuestaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener briefs por propuesta {PropuestaId}", propuestaId);
                throw;
            }
        }

        public async Task<IEnumerable<ESBriefDisenoMuestralOutputDto>> ObtenerPendientesAsync()
        {
            try
            {
                return await _adapter.ObtenerPendientesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener briefs pendientes");
                throw;
            }
        }

        public async Task<ESBriefDisenoMuestralOutputDto> ObtenerPorIdAsync(long id)
        {
            try
            {
                return await _adapter.ObtenerPorIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener brief {Id}", id);
                throw;
            }
        }

        public async Task<(bool Success, string Message, long Id)> CrearAsync(ESBriefDisenoMuestralInputDto dto, long usuarioId)
        {
            try
            {
                // Validaciones de negocio
                if (string.IsNullOrWhiteSpace(dto.Objetivo))
                {
                    return (false, "El objetivo es requerido", 0);
                }

                var id = await _adapter.CrearAsync(dto, usuarioId);
                
                _logger.LogInformation(
                    "Brief diseño muestral {Id} creado exitosamente por usuario {UsuarioId} para propuesta {PropuestaId}",
                    id, usuarioId, dto.PropuestaId
                );

                return (true, "Brief creado exitosamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear brief diseño muestral para propuesta {PropuestaId}", dto.PropuestaId);
                return (false, "Error al crear el brief de diseño muestral", 0);
            }
        }

        public async Task<(bool Success, string Message)> ActualizarAsync(long id, ESBriefDisenoMuestralInputDto dto)
        {
            try
            {
                // Validar que exista
                var existente = await _adapter.ObtenerPorIdAsync(id);
                if (existente == null)
                {
                    return (false, "El brief no existe");
                }

                // Validaciones de negocio
                if (string.IsNullOrWhiteSpace(dto.Objetivo))
                {
                    return (false, "El objetivo es requerido");
                }

                await _adapter.ActualizarAsync(id, dto);
                
                _logger.LogInformation("Brief diseño muestral {Id} actualizado exitosamente", id);

                return (true, "Brief actualizado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar brief {Id}", id);
                return (false, "Error al actualizar el brief");
            }
        }

        public async Task<(bool Success, string Message)> EliminarAsync(long id)
        {
            try
            {
                // Validar que exista
                var existente = await _adapter.ObtenerPorIdAsync(id);
                if (existente == null)
                {
                    return (false, "El brief no existe");
                }

                await _adapter.EliminarAsync(id);
                
                _logger.LogInformation("Brief diseño muestral {Id} eliminado exitosamente", id);

                return (true, "Brief eliminado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar brief {Id}", id);
                return (false, "Error al eliminar el brief");
            }
        }
    }
}
