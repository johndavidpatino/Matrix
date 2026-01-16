using MatrixNext.Data.DTOs.INV;

namespace MatrixNext.Data.Adapters.INV
{
    /// <summary>
    /// Interfaz para operaciones de datos de mantenimiento de equipos.
    /// </summary>
    public interface IMantenimientoEquiposAdapter
    {
        /// <summary>
        /// Obtiene todos los mantenimientos con filtros opcionales.
        /// </summary>
        Task<IEnumerable<MantenimientoEquipoDto>> ObtenerTodosAsync(
            long? id = null,
            long? idActivoFijo = null,
            long? idArticulo = null,
            int? tipoMantenimiento = null,
            long? idUsuarioResponsable = null);

        /// <summary>
        /// Obtiene mantenimientos de un activo fijo específico.
        /// </summary>
        Task<IEnumerable<MantenimientoEquipoDto>> ObtenerPorActivoAsync(long idActivoFijo);

        /// <summary>
        /// Obtiene un mantenimiento por su ID.
        /// </summary>
        Task<MantenimientoEquipoDto?> ObtenerPorIdAsync(long id);

        /// <summary>
        /// Crea un nuevo mantenimiento y retorna su ID.
        /// </summary>
        Task<long> CrearAsync(MantenimientoEquipoDto dto, long usuarioId);

        /// <summary>
        /// Actualiza un mantenimiento existente.
        /// </summary>
        Task ActualizarAsync(MantenimientoEquipoDto dto, long usuarioId);
    }
}
