using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace MatrixNext.Web.Services
{
    /// <summary>
    /// Implementación de IPYPermisosService
    /// Conecta a tablas US_PermisosUsuarios y US_RolesUsuarios en BD legacy
    /// Ref: MATRIZ_PERMISOS_ROLES.md § 5.1
    /// ISSUE RESUELTO: Sprint 0 GAP-0.1 / Sprint 6 GAP-6.1
    /// </summary>
    public class PYPermisosService : IPYPermisosService
    {
        private readonly ILogger<PYPermisosService> _logger;
        private readonly string _connectionString;

        public PYPermisosService(
            ILogger<PYPermisosService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("LegacyDatabase") 
                ?? throw new InvalidOperationException("LegacyDatabase connection string not found");
        }

        /// <summary>
        /// Verifica si un usuario tiene un permiso específico
        /// Consulta: SELECT COUNT(*) FROM US_PermisosUsuarios
        ///          WHERE UsuarioId = @UsuarioId AND PermisoId = @PermisoId
        /// </summary>
        public async Task<bool> VerificarPermisoAsync(int permisoId, long usuarioId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    const string query = @"
                        SELECT COUNT(*) 
                        FROM US_PermisosUsuarios 
                        WHERE UsuarioId = @UsuarioId 
                          AND PermisoId = @PermisoId";

                    var count = await connection.QueryFirstOrDefaultAsync<int>(
                        query,
                        new { UsuarioId = usuarioId, PermisoId = permisoId },
                        commandType: CommandType.Text);

                    var tienePermiso = count > 0;

                    _logger.LogInformation(
                        "Permiso {PermisoId} para usuario {UsuarioId}: {Resultado}",
                        permisoId, usuarioId, tienePermiso ? "Aprobado" : "Denegado");

                    return tienePermiso;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando permiso {PermisoId} para usuario {UsuarioId}",
                    permisoId, usuarioId);
                return false; // Por seguridad, retorna false si hay error
            }
        }

        /// <summary>
        /// Verifica si un usuario tiene un rol específico
        /// Consulta: SELECT COUNT(*) FROM US_RolesUsuarios ur
        ///          JOIN US_Roles r ON ur.RolId = r.id
        ///          WHERE ur.UsuarioId = @UsuarioId AND r.Rol = @Rol
        /// </summary>
        public async Task<bool> VerificarRolAsync(long usuarioId, string rolNombre)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    const string query = @"
                        SELECT COUNT(*) 
                        FROM US_RolesUsuarios ur
                        INNER JOIN US_Roles r ON ur.RolId = r.id
                        WHERE ur.UsuarioId = @UsuarioId 
                          AND r.Rol = @Rol";

                    var count = await connection.QueryFirstOrDefaultAsync<int>(
                        query,
                        new { UsuarioId = usuarioId, Rol = rolNombre },
                        commandType: CommandType.Text);

                    var tieneRol = count > 0;

                    _logger.LogInformation(
                        "Rol {RolNombre} para usuario {UsuarioId}: {Resultado}",
                        rolNombre, usuarioId, tieneRol ? "Aprobado" : "Denegado");

                    return tieneRol;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando rol {RolNombre} para usuario {UsuarioId}",
                    rolNombre, usuarioId);
                return false; // Por seguridad, retorna false
            }
        }

        /// <summary>
        /// Obtiene lista de permisos de un usuario
        /// Consulta: SELECT DISTINCT PermisoId 
        ///          FROM US_PermisosUsuarios 
        ///          WHERE UsuarioId = @UsuarioId
        /// </summary>
        public async Task<List<int>> ObtenerPermisosUsuarioAsync(long usuarioId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    const string query = @"
                        SELECT DISTINCT PermisoId 
                        FROM US_PermisosUsuarios 
                        WHERE UsuarioId = @UsuarioId";

                    var permisos = (await connection.QueryAsync<int>(
                        query,
                        new { UsuarioId = usuarioId },
                        commandType: CommandType.Text)).ToList();

                    _logger.LogInformation(
                        "Obtenidos {CantidadPermisos} permisos para usuario {UsuarioId}",
                        permisos.Count, usuarioId);

                    return permisos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo permisos para usuario {UsuarioId}", usuarioId);
                return new List<int>(); // Retorna lista vacía como fallback (seguridad)
            }
        }
    }
}
