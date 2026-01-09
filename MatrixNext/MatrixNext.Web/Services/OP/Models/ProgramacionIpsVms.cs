namespace MatrixNext.Web.Services.OP.Models;

/// <summary>
/// ViewModel para programación de campo cualitativo
/// Ref: ProgramacionCampo.aspx.vb líneas 45-89
/// </summary>
public class ProgramacionCampoVm
{
    public long ProgramacionId { get; set; } // Usado en vistas
    public long Id { get; set; } // Alias
    public long TrabajoId { get; set; }
    public string TrabajoNombre { get; set; } = string.Empty;
    public string NombreTrabajo { get; set; } = string.Empty; // Alias para vistas
    public long EntrevistadoId { get; set; }
    public string EntrevistadoNombre { get; set; } = string.Empty;
    public string NombreEntrevistado { get; set; } = string.Empty; // Alias para vistas
    public string EntrevistadoTelefono { get; set; } = string.Empty;
    public string EntrevistadoDireccion { get; set; } = string.Empty;
    
    // Estados: 1=Creado, 2=Asignado, 3=Confirmado, 4=Ejecutado, 5=Cancelado, 6=NoAsistio, 7=Reprogramado
    public int Estado { get; set; }
    public int EstadoId { get; set; } // Alias para vistas
    public string EstadoDescripcion { get; set; } = string.Empty;
    public string NombreEstado { get; set; } = string.Empty; // Alias para vistas
    
    public DateTime? FechaProgramada { get; set; }
    public TimeSpan? HoraProgramada { get; set; }
    public string? LugarCita { get; set; }
    public string? DireccionCita { get; set; }
    public string? MedioProgramacion { get; set; }
    public int? DuracionEstimada { get; set; }
    
    public long? EntrevistadorAsignadoId { get; set; }
    public string? EntrevistadorAsignadoNombre { get; set; }
    
    public string? Observaciones { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public long? CreadoPor { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public long? ModificadoPor { get; set; }
}

/// <summary>
/// ViewModel para entrevistados disponibles para programar
/// Ref: ProgramacionCampo.aspx.vb líneas 220-287
/// </summary>
public class EntrevistadoDisponibleVm
{
    public long Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public bool EstaDisponible { get; set; }
    public int CantidadProgramaciones { get; set; }
    public DateTime? UltimaProgramacion { get; set; }
}

/// <summary>
/// ViewModel para resultado de validación de participantes seleccionados
/// Usado en Sprint 4 (Validación Participantes)
/// </summary>
public class ParticipanteValidacionVm
{
    public long ParticipanteId { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public bool Disponible { get; set; }
    public string? MotivoNoValido { get; set; }
    public int ProgramacionesPrevias { get; set; }
    public DateTime? UltimaProgramacion { get; set; }
}

/// <summary>
/// ViewModel para procesos IPS
/// Ref: IPSCuali.aspx.vb líneas 28-35 (SqlDataSource OP_IPS_Procesos)
/// </summary>
public class ProcesoIpsVm
{
    public int Id { get; set; }
    public string Proceso { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
}

/// <summary>
/// ViewModel para revisión IPS
/// Ref: IPSCuali.aspx.vb líneas 38-125 (gvRevision_RowDataBound)
/// </summary>
public class IpsRevisionVm
{
    public long Id { get; set; }
    public long IdProceso { get; set; } // Alias para vistas
    public long TrabajoId { get; set; }
    public string TrabajoNombre { get; set; } = string.Empty;
    public string NombreTrabajo { get; set; } = string.Empty; // Alias para vistas
    public int ProcesoId { get; set; }
    public string ProcesoNombre { get; set; } = string.Empty;
    public string TipoProceso { get; set; } = string.Empty; // Alias para vistas
    public int TareaId { get; set; }
    public string TareaNombre { get; set; } = string.Empty;
    
    public int EstadoWorkflow { get; set; }
    public string EstadoWorkflowDescripcion { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty; // Alias para vistas
    
    public DateTime? FechaRevision { get; set; }
    public DateTime? FechaGeneracion { get; set; } // Alias para vistas
    public long? RevisadoPor { get; set; }
    public string? RevisadoPorNombre { get; set; }
    public string? UsuarioRevision { get; set; } // Alias para vistas
    public string? ObservacionesRevision { get; set; }
    
    public string? Observaciones { get; set; }
    public bool Aprobado { get; set; }
    public bool RequiereAtencion { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
}

