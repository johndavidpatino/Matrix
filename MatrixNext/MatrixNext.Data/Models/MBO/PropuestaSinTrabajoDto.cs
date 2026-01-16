namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para representar propuestas aprobadas sin trabajo asociado
/// Mapeado desde SPs: MBO_PropuestasAprobadasSinTrabajoPorUnidad, MBO_PropuestasAprobadasSinTrabajoUnidadMetodo
/// </summary>
public class PropuestaSinTrabajoDto
{
    /// <summary>
    /// Unidad de negocio
    /// </summary>
    public string Unidad { get; set; } = string.Empty;

    /// <summary>
    /// Número de propuestas aprobadas sin trabajo
    /// </summary>
    public int Propuestas { get; set; }

    /// <summary>
    /// Valor total del presupuesto de las propuestas
    /// </summary>
    public decimal VrPresupuesto { get; set; }

    /// <summary>
    /// Metodología de investigación (opcional, solo para detalles por metodología)
    /// </summary>
    public string? Metodologia { get; set; }
}
