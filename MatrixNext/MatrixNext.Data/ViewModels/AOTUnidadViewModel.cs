using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.ViewModels.MBO;

/// <summary>
/// ViewModel para dashboard AOT de una unidad específica
/// </summary>
public class AOTUnidadViewModel
{
    public int AñoSeleccionado { get; set; }
    public int MesSeleccionado { get; set; }
    public string SiglaSeleccionada { get; set; } = string.Empty;
    
    public UnidadUsuarioDto? UnidadInfo { get; set; }
    public AOTBudgetEjecucionDto? BudgetEjecucion { get; set; }
    public AOTMetaTotalDto? MetaTotal { get; set; }
    
    public bool TieneDatos => BudgetEjecucion != null;
}
