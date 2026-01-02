using System;
using System.Collections.Generic;
using System.Linq;
using MatrixNext.Data.Adapters;
using MatrixNext.Data.Models.Usuarios;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Services.Usuarios
{
    /// <summary>
    /// Servicio para gestión de usuarios
    /// Accede a los datos de usuarios a través del adaptador Dapper
    /// </summary>
    public class UsuarioService
    {
        private readonly string _connectionString;

        public UsuarioService(IConfiguration configuration)
        {
            _connectionString = configuration?.GetConnectionString("MatrixDatabase") 
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'MatrixDatabase' not found");
        }

        #region CRUD Operations

        /// <summary>
        /// Obtiene lista de usuarios activos
        /// </summary>
        public (bool success, string message, List<UsuarioListViewModel> data) ObtenerListaUsuarios()
        {
            try
            {
                using (var adapter = new UsuarioDataAdapter(_connectionString))
                {
                    var usuarios = adapter.ObtenerUsuariosPorActivo(true);
                    var viewModels = usuarios
                        .Select(u => new UsuarioListViewModel
                        {
                            Id = u.Id,
                            NombreUsuario = u.Usuario,
                            NombreCompleto = u.NombreCompleto,
                            Email = u.Email,
                            Activo = u.Activo,
                            FechaCreacion = u.FechaCreacion
                        })
                        .OrderBy(u => u.NombreUsuario)
                        .ToList();

                    return (true, "Listado obtenido exitosamente", viewModels);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener listado: {ex.Message}", new List<UsuarioListViewModel>());
            }
        }

        /// <summary>
        /// Obtiene detalle de un usuario por su Id
        /// </summary>
        public (bool success, string message, UsuarioDetailViewModel data) ObtenerDetalle(int usuarioId)
        {
            try
            {
                using (var adapter = new UsuarioDataAdapter(_connectionString))
                {
                    var usuario = adapter.ObtenerUsuarioPorId(usuarioId);
                    
                    if (usuario == null)
                        return (false, "Usuario no encontrado", null);

                    var viewModel = new UsuarioDetailViewModel
                    {
                        Id = usuario.Id,
                        NombreUsuario = usuario.Usuario,
                        Nombres = usuario.Nombres,
                        Apellidos = usuario.Apellidos,
                        Email = usuario.Email,
                        Activo = usuario.Activo,
                        FechaCreacion = usuario.FechaCreacion,
                        Roles = new List<RolViewModel>(),
                        Unidades = new List<UnidadViewModel>()
                    };

                    return (true, "Detalle obtenido exitosamente", viewModel);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener detalle: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        public (bool success, string message, int usuarioId) CrearUsuario(UsuarioFormViewModel viewModel)
        {
            try
            {
                if (viewModel == null)
                    return (false, "Los datos del usuario son requeridos", 0);

                if (string.IsNullOrWhiteSpace(viewModel.NombreUsuario))
                    return (false, "El nombre de usuario es requerido", 0);

                using (var adapter = new UsuarioDataAdapter(_connectionString))
                {
                    var usuarioExistente = adapter.ObtenerUsuarioPorUsuario(viewModel.NombreUsuario);
                    if (usuarioExistente != null)
                        return (false, $"El usuario '{viewModel.NombreUsuario}' ya existe", 0);

                    var usuarioDTO = new UsuarioDTO
                    {
                        Usuario = viewModel.NombreUsuario,
                        Nombres = viewModel.Nombres ?? "",
                        Apellidos = viewModel.Apellidos ?? "",
                        Email = viewModel.Email ?? "",
                        Password = EncriptarPassword(viewModel.Password ?? ""),
                        Activo = viewModel.Activo,
                        FechaCreacion = DateTime.Now
                    };

                    int newId = adapter.CrearUsuario(usuarioDTO);
                    return (true, "Usuario creado exitosamente", newId);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear usuario: {ex.Message}", 0);
            }
        }

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        public (bool success, string message) ActualizarUsuario(int usuarioId, UsuarioFormViewModel viewModel)
        {
            try
            {
                if (viewModel == null)
                    return (false, "Los datos del usuario son requeridos");

                using (var adapter = new UsuarioDataAdapter(_connectionString))
                {
                    var usuario = adapter.ObtenerUsuarioPorId(usuarioId);
                    if (usuario == null)
                        return (false, "Usuario no encontrado");

                    usuario.Nombres = viewModel.Nombres ?? "";
                    usuario.Apellidos = viewModel.Apellidos ?? "";
                    usuario.Email = viewModel.Email ?? "";
                    usuario.Activo = viewModel.Activo;

                    if (!string.IsNullOrEmpty(viewModel.Password))
                    {
                        var passwordEncriptada = EncriptarPassword(viewModel.Password);
                        var resultPassword = adapter.ActualizarPassword(usuarioId, passwordEncriptada);
                        if (!resultPassword)
                            return (false, "Error al actualizar la contraseña");
                    }

                    bool resultado = adapter.ActualizarUsuario(usuario);
                    if (!resultado)
                        return (false, "Error al actualizar usuario");

                    return (true, "Usuario actualizado exitosamente");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar usuario: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina un usuario (soft delete)
        /// </summary>
        public (bool success, string message) EliminarUsuario(int usuarioId)
        {
            try
            {
                using (var adapter = new UsuarioDataAdapter(_connectionString))
                {
                    var usuario = adapter.ObtenerUsuarioPorId(usuarioId);
                    if (usuario == null)
                        return (false, "Usuario no encontrado");

                    bool resultado = adapter.EliminarUsuario(usuarioId);
                    if (!resultado)
                        return (false, "Error al eliminar usuario");

                    return (true, "Usuario eliminado exitosamente");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar usuario: {ex.Message}");
            }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Obtiene usuario para formulario de edición
        /// </summary>
        public (bool success, UsuarioFormViewModel data) ObtenerParaEdicion(int usuarioId)
        {
            try
            {
                using (var adapter = new UsuarioDataAdapter(_connectionString))
                {
                    var usuario = adapter.ObtenerUsuarioPorId(usuarioId);
                    if (usuario == null)
                        return (false, null);

                    var viewModel = new UsuarioFormViewModel
                    {
                        Id = usuario.Id,
                        NombreUsuario = usuario.Usuario,
                        Nombres = usuario.Nombres,
                        Apellidos = usuario.Apellidos,
                        Email = usuario.Email,
                        Activo = usuario.Activo
                    };

                    return (true, viewModel);
                }
            }
            catch
            {
                return (false, null);
            }
        }

        /// <summary>
        /// Método para encriptar contraseñas
        /// Implementar con TripleDES para compatibilidad con legacy
        /// </summary>
        private string EncriptarPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return "";
            
            try
            {
                // TODO: Implementar TripleDES encryption compatible con legacy
                // Por ahora retorna la contraseña como placeholder
                return password; // Placeholder
            }
            catch
            {
                return password;
            }
        }

        #endregion
    }
}
