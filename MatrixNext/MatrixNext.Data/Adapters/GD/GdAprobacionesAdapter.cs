using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Adapters.GD
{
    public class GdAprobacionesAdapter : IGdAprobacionesAdapter
    {
        private readonly string _connectionString;

        public GdAprobacionesAdapter(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MatrixDb")!;
        }

        public async Task<IEnumerable<dynamic>> RevisionesGetRev(object parameters)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync(
                "GD_Revisiones_GetRev",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> RevisionesEdit(object parameters)
        {
            using var connection = new SqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync(
                "GD_Revisiones_Edit",
                parameters,
                commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        // SolicitudDocumentos_Update not confirmed in CO_Matrix_SP_Names; pending mapping.
    }
}
