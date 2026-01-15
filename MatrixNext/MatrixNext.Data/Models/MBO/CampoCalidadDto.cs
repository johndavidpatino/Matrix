namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para indicadores de calidad de campo
/// Mapea resultados del SP: MBO_CampoCalidadGeneral
/// </summary>
public class CampoCalidadDto
{
    /// <summary>Año de medición</summary>
    public int Año { get; set; }

    /// <summary>Mes de medición (1-12)</summary>
    public int Mes { get; set; }

    /// <summary>Sigla de la unidad</summary>
    public string Sigla { get; set; } = string.Empty;

    /// <summary>Total de encuestas revisadas</summary>
    public int EncuestasRevisadas { get; set; }

    /// <summary>Total de errores encontrados</summary>
    public int TotalErrores { get; set; }

    /// <summary>Errores críticos (nivel 1)</summary>
    public int ErroresCriticos { get; set; }

    /// <summary>Errores mayores (nivel 2)</summary>
    public int ErroresMayores { get; set; }

    /// <summary>Errores menores (nivel 3)</summary>
    public int ErroresMenores { get; set; }

    /// <summary>Meta de calidad (% encuestas sin errores)</summary>
    public decimal MetaCalidad { get; set; }

    /// <summary>Encuestas sin errores</summary>
    public int EncuestasSinErrores { get; set; }

    /// <summary>Porcentaje de calidad alcanzado (%)</summary>
    public decimal PorcentajeCalidad => EncuestasRevisadas > 0 
        ? Math.Round((decimal)EncuestasSinErrores / EncuestasRevisadas * 100, 2) 
        : 0;

    /// <summary>Índice de error promedio (errores/encuesta)</summary>
    public decimal IndiceError => EncuestasRevisadas > 0 
        ? Math.Round((decimal)TotalErrores / EncuestasRevisadas, 2) 
        : 0;

    /// <summary>Cumple meta de calidad</summary>
    public bool CumpleMeta => PorcentajeCalidad >= MetaCalidad;
}
