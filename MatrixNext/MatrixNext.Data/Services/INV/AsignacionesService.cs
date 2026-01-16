using MatrixNext.Data.Adapters.INV;
using MatrixNext.Data.DTOs.INV;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.INV
{
    public class AsignacionesService : IAsignacionesService
    {
        private readonly IAsignacionesAdapter _adapter;
        private readonly IRegistroArticulosAdapter _articulosAdapter;
        private readonly ILogger<AsignacionesService> _logger;

        public AsignacionesService(
            IAsignacionesAdapter adapter,
            IRegistroArticulosAdapter articulosAdapter,
            ILogger<AsignacionesService> logger)
        {
            _adapter = adapter;
            _articulosAdapter = articulosAdapter;
            _logger = logger;
        }

        public async Task<IEnumerable<AsignacionListDto>> ObtenerTodosAsync(
            long? idUsuarioAsignado = null,
            int? idBU = null,
            string? jobBookCodigo = null)
        {
            try
            {
                return await _adapter.ObtenerTodosAsync(
                    idUsuarioAsignado: idUsuarioAsignado,
                    idBU: idBU,
                    jobBookCodigo: jobBookCodigo
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo asignaciones. Usuario={Usuario}, BU={BU}",
                    idUsuarioAsignado, idBU);
                throw;
            }
        }

        public async Task<AsignacionActivoDto?> ObtenerPorIdAsync(long idActivoFijo)
        {
            try
            {
                return await _adapter.ObtenerPorIdAsync(idActivoFijo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo asignación del activo {IdActivoFijo}", idActivoFijo);
                throw;
            }
        }

        public async Task<(bool success, string message, long id)> CrearAsync(AsignacionActivoDto dto, long usuarioId)
        {
            try
            {
                // Validar que el activo fijo existe y está disponible
                var articulo = await _articulosAdapter.ObtenerPorIdAsync(dto.IdActivoFijo);
                if (articulo == null)
                {
                    return (false, "El activo fijo no existe", 0);
                }

                if (articulo.Asignado)
                {
                    return (false, "El activo fijo ya está asignado a otro usuario", 0);
                }

                // Validar fecha de asignación
                if (dto.FechaAsignacion > DateTime.Now)
                {
                    return (false, "La fecha de asignación no puede ser futura", 0);
                }

                // Crear asignación
                var id = await _adapter.CrearAsync(dto, usuarioId);

                // Crear log de auditoría
                await _adapter.CrearLogAsync(
                    dto.IdActivoFijo,
                    articulo.IdArticulo,
                    dto.IdUsuarioAsignado,
                    asignado: true
                );

                // Actualizar estado del artículo a asignado
                await _articulosAdapter.ActualizarAsignadoAsync(dto.IdActivoFijo, asignado: true);

                _logger.LogInformation("Activo {IdActivoFijo} asignado a usuario {IdUsuarioAsignado}. Asignación={Id}, UsuarioRegistra={UsuarioRegistra}",
                    dto.IdActivoFijo, dto.IdUsuarioAsignado, id, usuarioId);

                return (true, "Asignación creada exitosamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando asignación. Activo={IdActivoFijo}, Usuario={Usuario}",
                    dto.IdActivoFijo, usuarioId);
                return (false, "Error al crear la asignación", 0);
            }
        }

        public async Task<(bool success, string message)> ActualizarAsync(AsignacionActivoDto dto, long usuarioId)
        {
            try
            {
                // Validar que la asignación existe
                var existente = await _adapter.ObtenerPorIdAsync(dto.IdActivoFijo);
                if (existente == null)
                {
                    return (false, "La asignación no existe");
                }

                // Validar fecha
                if (dto.FechaAsignacion > DateTime.Now)
                {
                    return (false, "La fecha de asignación no puede ser futura");
                }

                await _adapter.ActualizarAsync(dto, usuarioId);

                _logger.LogInformation("Asignación del activo {IdActivoFijo} actualizada por usuario {Usuario}",
                    dto.IdActivoFijo, usuarioId);

                return (true, "Asignación actualizada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando asignación. Activo={IdActivoFijo}, Usuario={Usuario}",
                    dto.IdActivoFijo, usuarioId);
                return (false, "Error al actualizar la asignación");
            }
        }

        public async Task<(bool success, string message)> EliminarAsync(long idActivoFijo, long usuarioId)
        {
            try
            {
                // Validar que la asignación existe
                var asignacion = await _adapter.ObtenerPorIdAsync(idActivoFijo);
                if (asignacion == null)
                {
                    return (false, "La asignación no existe");
                }

                // Obtener artículo
                var articulo = await _articulosAdapter.ObtenerPorIdAsync(idActivoFijo);
                if (articulo == null)
                {
                    return (false, "El activo fijo no existe");
                }

                // Eliminar asignación
                await _adapter.EliminarAsync(idActivoFijo);

                // Crear log de auditoría (devolución)
                await _adapter.CrearLogAsync(
                    idActivoFijo,
                    articulo.IdArticulo,
                    asignacion.IdUsuarioAsignado,
                    asignado: false
                );

                // Actualizar estado del artículo a disponible
                await _articulosAdapter.ActualizarAsignadoAsync(idActivoFijo, asignado: false);

                _logger.LogInformation("Asignación del activo {IdActivoFijo} eliminada (devuelto) por usuario {Usuario}",
                    idActivoFijo, usuarioId);

                return (true, "Asignación eliminada exitosamente. El activo está ahora disponible");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando asignación. Activo={IdActivoFijo}, Usuario={Usuario}",
                    idActivoFijo, usuarioId);
                return (false, "Error al eliminar la asignación");
            }
        }

        public async Task<IEnumerable<AsignacionListDto>> ObtenerListadoAsync(
            string? busqueda = null,
            long? idUsuarioAsignado = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            int pagina = 1,
            int pageSize = 20)
        {
            try
            {
                return await _adapter.ObtenerTodosAsync(
                    idUsuarioAsignado: idUsuarioAsignado,
                    jobBookCodigo: busqueda
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo listado de asignaciones con paginación");
                throw;
            }
        }
    }
}
