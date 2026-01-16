using MatrixNext.Data.DTOs.PC;

namespace MatrixNext.Data.Adapters.PC
{
    /// <summary>
    /// Interfaz para acceso a datos de productos internos
    /// </summary>
    public interface IProductoInternoAdapter
    {
        /// <summary>
        /// Obtiene todos los productos internos
        /// </summary>
        Task<IEnumerable<ProductoInternoListDto>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene productos enviados por una unidad específica
        /// </summary>
        Task<IEnumerable<ProductoInternoListDto>> ObtenerPorUnidadEnviaAsync(int unidadId, int? proyectoId = null);

        /// <summary>
        /// Obtiene productos para recibir por una unidad específica
        /// </summary>
        Task<IEnumerable<ProductoInternoListDto>> ObtenerPorUnidadRecibeAsync(int unidadId, int? proyectoId = null);

        /// <summary>
        /// Obtiene un producto por ID
        /// </summary>
        Task<ProductoInternoListDto?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Crea un nuevo producto interno
        /// </summary>
        Task<int> CrearAsync(ProductoInternoDto dto, int userId);

        /// <summary>
        /// Actualiza un producto interno existente
        /// </summary>
        Task<bool> ActualizarAsync(ProductoInternoDto dto, int userId);

        /// <summary>
        /// Actualiza solo la cantidad de un producto
        /// </summary>
        Task<bool> ActualizarCantidadAsync(int id, decimal cantidad, int userId);

        /// <summary>
        /// Registra la recepción de un producto
        /// </summary>
        Task<bool> RegistrarRecepcionAsync(int id, int recibeUsuarioId, DateTime fechaRecepcion, string? observaciones);

        /// <summary>
        /// Elimina un producto interno
        /// </summary>
        Task<bool> EliminarAsync(int id);
    }
}
