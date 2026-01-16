using MatrixNext.Data.DTOs.INV;

namespace MatrixNext.Data.Adapters.INV
{
    /// <summary>
    /// Interfaz para operaciones de datos de legalizaciones de consumibles.
    /// </summary>
    public interface ILegalizacionesAdapter
    {
        /// <summary>
        /// Obtiene todas las legalizaciones con filtros opcionales.
        /// </summary>
        Task<IEnumerable<LegalizacionDto>> ObtenerTodosAsync(
            long? id = null,
            long? idConsumible = null,
            int? idBU = null,
            string? jobBookCodigo = null,
            long? idUsuarioAsignado = null);

        /// <summary>
        /// Obtiene una legalización por su ID.
        /// </summary>
        Task<LegalizacionDto?> ObtenerPorIdAsync(long id);

        /// <summary>
        /// Crea una nueva legalización y retorna su ID.
        /// </summary>
        Task<long> CrearAsync(LegalizacionDto dto, long usuarioId);

        /// <summary>
        /// Actualiza una legalización existente.
        /// </summary>
        Task ActualizarAsync(LegalizacionDto dto, long usuarioId);

        /// <summary>
        /// Elimina una legalización.
        /// </summary>
        Task<int> EliminarAsync(long id);
    }
}
