using Dapper;
using MatrixNext.Data.Modules.TH.Contratistas.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MatrixNext.Data.Modules.TH.Contratistas.Adapters;

/// <summary>
/// Interfaz para el adaptador de Contratistas
/// </summary>
public interface IContratistaAdapter
{
    // Consultas
    Task<IEnumerable<ContratistaDto>> ObtenerContratistasAsync(BuscarContratistasParams parametros);
    Task<ContratistaDto?> ObtenerContratistaPorIdAsync(long identificacion);
    Task<bool> ExisteContratistaAsync(long identificacion);
    
    // CRUD Contratistas
    Task GuardarContratistaAsync(ContratistaCreateEditDto dto);
    Task ActualizarContratistaAsync(ContratistaCreateEditDto dto);
    Task ActualizarEstadoContratistaAsync(long identificacion, int estado);
    
    // Servicios de Contratista
    Task<IEnumerable<ContratistaServicioDto>> ObtenerServiciosContratistaAsync(long identificacion);
    Task AgregarServicioContratistaAsync(ContratistaServicioCreateDto dto);
    Task ActualizarEstadoServicioAsync(long id, bool estado);
    
    // Log
    Task AgregarLogContratistaAsync(long contratistaId, string observacion, long usuarioId);
    Task<IEnumerable<ContratistaLogDto>> ObtenerLogContratistasAsync(long? contratistaId, string? nombre);
    
    // Combos
    Task<IEnumerable<EstadoContratistaDto>> ObtenerEstadosAsync();
    Task<IEnumerable<ServicioContratistaComboDto>> ObtenerServiciosComboAsync(long? id);
    Task<IEnumerable<ClasificacionContratistaDto>> ObtenerClasificacionesAsync();
    Task<IEnumerable<CiudadComboDto>> ObtenerCiudadesAsync();
}

/// <summary>
/// Implementación del adaptador de Contratistas usando Dapper
/// </summary>
public class ContratistaAdapter : IContratistaAdapter
{
    private readonly string _connectionString;
    private readonly ILogger<ContratistaAdapter> _logger;

    public ContratistaAdapter(IConfiguration configuration, ILogger<ContratistaAdapter> logger)
    {
        _connectionString = configuration.GetConnectionString("MatrixDb") 
            ?? throw new InvalidOperationException("Connection string 'MatrixDb' no encontrado");
        _logger = logger;
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    #region Consultas

    public async Task<IEnumerable<ContratistaDto>> ObtenerContratistasAsync(BuscarContratistasParams parametros)
    {
        using var connection = CreateConnection();
        
        var result = await connection.QueryAsync<ContratistaDto>(
            "TH_ContratistasGet",
            new 
            { 
                Identificacion = parametros.Identificacion,
                Nombre = parametros.Nombre,
                Activo = parametros.Activo
            },
            commandType: CommandType.StoredProcedure
        );
        
        return result;
    }

    public async Task<ContratistaDto?> ObtenerContratistaPorIdAsync(long identificacion)
    {
        using var connection = CreateConnection();
        
        // Usamos el SP de listar con filtro por ID
        var result = await connection.QueryFirstOrDefaultAsync<ContratistaDto>(
            "TH_ContratistasGet",
            new { Identificacion = identificacion, Nombre = (string?)null, Activo = (bool?)null },
            commandType: CommandType.StoredProcedure
        );
        
        return result;
    }

    public async Task<bool> ExisteContratistaAsync(long identificacion)
    {
        using var connection = CreateConnection();
        
        var count = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(1) FROM TH_Contratistas WHERE Identificacion = @Identificacion",
            new { Identificacion = identificacion }
        );
        
        return count > 0;
    }

    #endregion

    #region CRUD Contratistas

    public async Task GuardarContratistaAsync(ContratistaCreateEditDto dto)
    {
        using var connection = CreateConnection();
        
        await connection.ExecuteAsync(
            "TH_ContratistasAdd",
            new
            {
                dto.Identificacion,
                dto.Nombre,
                dto.Direccion,
                dto.Email,
                Activo = dto.Estado == 1 ? 1 : 0,
                dto.CiudadId,
                dto.NumeroSymphony,
                ServicioId = 0, // Servicios se agregan aparte
                dto.DescripcionCuenta,
                dto.Telefono,
                dto.FechaRegistro,
                dto.Estado,
                dto.Solicitud,
                dto.Aprobado,
                dto.Observaciones,
                dto.Clasificacion
            },
            commandType: CommandType.StoredProcedure
        );
        
        _logger.LogInformation("Contratista {Identificacion} creado exitosamente", dto.Identificacion);
    }

