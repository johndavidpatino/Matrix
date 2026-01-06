namespace MatrixNext.Web.Services
{
    /// <summary>
    /// Interface para validación de permisos
    /// Ref: MATRIZ_PERMISOS_ROLES.md § 5.1
    /// </summary>
    public interface IPYPermisosService
    {
        /// <summary>
        /// Verifica si usuario tiene un permiso específico
        /// </summary>
        Task<bool> VerificarPermisoAsync(int permisoId, long usuarioId);

        /// <summary>
        /// Verifica si usuario tiene un rol específico
        /// </summary>
        Task<bool> VerificarRolAsync(long usuarioId, string rolNombre);

        /// <summary>
        /// Obtiene permisos del usuario
        /// </summary>
        Task<List<int>> ObtenerPermisosUsuarioAsync(long usuarioId);
    }
}
