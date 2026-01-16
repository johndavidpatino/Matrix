using MatrixNext.Data.Adapters.INV;
using MatrixNext.Data.DTOs.INV;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.INV
{
    public class MantenimientoEquiposService : IMantenimientoEquiposService
    {
        private readonly IMantenimientoEquiposAdapter _adapter;
        private readonly IRegistroArticulosAdapter _articulosAdapter;
        private readonly ILogger<MantenimientoEquiposService> _logger;

        public MantenimientoEquiposService(
            IMantenimientoEquiposAdapter adapter,
            IRegistroArticulosAdapter articulosAdapter,
            ILogger<MantenimientoEquiposService> logger)
        {
            _adapter = adapter;
            _articulosAdapter = articulosAdapter;
            _logger = logger;
        }

        public async Task<IEnumerable<MantenimientoEquipoDto>> ObtenerTodosAsync(
            long? idActivoFijo = null,
            int? tipoMantenimiento = null,
            long? idUsuarioResponsable = null)
        {
            try
            {
                return await _adapter.ObtenerTodosAsync(
                    idActivoFijo: idActivoFijo,
                    tipoMantenimiento: tipoMantenimiento,
                    idUsuarioResponsable: idUsuarioResponsable
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo mantenimientos. Activo={Activo}, Tipo={Tipo}",
                    idActivoFijo, tipoMantenimiento);
                throw;
            }
        }

        public async Task<IEnumerable<MantenimientoEquipoDto>> ObtenerPorActivoAsync(long idActivoFijo)
        {
            try
            {
                return await _adapter.ObtenerPorActivoAsync(idActivoFijo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo histórico de mantenimientos del activo {IdActivoFijo}", idActivoFijo);
                throw;
            }
        }

        public async Task<MantenimientoEquipoDto?> ObtenerPorIdAsync(long id)
        {
            try
            {
                return await _adapter.ObtenerPorIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo mantenimiento {Id}", id);
                throw;
            }
        }

        public async Task<(bool success, string message, long id)> CrearAsync(MantenimientoEquipoDto dto, long usuarioId)
        {
            try
            {
                // Validar que el activo fijo existe
                var articulo = await _articulosAdapter.ObtenerPorIdAsync(dto.IdActivoFijo);
                if (articulo == null)
                {
                    return (false, "El activo fijo no existe", 0);
                }

                // Validar que el activo está asignado (para poder darle mantenimiento)
                if (!articulo.Asignado)
                {
                    _logger.LogWarning("Intento de registrar mantenimiento para activo {IdActivoFijo} no asignado. Usuario={Usuario}",
                        dto.IdActivoFijo, usuarioId);
                    // Permitir pero loguear advertencia
                }

                // Validar fecha no futura
                if (dto.Fecha > DateTime.Now)
                {
                    return (false, "La fecha de mantenimiento no puede ser futura", 0);
                }

                // Validar observaciones
                if (string.IsNullOrWhiteSpace(dto.Observaciones) || dto.Observaciones.Length < 10)
                {
                    return (false, "Las observaciones son requeridas y deben tener al menos 10 caracteres", 0);
                }

                var id = await _adapter.CrearAsync(dto, usuarioId);

                _logger.LogInformation("Mantenimiento {Id} registrado para activo {IdActivoFijo}. Tipo={TipoMantenimiento}, Usuario={Usuario}",
                    id, dto.IdActivoFijo, dto.TipoMantenimiento, usuarioId);

                return (true, "Mantenimiento registrado exitosamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando mantenimiento. Activo={IdActivoFijo}, Usuario={Usuario}",
                    dto.IdActivoFijo, usuarioId);
                return (false, "Error al registrar el mantenimiento", 0);
            }
        }

        public async Task<(bool success, string message)> ActualizarAsync(MantenimientoEquipoDto dto, long usuarioId)
        {
            try
            {
                // Validar que el mantenimiento existe
                var existente = await _adapter.ObtenerPorIdAsync(dto.Id);
                if (existente == null)
                {
                    return (false, "El mantenimiento no existe");
                }

                // Validar fecha no futura
                if (dto.Fecha > DateTime.Now)
                {
                    return (false, "La fecha de mantenimiento no puede ser futura");
                }

                // Validar observaciones
                if (string.IsNullOrWhiteSpace(dto.Observaciones) || dto.Observaciones.Length < 10)
                {
                    return (false, "Las observaciones son requeridas y deben tener al menos 10 caracteres");
                }

                await _adapter.ActualizarAsync(dto, usuarioId);

                _logger.LogInformation("Mantenimiento {Id} actualizado por usuario {Usuario}", dto.Id, usuarioId);

                return (true, "Mantenimiento actualizado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando mantenimiento {Id}. Usuario={Usuario}", dto.Id, usuarioId);
                return (false, "Error al actualizar el mantenimiento");
            }
        }

        public async Task<IEnumerable<MantenimientoEquipoDto>> ObtenerListadoAsync(
            string? busqueda = null,
            long? idActivoFijo = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            int pagina = 1,
            int pageSize = 20)
        {
            try
            {
                return await _adapter.ObtenerTodosAsync(
                    idActivoFijo: idActivoFijo
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo listado de mantenimientos con paginación");
                throw;
            }
        }
    }
}
