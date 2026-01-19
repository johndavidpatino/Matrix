using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using MatrixNext.Data.Modules.CU.Proyectos.Models;

namespace MatrixNext.Data.Modules.CU.Proyectos.Adapters;

/// <summary>
/// Interface para el adaptador de Proyectos
/// </summary>
public interface IProyectoAdapter
{
    // Proyectos
    Task<IEnumerable<ProyectoDto>> ObtenerProyectosAsync(ProyectoBusquedaParams? filtros = null);
    Task<ProyectoDto?> ObtenerProyectoPorIdAsync(long id);
    Task<ProyectoDto?> ObtenerProyectoPorJobBookAsync(string jobBook);
    Task<IEnumerable<ProyectoDto>> ObtenerProyectosPorEstudioAsync(long estudioId);
    Task<long> CrearProyectoAsync(ProyectoCreateEditDto dto);
    Task ActualizarProyectoAsync(ProyectoCreateEditDto dto);
    Task ActualizarGerenteProyectoAsync(long id, long gerenteProyectos);

    // Presupuestos
    Task<long> AgregarPresupuestoAsync(long presupuestoId, long proyectoId);
    Task EliminarPresupuestoAsync(long presupuestoId, long proyectoId);
    Task<IEnumerable<ProyectoPresupuestoDto>> ObtenerPresupuestosPorProyectoAsync(long proyectoId);

    // Catálogos
    Task<IEnumerable<TipoProyectoDto>> ObtenerTiposProyectoAsync();
    Task<IEnumerable<UnidadComboDto>> ObtenerUnidadesAsync();
}

/// <summary>
/// Adaptador para Proyectos - Acceso a BD via SP
/// </summary>
public class ProyectoAdapter : IProyectoAdapter
{
    private readonly string _connectionString;
    private readonly ILogger<ProyectoAdapter> _logger;

