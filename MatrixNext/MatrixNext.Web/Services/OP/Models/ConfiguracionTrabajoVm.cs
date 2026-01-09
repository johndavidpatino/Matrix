namespace MatrixNext.Web.Services.OP.Models;

/// <summary>
/// ViewModel para configuración de fechas y tipo de recolección
/// Ref: Trabajos.aspx.vb líneas 145-195 (modal de configuración)
/// </summary>
public class ConfiguracionTrabajoVm
{
    public long TrabajoId { get; set; }
    public DateTime? FechaInicioCampo { get; set; }
    public DateTime? FechaFinCampo { get; set; }
    
    /// <summary>
    /// 1 = Presencial
    /// 2 = Telefónica
    /// 3 = Online
    /// 4 = Mixta
    /// Ref: Trabajos.aspx líneas 98-101 (ddlTipoRecoleccion)
    /// </summary>
    public int TipoRecoleccion { get; set; }
    
    public string Observaciones { get; set; } = string.Empty;
    
    // Metadatos
    public string TrabajoNombre { get; set; } = string.Empty;
    public string UnidadNegocio { get; set; } = string.Empty;
}
