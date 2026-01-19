using Dapper;
using MatrixNext.Data.Modules.US.RolesPermisos.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MatrixNext.Data.Modules.US.RolesPermisos.Adapters;

/// <summary>
/// Adapter para RolesPermisos
/// SP: US_RolesPermisos_Get, US_RolesPermisos_Add, US_RolesPermisos_Del
/// Tabla: US_RolesPermisos
/// Ref: CoreProject/Clases/US/RolesPermisos.vb
/// </summary>
public interface IRolPermisoAdapter
{
    Task<IEnumerable<RolPermisoDto>> ObtenerRolesPermisosAsync(int permisoId);
    Task<int> GuardarRolPermisoAsync(int permisoId, int rolId);
    Task<bool> EliminarRolPermisoAsync(int permisoId, int rolId);
    Task<IEnumerable<RolComboDto>> ObtenerRolesComboAsync();
    Task<IEnumerable<PermisoComboDto>> ObtenerPermisosComboAsync();
}

public class RolPermisoAdapter : IRolPermisoAdapter
{
    private readonly string _connectionString;
    private readonly ILogger<RolPermisoAdapter> _logger;

    public RolPermisoAdapter(string connectionString, ILogger<RolPermisoAdapter> logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtiene los roles asignados a un permiso
    /// SP: US_RolesPermisos_Get
    /// </summary>
    public async Task<IEnumerable<RolPermisoDto>> ObtenerRolesPermisosAsync(int permisoId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.QueryAsync<RolPermisoDto>(
                "US_RolesPermisos_Get",
                new { Permiso = permisoId },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener roles permisos. PermisoId: {PermisoId}", permisoId);
            throw;
        }
    }

    /// <summary>
    /// Agrega un rol a un permiso
    /// SP: US_RolesPermisos_Add
    /// </summary>
    public async Task<int> GuardarRolPermisoAsync(int permisoId, int rolId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.ExecuteScalarAsync<int>(
                "US_RolesPermisos_Add",
                new { Permiso = permisoId, Rol = rolId },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar rol permiso. PermisoId: {PermisoId}, RolId: {RolId}", permisoId, rolId);
            throw;
        }
    }

    /// <summary>
    /// Elimina un rol de un permiso
    /// SP: US_RolesPermisos_Del
    /// </summary>
    public async Task<bool> EliminarRolPermisoAsync(int permisoId, int rolId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(
                "US_RolesPermisos_Del",
                new { Permiso = permisoId, Rol = rolId },
                commandType: CommandType.StoredProcedure
            );

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar rol permiso. PermisoId: {PermisoId}, RolId: {RolId}", permisoId, rolId);
            throw;
        }
    }

    /// <summary>
    /// Obtiene roles para combo
    /// SP: US_Roles_Get (filtro para combo)
    /// </summary>
    public async Task<IEnumerable<RolComboDto>> ObtenerRolesComboAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            // Query directo a la tabla para el combo (más simple)
            var result = await connection.QueryAsync<RolComboDto>(
                "SELECT Id, Rol FROM US_Roles WHERE Activo = 1 ORDER BY Rol",
                commandType: CommandType.Text
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener roles combo");
            throw;
        }
    }

    /// <summary>
    /// Obtiene permisos para combo
    /// </summary>
    public async Task<IEnumerable<PermisoComboDto>> ObtenerPermisosComboAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.QueryAsync<PermisoComboDto>(
                "SELECT Id, Permiso FROM US_Permisos WHERE Activo = 1 ORDER BY Permiso",
                commandType: CommandType.Text
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener permisos combo");
            throw;
        }
    }
}
