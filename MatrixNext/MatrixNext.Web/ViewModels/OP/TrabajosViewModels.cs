using MatrixNext.Web.Models.PY;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.ViewModels.OP;

/// <summary>
/// ViewModel para la vista Index de Trabajos (Portal COE)
/// </summary>
public class OpTrabajosViewModel
{
    public FiltrosVM Filtros { get; set; } = new();
    public PaginationResultVM<Trabajo> Trabajos { get; set; } = new();
    public long UserId { get; set; }
    public long? TrabajoSeleccionadoId { get; set; }
    public short? TipoRecoleccionId { get; set; }
    public bool TieneEstimacion { get; set; }
    public long? IdFicha { get; set; }
}

/// <summary>
/// ViewModel para selección de trabajo
/// </summary>
public class SeleccionarTrabajoViewModel
{
    public long TrabajoId { get; set; }
    public string NombreTrabajo { get; set; } = string.Empty;
    public string JobBook { get; set; } = string.Empty;
    public int Estado { get; set; }
    public string EstadoDescripcion { get; set; } = string.Empty;
}

/// <summary>
/// ViewModel para Ficha Cuantitativa
/// </summary>
public class FichaCuantitativaVM
{
    public long? Id { get; set; }
    public long IdTrabajo { get; set; }
    public string Incentivos { get; set; } = string.Empty;
    public string RegaloClientes { get; set; } = string.Empty;
    public string CompraIpsos { get; set; } = string.Empty;
    public string HabeasData { get; set; } = string.Empty;
    public string GrupoObjetivo { get; set; } = string.Empty;
    public string MarcoMuestral { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
}
