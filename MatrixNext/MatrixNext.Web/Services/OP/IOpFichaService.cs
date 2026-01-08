using MatrixNext.Web.ViewModels.OP;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Servicio para gestión de Ficha Cuantitativa
/// </summary>
public interface IOpFichaService
{
    /// <summary>
    /// Obtiene la ficha cuantitativa de un trabajo
    /// </summary>
    /// <param name="trabajoId">ID del trabajo</param>
    /// <returns>ViewModel de la ficha o null si no existe</returns>
    Task<FichaCuantitativaVM?> ObtenerPorTrabajoAsync(long trabajoId);

    /// <summary>
    /// Guarda o actualiza una ficha cuantitativa
    /// </summary>
    /// <param name="model">Datos de la ficha</param>
    /// <param name="usuarioId">ID del usuario que realiza la operación</param>
    /// <returns>ID de la ficha guardada</returns>
    Task<long> GuardarAsync(FichaCuantitativaVM model, long usuarioId);

    /// <summary>
    /// Sincroniza el campo Habeas Data con la tabla de Propuestas (GAP-OP-18)
    /// </summary>
    /// <param name="trabajoId">ID del trabajo</param>
    /// <param name="habeasData">Texto del habeas data</param>
    Task SincronizarHabeasDataAsync(long trabajoId, string habeasData);

    /// <summary>
    /// Obtiene el ID de proyecto asociado a un trabajo para sincronización
    /// </summary>
    /// <param name="trabajoId">ID del trabajo</param>
    /// <returns>ID del proyecto o null</returns>
    Task<long?> ObtenerIdProyectoPorTrabajoAsync(long trabajoId);
}
