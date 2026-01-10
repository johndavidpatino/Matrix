using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Adapters.GD
{
    public class GdSolicitudesAdapter : IGdSolicitudesAdapter
    {
        private readonly string _connectionString;

        public GdSolicitudesAdapter(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MatrixDb")!;
        }

        public async Task<int> SolDocumentosAdd(object parameters)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "GD_SolDocumentos_Add",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> RevisionesAdd(object parameters)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "GD_Revisiones_Add",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<dynamic>> UsuariosGet(object? parameters = null)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync(
                "GD_US_Usuarios_Get",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}
