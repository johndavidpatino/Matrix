namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para información de encuestas realizadas en campo
/// Mapea resultados del SP: MBO_CampoEncuestasRealizadas
/// </summary>
public class CampoEncuestaDto
{
    /// <summary>Año de la encuesta</summary>
    public int Año { get; set; }

    /// <summary>Mes de la encuesta (1-12)</summary>
    public int Mes { get; set; }

    /// <summary>Sigla de la unidad</summary>
    public string Sigla { get; set; } = string.Empty;

    /// <summary>Meta de encuestas del mes</summary>
    public int MetaEncuestas { get; set; }

    /// <summary>Encuestas realizadas en el mes</summary>
    public int EncuestasRealizadas { get; set; }

    /// <summary>Encuestas acumuladas en el año</summary>
    public int EncuestasAcumuladas { get; set; }

    /// <summary>Meta acumulada del año</summary>
    public int MetaAcumulada { get; set; }

    /// <summary>Porcentaje de logro del mes (%)</summary>
    public decimal PorcentajeLogroMes => MetaEncuestas > 0 
        ? Math.Round((decimal)EncuestasRealizadas / MetaEncuestas * 100, 2) 
        : 0;

    /// <summary>Porcentaje de logro acumulado (%)</summary>
    public decimal PorcentajeLogroAcumulado => MetaAcumulada > 0 
        ? Math.Round((decimal)EncuestasAcumuladas / MetaAcumulada * 100, 2) 
        : 0;
}
