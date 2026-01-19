namespace MatrixNext.Data.Dtos;

/// <summary>
/// DTO para solicitud de presupuesto interno de trabajo
/// </summary>
public class SolicitudPresupuestoInternoDto
{
    public long Id { get; set; }
    public long TrabajoId { get; set; }
    public DateTime Fecha { get; set; }
    public long UsuarioId { get; set; }
    public string? Observacion { get; set; }
    
    // Propiedades de navegación (solo lectura)
    public string? NombreTrabajo { get; set; }
    public string? JobBook { get; set; }
    public string? NombreUsuario { get; set; }
}

/// <summary>
/// ViewModel para vista de solicitud de presupuesto
/// </summary>
public class SolicitudPresupuestoViewModel
{
    public long TrabajoId { get; set; }
    public string JobBook { get; set; } = string.Empty;
    public string NombreTrabajo { get; set; } = string.Empty;
    public string? Metodologia { get; set; }
    public string Observacion { get; set; } = string.Empty;
    public bool YaSolicitado { get; set; }
    public SolicitudPresupuestoInternoDto? SolicitudExistente { get; set; }
}
