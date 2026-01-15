using Microsoft.Extensions.Logging;
using MatrixNext.Data.Services.PY.Interfaces;
using MatrixNext.Data.Adapters.PY.ControlCalidad;
using MatrixNext.Data.DTOs.PY.ControlCalidad;

namespace MatrixNext.Data.Services.PY.ControlCalidad
{
    /// <summary>
    /// Servicio para lÃ³gica de negocio de Preguntas de evaluaciÃ³n
    /// </summary>
    public class PreguntasService : IPreguntasService
    {
        private readonly IPreguntasAdapter _adapter;
        private readonly ILogger<PreguntasService> _logger;

        public PreguntasService(
            IPreguntasAdapter adapter,
            ILogger<PreguntasService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<List<PreguntaListDto>> ObtenerPorTipoAsync(int tipoProceso)
        {
            try
            {
                var preguntas = await _adapter.ObtenerPorTipoAsync(tipoProceso);
                return preguntas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo preguntas por tipo {TipoProceso}", tipoProceso);
                throw;
            }
        }

        public async Task<List<PreguntaListDto>> ObtenerPorTipoProcesoAsync(int tipoProceso)
        {
            return await ObtenerPorTipoAsync(tipoProceso);
        }

        public async Task<(bool success, string message, long id)> CrearAsync(PreguntaInputDto dto, int userId)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(dto.Pregunta))
                {
                    return (false, "El texto de la pregunta es requerido", 0);
                }

                if (dto.Pregunta.Length < 5)
                {
                    return (false, "La pregunta debe tener al menos 5 caracteres", 0);
                }

                if (dto.IdProceso <= 0)
                {
                    return (false, "El tipo de proceso es requerido", 0);
                }

                // Crear
                long preguntaId = await _adapter.CrearAsync(dto, userId);

                _logger.LogInformation("Pregunta {Id} creada por usuario {UserId}", preguntaId, userId);
                return (true, "Pregunta creada exitosamente", preguntaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando pregunta. UserId: {UserId}", userId);
                return (false, "Error al crear la pregunta", 0);
            }
        }

        public async Task<(bool success, string message)> EditarAsync(long id, PreguntaInputDto dto, int userId)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(dto.Pregunta))
                {
                    return (false, "El texto de la pregunta es requerido");
                }

                if (dto.Pregunta.Length < 5)
                {
                    return (false, "La pregunta debe tener al menos 5 caracteres");
                }

                if (dto.IdProceso <= 0)
                {
                    return (false, "El tipo de proceso es requerido");
                }

                // Editar
                await _adapter.EditarAsync(id, dto, userId);

                _logger.LogInformation("Pregunta {Id} actualizada por usuario {UserId}", id, userId);
                return (true, "Pregunta actualizada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando pregunta {Id}. UserId: {UserId}", id, userId);
                return (false, "Error al actualizar la pregunta");
            }
        }

        public async Task<(bool success, string message, bool nuevoEstado)> ToggleActivoAsync(long id, int userId)
        {
            try
            {
                var nuevoEstado = await _adapter.ToggleActivoAsync(id, userId);

                _logger.LogInformation("Pregunta {Id} toggled por usuario {UserId}", id, userId);
                return (true, "Estado de la pregunta actualizado exitosamente", nuevoEstado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggleando pregunta {Id}. UserId: {UserId}", id, userId);
                return (false, "Error al actualizar el estado de la pregunta", false);
            }
        }
    }
}


