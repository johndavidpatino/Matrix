using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MatrixNext.Web.Services.OP.Models;

namespace MatrixNext.Web.Services.OP;

public class OpTraficoDataAdapter : IOpTraficoDataAdapter
{
    private readonly string _connectionString;
    private const string SpTraficoCiudad = "OP_TraficoEncuestasCiudad";

    public OpTraficoDataAdapter(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("LegacyDatabase")
            ?? configuration.GetConnectionString("MatrixDb")
            ?? throw new InvalidOperationException("LegacyDatabase or MatrixDb connection string required for OP trafico");
    }

    public async Task<IReadOnlyCollection<TraficoCiudadDto>> ObtenerCiudadesPorTrabajoAsync(long trabajoId, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await EnsureStoredProcedureExistsAsync(connection, SpTraficoCiudad);
        var items = await connection.QueryAsync<OpTraficoEncuestasCiudadResult>(
            SpTraficoCiudad,
            new { trabajoId },
            commandType: CommandType.StoredProcedure);

        return items.Select(item => new TraficoCiudadDto(
            Id: item.Id,
            CiudadCodigo: item.Res_Ciudad,
            EncuestasEnviadas: item.cuenta ?? 0
        )).ToList();
    }

    private static async Task EnsureStoredProcedureExistsAsync(SqlConnection connection, string name)
    {
        var exists = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(1) FROM sys.objects WHERE type = 'P' AND name = @name",
            new { name });

        if (exists == 0)
        {
            throw new InvalidOperationException($"Stored Procedure no encontrado: {name}");
        }
    }

    private sealed class OpTraficoEncuestasCiudadResult
    {
        public decimal Id { get; set; }
        public decimal Res_Ciudad { get; set; }
        public int? cuenta { get; set; }
    }
}
