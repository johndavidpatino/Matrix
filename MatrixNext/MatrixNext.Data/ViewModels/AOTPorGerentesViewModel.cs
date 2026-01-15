using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.ViewModels.MBO;

/// <summary>
/// ViewModel para dashboard AOT desagregado por gerentes
/// </summary>
public class AOTPorGerentesViewModel
{
    public int AñoSeleccionado { get; set; }
    public int MesSeleccionado { get; set; }
    public string SiglaSeleccionada { get; set; } = string.Empty;
    
    public IEnumerable<UnidadUsuarioDto> UnidadesDisponibles { get; set; } = new List<UnidadUsuarioDto>();
    public IEnumerable<AOTGerenteDto> GerentesDetalle { get; set; } = new List<AOTGerenteDto>();
    
    public bool TieneDatos => GerentesDetalle.Any();
}
