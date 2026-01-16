using MatrixNext.Data.DTOs.INV;

namespace MatrixNext.Data.Adapters.INV
{
    /// <summary>
    /// Interfaz para operaciones de datos de stock de consumibles.
    /// </summary>
    public interface IStockConsumiblesAdapter
    {
        /// <summary>
        /// Obtiene todos los movimientos de stock con filtros opcionales.
        /// </summary>
        Task<IEnumerable<StockConsumibleListDto>> ObtenerTodosAsync(
            long? id = null,
            long? idConsumible = null,
            short? tipoMovimiento = null,
            int? idBU = null,
            string? jobBookCodigo = null,
            long? idUsuarioAsignado = null,
            bool? legalizado = null);

        /// <summary>
        /// Obtiene stock por consumible específico.
        /// </summary>
        Task<IEnumerable<StockConsumibleListDto>> ObtenerPorConsumibleAsync(long idConsumible);

        /// <summary>
        /// Obtiene stock pendiente de legalizar.
        /// </summary>
        Task<IEnumerable<StockConsumibleListDto>> ObtenerPorLegalizarAsync(
            int? idBU = null,
            string? jobBookCodigo = null,
            long? idArticulo = null,
            long? idUsuarioAsignado = null);

        /// <summary>
        /// Crea un nuevo movimiento de stock y retorna su ID.
        /// </summary>
        Task<long> CrearAsync(StockConsumibleDto dto, long usuarioId);
    }
}
