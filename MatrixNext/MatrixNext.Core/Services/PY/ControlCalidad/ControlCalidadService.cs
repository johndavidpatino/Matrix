using MatrixNext.Core.Interfaces.PY.ControlCalidad;
using MatrixNext.Infrastructure.Adapters.PY.ControlCalidad;
using MatrixNext.Web.DTOs.PY.ControlCalidad;

namespace MatrixNext.Core.Services.PY.ControlCalidad
{
    /// <summary>
    /// Servicio para lógica de negocio de Control de Calidad
    /// </summary>
    public class ControlCalidadService : IControlCalidadService
    {
        private readonly IControlCalidadAdapter _adapter;
        private readonly IPreguntasAdapter _preguntasAdapter;
        private readonly ILogger<ControlCalidadService> _logger;

        public ControlCalidadService(
            IControlCalidadAdapter adapter,
            IPreguntasAdapter preguntasAdapter,
            ILogger<ControlCalidadService> logger)
        {
            _adapter = adapter;
            _preguntasAdapter = preguntasAdapter;
            _logger = logger;
        }

        public async Task<List<ControlCalidadListDto>> ObtenerTodosAsync(int tipoProceso)
        {
            try
            {
                // Validar tipo de proceso
                if (tipoProceso < 1 || tipoProceso > 5)
                {
                    _logger.LogWarning("Tipo de proceso inválido: {TipoProceso}", tipoProceso);
                    return new List<ControlCalidadListDto>();
                }

                var controles = await _adapter.ObtenerTodosAsync(tipoProceso);
                return controles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo todos los controles de calidad - TipoProceso: {TipoProceso}", tipoProceso);
                throw;
            }
        }

        public async Task<List<ControlCalidadListDto>> ObtenerPorTrabajoAsync(long trabajoId, int tipoProceso)
        {
            try
            {
                if (trabajoId <= 0)
                {
                    return new List<ControlCalidadListDto>();
                }

                var controles = await _adapter.ObtenerPorTrabajoAsync(trabajoId, tipoProceso);
                return controles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo controles de calidad para trabajo {TrabajoId}", trabajoId);
                throw;
            }
        }

        public async Task<ControlCalidadDetailDto> ObtenerPorIdAsync(long id)
        {
            try
            {
                var control = await _adapter.ObtenerPorIdAsync(id);
                return control;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo control de calidad {Id}", id);
                throw;
            }
        }

        public async Task<(bool success, string message, long id)> CrearAsync(ControlCalidadInputDto dto, int userId)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(dto.Evaluador))
                {
                    return (false, "El evaluador es requerido", 0);
                }

                if (string.IsNullOrWhiteSpace(dto.RolEvaluador))
                {
                    return (false, "El rol del evaluador es requerido", 0);
                }

                if (dto.PersonaId <= 0)
                {
                    return (false, "Debe seleccionar un analista responsable", 0);
                }

                if (dto.Fecha > DateTime.Now)
                {
                    return (false, "La fecha de evaluación no puede ser futura", 0);
                }

                if (dto.Detalles == null || dto.Detalles.Count == 0)
                {
                    return (false, "Debe responder al menos una pregunta", 0);
                }

                // Crear
                long controlId = await _adapter.CrearAsync(dto, userId);

                _logger.LogInformation("Control de calidad {Id} creado por usuario {UserId}", controlId, userId);
                return (true, "Control de calidad creado exitosamente", controlId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando control de calidad. UserId: {UserId}", userId);
                return (false, "Error al crear el control de calidad", 0);
            }
        }

        public async Task<(bool success, string message)> EditarAsync(long id, ControlCalidadInputDto dto, int userId)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(dto.Evaluador))
                {
                    return (false, "El evaluador es requerido");
                }

                if (string.IsNullOrWhiteSpace(dto.RolEvaluador))
                {
                    return (false, "El rol del evaluador es requerido");
                }

                if (dto.PersonaId <= 0)
                {
                    return (false, "Debe seleccionar un analista responsable");
                }

                if (dto.Detalles == null || dto.Detalles.Count == 0)
                {
                    return (false, "Debe responder al menos una pregunta");
                }

                // Verificar que el control existe
                var existe = await _adapter.ObtenerPorIdAsync(id);
                if (existe == null)
                {
                    return (false, "El control de calidad no existe");
                }

                // Editar
                await _adapter.EditarAsync(id, dto, userId);

                _logger.LogInformation("Control de calidad {Id} actualizado por usuario {UserId}", id, userId);
                return (true, "Control de calidad actualizado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando control de calidad {Id}. UserId: {UserId}", id, userId);
                return (false, "Error al actualizar el control de calidad");
            }
        }

        public async Task<(bool success, string message)> EliminarAsync(long id, int userId)
        {
            try
            {
                // Verificar que existe
                var existe = await _adapter.ObtenerPorIdAsync(id);
                if (existe == null)
                {
                    return (false, "El control de calidad no existe");
                }

                // Eliminar
                await _adapter.EliminarAsync(id);

                _logger.LogInformation("Control de calidad {Id} eliminado por usuario {UserId}", id, userId);
                return (true, "Control de calidad eliminado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando control de calidad {Id}. UserId: {UserId}", id, userId);
                return (false, "Error al eliminar el control de calidad");
            }
        }

        public async Task<List<PreguntaListDto>> ObtenerPreguntasActivasAsync(int tipoProceso)
        {
            try
            {
                var preguntas = await _preguntasAdapter.ObtenerPorTipoAsync(tipoProceso);
                return preguntas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo preguntas para tipo {TipoProceso}", tipoProceso);
                throw;
            }
        }
    }
}
