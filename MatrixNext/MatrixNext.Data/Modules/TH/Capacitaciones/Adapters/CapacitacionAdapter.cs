using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MatrixNext.Data.Modules.TH.Capacitaciones.Models;

namespace MatrixNext.Data.Modules.TH.Capacitaciones.Adapters;

public interface ICapacitacionAdapter
{
    Task<IEnumerable<CapacitacionDto>> ObtenerCapacitacionesAsync(long? id = null, long? trabajoId = null);
    Task<CapacitacionDto?> ObtenerCapacitacionPorIdAsync(long id);
    Task<long> GuardarCapacitacionAsync(CapacitacionCreateEditDto dto);
    Task<bool> EliminarCapacitacionAsync(long id);
    Task<long> CrearRefuerzoAsync(long capacitacionId);
    
    // Participantes
    Task<IEnumerable<CapacitacionParticipanteDto>> ObtenerParticipantesAsync(long capacitacionId);
    Task<bool> AgregarParticipanteAsync(CapacitacionParticipanteCreateDto dto);
    Task<bool> ActualizarParticipanteAsync(CapacitacionParticipanteUpdateDto dto);
    Task<bool> EliminarParticipanteAsync(long participanteId);
    
    // Búsqueda de personas
    Task<IEnumerable<PersonaCapacitacionDto>> BuscarPersonasAsync(BuscarPersonasCapacitacionParams parametros);
    
    // Combos
    Task<IEnumerable<ResponsableComboDto>> ObtenerResponsablesAsync();
}

public class CapacitacionAdapter : ICapacitacionAdapter
{
    private readonly string _connectionString;

    public CapacitacionAdapter(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("aboraboraaboramaborar") 
            ?? throw new InvalidOperationException("Connection string 'MatrixConnection' not found.");
    }

    public async Task<IEnumerable<CapacitacionDto>> ObtenerCapacitacionesAsync(long? id = null, long? trabajoId = null)
    {
        using var connection = new SqlConnection(_connectionString);
        
        var result = await connection.QueryAsync<CapacitacionDto>(
            "TH_Capacitaciones_Get",
            new { ID = id, TrabajoID = trabajoId },
            commandType: CommandType.StoredProcedure
        );
        
        return result;
    }

    public async Task<CapacitacionDto?> ObtenerCapacitacionPorIdAsync(long id)
    {
        var capacitaciones = await ObtenerCapacitacionesAsync(id: id);
        return capacitaciones.FirstOrDefault();
    }

    public async Task<long> GuardarCapacitacionAsync(CapacitacionCreateEditDto dto)
    {
        using var connection = new SqlConnection(_connectionString);
        
        if (dto.Id > 0)
        {
            // Editar
            await connection.ExecuteAsync(
                "TH_Capacitaciones_Edit",
                new
                {
                    ID = dto.Id,
                    Ubicacion = dto.Ubicacion,
                    Fecha = dto.Fecha,
                    Duracion = dto.Duracion,
                    Actividad = dto.Actividad,
                    Responsable = dto.ResponsableId,
                    Capacitador = dto.Capacitador,
                    ObjetivoActividad = dto.ObjetivoActividad,
                    ModoEvaluacion = dto.ModoEvaluacion,
                    TrabajoId = dto.TrabajoId
                },
                commandType: CommandType.StoredProcedure
            );
            return dto.Id;
        }
        else
        {
            // Crear
            var result = await connection.QuerySingleAsync<decimal>(
                "TH_Capacitaciones_Add",
                new
                {
                    Ubicacion = dto.Ubicacion,
                    Fecha = dto.Fecha,
                    Duracion = dto.Duracion,
                    Actividad = dto.Actividad,
                    Responsable = dto.ResponsableId,
                    Capacitador = dto.Capacitador,
                    ObjetivoActividad = dto.ObjetivoActividad,
                    ModoEvaluacion = dto.ModoEvaluacion,
                    TrabajoId = dto.TrabajoId
                },
                commandType: CommandType.StoredProcedure
            );
            return (long)result;
        }
    }

