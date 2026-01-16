using Dapper;
using MatrixNext.Data.DTOs.INV;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MatrixNext.Data.Adapters.INV
{
    public class LegalizacionesAdapter : ILegalizacionesAdapter
    {
        private readonly string _connectionString;

        public LegalizacionesAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixConnection")
                ?? throw new InvalidOperationException("Connection string 'MatrixConnection' no encontrada");
        }

        public async Task<IEnumerable<LegalizacionDto>> ObtenerTodosAsync(
            long? id = null,
            long? idConsumible = null,
            int? idBU = null,
            string? jobBookCodigo = null,
            long? idUsuarioAsignado = null)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@IdConsumible", idConsumible);
            parameters.Add("@IdBU", idBU);
            parameters.Add("@JobBookCodigo", jobBookCodigo);
            parameters.Add("@IdArticulo", null);
            parameters.Add("@IdTipoLegalizacion", null);
            parameters.Add("@UsuarioRegistra", null);
            parameters.Add("@IdUsuarioAsignado", idUsuarioAsignado);

            var results = await connection.QueryAsync(
                "INV_Legalizaciones_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return results.Select(r => new LegalizacionDto
            {
                Id = r.Id,
                IdConsumible = r.IdConsumible,
                TipoLegalizacion = r.TipoLegalizacion,
                Radicado = r.Radicado ?? string.Empty,
                Fecha = r.Fecha,
                IdUsuarioResponsable = r.IdUsuarioResponsable,
                Unidades = r.Unidades,
                Firmas = r.Firmas,
                Devoluciones = r.Devoluciones,
                NotasCredito = r.NotasCredito,
                DescuentoNomina = r.DescuentoNomina,
                ValorLegalizado = r.ValorLegalizado,
                Pendiente = r.Pendiente,
                Observaciones = r.Observaciones,
                Legalizado = r.Legalizado,
                IdCentroCosto = r.IdCentroCosto,
                IdBU = r.IdBU,
                IdJobBook = r.IdJobBook,
                JobBookCodigo = r.JobBookCodigo,
                JobBookNombre = r.JobBookNombre,
                ValorCarrera = r.ValorCarrera,
                Verificado = r.Verificado,
                FechaVerificacion = r.FechaVerificacion,
                IdUsuarioVerifica = r.IdUsuarioVerifica,
                UsuarioRegistra = r.UsuarioRegistra
            });
        }

        public async Task<LegalizacionDto?> ObtenerPorIdAsync(long id)
        {
            var results = await ObtenerTodosAsync(id: id);
            return results.FirstOrDefault();
        }

        public async Task<long> CrearAsync(LegalizacionDto dto, long usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@IdConsumible", dto.IdConsumible);
            parameters.Add("@UsuarioRegistra", usuarioId);
            parameters.Add("@TipoLegalizacion", dto.TipoLegalizacion);
            parameters.Add("@Radicado", dto.Radicado);
            parameters.Add("@Fecha", dto.Fecha);
            parameters.Add("@IdUsuarioResponsable", dto.IdUsuarioResponsable);
            parameters.Add("@Unidades", dto.Unidades);
            parameters.Add("@Firmas", dto.Firmas);
            parameters.Add("@Devoluciones", dto.Devoluciones);
            parameters.Add("@NotasCredito", dto.NotasCredito);
            parameters.Add("@DescuentoNomina", dto.DescuentoNomina);
            parameters.Add("@ValorLegalizado", dto.ValorLegalizado);
            parameters.Add("@Pendiente", dto.Pendiente);
            parameters.Add("@Observaciones", dto.Observaciones);
            parameters.Add("@Legalizado", dto.Legalizado);
            parameters.Add("@IdCentroCosto", dto.IdCentroCosto);
            parameters.Add("@IdBU", dto.IdBU);
            parameters.Add("@IdJobBook", dto.IdJobBook);
            parameters.Add("@JobBookCodigo", dto.JobBookCodigo);
            parameters.Add("@JobBookNombre", dto.JobBookNombre);
            parameters.Add("@ValorCarrera", dto.ValorCarrera);
            parameters.Add("@Verificado", dto.Verificado);
            parameters.Add("@FechaVerificacion", dto.FechaVerificacion);
            parameters.Add("@IdUsuarioVerifica", dto.IdUsuarioVerifica);

            var id = await connection.ExecuteScalarAsync<decimal>(
                "INV_Legalizaciones_Add",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return (long)id;
        }

        public async Task ActualizarAsync(LegalizacionDto dto, long usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", dto.Id);
            parameters.Add("@IdConsumible", dto.IdConsumible);
            parameters.Add("@UsuarioRegistra", usuarioId);
            parameters.Add("@TipoLegalizacion", dto.TipoLegalizacion);
            parameters.Add("@Radicado", dto.Radicado);
            parameters.Add("@Fecha", dto.Fecha);
            parameters.Add("@IdUsuarioResponsable", dto.IdUsuarioResponsable);
            parameters.Add("@Unidades", dto.Unidades);
            parameters.Add("@Firmas", dto.Firmas);
            parameters.Add("@Devoluciones", dto.Devoluciones);
            parameters.Add("@NotasCredito", dto.NotasCredito);
            parameters.Add("@DescuentoNomina", dto.DescuentoNomina);
            parameters.Add("@ValorLegalizado", dto.ValorLegalizado);
            parameters.Add("@Pendiente", dto.Pendiente);
            parameters.Add("@Observaciones", dto.Observaciones);
            parameters.Add("@Legalizado", dto.Legalizado);
            parameters.Add("@IdCentroCosto", dto.IdCentroCosto);
            parameters.Add("@IdBU", dto.IdBU);
            parameters.Add("@IdJobBook", dto.IdJobBook);
            parameters.Add("@JobBookCodigo", dto.JobBookCodigo);
            parameters.Add("@JobBookNombre", dto.JobBookNombre);
            parameters.Add("@ValorCarrera", dto.ValorCarrera);
            parameters.Add("@Verificado", dto.Verificado);
            parameters.Add("@FechaVerificacion", dto.FechaVerificacion);
            parameters.Add("@IdUsuarioVerifica", dto.IdUsuarioVerifica);

            await connection.ExecuteAsync(
                "INV_Legalizaciones_Edit",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> EliminarAsync(long id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            return await connection.ExecuteAsync(
                "INV_Legalizaciones_Del",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
