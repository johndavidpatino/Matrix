using Dapper;
using MatrixNext.Data.DTOs.INV;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MatrixNext.Data.Adapters.INV
{
    /// <summary>
    /// Adapter para operaciones de datos de registro de artículos usando Dapper.
    /// </summary>
    public class RegistroArticulosAdapter : IRegistroArticulosAdapter
    {
        private readonly string _connectionString;

        public RegistroArticulosAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixConnection")
                ?? throw new InvalidOperationException("Connection string 'MatrixConnection' no encontrada");
        }

        public async Task<IEnumerable<RegistroArticuloListDto>> ObtenerTodosAsync(
            long? id = null,
            long? idTipoArticulo = null,
            long? idArticulo = null,
            long? idSede = null,
            long? idUsuarioAsignado = null,
            bool? asignado = null,
            string? todosCampos = null)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@IdTipoArticulo", idTipoArticulo);
            parameters.Add("@IdArticulo", idArticulo);
            parameters.Add("@IdTipoComputador", null);
            parameters.Add("@PertenecePC", null);
            parameters.Add("@IdTipoPeriferico", null);
            parameters.Add("@IdTipoProducto", null);
            parameters.Add("@IdEstado", null);
            parameters.Add("@IdSede", idSede);
            parameters.Add("@IdUsuarioAsignado", idUsuarioAsignado);
            parameters.Add("@UsuarioAsignado", null);
            parameters.Add("@Asignado", asignado);
            parameters.Add("@IdArticuloParam", null);
            parameters.Add("@TodosCampos", todosCampos);

            var results = await connection.QueryAsync(
                "INV_RegistroArticulos_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return results.Select(r => new RegistroArticuloListDto
            {
                Id = r.Id,
                IdTipoArticulo = r.IdTipoArticulo,
                TipoArticulo = r.TipoArticulo ?? string.Empty,
                IdArticulo = r.IdArticulo,
                Articulo = r.Articulo ?? string.Empty,
                FechaCompra = r.FechaCompra,
                JobBookCodigo = r.JobBookCodigo,
                JobBookNombre = r.JobBookNombre,
                ValorUnitario = r.Valor,
                IdEstado = r.IdEstado,
                Estado = r.Estado,
                Descripcion = r.Descripcion,
                Symphony = r.Symphony,
                IdFisico = r.IdFisico,
                Sede = r.Sede,
                Marca = r.Marca,
                Modelo = r.Modelo,
                Serial = r.Serial,
                NombreEquipo = r.NombreEquipo,
                Asignado = r.Asignado,
                Cantidad = r.Cantidad,
                IdUsuarioAsignado = r.IdUsuarioAsignado,
                UsuarioAsignado = r.UsuarioAsignado
            });
        }

        public async Task<RegistroArticuloDto?> ObtenerPorIdAsync(long id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@IdTipoArticulo", null);
            parameters.Add("@IdArticulo", null);
            parameters.Add("@IdTipoComputador", null);
            parameters.Add("@PertenecePC", null);
            parameters.Add("@IdTipoPeriferico", null);
            parameters.Add("@IdTipoProducto", null);
            parameters.Add("@IdEstado", null);
            parameters.Add("@IdSede", null);
            parameters.Add("@IdUsuarioAsignado", null);
            parameters.Add("@UsuarioAsignado", null);
            parameters.Add("@Asignado", null);
            parameters.Add("@IdArticuloParam", null);
            parameters.Add("@TodosCampos", null);

            var result = await connection.QueryFirstOrDefaultAsync(
                "INV_RegistroArticulos_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (result == null) return null;

            return new RegistroArticuloDto
            {
                Id = result.Id,
                IdTipoArticulo = result.IdTipoArticulo,
                IdArticulo = result.IdArticulo,
                FechaCompra = result.FechaCompra,
                IdCentroCosto = result.IdCentroCosto,
                IdBU = result.IdBU,
                IdTrabajo = result.IdTrabajo,
                JobBookCodigo = result.JobBookCodigo,
                JobBookNombre = result.JobBookNombre,
                IdCuentaContable = result.IdCuentaContable,
                ValorUnitario = result.Valor,
                IdEstado = result.IdEstado,
                Descripcion = result.Descripcion,
                Symphony = result.Symphony,
                IdFisico = result.IdFisico,
                IdSede = result.IdSede,
                IdTipoComputador = result.IdTipoComputador,
                PertenecePC = result.PertenecePC,
                Marca = result.Marca,
                Modelo = result.Modelo,
                Procesador = result.Procesador,
                Memoria = result.Memoria,
                Almacenamiento = result.Almacenamiento,
                SistemaOperativo = result.SistemaOperativo,
                Serial = result.Serial,
                NombreEquipo = result.NombreEquipo,
                Office = result.Office,
                Programas = result.Programas,
                TipoServidor = result.TipoServidor,
                Raid = result.Raid,
                IdTablet = result.IdTablet,
                IdSTG = result.IdSTG,
                TamanoPantalla = result.TamanoPantalla,
                Chip = result.Chip,
                IMEI = result.IMEI,
                Pertenece = result.Pertenece,
                Operador = result.Operador,
                NumeroCelular = result.NumeroCelular,
                CantidadMinutos = result.CantidadMinutos,
                IdTipoPeriferico = result.IdTipoPeriferico,
                IdTipoProducto = result.IdTipoProducto,
                Producto = result.Producto,
                TipoObsequio = result.TipoObsequio,
                TipoBono = result.TipoBono,
                Asignado = result.Asignado,
                FechaFinRenta = result.FechaFinRenta,
                NumeroPV = result.NumeroPV,
                ProveedorId = result.ProveedorId,
                Cantidad = result.Cantidad,
                IdProductoPapeleria = result.IdProductoPapeleria
            };
        }

        public async Task<long> CrearAsync(RegistroArticuloDto dto, long usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@IdTipoArticulo", dto.IdTipoArticulo);
            parameters.Add("@IdArticulo", dto.IdArticulo);
            parameters.Add("@FechaCompra", dto.FechaCompra);
            parameters.Add("@UsuarioRegistra", usuarioId);
            parameters.Add("@IdCentroCosto", dto.IdCentroCosto);
            parameters.Add("@IdBU", dto.IdBU);
            parameters.Add("@IdTrabajo", dto.IdTrabajo);
            parameters.Add("@JobBookCodigo", dto.JobBookCodigo);
            parameters.Add("@JobBookNombre", dto.JobBookNombre);
            parameters.Add("@IdCuentaContable", dto.IdCuentaContable);
            parameters.Add("@ValorUnitario", dto.ValorUnitario);
            parameters.Add("@IdEstado", dto.IdEstado);
            parameters.Add("@Descripcion", dto.Descripcion);
            parameters.Add("@Symphony", dto.Symphony);
            parameters.Add("@IdFisico", dto.IdFisico);
            parameters.Add("@IdSede", dto.IdSede);
            parameters.Add("@IdTipoComputador", dto.IdTipoComputador);
            parameters.Add("@PertenecePC", dto.PertenecePC);
            parameters.Add("@IdTipoPeriferico", dto.IdTipoPeriferico);
            parameters.Add("@Marca", dto.Marca);
            parameters.Add("@Modelo", dto.Modelo);
            parameters.Add("@Procesador", dto.Procesador);
            parameters.Add("@Memoria", dto.Memoria);
            parameters.Add("@Almacenamiento", dto.Almacenamiento);
            parameters.Add("@SistemaOperativo", dto.SistemaOperativo);
            parameters.Add("@Serial", dto.Serial);
            parameters.Add("@NombreEquipo", dto.NombreEquipo);
            parameters.Add("@Office", dto.Office);
            parameters.Add("@Programas", dto.Programas);
            parameters.Add("@TipoServidor", dto.TipoServidor);
            parameters.Add("@Raid", dto.Raid);
            parameters.Add("@IdTablet", dto.IdTablet);
            parameters.Add("@IdSTG", dto.IdSTG);
            parameters.Add("@TamanoPantalla", dto.TamanoPantalla);
            parameters.Add("@Chip", dto.Chip);
            parameters.Add("@IMEI", dto.IMEI);
            parameters.Add("@Pertenece", dto.Pertenece);
            parameters.Add("@Operador", dto.Operador);
            parameters.Add("@NumeroCelular", dto.NumeroCelular);
            parameters.Add("@CantidadMinutos", dto.CantidadMinutos);
            parameters.Add("@IdTipoProducto", dto.IdTipoProducto);
            parameters.Add("@Producto", dto.Producto);
            parameters.Add("@TipoObsequio", dto.TipoObsequio);
            parameters.Add("@TipoBono", dto.TipoBono);
            parameters.Add("@Asignado", dto.Asignado);
            parameters.Add("@FechaFinRenta", dto.FechaFinRenta);
            parameters.Add("@NumeroPV", dto.NumeroPV);
            parameters.Add("@ProveedorId", dto.ProveedorId);
            parameters.Add("@Cantidad", dto.Cantidad);
            parameters.Add("@IdProductoPapeleria", dto.IdProductoPapeleria);

            var id = await connection.ExecuteScalarAsync<decimal>(
                "INV_RegistroArticulos_Add",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return (long)id;
        }

        public async Task ActualizarAsync(RegistroArticuloDto dto, long usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", dto.Id);
            parameters.Add("@IdTipoArticulo", dto.IdTipoArticulo);
            parameters.Add("@IdArticulo", dto.IdArticulo);
            parameters.Add("@FechaCompra", dto.FechaCompra);
            parameters.Add("@UsuarioRegistra", usuarioId);
            parameters.Add("@IdCentroCosto", dto.IdCentroCosto);
            parameters.Add("@IdBU", dto.IdBU);
            parameters.Add("@IdTrabajo", dto.IdTrabajo);
            parameters.Add("@JobBookCodigo", dto.JobBookCodigo);
            parameters.Add("@JobBookNombre", dto.JobBookNombre);
            parameters.Add("@IdCuentaContable", dto.IdCuentaContable);
            parameters.Add("@ValorUnitario", dto.ValorUnitario);
            parameters.Add("@IdEstado", dto.IdEstado);
            parameters.Add("@Descripcion", dto.Descripcion);
            parameters.Add("@Symphony", dto.Symphony);
            parameters.Add("@IdFisico", dto.IdFisico);
            parameters.Add("@IdSede", dto.IdSede);
            parameters.Add("@IdTipoComputador", dto.IdTipoComputador);
            parameters.Add("@PertenecePC", dto.PertenecePC);
            parameters.Add("@IdTipoPeriferico", dto.IdTipoPeriferico);
            parameters.Add("@Marca", dto.Marca);
            parameters.Add("@Modelo", dto.Modelo);
            parameters.Add("@Procesador", dto.Procesador);
            parameters.Add("@Memoria", dto.Memoria);
            parameters.Add("@Almacenamiento", dto.Almacenamiento);
            parameters.Add("@SistemaOperativo", dto.SistemaOperativo);
            parameters.Add("@Serial", dto.Serial);
            parameters.Add("@NombreEquipo", dto.NombreEquipo);
            parameters.Add("@Office", dto.Office);
            parameters.Add("@Programas", dto.Programas);
            parameters.Add("@TipoServidor", dto.TipoServidor);
            parameters.Add("@Raid", dto.Raid);
            parameters.Add("@IdTablet", dto.IdTablet);
            parameters.Add("@IdSTG", dto.IdSTG);
            parameters.Add("@TamanoPantalla", dto.TamanoPantalla);
            parameters.Add("@Chip", dto.Chip);
            parameters.Add("@IMEI", dto.IMEI);
            parameters.Add("@Pertenece", dto.Pertenece);
            parameters.Add("@Operador", dto.Operador);
            parameters.Add("@NumeroCelular", dto.NumeroCelular);
            parameters.Add("@CantidadMinutos", dto.CantidadMinutos);
            parameters.Add("@IdTipoProducto", dto.IdTipoProducto);
            parameters.Add("@Producto", dto.Producto);
            parameters.Add("@TipoObsequio", dto.TipoObsequio);
            parameters.Add("@TipoBono", dto.TipoBono);
            parameters.Add("@Asignado", dto.Asignado);
            parameters.Add("@FechaFinRenta", dto.FechaFinRenta);
            parameters.Add("@NumeroPV", dto.NumeroPV);
            parameters.Add("@ProveedorId", dto.ProveedorId);
            parameters.Add("@Cantidad", dto.Cantidad);
            parameters.Add("@IdProductoPapeleria", dto.IdProductoPapeleria);

            await connection.ExecuteAsync(
                "INV_RegistroArticulos_Edit",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task ActualizarAsignadoAsync(long id, bool asignado)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Asignado", asignado);

            await connection.ExecuteAsync(
                "INV_RegistroArticulos_Asignado_Edit",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<RegistroArticuloListDto>> ObtenerDisponiblesAsync(long? idTipoArticulo = null)
        {
            return await ObtenerTodosAsync(
                idTipoArticulo: idTipoArticulo,
                asignado: false
            );
        }

        public async Task EliminarAsync(long id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            await connection.ExecuteAsync(
                "INV_RegistroArticulos_Del",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
