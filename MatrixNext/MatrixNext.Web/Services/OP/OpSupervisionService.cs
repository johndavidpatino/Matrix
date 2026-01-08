using System.Data;
using System.Data.Common;
using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services.OP;

public class OpSupervisionService : IOpSupervisionService
{
    private readonly MatrixDbContext _dbContext;
    private readonly ILogger<OpSupervisionService> _logger;

    public OpSupervisionService(MatrixDbContext dbContext, ILogger<OpSupervisionService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IReadOnlyList<UsuarioDto>> ObtenerUsuariosActivosAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        await EnsureOpenAsync(connection);

        var query = "SELECT id AS Id, Usuario AS Nombre FROM US_Usuarios WHERE Activo = 1";
        var usuarios = await connection.QueryAsync<UsuarioDto>(query);
        return usuarios.ToList();
    }

    public async Task<bool> GuardarSupervisionAsync(GuardarSupervisionRequest request, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        await EnsureOpenAsync(connection);

        var parameters = new DynamicParameters();
        parameters.Add("TrabajoId", request.TrabajoId);
        parameters.Add("IdentificadorCuestionario", request.Identificacion);
        parameters.Add("Supervisor", request.SupervisorId);
        parameters.Add("Operador", request.OperadorId);
        parameters.Add("FechaSupervision", request.FechaSupervision);
        for (var i = 0; i < 13; i++)
        {
            parameters.Add($"CRI{(i + 1):00}", request.CriFlags[i]);
        }
        for (var i = 0; i < 4; i++)
        {
            parameters.Add($"COM{(i + 1):00}", request.ComValues[i]);
            parameters.Add($"ACC{(i + 1):00}", request.AccValues[i]);
        }
        parameters.Add("Observacion", request.Observaciones);

        var result = await connection.ExecuteAsync("OP_SupervisionCampoTelefonico_Add", parameters, commandType: CommandType.StoredProcedure);
        _logger.LogInformation("Supervisión campo telefónico almacenada para trabajo {TrabajoId}", request.TrabajoId);
        return result > 0;
    }

    public async Task<IReadOnlyList<SupervisionHistoryRow>> ObtenerHistoricoAsync(long trabajoId, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        await EnsureOpenAsync(connection);

        var query = @"
SELECT TOP 20
    id AS Id,
    FechaSupervision AS Fecha,
    Operador AS Operador,
    Supervisor AS Supervisor,
    Observacion
FROM OP_SupervisionCampoTelefonico
WHERE TrabajoId = @TrabajoId
ORDER BY FechaSupervision DESC";

        var rows = await connection.QueryAsync<SupervisionHistoryRow>(query, new { TrabajoId = trabajoId });
        return rows.ToList();
    }

    public async Task<SupervisionSummary> ObtenerResumenAsync(long trabajoId, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        await EnsureOpenAsync(connection);

        var total = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM OP_SupervisionCampoTelefonico WHERE TrabajoId = @TrabajoId",
            new { TrabajoId = trabajoId });
        var hoy = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM OP_SupervisionCampoTelefonico WHERE TrabajoId = @TrabajoId AND FechaSupervision >= CAST(GETDATE() AS DATE)",
            new { TrabajoId = trabajoId });

        var alertas = _logger.IsEnabled(LogLevel.Information) ? Math.Max(0, 3 - hoy) : 0;
        return new SupervisionSummary(total, hoy, alertas);
    }

    public async Task<SupervisionSummary> ObtenerResumenGeneralAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        await EnsureOpenAsync(connection);

        var total = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM OP_SupervisionCampoTelefonico");
        var hoy = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM OP_SupervisionCampoTelefonico WHERE FechaSupervision >= CAST(GETDATE() AS DATE)");
        var alertas = Math.Max(0, 3 - hoy);

        return new SupervisionSummary(total, hoy, alertas);
    }

    private static async Task EnsureOpenAsync(DbConnection connection)
    {
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }
    }
}
