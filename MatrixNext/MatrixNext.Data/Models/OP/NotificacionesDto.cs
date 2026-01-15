namespace MatrixNext.Data.Models.OP;

/// <summary>
/// DTO para destinatarios de email en FichaCuantitativa
/// </summary>
public class DestinatarioEmailDto
{
    public long IdUsuario { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string EmailOrigen { get; set; } = string.Empty;  // Email LDAP/ActiveDirectory
    public string Rol { get; set; } = string.Empty;          // "Coordinador", "COE", "PMO"
    public long? IdUnidad { get; set; }
    public string NombreUnidad { get; set; } = string.Empty;
}

/// <summary>
/// DTO para parámetros de email a enviar
/// </summary>
public class ParamsNotificacionFichaDto
{
    public long IdTrabajo { get; set; }
    public string NumeroTrabajo { get; set; } = string.Empty;
    public string CodigoProyecto { get; set; } = string.Empty;
    public string NombreProyecto { get; set; } = string.Empty;
    public string TipoNotificacion { get; set; } = string.Empty; // "CreacionFicha", "CambioEstado", "Cierre"
    public DateTime FechaNotificacion { get; set; } = DateTime.UtcNow;
    public string? Observaciones { get; set; }
    public List<DestinatarioEmailDto> Destinatarios { get; set; } = new();
}
