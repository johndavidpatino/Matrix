using Dapper;
using MatrixNext.Data.Models.MBO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MatrixNext.Data.Adapters.MBO;

/// <summary>
/// Adapter para acceso a datos del módulo Campo (MBO)
/// 
/// NOTA DE MIGRACIÓN: Esta funcionalidad NO EXISTE en el sistema legacy (WebMatrix/CoreProject).
/// Los SP MBO_Campo* fueron inventados y no existen en la BD de producción.
/// Se mantiene la interfaz para no romper compilación, pero todas las operaciones 
/// retornan vacío/null con warning en log.
/// 
/// Si se requiere esta funcionalidad en el futuro, se debe:
/// 1. Crear los SP correspondientes en BD
/// 2. Implementar la lógica real en este adapter
/// </summary>
public class CampoAdapter : ICampoAdapter
{
    private readonly IDbConnection _connection;
    private readonly ILogger<CampoAdapter> _logger;
    private const string NOT_IMPLEMENTED_MSG = "MBO Campo: Funcionalidad no disponible - no existe en sistema legacy";

    public CampoAdapter(IDbConnection connection, ILogger<CampoAdapter> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<CampoEncuestaDto?> ObtenerEncuestasRealizadasAsync(int año, int mes, string sigla)
    {
        _logger.LogWarning(NOT_IMPLEMENTED_MSG + " (ObtenerEncuestasRealizadas)");
        return await Task.FromResult<CampoEncuestaDto?>(null);
    }

    /// <inheritdoc />
    public async Task<CampoCalidadDto?> ObtenerCalidadGeneralAsync(int año, int mes, string sigla)
    {
        _logger.LogWarning(NOT_IMPLEMENTED_MSG + " (ObtenerCalidadGeneral)");
        return await Task.FromResult<CampoCalidadDto?>(null);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CampoCiudadDto>> ObtenerCalidadPorCiudadAsync(int año, int mes, string sigla)
    {
        _logger.LogWarning(NOT_IMPLEMENTED_MSG + " (ObtenerCalidadPorCiudad)");
        return await Task.FromResult(Enumerable.Empty<CampoCiudadDto>());
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CampoEncuestadorDto>> ObtenerCalidadPorEncuestadorAsync(int año, int mes, string sigla)
    {
        _logger.LogWarning(NOT_IMPLEMENTED_MSG + " (ObtenerCalidadPorEncuestador)");
        return await Task.FromResult(Enumerable.Empty<CampoEncuestadorDto>());
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CampoErrorDto>> ObtenerErroresAsync(int año, int mes, string? sigla = null, int? idTrabajo = null, int? idEncuestador = null)
    {
        _logger.LogWarning(NOT_IMPLEMENTED_MSG + " (ObtenerErrores)");
        return await Task.FromResult(Enumerable.Empty<CampoErrorDto>());
    }

    /// <inheritdoc />
    public async Task<CampoErrorDto?> ObtenerErrorPorIdAsync(int idError)
    {
        _logger.LogWarning(NOT_IMPLEMENTED_MSG + " (ObtenerErrorPorId)");
        return await Task.FromResult<CampoErrorDto?>(null);
    }

    /// <inheritdoc />
    public async Task<int> InsertarErrorAsync(CampoErrorDto error, int usuarioId)
    {
        _logger.LogWarning(NOT_IMPLEMENTED_MSG + " (InsertarError)");
        return await Task.FromResult(0);
    }

    /// <inheritdoc />
    public async Task<bool> ActualizarErrorAsync(CampoErrorDto error, int usuarioId)
    {
        _logger.LogWarning(NOT_IMPLEMENTED_MSG + " (ActualizarError)");
        return await Task.FromResult(false);
    }

    /// <inheritdoc />
    public async Task<bool> EliminarErrorAsync(int idError, int usuarioId)
    {
        _logger.LogWarning(NOT_IMPLEMENTED_MSG + " (EliminarError)");
        return await Task.FromResult(false);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CampoTipoErrorDto>> ObtenerTiposErrorAsync(bool soloActivos = true)
    {
        _logger.LogWarning(NOT_IMPLEMENTED_MSG + " (ObtenerTiposError)");
        return await Task.FromResult(Enumerable.Empty<CampoTipoErrorDto>());
    }

    /// <inheritdoc />
    public async Task<IEnumerable<dynamic>> ObtenerCiudadesAsync(string? sigla = null)
    {
        _logger.LogWarning(NOT_IMPLEMENTED_MSG + " (ObtenerCiudades)");
        return await Task.FromResult(Enumerable.Empty<dynamic>());
    }

    /// <inheritdoc />
    public async Task<IEnumerable<dynamic>> ObtenerEncuestadoresAsync(string? sigla = null, int? idCiudad = null)
    {
        _logger.LogWarning(NOT_IMPLEMENTED_MSG + " (ObtenerEncuestadores)");
        return await Task.FromResult(Enumerable.Empty<dynamic>());
    }

    /// <inheritdoc />
    public async Task<(int insertados, int errores, string mensaje)> CargarErroresExcelAsync(
        IEnumerable<CampoErrorDto> errores, int usuarioId)
    {
        _logger.LogWarning(NOT_IMPLEMENTED_MSG + " (CargarErroresExcel)");
        return await Task.FromResult((0, errores.Count(), NOT_IMPLEMENTED_MSG));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<string>> ValidarErroresAsync(IEnumerable<CampoErrorDto> errores)
    {
        _logger.LogWarning(NOT_IMPLEMENTED_MSG + " (ValidarErrores)");
        return await Task.FromResult(new List<string> { NOT_IMPLEMENTED_MSG }.AsEnumerable());
    }

    /// <inheritdoc />
    public async Task<CampoEstadisticaDto?> ObtenerEstadisticasAsync(int año, int mes, string sigla)
    {
        _logger.LogWarning(NOT_IMPLEMENTED_MSG + " (ObtenerEstadisticas)");
        return await Task.FromResult<CampoEstadisticaDto?>(null);
    }
}
