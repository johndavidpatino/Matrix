using MatrixNext.Data.Adapters.ES;
using MatrixNext.Data.DTOs.ES;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.ES
{
    public class ESDisenoMuestralService : IESDisenoMuestralService
    {
        private readonly IESDisenoMuestralAdapter _adapter;
        private readonly ILogger<ESDisenoMuestralService> _logger;

        public ESDisenoMuestralService(
            IESDisenoMuestralAdapter adapter,
            ILogger<ESDisenoMuestralService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<IEnumerable<ESDisenoMuestralOutputDto>> ObtenerTodosAsync()
        {
            try
            {
                return await _adapter.ObtenerTodosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los diseños muestrales");
                throw;
            }
        }

        public async Task<IEnumerable<ESDisenoMuestralOutputDto>> ObtenerPorBriefAsync(long briefId)
        {
            try
            {
                return await _adapter.ObtenerPorBriefAsync(briefId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener diseños por brief {BriefId}", briefId);
                throw;
            }
        }

        public async Task<ESDisenoMuestralOutputDto> ObtenerPorIdAsync(long id)
        {
            try
            {
                return await _adapter.ObtenerPorIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener diseño muestral {Id}", id);
                throw;
            }
        }

        public async Task<(bool Success, string Message, long Id)> CrearAsync(ESDisenoMuestralInputDto dto)
        {
            try
            {
                var id = await _adapter.CrearAsync(dto);
                
                _logger.LogInformation("Diseño muestral {Id} creado exitosamente para brief {BriefId}", id, dto.BriefId);

                return (true, "Diseño muestral creado exitosamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear diseño muestral para brief {BriefId}", dto.BriefId);
                return (false, "Error al crear el diseño muestral", 0);
            }
        }

        public async Task<(bool Success, string Message)> ActualizarAsync(long id, ESDisenoMuestralInputDto dto)
        {
            try
            {
                var existente = await _adapter.ObtenerPorIdAsync(id);
                if (existente == null)
                {
                    return (false, "El diseño muestral no existe");
                }

                await _adapter.ActualizarAsync(id, dto);
                
                _logger.LogInformation("Diseño muestral {Id} actualizado exitosamente", id);

                return (true, "Diseño muestral actualizado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar diseño muestral {Id}", id);
                return (false, "Error al actualizar el diseño muestral");
            }
        }

        public async Task<(bool Success, string Message)> EliminarAsync(long id)
        {
            try
            {
                var existente = await _adapter.ObtenerPorIdAsync(id);
                if (existente == null)
                {
                    return (false, "El diseño muestral no existe");
                }

                await _adapter.EliminarAsync(id);
                
                _logger.LogInformation("Diseño muestral {Id} eliminado exitosamente", id);

                return (true, "Diseño muestral eliminado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar diseño muestral {Id}", id);
                return (false, "Error al eliminar el diseño muestral");
            }
        }
    }
}
