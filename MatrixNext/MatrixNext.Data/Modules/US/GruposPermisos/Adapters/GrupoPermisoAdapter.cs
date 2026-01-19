using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Modules.US.GruposPermisos.Adapters;

/// <summary>
/// Adaptador para Grupos de Permisos
/// SP: US_GruposPermisos_Get, US_GruposPermisos_Add, US_GruposPermisos_Edit, US_GruposPermisos_Del
/// Ref: CoreProject/Clases/US/GrupoPermisos.vb
/// </summary>
public interface IGrupoPermisoAdapter
{
    Task<IEnumerable<Models.GrupoPermisoDto>> ObtenerGruposPermisosAsync(string? nombre = null);
    Task<Models.GrupoPermisoDto?> ObtenerPorIdAsync(int id);
    Task<int> GuardarAsync(int id, string nombre);
    Task<int> EditarAsync(int id, string nombre);
    Task<int> EliminarAsync(int id);
}

public class GrupoPermisoAdapter : IGrupoPermisoAdapter
{
    private readonly string _connectionString;
    private readonly ILogger<GrupoPermisoAdapter> _logger;

    public GrupoPermisoAdapter(string connectionString, ILogger<GrupoPermisoAdapter> logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Models.GrupoPermisoDto>> ObtenerGruposPermisosAsync(string? nombre = null)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.QueryAsync<Models.GrupoPermisoDto>(
                "US_GruposPermisos_Get",
                new { GrupoPermisos = nombre },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener grupos de permisos. Nombre: {Nombre}", nombre);
            throw;
        }
    }

    public async Task<Models.GrupoPermisoDto?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.QueryFirstOrDefaultAsync<Models.GrupoPermisoDto>(
                "SELECT Id, GrupoPermisos FROM US_GruposPermisos WHERE Id = @Id",
                new { Id = id }
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener grupo de permisos por Id: {Id}", id);
            throw;
        }
    }

    public async Task<int> GuardarAsync(int id, string nombre)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.ExecuteAsync(
                "US_GruposPermisos_Add",
                new { Id = id, GrupoPermisos = nombre },
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("Grupo de permisos creado. Id: {Id}, Nombre: {Nombre}", id, nombre);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar grupo de permisos. Id: {Id}, Nombre: {Nombre}", id, nombre);
            throw;
        }
    }

    public async Task<int> EditarAsync(int id, string nombre)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.ExecuteAsync(
                "US_GruposPermisos_Edit",
                new { Id = id, GrupoPermisos = nombre },
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("Grupo de permisos editado. Id: {Id}, Nombre: {Nombre}", id, nombre);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al editar grupo de permisos. Id: {Id}", id);
            throw;
        }
    }

    public async Task<int> EliminarAsync(int id)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.ExecuteAsync(
                "US_GruposPermisos_Del",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("Grupo de permisos eliminado. Id: {Id}", id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar grupo de permisos. Id: {Id}", id);
            throw;
        }
    }
}
