namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para datos de Budget y Ejecución AOT (Achievement of Tasks)
/// Mapea resultado del SP MBO_PGAOTBudgetEjecucionAñoMes
/// </summary>
public class AOTBudgetEjecucionDto
{
    public int Año { get; set; }
    public int Mes { get; set; }
    public string Sigla { get; set; } = string.Empty;
    public long BudgetTotal { get; set; }
    public long MetaTotal { get; set; }
    public long AOTTotal { get; set; }
    public long BudgetAction { get; set; }
    public long MetaAction { get; set; }
    public long BudgetWatch { get; set; }
    public long MetaWatch { get; set; }
    public long BudgetBooster { get; set; }
    public long MetaBooster { get; set; }
    
    /// <summary>
    /// Porcentaje de logro total (AOTTotal / MetaTotal * 100)
    /// </summary>
    public decimal PorcentajeLogro => MetaTotal > 0 ? (decimal)AOTTotal / MetaTotal * 100 : 0;
}
