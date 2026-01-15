using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MatrixNext.Web.ViewModels.CORE;

namespace MatrixNext.Web.Services.CORE
{
    /// <summary>
    /// Adapter Dapper para la asignación de tareas a tipos de hilo (tabla CORE_TipoHilo_Tareas)
    /// Utiliza el SP legacy CORE_Configuracion_TareasXTipoHilo_Get para listados
    /// </summary>
    public class TareasPorTipoHiloDataAdapter
    {
        private const string SpListar = "CORE_Configuracion_TareasXTipoHilo_Get";
        private readonly string _connectionString;

        public TareasPorTipoHiloDataAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixDb")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<TareaPorTipoHiloVM>> ObtenerAsync(long tipoHiloId, bool? asignada = null)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@TipoHiloId", tipoHiloId, DbType.Int64);
            parameters.Add("@Asignada", asignada, DbType.Boolean);

            var result = await connection.QueryAsync<TareaPorTipoHiloVM>(
                SpListar,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<bool> AsignarAsync(long tipoHiloId, long tareaId)
        {
            const string sql = @"IF NOT EXISTS (SELECT 1 FROM CORE_TipoHilo_Tareas WHERE TipoHiloId = @TipoHiloId AND TareaId = @TareaId)
                                 INSERT INTO CORE_TipoHilo_Tareas (TipoHiloId, TareaId) VALUES (@TipoHiloId, @TareaId);";

            using var connection = new SqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync(sql, new { TipoHiloId = tipoHiloId, TareaId = tareaId });
            return affected > 0;
        }

        public async Task<bool> DesasignarAsync(long tipoHiloId, long tareaId)
        {
            const string sql = @"DELETE FROM CORE_TipoHilo_Tareas WHERE TipoHiloId = @TipoHiloId AND TareaId = @TareaId";

            using var connection = new SqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync(sql, new { TipoHiloId = tipoHiloId, TareaId = tareaId });
            return affected > 0;
        }
    }
}
