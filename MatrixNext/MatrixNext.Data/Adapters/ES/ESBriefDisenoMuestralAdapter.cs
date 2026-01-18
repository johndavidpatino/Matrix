using Dapper;
using MatrixNext.Data.DTOs.ES;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Data.Adapters.ES
{
    /// <summary>
    /// Adapter para acceso a datos de Brief Diseño Muestral
    /// Utiliza Dapper para ejecutar stored procedures
    /// </summary>
    public class ESBriefDisenoMuestralAdapter : IESBriefDisenoMuestralAdapter
    {
        private readonly string _connectionString;

        public ESBriefDisenoMuestralAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<ESBriefDisenoMuestralOutputDto>> ObtenerTodosAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<ESBriefDisenoMuestralOutputDto>(
                    "ES_BriefDisenoMuestral_Get",
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }

        public async Task<IEnumerable<ESBriefDisenoMuestralOutputDto>> ObtenerPorPropuestaAsync(long propuestaId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PropuestaId", propuestaId);

                var result = await connection.QueryAsync<ESBriefDisenoMuestralOutputDto>(
                    "ES_BriefDisenoMuestral_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.Where(b => b.PropuestaId == propuestaId);
            }
        }

        public async Task<IEnumerable<ESBriefDisenoMuestralOutputDto>> ObtenerPendientesAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // NOTA: CU_Cuentas no existe y CU_Propuestas no tiene CuentaId
                // Se simplifica query sin JOIN a cliente
                var query = @"
                    SELECT b.Id, b.PropuestaId, b.Fecha, b.Objetivo, b.Poblacion, b.Capacidad, 
                           b.Metodologia, b.NivelesDesagregacion, b.PosiblesMarcos, b.Variable, 
                           b.Observaciones, b.NoVersion,
                           p.Titulo as PropuestaNombre, '' as ClienteNombre
                    FROM ES_BriefDisenoMuestral b
                    INNER JOIN CU_Propuestas p ON b.PropuestaId = p.Id
                    WHERE NOT EXISTS (
                        SELECT 1 FROM ES_DisenoMuestral d WHERE d.BriefId = b.Id
                    )
                    ORDER BY b.Fecha DESC";

                var result = await connection.QueryAsync<ESBriefDisenoMuestralOutputDto>(query);

                return result;
            }
        }

        public async Task<ESBriefDisenoMuestralOutputDto> ObtenerPorIdAsync(long id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // NOTA: CU_Cuentas no existe - simplificado sin JOIN a cliente
                var query = @"
                    SELECT b.Id, b.PropuestaId, b.Fecha, b.Objetivo, b.Poblacion, b.Capacidad, 
                           b.Metodologia, b.NivelesDesagregacion, b.PosiblesMarcos, b.Variable, 
                           b.Observaciones, b.NoVersion,
                           p.Titulo as PropuestaNombre, '' as ClienteNombre
                    FROM ES_BriefDisenoMuestral b
                    LEFT JOIN CU_Propuestas p ON b.PropuestaId = p.Id
                    WHERE b.Id = @Id";

                var result = await connection.QueryFirstOrDefaultAsync<ESBriefDisenoMuestralOutputDto>(
                    query,
                    new { Id = id }
                );

                return result;
            }
        }

        public async Task<long> CrearAsync(ESBriefDisenoMuestralInputDto dto, long usuarioId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Obtener última versión
                var version = await ObtenerSiguienteVersionAsync(connection, dto.PropuestaId);

                var parameters = new DynamicParameters();
                parameters.Add("@PropuestaId", dto.PropuestaId);
                parameters.Add("@Fecha", DateTime.Now);
                parameters.Add("@Objetivo", dto.Objetivo);
                parameters.Add("@Poblacion", dto.Poblacion);
                parameters.Add("@Capacidad", dto.Capacidad);
                parameters.Add("@Metodologia", dto.Metodologia);
                parameters.Add("@NivelesDesagregacion", dto.NivelesDesagregacion);
                parameters.Add("@PosiblesMarcos", dto.PosiblesMarcos);
                parameters.Add("@Variable", dto.Variable);
                parameters.Add("@Observaciones", dto.Observaciones);
                parameters.Add("@NoVersion", version);

                var id = await connection.ExecuteScalarAsync<long>(
                    "ES_BriefDisenoMuestral_Add",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return id;
            }
        }

        public async Task ActualizarAsync(long id, ESBriefDisenoMuestralInputDto dto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                parameters.Add("@PropuestaId", dto.PropuestaId);
                parameters.Add("@Fecha", DateTime.Now);
                parameters.Add("@Objetivo", dto.Objetivo);
                parameters.Add("@Poblacion", dto.Poblacion);
                parameters.Add("@Capacidad", dto.Capacidad);
                parameters.Add("@Metodologia", dto.Metodologia);
                parameters.Add("@NivelesDesagregacion", dto.NivelesDesagregacion);
                parameters.Add("@PosiblesMarcos", dto.PosiblesMarcos);
                parameters.Add("@Variable", dto.Variable);
                parameters.Add("@Observaciones", dto.Observaciones);
                parameters.Add("@NoVersion", 1); // Mantener versión actual

                await connection.ExecuteAsync(
                    "ES_BriefDisenoMuestral_Edit",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task EliminarAsync(long id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                await connection.ExecuteAsync(
                    "ES_BriefDisenoMuestral_Del",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        private async Task<int> ObtenerSiguienteVersionAsync(SqlConnection connection, long propuestaId)
        {
            var query = "SELECT ISNULL(MAX(NoVersion), 0) + 1 FROM ES_BriefDisenoMuestral WHERE PropuestaId = @PropuestaId";
            var version = await connection.ExecuteScalarAsync<int>(query, new { PropuestaId = propuestaId });
            return version;
        }
    }
}
