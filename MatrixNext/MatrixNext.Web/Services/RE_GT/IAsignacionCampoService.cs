using MatrixNext.Data.DTOs.RE_GT;

namespace MatrixNext.Web.Services.RE_GT
{
    /// <summary>
    /// Interfaz para servicio de asignación de trabajos a coordinadores de campo
    /// </summary>
    public interface IAsignacionCampoService
    {
        /// <summary>
        /// Obtiene lista paginada de trabajos sin asignación
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
        /// Valida que el trabajo exista y esté en estado válido para asignación
        /// </summary>
        Task<(bool valid, string message)> ValidarTrabajoAsync(int idTrabajo);

        /// <summary>
        /// Realiza la asignación del trabajo a coordinador de campo
        /// </summary>
        Task<(bool success, string message)> AsignarTrabajoCampoAsync(
            AsignacionCampoDto dto, int usuarioId);

        /// <summary>
        /// Obtiene lista de COEs (Coordinadores de Operaciones Especializadas)
        /// </summary>
        Task<IEnumerable<dynamic>> ObtenerCOEsAsync();
    }
}
