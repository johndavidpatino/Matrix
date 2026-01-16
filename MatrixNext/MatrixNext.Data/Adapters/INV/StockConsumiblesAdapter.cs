using Dapper;
using MatrixNext.Data.DTOs.INV;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MatrixNext.Data.Adapters.INV
{
    public class StockConsumiblesAdapter : IStockConsumiblesAdapter
    {
        private readonly string _connectionString;

        public StockConsumiblesAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixConnection")
                ?? throw new InvalidOperationException("Connection string 'MatrixConnection' no encontrada");
        }

        public async Task<IEnumerable<StockConsumibleListDto>> ObtenerTodosAsync(
            long? id = null,
            long? idConsumible = null,
            short? tipoMovimiento = null,
            int? idBU = null,
            string? jobBookCodigo = null,
            long? idUsuarioAsignado = null,
            bool? legalizado = null)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@IdConsumible", idConsumible);
            parameters.Add("@IdArticulo", null);
            parameters.Add("@TipoMovimiento", tipoMovimiento);
            parameters.Add("@IdBU", idBU);
            parameters.Add("@JobBookCodigo", jobBookCodigo);
            parameters.Add("@JobBookNombre", null);
            parameters.Add("@IdCiudad", null);
            parameters.Add("@IdUsuarioAsignado", idUsuarioAsignado);
            parameters.Add("@UsuarioAsignado", null);
            parameters.Add("@TipoCargo", null);
            parameters.Add("@Legalizado", legalizado);

            var results = await connection.QueryAsync(
                "INV_StockConsumibles_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return results.Select(r => new StockConsumibleListDto
            {
                Id = r.Id,
                IdConsumible = r.IdConsumible,
                Articulo = r.Articulo ?? string.Empty,
                TipoProducto = r.TipoProducto,
                Producto = r.Producto,
                NumeroVale = r.NumeroVale,
                Fecha = r.Fecha,
                TipoMovimiento = r.TipoMovimiento,
                TipoMovimientoNombre = r.TipoMovimientoNombre ?? string.Empty,
                Estado = r.Estado,
                EstadoNombre = r.EstadoNombre,
                JobBookCodigo = r.JobBookCodigo,
                JobBookNombre = r.JobBookNombre,
                Ciudad = r.Ciudad,
                Valor = r.Valor,
                Total = r.Total,
                Disponible = r.Disponible,
                IdUsuarioAsignado = r.IdUsuarioAsignado,
                UsuarioAsignado = r.UsuarioAsignado,
                Cargo = r.Cargo,
                Legalizado = r.Legalizado,
                Observaciones = r.Observaciones,
                UsuarioRegistra = r.UsuarioRegistra,
                UsuarioRegistraNombre = r.UsuarioRegistraNombre,
                FechaRegistro = r.FechaRegistro
            });
        }

        public async Task<IEnumerable<StockConsumibleListDto>> ObtenerPorConsumibleAsync(long idConsumible)
        {
            return await ObtenerTodosAsync(idConsumible: idConsumible);
        }

        public async Task<IEnumerable<StockConsumibleListDto>> ObtenerPorLegalizarAsync(
            int? idBU = null,
            string? jobBookCodigo = null,
            long? idArticulo = null,
            long? idUsuarioAsignado = null)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@IdBU", idBU);
            parameters.Add("@JobBookCodigo", jobBookCodigo);
            parameters.Add("@IdArticulo", idArticulo);
            parameters.Add("@IdTipoProducto", null);
            parameters.Add("@UsuarioRegistra", null);
            parameters.Add("@IdUsuarioAsignado", idUsuarioAsignado);
            parameters.Add("@IdConsumible", null);

            var results = await connection.QueryAsync(
                "INV_StockxLegalizar_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return results.Select(r => new StockConsumibleListDto
            {
                Id = r.Id,
                IdConsumible = r.IdConsumible,
                Articulo = r.Articulo ?? string.Empty,
                TipoProducto = r.TipoProducto,
                Producto = r.Producto,
                Fecha = r.Fecha,
                JobBookCodigo = r.JobBookCodigo,
                JobBookNombre = r.JobBookNombre,
                Valor = r.Valor,
                Total = r.Total,
                Disponible = r.Disponible,
                IdUsuarioAsignado = r.IdUsuarioAsignado ?? 0,
                UsuarioAsignado = r.UsuarioAsignado,
                Legalizado = false
            });
        }

        public async Task<long> CrearAsync(StockConsumibleDto dto, long usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@IdConsumible", dto.IdConsumible);
            parameters.Add("@NumeroVale", dto.NumeroVale);
            parameters.Add("@Fecha", dto.Fecha);
            parameters.Add("@UsuarioRegistra", usuarioId);
            parameters.Add("@TipoMovimiento", dto.TipoMovimiento);
            parameters.Add("@Estado", dto.Estado);
            parameters.Add("@IdCentroCosto", dto.IdCentroCosto);
            parameters.Add("@IdBU", dto.IdBU);
            parameters.Add("@IdTrabajo", dto.IdTrabajo);
            parameters.Add("@JobBookCodigo", dto.JobBookCodigo);
            parameters.Add("@JobBookNombre", dto.JobBookNombre);
            parameters.Add("@IdCuentaContable", dto.IdCuentaContable);
            parameters.Add("@IdCiudad", dto.IdCiudad);
            parameters.Add("@Valor", dto.Valor);
            parameters.Add("@Total", dto.Total);
            parameters.Add("@Disponible", dto.Disponible);
            parameters.Add("@IdUsuarioAsignado", dto.IdUsuarioAsignado);
            parameters.Add("@TipoCargo", dto.TipoCargo);
            parameters.Add("@Observaciones", dto.Observaciones);

            var id = await connection.ExecuteScalarAsync<decimal>(
                "INV_StockConsumibles_Add",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return (long)id;
        }
    }
}
