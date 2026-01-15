/// <summary>
/// DTOs para supervisión telefónica
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.10
/// </summary>
namespace MatrixNext.Data.Models.OP;

public class SupervisionTelefonicaDto
{
    public long IdSupervision { get; set; }
    public long IdTrabajo { get; set; }
    public string NumeroTrabajo { get; set; }
    public long IdOperador { get; set; }
    public string NombreOperador { get; set; }
    public long IdSupervisor { get; set; }
    public string NombreSupervisor { get; set; }
    public DateTime FechaSupervision { get; set; }
    public string NumeroEncuesta { get; set; }
    public decimal CalificacionTotal { get; set; }
    public string ResultadoSupervision { get; set; } // Aprobado, Rechazado, Observado
    public string? Observaciones { get; set; }
    public DateTime FechaRegistro { get; set; }
    public long RegistradoPor { get; set; }
}

public class ChecklistSupervisionDto
{
    public long IdItem { get; set; }
    public long IdSupervision { get; set; }
    public string CodigoItem { get; set; } // CRI1, CRI2, etc.
    public string DescripcionItem { get; set; }
    public bool Cumple { get; set; }
    public int Puntaje { get; set; }
    public string? ObservacionItem { get; set; }
}

public class CatalogoSupervisionDto
{
    public long IdCatalogo { get; set; }
    public string Tipo { get; set; } // Operadores, Supervisores
    public long IdEmpleado { get; set; }
    public string NombreCompleto { get; set; }
    public string NumeroIdentificacion { get; set; }
    public bool Activo { get; set; }
}

public class FiltrosSupervisionDto
{
    public long? IdTrabajo { get; set; }
    public long? IdOperador { get; set; }
    public long? IdSupervisor { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string? ResultadoSupervision { get; set; }
}

public class RegistroSupervisionDto
{
    public long IdTrabajo { get; set; }
    public long IdOperador { get; set; }
    public long IdSupervisor { get; set; }
    public string NumeroEncuesta { get; set; }
    public List<ChecklistSupervisionDto> Checklist { get; set; } = new();
    public string? Observaciones { get; set; }
    public long RegistradoPor { get; set; }
}

public class ResumenSupervisionDto
{
    public int TotalSupervisiones { get; set; }
    public int Aprobadas { get; set; }
    public int Rechazadas { get; set; }
    public int Observadas { get; set; }
    public decimal PromedioCalificacion { get; set; }
    public List<SupervisionTelefonicaDto> Supervisiones { get; set; } = new();
}
