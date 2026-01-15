using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.ViewModels.MBO;

/// <summary>
/// ViewModel para dashboard AOT Dirección
/// Combina datos de múltiples SP para visualización
/// </summary>
public class AOTDireccionViewModel
{
    public int AñoSeleccionado { get; set; }
    public int MesSeleccionado { get; set; }
    public string SiglaSeleccionada { get; set; } = string.Empty;
    
    public IEnumerable<UnidadUsuarioDto> UnidadesDisponibles { get; set; } = new List<UnidadUsuarioDto>();
    
    public AOTBudgetEjecucionDto? BudgetEjecucion { get; set; }
    public AOTMetaTotalDto? MetaTotal { get; set; }
    public AOTEjecucionTotalDto? EjecucionTotal { get; set; }
    public IEnumerable<AOTUnidadDto> UnidadesDetalle { get; set; } = new List<AOTUnidadDto>();
    public AOTAcquisitionDto? Acquisition { get; set; }
    
    /// <summary>
    /// Indica si hay datos disponibles
    /// </summary>
    public bool TieneDatos => BudgetEjecucion != null || MetaTotal != null;
}
