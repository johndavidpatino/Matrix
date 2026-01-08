using MatrixNext.Web.Models.OP;

namespace MatrixNext.Web.Services.OP
{
    /// <summary>
    /// Servicio para gestión de estimación de producción por ciudad
    /// </summary>
    /// <remarks>
    /// Migrado desde PlaneacionProduccion.vb del legado
    /// Implementa:
    /// - Consulta de estimaciones por trabajo y ciudad
    /// - Creación de estimaciones con planeación automática
    /// - Activación de estimaciones
    /// - Actualización de cantidades diarias
    /// Ref: WebMatrix/OP_Cuantitativo/EstimacionProduccion.aspx.vb
    /// </remarks>
    public interface IOpEstimacionService
    {
        /// <summary>
        /// Obtiene todas las estimaciones creadas para un trabajo
        /// </summary>
        /// <param name="trabajoId">ID del trabajo</param>
        /// <returns>Lista de estimaciones por ciudad con estado</returns>
        Task<List<EstimacionCiudadListItemVM>> ObtenerEstimacionesPorTrabajoAsync(long trabajoId);

        /// <summary>
        /// Obtiene el detalle de una estimación con su planeación diaria
        /// </summary>
        /// <param name="estimacionId">ID de la estimación</param>
        /// <returns>Detalle de estimación con días y cantidades</returns>
        Task<EstimacionDetalleVM?> ObtenerEstimacionDetalleAsync(long estimacionId);

        /// <summary>
        /// Crea una nueva estimación por ciudad con planeación automática
        /// </summary>
        /// <param name="model">Datos de la estimación (ciudad, días incluidos, festivos)</param>
        /// <param name="usuarioId">ID del usuario que crea la estimación</param>
        /// <returns>ID de la estimación creada</returns>
        /// <remarks>
        /// Ejecuta stored procedure OP_PlaneaccionProduccionManual
        /// </remarks>
        Task<long> CrearEstimacionAsync(CrearEstimacionVM model, long usuarioId);

        /// <summary>
        /// Actualiza la cantidad estimada para un día específico de planeación
        /// </summary>
        /// <param name="planeacionId">ID del registro de planeación (OP_EstimacionProduccion.id)</param>
        /// <param name="cantidad">Nueva cantidad estimada</param>
        Task ActualizarCantidadDiaAsync(long planeacionId, short cantidad);

        /// <summary>
        /// Actualiza todas las cantidades de un día en batch
        /// </summary>
        /// <param name="actualizaciones">Lista de planeacionId y cantidad</param>
        Task ActualizarCantidadesBatchAsync(List<PlaneacionDiaVM> actualizaciones);

        /// <summary>
        /// Activa una estimación para que sea la vigente de la ciudad
        /// Desactiva otras estimaciones activas de la misma ciudad
        /// </summary>
        /// <param name="estimacionId">ID de la estimación a activar</param>
        /// <returns>True si se activó correctamente</returns>
        /// <remarks>
        /// Ejecuta stored procedure OP_Planeacion_ActivarEstimacion
        /// </remarks>
        Task<bool> ActivarEstimacionAsync(long estimacionId);

        /// <summary>
        /// Valida que la suma de cantidades estimadas coincida con la muestra
        /// </summary>
        /// <param name="estimacionId">ID de la estimación</param>
        /// <returns>Tupla (esCorrecto, sumaEstimada, muestraEsperada)</returns>
        Task<(bool esValido, long sumaEstimada, long muestraEsperada)> ValidarEstimacionVsMuestraAsync(long estimacionId);
    }
}
