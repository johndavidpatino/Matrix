using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.ViewModels.MBO;

/// <summary>
/// ViewModel para dashboard AOT Gerencia
/// Similar a Dirección pero con filtros por gerencia
/// </summary>
public class AOTGerenciaViewModel
{
    public int AñoSeleccionado { get; set; }
    public int MesSeleccionado { get; set; }
    public string SiglaSeleccionada { get; set; } = string.Empty;
    
    public IEnumerable<UnidadUsuarioDto> UnidadesDisponibles { get; set; } = new List<UnidadUsuarioDto>();
    
    public AOTBudgetEjecucionDto? BudgetEjecucion { get; set; }
    public AOTMetaTotalDto? MetaTotal { get; set; }
    public AOTEjecucionTotalDto? EjecucionTotal { get; set; }
    public IEnumerable<AOTUnidadDto> UnidadesDetalle { get; set; } = new List<AOTUnidadDto>();
    
    public bool TieneDatos => BudgetEjecucion != null || MetaTotal != null;
}
