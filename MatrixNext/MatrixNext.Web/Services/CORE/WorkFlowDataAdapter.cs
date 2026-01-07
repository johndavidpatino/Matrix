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
        private const string _spGetPorTrabajoYTarea = "CORE_WorkFlow_GetXTrabajoXTarea";
        private const string _spGetLista = "CORE_WorkFlow_Get";
        private const string _spCrearHilo = "CORE_WorkFlow_CrearHiloCrearTareas";
        private const string _spRegistrarLogCreacion = "CORE_Log_WorkFlow_MasivoEstadoCreada_Add";

        public WorkFlowDataAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixDb") 
                ?? throw new ArgumentNullException(nameof(configuration));

            // SP por defecto según migración de CoreProject; se valida su existencia en BD
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

            await EnsureStoredProcedureExistsAsync(connection, _spGetPorTrabajoYTarea);
            var result = await connection.QueryFirstOrDefaultAsync<WorkFlow>(
                _spGetPorTrabajoYTarea,
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

            await EnsureStoredProcedureExistsAsync(connection, _spGetLista);
            var result = await connection.QueryAsync<WorkFlow>(
                _spGetLista,
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

            await EnsureStoredProcedureExistsAsync(connection, _spCrearHilo);
            var affected = await connection.ExecuteAsync(
                _spCrearHilo,
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

            await EnsureStoredProcedureExistsAsync(connection, _spRegistrarLogCreacion);
            await connection.ExecuteAsync(
                _spRegistrarLogCreacion,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        private static async Task EnsureStoredProcedureExistsAsync(SqlConnection connection, string spName)
        {
            // Verifica existencia del SP para evitar errores crípticos en runtime
            var exists = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM sys.objects WHERE type = 'P' AND name = @name",
                new { name = spName });

            if (exists == 0)
            {
                throw new InvalidOperationException($"Stored Procedure no encontrado: {spName}. Configure 'StoredProcedures:{spName}' si el nombre difiere en su entorno.");
            }
        }
    }
}
