using System.Data;
using Dapper;
using MatrixNext.Data.Modules.TH.HojasVida.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace MatrixNext.Data.Modules.TH.HojasVida.Adapters;

/// <summary>
/// Interface para el adapter de Hojas de Vida
/// </summary>
public interface IHojaVidaAdapter
{
    // Hojas de Vida
    Task<IEnumerable<HojaVidaDto>> ObtenerHojasVidaAsync(BuscarHojasVidaParams parametros);
    Task<HojaVidaDto?> ObtenerHojaVidaPorIdAsync(long id);
    Task<long> CrearHojaVidaAsync(HojaVidaCreateEditDto dto);
    Task ActualizarHojaVidaAsync(HojaVidaCreateEditDto dto);
    
    // Entrevistas
    Task<IEnumerable<HojaVidaEntrevistaDto>> ObtenerEntrevistasAsync(long hojaVidaId);
    Task AgregarEntrevistaAsync(HojaVidaEntrevistaCreateDto dto);
    Task EliminarEntrevistaAsync(long id);
    
    // Experiencias Laborales
    Task<IEnumerable<HojaVidaExperienciaLaboralDto>> ObtenerExperienciasLaboralesAsync(long hojaVidaId);
    Task<long> AgregarExperienciaLaboralAsync(HojaVidaExperienciaLaboralCreateDto dto);
    Task EliminarExperienciaLaboralAsync(long id);
    
    // Keywords
    Task AgregarKeywordAsync(long hojaVidaId, string keyword);
    Task EliminarKeywordAsync(long hojaVidaId, string keyword);
    
    // Combos
    Task<IEnumerable<ProfesionComboDto>> ObtenerProfesionesAsync();
    Task<IEnumerable<NivelEducativoComboDto>> ObtenerNivelesEducativosAsync();
    Task<IEnumerable<CiudadComboDto>> ObtenerCiudadesAsync();
    Task<IEnumerable<TipoIdentificacionComboDto>> ObtenerTiposIdentificacionAsync();
}

/// <summary>
/// Adapter para acceso a datos de Hojas de Vida
/// SP de CoreProject: TH_HojasVida_Get, TH_HojasVida_Add, TH_HojasVida_Update, etc.
/// </summary>
public class HojaVidaAdapter : IHojaVidaAdapter
{
    private readonly string _connectionString;

