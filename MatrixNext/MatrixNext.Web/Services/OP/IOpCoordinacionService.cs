using MatrixNext.Web.Services.OP.Models;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Servicio para gestión de coordinación y asignación de personal
/// </summary>
public interface IOpCoordinacionService
{
    /// <summary>
    /// Obtiene trabajos asignados a un coordinador
    /// </summary>
    Task<List<TrabajoCoordinadorDto>> ObtenerTrabajosPorCoordinadorAsync(long coordinadorId, long? trabajoId = null, string? nombre = null, string? jobBook = null, int? estado = null);
    
    /// <summary>
    /// Obtiene trabajos de Call Center
    /// </summary>
    Task<List<TrabajoCoordinadorDto>> ObtenerTrabajosCallCenterAsync(long? trabajoId = null, string? nombre = null, string? jobBook = null, int? estado = null);
    
    /// <summary>
    /// Obtiene ciudades asignadas a un coordinador para un trabajo
    /// </summary>
    Task<List<CiudadAsignadaDto>> ObtenerCiudadesAsignadasAsync(long coordinadorId, long trabajoId);
    
    /// <summary>
    /// Obtiene personal asignado a un trabajo y ciudad
    /// </summary>
    Task<List<PersonalAsignadoDto>> ObtenerPersonalAsignadoAsync(long trabajoId, int? ciudadId = null);
    
    /// <summary>
    /// Obtiene encuestadores disponibles para asignar
    /// </summary>
    Task<List<PersonalDisponibleDto>> ObtenerPersonalDisponibleAsync(long trabajoId, int? ciudadId = null);
    
    /// <summary>
    /// Asigna personal a un trabajo
    /// </summary>
    Task<bool> AsignarPersonalAsync(long trabajoId, long personalId, int? ciudadId, long usuarioId);
    
    /// <summary>
    /// Retira personal de un trabajo
    /// </summary>
    Task<bool> RetirarPersonalAsync(long asignacionId, long usuarioId);
}

/// <summary>
/// DTOs para coordinación
/// </summary>
public record TrabajoCoordinadorDto
{
    public long Id { get; init; }
    public string JobBook { get; init; } = string.Empty;
    public string Nombre { get; init; } = string.Empty;
    public int Estado { get; init; }
    public string Metodologia { get; init; } = string.Empty;
    public long? IdProyecto { get; init; }
}

public record CiudadAsignadaDto
{
    public long Id { get; init; }
    public int CiudadId { get; init; }
    public string Ciudad { get; init; } = string.Empty;
    public int? Muestra { get; init; }
}

public record PersonalAsignadoDto
{
    public long Id { get; init; }
    public long PersonaId { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Cargo { get; init; } = string.Empty;
    public string Ciudad { get; init; } = string.Empty;
}

public record PersonalDisponibleDto
{
    public long Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Cargo { get; init; } = string.Empty;
    public string? Tipo { get; init; }
    public string? Contratacion { get; init; }
    public string Ciudad { get; init; } = string.Empty;
}
