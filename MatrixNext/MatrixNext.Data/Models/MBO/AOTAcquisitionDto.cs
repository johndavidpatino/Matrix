namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para datos de adquisición AOT
/// Mapea resultado del SP MBO_AOTAcquisition
/// </summary>
public class AOTAcquisitionDto
{
    public string Sigla { get; set; } = string.Empty;
    public long TotalAcquisition { get; set; }
    public DateTime? FechaUltimaActualizacion { get; set; }
}
