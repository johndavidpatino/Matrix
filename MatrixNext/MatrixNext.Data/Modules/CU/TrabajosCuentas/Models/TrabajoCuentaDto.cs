using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.Modules.CU.TrabajosCuentas.Models;

/// <summary>
/// DTO para Trabajo de Cuenta - Mapea resultado de CU_Trabajos_Get
/// </summary>
public class TrabajoCuentaDto
{
    public long Id { get; set; }
    public string? JobBook { get; set; }
    public string? NombreTrabajo { get; set; }
    public long ProyectoId { get; set; }
    public int Estado { get; set; }
    public string? EstadoTrabajo { get; set; }
    public long? COE { get; set; }
    public string? CoeAsignado { get; set; }
    public int? Unidad { get; set; }
    public string? NombreUnidad { get; set; }
    public string? GerenciaOperativa { get; set; }
    public string? GerenteProyectos { get; set; }
    public short? OP_MetodologiaId { get; set; }
    public string? Metodologia { get; set; }
    public long? IdPropuesta { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int? Muestra { get; set; }
    public int? MuestraReal { get; set; }
    public decimal? ValorTrabajo { get; set; }
}

/// <summary>
/// Parámetros de búsqueda para Trabajos de Cuenta
/// </summary>
public class TrabajoCuentaBusquedaParams
{
    public long? Id { get; set; }
    public int? Estado { get; set; }
    public string? NombreTrabajo { get; set; }
    public string? JobBook { get; set; }
    public long? ProyectoId { get; set; }
    public long? COE { get; set; }
    public long? GerenteCuentas { get; set; }
    public long? Unidad { get; set; }
    public long? Gerencia { get; set; }
    public long? Propuesta { get; set; }
    public long? EstudioId { get; set; }
}

/// <summary>
/// DTO para Estado de Trabajo
/// </summary>
public class EstadoTrabajoDto
{
    public int Id { get; set; }
    public string EstadoDesc { get; set; } = string.Empty;
}

/// <summary>
/// ViewModel para la vista de Trabajos de Cuenta
/// </summary>
public class TrabajosCuentasIndexViewModel
{
    public long? EstudioId { get; set; }
    public string? NombreEstudio { get; set; }
    public string? JobBookEstudio { get; set; }
    public IEnumerable<TrabajoCuentaDto> Trabajos { get; set; } = [];
    public IEnumerable<EstadoTrabajoDto> Estados { get; set; } = [];
}
