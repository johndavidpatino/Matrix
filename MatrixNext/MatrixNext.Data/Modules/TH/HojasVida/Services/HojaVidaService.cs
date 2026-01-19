using MatrixNext.Data.Modules.TH.HojasVida.Adapters;
using MatrixNext.Data.Modules.TH.HojasVida.Models;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Modules.TH.HojasVida.Services;

/// <summary>
/// Interface del servicio de Hojas de Vida
/// </summary>
public interface IHojaVidaService
{
    // Hojas de Vida
    Task<IEnumerable<HojaVidaDto>> ObtenerHojasVidaAsync(BuscarHojasVidaParams parametros);
    Task<HojaVidaDto?> ObtenerHojaVidaPorIdAsync(long id);
    Task<(bool Success, string Message, long Id)> GuardarHojaVidaAsync(HojaVidaCreateEditDto dto);
    
    // Entrevistas
    Task<IEnumerable<HojaVidaEntrevistaDto>> ObtenerEntrevistasAsync(long hojaVidaId);
    Task<(bool Success, string Message)> AgregarEntrevistaAsync(HojaVidaEntrevistaCreateDto dto);
    Task<(bool Success, string Message)> EliminarEntrevistaAsync(long id);
    
    // Experiencias Laborales
    Task<IEnumerable<HojaVidaExperienciaLaboralDto>> ObtenerExperienciasLaboralesAsync(long hojaVidaId);
    Task<(bool Success, string Message, long Id)> AgregarExperienciaLaboralAsync(HojaVidaExperienciaLaboralCreateDto dto);
    Task<(bool Success, string Message)> EliminarExperienciaLaboralAsync(long id);
    
    // Keywords
    Task<(bool Success, string Message)> AgregarKeywordAsync(long hojaVidaId, string keyword);
    Task<(bool Success, string Message)> EliminarKeywordAsync(long hojaVidaId, string keyword);
    
    // Combos
    Task<IEnumerable<ProfesionComboDto>> ObtenerProfesionesAsync();
    Task<IEnumerable<NivelEducativoComboDto>> ObtenerNivelesEducativosAsync();
    Task<IEnumerable<CiudadComboDto>> ObtenerCiudadesAsync();
    Task<IEnumerable<TipoIdentificacionComboDto>> ObtenerTiposIdentificacionAsync();
}

/// <summary>
/// Servicio de lógica de negocio para Hojas de Vida (Reclutamiento)
/// Equivalente a: WebMatrix/TH_TalentoHumano/HojasVida.aspx
/// </summary>
public class HojaVidaService : IHojaVidaService
{
    private readonly IHojaVidaAdapter _adapter;
    private readonly ILogger<HojaVidaService> _logger;

