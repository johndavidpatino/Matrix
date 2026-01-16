using MatrixNext.Data.Adapters.INV;
using MatrixNext.Data.DTOs.INV;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.INV
{
    public class RegistroArticulosService : IRegistroArticulosService
    {
        private readonly IRegistroArticulosAdapter _adapter;
        private readonly ILogger<RegistroArticulosService> _logger;

        public RegistroArticulosService(
            IRegistroArticulosAdapter adapter,
            ILogger<RegistroArticulosService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<IEnumerable<RegistroArticuloListDto>> ObtenerTodosAsync(
            long? idTipoArticulo = null,
            long? idSede = null,
            bool? asignado = null,
            string? busqueda = null)
        {
            try
            {
                return await _adapter.ObtenerTodosAsync(
                    idTipoArticulo: idTipoArticulo,
                    idSede: idSede,
                    asignado: asignado,
                    todosCampos: busqueda
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo artículos. Filtros: TipoArticulo={TipoArticulo}, Sede={Sede}, Asignado={Asignado}",
                    idTipoArticulo, idSede, asignado);
                throw;
            }
        }

        public async Task<RegistroArticuloDto?> ObtenerPorIdAsync(long id)
        {
            try
            {
                return await _adapter.ObtenerPorIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo artículo {Id}", id);
                throw;
            }
        }

        public async Task<(bool success, string message, long id)> CrearAsync(RegistroArticuloDto dto, long usuarioId)
        {
            try
            {
                // Validar fecha de compra no sea futura
                if (dto.FechaCompra > DateTime.Now)
                {
                    return (false, "La fecha de compra no puede ser futura", 0);
                }

                // Validar valor unitario si está presente
                if (dto.ValorUnitario.HasValue && dto.ValorUnitario <= 0)
                {
                    return (false, "El valor unitario debe ser mayor a 0", 0);
                }

                // Validar cantidad para papelería/consumibles
                if (dto.IdProductoPapeleria.HasValue || dto.IdTipoProducto.HasValue)
                {
                    if (!dto.Cantidad.HasValue || dto.Cantidad <= 0)
                    {
                        return (false, "La cantidad es requerida y debe ser mayor a 0", 0);
                    }
                }

                var id = await _adapter.CrearAsync(dto, usuarioId);

                _logger.LogInformation("Artículo {Id} creado. TipoArticulo={TipoArticulo}, Articulo={Articulo}, Usuario={Usuario}",
                    id, dto.IdTipoArticulo, dto.IdArticulo, usuarioId);

                return (true, "Artículo creado exitosamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando artículo. TipoArticulo={TipoArticulo}, Usuario={Usuario}",
                    dto.IdTipoArticulo, usuarioId);
                return (false, "Error al crear el artículo", 0);
            }
        }

        public async Task<(bool success, string message)> ActualizarAsync(RegistroArticuloDto dto, long usuarioId)
        {
            try
            {
                // Validar que el artículo existe
                var existente = await _adapter.ObtenerPorIdAsync(dto.Id);
                if (existente == null)
                {
                    return (false, "El artículo no existe");
                }

                // Validar que no esté asignado si se intenta cambiar datos críticos
                if (existente.Asignado)
                {
                    _logger.LogWarning("Intento de modificar artículo {Id} que está asignado. Usuario={Usuario}",
                        dto.Id, usuarioId);
                    // Permitir actualización pero logear advertencia
                }

                // Validar fecha de compra
                if (dto.FechaCompra > DateTime.Now)
                {
                    return (false, "La fecha de compra no puede ser futura");
                }

                // Validar valor unitario
                if (dto.ValorUnitario.HasValue && dto.ValorUnitario <= 0)
                {
                    return (false, "El valor unitario debe ser mayor a 0");
                }

                await _adapter.ActualizarAsync(dto, usuarioId);

                _logger.LogInformation("Artículo {Id} actualizado por usuario {Usuario}", dto.Id, usuarioId);

                return (true, "Artículo actualizado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando artículo {Id}. Usuario={Usuario}", dto.Id, usuarioId);
                return (false, "Error al actualizar el artículo");
            }
        }

        public async Task<IEnumerable<RegistroArticuloListDto>> ObtenerDisponiblesAsync(long? idTipoArticulo = null)
        {
            try
            {
                return await _adapter.ObtenerDisponiblesAsync(idTipoArticulo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo artículos disponibles. TipoArticulo={TipoArticulo}", idTipoArticulo);
                throw;
            }
        }

        public async Task<IEnumerable<RegistroArticuloListDto>> ObtenerListadoAsync(
            string? busqueda = null,
            long? idTipoArticulo = null,
            bool? asignado = null,
            int pagina = 1,
            int pageSize = 20)
        {
            try
            {
                return await _adapter.ObtenerTodosAsync(
                    idTipoArticulo: idTipoArticulo,
                    asignado: asignado,
                    todosCampos: busqueda
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo listado de artículos con paginación");
                throw;
            }
        }

        public async Task<(bool success, string message)> EliminarAsync(long id, long usuarioId)
        {
            try
            {
                var articulo = await _adapter.ObtenerPorIdAsync(id);
                if (articulo == null)
                {
                    return (false, "El artículo no existe");
                }

                if (articulo.Asignado)
                {
                    _logger.LogWarning("Intento de eliminar artículo {Id} que está asignado. Usuario={Usuario}",
                        id, usuarioId);
                    return (false, "No se puede eliminar un artículo que está asignado");
                }

                await _adapter.EliminarAsync(id);

                _logger.LogInformation("Artículo {Id} eliminado por usuario {Usuario}", id, usuarioId);

                return (true, "Artículo eliminado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando artículo {Id}. Usuario={Usuario}", id, usuarioId);
                return (false, "Error al eliminar el artículo");
            }
        }
    }
}
