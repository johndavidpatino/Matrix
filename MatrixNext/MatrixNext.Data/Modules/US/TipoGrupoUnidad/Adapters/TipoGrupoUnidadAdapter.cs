using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Modules.US.TipoGrupoUnidad.Adapters;

/// <summary>
/// Adaptador para Tipo de Grupo de Unidad
/// SP: US_TipoGrupoUnidad_Get, US_TipoGrupoUnidad_Add, US_TipoGrupoUnidad_Edit, US_TipoGrupoUnidad_Del
/// Ref: CoreProject/Clases/US/TipoGrupoUnidad.vb
/// </summary>
public interface ITipoGrupoUnidadAdapter
{
    Task<IEnumerable<Models.TipoGrupoUnidadDto>> ObtenerTodosAsync(string? nombre = null);
    Task<Models.TipoGrupoUnidadDto?> ObtenerPorIdAsync(int id);
    Task<int> GuardarAsync(int id, string nombre);
    Task<int> EditarAsync(int id, string nombre);
    Task<int> EliminarAsync(int id);
}

public class TipoGrupoUnidadAdapter : ITipoGrupoUnidadAdapter
{
    private readonly string _connectionString;
    private readonly ILogger<TipoGrupoUnidadAdapter> _logger;

    public TipoGrupoUnidadAdapter(string connectionString, ILogger<TipoGrupoUnidadAdapter> logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Models.TipoGrupoUnidadDto>> ObtenerTodosAsync(string? nombre = null)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.QueryAsync<Models.TipoGrupoUnidadDto>(
                "US_TipoGrupoUnidad_Get",
                new { TipoGrupoUnidad = nombre },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tipos de grupo de unidad. Nombre: {Nombre}", nombre);
            throw;
        }
    }

    public async Task<Models.TipoGrupoUnidadDto?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.QueryFirstOrDefaultAsync<Models.TipoGrupoUnidadDto>(
                "SELECT Id, TipoGrupoUnidad FROM US_TipoGrupoUnidad WHERE Id = @Id",
                new { Id = id }
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tipo de grupo de unidad por Id: {Id}", id);
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
                "US_TipoGrupoUnidad_Add",
                new { Id = id, TipoGrupoUnidad = nombre },
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("Tipo de grupo de unidad creado. Id: {Id}, Nombre: {Nombre}", id, nombre);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar tipo de grupo de unidad. Id: {Id}, Nombre: {Nombre}", id, nombre);
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
                "US_TipoGrupoUnidad_Edit",
                new { Id = id, TipoGrupoUnidad = nombre },
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("Tipo de grupo de unidad editado. Id: {Id}, Nombre: {Nombre}", id, nombre);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al editar tipo de grupo de unidad. Id: {Id}", id);
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
                "US_TipoGrupoUnidad_Del",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("Tipo de grupo de unidad eliminado. Id: {Id}", id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar tipo de grupo de unidad. Id: {Id}", id);
            throw;
        }
    }
}
