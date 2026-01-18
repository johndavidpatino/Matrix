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
    /// Adapter para acceso a datos de Diseño Muestral
    /// </summary>
    public class ESDisenoMuestralAdapter : IESDisenoMuestralAdapter
    {
        private readonly string _connectionString;

        public ESDisenoMuestralAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<ESDisenoMuestralOutputDto>> ObtenerTodosAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<ESDisenoMuestralOutputDto>(
                    "ES_DisenoMuestral_Get",
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }

        public async Task<IEnumerable<ESDisenoMuestralOutputDto>> ObtenerPorBriefAsync(long briefId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    SELECT d.*, b.Objetivo as BriefObjetivo, p.Titulo as PropuestaNombre
                    FROM ES_DisenoMuestral d
                    INNER JOIN ES_BriefDisenoMuestral b ON d.BriefId = b.Id
                    LEFT JOIN CU_Propuestas p ON b.PropuestaId = p.Id
                    WHERE d.BriefId = @BriefId
                    ORDER BY d.NoVersion DESC";

                var result = await connection.QueryAsync<ESDisenoMuestralOutputDto>(
                    query,
                    new { BriefId = briefId }
                );

                return result;
            }
        }

        public async Task<ESDisenoMuestralOutputDto> ObtenerPorIdAsync(long id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    SELECT d.*, b.Objetivo as BriefObjetivo, p.Titulo as PropuestaNombre
                    FROM ES_DisenoMuestral d
                    INNER JOIN ES_BriefDisenoMuestral b ON d.BriefId = b.Id
                    LEFT JOIN CU_Propuestas p ON b.PropuestaId = p.Id
                    WHERE d.Id = @Id";

                var result = await connection.QueryFirstOrDefaultAsync<ESDisenoMuestralOutputDto>(
                    query,
                    new { Id = id }
                );

                return result;
            }
        }

        public async Task<long> CrearAsync(ESDisenoMuestralInputDto dto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Obtener siguiente versión
                var version = await ObtenerSiguienteVersionAsync(connection, dto.BriefId);

                var parameters = new DynamicParameters();
                parameters.Add("@BriefId", dto.BriefId);
                parameters.Add("@Fecha", DateTime.Now);
                parameters.Add("@MuestroProbabilistico", dto.MuestroProbabilistico);
                
                // Campos de selección
                parameters.Add("@Objetivo", dto.Objetivo);
                parameters.Add("@Poblacion", dto.Poblacion);
                parameters.Add("@Mercado", dto.Mercado);
                parameters.Add("@Marco", dto.Marco);
                parameters.Add("@Tecnica", dto.Tecnica);
                parameters.Add("@Diseno", dto.Diseno);
                parameters.Add("@Tamano", dto.Tamano);
                parameters.Add("@Fiabilidad", dto.Fiabilidad);
                parameters.Add("@Desagregacion", dto.Desagregacion);
                parameters.Add("@Fuente", dto.Fuente);
                parameters.Add("@Ponderacion", dto.Ponderacion);
                parameters.Add("@Variable", dto.Variable);

                // Campos de texto
                parameters.Add("@ObjetivoT", dto.ObjetivoT);
                parameters.Add("@PoblacionT", dto.PoblacionT);
                parameters.Add("@MercadoT", dto.MercadoT);
                parameters.Add("@MarcoT", dto.MarcoT);
                parameters.Add("@TecnicaT", dto.TecnicaT);
                parameters.Add("@DisenoT", dto.DisenoT);
                parameters.Add("@TamanoT", dto.TamanoT);
                parameters.Add("@FiabilidadT", dto.FiabilidadT);
                parameters.Add("@DesagregacionT", dto.DesagregacionT);
                parameters.Add("@FuenteT", dto.FuenteT);
                parameters.Add("@PonderacionT", dto.PonderacionT);
                parameters.Add("@VariableT", dto.VariableT);
                parameters.Add("@Observaciones", dto.Observaciones);
                parameters.Add("@ObservacionesT", dto.ObservacionesT);
                parameters.Add("@NoVersion", version);

                var id = await connection.ExecuteScalarAsync<long>(
                    "ES_DisenoMuestral_Add",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return id;
            }
        }

        public async Task ActualizarAsync(long id, ESDisenoMuestralInputDto dto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                parameters.Add("@BriefId", dto.BriefId);
                parameters.Add("@Fecha", DateTime.Now);
                parameters.Add("@MuestroProbabilistico", dto.MuestroProbabilistico);
                
                // Campos de selección
                parameters.Add("@Objetivo", dto.Objetivo);
                parameters.Add("@Poblacion", dto.Poblacion);
                parameters.Add("@Mercado", dto.Mercado);
                parameters.Add("@Marco", dto.Marco);
                parameters.Add("@Tecnica", dto.Tecnica);
                parameters.Add("@Diseno", dto.Diseno);
                parameters.Add("@Tamano", dto.Tamano);
                parameters.Add("@Fiabilidad", dto.Fiabilidad);
                parameters.Add("@Desagregacion", dto.Desagregacion);
                parameters.Add("@Fuente", dto.Fuente);
                parameters.Add("@Ponderacion", dto.Ponderacion);
                parameters.Add("@Variable", dto.Variable);

                // Campos de texto
                parameters.Add("@ObjetivoT", dto.ObjetivoT);
                parameters.Add("@PoblacionT", dto.PoblacionT);
                parameters.Add("@MercadoT", dto.MercadoT);
                parameters.Add("@MarcoT", dto.MarcoT);
                parameters.Add("@TecnicaT", dto.TecnicaT);
                parameters.Add("@DisenoT", dto.DisenoT);
                parameters.Add("@TamanoT", dto.TamanoT);
                parameters.Add("@FiabilidadT", dto.FiabilidadT);
                parameters.Add("@DesagregacionT", dto.DesagregacionT);
                parameters.Add("@FuenteT", dto.FuenteT);
                parameters.Add("@PonderacionT", dto.PonderacionT);
                parameters.Add("@VariableT", dto.VariableT);
                parameters.Add("@Observaciones", dto.Observaciones);
                parameters.Add("@ObservacionesT", dto.ObservacionesT);
                parameters.Add("@NoVersion", 1);

                await connection.ExecuteAsync(
                    "ES_DisenoMuestral_Edit",
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
                    "ES_DisenoMuestral_Del",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        private async Task<int> ObtenerSiguienteVersionAsync(SqlConnection connection, long briefId)
        {
            var query = "SELECT ISNULL(MAX(NoVersion), 0) + 1 FROM ES_DisenoMuestral WHERE BriefId = @BriefId";
            var version = await connection.ExecuteScalarAsync<int>(query, new { BriefId = briefId });
            return version;
        }
    }
}
