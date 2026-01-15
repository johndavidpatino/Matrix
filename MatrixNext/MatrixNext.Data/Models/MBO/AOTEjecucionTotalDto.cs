namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para ejecución total acumulada
/// Mapea resultado del SP MBO_PGAOTEjecucionTotal
/// </summary>
public class AOTEjecucionTotalDto
{
    public int Año { get; set; }
    public int Mes { get; set; }
    public string Sigla { get; set; } = string.Empty;
    public long AOTAcumulado { get; set; }
    public decimal PorcentajeVsMeta { get; set; }
}
