using Dapper;
using MatrixNext.Core.DTOs.RE_GT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Data.Adapters.RE_GT
{
    /// <summary>
    /// Implementación del adapter para CambioJBI
    /// Acceso a datos mediante Dapper (SPs) y EF Core
    /// </summary>
    public class CambioJBIAdapter : ICambioJBIAdapter
    {
        private readonly IDbConnection _connection;

        public CambioJBIAdapter(IDbConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Obtiene lista de fases activas (SPs: IQ_Fase.ObtenerActivas o similar)
        /// </summary>
        public async Task<List<FaseDto>> ObtenerFasesAsync()
        {
            var sql = @"
                SELECT 
                    IdFase,
                    DescFase
                FROM IQ_Fase
                WHERE Activo = 1
                ORDER BY DescFase
            ";

            try
            {
                var fases = await _connection.QueryAsync<FaseDto>(sql);
                return fases.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener fases desde la base de datos", ex);
            }
        }

        /// <summary>
        /// Obtiene información de trabajo para validación de existencia
        /// </summary>
        public async Task<TrabajoInfoDto> ObtenerTrabajoAsync(int idTrabajo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", idTrabajo);

            try
            {
                var trabajo = await _connection.QueryFirstOrDefaultAsync<TrabajoInfoDto>(
                    @"
                    SELECT 
                        IdTrabajo,
                        IdPropuesta,
                        Alternativa,
                        JobBook,
                        MetCodigo
                    FROM PY_Trabajos
                    WHERE IdTrabajo = @IdTrabajo
                    ",
                    parameters
                );

                return trabajo;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener trabajo {idTrabajo}", ex);
            }
        }

        /// <summary>
        /// Valida si la fase existe en presupuestos del trabajo
        /// </summary>
        public async Task<bool> ValidarFaseCreadaAsync(int idPropuesta, int alternativa, int idFase, string metCodigo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdPropuesta", idPropuesta);
            parameters.Add("@Alternativa", alternativa);
            parameters.Add("@IdFase", idFase);
            parameters.Add("@MetCodigo", metCodigo);

            try
            {
                var existe = await _connection.QueryFirstOrDefaultAsync<bool>(
                    @"
                    SELECT COUNT(*) > 0
                    FROM IQ_Presupuestos
                    WHERE IdPropuesta = @IdPropuesta
                        AND Alternativa = @Alternativa
                        AND IdFase = @IdFase
                        AND MetCodigo = @MetCodigo
                    ",
                    parameters
                );

                return existe;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar fase en presupuestos", ex);
            }
        }

        /// <summary>
        /// Ejecuta SP para cambiar JBI (IQ_JBI.CambiarJBI o UPDATE directo)
        /// </summary>
        public async Task CambiarJBIAsync(CambioJBIDto dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", dto.IdTrabajo);
            parameters.Add("@IdFase", dto.IdFase);
            parameters.Add("@NuevoJBI", dto.NuevoJBI);

            try
            {
                // SP probablemente: IQ_JBI.CambiarJBI
                await _connection.ExecuteAsync(
                    "IQ_JBI.CambiarJBI",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar cambio de JBI en base de datos", ex);
            }
        }

        /// <summary>
        /// Guarda log de cambio de JBI para auditoría
        /// </summary>
        public async Task GuardarLogCambioAsync(LogCambioJBIDto logDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", logDto.IdTrabajo);
            parameters.Add("@JBIAnterior", logDto.JBIAnterior);
            parameters.Add("@JBINuevo", logDto.JBINuevo);
            parameters.Add("@IdUsuario", logDto.IdUsuario);
            parameters.Add("@FechaCambio", logDto.FechaCambio);

            try
            {
                // SP probablemente: IQ_JBI.GuardarLogCambios
                await _connection.ExecuteAsync(
                    "IQ_JBI.GuardarLogCambios",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar log de cambio de JBI", ex);
            }
        }
    }
}
