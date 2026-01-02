using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace MatrixNext.Data.Adapters
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
                        "SELECT Id, Usuario, Nombres, Apellidos, Email, Password, Activo, FechaCreacion FROM US_Usuarios ORDER BY Usuario"
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
        public UsuarioDTO ObtenerUsuarioPorId(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var usuario = conn.QueryFirstOrDefault<UsuarioDTO>(
                        "SELECT Id, Usuario, Nombres, Apellidos, Email, Password, Activo, FechaCreacion FROM US_Usuarios WHERE Id = @Id",
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
                        "SELECT Id, Usuario, Nombres, Apellidos, Email, Password, Activo, FechaCreacion FROM US_Usuarios WHERE Activo = @Activo ORDER BY Usuario",
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
        public UsuarioDTO ObtenerUsuarioPorUsuario(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return null;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var result = conn.QueryFirstOrDefault<UsuarioDTO>(
                        "SELECT Id, Usuario, Nombres, Apellidos, Email, Password, Activo, FechaCreacion FROM US_Usuarios WHERE Usuario = @Usuario",
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
                    var sql = @"INSERT INTO US_Usuarios (Usuario, Nombres, Apellidos, Email, Password, Activo, FechaCreacion) 
                               OUTPUT INSERTED.Id
                               VALUES (@Usuario, @Nombres, @Apellidos, @Email, @Password, @Activo, @FechaCreacion)";
                    
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
                        @"SELECT DISTINCT r.Id, r.Rol, r.Descripcion, r.Activo, r.FechaCreacion 
                          FROM US_RolesUsuarios ru
                          INNER JOIN US_Roles r ON ru.IdRol = r.Id
                          WHERE ru.IdUsuario = @UsuarioId AND r.Activo = 1
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
                        "SELECT Id, Rol, Descripcion, Activo, FechaCreacion FROM US_Roles WHERE Activo = 1 ORDER BY Rol"
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
                        @"SELECT DISTINCT gu.Id, gu.Nombre, gu.Descripcion, gu.Activo, gu.FechaCreacion 
                          FROM US_UsuariosUnidades uu
                          INNER JOIN GR_GruposUnidad gu ON uu.IdGrupoUnidad = gu.Id
                          WHERE uu.IdUsuario = @UsuarioId AND gu.Activo = 1
                          ORDER BY gu.Nombre",
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
                        "SELECT Id, Nombre, Descripcion, Activo, FechaCreacion FROM GR_GruposUnidad WHERE Activo = 1 ORDER BY Nombre"
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
        public string Usuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();
    }

    /// <summary>
    /// DTO para Rol
    /// </summary>
    public class RolDTO
    {
        public int Id { get; set; }
        public string Rol { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    /// <summary>
    /// DTO para Unidad/Grupo
    /// </summary>
    public class UnidadDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
