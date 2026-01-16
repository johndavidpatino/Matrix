using MatrixNext.Data.Adapters.MBO;
using MatrixNext.Data.Models.MBO;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.MBO;

/// <summary>
/// Implementación del servicio de Propuestas y Gestión MBO
/// Coordina la lógica de negocio para dashboards gerenciales
/// </summary>
public class PropuestasService : IPropuestasService
{
    private readonly IPropuestasAdapter _adapter;
    private readonly ILogger<PropuestasService> _logger;

    public PropuestasService(
        IPropuestasAdapter adapter,
        ILogger<PropuestasService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<(IEnumerable<PropuestaEstadoDto> PorUnidad, IEnumerable<PropuestaPorGerenteDto> PorGerente)> 
        ObtenerPropuestasCreadasEnviadasAsync(string sigla)
    {
        try
        {
            _logger.LogInformation("Obteniendo propuestas creadas/enviadas para sigla: {Sigla}", sigla);

            // Ejecutar ambas consultas en paralelo para mejor rendimiento
            var taskPorUnidad = _adapter.ObtenerPropuestasCreadasEnviadasAsync(sigla);
            var taskPorGerente = _adapter.ObtenerPropuestasPorGerenteAsync(sigla);

            await Task.WhenAll(taskPorUnidad, taskPorGerente);

            var porUnidad = await taskPorUnidad;
            var porGerente = await taskPorGerente;

            _logger.LogInformation(
                "Propuestas obtenidas: {UnidadesCount} unidades, {GerentesCount} gerentes",
                porUnidad.Count(), porGerente.Count());

            return (porUnidad, porGerente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener propuestas creadas/enviadas para sigla: {Sigla}", sigla);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PropuestaAltaProbabilidadDto>> ObtenerPropuestasAltaProbabilidadAsync(string sigla)
    {
        try
        {
            _logger.LogInformation("Obteniendo propuestas alta probabilidad para sigla: {Sigla}", sigla);

            var propuestas = await _adapter.ObtenerPropuestasAltaProbabilidadAsync(sigla);

            _logger.LogInformation(
                "Propuestas alta probabilidad obtenidas: {Count} registros",
                propuestas.Count());

            return propuestas;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener propuestas alta probabilidad para sigla: {Sigla}", sigla);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<(IEnumerable<PropuestaSinTrabajoDto> PorUnidad, IEnumerable<PropuestaSinTrabajoDto>? PorMetodologia)> 
        ObtenerPropuestasSinTrabajoAsync(string? unidadSeleccionada = null)
    {
        try
        {
            _logger.LogInformation("Obteniendo propuestas sin trabajo. Unidad seleccionada: {Unidad}", unidadSeleccionada ?? "Todas");

            // Siempre obtener resumen por unidad
            var porUnidad = await _adapter.ObtenerPropuestasSinTrabajoPorUnidadAsync();

            // Si hay unidad seleccionada, obtener detalle por metodología
            IEnumerable<PropuestaSinTrabajoDto>? porMetodologia = null;
            if (!string.IsNullOrEmpty(unidadSeleccionada))
            {
                porMetodologia = await _adapter.ObtenerPropuestasSinTrabajoPorMetodologiaAsync(unidadSeleccionada);
                
                _logger.LogInformation(
                    "Propuestas sin trabajo obtenidas: {UnidadesCount} unidades, {MetodologiasCount} metodologías",
                    porUnidad.Count(), porMetodologia.Count());
            }
            else
            {
                _logger.LogInformation(
                    "Propuestas sin trabajo obtenidas: {UnidadesCount} unidades",
                    porUnidad.Count());
            }

            return (porUnidad, porMetodologia);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener propuestas sin trabajo. Unidad: {Unidad}", unidadSeleccionada ?? "Todas");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<GestionMatrixDto?> ObtenerGestionMatrixAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo datos de gestión Matrix");

            var gestion = await _adapter.ObtenerGestionMatrixAsync();

            if (gestion != null)
            {
                _logger.LogInformation(
                    "Gestión Matrix obtenida: {Briefs} briefs, {Propuestas} propuestas, {Trabajos} trabajos",
                    gestion.Brief, gestion.Propuestas, gestion.Trabajos);
            }
            else
            {
                _logger.LogWarning("No se obtuvieron datos de gestión Matrix");
            }

            return gestion;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener datos de gestión Matrix");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<IndiceManualDto>> ObtenerIndicesManualesAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo índices manuales");

            var indices = await _adapter.ObtenerIndicesManualesAsync();

            _logger.LogInformation("Índices manuales obtenidos: {Count} registros", indices.Count());

            return indices;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener índices manuales");
            throw;
        }
    }

    /// <inheritdoc/>
    public int CalcularMaximoPropuestas(IEnumerable<PropuestaEstadoDto> datos)
    {
        if (!datos.Any())
            return 0;

        return datos.Max(d => d.PropuestasEnGestion);
    }

    /// <inheritdoc/>
    public int CalcularMaximoPropuestasAltaProbabilidad(IEnumerable<PropuestaAltaProbabilidadDto> datos)
    {
        if (!datos.Any())
            return 0;

        return datos.Max(d => d.TPropuestas);
    }
}
