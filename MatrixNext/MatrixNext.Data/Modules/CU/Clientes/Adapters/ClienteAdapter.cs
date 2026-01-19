using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using MatrixNext.Data.Modules.CU.Clientes.Models;

namespace MatrixNext.Data.Modules.CU.Clientes.Adapters;

/// <summary>
/// Interface para el adaptador de Clientes y Contactos
/// </summary>
public interface IClienteAdapter
{
    // Clientes
    Task<IEnumerable<ClienteDto>> ObtenerClientesAsync(ClienteBusquedaParams? filtros = null);
    Task<ClienteDto?> ObtenerClientePorIdAsync(long id);
    Task<long> CrearClienteAsync(ClienteCreateEditDto dto);
    Task ActualizarClienteAsync(ClienteCreateEditDto dto);

    // Contactos
    Task<IEnumerable<ContactoDto>> ObtenerContactosPorClienteAsync(long idCliente);
    Task<ContactoDto?> ObtenerContactoPorIdAsync(long id);
    Task<long> CrearContactoAsync(ContactoCreateEditDto dto);
    Task ActualizarContactoAsync(ContactoCreateEditDto dto);

    // Catálogos geográficos
    Task<IEnumerable<PaisDto>> ObtenerPaisesAsync();
    Task<IEnumerable<DepartamentoDto>> ObtenerDepartamentosPorPaisAsync(int idPais);
    Task<IEnumerable<CiudadDto>> ObtenerCiudadesPorDepartamentoAsync(int idDepartamento);
    Task<IEnumerable<SectorDto>> ObtenerSectoresAsync();
    Task<IEnumerable<TipoClienteDto>> ObtenerTiposClienteAsync();
}

/// <summary>
/// Adaptador para Clientes y Contactos - Acceso a BD via SP
/// </summary>
public class ClienteAdapter : IClienteAdapter
{
    private readonly string _connectionString;
    private readonly ILogger<ClienteAdapter> _logger;

