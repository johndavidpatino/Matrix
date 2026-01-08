using MatrixNext.Web.Services.OP.Models;
using MatrixNext.Web.ViewModels.OP;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Servicio para gestión de trabajos OP (Portal COE)
/// </summary>
public interface IOpTrabajosService
{
    /// <summary>
    /// Obtiene la configuración de un trabajo OP
    /// </summary>
    /// <param name="trabajoId">ID del trabajo</param>
    /// <returns>Configuración del trabajo o null si no existe</returns>
    Task<TrabajoOpConfiguracion?> ObtenerConfiguracionAsync(long trabajoId);
    
    /// <summary>
    /// Guarda o actualiza la configuración de un trabajo OP
    /// </summary>
    /// <param name="trabajoId">ID del trabajo</param>
    /// <param name="tipoRecoleccionId">ID del tipo de recolección</param>
    /// <param name="usuarioId">ID del usuario que actualiza</param>
    /// <returns>True si se guardó exitosamente</returns>
    Task<bool> GuardarConfiguracionAsync(long trabajoId, short tipoRecoleccionId, long usuarioId);
    
    /// <summary>
    /// Valida si un trabajo está en estado cerrado o anulado
    /// </summary>
    /// <param name="trabajoId">ID del trabajo</param>
    /// <returns>True si el trabajo está bloqueado</returns>
    Task<bool> EstaTrabajoBloquadoAsync(long trabajoId);
    
    /// <summary>
    /// Obtiene el ID de la ficha cuantitativa asociada a un trabajo
    /// </summary>
    /// <param name="trabajoId">ID del trabajo</param>
    /// <returns>ID de la ficha cuantitativa o null si no existe</returns>
    Task<long?> ObtenerIdFichaCuantitativaAsync(long trabajoId);
    
    /// <summary>
    /// Verifica si existe estimación de producción para un trabajo
    /// </summary>
    /// <param name="trabajoId">ID del trabajo</param>
    /// <returns>True si existe estimación</returns>
    Task<bool> TieneEstimacionAsync(long trabajoId);
}