    public HojaVidaService(IHojaVidaAdapter adapter, ILogger<HojaVidaService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    #region Hojas de Vida

    /// <summary>
    /// Obtener hojas de vida con filtros
    /// </summary>
    public async Task<IEnumerable<HojaVidaDto>> ObtenerHojasVidaAsync(BuscarHojasVidaParams parametros)
    {
        try
        {
            return await _adapter.ObtenerHojasVidaAsync(parametros);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener hojas de vida con parámetros: {@Parametros}", parametros);
            return Enumerable.Empty<HojaVidaDto>();
        }
    }

    /// <summary>
    /// Obtener una hoja de vida por ID
    /// </summary>
    public async Task<HojaVidaDto?> ObtenerHojaVidaPorIdAsync(long id)
    {
        try
        {
            return await _adapter.ObtenerHojaVidaPorIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener hoja de vida con ID: {Id}", id);
            return null;
        }
    }

    /// <summary>
    /// Guardar hoja de vida (crear o actualizar)
    /// </summary>
    public async Task<(bool Success, string Message, long Id)> GuardarHojaVidaAsync(HojaVidaCreateEditDto dto)
    {
        try
        {
            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(dto.Identificacion))
            {
                return (false, "La identificación es obligatoria", 0);
            }
            
            if (string.IsNullOrWhiteSpace(dto.Nombres))
            {
                return (false, "El nombre es obligatorio", 0);
            }
            
            if (string.IsNullOrWhiteSpace(dto.Apellidos))
            {
                return (false, "Los apellidos son obligatorios", 0);
            }

            long id;
            string mensaje;
            
            if (dto.EsActualizacion && dto.Id.HasValue)
            {
                await _adapter.ActualizarHojaVidaAsync(dto);
                id = dto.Id.Value;
                mensaje = "Hoja de vida actualizada exitosamente";
                _logger.LogInformation("Hoja de vida {Id} actualizada: {Nombre} {Apellido}", 
                    id, dto.Nombres, dto.Apellidos);
            }
            else
            {
                id = await _adapter.CrearHojaVidaAsync(dto);
                mensaje = "Hoja de vida creada exitosamente";
                _logger.LogInformation("Hoja de vida {Id} creada: {Nombre} {Apellido}", 
                    id, dto.Nombres, dto.Apellidos);
            }
            
            return (true, mensaje, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar hoja de vida. Dto: {@Dto}", dto);
            return (false, "Error al guardar la hoja de vida", 0);
        }
    }

    #endregion

    #region Entrevistas

    /// <summary>
    /// Obtener entrevistas de una hoja de vida
    /// </summary>
    public async Task<IEnumerable<HojaVidaEntrevistaDto>> ObtenerEntrevistasAsync(long hojaVidaId)
    {
        try
        {
            return await _adapter.ObtenerEntrevistasAsync(hojaVidaId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener entrevistas de hoja de vida {HojaVidaId}", hojaVidaId);
            return Enumerable.Empty<HojaVidaEntrevistaDto>();
        }
    }

    /// <summary>
    /// Agregar una entrevista
    /// </summary>
    public async Task<(bool Success, string Message)> AgregarEntrevistaAsync(HojaVidaEntrevistaCreateDto dto)
    {
        try
        {
            if (dto.FechaEntrevista == default)
            {
                return (false, "La fecha de entrevista es obligatoria");
            }
            
            await _adapter.AgregarEntrevistaAsync(dto);
            
            _logger.LogInformation("Entrevista agregada a hoja de vida {HojaVidaId} para fecha {Fecha}", 
                dto.HojaVidaId, dto.FechaEntrevista);
            
            return (true, "Entrevista agregada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar entrevista a hoja de vida {HojaVidaId}", dto.HojaVidaId);
            return (false, "Error al agregar la entrevista");
        }
    }

    /// <summary>
    /// Eliminar una entrevista
    /// </summary>
    public async Task<(bool Success, string Message)> EliminarEntrevistaAsync(long id)
    {
        try
        {
            await _adapter.EliminarEntrevistaAsync(id);
            
            _logger.LogInformation("Entrevista {Id} eliminada", id);
            
            return (true, "Entrevista eliminada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar entrevista {Id}", id);
            return (false, "Error al eliminar la entrevista");
        }
    }

    #endregion

    #region Experiencias Laborales

    /// <summary>
    /// Obtener experiencias laborales de una hoja de vida
    /// </summary>
    public async Task<IEnumerable<HojaVidaExperienciaLaboralDto>> ObtenerExperienciasLaboralesAsync(long hojaVidaId)
    {
        try
        {
            return await _adapter.ObtenerExperienciasLaboralesAsync(hojaVidaId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener experiencias laborales de hoja de vida {HojaVidaId}", hojaVidaId);
            return Enumerable.Empty<HojaVidaExperienciaLaboralDto>();
        }
    }

    /// <summary>
    /// Agregar experiencia laboral
    /// </summary>
    public async Task<(bool Success, string Message, long Id)> AgregarExperienciaLaboralAsync(HojaVidaExperienciaLaboralCreateDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Empresa))
            {
                return (false, "El nombre de la empresa es obligatorio", 0);
            }
            
            if (dto.DuracionAnos <= 0)
            {
                return (false, "La duración debe ser mayor a 0", 0);
            }
            
            var id = await _adapter.AgregarExperienciaLaboralAsync(dto);
            
            _logger.LogInformation("Experiencia laboral agregada a hoja de vida {HojaVidaId}: {Empresa}", 
                dto.HojaVidaId, dto.Empresa);
            
            return (true, "Experiencia laboral agregada exitosamente", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar experiencia laboral a hoja de vida {HojaVidaId}", dto.HojaVidaId);
            return (false, "Error al agregar la experiencia laboral", 0);
        }
    }

    /// <summary>
    /// Eliminar experiencia laboral
    /// </summary>
    public async Task<(bool Success, string Message)> EliminarExperienciaLaboralAsync(long id)
    {
        try
        {
            await _adapter.EliminarExperienciaLaboralAsync(id);
            
            _logger.LogInformation("Experiencia laboral {Id} eliminada", id);
            
            return (true, "Experiencia laboral eliminada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar experiencia laboral {Id}", id);
            return (false, "Error al eliminar la experiencia laboral");
        }
    }

    #endregion

    #region Keywords

    /// <summary>
    /// Agregar keyword a hoja de vida
    /// </summary>
    public async Task<(bool Success, string Message)> AgregarKeywordAsync(long hojaVidaId, string keyword)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return (false, "La palabra clave es obligatoria");
            }
            
