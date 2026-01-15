/// <summary>
/// DTOs para distribución de entrevistas por metodología
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.2.1
/// </summary>
namespace MatrixNext.Data.Models.PY;

public class DistribucionEntrevistaDto
{
    public long IdDistribucion { get; set; }
    public long IdTrabajo { get; set; }
    public string NumeroTrabajo { get; set; }
    public int IdMetodologia { get; set; }
    public string NombreMetodologia { get; set; }
    public int IdUnidad { get; set; }
    public string NombreUnidad { get; set; }
    public string? Ciudad { get; set; }
    public int CantidadAsignada { get; set; }
    public int? CantidadCompletada { get; set; }
    public decimal PorcentajeAvance { get; set; }
    public DateTime FechaAsignacion { get; set; }
    public long AsignadoPor { get; set; }
}

public class CuotaDistribucionDto
{
    public long IdCuota { get; set; }
    public long IdDistribucion { get; set; }
    public string VariableCuota { get; set; } // Edad, Género, NSE, etc.
    public string ValorCuota { get; set; }
    public int CantidadRequerida { get; set; }
    public int CantidadObtenida { get; set; }
    public bool CumpleCuota { get; set; }
}

public class DistribuirPorUnidadDto
{
    public long IdTrabajo { get; set; }
    public int IdMetodologia { get; set; }
    public List<UnidadDistribucionDto> Unidades { get; set; } = new();
    public long AsignadoPor { get; set; }
}

public class UnidadDistribucionDto
{
    public int IdUnidad { get; set; }
    public string? Ciudad { get; set; }
    public int Cantidad { get; set; }
}

public class ResumenDistribucionDto
{
    public long IdTrabajo { get; set; }
    public int TotalMuestra { get; set; }
    public int TotalDistribuido { get; set; }
    public int TotalCompletado { get; set; }
    public decimal PorcentajeDistribucion { get; set; }
    public decimal PorcentajeAvance { get; set; }
    public List<DistribucionEntrevistaDto> Distribuciones { get; set; } = new();
}

public class VariableControlDto
{
    public long IdVariable { get; set; }
    public long IdTrabajo { get; set; }
    public string NombreVariable { get; set; }
    public string TipoDato { get; set; } // Numérico, Texto, Rango
    public decimal? ValorMinimo { get; set; }
    public decimal? ValorMaximo { get; set; }
    public string? ValoresPermitidos { get; set; }
    public bool Obligatorio { get; set; }
    public string? Descripcion { get; set; }
    public DateTime FechaRegistro { get; set; }
    public long RegistradoPor { get; set; }
}

public class InHomeVisitDto
{
    public long IdVisita { get; set; }
    public long IdTrabajo { get; set; }
    public string NumeroTrabajo { get; set; }
    public string LugarVisita { get; set; }
    public DateTime FechaProgramada { get; set; }
    public DateTime? FechaRealizada { get; set; }
    public string Estado { get; set; } // Programada, Realizada, Cancelada, Reprogramada
    public int CantidadParticipantes { get; set; }
    public string? Recursos { get; set; } // Equipo necesario
    public string? Observaciones { get; set; }
    public long? ResponsableId { get; set; }
    public string? NombreResponsable { get; set; }
    public DateTime FechaRegistro { get; set; }
    public long RegistradoPor { get; set; }
}
