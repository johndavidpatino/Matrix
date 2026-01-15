using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.ViewModels;

/// <summary>
/// ViewModel para gestión de errores de campo
/// </summary>
public class CampoErroresViewModel
{
    /// <summary>Año de filtro</summary>
    public int Año { get; set; }

    /// <summary>Mes de filtro</summary>
    public int Mes { get; set; }

    /// <summary>Sigla de filtro (opcional)</summary>
    public string? Sigla { get; set; }

    /// <summary>ID de trabajo de filtro (opcional)</summary>
    public int? IdTrabajo { get; set; }

    /// <summary>ID de encuestador de filtro (opcional)</summary>
    public int? IdEncuestador { get; set; }

    /// <summary>Listado de errores</summary>
    public IEnumerable<CampoErrorDto> Errores { get; set; } = new List<CampoErrorDto>();

    /// <summary>Tipos de error disponibles</summary>
    public IEnumerable<CampoTipoErrorDto> TiposError { get; set; } = new List<CampoTipoErrorDto>();

    /// <summary>Ciudades disponibles</summary>
    public IEnumerable<dynamic> Ciudades { get; set; } = new List<dynamic>();

    /// <summary>Encuestadores disponibles</summary>
    public IEnumerable<dynamic> Encuestadores { get; set; } = new List<dynamic>();
}
