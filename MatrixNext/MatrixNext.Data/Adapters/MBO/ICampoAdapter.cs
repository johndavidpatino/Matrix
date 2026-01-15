using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.Adapters.MBO;

/// <summary>
/// Interface para acceso a datos del módulo Campo (MBO)
/// </summary>
public interface ICampoAdapter
{
    /// <summary>
    /// Obtiene información de encuestas realizadas en campo
    /// SP: MBO_CampoEncuestasRealizadas
    /// </summary>
    Task<CampoEncuestaDto?> ObtenerEncuestasRealizadasAsync(int año, int mes, string sigla);

    /// <summary>
    /// Obtiene indicadores de calidad general de campo
    /// SP: MBO_CampoCalidadGeneral
    /// </summary>
    Task<CampoCalidadDto?> ObtenerCalidadGeneralAsync(int año, int mes, string sigla);

    /// <summary>
    /// Obtiene calidad de campo por ciudad
    /// SP: MBO_CampoCalidadPorCiudad
    /// </summary>
    Task<IEnumerable<CampoCiudadDto>> ObtenerCalidadPorCiudadAsync(int año, int mes, string sigla);

    /// <summary>
    /// Obtiene calidad de campo por encuestador
    /// SP: MBO_CampoCalidadPorEncuestador
    /// </summary>
    Task<IEnumerable<CampoEncuestadorDto>> ObtenerCalidadPorEncuestadorAsync(int año, int mes, string sigla);

    /// <summary>
    /// Obtiene listado de errores de campo con filtros
    /// SP: MBO_CampoErroresGet
    /// </summary>
    Task<IEnumerable<CampoErrorDto>> ObtenerErroresAsync(int año, int mes, string? sigla = null, int? idTrabajo = null, int? idEncuestador = null);

    /// <summary>
    /// Obtiene un error específico por ID
    /// SP: MBO_CampoErroresGet
    /// </summary>
    Task<CampoErrorDto?> ObtenerErrorPorIdAsync(int idError);

    /// <summary>
    /// Inserta un nuevo error de campo
    /// SP: MBO_CampoErroresInsert
    /// </summary>
    Task<int> InsertarErrorAsync(CampoErrorDto error, int usuarioId);

    /// <summary>
    /// Actualiza un error de campo existente
    /// SP: MBO_CampoErroresUpdate
    /// </summary>
    Task<bool> ActualizarErrorAsync(CampoErrorDto error, int usuarioId);

    /// <summary>
    /// Elimina un error de campo
    /// SP: MBO_CampoErroresDelete
    /// </summary>
    Task<bool> EliminarErrorAsync(int idError, int usuarioId);

    /// <summary>
    /// Obtiene catálogo de tipos de error
    /// SP: MBO_CampoTiposErrorGet
    /// </summary>
    Task<IEnumerable<CampoTipoErrorDto>> ObtenerTiposErrorAsync(bool soloActivos = true);

    /// <summary>
    /// Obtiene listado de ciudades activas
    /// SP: MBO_CampoCiudadesGet
    /// </summary>
    Task<IEnumerable<dynamic>> ObtenerCiudadesAsync(string? sigla = null);

    /// <summary>
    /// Obtiene listado de encuestadores activos
    /// SP: MBO_CampoEncuestadoresGet
    /// </summary>
    Task<IEnumerable<dynamic>> ObtenerEncuestadoresAsync(string? sigla = null, int? idCiudad = null);

    /// <summary>
    /// Carga masiva de errores desde Excel
    /// SP: MBO_CampoCargarErroresExcel
    /// </summary>
    Task<(int insertados, int errores, string mensaje)> CargarErroresExcelAsync(IEnumerable<CampoErrorDto> errores, int usuarioId);

    /// <summary>
    /// Valida errores antes de carga masiva
    /// SP: MBO_CampoValidarErrores
    /// </summary>
    Task<IEnumerable<string>> ValidarErroresAsync(IEnumerable<CampoErrorDto> errores);

    /// <summary>
    /// Obtiene estadísticas generales de campo
    /// SP: MBO_CampoEstadisticasEncuestas
    /// </summary>
    Task<CampoEstadisticaDto?> ObtenerEstadisticasAsync(int año, int mes, string sigla);
}
