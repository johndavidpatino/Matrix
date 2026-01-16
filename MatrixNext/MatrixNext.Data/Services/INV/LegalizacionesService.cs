using MatrixNext.Data.Adapters.INV;
using MatrixNext.Data.DTOs.INV;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.INV
{
    public class LegalizacionesService : ILegalizacionesService
    {
        private readonly ILegalizacionesAdapter _adapter;
        private readonly ILogger<LegalizacionesService> _logger;

        public LegalizacionesService(
            ILegalizacionesAdapter adapter,
            ILogger<LegalizacionesService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<IEnumerable<LegalizacionDto>> ObtenerTodosAsync(
            long? idConsumible = null,
            int? idBU = null,
            long? idUsuarioAsignado = null)
        {
            try
            {
                return await _adapter.ObtenerTodosAsync(
                    idConsumible: idConsumible,
                    idBU: idBU,
                    idUsuarioAsignado: idUsuarioAsignado
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo legalizaciones. Consumible={Consumible}, BU={BU}",
                    idConsumible, idBU);
                throw;
            }
        }

        public async Task<LegalizacionDto?> ObtenerPorIdAsync(long id)
        {
            try
            {
                return await _adapter.ObtenerPorIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo legalización {Id}", id);
                throw;
            }
        }

        public async Task<(bool success, string message, long id)> CrearAsync(LegalizacionDto dto, long usuarioId)
        {
            try
            {
                // Validar fecha no futura
                if (dto.Fecha > DateTime.Now)
                {
                    return (false, "La fecha no puede ser futura", 0);
                }

                // Validar radicado
                if (string.IsNullOrWhiteSpace(dto.Radicado))
                {
                    return (false, "El radicado es requerido", 0);
                }

                // Validar que la suma de componentes sea coherente
                long totalComponentes = (dto.Firmas ?? 0) + (dto.Devoluciones ?? 0) + 
                                       (dto.NotasCredito ?? 0) + (dto.DescuentoNomina ?? 0);

                if (dto.ValorLegalizado.HasValue && totalComponentes > dto.ValorLegalizado)
                {
                    return (false, "La suma de firmas, devoluciones, notas de crédito y descuento no puede exceder el valor legalizado", 0);
                }

                // Calcular pendiente
                if (dto.ValorLegalizado.HasValue)
                {
                    dto.Pendiente = dto.ValorLegalizado - totalComponentes;
                }

                var id = await _adapter.CrearAsync(dto, usuarioId);

                _logger.LogInformation("Legalización {Id} creada. Consumible={Consumible}, Radicado={Radicado}, Usuario={Usuario}",
                    id, dto.IdConsumible, dto.Radicado, usuarioId);

                return (true, "Legalización creada exitosamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando legalización. Consumible={Consumible}, Usuario={Usuario}",
                    dto.IdConsumible, usuarioId);
                return (false, "Error al crear la legalización", 0);
            }
        }

        public async Task<(bool success, string message)> ActualizarAsync(LegalizacionDto dto, long usuarioId)
        {
            try
            {
                // Validar que la legalización existe
                var existente = await _adapter.ObtenerPorIdAsync(dto.Id);
                if (existente == null)
                {
                    return (false, "La legalización no existe");
                }

                // Validar si ya está verificada
                if (existente.Verificado)
                {
                    _logger.LogWarning("Intento de modificar legalización {Id} que ya está verificada. Usuario={Usuario}",
                        dto.Id, usuarioId);
                    return (false, "No se puede modificar una legalización verificada");
                }

                // Validar fecha
                if (dto.Fecha > DateTime.Now)
                {
                    return (false, "La fecha no puede ser futura");
                }

                // Validar radicado
                if (string.IsNullOrWhiteSpace(dto.Radicado))
                {
                    return (false, "El radicado es requerido");
                }

                // Recalcular pendiente
                long totalComponentes = (dto.Firmas ?? 0) + (dto.Devoluciones ?? 0) + 
                                       (dto.NotasCredito ?? 0) + (dto.DescuentoNomina ?? 0);

                if (dto.ValorLegalizado.HasValue)
                {
                    dto.Pendiente = dto.ValorLegalizado - totalComponentes;
                }

                await _adapter.ActualizarAsync(dto, usuarioId);

                _logger.LogInformation("Legalización {Id} actualizada por usuario {Usuario}", dto.Id, usuarioId);

                return (true, "Legalización actualizada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando legalización {Id}. Usuario={Usuario}", dto.Id, usuarioId);
                return (false, "Error al actualizar la legalización");
            }
        }

        public async Task<(bool success, string message)> EliminarAsync(long id, long usuarioId)
        {
            try
            {
                // Validar que la legalización existe
                var existente = await _adapter.ObtenerPorIdAsync(id);
                if (existente == null)
                {
                    return (false, "La legalización no existe");
                }

                // Validar si ya está verificada
                if (existente.Verificado)
                {
                    return (false, "No se puede eliminar una legalización verificada");
                }

                await _adapter.EliminarAsync(id);

                _logger.LogInformation("Legalización {Id} eliminada por usuario {Usuario}", id, usuarioId);

                return (true, "Legalización eliminada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando legalización {Id}. Usuario={Usuario}", id, usuarioId);
                return (false, "Error al eliminar la legalización");
            }
        }

        public async Task<IEnumerable<LegalizacionDto>> ObtenerListadoAsync(
            string? busqueda = null,
            bool? verificado = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int pagina = 1,
            int pageSize = 20)
        {
            try
            {
                var legalizaciones = await _adapter.ObtenerTodosAsync();
                
                // Aplicar filtros
                var resultado = legalizaciones.AsEnumerable();
                
                if (verificado.HasValue)
                {
                    resultado = resultado.Where(l => l.Verificado == verificado.Value);
                }
                
                if (fechaInicio.HasValue)
                {
                    resultado = resultado.Where(l => l.Fecha >= fechaInicio.Value);
                }
                
                if (fechaFin.HasValue)
                {
                    resultado = resultado.Where(l => l.Fecha <= fechaFin.Value);
                }
                
                if (!string.IsNullOrEmpty(busqueda))
                {
                    resultado = resultado.Where(l => 
                        l.Radicado.Contains(busqueda, StringComparison.OrdinalIgnoreCase));
                }

                return resultado.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo listado de legalizaciones con paginación");
                throw;
            }
        }
    }
}
