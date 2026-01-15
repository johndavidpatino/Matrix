using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MatrixNext.Web.ViewModels.CORE;

namespace MatrixNext.Web.Services.CORE
{
    /// <summary>
    /// Adapter Dapper para documentos requeridos por tarea (CORE_Tareas_Documentos)
    /// Usa SP legacy CORE_Configuracion_DocumentosXTarea_Get para listados
    /// </summary>
    public class TareasDocumentosDataAdapter
    {
        private const string SpListar = "CORE_Configuracion_DocumentosXTarea_Get";
        private readonly string _connectionString;

        public TareasDocumentosDataAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixDb")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<DocumentoPorTareaVM>> ObtenerAsync(long tareaId, short tipoDocumentoTareaId, bool? asignado = null)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@TareaId", tareaId, DbType.Int64);
            parameters.Add("@TipoDocumentoTareaId", tipoDocumentoTareaId, DbType.Int16);
            parameters.Add("@Asignado", asignado, DbType.Boolean);

            var result = await connection.QueryAsync<DocumentoPorTareaVM>(
                SpListar,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<bool> AsignarAsync(long tareaId, long documentoId, short tipoDocumentoTareaId, bool esOpcional)
        {
            const string sql = @"IF NOT EXISTS (SELECT 1 FROM CORE_Tareas_Documentos WHERE TareaId = @TareaId AND DocumentoId = @DocumentoId AND TipoDocumentoTareaId = @TipoDocumentoTareaId)
                                 INSERT INTO CORE_Tareas_Documentos (TareaId, DocumentoId, TipoDocumentoTareaId, EsOpcional)
                                 VALUES (@TareaId, @DocumentoId, @TipoDocumentoTareaId, @EsOpcional);";

            using var connection = new SqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync(sql, new
            {
                TareaId = tareaId,
                DocumentoId = documentoId,
                TipoDocumentoTareaId = tipoDocumentoTareaId,
                EsOpcional = esOpcional
            });
            return affected > 0;
        }

        public async Task<bool> DesasignarAsync(long tareaId, long documentoId, short tipoDocumentoTareaId)
        {
            const string sql = @"DELETE FROM CORE_Tareas_Documentos WHERE TareaId = @TareaId AND DocumentoId = @DocumentoId AND TipoDocumentoTareaId = @TipoDocumentoTareaId";

            using var connection = new SqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync(sql, new
            {
                TareaId = tareaId,
                DocumentoId = documentoId,
                TipoDocumentoTareaId = tipoDocumentoTareaId
            });
            return affected > 0;
        }
    }
}
