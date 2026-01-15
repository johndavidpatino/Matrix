namespace MatrixNext.Data.Models.OP;

/// <summary>
/// DTO para widget de métrica del dashboard HomeRecoleccion
/// </summary>
public class DashboardMetricaDto
{
    public string Etiqueta { get; set; } = string.Empty;
    public int Valor { get; set; }
    public string Icono { get; set; } = "fas fa-chart-bar";
    public string Color { get; set; } = "primary"; // primary, success, warning, danger, info
    public string? Descripcion { get; set; }
}

/// <summary>
/// DTO para trabajo activo en dashboard
/// </summary>
public class TrabajoActivoDashboardDto
{
    public long IdTrabajo { get; set; }
    public string NumeroTrabajo { get; set; } = string.Empty;
    public string CodigoProyecto { get; set; } = string.Empty;
    public string NombreProyecto { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Metodologia { get; set; } = string.Empty;
    public int MetaEncuestas { get; set; }
    public int EncuestasActuales { get; set; }
    public decimal AvancePercentual { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFinaProgramada { get; set; }
    public string CoordinadorNombre { get; set; } = string.Empty;
    public long? IdUnidad { get; set; }
    public string? NombreUnidad { get; set; }
}

/// <summary>
/// Modelo de datos para HomeRecoleccion dashboard
/// </summary>
public class HomeRecoleccionDashboardDto
{
    public List<DashboardMetricaDto> Metricas { get; set; } = new();
    public List<TrabajoActivoDashboardDto> TrabajosActivos { get; set; } = new();
    public DateTime FechaConsulta { get; set; } = DateTime.UtcNow;
    public string PeriodoReporte { get; set; } = string.Empty; // ej: "Semana 1: 02-08 Ene 2026"
}

/// <summary>
/// DTO para indicador de producción diaria
/// </summary>
public class ProduccionDiariaDto
{
    public DateTime Fecha { get; set; }
    public int EncuestasPlaneadas { get; set; }
    public int EncuestasEjecutadas { get; set; }
    public int Diferencia { get; set; }
    public decimal ProcentajeAvance { get; set; }
}
