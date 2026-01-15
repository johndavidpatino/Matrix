using MatrixNext.Data.Adapters.ES;
using MatrixNext.Data.DTOs.ES;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.ES
{
    public class ESMetodologiaCampoService : IESMetodologiaCampoService
    {
        private readonly IESMetodologiaCampoAdapter _adapter;
        private readonly ILogger<ESMetodologiaCampoService> _logger;

        public ESMetodologiaCampoService(
            IESMetodologiaCampoAdapter adapter,
            ILogger<ESMetodologiaCampoService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<IEnumerable<ESMetodologiaCampoOutputDto>> ObtenerTodosAsync()
        {
            try
            {
                return await _adapter.ObtenerTodosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las metodologías");
                throw;
            }
        }

        public async Task<IEnumerable<ESMetodologiaCampoOutputDto>> ObtenerPorTrabajoAsync(long trabajoId)
        {
            try
            {
                return await _adapter.ObtenerPorTrabajoAsync(trabajoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener metodologías por trabajo {TrabajoId}", trabajoId);
                throw;
            }
        }

        public async Task<IEnumerable<ESMetodologiaCampoOutputDto>> ObtenerPendientesAsync()
        {
            try
            {
                return await _adapter.ObtenerPendientesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener metodologías pendientes");
                throw;
            }
        }

        public async Task<ESMetodologiaCampoOutputDto> ObtenerPorIdAsync(long id)
        {
            try
            {
                return await _adapter.ObtenerPorIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener metodología {Id}", id);
                throw;
            }
        }

        public async Task<(bool Success, string Message, long Id)> CrearAsync(ESMetodologiaCampoInputDto dto, long usuarioId)
        {
            try
            {
                // Validaciones de negocio
                if (string.IsNullOrWhiteSpace(dto.NombreEstudio))
                {
                    return (false, "El nombre del estudio es requerido", 0);
                }

                var id = await _adapter.CrearAsync(dto, usuarioId);
                
                _logger.LogInformation(
                    "Metodología de campo {Id} creada exitosamente por usuario {UsuarioId} para trabajo {TrabajoId}",
                    id, usuarioId, dto.TrabajoId
                );

                return (true, "Metodología creada exitosamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear metodología para trabajo {TrabajoId}", dto.TrabajoId);
                return (false, "Error al crear la metodología", 0);
            }
        }

        public async Task<(bool Success, string Message)> ActualizarAsync(long id, ESMetodologiaCampoInputDto dto)
        {
            try
            {
                var existente = await _adapter.ObtenerPorIdAsync(id);
                if (existente == null)
                {
                    return (false, "La metodología no existe");
                }

                // Validaciones de negocio
                if (string.IsNullOrWhiteSpace(dto.NombreEstudio))
                {
                    return (false, "El nombre del estudio es requerido");
                }

                await _adapter.ActualizarAsync(id, dto);
                
                _logger.LogInformation("Metodología {Id} actualizada exitosamente", id);

                return (true, "Metodología actualizada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar metodología {Id}", id);
                return (false, "Error al actualizar la metodología");
            }
        }

        public async Task<(bool Success, string Message)> EliminarAsync(long id)
        {
            try
            {
                var existente = await _adapter.ObtenerPorIdAsync(id);
                if (existente == null)
                {
                    return (false, "La metodología no existe");
                }

                await _adapter.EliminarAsync(id);
                
                _logger.LogInformation("Metodología {Id} eliminada exitosamente", id);

                return (true, "Metodología eliminada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar metodología {Id}", id);
                return (false, "Error al eliminar la metodología");
            }
        }
    }
}
