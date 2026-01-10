using MatrixNext.Web.Models.OP;

namespace MatrixNext.Web.Services.OP
{
    /// <summary>
    /// Servicio para gestión de muestra por ciudad en trabajos OP
    /// </summary>
    /// <remarks>
    /// Migrado desde CoordinacionCampo.vb del legado
    /// Implementa:
    /// - Consulta de muestra por trabajo
    /// - CRUD de muestra por ciudad
    /// - Actualización de fechas con auto-planeación
    /// - Eliminación de muestra
    /// Ref: WebMatrix/OP_Cuantitativo/MuestraTrabajos.aspx.vb
    /// </remarks>
    public interface IOpMuestraService
    {
        /// <summary>
        /// Obtiene la muestra configurada para un trabajo (todas las ciudades)
        /// </summary>
        /// <param name="trabajoId">ID del trabajo</param>
        /// <returns>Lista de ciudades con su muestra</returns>
        Task<List<MuestraCiudadListItemVM>> ObtenerMuestraPorTrabajoAsync(long trabajoId);

        /// <summary>
        /// Obtiene una muestra específica por ID
        /// </summary>
        /// <param name="idMuestra">ID de la muestra</param>
        /// <returns>Datos de la muestra o null si no existe</returns>
        Task<MuestraCiudadVM?> ObtenerMuestraPorIdAsync(long idMuestra);

        /// <summary>
        /// Obtiene la cantidad de muestra de una ciudad específica en un trabajo
        /// </summary>
        /// <param name="trabajoId">ID del trabajo</param>
        /// <param name="ciudadId">ID de la ciudad (código Divipola)</param>
        /// <returns>Cantidad de muestra o 0 si no existe</returns>
        Task<double> ObtenerMuestraPorCiudadAsync(long trabajoId, int ciudadId);

        /// <summary>
        /// Guarda o actualiza la muestra de una ciudad
        /// </summary>
        /// <param name="model">Datos de la muestra</param>
        /// <returns>ID de la muestra guardada</returns>
        Task<long> GuardarMuestraAsync(MuestraCiudadVM model);

        /// <summary>
        /// Actualiza las fechas de inicio y fin de una muestra y ejecuta auto-planeación
        /// </summary>
        /// <param name="model">Fechas y días de trabajo</param>
        /// <returns>True si se actualizó correctamente</returns>
        /// <remarks>
        /// Ejecuta stored procedure OP_AjusteProduccionAutoCiudad
        /// </remarks>
        Task<bool> ActualizarFechasConPlaneacionAsync(ActualizarFechasMuestraVM model);

        /// <summary>
        /// Elimina una muestra de ciudad
        /// </summary>
        /// <param name="idMuestra">ID de la muestra a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        Task<bool> EliminarMuestraAsync(long idMuestra);

        /// <summary>
        /// Calcula el total de muestra de un trabajo sumando todas las ciudades
        /// </summary>
        /// <param name="trabajoId">ID del trabajo</param>
        /// <returns>Total de muestra del trabajo</returns>
        Task<double> CalcularTotalMuestraAsync(long trabajoId);

        /// <summary>
        /// Obtiene detalle de muestra para notificación por email (incluye email del coordinador)
        /// </summary>
        Task<MuestraEmailDetalle?> ObtenerDetalleMuestraParaEmailAsync(long idMuestra);
    }
}
