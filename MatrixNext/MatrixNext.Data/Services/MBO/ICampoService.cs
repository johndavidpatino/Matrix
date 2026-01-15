using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.Services.MBO;

/// <summary>
/// Interface para lógica de negocio del módulo Campo (MBO)
/// </summary>
public interface ICampoService
{
    /// <summary>
    /// Obtiene datos consolidados para dashboard de encuestas
    /// </summary>
    Task<(CampoEncuestaDto? encuestas, CampoCalidadDto? calidad, CampoEstadisticaDto? estadisticas)> 
        ObtenerDashboardEncuestasAsync(int año, int mes, string sigla, int usuarioId);

    /// <summary>
    /// Obtiene datos consolidados para dashboard de calidad
    /// </summary>
    Task<(CampoCalidadDto? calidad, IEnumerable<CampoCiudadDto> ciudades, IEnumerable<CampoEncuestadorDto> encuestadores)> 
        ObtenerDashboardCalidadAsync(int año, int mes, string sigla, int usuarioId);

    /// <summary>
    /// Obtiene listado de errores con filtros
    /// </summary>
    Task<IEnumerable<CampoErrorDto>> ObtenerErroresAsync(int año, int mes, string? sigla = null, int? idTrabajo = null, int? idEncuestador = null);

    /// <summary>
    /// Obtiene error específico para edición
    /// </summary>
    Task<CampoErrorDto?> ObtenerErrorPorIdAsync(int idError);

    /// <summary>
    /// Crea nuevo error de campo
    /// </summary>
    Task<(bool success, string message, int idError)> CrearErrorAsync(CampoErrorDto error, int usuarioId);

    /// <summary>
    /// Actualiza error existente
    /// </summary>
    Task<(bool success, string message)> ActualizarErrorAsync(CampoErrorDto error, int usuarioId);

    /// <summary>
    /// Elimina error de campo
    /// </summary>
    Task<(bool success, string message)> EliminarErrorAsync(int idError, int usuarioId);

    /// <summary>
    /// Carga masiva de errores desde Excel
    /// </summary>
    Task<(bool success, string message, int insertados, int errores)> 
        CargarErroresExcelAsync(IEnumerable<CampoErrorDto> errores, int usuarioId);

    /// <summary>
    /// Obtiene catálogos para dropdowns (tipos error, ciudades, encuestadores)
    /// </summary>
    Task<(IEnumerable<CampoTipoErrorDto> tiposError, IEnumerable<dynamic> ciudades, IEnumerable<dynamic> encuestadores)> 
        ObtenerCatalogosAsync(string? sigla = null, int? idCiudad = null);
}
