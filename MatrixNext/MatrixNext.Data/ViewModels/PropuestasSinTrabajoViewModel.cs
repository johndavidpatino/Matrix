using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.ViewModels;

/// <summary>
/// ViewModel para la vista de propuestas aprobadas sin trabajo
/// Muestra propuestas pendientes de entrega a operaciones
/// </summary>
public class PropuestasSinTrabajoViewModel
{
    /// <summary>
    /// Unidad seleccionada para filtro de detalle
    /// </summary>
    public string? UnidadSeleccionada { get; set; }

    /// <summary>
    /// Propuestas sin trabajo agrupadas por unidad
    /// </summary>
    public IEnumerable<PropuestaSinTrabajoDto> PropuestasPorUnidad { get; set; } = new List<PropuestaSinTrabajoDto>();

    /// <summary>
    /// Propuestas sin trabajo detalladas por metodología (cuando hay unidad seleccionada)
    /// </summary>
    public IEnumerable<PropuestaSinTrabajoDto>? PropuestasPorMetodologia { get; set; }
}
