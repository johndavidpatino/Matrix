/// <summary>
/// DTOs para gestión completa de tráfico de encuestas
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.9
/// </summary>
namespace MatrixNext.Data.Models.OP;

/// <summary>
/// Datos de movimiento de tráfico (envío/recepción/devolución)
/// </summary>
public class TraficoEncuestaDto
{
    public long IdMovimiento { get; set; }
    public long IdTrabajo { get; set; }
    public string NumeroTrabajo { get; set; }
    public int IdUnidadOrigen { get; set; }
    public string NombreUnidadOrigen { get; set; }
    public int IdUnidadDestino { get; set; }
    public string NombreUnidadDestino { get; set; }
    public string TipoMovimiento { get; set; } // Envío, Recepción, Devolución
    public int CantidadEnviada { get; set; }
    public int? CantidadRecibida { get; set; }
    public int? Discrepancia { get; set; } // CantidadEnviada - CantidadRecibida
    public DateTime FechaEnvio { get; set; }
    public DateTime? FechaRecepcion { get; set; }
    public long EnviadoPor { get; set; }
    public string NombreEnviador { get; set; }
    public long? RecibidoPor { get; set; }
    public string? NombreReceptor { get; set; }
    public string? Observaciones { get; set; }
    public string? ObservacionesDiscrepancia { get; set; }
    public string Estado { get; set; } // Enviado, Recibido, Devuelto, EnTransito
    public string? Ciudad { get; set; } // Para RMC
}

/// <summary>
/// Datos de personal asignado a tráfico
/// </summary>
public class PersonalTraficoDto
{
    public long IdAsignacion { get; set; }
    public long IdMovimiento { get; set; }
    public long IdEmpleado { get; set; }
    public string NombreEmpleado { get; set; }
    public string NumeroIdentificacion { get; set; }
    public string Cargo { get; set; } // Encuestador, Supervisor, Crítico, Digitador
    public int CantidadAsignada { get; set; }
    public DateTime FechaAsignacion { get; set; }
    public long AsignadoPor { get; set; }
}

/// <summary>
/// Resumen de tráfico por unidad
/// </summary>
public class ResumenTraficoDto
{
    public int IdUnidad { get; set; }
    public string NombreUnidad { get; set; }
    public int TotalEnviado { get; set; }
    public int TotalRecibido { get; set; }
    public int TotalDevuelto { get; set; }
    public int EnTransito { get; set; }
    public int TotalDiscrepancias { get; set; }
    public DateTime? UltimoMovimiento { get; set; }
}

/// <summary>
/// Filtros de búsqueda para tráfico
/// </summary>
public class FiltrosTraficoDto
{
    public long? IdTrabajo { get; set; }
    public int? IdUnidadOrigen { get; set; }
    public int? IdUnidadDestino { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string? TipoMovimiento { get; set; }
    public string? Estado { get; set; }
    public bool? SoloConDiscrepancia { get; set; }
}

/// <summary>
/// Datos para envío de encuestas
/// </summary>
public class EnvioEncuestasDto
{
    public long IdTrabajo { get; set; }
    public int IdUnidadOrigen { get; set; }
    public int IdUnidadDestino { get; set; }
    public int Cantidad { get; set; }
    public string? Ciudad { get; set; } // Requerido para RMC (unidad 119)
    public string? Observaciones { get; set; }
    public long EnviadoPor { get; set; }
}

/// <summary>
/// Datos para recepción de encuestas
/// </summary>
public class RecepcionEncuestasDto
{
    public long IdMovimiento { get; set; }
    public int CantidadRecibida { get; set; }
    public string? ObservacionesDiscrepancia { get; set; }
    public long RecibidoPor { get; set; }
}

/// <summary>
/// Datos para devolución de encuestas
/// </summary>
public class DevolucionEncuestasDto
{
    public long IdMovimiento { get; set; }
    public int CantidadDevuelta { get; set; }
    public string MotivoDevolucion { get; set; }
    public long DevueltoPor { get; set; }
}

/// <summary>
/// Datos para asignación de personal
/// </summary>
public class AsignacionPersonalDto
{
    public long IdMovimiento { get; set; }
    public long IdEmpleado { get; set; }
    public int CantidadAsignada { get; set; }
    public string Cargo { get; set; }
    public long AsignadoPor { get; set; }
}
