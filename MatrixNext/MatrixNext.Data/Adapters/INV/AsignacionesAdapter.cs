using Dapper;
using MatrixNext.Data.DTOs.INV;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MatrixNext.Data.Adapters.INV
{
    public class AsignacionesAdapter : IAsignacionesAdapter
    {
        private readonly string _connectionString;

        public AsignacionesAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixConnection")
                ?? throw new InvalidOperationException("Connection string 'MatrixConnection' no encontrada");
        }

        public async Task<IEnumerable<AsignacionListDto>> ObtenerTodosAsync(
            long? idActivoFijo = null,
            long? idArticulo = null,
            int? idBU = null,
            string? jobBookCodigo = null,
            long? idUsuarioAsignado = null,
            bool? asignado = null)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@IdActivoFijo", idActivoFijo);
            parameters.Add("@IdArticulo", idArticulo);
            parameters.Add("@IdBU", idBU);
            parameters.Add("@JobBookCodigo", jobBookCodigo);
            parameters.Add("@JobBookNombre", null);
            parameters.Add("@IdCiudad", null);
            parameters.Add("@IdEstadoTablet", null);
            parameters.Add("@IdUsuarioAsignado", idUsuarioAsignado);
            parameters.Add("@UsuarioAsignado", null);
            parameters.Add("@TipoCargo", null);
            parameters.Add("@Asignado", asignado);

            var results = await connection.QueryAsync(
                "INV_Asignaciones_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return results.Select(r => new AsignacionListDto
            {
                Id = r.Id,
                IdActivoFijo = r.IdActivoFijo,
                Articulo = r.Articulo ?? string.Empty,
                Marca = r.Marca,
                Modelo = r.Modelo,
                Serial = r.Serial,
                FechaAsignacion = r.FechaAsignacion,
                JobBookCodigo = r.JobBookCodigo,
                JobBookNombre = r.JobBookNombre,
                Ciudad = r.Ciudad,
                EstadoTablet = r.EstadoTablet,
                IdUsuarioAsignado = r.IdUsuarioAsignado,
                UsuarioAsignado = r.UsuarioAsignado ?? string.Empty,
                Cargo = r.Cargo,
                Observacion = r.Observacion,
                Sede = r.Sede,
                GrupoUnidad = r.GrupoUnidad,
                Unidad = r.Unidad,
                UsuarioRegistra = r.UsuarioRegistra,
                UsuarioRegistraNombre = r.UsuarioRegistraNombre,
                FechaRegistro = r.FechaRegistro
            });
        }

        public async Task<AsignacionActivoDto?> ObtenerPorIdAsync(long idActivoFijo)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@IdActivoFijo", idActivoFijo);
            parameters.Add("@IdArticulo", null);
            parameters.Add("@IdBU", null);
            parameters.Add("@JobBookCodigo", null);
            parameters.Add("@JobBookNombre", null);
            parameters.Add("@IdCiudad", null);
            parameters.Add("@IdEstadoTablet", null);
            parameters.Add("@IdUsuarioAsignado", null);
            parameters.Add("@UsuarioAsignado", null);
            parameters.Add("@TipoCargo", null);
            parameters.Add("@Asignado", null);

            var result = await connection.QueryFirstOrDefaultAsync(
                "INV_Asignaciones_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (result == null) return null;

            return new AsignacionActivoDto
            {
                Id = result.Id,
                IdActivoFijo = result.IdActivoFijo,
                FechaAsignacion = result.FechaAsignacion,
                IdCentroCosto = result.IdCentroCosto,
                IdBU = result.IdBU,
                IdTrabajo = result.IdTrabajo,
                JobBookCodigo = result.JobBookCodigo,
                JobBookNombre = result.JobBookNombre,
                IdCiudad = result.IdCiudad,
                IdEstadoTablet = result.IdEstadoTablet,
                IdUsuarioAsignado = result.IdUsuarioAsignado,
                TipoCargo = result.TipoCargo,
                Cargo = result.Cargo,
                Observacion = result.Observacion,
                IdSede = result.IdSede,
                TipoGrupoUnidad = result.TipoGrupoUnidad,
                IdGrupoUnidad = result.IdGrupoUnidad,
                IdUnidad = result.IdUnidad
            };
        }

        public async Task<long> CrearAsync(AsignacionActivoDto dto, long usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@IdActivoFijo", dto.IdActivoFijo);
            parameters.Add("@UsuarioRegistra", usuarioId);
            parameters.Add("@FechaAsignacion", dto.FechaAsignacion);
            parameters.Add("@IdCentroCosto", dto.IdCentroCosto);
            parameters.Add("@IdBU", dto.IdBU);
            parameters.Add("@IdTrabajo", dto.IdTrabajo);
            parameters.Add("@JobBookCodigo", dto.JobBookCodigo);
            parameters.Add("@JobBookNombre", dto.JobBookNombre);
            parameters.Add("@IdCiudad", dto.IdCiudad);
            parameters.Add("@IdEstadoTablet", dto.IdEstadoTablet);
            parameters.Add("@IdUsuarioAsignado", dto.IdUsuarioAsignado);
            parameters.Add("@TipoCargo", dto.TipoCargo);
            parameters.Add("@Cargo", dto.Cargo);
            parameters.Add("@Observacion", dto.Observacion);
            parameters.Add("@IdSede", dto.IdSede);
            parameters.Add("@TipoGrupoUnidad", dto.TipoGrupoUnidad);
            parameters.Add("@IdGrupoUnidad", dto.IdGrupoUnidad);
            parameters.Add("@IdUnidad", dto.IdUnidad);

            var id = await connection.ExecuteScalarAsync<decimal>(
                "INV_Asignaciones_Add",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return (long)id;
        }

        public async Task ActualizarAsync(AsignacionActivoDto dto, long usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", dto.Id);
            parameters.Add("@IdActivoFijo", dto.IdActivoFijo);
            parameters.Add("@UsuarioRegistra", usuarioId);
            parameters.Add("@FechaAsignacion", dto.FechaAsignacion);
            parameters.Add("@IdCentroCosto", dto.IdCentroCosto);
            parameters.Add("@IdBU", dto.IdBU);
            parameters.Add("@IdTrabajo", dto.IdTrabajo);
            parameters.Add("@JobBookCodigo", dto.JobBookCodigo);
            parameters.Add("@JobBookNombre", dto.JobBookNombre);
            parameters.Add("@IdCiudad", dto.IdCiudad);
            parameters.Add("@IdEstadoTablet", dto.IdEstadoTablet);
            parameters.Add("@IdUsuarioAsignado", dto.IdUsuarioAsignado);
            parameters.Add("@TipoCargo", dto.TipoCargo);
            parameters.Add("@Cargo", dto.Cargo);
            parameters.Add("@Observacion", dto.Observacion);
            parameters.Add("@IdSede", dto.IdSede);
            parameters.Add("@TipoGrupoUnidad", dto.TipoGrupoUnidad);
            parameters.Add("@IdGrupoUnidad", dto.IdGrupoUnidad);
            parameters.Add("@IdUnidad", dto.IdUnidad);

            await connection.ExecuteAsync(
                "INV_Asignaciones_Edit",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> EliminarAsync(long idActivoFijo)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@IdActivoFijo", idActivoFijo);

            return await connection.ExecuteAsync(
                "INV_Asignaciones_Del",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<long> CrearLogAsync(long idActivoFijo, long idArticulo, long idUsuario, bool asignado)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@IdActivoFijo", idActivoFijo);
            parameters.Add("@IdArticulo", idArticulo);
            parameters.Add("@IdUsuario", idUsuario);
            parameters.Add("@IdCentroCosto", null);
            parameters.Add("@IdBU", null);
            parameters.Add("@IdTrabajo", null);
            parameters.Add("@JobBookCodigo", null);
            parameters.Add("@JobBookNombre", null);
            parameters.Add("@IdCiudad", null);
            parameters.Add("@IdEstadoTablet", null);
            parameters.Add("@Asignado", asignado);

            var id = await connection.ExecuteScalarAsync<decimal>(
                "INV_LogAsignaciones_Add",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return (long)id;
        }
    }
}
