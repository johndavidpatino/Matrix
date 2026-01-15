using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.Models.GD;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Adapters.GD
{
    public class GdCatalogosAdapter : IGdCatalogosAdapter
    {
        private readonly string _connectionString;

        public GdCatalogosAdapter(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MatrixDb")!;
        }

        public async Task<List<TipoSolicitudDto>> ObtenerTipoSolicitudes()
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.QueryAsync<TipoSolicitudDto>(
                "GD_TipoSolicitud_Get",
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<int> CrearTipoSolicitud(string nombre, string? descripcion)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new { Nombre = nombre, Descripcion = descripcion };
            var result = await connection.ExecuteScalarAsync<int>(
                "GD_TipoSolicitud_Add",
                parameters,
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<bool> ActualizarTipoSolicitud(int id, string nombre, string? descripcion)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new { id, Nombre = nombre, Descripcion = descripcion };
            var affected = await connection.ExecuteAsync(
                "GD_TipoSolicitud_Edit",
                parameters,
                commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> EliminarTipoSolicitud(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new { id };
            var affected = await connection.ExecuteAsync(
                "GD_TipoSolicitud_Del",
                parameters,
                commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<List<EstadoSolicitudDto>> ObtenerEstadosSolicitud()
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.QueryAsync<EstadoSolicitudDto>(
                "GD_Estados_Get",
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<int> CrearEstadoSolicitud(string nombre, string? descripcion)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new { Nombre = nombre, Descripcion = descripcion };
            var result = await connection.ExecuteScalarAsync<int>(
                "GD_EstadoSolicitud_Add",
                parameters,
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<bool> ActualizarEstadoSolicitud(int id, string nombre, string? descripcion)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new { id, Nombre = nombre, Descripcion = descripcion };
            var affected = await connection.ExecuteAsync(
                "GD_EstadoSolicitud_Edit",
                parameters,
                commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> EliminarEstadoSolicitud(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new { id };
            var affected = await connection.ExecuteAsync(
                "GD_EstadoSolicitud_Del",
                parameters,
                commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<List<ProcesoDto>> ObtenerProcesos()
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.QueryAsync<ProcesoDto>(
                "GD_Procesos_Get",
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<int> CrearProceso(string nombre, string? descripcion)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new { Nombre = nombre, Descripcion = descripcion };
            var result = await connection.ExecuteScalarAsync<int>(
                "GD_Procesos_Add",
                parameters,
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<bool> ActualizarProceso(int id, string nombre, string? descripcion)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new { id, Nombre = nombre, Descripcion = descripcion };
            var affected = await connection.ExecuteAsync(
                "GD_Procesos_Edit",
                parameters,
                commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> EliminarProceso(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new { id };
            var affected = await connection.ExecuteAsync(
                "GD_Procesos_Del",
                parameters,
                commandType: CommandType.StoredProcedure);
            return affected > 0;
        }
    }
}

