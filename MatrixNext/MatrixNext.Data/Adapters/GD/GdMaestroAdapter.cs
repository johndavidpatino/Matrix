using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Adapters.GD
{
    public class GdMaestroAdapter : IGdMaestroAdapter
    {
        private readonly string _connectionString;

        public GdMaestroAdapter(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MatrixDb")!;
        }

        public async Task<IEnumerable<dynamic>> MaestroDocumentosGet()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync(
                "GD_MaestroDocumentos_Get",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> MaestroDocumentosAdd(object parameters)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "GD_MaestroDocumentos_Add2",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DocumentosControladosAdd(object parameters)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "GD_DocumentosControlados_Add",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> DocumentosMaestrosUpdate(object parameters)
        {
            using var connection = new SqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync(
                "GD_DocumentosMaestros_Update",
                parameters,
                commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DocumentosControladosActivo(int documentoId, bool activo)
        {
            using var connection = new SqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync(
                "GD_DocumentosControlados_Activo",
                new { DocumentoId = documentoId, Activo = activo },
                commandType: CommandType.StoredProcedure);
            return affected > 0;
        }
    }
}
