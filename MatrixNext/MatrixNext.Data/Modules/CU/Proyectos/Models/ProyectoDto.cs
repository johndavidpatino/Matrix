using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.Modules.CU.Proyectos.Models;

/// <summary>
/// DTO para Proyecto - Mapea resultado de PY_Proyectos_Get
/// </summary>
public class ProyectoDto
{
    public long Id { get; set; }
    public string JobBook { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int UnidadId { get; set; }
    public string? Unidad { get; set; }
    public long? GerenteProyectos { get; set; }
    public string? GP_Nombres { get; set; }
    public long GP_Id { get; set; }
    public long EstudioId { get; set; }
    public short TipoProyectoId { get; set; }
    public string? TipoProyecto { get; set; }
    public string? A1 { get; set; }
    public string? A2 { get; set; }
    public string? A3 { get; set; }
    public string? A4 { get; set; }
    public string? A5 { get; set; }
    public string? A6 { get; set; }
    public string? A7 { get; set; }
    public int Estado { get; set; }
    public long? GerenteCuentas { get; set; }
}

/// <summary>
/// DTO para crear/editar Proyecto
/// </summary>
public class ProyectoCreateEditDto
{
    public long Id { get; set; }

    [Required(ErrorMessage = "El JobBook es requerido")]
    [StringLength(20, ErrorMessage = "El JobBook no puede exceder 20 caracteres")]
    public string JobBook { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(250, ErrorMessage = "El nombre no puede exceder 250 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La unidad es requerida")]
    public int UnidadId { get; set; }

    public long? GerenteProyectos { get; set; }

    [Required(ErrorMessage = "El estudio es requerido")]
    public long EstudioId { get; set; }

    [Required(ErrorMessage = "El tipo de proyecto es requerido")]
    public short TipoProyectoId { get; set; }

    public string? A1 { get; set; }
    public string? A2 { get; set; }
    public string? A3 { get; set; }
    public string? A4 { get; set; }
    public string? A5 { get; set; }
    public string? A6 { get; set; }
    public string? A7 { get; set; }
}

/// <summary>
/// Parámetros de búsqueda para Proyectos
/// </summary>
public class ProyectoBusquedaParams
{
    public long? Id { get; set; }
    public string? JobBook { get; set; }
    public string? Nombre { get; set; }
    public int? UnidadId { get; set; }
    public long? GerenteProyectos { get; set; }
    public long? EstudioId { get; set; }
    public short? TipoProyectoId { get; set; }
    public string? TodosCampos { get; set; }
    public long? GerenteCuentas { get; set; }
}

/// <summary>
/// DTO para Tipo de Proyecto
/// </summary>
public class TipoProyectoDto
{
    public short Id { get; set; }
    public string TipoProyecto { get; set; } = string.Empty;
}

/// <summary>
/// DTO para Unidad (combo)
/// </summary>
public class UnidadComboDto
{
    public int Id { get; set; }
    public string Unidad { get; set; } = string.Empty;
}

/// <summary>
/// DTO para Presupuesto asociado a Proyecto
/// </summary>
public class ProyectoPresupuestoDto
{
    public long Id { get; set; }
    public long PresupuestoId { get; set; }
    public long ProyectoId { get; set; }
}

/// <summary>
/// ViewModel para la vista de Proyectos
/// </summary>
public class ProyectosIndexViewModel
{
    public long? EstudioId { get; set; }
    public string? NombreEstudio { get; set; }
    public string? JobBookEstudio { get; set; }
    public IEnumerable<ProyectoDto> Proyectos { get; set; } = [];
    public IEnumerable<TipoProyectoDto> TiposProyecto { get; set; } = [];
    public IEnumerable<UnidadComboDto> Unidades { get; set; } = [];
}
