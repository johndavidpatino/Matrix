using MatrixNext.Data.Adapters.MBO;
using MatrixNext.Data.Models.MBO;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.MBO;

/// <summary>
/// Servicio para lógica de negocio del módulo Campo (MBO)
/// </summary>
public class CampoService : ICampoService
{
    private readonly ICampoAdapter _campoAdapter;
    private readonly IAOTAdapter _aotAdapter;
    private readonly ILogger<CampoService> _logger;

    public CampoService(
        ICampoAdapter campoAdapter,
        IAOTAdapter aotAdapter,
        ILogger<CampoService> logger)
    {
        _campoAdapter = campoAdapter;
        _aotAdapter = aotAdapter;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<(CampoEncuestaDto? encuestas, CampoCalidadDto? calidad, CampoEstadisticaDto? estadisticas)> 
        ObtenerDashboardEncuestasAsync(int año, int mes, string sigla, int usuarioId)
    {
        try
        {
            // Ejecutar queries en paralelo
            var encuestasTask = _campoAdapter.ObtenerEncuestasRealizadasAsync(año, mes, sigla);
            var calidadTask = _campoAdapter.ObtenerCalidadGeneralAsync(año, mes, sigla);
            var estadisticasTask = _campoAdapter.ObtenerEstadisticasAsync(año, mes, sigla);

            await Task.WhenAll(encuestasTask, calidadTask, estadisticasTask);

            return (
                await encuestasTask,
                await calidadTask,
                await estadisticasTask
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo dashboard encuestas. Usuario: {UsuarioId}, Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", 
                usuarioId, año, mes, sigla);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<(CampoCalidadDto? calidad, IEnumerable<CampoCiudadDto> ciudades, IEnumerable<CampoEncuestadorDto> encuestadores)> 
        ObtenerDashboardCalidadAsync(int año, int mes, string sigla, int usuarioId)
    {
        try
        {
            // Ejecutar queries en paralelo
            var calidadTask = _campoAdapter.ObtenerCalidadGeneralAsync(año, mes, sigla);
            var ciudadesTask = _campoAdapter.ObtenerCalidadPorCiudadAsync(año, mes, sigla);
            var encuestadoresTask = _campoAdapter.ObtenerCalidadPorEncuestadorAsync(año, mes, sigla);

            await Task.WhenAll(calidadTask, ciudadesTask, encuestadoresTask);

            return (
                await calidadTask,
                await ciudadesTask,
                await encuestadoresTask
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo dashboard calidad. Usuario: {UsuarioId}, Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", 
                usuarioId, año, mes, sigla);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CampoErrorDto>> ObtenerErroresAsync(int año, int mes, string? sigla = null, int? idTrabajo = null, int? idEncuestador = null)
    {
        try
        {
            return await _campoAdapter.ObtenerErroresAsync(año, mes, sigla, idTrabajo, idEncuestador);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo errores. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", año, mes, sigla);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<CampoErrorDto?> ObtenerErrorPorIdAsync(int idError)
    {
        try
        {
            return await _campoAdapter.ObtenerErrorPorIdAsync(idError);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo error por ID. IdError: {IdError}", idError);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<(bool success, string message, int idError)> CrearErrorAsync(CampoErrorDto error, int usuarioId)
    {
        try
        {
            // Validar datos
            var validaciones = await ValidarErrorAsync(error);
            if (validaciones.Any())
            {
                var mensajeError = string.Join(", ", validaciones);
                _logger.LogWarning("Validación fallida al crear error. Usuario: {UsuarioId}, Errores: {Errores}", 
                    usuarioId, mensajeError);
                return (false, $"Datos inválidos: {mensajeError}", 0);
            }

            // Crear error
            var idError = await _campoAdapter.InsertarErrorAsync(error, usuarioId);

            _logger.LogInformation("Error de campo creado. IdError: {IdError}, Usuario: {UsuarioId}", idError, usuarioId);
            return (true, "Error registrado exitosamente", idError);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando error de campo. Usuario: {UsuarioId}", usuarioId);
            return (false, "Error al registrar el error de campo", 0);
        }
    }

    /// <inheritdoc />
    public async Task<(bool success, string message)> ActualizarErrorAsync(CampoErrorDto error, int usuarioId)
    {
        try
        {
            // Validar que existe
            var errorExistente = await _campoAdapter.ObtenerErrorPorIdAsync(error.IdError);
            if (errorExistente == null)
            {
                return (false, "Error no encontrado");
            }

            // Validar datos
            var validaciones = await ValidarErrorAsync(error);
            if (validaciones.Any())
            {
                var mensajeError = string.Join(", ", validaciones);
                return (false, $"Datos inválidos: {mensajeError}");
            }

            // Actualizar
            var actualizado = await _campoAdapter.ActualizarErrorAsync(error, usuarioId);

            if (actualizado)
            {
                _logger.LogInformation("Error de campo actualizado. IdError: {IdError}, Usuario: {UsuarioId}", 
                    error.IdError, usuarioId);
                return (true, "Error actualizado exitosamente");
            }

            return (false, "No se pudo actualizar el error");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando error de campo. IdError: {IdError}, Usuario: {UsuarioId}", 
                error.IdError, usuarioId);
            return (false, "Error al actualizar el error de campo");
        }
    }

    /// <inheritdoc />
    public async Task<(bool success, string message)> EliminarErrorAsync(int idError, int usuarioId)
    {
        try
        {
            // Validar que existe
            var errorExistente = await _campoAdapter.ObtenerErrorPorIdAsync(idError);
            if (errorExistente == null)
            {
                return (false, "Error no encontrado");
            }

            // Eliminar
            var eliminado = await _campoAdapter.EliminarErrorAsync(idError, usuarioId);

            if (eliminado)
            {
                _logger.LogInformation("Error de campo eliminado. IdError: {IdError}, Usuario: {UsuarioId}", 
                    idError, usuarioId);
                return (true, "Error eliminado exitosamente");
            }

            return (false, "No se pudo eliminar el error");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando error de campo. IdError: {IdError}, Usuario: {UsuarioId}", 
                idError, usuarioId);
            return (false, "Error al eliminar el error de campo");
        }
    }

    /// <inheritdoc />
    public async Task<(bool success, string message, int insertados, int errores)> 
        CargarErroresExcelAsync(IEnumerable<CampoErrorDto> errores, int usuarioId)
    {
        try
        {
            // Validar errores antes de cargar
            var listaErrores = errores.ToList();
            
            if (!listaErrores.Any())
            {
                return (false, "No hay errores para cargar", 0, 0);
            }

            _logger.LogInformation("Iniciando carga masiva de {Cantidad} errores. Usuario: {UsuarioId}", 
                listaErrores.Count, usuarioId);

            // Validar datos
            var validacionesGlobales = await _campoAdapter.ValidarErroresAsync(listaErrores);
            if (validacionesGlobales.Any())
            {
                var mensajeError = string.Join("; ", validacionesGlobales);
                _logger.LogWarning("Validación fallida en carga masiva. Errores: {Errores}", mensajeError);
                return (false, $"Errores de validación: {mensajeError}", 0, listaErrores.Count);
            }

            // Cargar
            var (insertados, erroresCarga, mensaje) = await _campoAdapter.CargarErroresExcelAsync(listaErrores, usuarioId);

            _logger.LogInformation("Carga masiva completada. Insertados: {Insertados}, Errores: {Errores}, Usuario: {UsuarioId}", 
                insertados, erroresCarga, usuarioId);

            var success = insertados > 0;
            return (success, mensaje, insertados, erroresCarga);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en carga masiva de errores. Usuario: {UsuarioId}", usuarioId);
            return (false, "Error al procesar la carga masiva", 0, errores.Count());
        }
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<CampoTipoErrorDto> tiposError, IEnumerable<dynamic> ciudades, IEnumerable<dynamic> encuestadores)> 
        ObtenerCatalogosAsync(string? sigla = null, int? idCiudad = null)
    {
        try
        {
            // Ejecutar queries en paralelo
            var tiposErrorTask = _campoAdapter.ObtenerTiposErrorAsync(soloActivos: true);
            var ciudadesTask = _campoAdapter.ObtenerCiudadesAsync(sigla);
            var encuestadoresTask = _campoAdapter.ObtenerEncuestadoresAsync(sigla, idCiudad);

            await Task.WhenAll(tiposErrorTask, ciudadesTask, encuestadoresTask);

            return (
                await tiposErrorTask,
                await ciudadesTask,
                await encuestadoresTask
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo catálogos. Sigla: {Sigla}, IdCiudad: {IdCiudad}", sigla, idCiudad);
            throw;
        }
    }

    /// <summary>
    /// Valida datos de un error antes de crear/actualizar
    /// </summary>
    private Task<List<string>> ValidarErrorAsync(CampoErrorDto error)
    {
        var errores = new List<string>();

        if (error.IdTrabajo <= 0)
            errores.Add("Trabajo inválido");

        if (error.IdEncuestador <= 0)
            errores.Add("Encuestador inválido");

        if (error.IdCiudad <= 0)
            errores.Add("Ciudad inválida");

        if (error.FechaEncuesta == default || error.FechaEncuesta > DateTime.Now)
            errores.Add("Fecha de encuesta inválida");

        if (string.IsNullOrWhiteSpace(error.NumeroEncuesta))
            errores.Add("Número de encuesta requerido");

        if (error.IdTipoError <= 0)
            errores.Add("Tipo de error inválido");

        return Task.FromResult(errores);
    }
}
