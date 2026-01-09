namespace MatrixNext.Web.Services.OP.Models;

/// <summary>
/// ViewModel para item en grid de planillas
/// </summary>
public class PlanillaListItemVm
{
    public long IdPlanilla { get; set; }
    public string TipoPlantilla { get; set; } = string.Empty; // "Moderacion" o "Informes"
    public long? IdJob { get; set; }
    public string JobDesc { get; set; } = string.Empty;
    public string JobBook => JobDesc; // Alias para vistas
    public DateTime? Fecha { get; set; }
    public string Tecnica { get; set; } = string.Empty;
    public int? Muestra { get; set; }
    public string ResponsableNombre { get; set; } = string.Empty; // Moderador o Analista
    public short IdEstadoAprobacion { get; set; } // 1=EnEspera, 2=Aprobado, 3=NoAprobado
    public string EstadoAprobacion { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public string UsuarioCreacion { get; set; } = string.Empty;
    public DateTime? FechaModificacion { get; set; }
}

/// <summary>
/// ViewModel para planilla de moderación (create/edit)
/// </summary>
public class PlanillaModeracionVm
{
    public long IdPlanilla { get; set; }
    public long? IdJob { get; set; }
    public string JobDesc { get; set; } = string.Empty;
    public string JobBook
    {
        get => JobDesc;
        set => JobDesc = value;
    } // Alias para vistas
    public DateTime? FechaPlanilla { get; set; }
    public int? IdTecnica { get; set; }
    public string NombreTecnica { get; set; } = string.Empty;
    public int? Muestra { get; set; }
    public long? IdModerador { get; set; }
    public string NombreModerador { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
    public short IdEstadoAprobacion { get; set; } = 1; // Default: EnEspera
    public string EstadoAprobacion { get; set; } = "En Espera";
    public DateTime? FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public string UsuarioCreacion { get; set; } = string.Empty; // Nombre usuario creación
    public long? IdUsuarioCreacion { get; set; }
    public long? UsuarioModificacion { get; set; }
}

/// <summary>
/// ViewModel para planilla de informes (create/edit)
/// </summary>
public class PlanillaInformeVm
{
    public long IdPlanilla { get; set; }
    public long? IdJob { get; set; }
    public string JobDesc { get; set; } = string.Empty;
    public string JobBook
    {
        get => JobDesc;
        set => JobDesc = value;
    } // Alias para vistas
    public DateTime? Fecha { get; set; }
    public string Tecnica { get; set; } = string.Empty;
    public int? Muestra { get; set; }
    public long? IdCuentasUU { get; set; }
    public string Analista { get; set; } = string.Empty;
    public string ServiceLineName { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
    public short IdEstadoAprobacion { get; set; } = 1; // Default: EnEspera
    public string EstadoAprobacion { get; set; } = "En Espera";
    public DateTime? FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public string UsuarioCreacion { get; set; } = string.Empty; // Nombre usuario creación
    public long? IdUsuarioCreacion { get; set; }
    public long? UsuarioModificacion { get; set; }
    public string UsuarioAprobacion { get; set; } = string.Empty; // Nombre usuario aprobación
    public DateTime? FechaAprobacion { get; set; }
}

/// <summary>
/// ViewModel para aprobar/rechazar planilla
/// </summary>
public class AprobacionPlanillaVm
{
    public long IdPlanilla { get; set; }
    public string TipoPlantilla { get; set; } = string.Empty; // "Moderacion" o "Informes"
    public string Accion { get; set; } = string.Empty; // "Aprobar" o "Rechazar"
    public string Observaciones { get; set; } = string.Empty;
}

/// <summary>
/// ViewModel para búsqueda de JobBooks
/// </summary>
public class JobBookSearchVm
{
    public long IdJob { get; set; }
    public string JobDesc { get; set; } = string.Empty;
    public string JobBook => JobDesc; // Alias para vistas
    public string NombreTrabajo => JobDesc; // Alias adicional
    public string Cliente { get; set; } = string.Empty;
    public DateTime? FechaInicio { get; set; }
}

/// <summary>
/// ViewModel para moderadores
/// </summary>
public class ModeradorVm
{
    public long IdModerador { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string NombreModerador => Nombre; // Alias para vistas
    public string Email { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}

/// <summary>
/// ViewModel para técnicas cualitativas
/// </summary>
public class TecnicaVm
{
    public int IdTecnica { get; set; }
    public string NombreTecnica { get; set; } = string.Empty;
    public string TipoTecnica { get; set; } = string.Empty; // "Moderacion", "Observacion", etc.
    public bool Activo { get; set; } = true;
}
