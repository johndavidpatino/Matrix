using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Modules.US.Unidades.Adapters;

/// <summary>
/// Adaptador para Unidades
/// SP: US_Unidades_Get, US_Unidades_Add, US_Unidades_Edit, US_Unidades_Del
/// Ref: CoreProject/Clases/US/Unidades.vb
/// </summary>
public interface IUnidadAdapter
{
    Task<IEnumerable<Models.UnidadDto>> ObtenerTodosAsync(string? nombre = null, int? idGrupoUnidad = null);
    Task<Models.UnidadDto?> ObtenerPorIdAsync(int id);
    Task<int> GuardarAsync(int id, string nombre, int? idGrupoUnidad);
    Task<int> EditarAsync(int id, string nombre, int? idGrupoUnidad);
    Task<int> EliminarAsync(int id);
}

public class UnidadAdapter : IUnidadAdapter
{
    private readonly string _connectionString;
    private readonly ILogger<UnidadAdapter> _logger;

    public UnidadAdapter(string connectionString, ILogger<UnidadAdapter> logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Models.UnidadDto>> ObtenerTodosAsync(string? nombre = null, int? idGrupoUnidad = null)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.QueryAsync<Models.UnidadDto>(
                "US_Unidades_Get",
                new { Unidad = nombre, IdGrupoUnidad = idGrupoUnidad },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener unidades. Nombre: {Nombre}, IdGrupoUnidad: {IdGrupoUnidad}", nombre, idGrupoUnidad);
            throw;
        }
    }

    public async Task<Models.UnidadDto?> ObtenerPorIdAsync(int id)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.QueryFirstOrDefaultAsync<Models.UnidadDto>(
                @"SELECT u.Id, u.Unidad, u.IdGrupoUnidad, g.GrupoUnidad as GrupoUnidadNombre 
                  FROM US_Unidades u 
                  LEFT JOIN US_GrupoUnidad g ON u.IdGrupoUnidad = g.Id 
                  WHERE u.Id = @Id",
                new { Id = id }
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener unidad por Id: {Id}", id);
            throw;
        }
    }

    public async Task<int> GuardarAsync(int id, string nombre, int? idGrupoUnidad)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.ExecuteAsync(
                "US_Unidades_Add",
                new { Id = id, Unidad = nombre, IdGrupoUnidad = idGrupoUnidad },
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("Unidad creada. Id: {Id}, Nombre: {Nombre}", id, nombre);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar unidad. Id: {Id}, Nombre: {Nombre}", id, nombre);
            throw;
        }
    }

    public async Task<int> EditarAsync(int id, string nombre, int? idGrupoUnidad)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.ExecuteAsync(
                "US_Unidades_Edit",
                new { Id = id, Unidad = nombre, IdGrupoUnidad = idGrupoUnidad },
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("Unidad editada. Id: {Id}, Nombre: {Nombre}", id, nombre);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al editar unidad. Id: {Id}", id);
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
                "US_Unidades_Del",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("Unidad eliminada. Id: {Id}", id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar unidad. Id: {Id}", id);
            throw;
        }
    }
}
