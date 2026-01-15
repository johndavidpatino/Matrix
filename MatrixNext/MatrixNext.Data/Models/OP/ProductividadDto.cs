/// <summary>
/// DTOs para consolidación de productividad multi-roles
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.8
/// </summary>
namespace MatrixNext.Data.Models.OP;

/// <summary>
/// Datos de planilla de productividad para cualquier rol
/// </summary>
public class ProductividadPlanillaDto
{
    public long IdPlanilla { get; set; }
    public long IdTrabajo { get; set; }
    public string NumeroTrabajo { get; set; }
    public long IdEmpleado { get; set; }
    public string NombreEmpleado { get; set; }
    public string NumeroIdentificacion { get; set; }
    public DateTime Fecha { get; set; }
    public int Cantidad { get; set; }
    public decimal MontoReportado { get; set; }
    public decimal? MontoAutorizado { get; set; }
    public string TipoProductividad { get; set; } // Encuestas, Llamadas, etc.
    public string Estado { get; set; } // Pendiente, Aprobado, Rechazado
    public string? Observaciones { get; set; }
    public string? ObservacionesRechazo { get; set; }
    public DateTime FechaRegistro { get; set; }
    public long RegistradoPor { get; set; }
    public DateTime? FechaAprobacion { get; set; }
    public long? AprobadoPor { get; set; }
    public int Corte16_15 { get; set; } // 1 o 2
    public int Mes { get; set; }
    public int Año { get; set; }
    public bool PuedeAprobar { get; set; }
    public bool PuedeRechazar { get; set; }
    public bool PuedeEditar { get; set; }
}

/// <summary>
/// Filtros de búsqueda para productividad
/// </summary>
public class FiltrosProductividadDto
{
    public long? IdTrabajo { get; set; }
    public long? IdEmpleado { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int? Corte { get; set; } // 1 o 2
    public int? Mes { get; set; }
    public int? Año { get; set; }
    public string? Estado { get; set; } // Pendiente, Aprobado, Rechazado, Todos
    public string? TipoProductividad { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

/// <summary>
/// Datos para aprobación/rechazo de planilla
/// </summary>
public class AprobacionPlanillaDto
{
    public long IdPlanilla { get; set; }
    public decimal MontoAutorizado { get; set; }
    public string? Observaciones { get; set; }
    public long AprobadoPor { get; set; }
    public string Accion { get; set; } // "Aprobar" o "Rechazar"
}

/// <summary>
/// Resumen de productividad por corte/mes
/// </summary>
public class ResumenProductividadDto
{
    public int TotalPlanillas { get; set; }
    public int PendientesAprobacion { get; set; }
    public int Aprobadas { get; set; }
    public int Rechazadas { get; set; }
    public decimal TotalMontoReportado { get; set; }
    public decimal TotalMontoAutorizado { get; set; }
    public int Corte { get; set; }
    public int Mes { get; set; }
    public int Año { get; set; }
    public List<ProductividadPlanillaDto> Planillas { get; set; } = new();
}

/// <summary>
/// Permisos por rol para gestión de productividad
/// </summary>
public class PermisosProductividadDto
{
    public bool PuedeVerPMO { get; set; } // Permiso 100
    public bool PuedeVerCoordinador { get; set; } // Permiso 135
    public bool PuedeVerCampo { get; set; } // Permiso 156
    public bool PuedeVerMyS { get; set; } // Permiso 157
    public bool PuedeAprobar { get; set; }
    public bool PuedeRechazar { get; set; }
    public bool PuedeEditar { get; set; }
    public string RolActual { get; set; } // PMO, Coordinador, Campo, MyS
}
