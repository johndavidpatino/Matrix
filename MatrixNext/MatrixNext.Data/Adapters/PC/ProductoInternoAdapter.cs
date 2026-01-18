using Dapper;
using MatrixNext.Data.DTOs.PC;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MatrixNext.Data.Adapters.PC
{
    /// <summary>
    /// Adapter para acceso a datos de productos internos usando Dapper
    /// </summary>
    public class ProductoInternoAdapter : IProductoInternoAdapter
    {
        private readonly string _connectionString;

        public ProductoInternoAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' no encontrada");
        }

        public async Task<IEnumerable<ProductoInternoListDto>> ObtenerTodosAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            
            return await connection.QueryAsync<ProductoInternoListDto>(
                "CU_ProductoInterno_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ProductoInternoListDto>> ObtenerPorUnidadEnviaAsync(int unidadId, int? proyectoId = null)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", unidadId);
            if (proyectoId.HasValue)
                parameters.Add("@ProyectoId", proyectoId.Value); // NOTA: Parámetro correcto es @ProyectoId

            return await connection.QueryAsync<ProductoInternoListDto>(
                "CU_ProductoInterno_GetEnvia",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ProductoInternoListDto>> ObtenerPorUnidadRecibeAsync(int unidadId, int? proyectoId = null)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", unidadId);
            if (proyectoId.HasValue)
                parameters.Add("@ProyectoId", proyectoId.Value); // NOTA: Parámetro correcto es @ProyectoId

            return await connection.QueryAsync<ProductoInternoListDto>(
                "CU_ProductoInterno_GetRecibe",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<ProductoInternoListDto?> ObtenerPorIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var result = await connection.QueryAsync<ProductoInternoListDto>(
                "CU_ProductoInterno_Get",
                commandType: CommandType.StoredProcedure
            );

            return result.FirstOrDefault(x => x.Id == id);
        }

        public async Task<int> CrearAsync(ProductoInternoDto dto, int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@ProyectoId", dto.ProyectoId);
            parameters.Add("@FechaEnvio", dto.FechaEnvio ?? DateTime.Now);
            parameters.Add("@UnidadEnvia", dto.UnidadEnvia);
            parameters.Add("@UnidadRecibe", dto.UnidadRecibe);
            parameters.Add("@Tipo", dto.Tipo);
            parameters.Add("@Producto", dto.Producto);
            parameters.Add("@Descripcion", dto.Descripcion);
            parameters.Add("@Cantidad", dto.Cantidad);
            parameters.Add("@Envia", dto.Envia);
            parameters.Add("@Recibe", dto.Recibe);
            parameters.Add("@FechaRecepcion", dto.FechaRecepcion);
            parameters.Add("@Observaciones", dto.Observaciones);
            // NOTA: SP CU_ProductoInterno_Add NO tiene parámetro OUTPUT @Id

            await connection.ExecuteAsync(
                "CU_ProductoInterno_Add",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            // Obtener el ID insertado usando SCOPE_IDENTITY
            var newId = await connection.QuerySingleAsync<int>("SELECT CAST(SCOPE_IDENTITY() AS INT)");
            return newId;
        }

        public async Task<bool> ActualizarAsync(ProductoInternoDto dto, int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", dto.Id);
            parameters.Add("@ProyectoId", dto.ProyectoId);
            parameters.Add("@FechaEnvio", dto.FechaEnvio);
            parameters.Add("@UnidadEnvia", dto.UnidadEnvia);
            parameters.Add("@UnidadRecibe", dto.UnidadRecibe);
            parameters.Add("@Tipo", dto.Tipo);
            parameters.Add("@Producto", dto.Producto);
            parameters.Add("@Descripcion", dto.Descripcion);
            parameters.Add("@Cantidad", dto.Cantidad);
            parameters.Add("@Envia", dto.Envia);
            parameters.Add("@Recibe", dto.Recibe);
            parameters.Add("@FechaRecepcion", dto.FechaRecepcion);
            parameters.Add("@Observaciones", dto.Observaciones);

            var rowsAffected = await connection.ExecuteAsync(
                "CU_ProductoInterno_Edit",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> ActualizarCantidadAsync(int id, decimal cantidad, int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Cantidad", cantidad);

            var rowsAffected = await connection.ExecuteAsync(
                "CU_ProductoInterno_EditCant",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> RegistrarRecepcionAsync(int id, int recibeUsuarioId, DateTime fechaRecepcion, string? observaciones)
        {
            using var connection = new SqlConnection(_connectionString);
            
            // Obtener producto actual para actualizar solo recepción
            var producto = await ObtenerPorIdAsync(id);
            if (producto == null)
                return false;

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@ProyectoId", producto.ProyectoId);
            parameters.Add("@FechaEnvio", producto.FechaEnvio);
            parameters.Add("@UnidadEnvia", producto.UnidadEnvia);
            parameters.Add("@UnidadRecibe", producto.UnidadRecibe);
            parameters.Add("@Tipo", producto.Tipo);
            parameters.Add("@Producto", producto.Producto);
            parameters.Add("@Descripcion", producto.Descripcion);
            parameters.Add("@Cantidad", producto.Cantidad);
            parameters.Add("@Envia", producto.Envia);
            parameters.Add("@Recibe", recibeUsuarioId);
            parameters.Add("@FechaRecepcion", fechaRecepcion);
            parameters.Add("@Observaciones", observaciones ?? producto.Observaciones);

            var rowsAffected = await connection.ExecuteAsync(
                "CU_ProductoInterno_Edit",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            var rowsAffected = await connection.ExecuteAsync(
                "CU_ProductoInterno_Del",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }
    }
}
