using System.Threading.Tasks;

namespace MatrixNext.Data.Services.Authorization
{
    /// <summary>
    /// Servicio de autorización para validar permisos de usuarios
    /// Sprint 10-11: Implementación de validación de permisos
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// Valida si un usuario tiene permiso para realizar una acción sobre un recurso
        /// </summary>
        /// <param name="usuarioId">ID del usuario</param>
        /// <param name="recurso">Nombre del recurso (ej: "Reporte", "Revision", "Trafico")</param>
        /// <param name="accion">Acción a realizar (ej: "Ver", "Crear", "Editar", "Eliminar", "Aprobar")</param>
        /// <param name="recursoId">ID opcional del recurso específico</param>
        /// <returns>True si tiene permiso, False en caso contrario</returns>
        Task<bool> ValidarPermisoAsync(int usuarioId, string recurso, string accion, int? recursoId = null);

        /// <summary>
        /// Valida si un usuario tiene un rol específico
        /// </summary>
        /// <param name="usuarioId">ID del usuario</param>
        /// <param name="rol">Nombre del rol</param>
        /// <returns>True si tiene el rol, False en caso contrario</returns>
        Task<bool> TieneRolAsync(int usuarioId, string rol);

        /// <summary>
        /// Obtiene todos los permisos de un usuario
        /// </summary>
        /// <param name="usuarioId">ID del usuario</param>
        /// <returns>Lista de permisos del usuario</returns>
        Task<List<string>> ObtenerPermisosUsuarioAsync(int usuarioId);

        /// <summary>
        /// Valida si un usuario es propietario de un recurso
        /// </summary>
        /// <param name="usuarioId">ID del usuario</param>
        /// <param name="recurso">Nombre del recurso</param>
        /// <param name="recursoId">ID del recurso</param>
        /// <returns>True si es propietario, False en caso contrario</returns>
        Task<bool> EsPropietarioAsync(int usuarioId, string recurso, int recursoId);
    }
}
