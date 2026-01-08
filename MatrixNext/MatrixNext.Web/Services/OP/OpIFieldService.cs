using System.Data;
using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services.OP;

public class OpIFieldService : IOpIFieldService
{
    private readonly MatrixDbContext _dbContext;
    private readonly ILogger<OpIFieldService> _logger;

    public OpIFieldService(MatrixDbContext dbContext, ILogger<OpIFieldService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IReadOnlyList<IFieldProjectDto>> GetProjectsAsync(int tipo)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        await EnsureOpenAsync(connection);

        var query = tipo switch
        {
            2 => "SELECT IdProjecto AS ProjectId, NombreProjecto, TrabajoId, Activo FROM OP_ProyectosIField WHERE Activo=1 AND TrabajoId IS NULL",
            3 => "SELECT IdProjecto AS ProjectId, NombreProjecto, TrabajoId, Activo FROM OP_ProyectosIField WHERE Activo=0 AND TrabajoId IS NOT NULL AND IdProjecto>1005",
            _ => "SELECT IdProjecto AS ProjectId, NombreProjecto, TrabajoId, Activo FROM OP_ProyectosIField WHERE Activo=1 AND NOT(TrabajoId IS NULL)"
        };

        var result = await connection.QueryAsync<IFieldProjectDto>(query);
        return result.ToList();
    }

    public async Task<IFieldProjectDto?> GetProjectAsync(int projectId)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        await EnsureOpenAsync(connection);

        var query = "SELECT IdProjecto AS ProjectId, NombreProjecto, TrabajoId, Activo FROM OP_ProyectosIField WHERE IdProjecto = @ProjectId";
        return await connection.QueryFirstOrDefaultAsync<IFieldProjectDto>(query, new { ProjectId = projectId });
    }

    public async Task<IReadOnlyList<IFieldConfigRow>> GetProjectConfigAsync(int projectId)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        await EnsureOpenAsync(connection);

        var query = @"
SELECT 
    C.id AS ConfigId,
    C.UserIField AS UsuarioIfield,
    C.CCEncuestador AS Encuestador,
    C.CCSupervisor AS Supervisor,
    U.Usuario,
    C.FechaConfig
FROM OP_ConfigIfieldData C
LEFT JOIN US_Usuarios U ON U.id = C.IDUsuario
LEFT JOIN OP_ProyectosIField P ON P.TrabajoId = C.IdTrabajo
WHERE P.IdProjecto = @ProjectId";

        var configs = await connection.QueryAsync<IFieldConfigRow>(query, new { ProjectId = projectId });
        return configs.ToList();
    }

    public async Task<IReadOnlyList<IFieldPendingRow>> GetPendientesAsync(int projectId)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        await EnsureOpenAsync(connection);

        var query = "SELECT IdIfield, NumEncuesta, Encuestador, Ciudad, FechaEncuesta, FechaSync FROM OP_DataFromIFieldPass WHERE IdIfield = @ProjectId";
        var pendientes = await connection.QueryAsync<IFieldPendingRow>(query, new { ProjectId = projectId });
        return pendientes.ToList();
    }

    public async Task UpdateProjectJobBookAsync(int projectId, int trabajoId)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        await EnsureOpenAsync(connection);

        var sql = "UPDATE OP_ProyectosIField SET TrabajoId = @TrabajoId WHERE IdProjecto = @ProjectId";
        await connection.ExecuteAsync(sql, new { ProjectId = projectId, TrabajoId = trabajoId });
    }

    public async Task InsertConfigItemsAsync(IEnumerable<IFieldAddConfigInput> inputs)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        await EnsureOpenAsync(connection);

        foreach (var input in inputs)
        {
            var parameters = new DynamicParameters();
            parameters.Add("IdIField", input.ProjectId);
            parameters.Add("UserIfield", input.UserIField);
            parameters.Add("Encuestador", input.Encuestador);
            parameters.Add("Supervisor", input.Supervisor);
            parameters.Add("Usuario", input.UsuarioId);

            await connection.ExecuteAsync("OP_ConfigIfieldData_ADD", parameters, commandType: CommandType.StoredProcedure);
        }
    }

    public async Task RemoveConfigItemAsync(int configId)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        await EnsureOpenAsync(connection);

        var sql = "DELETE FROM OP_ConfigIfieldData WHERE id = @ConfigId";
        await connection.ExecuteAsync(sql, new { ConfigId = configId });
    }

    private static async Task EnsureOpenAsync(System.Data.Common.DbConnection connection)
    {
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }
    }
}
