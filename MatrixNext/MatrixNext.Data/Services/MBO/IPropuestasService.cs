using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.Services.MBO;

/// <summary>
/// Interfaz para el servicio de Propuestas y Gestión MBO
/// Define operaciones de negocio para dashboards gerenciales
/// </summary>
public interface IPropuestasService
{
    /// <summary>
    /// Obtiene datos para el dashboard de propuestas creadas/enviadas
    /// Incluye estadísticas generales y por gerente
    /// </summary>
    Task<(IEnumerable<PropuestaEstadoDto> PorUnidad, IEnumerable<PropuestaPorGerenteDto> PorGerente)> 
        ObtenerPropuestasCreadasEnviadasAsync(string sigla);

    /// <summary>
    /// Obtiene datos para el dashboard de propuestas con alta probabilidad
    /// </summary>
    Task<IEnumerable<PropuestaAltaProbabilidadDto>> ObtenerPropuestasAltaProbabilidadAsync(string sigla);

    /// <summary>
    /// Obtiene propuestas aprobadas sin trabajo asociado
    /// Incluye vista resumida por unidad y detalle por metodología
    /// </summary>
    Task<(IEnumerable<PropuestaSinTrabajoDto> PorUnidad, IEnumerable<PropuestaSinTrabajoDto>? PorMetodologia)> 
        ObtenerPropuestasSinTrabajoAsync(string? unidadSeleccionada = null);

    /// <summary>
    /// Obtiene datos para el dashboard de gestión Matrix
    /// Muestra métricas de briefs, propuestas, presupuestos, estudios, proyectos y trabajos
    /// </summary>
    Task<GestionMatrixDto?> ObtenerGestionMatrixAsync();

    /// <summary>
    /// Obtiene índices manuales de cuentas
    /// </summary>
    Task<IEnumerable<IndiceManualDto>> ObtenerIndicesManualesAsync();

    /// <summary>
    /// Calcula el máximo de propuestas para configurar escalas de gráficos
    /// </summary>
    int CalcularMaximoPropuestas(IEnumerable<PropuestaEstadoDto> datos);

    /// <summary>
    /// Calcula el máximo de propuestas con alta probabilidad para escalas
    /// </summary>
    int CalcularMaximoPropuestasAltaProbabilidad(IEnumerable<PropuestaAltaProbabilidadDto> datos);
}
