using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace MatrixNext.Data.Modules.US.Usuarios.Adapters
{
    /// <summary>
    /// Adaptador para acceder a datos de Usuarios usando Dapper
    /// Proporciona acceso directo a base de datos sin dependencias de CoreProject
    /// </summary>
    public class UsuarioDataAdapter : IDisposable
    {
        private readonly string _connectionString;

        public UsuarioDataAdapter(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        #region Usuarios CRUD

        /// <summary>
        /// Obtiene todos los usuarios de la base de datos
        /// </summary>
        public List<UsuarioDTO> ObtenerTodosUsuarios()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var usuarios = conn.Query<UsuarioDTO>(
                        "SELECT Id, Usuario, Nombres, Apellidos, Email, Password, Activo FROM US_Usuarios ORDER BY Usuario"
                    ).ToList();
                    return usuarios ?? new List<UsuarioDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al obtener usuarios", ex);
            }
        }

        /// <summary>
        /// Obtiene un usuario específico por su Id
        /// </summary>
        public UsuarioDTO? ObtenerUsuarioPorId(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var usuario = conn.QueryFirstOrDefault<UsuarioDTO>(
                        "SELECT Id, Usuario, Nombres, Apellidos, Email, Password, Activo FROM US_Usuarios WHERE Id = @Id",
                        new { Id = id }
                    );
                    return usuario;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener usuario con Id {id}", ex);
            }
        }

        /// <summary>
        /// Obtiene usuarios filtrados por estado (Activo/Inactivo)
        /// </summary>
        public List<UsuarioDTO> ObtenerUsuariosPorActivo(bool activo = true)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var usuarios = conn.Query<UsuarioDTO>(
                        "SELECT Id, Usuario, Nombres, Apellidos, Email, Password, Activo FROM US_Usuarios WHERE Activo = @Activo ORDER BY Usuario",
                        new { Activo = activo }
                    ).ToList();
                    return usuarios ?? new List<UsuarioDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al obtener usuarios activos", ex);
            }
        }

        /// <summary>
        /// Obtiene un usuario por su nombre de usuario (login)
        /// </summary>
        public UsuarioDTO? ObtenerUsuarioPorUsuario(string? usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return null;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var result = conn.QueryFirstOrDefault<UsuarioDTO>(
                        "SELECT Id, Usuario, Nombres, Apellidos, Email, Password, Activo FROM US_Usuarios WHERE Usuario = @Usuario",
                        new { Usuario = usuario }
                    );
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener usuario {usuario}", ex);
            }
        }

        /// <summary>
        /// Crea un nuevo usuario en la base de datos
        /// </summary>
        public int CrearUsuario(UsuarioDTO usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var sql = @"INSERT INTO US_Usuarios (Usuario, Nombres, Apellidos, Email, Password, Activo) 
                               OUTPUT INSERTED.Id
                               VALUES (@Usuario, @Nombres, @Apellidos, @Email, @Password, @Activo)";
                    
                    int newId = conn.QuerySingle<int>(sql, usuario);
                    return newId;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al crear usuario", ex);
            }
        }

        /// <summary>
        /// Actualiza un usuario existente (excepto contraseña)
        /// </summary>
        public bool ActualizarUsuario(UsuarioDTO usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    int result = conn.Execute(
                        @"UPDATE US_Usuarios SET Nombres = @Nombres, Apellidos = @Apellidos, Email = @Email, Activo = @Activo 
                          WHERE Id = @Id",
                        usuario
                    );
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al actualizar usuario", ex);
            }
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario
        /// </summary>
        public bool ActualizarPassword(int usuarioId, string passwordEncriptada)
        {
            if (string.IsNullOrEmpty(passwordEncriptada))
                throw new ArgumentException("La contraseña no puede estar vacía", nameof(passwordEncriptada));

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    int result = conn.Execute(
                        "UPDATE US_Usuarios SET Password = @Password WHERE Id = @Id",
                        new { Password = passwordEncriptada, Id = usuarioId }
                    );
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al actualizar contraseña", ex);
            }
        }

        /// <summary>
        /// Elimina un usuario (soft delete - marca como inactivo)
        /// </summary>
        public bool EliminarUsuario(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    int result = conn.Execute(
                        "UPDATE US_Usuarios SET Activo = 0 WHERE Id = @Id",
                        new { Id = id }
                    );
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al eliminar usuario con Id {id}", ex);
            }
        }

        #endregion

        #region Roles - assign/unassign

        /// <summary>
        /// Asigna un rol a un usuario (evita duplicados)
        /// </summary>
        public bool GuardarRolUsuario(int usuarioId, int rolId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    // Evitar duplicados
                    var exists = conn.QueryFirstOrDefault<int?>(
                        "SELECT 1 FROM US_RolesUsuarios WHERE UsuarioId = @UsuarioId AND RolId = @RolId",
                        new { UsuarioId = usuarioId, RolId = rolId });
                    if (exists.HasValue)
                        return false;

                    int result = conn.Execute(
                        "INSERT INTO US_RolesUsuarios (UsuarioId, RolId) VALUES (@UsuarioId, @RolId)",
                        new { UsuarioId = usuarioId, RolId = rolId });
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al asignar rol al usuario {usuarioId}", ex);
            }
        }

        public bool EliminarRolUsuario(int usuarioId, int rolId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    int result = conn.Execute(
                        "DELETE FROM US_RolesUsuarios WHERE UsuarioId = @UsuarioId AND RolId = @RolId",
                        new { UsuarioId = usuarioId, RolId = rolId });
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al eliminar rol del usuario {usuarioId}", ex);
            }
        }

        /// <summary>
        /// Obtiene roles disponibles (no asignados) para un usuario
        /// </summary>
        public List<RolDTO> ObtenerRolesNoAsignados(int usuarioId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                                        var roles = conn.Query<RolDTO>(
                                                @"SELECT r.id AS Id, r.Rol AS Rol, '' AS Descripcion, 1 AS Activo
                                                    FROM US_Roles r
                                                    WHERE r.id NOT IN (SELECT RolId FROM US_RolesUsuarios WHERE UsuarioId = @UsuarioId)
                                                    ORDER BY r.Rol",
                        new { UsuarioId = usuarioId }
                    ).ToList();
                    return roles ?? new List<RolDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener roles no asignados para usuario {usuarioId}", ex);
            }
        }

        #endregion

        #region Roles

        /// <summary>
        /// Obtiene los roles asignados a un usuario
        /// </summary>
        public List<RolDTO> ObtenerRolesUsuario(int usuarioId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                                        var roles = conn.Query<RolDTO>(
                                                @"SELECT DISTINCT r.id AS Id, r.Rol AS Rol, '' AS Descripcion, 1 AS Activo
                                                    FROM US_RolesUsuarios ru
                                                    INNER JOIN US_Roles r ON ru.RolId = r.id
                                                    WHERE ru.UsuarioId = @UsuarioId
                                                    ORDER BY r.Rol",
                                                new { UsuarioId = usuarioId }
                                        ).ToList();
                    return roles ?? new List<RolDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener roles del usuario {usuarioId}", ex);
            }
        }

        /// <summary>
        /// Obtiene todos los roles disponibles
        /// </summary>
        public List<RolDTO> ObtenerTodosRoles()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var roles = conn.Query<RolDTO>(
                        "SELECT id AS Id, Rol AS Rol, '' AS Descripcion, 1 AS Activo FROM US_Roles ORDER BY Rol"
                    ).ToList();
                    return roles ?? new List<RolDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al obtener roles", ex);
            }
        }

        #endregion

        #region Unidades - assign/unassign

        public bool GuardarUsuariosUnidades(int usuarioId, int grupoUnidadId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var exists = conn.QueryFirstOrDefault<int?>(
                        "SELECT 1 FROM US_UsuariosUnidades WHERE UsuarioId = @UsuarioId AND UnidadId = @UnidadId",
                        new { UsuarioId = usuarioId, UnidadId = grupoUnidadId });
                    if (exists.HasValue)
                        return false;

                    int result = conn.Execute(
                        "INSERT INTO US_UsuariosUnidades (UsuarioId, UnidadId) VALUES (@UsuarioId, @UnidadId)",
                        new { UsuarioId = usuarioId, UnidadId = grupoUnidadId });
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al asignar unidad al usuario {usuarioId}", ex);
            }
        }

        public bool EliminarUsuariosUnidades(int usuarioId, int grupoUnidadId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    int result = conn.Execute(
                        "DELETE FROM US_UsuariosUnidades WHERE UsuarioId = @UsuarioId AND UnidadId = @UnidadId",
                        new { UsuarioId = usuarioId, UnidadId = grupoUnidadId });
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al eliminar unidad del usuario {usuarioId}", ex);
            }
        }

        public List<UnidadDTO> ObtenerUnidadesNoAsignadas(int usuarioId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                                        var unidades = conn.Query<UnidadDTO>(
                                                @"SELECT u.id AS Id, u.Unidad AS Nombre, '' AS Descripcion, 1 AS Activo
                                                    FROM US_Unidades u
                                                    WHERE u.id NOT IN (SELECT UnidadId FROM US_UsuariosUnidades WHERE UsuarioId = @UsuarioId)
                                                    ORDER BY u.Unidad",
                                                new { UsuarioId = usuarioId }
                                        ).ToList();
                    return unidades ?? new List<UnidadDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener unidades no asignadas para usuario {usuarioId}", ex);
            }
        }

        #endregion

        #region Permisos

        public class PermisoDTO
        {
            public int Id { get; set; }
            public string? Permiso { get; set; }
            public string? Descripcion { get; set; }
            public bool Activo { get; set; }
        }

        public List<PermisoDTO> ObtenerPermisosUsuario(int usuarioId, bool asignados)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    if (asignados)
                    {
                                                var list = conn.Query<PermisoDTO>(
                                                        @"SELECT p.id AS Id, p.Permiso AS Permiso, '' AS Descripcion, 1 AS Activo
                                                            FROM US_PermisosUsuarios pu
                                                            INNER JOIN US_Permisos p ON pu.PermisoId = p.id
                                                            WHERE pu.UsuarioId = @UsuarioId
                                                            ORDER BY p.Permiso",
                                                        new { UsuarioId = usuarioId }).ToList();
                        return list ?? new List<PermisoDTO>();
                    }

                                        var available = conn.Query<PermisoDTO>(
                                                @"SELECT p.id AS Id, p.Permiso AS Permiso, '' AS Descripcion, 1 AS Activo
                                                    FROM US_Permisos p
                                                    WHERE p.id NOT IN (SELECT PermisoId FROM US_PermisosUsuarios WHERE UsuarioId = @UsuarioId)
                                                    ORDER BY p.Permiso",
                                                new { UsuarioId = usuarioId }).ToList();
                    return available ?? new List<PermisoDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener permisos para usuario {usuarioId}", ex);
            }
        }

        public bool GuardarPermisoUsuario(int usuarioId, int permisoId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var exists = conn.QueryFirstOrDefault<int?>(
                        "SELECT 1 FROM US_PermisosUsuarios WHERE UsuarioId = @UsuarioId AND PermisoId = @PermisoId",
                        new { UsuarioId = usuarioId, PermisoId = permisoId });
                    if (exists.HasValue)
                        return false;

                    int result = conn.Execute(
                        "INSERT INTO US_PermisosUsuarios (UsuarioId, PermisoId) VALUES (@UsuarioId, @PermisoId)",
                        new { UsuarioId = usuarioId, PermisoId = permisoId });
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al asignar permiso al usuario {usuarioId}", ex);
            }
        }

        public bool EliminarPermisoUsuario(int usuarioId, int permisoId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    int result = conn.Execute(
                        "DELETE FROM US_PermisosUsuarios WHERE UsuarioId = @UsuarioId AND PermisoId = @PermisoId",
                        new { UsuarioId = usuarioId, PermisoId = permisoId });
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al eliminar permiso del usuario {usuarioId}", ex);
            }
        }

        #endregion

        #region Unidades

        /// <summary>
        /// Obtiene las unidades asignadas a un usuario
        /// </summary>
        public List<UnidadDTO> ObtenerUnidadesUsuario(int usuarioId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                                        var unidades = conn.Query<UnidadDTO>(
                                                @"SELECT DISTINCT u.id AS Id, u.Unidad AS Nombre, '' AS Descripcion, 1 AS Activo
                                                    FROM US_UsuariosUnidades uu
                                                    INNER JOIN US_Unidades u ON uu.UnidadId = u.id
                                                    WHERE uu.UsuarioId = @UsuarioId
                                                    ORDER BY u.Unidad",
                                                new { UsuarioId = usuarioId }
                                        ).ToList();
                    return unidades ?? new List<UnidadDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener unidades del usuario {usuarioId}", ex);
            }
        }

        /// <summary>
        /// Obtiene todas las unidades disponibles
        /// </summary>
        public List<UnidadDTO> ObtenerTodasUnidades()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var unidades = conn.Query<UnidadDTO>(
                        "SELECT id AS Id, Unidad AS Nombre, '' AS Descripcion, 1 AS Activo FROM US_Unidades ORDER BY Unidad"
                    ).ToList();
                    return unidades ?? new List<UnidadDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al obtener unidades", ex);
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            // No unmanaged resources to dispose
        }

        #endregion
    }

    /// <summary>
    /// DTO para Usuario
    /// </summary>
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string? Usuario { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool Activo { get; set; }

        public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();
    }

    /// <summary>
    /// DTO para Rol
    /// </summary>
    public class RolDTO
    {
        public int Id { get; set; }
        public string? Rol { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
    }

    /// <summary>
    /// DTO para Unidad/Grupo
    /// </summary>
    public class UnidadDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
