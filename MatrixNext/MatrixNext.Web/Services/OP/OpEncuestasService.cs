using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace MatrixNext.Web.Services.OP;

public class OpEncuestasService : IOpEncuestasService
{
    private readonly string _connectionString;
    private readonly ILogger<OpEncuestasService> _logger;

    public OpEncuestasService(IConfiguration configuration, ILogger<OpEncuestasService> logger)
    {
        _connectionString = configuration.GetConnectionString("LegacyDatabase")
            ?? throw new InvalidOperationException("LegacyDatabase connection string is required for OP encuestas");
        _logger = logger;
    }

    public async Task<bool> ActivarEncuestaAsync(long trabajoId, decimal numeroEncuesta, string observacion, long usuarioId)
    {
        return await EjecutarAsync("OP_GestionCampo_ActivarEncuesta", new
        {
            idTrabajo = trabajoId,
            noEncuesta = numeroEncuesta,
            observacion,
            usuario = usuarioId
        });
    }

    public async Task<bool> AnularEncuestaAsync(long trabajoId, decimal numeroEncuesta, string observacion)
    {
        return await EjecutarAsync("OP_GestionCampo_AnularEncuesta", new
        {
            idTrabajo = trabajoId,
            noEncuesta = numeroEncuesta,
            observacion
        });
    }

    private async Task<bool> EjecutarAsync(string spName, object parametros)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await connection.ExecuteAsync(spName, parametros, commandType: CommandType.StoredProcedure);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ejecutando SP {spName}", spName);
            return false;
        }
    }
}