    public async Task<bool> EliminarCapacitacionAsync(long id)
    {
        using var connection = new SqlConnection(_connectionString);
        
        await connection.ExecuteAsync(
            "TH_Capacitaciones_Del",
            new { ID = id },
            commandType: CommandType.StoredProcedure
        );
        
        return true;
    }

    public async Task<long> CrearRefuerzoAsync(long capacitacionId)
    {
        using var connection = new SqlConnection(_connectionString);
        
        var result = await connection.QuerySingleAsync<decimal>(
            "TH_Capacitaciones_AddRefuerzo",
            new { ID = capacitacionId },
            commandType: CommandType.StoredProcedure
        );
        
        return (long)result;
    }

    public async Task<IEnumerable<CapacitacionParticipanteDto>> ObtenerParticipantesAsync(long capacitacionId)
    {
        using var connection = new SqlConnection(_connectionString);
        
        var result = await connection.QueryAsync<CapacitacionParticipanteDto>(
            "TH_CapacitacionParticipantes_Get",
            new { CapacitacionId = capacitacionId },
            commandType: CommandType.StoredProcedure
        );
        
        return result;
    }

    public async Task<bool> AgregarParticipanteAsync(CapacitacionParticipanteCreateDto dto)
    {
        using var connection = new SqlConnection(_connectionString);
        
        await connection.ExecuteAsync(
            "TH_CapacitacionesParticipantes_Add",
            new
            {
                Participante = dto.ParticipanteId,
                CapacitacionId = dto.CapacitacionId,
                Eficacia = dto.Eficacia,
                OportunidadMejora = dto.OportunidadMejora,
                Aprobo = dto.Aprobo
            },
            commandType: CommandType.StoredProcedure
        );
        
        return true;
    }

    public async Task<bool> ActualizarParticipanteAsync(CapacitacionParticipanteUpdateDto dto)
    {
        using var connection = new SqlConnection(_connectionString);
        
        await connection.ExecuteAsync(
            "TH_CapacitacionesParticipantes_Edit",
            new
            {
                ID = dto.Id,
                CapacitacionId = dto.CapacitacionId,
                Participante = dto.ParticipanteId,
                Eficacia = dto.Eficacia,
                OportunidadMejora = dto.OportunidadMejora,
                Aprobo = dto.Aprobo
            },
            commandType: CommandType.StoredProcedure
        );
        
        return true;
    }

    public async Task<bool> EliminarParticipanteAsync(long participanteId)
    {
        using var connection = new SqlConnection(_connectionString);
        
        await connection.ExecuteAsync(
            "TH_CapacitacionesParticipantes_Del",
            new { ParticipantId = participanteId },
            commandType: CommandType.StoredProcedure
        );
        
        return true;
    }

    public async Task<IEnumerable<PersonaCapacitacionDto>> BuscarPersonasAsync(BuscarPersonasCapacitacionParams parametros)
    {
        using var connection = new SqlConnection(_connectionString);
        
        var result = await connection.QueryAsync<PersonaCapacitacionDto>(
            "TH_CapacitacionPersonas_Get",
            new
            {
                Identificacion = parametros.Identificacion,
                Nombre = parametros.Nombre,
                ContratistaId = parametros.ContratistaId,
                NombreContratista = parametros.NombreContratista,
                CapacitacionId = parametros.CapacitacionId,
                SonParticipantes = parametros.SonParticipantes,
                Page = parametros.Page,
                PageSize = parametros.PageSize
            },
            commandType: CommandType.StoredProcedure
        );
        
        return result;
    }

    public async Task<IEnumerable<ResponsableComboDto>> ObtenerResponsablesAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        
        // Usar SP de usuarios activos con rol de responsable
        var result = await connection.QueryAsync<ResponsableComboDto>(
            @"SELECT Id, CONCAT(Nombres, ' ', Apellidos) as Nombre 
              FROM US_Usuarios 
              WHERE Activo = 1 
              ORDER BY Nombres, Apellidos"
        );
        
        return result;
    }
}
