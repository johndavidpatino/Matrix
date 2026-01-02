using System;
using System.Collections.Generic;
using System.Linq;
using MatrixNext.Data.Adapters;
using MatrixNext.Data.Models.Usuarios;
using MatrixNext.Data.Utilities;
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
        public (bool success, string message, List<UsuarioListViewModel> data) ObtenerListaUsuarios(string filtro = null)
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
                            Activo = u.Activo
                        })
                        .ToList();

                    if (!string.IsNullOrWhiteSpace(filtro))
                    {
                        var f = filtro.Trim().ToLowerInvariant();
                        viewModels = viewModels.Where(u =>
                            (u.NombreUsuario ?? "").ToLowerInvariant().Contains(f) ||
                            (u.NombreCompleto ?? "").ToLowerInvariant().Contains(f) ||
                            (u.Email ?? "").ToLowerInvariant().Contains(f)
                        ).OrderBy(u => u.NombreUsuario).ToList();
                    }
                    else
                    {
                        viewModels = viewModels.OrderBy(u => u.NombreUsuario).ToList();
                    }

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
                        Roles = new List<RolViewModel>(),
                        Unidades = new List<UnidadViewModel>()
                    };

                    // Cargar roles y unidades asignadas
                    var roles = adapter.ObtenerRolesUsuario(usuario.Id);
                    viewModel.Roles = roles.Select(r => new RolViewModel { Id = r.Id, Nombre = r.Rol, Descripcion = r.Descripcion }).ToList();

                    var unidades = adapter.ObtenerUnidadesUsuario(usuario.Id);
                    viewModel.Unidades = unidades.Select(u => new UnidadViewModel { Id = u.Id, Nombre = u.Nombre, Descripcion = u.Descripcion }).ToList();

                    // Cargar permisos asignados
                    try
                    {
                        var permisos = adapter.ObtenerPermisosUsuario(usuario.Id, true);
                        viewModel.Permisos = permisos.Select(p => new PermisoViewModel { Id = p.Id, Permiso = p.Permiso, Descripcion = p.Descripcion }).ToList();
                    }
                    catch
                    {
                        viewModel.Permisos = new List<PermisoViewModel>();
                    }

                    return (true, "Detalle obtenido exitosamente", viewModel);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener detalle: {ex.Message}", null);
            }
        }

        #region Roles/Unidades/Permisos management

        // Roles
        public (bool success, string message, List<RolViewModel> data) ObtenerRolesAsignados(int usuarioId)
        {
            try
            {
                using var adapter = new UsuarioDataAdapter(_connectionString);
                var list = adapter.ObtenerRolesUsuario(usuarioId);
                var vm = list.Select(r => new RolViewModel { Id = r.Id, Nombre = r.Rol, Descripcion = r.Descripcion }).ToList();
                return (true, "Roles obtenidos", vm);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, new List<RolViewModel>());
            }
        }

        public (bool success, string message, List<RolViewModel> data) ObtenerRolesDisponibles(int usuarioId)
        {
            try
            {
                using var adapter = new UsuarioDataAdapter(_connectionString);
                var list = adapter.ObtenerRolesNoAsignados(usuarioId);
                var vm = list.Select(r => new RolViewModel { Id = r.Id, Nombre = r.Rol, Descripcion = r.Descripcion }).ToList();
                return (true, "Roles disponibles obtenidos", vm);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, new List<RolViewModel>());
            }
        }

        public (bool success, string message) AsignarRol(int usuarioId, int rolId)
        {
            try
            {
                using var adapter = new UsuarioDataAdapter(_connectionString);
                var ok = adapter.GuardarRolUsuario(usuarioId, rolId);
                return ok ? (true, "Rol asignado") : (false, "El rol ya estaba asignado o no se pudo asignar");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool success, string message) EliminarRol(int usuarioId, int rolId)
        {
            try
            {
                using var adapter = new UsuarioDataAdapter(_connectionString);
                var ok = adapter.EliminarRolUsuario(usuarioId, rolId);
                return ok ? (true, "Rol removido") : (false, "No se pudo remover el rol");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        // Unidades
        public (bool success, string message, List<UnidadViewModel> data) ObtenerUnidadesAsignadas(int usuarioId)
        {
            try
            {
                using var adapter = new UsuarioDataAdapter(_connectionString);
                var list = adapter.ObtenerUnidadesUsuario(usuarioId);
                var vm = list.Select(u => new UnidadViewModel { Id = u.Id, Nombre = u.Nombre, Descripcion = u.Descripcion }).ToList();
                return (true, "Unidades obtenidas", vm);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, new List<UnidadViewModel>());
            }
        }

        public (bool success, string message, List<UnidadViewModel> data) ObtenerUnidadesDisponibles(int usuarioId)
        {
            try
            {
                using var adapter = new UsuarioDataAdapter(_connectionString);
                var list = adapter.ObtenerUnidadesNoAsignadas(usuarioId);
                var vm = list.Select(u => new UnidadViewModel { Id = u.Id, Nombre = u.Nombre, Descripcion = u.Descripcion }).ToList();
                return (true, "Unidades disponibles obtenidas", vm);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, new List<UnidadViewModel>());
            }
        }

        public (bool success, string message) AsignarUnidad(int usuarioId, int grupoUnidadId)
        {
            try
            {
                using var adapter = new UsuarioDataAdapter(_connectionString);
                var ok = adapter.GuardarUsuariosUnidades(usuarioId, grupoUnidadId);
                return ok ? (true, "Unidad asignada") : (false, "La unidad ya estaba asignada o no se pudo asignar");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool success, string message) EliminarUnidad(int usuarioId, int grupoUnidadId)
        {
            try
            {
                using var adapter = new UsuarioDataAdapter(_connectionString);
                var ok = adapter.EliminarUsuariosUnidades(usuarioId, grupoUnidadId);
                return ok ? (true, "Unidad removida") : (false, "No se pudo remover la unidad");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        // Permisos
        public (bool success, string message, List<object> data) ObtenerPermisos(int usuarioId, bool asignados)
        {
            try
            {
                using var adapter = new UsuarioDataAdapter(_connectionString);
                var list = adapter.ObtenerPermisosUsuario(usuarioId, asignados);
                var vm = list.Select(p => new { p.Id, p.Permiso, p.Descripcion }).Cast<object>().ToList();
                return (true, "Permisos obtenidos", vm);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, new List<object>());
            }
        }

        public (bool success, string message) AsignarPermiso(int usuarioId, int permisoId)
        {
            try
            {
                using var adapter = new UsuarioDataAdapter(_connectionString);
                var ok = adapter.GuardarPermisoUsuario(usuarioId, permisoId);
                return ok ? (true, "Permiso asignado") : (false, "El permiso ya estaba asignado o no se pudo asignar");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool success, string message) EliminarPermiso(int usuarioId, int permisoId)
        {
            try
            {
                using var adapter = new UsuarioDataAdapter(_connectionString);
                var ok = adapter.EliminarPermisoUsuario(usuarioId, permisoId);
                return ok ? (true, "Permiso removido") : (false, "No se pudo remover el permiso");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// Obtiene el Id de usuario por nombre de usuario (login)
        /// </summary>
        public (bool success, string message, int usuarioId) ObtenerIdPorNombre(string nombreUsuario)
        {
            try
            {
                using var adapter = new UsuarioDataAdapter(_connectionString);
                var usuario = adapter.ObtenerUsuarioPorUsuario(nombreUsuario);
                if (usuario == null)
                    return (false, "Usuario no encontrado", 0);
                return (true, "OK", (int)usuario.Id);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, 0);
            }
        }

        #endregion

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
                        Activo = viewModel.Activo
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

            return EncryptionService.EncryptPassword(password);
        }

        /// <summary>
        /// Cambia la contraseña del usuario verificando la contraseña actual
        /// </summary>
        public (bool success, string message) CambiarContrasena(int usuarioId, string contrasenaActual, string contrasenaNueva)
        {
            try
            {
                if (string.IsNullOrEmpty(contrasenaNueva))
                    return (false, "La nueva contraseña no puede estar vacía");

                using (var adapter = new UsuarioDataAdapter(_connectionString))
                {
                    var usuario = adapter.ObtenerUsuarioPorId(usuarioId);
                    if (usuario == null)
                        return (false, "Usuario no encontrado");

                    var actualEsValida = EncryptionService.VerifyPassword(contrasenaActual ?? "", usuario.Password);
                    if (!actualEsValida)
                        return (false, "La contraseña actual es incorrecta");

                    var nuevaEncriptada = EncriptarPassword(contrasenaNueva);
                    var ok = adapter.ActualizarPassword(usuarioId, nuevaEncriptada);
                    if (!ok)
                        return (false, "Error al actualizar la contraseña");

                    return (true, "Contraseña actualizada exitosamente");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error al cambiar contraseña: {ex.Message}");
            }
        }

        #endregion
    }
}
