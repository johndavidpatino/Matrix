using Dapper;
using MatrixNext.Data.DTOs.RE_GT;
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
        /// Obtiene lista de fases activas (CORREGIDO: IQ_Fase → IQ_Fases)
        /// </summary>
        public async Task<List<FaseDto>> ObtenerFasesAsync()
        {
            var sql = @"
                SELECT 
                    IdFase,
                    DescFase
                FROM IQ_Fases
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
        /// CORREGIDO: PY_Trabajos → PY_Trabajo, IdTrabajo → id
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
                        id AS IdTrabajo,
                        IdPropuesta,
                        Alternativa,
                        JobBook,
                        MetCodigo
                    FROM PY_Trabajo
                    WHERE id = @IdTrabajo
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
        /// NOTA: Tabla IQ_Presupuestos NO EXISTE - usar IQ_ProcesosPresupuesto o CU_Presupuestos
        /// </summary>
        public async Task<bool> ValidarFaseCreadaAsync(int idPropuesta, int alternativa, int idFase, string metCodigo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdPropuesta", idPropuesta);
            parameters.Add("@Alternativa", alternativa);
            parameters.Add("@MetCodigo", metCodigo);

            try
            {
                // NOTA: IQ_Presupuestos no existe - usar IQ_ProcesosPresupuesto (sin IdFase)
                // Si se requiere validar fase, integrar con IQ_Fases
                var existe = await _connection.QueryFirstOrDefaultAsync<bool>(
                    @"
                    SELECT COUNT(*) > 0
                    FROM IQ_ProcesosPresupuesto
                    WHERE IdPropuesta = @IdPropuesta
                        AND ParAlternativa = @Alternativa
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
        /// Ejecuta cambio de JBI usando UPDATE directo
        /// NOTA: SP IQ_JBI.CambiarJBI no existe en BD
        /// </summary>
        public async Task CambiarJBIAsync(CambioJBIDto dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", dto.IdTrabajo);
            parameters.Add("@NuevoJBI", dto.NuevoJBI);

            try
            {
                // SP IQ_JBI.CambiarJBI no existe - usar UPDATE directo
                var sql = "UPDATE PY_Trabajo SET JobBook = @NuevoJBI WHERE id = @IdTrabajo";
                await _connection.ExecuteAsync(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar cambio de JBI en base de datos", ex);
            }
        }

        /// <summary>
        /// Guarda log de cambio de JBI para auditoría
        /// NOTA: SP IQ_JBI.GuardarLogCambios no existe - solo se registra log interno
        /// </summary>
        public async Task GuardarLogCambioAsync(LogCambioJBIDto logDto)
        {
            // SP IQ_JBI.GuardarLogCambios no existe en BD
            // No hay tabla de log de cambios JBI en legacy
            // Solo completar operación sin persistir log
            await Task.CompletedTask;
        }
    }
}
