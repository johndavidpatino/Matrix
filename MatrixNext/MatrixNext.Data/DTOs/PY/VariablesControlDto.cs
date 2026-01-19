namespace MatrixNext.Data.DTOs.PY;

/// <summary>
/// Variables de control de calidad para trabajos
/// </summary>
public class VariableControlDto
{
    public long Id { get; set; }
    public long IdTrabajo { get; set; }
    public long IdEvaluado { get; set; }
    public string? TipoEvaluado { get; set; }
    
    // Seguridad y confidencialidad de la información
    public byte? CumpleSeguridad { get; set; }
    public string? ObsSeguridad { get; set; }
    
    // Forma de obtención de los entrevistados
    public byte? CumpleObtencion { get; set; }
    public string? ObsObtencion { get; set; }
    
    // Grupo objetivo
    public byte? CumpleObjetivo { get; set; }
    public string? ObsObjetivo { get; set; }
    
    // Aplicación de instrumentos
    public byte? CumpleAplicacion { get; set; }
    public string? ObsAplicacion { get; set; }
    
    // Distribución de cuotas
    public byte? CumpleDistribucion { get; set; }
    public string? ObsDistribucion { get; set; }
    
    // Cumplimiento de metodología
    public byte? CumpleCumplimiento { get; set; }
    public string? ObsCumplimiento { get; set; }
    
    public long Usuario { get; set; }
    public DateTime FechaCreacion { get; set; }
    
    // Campos calculados
    public string? NombreEvaluado { get; set; }
    public string? NombreUsuario { get; set; }
}

/// <summary>
/// ViewModel para página de variables de control
/// </summary>
public class VariablesControlViewModel
{
    public long IdTrabajo { get; set; }
    public string? JobBook { get; set; }
    public string? NombreTrabajo { get; set; }
    public string? Modalidad { get; set; }
    public long? IdCOE { get; set; }
    public string? NombreCOE { get; set; }
    public long? IdGerente { get; set; }
    public string? NombreGerente { get; set; }
    
    public List<VariableControlDto> VariablesRegistradas { get; set; } = new();
    public Dictionary<long, string> EmpleadosDisponibles { get; set; } = new();
    public string? TipoEvaluadoSeleccionado { get; set; }
}

/// <summary>
/// Filtros para reportes de variables de control
/// </summary>
public class VariablesControlFiltrosDto
{
    public short? Ano { get; set; }
    public byte? Mes { get; set; }
    public long? IdEvaluado { get; set; }
}

/// <summary>
/// Resultado reporte variables de control
/// </summary>
public class ReporteVariableControlDto
{
    public long Id { get; set; }
    public long IdTrabajo { get; set; }
    public string? JobBook { get; set; }
    public string? NombreTrabajo { get; set; }
    public string? Cliente { get; set; }
    public string? Modalidad { get; set; }
    
    public long IdEvaluado { get; set; }
    public string? NombreEvaluado { get; set; }
    public string? TipoEvaluado { get; set; }
    
    // Cumplimientos
    public byte? CumpleSeguridad { get; set; }
    public string? ObsSeguridad { get; set; }
    public byte? CumpleObtencion { get; set; }
    public string? ObsObtencion { get; set; }
    public byte? CumpleObjetivo { get; set; }
    public string? ObsObjetivo { get; set; }
    public byte? CumpleAplicacion { get; set; }
    public string? ObsAplicacion { get; set; }
    public byte? CumpleDistribucion { get; set; }
    public string? ObsDistribucion { get; set; }
    public byte? CumpleCumplimiento { get; set; }
    public string? ObsCumplimiento { get; set; }
    
    public DateTime FechaCreacion { get; set; }
    public string? UsuarioRegistro { get; set; }
    
    // Campos calculados
    public int TotalCumple { get; set; }
    public int TotalNoCumple { get; set; }
    public decimal? PorcentajeCumplimiento { get; set; }
}

/// <summary>
/// Resultado reporte variables de control por mes
/// </summary>
public class ReporteVariableControlPorMesDto
{
    public short Ano { get; set; }
    public byte Mes { get; set; }
    public string? NombreMes { get; set; }
    public long IdEvaluado { get; set; }
    public string? NombreEvaluado { get; set; }
    
    public int TotalTrabajos { get; set; }
    public int TotalEvaluaciones { get; set; }
    
    // Totales de cumplimiento por variable
    public int TotalCumpleSeguridad { get; set; }
    public int TotalCumpleObtencion { get; set; }
    public int TotalCumpleObjetivo { get; set; }
    public int TotalCumpleAplicacion { get; set; }
    public int TotalCumpleDistribucion { get; set; }
    public int TotalCumpleCumplimiento { get; set; }
    
    // Porcentajes
    public decimal? PorcentajeSeguridad { get; set; }
    public decimal? PorcentajeObtencion { get; set; }
    public decimal? PorcentajeObjetivo { get; set; }
    public decimal? PorcentajeAplicacion { get; set; }
    public decimal? PorcentajeDistribucion { get; set; }
    public decimal? PorcentajeCumplimiento { get; set; }
    public decimal? PorcentajeGeneral { get; set; }
}
