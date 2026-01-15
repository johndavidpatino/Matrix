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
                var result = await _connection.QueryAsync<PreguntaListDto>(
                    "PY_Preguntas_Get",
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
                var parameters = new DynamicParameters();
                parameters.Add("@IdTipoProceso", tipoProceso);

                var result = await _connection.QueryAsync<PreguntaListDto>(
                    "PY_Preguntas_GetByTipo",
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
                parameters.Add("@IdProceso", dto.IdProceso);
                parameters.Add("@Pregunta", dto.Pregunta);
                parameters.Add("@Activa", dto.Activa);
                parameters.Add("@RegistradoPor", userId);
                parameters.Add("@IdPregunta", dbType: DbType.Int64, direction: ParameterDirection.Output);

                await _connection.ExecuteAsync(
                    "PY_Preguntas_Add",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                long preguntaId = parameters.Get<long>("@IdPregunta");
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
                parameters.Add("@IdProceso", dto.IdProceso);
                parameters.Add("@Pregunta", dto.Pregunta);
                parameters.Add("@Activa", dto.Activa);
                parameters.Add("@ModificadoPor", userId);

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


