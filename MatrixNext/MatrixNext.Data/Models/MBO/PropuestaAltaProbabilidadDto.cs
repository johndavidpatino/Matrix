namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para representar propuestas con alta probabilidad de cierre
/// Mapeado desde SP: MBO_PropuestasAltaProbabilidadPorActualizar
/// </summary>
public class PropuestaAltaProbabilidadDto
{
    /// <summary>
    /// Identificador de la unidad de negocio
    /// </summary>
    public string GrupoUnidad { get; set; } = string.Empty;

    /// <summary>
    /// Total de propuestas con alta probabilidad
    /// </summary>
    public int TPropuestas { get; set; }

    /// <summary>
    /// Número de propuestas sin actualizar
    /// </summary>
    public int NSinActualizar { get; set; }

    /// <summary>
    /// Porcentaje de propuestas sin actualizar
    /// </summary>
    public decimal PorcentajeSinActualizar => TPropuestas > 0 
        ? Math.Round((decimal)NSinActualizar / TPropuestas * 100, 2) 
        : 0;
}