            await _adapter.AgregarKeywordAsync(hojaVidaId, keyword.Trim());
            
            _logger.LogInformation("Keyword '{Keyword}' agregado a hoja de vida {HojaVidaId}", 
                keyword, hojaVidaId);
            
            return (true, "Palabra clave agregada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar keyword a hoja de vida {HojaVidaId}", hojaVidaId);
            return (false, "Error al agregar la palabra clave");
        }
    }

    /// <summary>
    /// Eliminar keyword de hoja de vida
    /// </summary>
    public async Task<(bool Success, string Message)> EliminarKeywordAsync(long hojaVidaId, string keyword)
    {
        try
        {
            await _adapter.EliminarKeywordAsync(hojaVidaId, keyword);
            
            _logger.LogInformation("Keyword '{Keyword}' eliminado de hoja de vida {HojaVidaId}", 
                keyword, hojaVidaId);
            
            return (true, "Palabra clave eliminada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar keyword de hoja de vida {HojaVidaId}", hojaVidaId);
            return (false, "Error al eliminar la palabra clave");
        }
    }

    #endregion

    #region Combos

    /// <summary>
    /// Obtener profesiones para combo
    /// </summary>
    public async Task<IEnumerable<ProfesionComboDto>> ObtenerProfesionesAsync()
    {
        try
        {
            return await _adapter.ObtenerProfesionesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener profesiones");
            return Enumerable.Empty<ProfesionComboDto>();
        }
    }

    /// <summary>
    /// Obtener niveles educativos para combo
    /// </summary>
    public async Task<IEnumerable<NivelEducativoComboDto>> ObtenerNivelesEducativosAsync()
    {
        try
        {
            return await _adapter.ObtenerNivelesEducativosAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener niveles educativos");
            return Enumerable.Empty<NivelEducativoComboDto>();
        }
    }

    /// <summary>
    /// Obtener ciudades para combo
    /// </summary>
    public async Task<IEnumerable<CiudadComboDto>> ObtenerCiudadesAsync()
    {
        try
        {
            return await _adapter.ObtenerCiudadesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener ciudades");
            return Enumerable.Empty<CiudadComboDto>();
        }
    }

    /// <summary>
    /// Obtener tipos de identificación para combo
    /// </summary>
    public async Task<IEnumerable<TipoIdentificacionComboDto>> ObtenerTiposIdentificacionAsync()
    {
        try
        {
            return await _adapter.ObtenerTiposIdentificacionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tipos de identificación");
            return Enumerable.Empty<TipoIdentificacionComboDto>();
        }
    }

    #endregion
}
