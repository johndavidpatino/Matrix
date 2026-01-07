using Dapper;
using Microsoft.Data.SqlClient;
using MatrixNext.Web.Models.CORE;

namespace MatrixNext.Web.Services.CORE
{
    /// <summary>
    /// Adapter para acceso a datos de TareasPrevias (precedencias)
    /// SP para lecturas, EF para escrituras con validación de ciclos
    /// </summary>
    public class TareasPreviasDataAdapter
    {
        private readonly string _connectionString;

        public TareasPreviasDataAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixDb") 
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// SP: CORE_WorkFlow_TareasPrevias_Get
        /// Obtiene precedencias de una tarea específica
        /// </summary>
        public async Task<IEnumerable<TareaPrevía>> ObtenerPorTareaAsync(long idTarea)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@IdTarea", idTarea);

            var result = await connection.QueryAsync<TareaPrevía>(
                "CORE_WorkFlow_TareasPrevias_Get",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );

            return result;
        }

        /// <summary>
        /// Lectura completa de todas las precedencias (para validación de ciclos)
        /// </summary>
        public async Task<IEnumerable<TareaPrevía>> ObtenerTodasAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            
            var result = await connection.QueryAsync<TareaPrevía>(
                "SELECT Id, IdTarea, IdTareaPreviaRequerida, Orden, FechaCreacion, FechaActualizacion FROM CORE_WorkFlow_TareasPrevias"
            );

            return result;
        }
    }
}
