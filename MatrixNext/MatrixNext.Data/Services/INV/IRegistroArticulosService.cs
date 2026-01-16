using MatrixNext.Data.DTOs.INV;

namespace MatrixNext.Data.Services.INV
{
    /// <summary>
    /// Interfaz para lógica de negocio de registro de artículos.
    /// </summary>
    public interface IRegistroArticulosService
    {
        /// <summary>
        /// Obtiene todos los artículos con filtros opcionales.
        /// </summary>
        Task<IEnumerable<RegistroArticuloListDto>> ObtenerTodosAsync(
            long? idTipoArticulo = null,
            long? idSede = null,
            bool? asignado = null,
            string? busqueda = null);

        /// <summary>
        /// Obtiene un artículo por su ID.
        /// </summary>
        Task<RegistroArticuloDto?> ObtenerPorIdAsync(long id);

        /// <summary>
        /// Crea un nuevo artículo validando reglas de negocio.
        /// </summary>
        Task<(bool success, string message, long id)> CrearAsync(RegistroArticuloDto dto, long usuarioId);

        /// <summary>
        /// Actualiza un artículo existente validando reglas de negocio.
        /// </summary>
        Task<(bool success, string message)> ActualizarAsync(RegistroArticuloDto dto, long usuarioId);

        /// <summary>
        /// Obtiene artículos disponibles para asignación.
        /// </summary>
        Task<IEnumerable<RegistroArticuloListDto>> ObtenerDisponiblesAsync(long? idTipoArticulo = null);

        /// <summary>
        /// Obtiene listado paginado de artículos con filtros.
        /// </summary>
        Task<IEnumerable<RegistroArticuloListDto>> ObtenerListadoAsync(
            string? busqueda = null,
            long? idTipoArticulo = null,
            bool? asignado = null,
            int pagina = 1,
            int pageSize = 20);

        /// <summary>
        /// Elimina un artículo del sistema.
        /// </summary>
        Task<(bool success, string message)> EliminarAsync(long id, long usuarioId);
    }
}
