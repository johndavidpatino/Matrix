namespace MatrixNext.Data.DTOs.RP;

/// <summary>
/// DTO para avance general de campo
/// SP: REP_AvanceCampoGeneral
/// </summary>
public class AvanceCampoGeneralDto
{
    public long TrabajoId { get; set; }
    public int MuestraTotal { get; set; }
    public int EncuestasRealizadas { get; set; }
    public decimal PorcentajeAvance { get; set; }
    public int Remanente { get; set; }
    // Propiedades adicionales (compatibilidad)
    public double? Fecha { get; set; }
    public double? Ejecucion { get; set; }
    public double? Variacion { get; set; }
}

/// <summary>
/// DTO para avance de campo por ciudad
/// SP: REP_AvanceCampoxCiudad
/// </summary>
public class AvanceCampoCiudadDto
{
    public string? Ciudad { get; set; }
    public int Muestra { get; set; }
    public int Realizadas { get; set; }
    public decimal PorcentajeAvance { get; set; }
    public int Remanente { get; set; }
    public DateTime? FechaEstimadaCierre { get; set; }
    // Propiedades alternativas (compatibilidad con diferentes SP)
    public int? Meta { get; set; }
    public int? Ejecutado { get; set; }
    public double? Avance { get; set; }
    public int? Pendiente { get; set; }
    public string? Estado { get; set; }
}

/// <summary>
/// DTO para avance porcentual por áreas
/// SP: REP_AvancePorcentualAreas
/// </summary>
public class AvanceAreaDto
{
    public string? Area { get; set; }
    public string? Variable { get; set; }
    public int Muestra { get; set; }
    public int Realizadas { get; set; }
    public decimal PorcentajeAvance { get; set; }
    public decimal? PromedioDiario { get; set; }
    // Propiedades alternativas (compatibilidad)
    public int? Meta { get; set; }
    public int? Ejecutado { get; set; }
    public double? Porcentaje { get; set; }
}

/// <summary>
/// DTO para áreas remanentes
/// SP: REP_AvanceAreasRemanentes
/// </summary>
public class AvanceRemanenteDto
{
    public string? Area { get; set; }
    public string? Ciudad { get; set; }
    public string? Variable { get; set; }
    public int Remanente { get; set; }
    public decimal? DiasEstimados { get; set; }
    public int? EncuestadoresRequeridos { get; set; }
    public string? Observacion { get; set; }
    public int? Ejecutado { get; set; }
}

/// <summary>
/// DTO para matriz de cumplimiento
/// SP: REP_MatrizEstimacionCumplimiento
/// </summary>
public class MatrizCumplimientoDto
{
    // Propiedades generales de fila
    public string? Semana { get; set; }
    public string? Fila { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int Meta { get; set; }
    public int Real { get; set; }
    public decimal PorcentajeCumplimiento { get; set; }
    public int Diferencia { get; set; }
    public string? Estado { get; set; }
    
    // Columnas CAMPO
    public int? CampoMeta { get; set; }
    public int? CampoEjecutado { get; set; }
    public double? CampoPorcentaje { get; set; }
    public int? CampoPendiente { get; set; }
    public string? CampoEstado { get; set; }
    // Columnas RMC
    public int? RmcMeta { get; set; }
    public int? RmcEjecutado { get; set; }
    public double? RmcPorcentaje { get; set; }
    public int? RmcPendiente { get; set; }
    public string? RmcEstado { get; set; }
    // Columnas TOTAL
    public int? TotalMeta { get; set; }
    public int? TotalEjecutado { get; set; }
    public double? TotalPorcentaje { get; set; }
    public int? TotalPendiente { get; set; }
    public string? TotalEstado { get; set; }
}

/// <summary>
/// DTO para encuestas anuladas
/// </summary>
public class EncuestaAnuladaDto
{
    public long? Id { get; set; }
    public string? Ciudad { get; set; }
    public string? Encuestador { get; set; }
    public DateTime? Fecha { get; set; }
    public string? Motivo { get; set; }
}

/// <summary>
/// ViewModel completo para vista de avance de campo
/// </summary>
public class AvanceCampoViewModel
{
    public long TrabajoId { get; set; }
    public long IdTrabajoSeleccionado { get; set; }
    public string? NombreTrabajo { get; set; }
    public bool TieneDatos { get; set; }
    
    // Trabajos disponibles para dropdown (Key=Id, Value=Nombre)
    public Dictionary<long, string> TrabajosDisponibles { get; set; } = new();
    
    // Avance general (como lista para mantener consistencia)
    public List<AvanceCampoGeneralDto> AvanceGeneral { get; set; } = new();
    public string? MensajeVariacion { get; set; }
    
    // Detalle por ciudad
    public List<AvanceCampoCiudadDto> AvancePorCiudad { get; set; } = new();
    
    // Detalle por áreas
    public List<AvanceAreaDto> AvancePorAreas { get; set; } = new();
    
    // Remanentes
    public List<AvanceRemanenteDto> Remanentes { get; set; } = new();
    
    // Matriz de cumplimiento
    public List<MatrizCumplimientoDto> MatrizCumplimiento { get; set; } = new();
    
    // Encuestas anuladas
    public List<EncuestaAnuladaDto> EncuestasAnuladas { get; set; } = new();
    
    // Variables para dropdown de cuotas
    public List<string> VariablesDisponibles { get; set; } = new();
}
