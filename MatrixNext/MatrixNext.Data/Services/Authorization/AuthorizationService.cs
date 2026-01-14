using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.Authorization
{
    /// <summary>
    /// Implementación del servicio de autorización
    /// Sprint 10-11: Validación de permisos basada en roles y ownership
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<AuthorizationService> _logger;

        public AuthorizationService(IDbConnection connection, ILogger<AuthorizationService> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<bool> ValidarPermisoAsync(int usuarioId, string recurso, string accion, int? recursoId = null)
        {
            try
            {
                _logger.LogInformation($"[Auth] Validando permiso: Usuario={usuarioId}, Recurso={recurso}, Accion={accion}");

                // Validar permisos por rol
                var tienePermisoRol = await ValidarPermisoPorRolAsync(usuarioId, recurso, accion);
                if (tienePermisoRol)
                {
                    _logger.LogInformation($"[Auth] Permiso concedido por rol");
                    return true;
                }

                // Si hay recursoId, validar ownership
                if (recursoId.HasValue)
                {
                    var esPropietario = await EsPropietarioAsync(usuarioId, recurso, recursoId.Value);
                    if (esPropietario)
                    {
                        _logger.LogInformation($"[Auth] Permiso concedido por ownership");
                        return true;
                    }
                }

                _logger.LogWarning($"[Auth] Permiso denegado: Usuario={usuarioId}, Recurso={recurso}, Accion={accion}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Auth] Error validando permiso: Usuario={usuarioId}, Recurso={recurso}, Accion={accion}");
                // En caso de error, denegar acceso por seguridad
                return false;
            }
        }

        public async Task<bool> TieneRolAsync(int usuarioId, string rol)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@UsuarioId", usuarioId);
                parametros.Add("@Rol", rol);

                var resultado = await _connection.QueryFirstOrDefaultAsync<int>(
                    "US_Usuario_TieneRol",
                    parametros,
                    commandType: CommandType.StoredProcedure
                );

                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Auth] Error verificando rol: Usuario={usuarioId}, Rol={rol}");
                return false;
            }
        }

        public async Task<List<string>> ObtenerPermisosUsuarioAsync(int usuarioId)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@UsuarioId", usuarioId);

                var permisos = await _connection.QueryAsync<string>(
                    "US_Permisos_Get",
                    parametros,
                    commandType: CommandType.StoredProcedure
                );

                return permisos.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Auth] Error obteniendo permisos: Usuario={usuarioId}");
                return new List<string>();
            }
        }

        public async Task<bool> EsPropietarioAsync(int usuarioId, string recurso, int recursoId)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@UsuarioId", usuarioId);
                parametros.Add("@Recurso", recurso);
                parametros.Add("@RecursoId", recursoId);

                var resultado = await _connection.QueryFirstOrDefaultAsync<int>(
                    "AUTH_ValidarOwnership",
                    parametros,
                    commandType: CommandType.StoredProcedure
                );

                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Auth] Error validando ownership: Usuario={usuarioId}, Recurso={recurso}, RecursoId={recursoId}");
                return false;
            }
        }

        private async Task<bool> ValidarPermisoPorRolAsync(int usuarioId, string recurso, string accion)
        {
            try
            {
                // Roles con permisos completos
                var esAdmin = await TieneRolAsync(usuarioId, "Administrador");
                if (esAdmin) return true;

                // Validar permisos específicos por recurso
                var permisoRequerido = $"{recurso}.{accion}";
                var permisos = await ObtenerPermisosUsuarioAsync(usuarioId);

                return permisos.Contains(permisoRequerido) || permisos.Contains($"{recurso}.*");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Auth] Error en ValidarPermisoPorRolAsync");
                return false;
            }
        }
    }
}
