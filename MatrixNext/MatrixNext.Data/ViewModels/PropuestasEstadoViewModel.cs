using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.ViewModels;

/// <summary>
/// ViewModel para la vista de propuestas por estado (creadas/enviadas)
/// Muestra gráficos de propuestas por unidad, alta probabilidad y por gerente
/// </summary>
public class PropuestasEstadoViewModel
{
    /// <summary>
    /// Sigla de unidad seleccionada ('9' para todas)
    /// </summary>
    public string Sigla { get; set; } = "9";

    /// <summary>
    /// Unidades disponibles para filtrado
    /// </summary>
    public IEnumerable<UnidadUsuarioDto> UnidadesDisponibles { get; set; } = new List<UnidadUsuarioDto>();

    /// <summary>
    /// Propuestas creadas/enviadas agrupadas por unidad
    /// </summary>
    public IEnumerable<PropuestaEstadoDto> PropuestasPorUnidad { get; set; } = new List<PropuestaEstadoDto>();

    /// <summary>
    /// Propuestas con alta probabilidad de cierre
    /// </summary>
    public IEnumerable<PropuestaAltaProbabilidadDto> PropuestasAltaProbabilidad { get; set; } = new List<PropuestaAltaProbabilidadDto>();

    /// <summary>
    /// Propuestas agrupadas por gerente de cuentas
    /// </summary>
    public IEnumerable<PropuestaPorGerenteDto> PropuestasPorGerente { get; set; } = new List<PropuestaPorGerenteDto>();

    /// <summary>
    /// Máximo de propuestas para escala de gráficos
    /// </summary>
    public int MaximoPropuestas { get; set; }

    /// <summary>
    /// Máximo de propuestas alta probabilidad para escala
    /// </summary>
    public int MaximoPropuestasAltaProbabilidad { get; set; }
}
