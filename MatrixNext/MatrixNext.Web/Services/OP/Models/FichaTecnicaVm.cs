namespace MatrixNext.Web.Services.OP.Models;

/// <summary>
/// ViewModel para ficha técnica (Entrevista, Sesión, Observación)
/// Ref: FichaEntrevista.aspx.vb líneas 41-123
/// </summary>
public class FichaTecnicaVm
{
    public long TrabajoId { get; set; }
    public string TrabajoNombre { get; set; } = string.Empty;
    public int TipoFicha { get; set; } // 1=Entrevista, 2=Sesión, 3=Observación
    
    // Datos generales
    public string Objetivos { get; set; } = string.Empty;
    public string PerfilEntrevistados { get; set; } = string.Empty;
    public int CantidadEntrevistas { get; set; }
    public string Metodologia { get; set; } = string.Empty;
    public string TematicaPrincipal { get; set; } = string.Empty;
    
    // Recursos y presupuesto
    public decimal MontoIncentivos { get; set; }
    public decimal PresupuestoDisponible { get; set; }
    public string AyudasAudiovisuales { get; set; } = string.Empty;
    public string RecursosAdicionales { get; set; } = string.Empty;
    
    // Reclutamiento
    public int CantidadReclutadores { get; set; }
    public string PerfilReclutadores { get; set; } = string.Empty;
    public DateTime? FechaInicioReclutamiento { get; set; }
    public DateTime? FechaFinReclutamiento { get; set; }
    
    // Logística
    public string LugarRealizacion { get; set; } = string.Empty;
    public string DireccionCompleta { get; set; } = string.Empty;
    public long? CiudadId { get; set; }
    public string CiudadNombre { get; set; } = string.Empty;
    public DateTime? FechaRealizacion { get; set; }
    public TimeSpan? HoraInicio { get; set; }
    public TimeSpan? HoraFin { get; set; }
    
    // Control
    public bool HabeasDataFirmado { get; set; }
    public string ObservacionesGenerales { get; set; } = string.Empty;
    public string EstadoFicha { get; set; } = string.Empty; // Borrador, Entregada, Aprobada
    
    // Metadatos
    public DateTime? FechaCreacion { get; set; }
    public long? CreadoPor { get; set; }
    public DateTime? FechaEntrega { get; set; }
    public long? EntregadoPor { get; set; }
    public DateTime? FechaModificacion { get; set; }
}
