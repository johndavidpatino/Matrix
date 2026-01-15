using Dapper;
using MatrixNext.Web.DTOs.PY.ControlCalidad;
using System.Data;

namespace MatrixNext.Infrastructure.Adapters.PY.ControlCalidad
{
    /// <summary>
    /// Adapter para acceso a datos de Control de Calidad usando Dapper
    /// </summary>
    public class ControlCalidadAdapter : IControlCalidadAdapter
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<ControlCalidadAdapter> _logger;

        public ControlCalidadAdapter(IDbConnection connection, ILogger<ControlCalidadAdapter> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<List<ControlCalidadListDto>> ObtenerTodosAsync(int tipoProceso)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TipoProceso", tipoProceso);

                var result = await _connection.QueryAsync<ControlCalidadListDto>(
                    "PY_ControlCalidad_GetByTipo",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ObtenerTodosAsync - TipoProceso: {TipoProceso}", tipoProceso);
                throw;
            }
        }

        public async Task<List<ControlCalidadListDto>> ObtenerPorTrabajoAsync(long trabajoId, int tipoProceso)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", trabajoId);
                parameters.Add("@TipoProceso", tipoProceso);

                var result = await _connection.QueryAsync<ControlCalidadListDto>(
                    "PY_ControlCalidad_GetByTrabajo",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
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
                var parameters = new DynamicParameters();
                parameters.Add("@ID", id);

                var result = await _connection.QueryFirstOrDefaultAsync<ControlCalidadDetailDto>(
                    "PY_ControlCalidad_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (result != null)
                {
                    // Cargar detalles
                    result.Detalles = await ObtenerDetallesAsync(id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo control de calidad {Id}", id);
                throw;
            }
        }

        public async Task<long> CrearAsync(ControlCalidadInputDto dto, int userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", dto.TrabajoId);
                parameters.Add("@Evaluador", dto.Evaluador);
                parameters.Add("@RolEvaluador", dto.RolEvaluador);
                parameters.Add("@Persona", dto.PersonaId);
                parameters.Add("@Fecha", dto.Fecha);
                parameters.Add("@TipoProceso", dto.TipoProceso);
                parameters.Add("@RegistradoPor", userId);
                parameters.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);

                await _connection.ExecuteAsync(
                    "PY_ControlCalidad_Add",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                long controlId = parameters.Get<long>("@Id");

                // Guardar detalles
                if (dto.Detalles?.Count > 0)
                {
                    await GuardarDetallesAsync(controlId, dto.Detalles, userId);
                }

                _logger.LogInformation("Control de calidad {Id} creado por usuario {UserId}", controlId, userId);
                return controlId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando control de calidad para trabajo {TrabajoId}", dto.TrabajoId);
                throw;
            }
        }

        public async Task EditarAsync(long id, ControlCalidadInputDto dto, int userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                parameters.Add("@TrabajoId", dto.TrabajoId);
                parameters.Add("@Evaluador", dto.Evaluador);
                parameters.Add("@RolEvaluador", dto.RolEvaluador);
                parameters.Add("@Persona", dto.PersonaId);
                parameters.Add("@Fecha", dto.Fecha);
                parameters.Add("@TipoProceso", dto.TipoProceso);
                parameters.Add("@ModificadoPor", userId);

                await _connection.ExecuteAsync(
                    "PY_ControlCalidad_Edit",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                // Eliminar detalles anteriores y guardar nuevos
                await EliminarDetallesAsync(id);
                if (dto.Detalles?.Count > 0)
                {
                    await GuardarDetallesAsync(id, dto.Detalles, userId);
                }

                _logger.LogInformation("Control de calidad {Id} actualizado por usuario {UserId}", id, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando control de calidad {Id}", id);
                throw;
            }
        }

        public async Task EliminarAsync(long id)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdControlCalidad", id);

                await _connection.ExecuteAsync(
                    "PY_ControlCalidad_Del",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("Control de calidad {Id} eliminado", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando control de calidad {Id}", id);
                throw;
            }
        }

        public async Task<List<DetalleControlCalidadDetailDto>> ObtenerDetallesAsync(long controlCalidadId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdControlCalidad", controlCalidadId);

                var result = await _connection.QueryAsync<DetalleControlCalidadDetailDto>(
                    "PY_DetalleControlCalidad_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo detalles de control {ControlCalidadId}", controlCalidadId);
                throw;
            }
        }

        public async Task GuardarDetallesAsync(long controlCalidadId, List<DetalleControlCalidadInputDto> detalles, int userId)
        {
            try
            {
                foreach (var detalle in detalles)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@IdControlCalidad", controlCalidadId);
                    parameters.Add("@IdPregunta", detalle.IdPregunta);
                    parameters.Add("@SI", detalle.Cumple);
                    parameters.Add("@Comentarios", detalle.Comentarios ?? string.Empty);
                    parameters.Add("@RegistradoPor", userId);
                    parameters.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    await _connection.ExecuteAsync(
                        "PY_DetalleControlCalidad_Add",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }

                _logger.LogInformation("Detalles de control de calidad {ControlCalidadId} guardados", controlCalidadId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando detalles de control {ControlCalidadId}", controlCalidadId);
                throw;
            }
        }

        private async Task EliminarDetallesAsync(long controlCalidadId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdControlCalidad", controlCalidadId);

                await _connection.ExecuteAsync(
                    "PY_DetalleControlCalidad_DelxIdControl",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando detalles de control {ControlCalidadId}", controlCalidadId);
                throw;
            }
        }
    }
}
