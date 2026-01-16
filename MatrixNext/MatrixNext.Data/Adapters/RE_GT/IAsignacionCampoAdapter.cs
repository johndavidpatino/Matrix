using MatrixNext.Core.DTOs.RE_GT;

namespace MatrixNext.Data.Adapters.RE_GT
{
    /// <summary>
    /// Interfaz para adapter de asignación de trabajos a coordinadores de campo
    /// </summary>
    public interface IAsignacionCampoAdapter
    {
        /// <summary>
        /// Obtiene lista paginada de trabajos para asignación
        /// </summary>
        Task<(IEnumerable<TrabajoAsignacionDto> trabajos, int totalRecords)> ObtenerTrabajosParaAsignacionAsync(
            BusquedaAsignacionDto busqueda);

        /// <summary>
        /// Obtiene información del trabajo por ID
        /// </summary>
        Task<TrabajoAsignacionDto> ObtenerTrabajoAsync(int idTrabajo);

        /// <summary>
        /// Obtiene lista de usuarios COE disponibles
        /// </summary>
        Task<IEnumerable<UsuarioCOEDto>> ObtenerUsuariosCOEAsync();

        /// <summary>
        /// Realiza la asignación del trabajo
        /// </summary>
        Task AsignarTrabajoCampoAsync(AsignacionCampoDto dto);

        /// <summary>
        /// Registra el cambio en tabla de auditoria
        /// </summary>
        Task GuardarLogAsignacionAsync(LogAsignacionCampoDto dto);

        /// <summary>
        /// Obtiene lista de COEs
        /// </summary>
        Task<IEnumerable<dynamic>> ObtenerCOEsAsync();
    }
}
