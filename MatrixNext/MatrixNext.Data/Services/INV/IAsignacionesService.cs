using MatrixNext.Data.DTOs.INV;

namespace MatrixNext.Data.Services.INV
{
    /// <summary>
    /// Interfaz para lógica de negocio de asignaciones de activos fijos.
    /// </summary>
    public interface IAsignacionesService
    {
        /// <summary>
        /// Obtiene todas las asignaciones con filtros opcionales.
        /// </summary>
        Task<IEnumerable<AsignacionListDto>> ObtenerTodosAsync(
            long? idUsuarioAsignado = null,
            int? idBU = null,
            string? jobBookCodigo = null);

        /// <summary>
        /// Obtiene una asignación por ID de activo fijo.
        /// </summary>
        Task<AsignacionActivoDto?> ObtenerPorIdAsync(long idActivoFijo);

        /// <summary>
        /// Crea una nueva asignación validando reglas de negocio.
        /// </summary>
        Task<(bool success, string message, long id)> CrearAsync(AsignacionActivoDto dto, long usuarioId);

        /// <summary>
        /// Actualiza una asignación existente.
        /// </summary>
        Task<(bool success, string message)> ActualizarAsync(AsignacionActivoDto dto, long usuarioId);

        /// <summary>
        /// Elimina una asignación (devuelve el activo).
        /// </summary>
        Task<(bool success, string message)> EliminarAsync(long idActivoFijo, long usuarioId);

        /// <summary>
        /// Obtiene listado paginado de asignaciones con filtros.
        /// </summary>
        Task<IEnumerable<AsignacionListDto>> ObtenerListadoAsync(
            string? busqueda = null,
            long? idUsuarioAsignado = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            int pagina = 1,
            int pageSize = 20);
    }
}
