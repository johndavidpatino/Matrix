using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Adapters.GD
{
    public class GdRepositorioAdapter : IGdRepositorioAdapter
    {
        private readonly string _connectionString;

        public GdRepositorioAdapter(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MatrixDb")!;
        }

        public async Task<IEnumerable<dynamic>> RepositorioDocumentosGetXTrabajo(int idTrabajo)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync(
                "GD_RepositorioDocumentos_GetXTrabajo",
                new { IdTrabajo = idTrabajo },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> RepositorioDocumentosAdd(object parameters)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "GD_RepositorioDocumentos_Add",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> EscanerDocumentosDel(int idContenedor)
        {
            using var connection = new SqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync(
                "GD_EscanerDocumentos_Del",
                new { IdContenedor = idContenedor },
                commandType: CommandType.StoredProcedure);
            return affected > 0;
        }
    }
}
