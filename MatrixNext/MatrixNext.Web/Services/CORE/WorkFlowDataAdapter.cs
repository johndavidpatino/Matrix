using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MatrixNext.Web.Models.CORE;

namespace MatrixNext.Web.Services.CORE
{
    /// <summary>
    /// Adapter para acceso a datos de WorkFlow
    /// Patrón: SP para lecturas complejas, EF para escrituras (via service)
    /// </summary>
    public class WorkFlowDataAdapter
    {
        private readonly string _connectionString;

        public WorkFlowDataAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixDb") 
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// SP: CORE_WorkFlow_GetXTrabajoXTarea
        /// Obtiene workflow por IdTrabajo e IdTarea
        /// </summary>
        public async Task<WorkFlow?> ObtenerPorTrabajoYTareaAsync(long idTrabajo, long idTarea)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", idTrabajo);
            parameters.Add("@IdTarea", idTarea);

            var result = await connection.QueryFirstOrDefaultAsync<WorkFlow>(
                "CORE_WorkFlow_GetXTrabajoXTarea",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );

            return result;
        }

        /// <summary>
        /// SP: CORE_WorkFlow_Get (lectura paginada o filtrada)
        /// </summary>
        public async Task<IEnumerable<WorkFlow>> ObtenerListaAsync(
            long? idTrabajo = null, 
            long? idTarea = null, 
            int? estado = null)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            if (idTrabajo.HasValue) parameters.Add("@IdTrabajo", idTrabajo.Value);
            if (idTarea.HasValue) parameters.Add("@IdTarea", idTarea.Value);
            if (estado.HasValue) parameters.Add("@Estado", estado.Value);

            // TODO: validar nombre exacto del SP en BD real
            var result = await connection.QueryAsync<WorkFlow>(
                "CORE_WorkFlow_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        /// <summary>
        /// Invoca el SP legacy que crea el hilo y genera las tareas CORE iniciales para un trabajo.
        /// Ref: WorkFlow.CrearHiloCrearTareas() en WebForms legacy.
        /// </summary>
        public async Task<bool> CrearHiloCrearTareasAsync(long idTrabajo, long idProyecto)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", idTrabajo);
            parameters.Add("@IdProyecto", idProyecto);

            var affected = await connection.ExecuteAsync(
                "CORE_WorkFlow_CrearHiloCrearTareas",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            // Algunos SP devuelven 0 filas afectadas aunque se ejecuten OK; devolvemos true si no hubo excepción.
            return affected >= 0;
        }

        /// <summary>
        /// Registra en auditoría legacy la creación masiva en estado 'Creada'.
        /// Ref: CORE_Log_WorkFlow_MasivoEstadoCreada_Add()
        /// </summary>
        public async Task RegistrarLogCreacionAsync(long idTrabajo)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", idTrabajo);

            await connection.ExecuteAsync(
                "CORE_Log_WorkFlow_MasivoEstadoCreada_Add",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
