using MatrixNext.Data.DTOs.INV;

namespace MatrixNext.Data.Services.INV
{
    /// <summary>
    /// Interfaz para lógica de negocio de legalizaciones de consumibles.
    /// </summary>
    public interface ILegalizacionesService
    {
        /// <summary>
        /// Obtiene todas las legalizaciones con filtros opcionales.
        /// </summary>
        Task<IEnumerable<LegalizacionDto>> ObtenerTodosAsync(
            long? idConsumible = null,
            int? idBU = null,
            long? idUsuarioAsignado = null);

        /// <summary>
        /// Obtiene una legalización por su ID.
        /// </summary>
        Task<LegalizacionDto?> ObtenerPorIdAsync(long id);

        /// <summary>
        /// Crea una nueva legalización validando reglas de negocio.
        /// </summary>
        Task<(bool success, string message, long id)> CrearAsync(LegalizacionDto dto, long usuarioId);

        /// <summary>
        /// Actualiza una legalización existente.
        /// </summary>
        Task<(bool success, string message)> ActualizarAsync(LegalizacionDto dto, long usuarioId);

        /// <summary>
        /// Elimina una legalización.
        /// </summary>
        Task<(bool success, string message)> EliminarAsync(long id, long usuarioId);

        /// <summary>
        /// Obtiene listado paginado de legalizaciones con filtros.
        /// </summary>
        Task<IEnumerable<LegalizacionDto>> ObtenerListadoAsync(
            string? busqueda = null,
            bool? verificado = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int pagina = 1,
            int pageSize = 20);
    }
}
