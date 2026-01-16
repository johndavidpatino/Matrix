namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para representar el estado de propuestas creadas/enviadas por unidad
/// Mapeado desde SP: MBO_PropuestasCreadasEnviadasSinAnuncioActualizar
/// </summary>
public class PropuestaEstadoDto
{
    /// <summary>
    /// Identificador de la unidad de negocio
    /// </summary>
    public string GrupoUnidad { get; set; } = string.Empty;

    /// <summary>
    /// Total de propuestas en gestión (creadas y enviadas)
    /// </summary>
    public int PropuestasEnGestion { get; set; }

    /// <summary>
    /// Número de propuestas que requieren actualización
    /// </summary>
    public int PropuestasPorActualizar { get; set; }

    /// <summary>
    /// Porcentaje de propuestas por actualizar respecto al total
    /// </summary>
    public decimal PorcentajePorActualizar => PropuestasEnGestion > 0 
        ? Math.Round((decimal)PropuestasPorActualizar / PropuestasEnGestion * 100, 2) 
        : 0;
}
