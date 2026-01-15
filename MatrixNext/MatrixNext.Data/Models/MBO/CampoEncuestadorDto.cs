namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para calidad de campo por encuestador
/// Mapea resultados del SP: MBO_CampoCalidadPorEncuestador
/// </summary>
public class CampoEncuestadorDto
{
    /// <summary>ID del encuestador</summary>
    public int IdEncuestador { get; set; }

    /// <summary>Código del encuestador</summary>
    public string CodigoEncuestador { get; set; } = string.Empty;

    /// <summary>Nombre completo del encuestador</summary>
    public string NombreEncuestador { get; set; } = string.Empty;

    /// <summary>Ciudad asignada</summary>
    public string Ciudad { get; set; } = string.Empty;

    /// <summary>Año de medición</summary>
    public int Año { get; set; }

    /// <summary>Mes de medición (1-12)</summary>
    public int Mes { get; set; }

    /// <summary>Encuestas realizadas</summary>
    public int EncuestasRealizadas { get; set; }

    /// <summary>Encuestas revisadas</summary>
    public int EncuestasRevisadas { get; set; }

    /// <summary>Total de errores cometidos</summary>
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

    /// <summary>Calificación semáforo (Verde/Amarillo/Rojo)</summary>
    public string Semaforo => PorcentajeCalidad >= 95 ? "Verde" 
        : PorcentajeCalidad >= 85 ? "Amarillo" 
        : "Rojo";
}
