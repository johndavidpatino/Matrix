namespace MatrixNext.Data.DTOs.RP;

/// <summary>
/// DTO para indicadores de calidad - Esquema de Análisis
/// SP: REP_Diligenciamiento_Esquema_Analisis
/// </summary>
public class EsquemaAnalisisDto
{
    public int? IdPDC { get; set; }
    public string? GerenteCuentas { get; set; }
    public int? MesPDC { get; set; }
    public int? AñoPDC { get; set; }
    public string? Cliente { get; set; }
    public string? TieneEsquemaAnalisis { get; set; }
    public DateTime? FechaCreacionPDC { get; set; }
    public DateTime? FechaEsquemaAnalisis { get; set; }
}

/// <summary>
/// DTO para resumen agrupado de esquema análisis
/// </summary>
public class EsquemaAnalisisResumenDto
{
    public string? Gerente { get; set; }
    public int? Año { get; set; }
    public int? Mes { get; set; }
    public int Base { get; set; }
    public int Cumplimiento { get; set; }
    public string? Porcentaje { get; set; }
}

/// <summary>
/// DTO para indicadores de diligenciamiento de Brief
/// SP: REP_Porcentaje_Diligenciamiento_Brief
/// </summary>
public class DiligenciamientoBriefDto
{
    public long? IdBrief { get; set; }
    public string? PorcentajeDiligenciamiento { get; set; }
    public DateTime? FechaCreacionBrief { get; set; }
    public int? Año { get; set; }
    public int? Mes { get; set; }
    public string? Usuario { get; set; }
}

/// <summary>
/// DTO para resumen agrupado de brief
/// </summary>
public class DiligenciamientoBriefResumenDto
{
    public string? Gerente { get; set; }
    public int? Año { get; set; }
    public int? Mes { get; set; }
    public int Base { get; set; }
    public string? Porcentaje { get; set; }
}

/// <summary>
/// DTO para indicadores de envío propuestas 48 horas
/// SP: REP_Envio_Propuestas_48Horas
/// </summary>
public class EnvioPropuestas48HorasDto
{
    public long? IdPropuesta { get; set; }
    public string? GerenteCuentas { get; set; }
    public int? MesCreacionBrief { get; set; }
    public int? AnoCreacionBrief { get; set; }
    public string? Cliente { get; set; }
    public DateTime? FechaCreacionBrief { get; set; }
    public DateTime? FechaEnvioPropuesta { get; set; }
    public int? HorasTranscurridas { get; set; }
    public string? CumpleEnvio48Horas { get; set; }
}

/// <summary>
/// DTO para resumen agrupado de propuestas 48h
/// </summary>
public class EnvioPropuestas48HorasResumenDto
{
    public string? Gerente { get; set; }
    public int? Año { get; set; }
    public int? Mes { get; set; }
    public int Base { get; set; }
    public int Cumplen { get; set; }
    public string? Porcentaje { get; set; }
}

/// <summary>
/// Filtros para indicadores de calidad
/// </summary>
public class IndicadoresCalidadFiltrosDto
{
    public short? Año { get; set; }
    public short? Mes { get; set; }
    public short? Estado { get; set; }
    public string? Usuario { get; set; }
    public int TipoReporte { get; set; } = 1; // 1=EsquemaAnalisis, 2=Brief, 3=Propuestas48h
}

/// <summary>
/// ViewModel para vista de indicadores de calidad
/// </summary>
public class IndicadoresCalidadViewModel
{
    public short? AñoSeleccionado { get; set; }
    public short? MesSeleccionado { get; set; }
    public int TipoReporteSeleccionado { get; set; }
    public List<int> AñosDisponibles { get; set; } = new();
    public List<string> UsuariosDisponibles { get; set; } = new();
    
    // Datos de esquema análisis
    public List<EsquemaAnalisisResumenDto> ResumenEsquema { get; set; } = new();
    public List<EsquemaAnalisisDto> DetalleEsquema { get; set; } = new();
    
    // Datos de brief
    public List<DiligenciamientoBriefResumenDto> ResumenBrief { get; set; } = new();
    public List<DiligenciamientoBriefDto> DetalleBrief { get; set; } = new();
    
    // Datos de propuestas 48h
    public List<EnvioPropuestas48HorasResumenDto> ResumenPropuestas { get; set; } = new();
    public List<EnvioPropuestas48HorasDto> DetallePropuestas { get; set; } = new();
}
