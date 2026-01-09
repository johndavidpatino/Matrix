namespace MatrixNext.Web.Services.OP.Models;

/// <summary>
/// ViewModel para trabajos cualitativos (lista en grid)
/// Ref: ANALISIS_OP_CUALITATIVO_FASE3_FLUJO1.md PASO 1.2-1.3
/// </summary>
public class TrabajoCualitativoVm
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string UnidadNegocio { get; set; } = string.Empty;
    public long? CoeId { get; set; }
    public string CoeNombre { get; set; } = string.Empty;
    public int? Tipo { get; set; }
    public string TipoDescripcion { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaTerminacion { get; set; }
    
    // Configuración específica de COE
    public DateTime? FechaInicioCampo { get; set; }
    public DateTime? FechaFinCampo { get; set; }
    public int? TipoRecoleccion { get; set; } // 1=Presencial, 2=Telefónica, 3=Online, 4=Mixta
    public string TipoRecoleccionDescripcion { get; set; } = string.Empty;
    
    // Para navegación a otros módulos
    public bool TieneFichaEntrevista { get; set; }
    public bool TieneFichaSesion { get; set; }
    public bool TieneFichaObservacion { get; set; }
    public bool TieneMuestra { get; set; }
    public bool TieneFiltroReclutamiento { get; set; }
    public bool TieneFiltroAsistencia { get; set; }
}

/// <summary>
/// ViewModel para navegación desde Trabajos a módulos relacionados
/// Ref: Trabajos.aspx botones de navegación condicionales
/// </summary>
public class NavigacionTrabajoVm
{
    public long TrabajoId { get; set; }
    public bool PuedeIrAFichaEntrevista { get; set; }
    public bool PuedeIrAFichaSesion { get; set; }
    public bool PuedeIrAFichaObservacion { get; set; }
    public bool PuedeIrAMuestra { get; set; }
    public bool PuedeIrAFiltroReclutamiento { get; set; }
    public bool PuedeIrAFiltroAsistencia { get; set; }
    public bool PuedeIrAProgramacion { get; set; }
    public bool PuedeIrAIps { get; set; }
    public string MensajeNavegacion { get; set; } = string.Empty;
}
