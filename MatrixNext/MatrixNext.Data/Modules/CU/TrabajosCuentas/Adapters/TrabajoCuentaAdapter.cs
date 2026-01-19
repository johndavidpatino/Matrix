using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using MatrixNext.Data.Modules.CU.TrabajosCuentas.Models;

namespace MatrixNext.Data.Modules.CU.TrabajosCuentas.Adapters;

/// <summary>
/// Interface para el adaptador de Trabajos de Cuenta
/// </summary>
public interface ITrabajoCuentaAdapter
{
    Task<IEnumerable<TrabajoCuentaDto>> ObtenerTrabajosAsync(TrabajoCuentaBusquedaParams? filtros = null);
    Task<TrabajoCuentaDto?> ObtenerTrabajoPorIdAsync(long id);
    Task<IEnumerable<TrabajoCuentaDto>> ObtenerTrabajosPorEstudioAsync(long estudioId);
    Task<IEnumerable<EstadoTrabajoDto>> ObtenerEstadosTrabajoAsync();
}

/// <summary>
/// Adaptador para Trabajos de Cuenta - Acceso a BD via SP CU_Trabajos_Get
/// </summary>
public class TrabajoCuentaAdapter : ITrabajoCuentaAdapter
{
    private readonly string _connectionString;
    private readonly ILogger<TrabajoCuentaAdapter> _logger;

    public TrabajoCuentaAdapter(IConfiguration configuration, ILogger<TrabajoCuentaAdapter> logger)
    {
        _connectionString = configuration.GetConnectionString("MatrixDb") 
            ?? throw new InvalidOperationException("Connection string 'MatrixDb' no encontrado");
        _logger = logger;
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<TrabajoCuentaDto>> ObtenerTrabajosAsync(TrabajoCuentaBusquedaParams? filtros = null)
    {
        using var connection = CreateConnection();
        
        var parameters = new DynamicParameters();
        parameters.Add("@ID", filtros?.Id);
        parameters.Add("@Estado", filtros?.Estado);
        parameters.Add("@NombreTrabajo", filtros?.NombreTrabajo);
        parameters.Add("@JobBook", filtros?.JobBook);
        parameters.Add("@ProyectoId", filtros?.ProyectoId);
        parameters.Add("@COE", filtros?.COE);
        parameters.Add("@GerenteCuentas", filtros?.GerenteCuentas);
        parameters.Add("@Unidad", filtros?.Unidad);
        parameters.Add("@Gerencia", filtros?.Gerencia);
        parameters.Add("@Propuesta", filtros?.Propuesta);
        parameters.Add("@EstudioId", filtros?.EstudioId);

        var trabajos = await connection.QueryAsync<TrabajoCuentaDto>(
            "CU_Trabajos_Get",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return trabajos;
    }

    public async Task<TrabajoCuentaDto?> ObtenerTrabajoPorIdAsync(long id)
    {
        using var connection = CreateConnection();
        
        var parameters = new DynamicParameters();
        parameters.Add("@ID", id);
        parameters.Add("@Estado", null);
        parameters.Add("@NombreTrabajo", null);
        parameters.Add("@JobBook", null);
        parameters.Add("@ProyectoId", null);
        parameters.Add("@COE", null);
        parameters.Add("@GerenteCuentas", null);
        parameters.Add("@Unidad", null);
        parameters.Add("@Gerencia", null);
        parameters.Add("@Propuesta", null);
        parameters.Add("@EstudioId", null);

        var trabajo = await connection.QueryFirstOrDefaultAsync<TrabajoCuentaDto>(
            "CU_Trabajos_Get",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return trabajo;
    }

    public async Task<IEnumerable<TrabajoCuentaDto>> ObtenerTrabajosPorEstudioAsync(long estudioId)
    {
        using var connection = CreateConnection();
        
        var parameters = new DynamicParameters();
        parameters.Add("@ID", null);
        parameters.Add("@Estado", null);
        parameters.Add("@NombreTrabajo", null);
        parameters.Add("@JobBook", null);
        parameters.Add("@ProyectoId", null);
        parameters.Add("@COE", null);
        parameters.Add("@GerenteCuentas", null);
        parameters.Add("@Unidad", null);
        parameters.Add("@Gerencia", null);
        parameters.Add("@Propuesta", null);
        parameters.Add("@EstudioId", estudioId);

        var trabajos = await connection.QueryAsync<TrabajoCuentaDto>(
            "CU_Trabajos_Get",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return trabajos;
    }

    public async Task<IEnumerable<EstadoTrabajoDto>> ObtenerEstadosTrabajoAsync()
    {
        using var connection = CreateConnection();
        
        var estados = await connection.QueryAsync<EstadoTrabajoDto>(
            "SELECT Id, EstadoDesc FROM PY_EstadosTrabajo ORDER BY Id"
        );

        return estados;
    }
}
