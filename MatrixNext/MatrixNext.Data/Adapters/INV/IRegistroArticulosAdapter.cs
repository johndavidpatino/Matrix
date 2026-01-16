using MatrixNext.Data.DTOs.INV;

namespace MatrixNext.Data.Adapters.INV
{
    /// <summary>
    /// Interfaz para operaciones de datos de registro de artículos.
    /// </summary>
    public interface IRegistroArticulosAdapter
    {
        /// <summary>
        /// Obtiene todos los artículos con filtros opcionales.
        /// </summary>
        Task<IEnumerable<RegistroArticuloListDto>> ObtenerTodosAsync(
            long? id = null,
            long? idTipoArticulo = null,
            long? idArticulo = null,
            long? idSede = null,
            long? idUsuarioAsignado = null,
            bool? asignado = null,
            string? todosCampos = null);

        /// <summary>
        /// Obtiene un artículo por su ID.
        /// </summary>
        Task<RegistroArticuloDto?> ObtenerPorIdAsync(long id);

        /// <summary>
        /// Crea un nuevo artículo y retorna su ID.
        /// </summary>
        Task<long> CrearAsync(RegistroArticuloDto dto, long usuarioId);

        /// <summary>
        /// Actualiza un artículo existente.
        /// </summary>
        Task ActualizarAsync(RegistroArticuloDto dto, long usuarioId);

        /// <summary>
        /// Actualiza el estado de asignación de un artículo.
        /// </summary>
        Task ActualizarAsignadoAsync(long id, bool asignado);

        /// <summary>
        /// Obtiene artículos disponibles para asignación (Asignado = false).
        /// </summary>
        Task<IEnumerable<RegistroArticuloListDto>> ObtenerDisponiblesAsync(long? idTipoArticulo = null);

        /// <summary>
        /// Elimina un artículo del sistema.
        /// </summary>
        Task EliminarAsync(long id);
    }
}
