using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Adapters.PY
{
    public class PyDistribucionEntrevistasAdapter : IPyDistribucionEntrevistasAdapter
    {
        private readonly string _connectionString;
        private readonly MatrixDbContext _context;

        public PyDistribucionEntrevistasAdapter(IConfiguration config, MatrixDbContext context)
        {
            _connectionString = config.GetConnectionString("MatrixDb")!;
            _context = context;
        }

        /// <summary>
        /// Obtiene entrevistas por trabajo usando SP
        /// SP: OP_MuestraTrabajosCuali_EntrevistasGet(@Id BIGINT, @TrabajoId BIGINT)
        /// </summary>
        public async Task<List<EntrevistaCualiDto>> ObtenerEntrevistasPorTrabajo(long trabajoId)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@Id", null);
            parametros.Add("@TrabajoId", trabajoId);

            var resultado = await connection.QueryAsync<EntrevistaCualiDto>(
                "OP_MuestraTrabajosCuali_EntrevistasGet",
                parametros,
                commandType: CommandType.StoredProcedure);

            return resultado.ToList();
        }

        /// <summary>
        /// Obtiene entrevista por ID usando SP
        /// </summary>
        public async Task<EntrevistaCualiDto?> ObtenerEntrevistaPorId(long id)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@Id", id);
            parametros.Add("@TrabajoId", null);

            var resultado = await connection.QueryFirstOrDefaultAsync<EntrevistaCualiDto>(
                "OP_MuestraTrabajosCuali_EntrevistasGet",
                parametros,
                commandType: CommandType.StoredProcedure);

            return resultado;
        }

        /// <summary>
        /// Obtiene distribuciones por entrevista usando SP
        /// SP: OP_EntrevistasCuali_DistribucionGet(@Id INT, @IdEntrevista BIGINT, @IdTrabajo BIGINT)
        /// </summary>
        public async Task<List<DistribucionEntrevistaDto>> ObtenerDistribucionesPorEntrevista(long entrevistaId)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@Id", null);
            parametros.Add("@IdEntrevista", entrevistaId);
            parametros.Add("@IdTrabajo", null);

            var resultado = await connection.QueryAsync<DistribucionEntrevistaDto>(
                "OP_EntrevistasCuali_DistribucionGet",
                parametros,
                commandType: CommandType.StoredProcedure);

            return resultado.ToList();
        }

        /// <summary>
        /// Obtiene distribución por ID usando SP
        /// </summary>
        public async Task<DistribucionEntrevistaDto?> ObtenerDistribucionPorId(long id)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@Id", id);
            parametros.Add("@IdEntrevista", null);
            parametros.Add("@IdTrabajo", null);

            var resultado = await connection.QueryFirstOrDefaultAsync<DistribucionEntrevistaDto>(
                "OP_EntrevistasCuali_DistribucionGet",
                parametros,
                commandType: CommandType.StoredProcedure);

            return resultado;
        }

        /// <summary>
        /// Obtiene moderadores cualitativos usando SP
        /// SP: US_UsuariosModeradoresCualitativos
        /// </summary>
        public async Task<List<ModeradorCualiDto>> ObtenerModeradores()
        {
            using var connection = new SqlConnection(_connectionString);
            var resultado = await connection.QueryAsync<ModeradorCualiDto>(
                "US_UsuariosModeradoresCualitativos",
                commandType: CommandType.StoredProcedure);

            return resultado.ToList();
        }

        /// <summary>
        /// Obtiene log de entrevistas usando SP
        /// SP: OP_LogEntrevistasCuali_Get(@IdDistribucion BIGINT)
        /// </summary>
        public async Task<List<LogEntrevistaCualiDto>> ObtenerLogEntrevistas(long distribucionId)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@IdDistribucion", distribucionId);

            var resultado = await connection.QueryAsync<LogEntrevistaCualiDto>(
                "OP_LogEntrevistasCuali_Get",
                parametros,
                commandType: CommandType.StoredProcedure);

            return resultado.ToList();
        }

        /// <summary>
        /// Guarda distribución de entrevista usando SP
        /// SP: OP_EntrevistasCuali_Distribucion_Add
        /// Legacy confirmado: oMatrixContext.OP_EntrevistasCuali_Distribucion_Add(...)
        /// </summary>
        public async Task<long> GuardarDistribucion(DistribucionEntrevistaInputDto input)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@Cantidad", input.Cantidad);
            parametros.Add("@IdEntrevista", input.IdEntrevista);
            parametros.Add("@TrabajoId", input.TrabajoId);
            parametros.Add("@GrupoObjetivo", input.GrupoObjetivo);
            parametros.Add("@CiudadId", input.CiudadId);
            parametros.Add("@FechaInicio", input.FechaInicio);
            parametros.Add("@FechaFin", input.FechaFin);
            parametros.Add("@Moderador", input.Moderador);
            parametros.Add("@Usuario", input.UsuarioId.ToString());
            parametros.Add("@IdDistribucion", dbType: DbType.Int64, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "OP_EntrevistasCuali_Distribucion_Add",
                parametros,
                commandType: CommandType.StoredProcedure);

            return parametros.Get<long>("@IdDistribucion");
        }

        /// <summary>
        /// Actualiza estado de distribución usando EF Core
        /// </summary>
        public async Task ActualizarEstadoDistribucion(long distribucionId, short estado)
        {
            // TODO: Registrar OP_EntrevistasCuali_Distribucion en MatrixDbContext
            await Task.CompletedTask;
            /*
            var entidad = await _context.Set<Entities.OP_EntrevistasCuali_Distribucion>()
                .FirstOrDefaultAsync(x => x.Id == distribucionId);

            if (entidad != null)
            {
                entidad.IdEstado = estado;
                await _context.SaveChangesAsync();
            }
            */
        }

        /// <summary>
        /// Guarda log de entrevista usando EF Core
        /// </summary>
        public async Task GuardarLogEntrevista(long distribucionId, long entrevistaId, string usuario, short estado, string observacion)
        {
            // TODO: Registrar OP_LogEntrevistasCuali en MatrixDbContext
            await Task.CompletedTask;
            /*
            var log = new Entities.OP_LogEntrevistasCuali
            {
                IdDistribucion = distribucionId,
                IdEntrevista = entrevistaId,
                Fecha = DateTime.Now,
                Usuario = usuario,
                Estado = estado.ToString(),
                Observacion = observacion
            };

            _context.Set<Entities.OP_LogEntrevistasCuali>().Add(log);
            await _context.SaveChangesAsync();
            */
        }
    }
}