    public async Task ActualizarContratistaAsync(ContratistaCreateEditDto dto)
    {
        using var connection = CreateConnection();
        
        await connection.ExecuteAsync(
            "TH_ContratistaUpdate",
            new
            {
                dto.Identificacion,
                dto.Nombre,
                dto.Direccion,
                dto.Email,
                Activo = dto.Estado == 1 ? 1 : 0,
                dto.CiudadId,
                dto.NumeroSymphony,
                ServicioId = 0,
                dto.DescripcionCuenta,
                dto.Telefono,
                dto.FechaRegistro,
                dto.Estado,
                dto.Solicitud,
                dto.Aprobado,
                dto.Observaciones,
                dto.Clasificacion
            },
            commandType: CommandType.StoredProcedure
        );
        
        _logger.LogInformation("Contratista {Identificacion} actualizado exitosamente", dto.Identificacion);
    }

    public async Task ActualizarEstadoContratistaAsync(long identificacion, int estado)
    {
        using var connection = CreateConnection();
        
        await connection.ExecuteAsync(
            "TH_ContratistaActualizarEstado",
            new { Identificacion = identificacion, Estado = estado },
            commandType: CommandType.StoredProcedure
        );
    }

    #endregion

    #region Servicios de Contratista

    public async Task<IEnumerable<ContratistaServicioDto>> ObtenerServiciosContratistaAsync(long identificacion)
    {
        using var connection = CreateConnection();
        
        var result = await connection.QueryAsync<ContratistaServicioDto>(
            "TH_ContratistasDetalleServiciosGet",
            new { IdentificacionId = identificacion },
            commandType: CommandType.StoredProcedure
        );
        
        return result;
    }

    public async Task AgregarServicioContratistaAsync(ContratistaServicioCreateDto dto)
    {
        using var connection = CreateConnection();
        
        await connection.ExecuteAsync(
            "TH_ContratistasDetalleServiciosAdd",
            new
            {
                IdentificacionId = dto.ContratistaId,
                dto.ServicioId,
                dto.NombreServicio,
                dto.Estado
            },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task ActualizarEstadoServicioAsync(long id, bool estado)
    {
        using var connection = CreateConnection();
        
        await connection.ExecuteAsync(
            "TH_ContratistasDetalleServiciosUpdate",
            new { Id = id, Estado = estado },
            commandType: CommandType.StoredProcedure
        );
    }

    #endregion

    #region Log

    public async Task AgregarLogContratistaAsync(long contratistaId, string observacion, long usuarioId)
    {
        using var connection = CreateConnection();
        
        await connection.ExecuteAsync(
            "TH_LogContratistasAdd",
            new { ContratistaId = contratistaId, Observacion = observacion, UsuarioId = usuarioId },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<IEnumerable<ContratistaLogDto>> ObtenerLogContratistasAsync(long? contratistaId, string? nombre)
    {
        using var connection = CreateConnection();
        
        var result = await connection.QueryAsync<ContratistaLogDto>(
            "TH_LogContratistaGet",
            new { ContratistaId = contratistaId, Nombre = nombre },
            commandType: CommandType.StoredProcedure
        );
        
        return result;
    }

    #endregion

    #region Combos

    public async Task<IEnumerable<EstadoContratistaDto>> ObtenerEstadosAsync()
    {
        using var connection = CreateConnection();
        
        var result = await connection.QueryAsync<EstadoContratistaDto>(
            "TH_ContratistasEstadosGet",
            commandType: CommandType.StoredProcedure
        );
        
        return result;
    }

    public async Task<IEnumerable<ServicioContratistaComboDto>> ObtenerServiciosComboAsync(long? id)
    {
        using var connection = CreateConnection();
        
        var result = await connection.QueryAsync<ServicioContratistaComboDto>(
            "TH_ServicioContratistaGet",
            new { Id = id },
            commandType: CommandType.StoredProcedure
        );
        
        return result;
    }

    public async Task<IEnumerable<ClasificacionContratistaDto>> ObtenerClasificacionesAsync()
    {
        using var connection = CreateConnection();
        
        // Consultar tabla de clasificaciones directamente
        var result = await connection.QueryAsync<ClasificacionContratistaDto>(
            "SELECT Id, Clasificacion FROM TH_ContratistasClasificacion ORDER BY Clasificacion"
        );
        
        return result;
    }

    public async Task<IEnumerable<CiudadComboDto>> ObtenerCiudadesAsync()
    {
        using var connection = CreateConnection();
        
        // Consultar ciudades 
        var result = await connection.QueryAsync<CiudadComboDto>(
            "SELECT id as Id, NombreMpio as Ciudad FROM Divipola ORDER BY NombreMpio"
        );
        
        return result;
    }

    #endregion
}
