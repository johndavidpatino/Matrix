using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.ViewModels;

/// <summary>
/// ViewModel para dashboard de encuestas de campo
/// </summary>
public class CampoEncuestasViewModel
{
    /// <summary>Año seleccionado</summary>
    public int Año { get; set; }

    /// <summary>Mes seleccionado</summary>
    public int Mes { get; set; }

    /// <summary>Sigla de la unidad seleccionada</summary>
    public string Sigla { get; set; } = string.Empty;

    /// <summary>Unidades disponibles para el usuario</summary>
    public IEnumerable<UnidadUsuarioDto> UnidadesDisponibles { get; set; } = new List<UnidadUsuarioDto>();

    /// <summary>Datos de encuestas realizadas</summary>
    public CampoEncuestaDto? EncuestasRealizadas { get; set; }

    /// <summary>Datos de calidad general</summary>
    public CampoCalidadDto? CalidadGeneral { get; set; }

    /// <summary>Estadísticas generales</summary>
    public CampoEstadisticaDto? Estadisticas { get; set; }
}
