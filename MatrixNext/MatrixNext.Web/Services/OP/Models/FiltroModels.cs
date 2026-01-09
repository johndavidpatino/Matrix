namespace MatrixNext.Web.Services.OP.Models;

/// <summary>
/// ViewModel para configuración de filtro (reclutamiento o asistencia)
/// Ref: DisenarFiltros.aspx.vb líneas 45-89
/// </summary>
public class FiltroConfigVm
{
    public long TrabajoId { get; set; }
    public int TipoFiltro { get; set; } // 1=Reclutamiento, 2=Asistencia
    public string TrabajoNombre { get; set; } = string.Empty;
    public List<PreguntaFiltroVm> Preguntas { get; set; } = new();
    public string LinkVisualizacion { get; set; } = string.Empty;
    public DateTime? FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
}

/// <summary>
/// ViewModel para pregunta de filtro (dinámica según tipoPregunta)
/// Ref: DisenarFiltros.aspx.vb líneas 321-459 (generación dinámica)
/// </summary>
public class PreguntaFiltroVm
{
    public long Id { get; set; }
    public long TrabajoId { get; set; }
    public int TipoFiltro { get; set; }
    public int TipoPregunta { get; set; } // 1=Texto, 2=NumÉrica, 3=Selección, 4=Multi, 5=Fecha, etc.
    public string TextoPregunta { get; set; } = string.Empty;
    public bool Obligatoria { get; set; }
    public int Orden { get; set; }
    
    // Para preguntas de selección/multi
    public List<OpcionPreguntaVm> Opciones { get; set; } = new();
    
    // Validaciones específicas
    public int? LongitudMinima { get; set; }
    public int? LongitudMaxima { get; set; }
    public decimal? ValorMinimo { get; set; }
    public decimal? ValorMaximo { get; set; }
    public DateTime? FechaMinima { get; set; }
    public DateTime? FechaMaxima { get; set; }
    
    // Metadatos
    public DateTime FechaCreacion { get; set; }
    public long CreadoPor { get; set; }
}

public class OpcionPreguntaVm
{
    public long Id { get; set; }
    public long PreguntaId { get; set; }
    public string Texto { get; set; } = string.Empty;
    public int Orden { get; set; }
}

/// <summary>
/// ViewModel para respuesta de filtro (aprobación)
/// Ref: AprobacionesFiltros.aspx.vb líneas 28-91
/// </summary>
public class RespuestaFiltroVm
{
    public long Id { get; set; }
    public long TrabajoId { get; set; }
    public string TrabajoNombre { get; set; } = string.Empty;
    public int TipoFiltro { get; set; }
    public long PersonaId { get; set; }
    public string PersonaNombre { get; set; } = string.Empty;
    public string PersonaDocumento { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty; // Pendiente, Aprobada, Rechazada
    public DateTime FechaRespuesta { get; set; }
    public DateTime? FechaAprobacion { get; set; }
    public long? AprobadoPor { get; set; }
    public string ObservacionesAprobacion { get; set; } = string.Empty;
    
    // Respuestas específicas (JSON o detalle)
    public List<DetalleRespuestaVm> Respuestas { get; set; } = new();
}

public class DetalleRespuestaVm
{
    public long PreguntaId { get; set; }
    public string TextoPregunta { get; set; } = string.Empty;
    public string Respuesta { get; set; } = string.Empty;
}
