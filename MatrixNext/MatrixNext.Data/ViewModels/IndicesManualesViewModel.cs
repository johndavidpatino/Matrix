using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.ViewModels;

/// <summary>
/// ViewModel para la vista de índices manuales de cuentas
/// Muestra 3 gráficos con diferentes criterios de medición
/// </summary>
public class IndicesManualesViewModel
{
    /// <summary>
    /// Lista de índices manuales de cuentas
    /// </summary>
    public IEnumerable<IndiceManualDto> IndicesManuales { get; set; } = new List<IndiceManualDto>();

    /// <summary>
    /// Índices agrupados para gráfico CT1
    /// </summary>
    public IEnumerable<IndiceManualDto> IndicesCT1 => IndicesManuales
        .OrderByDescending(i => i.IndiceCT1)
        .ToList();

    /// <summary>
    /// Índices agrupados para gráfico CT2
    /// </summary>
    public IEnumerable<IndiceManualDto> IndicesCT2 => IndicesManuales
        .OrderByDescending(i => i.IndiceCT2)
        .ToList();

    /// <summary>
    /// Índices agrupados para gráfico CT3
    /// </summary>
    public IEnumerable<IndiceManualDto> IndicesCT3 => IndicesManuales
        .OrderByDescending(i => i.IndiceCT3)
        .ToList();
}