    public ClienteAdapter(IConfiguration configuration, ILogger<ClienteAdapter> logger)
    {
        _connectionString = configuration.GetConnectionString("MatrixDb") 
            ?? throw new InvalidOperationException("Connection string 'MatrixDb' no encontrado");
        _logger = logger;
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    #region Clientes

    public async Task<IEnumerable<ClienteDto>> ObtenerClientesAsync(ClienteBusquedaParams? filtros = null)
    {
        using var connection = CreateConnection();
        
        var parameters = new DynamicParameters();
        parameters.Add("@ID", null);
        parameters.Add("@Nit", null);
        parameters.Add("@RazonSocial", filtros?.Buscar);
        parameters.Add("@Ciudad", filtros?.IdCiudad);
        parameters.Add("@SectorID", filtros?.IdSector);

        var clientes = await connection.QueryAsync<ClienteDto>(
            "CU_Cliente_Get",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return clientes;
    }

    public async Task<ClienteDto?> ObtenerClientePorIdAsync(long id)
    {
        using var connection = CreateConnection();
        
        var parameters = new DynamicParameters();
        parameters.Add("@ID", id);
        parameters.Add("@Nit", null);
        parameters.Add("@RazonSocial", null);
        parameters.Add("@Ciudad", null);
        parameters.Add("@SectorID", null);

        var cliente = await connection.QueryFirstOrDefaultAsync<ClienteDto>(
            "CU_Cliente_Get",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return cliente;
    }

    public async Task<long> CrearClienteAsync(ClienteCreateEditDto dto)
    {
        using var connection = CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@Nit", dto.Nit);
        parameters.Add("@GrupoEconomico", dto.GrupoEconomico);
        parameters.Add("@RazonSocial", dto.RazonSocial);
        parameters.Add("@Ciudad", dto.IdCiudad);
        parameters.Add("@Apodo", dto.Apodo);
        parameters.Add("@TipoCliente", dto.IdTipoCliente);
        parameters.Add("@Direccion", dto.Direccion);
        parameters.Add("@Telefono", dto.Telefono);
        parameters.Add("@SectorID", dto.IdSector);
        parameters.Add("@Anticipo", dto.Anticipo);
        parameters.Add("@Saldo", dto.Saldo);
        parameters.Add("@Plazo", dto.Plazo);

        var id = await connection.ExecuteScalarAsync<decimal>(
            "CU_Cliente_Add",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        _logger.LogInformation("Cliente creado con ID {ClienteId}", id);
        return (long)id;
    }

    public async Task ActualizarClienteAsync(ClienteCreateEditDto dto)
    {
        using var connection = CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@ID", dto.Id);
        parameters.Add("@Nit", dto.Nit);
        parameters.Add("@GrupoEconomico", dto.GrupoEconomico);
        parameters.Add("@RazonSocial", dto.RazonSocial);
        parameters.Add("@Ciudad", dto.IdCiudad);
        parameters.Add("@Apodo", dto.Apodo);
        parameters.Add("@TipoCliente", dto.IdTipoCliente);
        parameters.Add("@Direccion", dto.Direccion);
        parameters.Add("@Telefono", dto.Telefono);
        parameters.Add("@SectorID", dto.IdSector);
        parameters.Add("@Anticipo", dto.Anticipo);
        parameters.Add("@Saldo", dto.Saldo);
        parameters.Add("@Plazo", dto.Plazo);

        await connection.ExecuteAsync(
            "CU_Cliente_Edit",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        _logger.LogInformation("Cliente actualizado: {ClienteId}", dto.Id);
    }

    #endregion

    #region Contactos

    public async Task<IEnumerable<ContactoDto>> ObtenerContactosPorClienteAsync(long idCliente)
    {
        using var connection = CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@Id", null);
        parameters.Add("@Nombre", null);
        parameters.Add("@ClienteID", idCliente);

        var contactos = await connection.QueryAsync<ContactoDto>(
            "CU_Contactos_Get",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return contactos;
    }

    public async Task<ContactoDto?> ObtenerContactoPorIdAsync(long id)
    {
        using var connection = CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);
        parameters.Add("@Nombre", null);
        parameters.Add("@ClienteID", null);

        var contacto = await connection.QueryFirstOrDefaultAsync<ContactoDto>(
            "CU_Contactos_Get",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return contacto;
    }

    public async Task<long> CrearContactoAsync(ContactoCreateEditDto dto)
    {
        using var connection = CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@Nombre", dto.Nombre);
        parameters.Add("@Telefono", dto.Telefono);
        parameters.Add("@Celular", dto.Celular);
        parameters.Add("@Email", dto.Email);
        parameters.Add("@Cargo", dto.Cargo);
        parameters.Add("@Activo", dto.Activo);
        parameters.Add("@ClienteId", dto.IdCliente);

        var id = await connection.ExecuteScalarAsync<decimal>(
            "CU_Contactos_Add",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        _logger.LogInformation("Contacto creado con ID {ContactoId} para cliente {ClienteId}", id, dto.IdCliente);
        return (long)id;
    }

    public async Task ActualizarContactoAsync(ContactoCreateEditDto dto)
    {
        using var connection = CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@ID", dto.Id);
        parameters.Add("@Nombre", dto.Nombre);
        parameters.Add("@Telefono", dto.Telefono);
        parameters.Add("@Celular", dto.Celular);
        parameters.Add("@Email", dto.Email);
        parameters.Add("@Cargo", dto.Cargo);
        parameters.Add("@Activo", dto.Activo);
        parameters.Add("@ClienteId", dto.IdCliente);

        await connection.ExecuteAsync(
            "CU_Contactos_Edit",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        _logger.LogInformation("Contacto actualizado: {ContactoId}", dto.Id);
    }

    #endregion

    #region Catálogos Geográficos

    public async Task<IEnumerable<PaisDto>> ObtenerPaisesAsync()
    {
        using var connection = CreateConnection();
        
        // Usar query directo a tabla de países
        var paises = await connection.QueryAsync<PaisDto>(
            "SELECT Id, Nombre FROM CO_Pais ORDER BY Nombre"
        );

        return paises;
    }

    public async Task<IEnumerable<DepartamentoDto>> ObtenerDepartamentosPorPaisAsync(int idPais)
    {
        using var connection = CreateConnection();
        
        var departamentos = await connection.QueryAsync<DepartamentoDto>(
            "SELECT Id, Nombre, IdPais FROM CO_Departamento WHERE IdPais = @IdPais ORDER BY Nombre",
            new { IdPais = idPais }
        );

        return departamentos;
    }

    public async Task<IEnumerable<CiudadDto>> ObtenerCiudadesPorDepartamentoAsync(int idDepartamento)
    {
        using var connection = CreateConnection();
        
        var ciudades = await connection.QueryAsync<CiudadDto>(
            "SELECT Id, Nombre, IdDepartamento FROM CO_Ciudad WHERE IdDepartamento = @IdDepartamento ORDER BY Nombre",
            new { IdDepartamento = idDepartamento }
        );

        return ciudades;
    }

    public async Task<IEnumerable<SectorDto>> ObtenerSectoresAsync()
    {
        using var connection = CreateConnection();
        
        var sectores = await connection.QueryAsync<SectorDto>(
            "SELECT Id, Nombre FROM CU_Sector WHERE Activo = 1 ORDER BY Nombre"
        );

        return sectores;
    }

    public async Task<IEnumerable<TipoClienteDto>> ObtenerTiposClienteAsync()
    {
        using var connection = CreateConnection();
        
        var tipos = await connection.QueryAsync<TipoClienteDto>(
            "SELECT Id, Nombre FROM CU_TipoCliente WHERE Activo = 1 ORDER BY Nombre"
        );

        return tipos;
    }

    #endregion
}
