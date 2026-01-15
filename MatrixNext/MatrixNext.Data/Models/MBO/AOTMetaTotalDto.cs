namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para meta total anual
/// Mapea resultado del SP MBO_PGAOTBudgetMetaTotal
/// </summary>
public class AOTMetaTotalDto
{
    public string Sigla { get; set; } = string.Empty;
    public long BudgetAnual { get; set; }
    public long MetaAnual { get; set; }
    public long LowerLimit { get; set; }
    public long UpperLimit { get; set; }
}
