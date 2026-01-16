using MatrixNext.Data.DTOs.PC;

namespace MatrixNext.Data.Services.PC
{
    /// <summary>
    /// Interfaz para lógica de negocio de productos internos
    /// </summary>
    public interface IProductoInternoService
    {
        /// <summary>
        /// Obtiene todos los productos internos
        /// </summary>
        Task<IEnumerable<ProductoInternoListDto>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene productos filtrados por unidad y proyecto
        /// </summary>
        Task<IEnumerable<ProductoInternoListDto>> ObtenerFiltradosAsync(int? unidadId = null, int? proyectoId = null, bool soloEnviados = false, bool soloPendientes = false);

        /// <summary>
        /// Obtiene un producto por ID
        /// </summary>
        Task<ProductoInternoListDto?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Crea un nuevo producto interno con validaciones
        /// </summary>
        Task<(bool success, string message, int id)> CrearAsync(ProductoInternoDto dto, int userId);

        /// <summary>
        /// Actualiza un producto interno con validaciones
        /// </summary>
        Task<(bool success, string message)> ActualizarAsync(ProductoInternoDto dto, int userId);

        /// <summary>
        /// Registra el envío de un producto
        /// </summary>
        Task<(bool success, string message)> RegistrarEnvioAsync(int id, int userId);

        /// <summary>
        /// Registra la recepción de un producto
        /// </summary>
        Task<(bool success, string message)> RegistrarRecepcionAsync(int id, int recibeUsuarioId, string? observaciones);

        /// <summary>
        /// Elimina un producto interno
        /// </summary>
        Task<(bool success, string message)> EliminarAsync(int id, int userId);

        /// <summary>
        /// Valida que el usuario puede editar el producto
        /// </summary>
        Task<bool> PuedeEditarAsync(int productoId, int userId);
    }
}
