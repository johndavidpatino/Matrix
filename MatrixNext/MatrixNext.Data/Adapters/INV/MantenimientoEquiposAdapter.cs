using Dapper;
using MatrixNext.Data.DTOs.INV;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MatrixNext.Data.Adapters.INV
{
    public class MantenimientoEquiposAdapter : IMantenimientoEquiposAdapter
    {
        private readonly string _connectionString;

        public MantenimientoEquiposAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixConnection")
                ?? throw new InvalidOperationException("Connection string 'MatrixConnection' no encontrada");
        }

        public async Task<IEnumerable<MantenimientoEquipoDto>> ObtenerTodosAsync(
            long? id = null,
            long? idActivoFijo = null,
            long? idArticulo = null,
            int? tipoMantenimiento = null,
            long? idUsuarioResponsable = null)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@IdActivoFijo", idActivoFijo);
            parameters.Add("@IdArticulo", idArticulo);
            parameters.Add("@TipoMantenimiento", tipoMantenimiento);
            parameters.Add("@IdUsuarioResponsable", idUsuarioResponsable);
            parameters.Add("@UsuarioResponsable", null);

            var results = await connection.QueryAsync(
                "INV_MantenimientoEquipos_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return results.Select(r => new MantenimientoEquipoDto
            {
                Id = r.Id,
                IdActivoFijo = r.IdActivoFijo,
                Fecha = r.Fecha,
                TipoMantenimiento = r.TipoMantenimiento,
                IdUsuarioResponsable = r.IdUsuarioResponsable,
                Observaciones = r.Observaciones ?? string.Empty,
                UsuarioRegistra = r.UsuarioRegistra
            });
        }

        public async Task<IEnumerable<MantenimientoEquipoDto>> ObtenerPorActivoAsync(long idActivoFijo)
        {
            return await ObtenerTodosAsync(idActivoFijo: idActivoFijo);
        }

        public async Task<MantenimientoEquipoDto?> ObtenerPorIdAsync(long id)
        {
            var results = await ObtenerTodosAsync(id: id);
            return results.FirstOrDefault();
        }

        public async Task<long> CrearAsync(MantenimientoEquipoDto dto, long usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@IdActivoFijo", dto.IdActivoFijo);
            parameters.Add("@UsuarioRegistra", usuarioId);
            parameters.Add("@Fecha", dto.Fecha);
            parameters.Add("@TipoMantenimiento", dto.TipoMantenimiento);
            parameters.Add("@IdUsuarioResponsable", dto.IdUsuarioResponsable);
            parameters.Add("@Observaciones", dto.Observaciones);

            var id = await connection.ExecuteScalarAsync<decimal>(
                "INV_MantenimientoEquipos_Add",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return (long)id;
        }

        public async Task ActualizarAsync(MantenimientoEquipoDto dto, long usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", dto.Id);
            parameters.Add("@IdActivoFijo", dto.IdActivoFijo);
            parameters.Add("@UsuarioRegistra", usuarioId);
            parameters.Add("@Fecha", dto.Fecha);
            parameters.Add("@TipoMantenimiento", dto.TipoMantenimiento);
            parameters.Add("@IdUsuarioResponsable", dto.IdUsuarioResponsable);
            parameters.Add("@Observaciones", dto.Observaciones);

            await connection.ExecuteAsync(
                "INV_MantenimientoEquipos_Edit",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
