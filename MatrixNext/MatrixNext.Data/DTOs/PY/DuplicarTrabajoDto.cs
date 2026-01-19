namespace MatrixNext.Data.DTOs.PY;

/// <summary>
/// DTO para duplicar trabajos con opciones personalizables
/// </summary>
public class DuplicarTrabajoDto
{
    // Trabajo origen
    public long IdTrabajoOrigen { get; set; }
    public long IdProyecto { get; set; }
    public string? JobBookOrigen { get; set; }
    public string? NombreTrabajoOrigen { get; set; }
    
    // Nuevo trabajo
    public string NombreNuevo { get; set; } = string.Empty;
    public DateTime? FechaTentativaInicioCampo { get; set; }
    public DateTime? FechaTentativaFinalizacion { get; set; }
    public int? NumeroMedicion { get; set; }
    
    // Opciones de duplicación
    public bool DuplicarDocumentos { get; set; }
    public bool DuplicarEspecificaciones { get; set; }
    public bool SumarUnMes { get; set; }
    
    // Resultado
    public long? IdTrabajoNuevo { get; set; }
    public string? MensajeResultado { get; set; }
}

/// <summary>
/// ViewModel para la vista de duplicar trabajos
/// </summary>
public class DuplicarTrabajoViewModel
{
    public long IdTrabajo { get; set; }
    public long IdProyecto { get; set; }
    public string? JobBook { get; set; }
    public string? NombreTrabajo { get; set; }
    public string? Modalidad { get; set; }
    public int? Muestra { get; set; }
    public DateTime? FechaTentativaInicioCampo { get; set; }
    public DateTime? FechaTentativaFinalizacion { get; set; }
    public int? NoMedicion { get; set; }
    public int? TipoProyectoId { get; set; }
    
    // Info para navegación
    public string? NombreCliente { get; set; }
    public string? GerenteProyecto { get; set; }
}
