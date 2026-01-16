using MatrixNext.Data.Adapters.INV;
using MatrixNext.Data.DTOs.INV;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.INV
{
    public class StockConsumiblesService : IStockConsumiblesService
    {
        private readonly IStockConsumiblesAdapter _adapter;
        private readonly ILogger<StockConsumiblesService> _logger;

        public StockConsumiblesService(
            IStockConsumiblesAdapter adapter,
            ILogger<StockConsumiblesService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<IEnumerable<StockConsumibleListDto>> ObtenerTodosAsync(
            long? idConsumible = null,
            short? tipoMovimiento = null,
            int? idBU = null,
            bool? legalizado = null)
        {
            try
            {
                return await _adapter.ObtenerTodosAsync(
                    idConsumible: idConsumible,
                    tipoMovimiento: tipoMovimiento,
                    idBU: idBU,
                    legalizado: legalizado
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo stock de consumibles. Consumible={Consumible}, TipoMovimiento={TipoMovimiento}",
                    idConsumible, tipoMovimiento);
                throw;
            }
        }

        public async Task<IEnumerable<StockConsumibleListDto>> ObtenerPorLegalizarAsync(
            int? idBU = null,
            string? jobBookCodigo = null,
            long? idUsuarioAsignado = null)
        {
            try
            {
                return await _adapter.ObtenerPorLegalizarAsync(idBU, jobBookCodigo, null, idUsuarioAsignado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo stock por legalizar. BU={BU}, Usuario={Usuario}",
                    idBU, idUsuarioAsignado);
                throw;
            }
        }

        public async Task<(bool success, string message, long id)> CrearAsync(StockConsumibleDto dto, long usuarioId)
        {
            try
            {
                // Validar fecha no futura
                if (dto.Fecha > DateTime.Now)
                {
                    return (false, "La fecha no puede ser futura", 0);
                }

                // Validar valor y total
                if (dto.Valor <= 0)
                {
                    return (false, "El valor debe ser mayor a 0", 0);
                }

                if (dto.Total <= 0)
                {
                    return (false, "El total debe ser mayor a 0", 0);
                }

                // Validar tipo de movimiento
                // 1 = Entrada, 2 = Salida
                if (dto.TipoMovimiento == 2) // Salida
                {
                    // Para salidas, se requiere usuario asignado
                    if (!dto.IdUsuarioAsignado.HasValue || dto.IdUsuarioAsignado <= 0)
                    {
                        return (false, "Para salidas es requerido el usuario asignado", 0);
                    }

                    // Validar stock disponible
                    var stockDisponible = await CalcularStockDisponibleAsync(dto.IdConsumible);
                    if (stockDisponible < dto.Total)
                    {
                        return (false, $"Stock insuficiente. Disponible: {stockDisponible}", 0);
                    }

                    // Calcular stock después de la salida
                    dto.Disponible = stockDisponible - dto.Total;
                }
                else if (dto.TipoMovimiento == 1) // Entrada
                {
                    // Calcular stock después de la entrada
                    var stockActual = await CalcularStockDisponibleAsync(dto.IdConsumible);
                    dto.Disponible = stockActual + dto.Total;
                }
                else
                {
                    return (false, "Tipo de movimiento inválido", 0);
                }

                var id = await _adapter.CrearAsync(dto, usuarioId);

                _logger.LogInformation("Movimiento de stock {Id} creado. Consumible={Consumible}, TipoMovimiento={TipoMovimiento}, Total={Total}, Usuario={Usuario}",
                    id, dto.IdConsumible, dto.TipoMovimiento, dto.Total, usuarioId);

                return (true, "Movimiento de stock registrado exitosamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando movimiento de stock. Consumible={Consumible}, Usuario={Usuario}",
                    dto.IdConsumible, usuarioId);
                return (false, "Error al registrar el movimiento de stock", 0);
            }
        }

        public async Task<long> CalcularStockDisponibleAsync(long idConsumible)
        {
            try
            {
                var movimientos = await _adapter.ObtenerTodosAsync(idConsumible: idConsumible);
                
                long stock = 0;
                foreach (var mov in movimientos.OrderBy(m => m.Fecha))
                {
                    if (mov.TipoMovimiento == 1) // Entrada
                    {
                        stock += mov.Total ?? 0;
                    }
                    else if (mov.TipoMovimiento == 2) // Salida
                    {
                        stock -= mov.Total ?? 0;
                    }
                }

                return stock;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculando stock disponible. Consumible={Consumible}", idConsumible);
                return 0;
            }
        }

        public async Task<IEnumerable<StockConsumibleListDto>> ObtenerListadoAsync(
            string? busqueda = null,
            long? idConsumible = null,
            short? tipoMovimiento = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            bool? legalizado = null,
            int pagina = 1,
            int pageSize = 20)
        {
            try
            {
                return await _adapter.ObtenerTodosAsync(
                    idConsumible: idConsumible,
                    tipoMovimiento: tipoMovimiento,
                    legalizado: legalizado
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo listado de stock con paginación");
                throw;
            }
        }
    }
}
