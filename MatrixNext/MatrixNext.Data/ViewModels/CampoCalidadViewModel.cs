using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.ViewModels;

/// <summary>
/// ViewModel para dashboard de calidad de campo
/// </summary>
public class CampoCalidadViewModel
{
    /// <summary>Año seleccionado</summary>
    public int Año { get; set; }

    /// <summary>Mes seleccionado</summary>
    public int Mes { get; set; }

    /// <summary>Sigla de la unidad seleccionada</summary>
    public string Sigla { get; set; } = string.Empty;

    /// <summary>Unidades disponibles para el usuario</summary>
    public IEnumerable<UnidadUsuarioDto> UnidadesDisponibles { get; set; } = new List<UnidadUsuarioDto>();

    /// <summary>Calidad general</summary>
    public CampoCalidadDto? CalidadGeneral { get; set; }

    /// <summary>Calidad por ciudad</summary>
    public IEnumerable<CampoCiudadDto> CalidadPorCiudad { get; set; } = new List<CampoCiudadDto>();

    /// <summary>Calidad por encuestador</summary>
    public IEnumerable<CampoEncuestadorDto> CalidadPorEncuestador { get; set; } = new List<CampoEncuestadorDto>();
}
