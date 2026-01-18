using Microsoft.Extensions.Logging;
using Dapper;
using MatrixNext.Data.DTOs.PY.ControlCalidad;
using System.Data;

namespace MatrixNext.Data.Adapters.PY.ControlCalidad
{
    /// <summary>
    /// Adapter para acceso a datos de Preguntas usando Dapper
    /// </summary>
    public class PreguntasAdapter : IPreguntasAdapter
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<PreguntasAdapter> _logger;

        public PreguntasAdapter(IDbConnection connection, ILogger<PreguntasAdapter> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<List<PreguntaListDto>> ObtenerTodasAsync()
        {
            try
            {
                // SP PY_Preguntas_Get acepta @ID y @IdTipo
                // Pasar null en ambos para obtener todas
                var parameters = new DynamicParameters();
                parameters.Add("@ID", null);
                parameters.Add("@IdTipo", null);

                var result = await _connection.QueryAsync<PreguntaListDto>(
                    "PY_Preguntas_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.OrderBy(p => p.Orden).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo todas las preguntas");
                throw;
            }
        }

        public async Task<List<PreguntaListDto>> ObtenerPorTipoAsync(int tipoProceso)
        {
            try
            {
                // SP PY_Preguntas_Get acepta @ID y @IdTipo
                var parameters = new DynamicParameters();
                parameters.Add("@ID", null);
                parameters.Add("@IdTipo", tipoProceso);

                var result = await _connection.QueryAsync<PreguntaListDto>(
                    "PY_Preguntas_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.Where(p => p.Activa).OrderBy(p => p.Orden).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo preguntas por tipo {TipoProceso}", tipoProceso);
                throw;
            }
        }

        public async Task<long> CrearAsync(PreguntaInputDto dto, int userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Pregunta", dto.Pregunta);
                parameters.Add("@IdTipo", dto.IdProceso); // El SP usa @IdTipo, no @IdProceso
                parameters.Add("@Activa", dto.Activa);
                // Nota: El SP no tiene @RegistradoPor ni @IdPregunta OUTPUT

                await _connection.ExecuteAsync(
                    "PY_Preguntas_Add",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                // Obtener el ID insertado usando SCOPE_IDENTITY
                long preguntaId = await _connection.QueryFirstAsync<long>("SELECT SCOPE_IDENTITY()");
                _logger.LogInformation("Pregunta {Id} creada por usuario {UserId}", preguntaId, userId);
                return preguntaId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando pregunta para tipo {TipoProceso}", dto.IdProceso);
                throw;
            }
        }

        public async Task EditarAsync(long id, PreguntaInputDto dto, int userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdPregunta", id);
                parameters.Add("@Pregunta", dto.Pregunta);
                parameters.Add("@IdTipo", dto.IdProceso); // El SP usa @IdTipo, no @IdProceso
                parameters.Add("@Activa", dto.Activa);
                // Nota: El SP no tiene @ModificadoPor

                await _connection.ExecuteAsync(
                    "PY_Preguntas_Edit",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("Pregunta {Id} actualizada por usuario {UserId}", id, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando pregunta {Id}", id);
                throw;
            }
        }

        public async Task<bool> ToggleActivoAsync(long id, int userId)
        {
            try
            {
                // Obtener pregunta actual para toggle
                var current = await _connection.QueryFirstOrDefaultAsync<dynamic>(
                    "SELECT IdProceso, Pregunta, Activa FROM PY_Preguntas WHERE IdPregunta = @Id",
                    new { Id = id }
                );

                if (current == null)
                {
                    throw new InvalidOperationException("Pregunta no encontrada");
                }

                bool nuevoEstado = !current.Activa;

                var parameters = new DynamicParameters();
                parameters.Add("@IdPregunta", id);
                parameters.Add("@IdProceso", current.IdProceso);
                parameters.Add("@Pregunta", current.Pregunta);
                parameters.Add("@Activa", nuevoEstado);
                parameters.Add("@ModificadoPor", userId);

                await _connection.ExecuteAsync(
                    "PY_Preguntas_Edit",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("Pregunta {Id} toggleada a {Activo} por usuario {UserId}", 
                    id, nuevoEstado, userId);
                return nuevoEstado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggleando pregunta {Id}", id);
                throw;
            }
        }
    }
}


