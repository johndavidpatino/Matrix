namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para calidad de campo por ciudad
/// Mapea resultados del SP: MBO_CampoCalidadPorCiudad
/// </summary>
public class CampoCiudadDto
{
    /// <summary>ID de la ciudad</summary>
    public int IdCiudad { get; set; }

    /// <summary>Nombre de la ciudad</summary>
    public string NombreCiudad { get; set; } = string.Empty;

    /// <summary>Sigla de la unidad</summary>
    public string Sigla { get; set; } = string.Empty;

    /// <summary>Año de medición</summary>
    public int Año { get; set; }

    /// <summary>Mes de medición (1-12)</summary>
    public int Mes { get; set; }

    /// <summary>Encuestas realizadas en la ciudad</summary>
    public int EncuestasRealizadas { get; set; }

    /// <summary>Encuestas revisadas</summary>
    public int EncuestasRevisadas { get; set; }

    /// <summary>Total de errores encontrados</summary>
    public int TotalErrores { get; set; }

    /// <summary>Errores críticos</summary>
    public int ErroresCriticos { get; set; }

    /// <summary>Errores mayores</summary>
    public int ErroresMayores { get; set; }

    /// <summary>Errores menores</summary>
    public int ErroresMenores { get; set; }

    /// <summary>Encuestas sin errores</summary>
    public int EncuestasSinErrores { get; set; }

    /// <summary>Porcentaje de calidad (%)</summary>
    public decimal PorcentajeCalidad => EncuestasRevisadas > 0 
        ? Math.Round((decimal)EncuestasSinErrores / EncuestasRevisadas * 100, 2) 
        : 0;

    /// <summary>Índice de error (errores/encuesta)</summary>
    public decimal IndiceError => EncuestasRevisadas > 0 
        ? Math.Round((decimal)TotalErrores / EncuestasRevisadas, 2) 
        : 0;
}
