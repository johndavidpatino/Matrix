using MatrixNext.Data.Dtos;

namespace MatrixNext.Data.Services;

/// <summary>
/// Interfaz para servicio de solicitudes de presupuestos internos
/// </summary>
public interface ISolicitudPresupuestoInternoService
{
    /// <summary>
    /// Obtiene ViewModel para crear solicitud de presupuesto
    /// </summary>
    Task<SolicitudPresupuestoViewModel> PrepararSolicitudAsync(long trabajoId);
    
    /// <summary>
    /// Crea solicitud de presupuesto interno
    /// </summary>
    Task<(bool success, string message)> CrearSolicitudAsync(long trabajoId, string observacion, long usuarioId);
    
    /// <summary>
    /// Verifica si ya existe solicitud para el trabajo
    /// </summary>
    Task<SolicitudPresupuestoInternoDto?> ObtenerSolicitudPorTrabajoAsync(long trabajoId);
}
