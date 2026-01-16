using MatrixNext.Data.DTOs.INV;

namespace MatrixNext.Data.Services.INV
{
    /// <summary>
    /// Interfaz para lógica de negocio de stock de consumibles.
    /// </summary>
    public interface IStockConsumiblesService
    {
        /// <summary>
        /// Obtiene todos los movimientos de stock con filtros opcionales.
        /// </summary>
        Task<IEnumerable<StockConsumibleListDto>> ObtenerTodosAsync(
            long? idConsumible = null,
            short? tipoMovimiento = null,
            int? idBU = null,
            bool? legalizado = null);

        /// <summary>
        /// Obtiene stock pendiente de legalizar.
        /// </summary>
        Task<IEnumerable<StockConsumibleListDto>> ObtenerPorLegalizarAsync(
            int? idBU = null,
            string? jobBookCodigo = null,
            long? idUsuarioAsignado = null);

        /// <summary>
        /// Crea un nuevo movimiento de stock validando reglas de negocio.
        /// </summary>
        Task<(bool success, string message, long id)> CrearAsync(StockConsumibleDto dto, long usuarioId);

        /// <summary>
        /// Calcula el stock disponible para un consumible.
        /// </summary>
        Task<long> CalcularStockDisponibleAsync(long idConsumible);

        /// <summary>
        /// Obtiene listado paginado de movimientos de stock con filtros.
        /// </summary>
        Task<IEnumerable<StockConsumibleListDto>> ObtenerListadoAsync(
            string? busqueda = null,
            long? idConsumible = null,
            short? tipoMovimiento = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            bool? legalizado = null,
            int pagina = 1,
            int pageSize = 20);
    }
}
