namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para AOT desagregado por gerente de cuenta
/// Mapea resultado del SP MBO_PGAOTPorUnidadGerente
/// </summary>
public class AOTGerenteDto
{
    public string Unidad { get; set; } = string.Empty;
    public string NombreGerente { get; set; } = string.Empty;
    public string ApellidoGerente { get; set; } = string.Empty;
    public long BudgetGerente { get; set; }
    public long MetaGerente { get; set; }
    public long AOTGerente { get; set; }
    
    /// <summary>
    /// Nombre completo del gerente
    /// </summary>
    public string NombreCompleto => $"{NombreGerente} {ApellidoGerente}".Trim();
    
    /// <summary>
    /// Porcentaje de logro del gerente
    /// </summary>
    public decimal PorcentajeLogro => MetaGerente > 0 ? (decimal)AOTGerente / MetaGerente * 100 : 0;
}