    public HojaVidaAdapter(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MatrixConnection")
            ?? throw new InvalidOperationException("La cadena de conexión 'MatrixConnection' no está configurada");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    #region Hojas de Vida

    /// <summary>
    /// Obtener hojas de vida con filtros
    /// SP: TH_HojasVida_Get
    /// </summary>
    public async Task<IEnumerable<HojaVidaDto>> ObtenerHojasVidaAsync(BuscarHojasVidaParams parametros)
    {
        using var connection = CreateConnection();
        
        var result = await connection.QueryAsync<HojaVidaDto>(
            "TH_HojasVida_Get",
            new
            {
                Nombres = parametros.Nombres,
                Apellidos = parametros.Apellidos,
                NivelIngles = parametros.NivelIngles,
                Keywords = parametros.Keywords,
                Id = parametros.Id,
                AnosExperienciaInicio = parametros.AnosExperienciaInicio,
                AnosExperienciaFin = parametros.AnosExperienciaFin,
                NivelEducativo = parametros.NivelEducativo,
                CiudadResidencia = parametros.CiudadResidencia,
                TieneEntrevista = parametros.TieneEntrevista,
                Profesion = parametros.Profesion
            },
            commandType: CommandType.StoredProcedure
        );
        
        return result;
    }

    /// <summary>
    /// Obtener una hoja de vida por ID
    /// SP: TH_HojasVida_Get con Id
    /// </summary>
    public async Task<HojaVidaDto?> ObtenerHojaVidaPorIdAsync(long id)
    {
        using var connection = CreateConnection();
        
        var result = await connection.QueryFirstOrDefaultAsync<HojaVidaDto>(
            "TH_HojasVida_Get",
            new
            {
                Nombres = (string?)null,
                Apellidos = (string?)null,
                NivelIngles = (byte?)null,
                Keywords = (string?)null,
                Id = id,
                AnosExperienciaInicio = (byte?)null,
                AnosExperienciaFin = (byte?)null,
                NivelEducativo = (short?)null,
                CiudadResidencia = (short?)null,
                TieneEntrevista = (bool?)null,
                Profesion = (short?)null
            },
            commandType: CommandType.StoredProcedure
        );
        
        return result;
    }

    /// <summary>
    /// Crear nueva hoja de vida
    /// SP: TH_HojasVida_Add
    /// </summary>
    public async Task<long> CrearHojaVidaAsync(HojaVidaCreateEditDto dto)
    {
        using var connection = CreateConnection();
        
        var result = await connection.QueryFirstOrDefaultAsync<long?>(
            "TH_HojasVida_Add",
            new
            {
                TipoIdentificacion = dto.TipoIdentificacion,
                Identificacion = dto.Identificacion,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Edad = dto.Edad,
                AnosExperiencia = dto.AnosExperiencia,
                NivelIngles = dto.NivelIngles,
                NumeroCelular = dto.NumeroCelular,
                Correo = dto.Correo,
                Direccion = (string?)null,
                CiudadResidencia = dto.CiudadResidencia,
                NivelEducativo = dto.NivelEducativo,
                FechaCreacion = DateTime.UtcNow.AddHours(-5),
                Profesion = dto.Profesion
            },
            commandType: CommandType.StoredProcedure
        );
        
        return result ?? 0;
    }

    /// <summary>
    /// Actualizar hoja de vida existente
    /// SP: TH_HojasVida_Update
    /// </summary>
    public async Task ActualizarHojaVidaAsync(HojaVidaCreateEditDto dto)
    {
        if (!dto.Id.HasValue)
            throw new ArgumentException("El ID es requerido para actualizar", nameof(dto));
            
        using var connection = CreateConnection();
        
        await connection.ExecuteAsync(
            "TH_HojasVida_Update",
            new
            {
                Id = dto.Id.Value,
                TipoIdentificacion = dto.TipoIdentificacion,
                Identificacion = dto.Identificacion,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Edad = dto.Edad,
                AnosExperiencia = dto.AnosExperiencia,
                NivelIngles = dto.NivelIngles,
                NumeroCelular = dto.NumeroCelular,
                Correo = dto.Correo,
                Direccion = (string?)null,
                @AnosExperiencia2 = dto.AnosExperiencia,
                CiudadResidencia = dto.CiudadResidencia,
                NivelEducativo = dto.NivelEducativo,
                Profesion = dto.Profesion
            },
            commandType: CommandType.StoredProcedure
        );
    }

    #endregion

    #region Entrevistas

    /// <summary>
    /// Obtener entrevistas de una hoja de vida
    /// SP: TH_HojasVida_Entrevistas_Get
    /// </summary>
    public async Task<IEnumerable<HojaVidaEntrevistaDto>> ObtenerEntrevistasAsync(long hojaVidaId)
    {
        using var connection = CreateConnection();
        
        return await connection.QueryAsync<HojaVidaEntrevistaDto>(
            "TH_HojasVida_Entrevistas_Get",
            new { HojaVidaId = hojaVidaId },
            commandType: CommandType.StoredProcedure
        );
    }

    /// <summary>
    /// Agregar entrevista
    /// SP: TH_HojasVida_Entrevistas_Add
    /// </summary>
    public async Task AgregarEntrevistaAsync(HojaVidaEntrevistaCreateDto dto)
    {
        using var connection = CreateConnection();
        
        await connection.ExecuteAsync(
            "TH_HojasVida_Entrevistas_Add",
            new
            {
                HojasVidaId = dto.HojaVidaId,
                FechaEntrevista = dto.FechaEntrevista,
                Observaciones = dto.Observaciones
            },
            commandType: CommandType.StoredProcedure
        );
    }

    /// <summary>
    /// Eliminar entrevista
    /// SP: TH_HojasVida_Entrevistas_Delete
    /// </summary>
    public async Task EliminarEntrevistaAsync(long id)
    {
        using var connection = CreateConnection();
        
        await connection.ExecuteAsync(
            "TH_HojasVida_Entrevistas_Delete",
            new { Id = id },
            commandType: CommandType.StoredProcedure
        );
    }

    #endregion

    #region Experiencias Laborales

    /// <summary>
    /// Obtener experiencias laborales de una hoja de vida
    /// SP: TH_HojasVida_ExperienciasLaborales_Get
    /// </summary>
    public async Task<IEnumerable<HojaVidaExperienciaLaboralDto>> ObtenerExperienciasLaboralesAsync(long hojaVidaId)
    {
        using var connection = CreateConnection();
        
        return await connection.QueryAsync<HojaVidaExperienciaLaboralDto>(
            "TH_HojasVida_ExperienciasLaborales_Get",
            new { HojasVidaId = hojaVidaId },
            commandType: CommandType.StoredProcedure
        );
    }

    /// <summary>
    /// Agregar experiencia laboral
    /// SP: TH_HojasVida_ExperienciasLaborales_Add
    /// </summary>
    public async Task<long> AgregarExperienciaLaboralAsync(HojaVidaExperienciaLaboralCreateDto dto)
    {
        using var connection = CreateConnection();
        
        var result = await connection.QueryFirstOrDefaultAsync<long?>(
            "TH_HojasVida_ExperienciasLaborales_Add",
            new
            {
                HojasVidaId = dto.HojaVidaId,
                Empresa = dto.Empresa,
                DuracionAnos = dto.DuracionAnos
            },
            commandType: CommandType.StoredProcedure
        );
        
        return result ?? 0;
    }

    /// <summary>
    /// Eliminar experiencia laboral
    /// SP: TH_HojasVida_ExperienciasLaborales_Del
    /// </summary>
    public async Task EliminarExperienciaLaboralAsync(long id)
    {
        using var connection = CreateConnection();
        
        await connection.ExecuteAsync(
            "TH_HojasVida_ExperienciasLaborales_Del",
            new { Id = id },
            commandType: CommandType.StoredProcedure
        );
    }

    #endregion

    #region Keywords

    /// <summary>
    /// Agregar keyword a hoja de vida
    /// SP: TH_HojasVida_Keywords_Add
    /// </summary>
    public async Task AgregarKeywordAsync(long hojaVidaId, string keyword)
    {
        using var connection = CreateConnection();
        
        await connection.ExecuteAsync(
            "TH_HojasVida_Keywords_Add",
            new { Id = hojaVidaId, Keyword = keyword },
            commandType: CommandType.StoredProcedure
        );
    }

    /// <summary>
    /// Eliminar keyword de hoja de vida
    /// SP: TH_HojasVida_Keywords_Delete
    /// </summary>
    public async Task EliminarKeywordAsync(long hojaVidaId, string keyword)
    {
        using var connection = CreateConnection();
        
        await connection.ExecuteAsync(
            "TH_HojasVida_Keywords_Delete",
            new { HojasVidaId = hojaVidaId, Keyword = keyword },
            commandType: CommandType.StoredProcedure
        );
    }

    #endregion

    #region Combos

    /// <summary>
    /// Obtener profesiones para combo
    /// SP: TH_HojasVida_Profesiones_Get
    /// </summary>
    public async Task<IEnumerable<ProfesionComboDto>> ObtenerProfesionesAsync()
    {
        using var connection = CreateConnection();
        
        return await connection.QueryAsync<ProfesionComboDto>(
            "TH_HojasVida_Profesiones_Get",
            commandType: CommandType.StoredProcedure
        );
    }

    /// <summary>
    /// Obtener niveles educativos para combo
    /// Tabla: TH_NivelesEducativos
    /// </summary>
    public async Task<IEnumerable<NivelEducativoComboDto>> ObtenerNivelesEducativosAsync()
    {
        using var connection = CreateConnection();
        
        return await connection.QueryAsync<NivelEducativoComboDto>(
            "SELECT id as Id, NivelEducativo FROM TH_NivelesEducativos ORDER BY id"
        );
    }

    /// <summary>
    /// Obtener ciudades para combo
    /// Tabla: TH_Ciudades
    /// </summary>
    public async Task<IEnumerable<CiudadComboDto>> ObtenerCiudadesAsync()
    {
        using var connection = CreateConnection();
        
        return await connection.QueryAsync<CiudadComboDto>(
            "SELECT id as Id, Ciudad FROM TH_Ciudades ORDER BY Ciudad"
        );
    }

    /// <summary>
    /// Obtener tipos de identificación para combo
    /// Tabla: TH_TipoIdentificacion
    /// </summary>
    public async Task<IEnumerable<TipoIdentificacionComboDto>> ObtenerTiposIdentificacionAsync()
    {
        using var connection = CreateConnection();
        
        return await connection.QueryAsync<TipoIdentificacionComboDto>(
            "SELECT id as Id, TipoIdentificacion FROM TH_TipoIdentificacion ORDER BY id"
        );
    }

    #endregion
}
