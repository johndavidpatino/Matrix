namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para datos AOT desagregados por unidad
/// Mapea resultado del SP MBO_PGAOTBudgetEjecucionUnidad
/// </summary>
public class AOTUnidadDto
{
    public string Unidad { get; set; } = string.Empty;
    public string NombreUnidad { get; set; } = string.Empty;
    public long BudgetUnidad { get; set; }
    public long MetaUnidad { get; set; }
    public long ActualUnidad { get; set; }
    
    /// <summary>
    /// Porcentaje de logro de la unidad
    /// </summary>
    public decimal PorcentajeLogro => MetaUnidad > 0 ? (decimal)ActualUnidad / MetaUnidad * 100 : 0;
}
