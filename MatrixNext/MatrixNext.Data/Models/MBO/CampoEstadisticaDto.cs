namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para estadísticas generales de campo
/// Mapea resultados del SP: MBO_CampoEstadisticasEncuestas
/// </summary>
public class CampoEstadisticaDto
{
    /// <summary>Año de estadística</summary>
    public int Año { get; set; }

    /// <summary>Mes de estadística (1-12)</summary>
    public int Mes { get; set; }

    /// <summary>Sigla de la unidad</summary>
    public string Sigla { get; set; } = string.Empty;

    /// <summary>Total de encuestadores activos</summary>
    public int EncuestadoresActivos { get; set; }

    /// <summary>Total de ciudades con operación</summary>
    public int CiudadesActivas { get; set; }

    /// <summary>Total de trabajos en campo</summary>
    public int TrabajosEnCampo { get; set; }

    /// <summary>Promedio de encuestas por encuestador</summary>
    public decimal PromedioEncuestasPorEncuestador { get; set; }

    /// <summary>Promedio de calidad general (%)</summary>
    public decimal PromedioCalidad { get; set; }

    /// <summary>Total de errores del periodo</summary>
    public int TotalErrores { get; set; }

    /// <summary>Días laborables del mes</summary>
    public int DiasLaborables { get; set; }

    /// <summary>Productividad diaria (encuestas/día)</summary>
    public decimal ProductividadDiaria => DiasLaborables > 0 
        ? Math.Round(PromedioEncuestasPorEncuestador / DiasLaborables, 2) 
        : 0;
}
