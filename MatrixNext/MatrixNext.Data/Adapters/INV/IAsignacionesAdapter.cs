using MatrixNext.Data.DTOs.INV;

namespace MatrixNext.Data.Adapters.INV
{
    /// <summary>
    /// Interfaz para operaciones de datos de asignaciones de activos fijos.
    /// </summary>
    public interface IAsignacionesAdapter
    {
        /// <summary>
        /// Obtiene todas las asignaciones con filtros opcionales.
        /// </summary>
        Task<IEnumerable<AsignacionListDto>> ObtenerTodosAsync(
            long? idActivoFijo = null,
            long? idArticulo = null,
            int? idBU = null,
            string? jobBookCodigo = null,
            long? idUsuarioAsignado = null,
            bool? asignado = null);

        /// <summary>
        /// Obtiene una asignación por su ID.
        /// </summary>
        Task<AsignacionActivoDto?> ObtenerPorIdAsync(long idActivoFijo);

        /// <summary>
        /// Crea una nueva asignación y retorna su ID.
        /// </summary>
        Task<long> CrearAsync(AsignacionActivoDto dto, long usuarioId);

        /// <summary>
        /// Actualiza una asignación existente.
        /// </summary>
        Task ActualizarAsync(AsignacionActivoDto dto, long usuarioId);

        /// <summary>
        /// Elimina una asignación.
        /// </summary>
        Task<int> EliminarAsync(long idActivoFijo);

        /// <summary>
        /// Crea un registro de log de asignación para auditoría.
        /// </summary>
        Task<long> CrearLogAsync(long idActivoFijo, long idArticulo, long idUsuario, bool asignado);
    }
}