    public ProyectoAdapter(IConfiguration configuration, ILogger<ProyectoAdapter> logger)
    {
        _connectionString = configuration.GetConnectionString("MatrixDb") 
            ?? throw new InvalidOperationException("Connection string 'MatrixDb' no encontrado");
        _logger = logger;
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    #region Proyectos

    public async Task<IEnumerable<ProyectoDto>> ObtenerProyectosAsync(ProyectoBusquedaParams? filtros = null)
    {
        using var connection = CreateConnection();
        
        var parameters = new DynamicParameters();
        parameters.Add("@Id", filtros?.Id);
        parameters.Add("@JobBook", filtros?.JobBook);
        parameters.Add("@Nombre", filtros?.Nombre);
        parameters.Add("@Unidad", filtros?.UnidadId);
        parameters.Add("@GerenteProyectos", filtros?.GerenteProyectos);
        parameters.Add("@EstudioId", filtros?.EstudioId);
        parameters.Add("@TipoProyectoId", filtros?.TipoProyectoId);
        parameters.Add("@TodosCampos", filtros?.TodosCampos);
        parameters.Add("@GerenteCuentas", filtros?.GerenteCuentas);

        var proyectos = await connection.QueryAsync<ProyectoDto>(
            "PY_Proyectos_Get",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return proyectos;
    }

    public async Task<ProyectoDto?> ObtenerProyectoPorIdAsync(long id)
    {
        using var connection = CreateConnection();
        
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);
        parameters.Add("@JobBook", null);
        parameters.Add("@Nombre", null);
        parameters.Add("@Unidad", null);
        parameters.Add("@GerenteProyectos", null);
        parameters.Add("@EstudioId", null);
        parameters.Add("@TipoProyectoId", null);
        parameters.Add("@TodosCampos", null);
        parameters.Add("@GerenteCuentas", null);

        var proyecto = await connection.QueryFirstOrDefaultAsync<ProyectoDto>(
            "PY_Proyectos_Get",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return proyecto;
    }

    public async Task<ProyectoDto?> ObtenerProyectoPorJobBookAsync(string jobBook)
    {
        using var connection = CreateConnection();
        
        var parameters = new DynamicParameters();
        parameters.Add("@Id", null);
        parameters.Add("@JobBook", jobBook);
        parameters.Add("@Nombre", null);
        parameters.Add("@Unidad", null);
        parameters.Add("@GerenteProyectos", null);
        parameters.Add("@EstudioId", null);
        parameters.Add("@TipoProyectoId", null);
        parameters.Add("@TodosCampos", null);
        parameters.Add("@GerenteCuentas", null);

        var proyecto = await connection.QueryFirstOrDefaultAsync<ProyectoDto>(
            "PY_Proyectos_Get",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return proyecto;
    }

    public async Task<IEnumerable<ProyectoDto>> ObtenerProyectosPorEstudioAsync(long estudioId)
    {
        using var connection = CreateConnection();
        
        var parameters = new DynamicParameters();
        parameters.Add("@Id", null);
        parameters.Add("@JobBook", null);
        parameters.Add("@Nombre", null);
        parameters.Add("@Unidad", null);
        parameters.Add("@GerenteProyectos", null);
        parameters.Add("@EstudioId", estudioId);
        parameters.Add("@TipoProyectoId", null);
        parameters.Add("@TodosCampos", null);
        parameters.Add("@GerenteCuentas", null);

        var proyectos = await connection.QueryAsync<ProyectoDto>(
            "PY_Proyectos_Get",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return proyectos;
    }

    public async Task<long> CrearProyectoAsync(ProyectoCreateEditDto dto)
    {
        using var connection = CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@JobBook", dto.JobBook);
        parameters.Add("@Nombre", dto.Nombre);
        parameters.Add("@UnidadId", dto.UnidadId);
        parameters.Add("@GerenteProyectos", dto.GerenteProyectos);
        parameters.Add("@EstudioId", dto.EstudioId);
        parameters.Add("@TipoProyectoId", dto.TipoProyectoId);
        parameters.Add("@A1", dto.A1);
        parameters.Add("@A2", dto.A2);
        parameters.Add("@A3", dto.A3);
        parameters.Add("@A4", dto.A4);
        parameters.Add("@A5", dto.A5);
        parameters.Add("@A6", dto.A6);
        parameters.Add("@A7", dto.A7);

        var id = await connection.ExecuteScalarAsync<decimal>(
            "PY_Proyecto_Add",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        _logger.LogInformation("Proyecto creado con ID {ProyectoId}", id);
        return (long)id;
    }

    public async Task ActualizarProyectoAsync(ProyectoCreateEditDto dto)
    {
        using var connection = CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@Id", dto.Id);
        parameters.Add("@JobBook", dto.JobBook);
        parameters.Add("@Nombre", dto.Nombre);
        parameters.Add("@UnidadId", dto.UnidadId);
        parameters.Add("@GerenteProyectos", dto.GerenteProyectos);
        parameters.Add("@EstudioId", dto.EstudioId);
        parameters.Add("@TipoProyectoId", dto.TipoProyectoId);
        parameters.Add("@A1", dto.A1);
        parameters.Add("@A2", dto.A2);
        parameters.Add("@A3", dto.A3);
        parameters.Add("@A4", dto.A4);
        parameters.Add("@A5", dto.A5);
        parameters.Add("@A6", dto.A6);
        parameters.Add("@A7", dto.A7);

        await connection.ExecuteAsync(
            "PY_Proyectos_Edit",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        _logger.LogInformation("Proyecto actualizado: {ProyectoId}", dto.Id);
    }

    public async Task ActualizarGerenteProyectoAsync(long id, long gerenteProyectos)
    {
        using var connection = CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);
        parameters.Add("@GerenteProyectos", gerenteProyectos);

        await connection.ExecuteAsync(
            "PY_Proyectos_EditGerentePY",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        _logger.LogInformation("Gerente actualizado para proyecto {ProyectoId}: {GerenteId}", id, gerenteProyectos);
    }

    #endregion

    #region Presupuestos

    public async Task<long> AgregarPresupuestoAsync(long presupuestoId, long proyectoId)
    {
        using var connection = CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@PresupuestoId", presupuestoId);
        parameters.Add("@ProyectoId", proyectoId);

        var id = await connection.ExecuteScalarAsync<decimal>(
            "PY_Proyecto_Presupuesto_Add",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        _logger.LogInformation("Presupuesto {PresupuestoId} agregado al proyecto {ProyectoId}", presupuestoId, proyectoId);
        return (long)id;
    }

    public async Task EliminarPresupuestoAsync(long presupuestoId, long proyectoId)
    {
        using var connection = CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@PresupuestoId", presupuestoId);
        parameters.Add("@ProyectoId", proyectoId);

        await connection.ExecuteAsync(
            "PY_Proyecto_Presupuesto_Del",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        _logger.LogInformation("Presupuesto {PresupuestoId} eliminado del proyecto {ProyectoId}", presupuestoId, proyectoId);
    }

    public async Task<IEnumerable<ProyectoPresupuestoDto>> ObtenerPresupuestosPorProyectoAsync(long proyectoId)
    {
        using var connection = CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@PresupuestoId", null);
        parameters.Add("@ProyectoId", proyectoId);

        var presupuestos = await connection.QueryAsync<ProyectoPresupuestoDto>(
            "PY_Proyecto_Presupuesto_Get",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return presupuestos;
    }

    #endregion

    #region Catálogos

    public async Task<IEnumerable<TipoProyectoDto>> ObtenerTiposProyectoAsync()
    {
        using var connection = CreateConnection();
        
        var tipos = await connection.QueryAsync<TipoProyectoDto>(
            "PY_TiposProyectos_Get",
            commandType: CommandType.StoredProcedure
        );

        return tipos;
    }

    public async Task<IEnumerable<UnidadComboDto>> ObtenerUnidadesAsync()
    {
        using var connection = CreateConnection();
        
        // Usar query directo a tabla de unidades
        var unidades = await connection.QueryAsync<UnidadComboDto>(
            "SELECT Id, Unidad FROM US_Unidades WHERE Activo = 1 ORDER BY Unidad"
        );

        return unidades;
    }

    #endregion
}
