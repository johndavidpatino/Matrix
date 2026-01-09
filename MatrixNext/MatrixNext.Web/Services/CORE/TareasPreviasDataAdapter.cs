using Dapper;
using Microsoft.Data.SqlClient;
using MatrixNext.Web.Models.CORE;

namespace MatrixNext.Web.Services.CORE
{
    /// <summary>
    /// Adapter para acceso a datos de TareasPrevias (precedencias)
    /// Dapper para lecturas, EF para escrituras con validación de ciclos
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
        /// Tabla: CORE_TareasPrevias
        /// Obtiene precedencias de una tarea específica
        /// </summary>
        public async Task<IEnumerable<TareaPrevia>> ObtenerPorTareaAsync(long idTarea)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.QueryAsync<TareaPrevia>(
                @"SELECT 
                      id AS Id,
                      TareaId AS IdTarea,
                      TareaPreviaId AS IdTareaPreviaRequerida,
                      1 AS Orden
                  FROM CORE_TareasPrevias
                  WHERE TareaId = @IdTarea",
                new { IdTarea = idTarea },
                commandType: System.Data.CommandType.Text
            );

            return result;
        }

        /// <summary>
        /// Lectura completa de todas las precedencias (para validación de ciclos)
        /// </summary>
        public async Task<IEnumerable<TareaPrevia>> ObtenerTodasAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            
            var result = await connection.QueryAsync<TareaPrevia>(
                @"SELECT 
                      id AS Id,
                      TareaId AS IdTarea,
                      TareaPreviaId AS IdTareaPreviaRequerida,
                      1 AS Orden
                  FROM CORE_TareasPrevias"
            );

            return result;
        }
    }
}
