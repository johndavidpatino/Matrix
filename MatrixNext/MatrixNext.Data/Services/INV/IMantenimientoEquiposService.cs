using MatrixNext.Data.DTOs.INV;

namespace MatrixNext.Data.Services.INV
{
    /// <summary>
    /// Interfaz para lógica de negocio de mantenimiento de equipos.
    /// </summary>
    public interface IMantenimientoEquiposService
    {
        /// <summary>
        /// Obtiene todos los mantenimientos con filtros opcionales.
        /// </summary>
        Task<IEnumerable<MantenimientoEquipoDto>> ObtenerTodosAsync(
            long? idActivoFijo = null,
            int? tipoMantenimiento = null,
            long? idUsuarioResponsable = null);

        /// <summary>
        /// Obtiene el histórico de mantenimientos de un activo.
        /// </summary>
        Task<IEnumerable<MantenimientoEquipoDto>> ObtenerPorActivoAsync(long idActivoFijo);

        /// <summary>
        /// Obtiene un mantenimiento por su ID.
        /// </summary>
        Task<MantenimientoEquipoDto?> ObtenerPorIdAsync(long id);

        /// <summary>
        /// Crea un nuevo mantenimiento validando reglas de negocio.
        /// </summary>
        Task<(bool success, string message, long id)> CrearAsync(MantenimientoEquipoDto dto, long usuarioId);

        /// <summary>
        /// Actualiza un mantenimiento existente.
        /// </summary>
        Task<(bool success, string message)> ActualizarAsync(MantenimientoEquipoDto dto, long usuarioId);

        /// <summary>
        /// Obtiene listado paginado de mantenimientos con filtros.
        /// </summary>
        Task<IEnumerable<MantenimientoEquipoDto>> ObtenerListadoAsync(
            string? busqueda = null,
            long? idActivoFijo = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            int pagina = 1,
            int pageSize = 20);
    }
}
