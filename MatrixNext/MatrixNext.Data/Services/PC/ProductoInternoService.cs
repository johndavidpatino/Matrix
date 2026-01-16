using MatrixNext.Data.Adapters.PC;
using MatrixNext.Data.DTOs.PC;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.PC
{
    /// <summary>
    /// Servicio para lógica de negocio de productos internos
    /// </summary>
    public class ProductoInternoService : IProductoInternoService
    {
        private readonly IProductoInternoAdapter _adapter;
        private readonly ILogger<ProductoInternoService> _logger;

        public ProductoInternoService(
            IProductoInternoAdapter adapter,
            ILogger<ProductoInternoService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductoInternoListDto>> ObtenerTodosAsync()
        {
            return await _adapter.ObtenerTodosAsync();
        }

        public async Task<IEnumerable<ProductoInternoListDto>> ObtenerFiltradosAsync(
            int? unidadId = null, 
            int? proyectoId = null, 
            bool soloEnviados = false, 
            bool soloPendientes = false)
        {
            IEnumerable<ProductoInternoListDto> productos;

            if (unidadId.HasValue)
            {
                if (soloEnviados)
                {
                    productos = await _adapter.ObtenerPorUnidadEnviaAsync(unidadId.Value, proyectoId);
                }
                else
                {
                    productos = await _adapter.ObtenerPorUnidadRecibeAsync(unidadId.Value, proyectoId);
                }
            }
            else
            {
                productos = await _adapter.ObtenerTodosAsync();
            }

            if (soloPendientes)
            {
                productos = productos.Where(p => !p.FechaRecepcion.HasValue);
            }

            if (proyectoId.HasValue && !unidadId.HasValue)
            {
                productos = productos.Where(p => p.ProyectoId == proyectoId.Value);
            }

            return productos;
        }

        public async Task<ProductoInternoListDto?> ObtenerPorIdAsync(int id)
        {
            return await _adapter.ObtenerPorIdAsync(id);
        }

        public async Task<(bool success, string message, int id)> CrearAsync(ProductoInternoDto dto, int userId)
        {
            try
            {
                // Validación: Cantidad debe ser mayor a 0
                if (dto.Cantidad <= 0)
                {
                    return (false, "La cantidad debe ser mayor a 0", 0);
                }

                // Validación: UnidadEnvia debe ser diferente de UnidadRecibe
                if (dto.UnidadEnvia == dto.UnidadRecibe)
                {
                    return (false, "La unidad que envía no puede ser la misma que recibe", 0);
                }

                // Validación: Producto no puede estar vacío
                if (string.IsNullOrWhiteSpace(dto.Producto))
                {
                    return (false, "El nombre del producto es requerido", 0);
                }

                // Establecer fecha de envío si no se proporcionó
                if (!dto.FechaEnvio.HasValue)
                {
                    dto.FechaEnvio = DateTime.Now;
                }

                var id = await _adapter.CrearAsync(dto, userId);

                _logger.LogInformation(
                    "Producto interno {Id} creado. Producto: {Producto}, Cantidad: {Cantidad}, Usuario: {UserId}",
                    id, dto.Producto, dto.Cantidad, userId);

                return (true, "Producto creado exitosamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando producto interno. Usuario: {UserId}, Producto: {Producto}",
                    userId, dto.Producto);
                return (false, "Error al crear el producto. Por favor intente nuevamente.", 0);
            }
        }

        public async Task<(bool success, string message)> ActualizarAsync(ProductoInternoDto dto, int userId)
        {
            try
            {
                // Validaciones similares a Crear
                if (dto.Cantidad <= 0)
                {
                    return (false, "La cantidad debe ser mayor a 0");
                }

                if (dto.UnidadEnvia == dto.UnidadRecibe)
                {
                    return (false, "La unidad que envía no puede ser la misma que recibe");
                }

                if (string.IsNullOrWhiteSpace(dto.Producto))
                {
                    return (false, "El nombre del producto es requerido");
                }

                // Validar que el producto existe
                var productoExistente = await _adapter.ObtenerPorIdAsync(dto.Id);
                if (productoExistente == null)
                {
                    return (false, "Producto no encontrado");
                }

                // No permitir editar si ya fue recibido
                if (productoExistente.FechaRecepcion.HasValue)
                {
                    return (false, "No se puede editar un producto que ya fue recibido");
                }

                var actualizado = await _adapter.ActualizarAsync(dto, userId);

                if (actualizado)
                {
                    _logger.LogInformation(
                        "Producto interno {Id} actualizado por usuario {UserId}",
                        dto.Id, userId);
                    return (true, "Producto actualizado exitosamente");
                }

                return (false, "No se pudo actualizar el producto");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando producto interno {Id}. Usuario: {UserId}",
                    dto.Id, userId);
                return (false, "Error al actualizar el producto. Por favor intente nuevamente.");
            }
        }

        public async Task<(bool success, string message)> RegistrarEnvioAsync(int id, int userId)
        {
            try
            {
                var producto = await _adapter.ObtenerPorIdAsync(id);
                if (producto == null)
                {
                    return (false, "Producto no encontrado");
                }

                if (producto.FechaEnvio.HasValue)
                {
                    return (false, "El producto ya fue enviado");
                }

                // Actualizar solo cantidad podría usarse para confirmar envío
                // En este caso, simplemente registramos que el envío se confirmó
                _logger.LogInformation("Envío de producto {Id} confirmado por usuario {UserId}", id, userId);
                
                return (true, "Envío registrado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando envío de producto {Id}", id);
                return (false, "Error al registrar el envío");
            }
        }

        public async Task<(bool success, string message)> RegistrarRecepcionAsync(int id, int recibeUsuarioId, string? observaciones)
        {
            try
            {
                var producto = await _adapter.ObtenerPorIdAsync(id);
                if (producto == null)
                {
                    return (false, "Producto no encontrado");
                }

                if (producto.FechaRecepcion.HasValue)
                {
                    return (false, "El producto ya fue recibido previamente");
                }

                var recibido = await _adapter.RegistrarRecepcionAsync(
                    id, 
                    recibeUsuarioId, 
                    DateTime.Now, 
                    observaciones);

                if (recibido)
                {
                    _logger.LogInformation(
                        "Recepción de producto {Id} registrada. Usuario: {UserId}, Observaciones: {Obs}",
                        id, recibeUsuarioId, observaciones ?? "N/A");
                    return (true, "Recepción registrada exitosamente");
                }

                return (false, "No se pudo registrar la recepción");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando recepción de producto {Id}", id);
                return (false, "Error al registrar la recepción. Por favor intente nuevamente.");
            }
        }

        public async Task<(bool success, string message)> EliminarAsync(int id, int userId)
        {
            try
            {
                var producto = await _adapter.ObtenerPorIdAsync(id);
                if (producto == null)
                {
                    return (false, "Producto no encontrado");
                }

                // No permitir eliminar si ya fue recibido
                if (producto.FechaRecepcion.HasValue)
                {
                    return (false, "No se puede eliminar un producto que ya fue recibido");
                }

                var eliminado = await _adapter.EliminarAsync(id);

                if (eliminado)
                {
                    _logger.LogInformation(
                        "Producto interno {Id} eliminado por usuario {UserId}",
                        id, userId);
                    return (true, "Producto eliminado exitosamente");
                }

                return (false, "No se pudo eliminar el producto");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando producto {Id}. Usuario: {UserId}", id, userId);
                return (false, "Error al eliminar el producto. Por favor intente nuevamente.");
            }
        }

        public async Task<bool> PuedeEditarAsync(int productoId, int userId)
        {
            var producto = await _adapter.ObtenerPorIdAsync(productoId);
            
            if (producto == null)
                return false;

            // No se puede editar si ya fue recibido
            if (producto.FechaRecepcion.HasValue)
                return false;

            // Solo el usuario que envía puede editar (o admin - agregar lógica de roles si es necesario)
            return producto.Envia == userId;
        }
    }
}
