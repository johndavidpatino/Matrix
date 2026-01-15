namespace MatrixNext.Data.Models.OP;

/// <summary>
/// DTO para cierre de trabajo
/// </summary>
public class CierreTrabajoDto
{
    public long IdTrabajo { get; set; }
    public string EstadoAnterior { get; set; } = string.Empty;
    public string EstadoNuevo { get; set; } = "Cerrado";
    public DateTime FechaCierre { get; set; } = DateTime.UtcNow;
    public string? Observaciones { get; set; }
    public bool ValidacionDocumentosOk { get; set; }
    public int TotalDocumentosValidados { get; set; }
    public long? UsuarioId { get; set; }
}

/// <summary>
/// Resultado de validación de documentos para cierre
/// </summary>
public class ValidacionDocumentosDto
{
    public bool EsValido { get; set; }
    public int TotalDocumentos { get; set; }
    public int DocumentosValidados { get; set; }
    public List<string> ErroresValidacion { get; set; } = new();
    public string? MensajeError { get; set; }
}

/// <summary>
/// Configuración de rutas GD
/// </summary>
public class ConfiguracionGdDto
{
    public string ServidorGd { get; set; } = string.Empty;
    public string UnidadGd { get; set; } = string.Empty;
    public string JbiPath { get; set; } = string.Empty;
    public bool EstaConfigured => !string.IsNullOrEmpty(ServidorGd) && !string.IsNullOrEmpty(UnidadGd);
}
