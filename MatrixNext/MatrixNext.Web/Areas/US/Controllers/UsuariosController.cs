using System;
using System.Collections.Generic;
using System.Linq;
using MatrixNext.Data.Modules.US.Usuarios.Models;
using MatrixNext.Data.Modules.US.Usuarios.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.US.Controllers
{
    /// <summary>
    /// Controlador para gestión de Usuarios
    /// Proporciona operaciones CRUD para usuarios del sistema
    /// </summary>
    [Area("US")]
    [Route("US/Usuarios")]
    [Route("Usuarios")]
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _usuarioService;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(UsuarioService usuarioService, ILogger<UsuariosController> logger)
        {
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Request DTOs for JSON binding from the client
        public class AssignRoleRequest { public int usuarioId { get; set; } public int rolId { get; set; } }
        public class AssignUnidadRequest { public int usuarioId { get; set; } public int grupoUnidadId { get; set; } }
        public class AssignPermisoRequest { public int usuarioId { get; set; } public int permisoId { get; set; } }

        #region Assignments AJAX endpoints

        [HttpPost("AssignRole")]
        public IActionResult AssignRole([FromBody] AssignRoleRequest req)
        {
            try
            {
                var (success, message) = _usuarioService.AsignarRol(req.usuarioId, req.rolId);
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("RemoveRole")]
        public IActionResult RemoveRole([FromBody] AssignRoleRequest req)
        {
            try
            {
                var (success, message) = _usuarioService.EliminarRol(req.usuarioId, req.rolId);
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing role");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("AssignUnidad")]
        public IActionResult AssignUnidad([FromBody] AssignUnidadRequest req)
        {
            try
            {
                var (success, message) = _usuarioService.AsignarUnidad(req.usuarioId, req.grupoUnidadId);
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning unidad");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("RemoveUnidad")]
        public IActionResult RemoveUnidad([FromBody] AssignUnidadRequest req)
        {
            try
            {
                var (success, message) = _usuarioService.EliminarUnidad(req.usuarioId, req.grupoUnidadId);
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing unidad");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("AssignPermiso")]
        public IActionResult AssignPermiso([FromBody] AssignPermisoRequest req)
        {
            try
            {
                var (success, message) = _usuarioService.AsignarPermiso(req.usuarioId, req.permisoId);
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning permiso");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("RemovePermiso")]
        public IActionResult RemovePermiso([FromBody] AssignPermisoRequest req)
        {
            try
            {
                var (success, message) = _usuarioService.EliminarPermiso(req.usuarioId, req.permisoId);
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing permiso");
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Fetch lists (AJAX GET)

        [HttpGet("GetRolesAsignados/{usuarioId}")]
        public IActionResult GetRolesAsignados(int usuarioId)
        {
            try
            {
                var (success, message, data) = _usuarioService.ObtenerRolesAsignados(usuarioId);
                return Json(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching roles asignados");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        [HttpGet("GetRolesDisponibles/{usuarioId}")]
        public IActionResult GetRolesDisponibles(int usuarioId)
        {
            try
            {
                var (success, message, data) = _usuarioService.ObtenerRolesDisponibles(usuarioId);
                return Json(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching roles disponibles");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        [HttpGet("GetUnidadesAsignadas/{usuarioId}")]
        public IActionResult GetUnidadesAsignadas(int usuarioId)
        {
            try
            {
                var (success, message, data) = _usuarioService.ObtenerUnidadesAsignadas(usuarioId);
                return Json(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching unidades asignadas");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        [HttpGet("GetUnidadesDisponibles/{usuarioId}")]
        public IActionResult GetUnidadesDisponibles(int usuarioId)
        {
            try
            {
                var (success, message, data) = _usuarioService.ObtenerUnidadesDisponibles(usuarioId);
                return Json(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching unidades disponibles");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        [HttpGet("GetPermisosAsignados/{usuarioId}")]
        public IActionResult GetPermisosAsignados(int usuarioId)
        {
            try
            {
                var (success, message, data) = _usuarioService.ObtenerPermisos(usuarioId, true);
                return Json(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching permisos asignados");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        [HttpGet("GetPermisosDisponibles/{usuarioId}")]
        public IActionResult GetPermisosDisponibles(int usuarioId)
        {
            try
            {
                var (success, message, data) = _usuarioService.ObtenerPermisos(usuarioId, false);
                return Json(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching permisos disponibles");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        #endregion

        #region Modal endpoints

        [HttpGet("CreateModal")]
        public IActionResult CreateModal()
        {
            var vm = new UsuarioFormViewModel { Activo = true };
            return PartialView("_CreateModal", vm);
        }

        [HttpGet("EditModal/{id}")]
        public IActionResult EditModal(int id)
        {
            var (success, vm) = _usuarioService.ObtenerParaEdicion(id);
            if (!success || vm == null)
                return NotFound();
            return PartialView("_EditModal", vm);
        }

        [HttpGet("DeleteModal/{id}")]
        public IActionResult DeleteModal(int id)
        {
            var (success, message, detail) = _usuarioService.ObtenerDetalle(id);
            if (!success || detail == null)
                return NotFound();
            return PartialView("_DeleteModal", detail);
        }

        #endregion

        /// <summary>
        /// Listado de usuarios
        /// GET: /Usuarios
        /// </summary>
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            try
            {
                var (success, message, usuarios) = _usuarioService.ObtenerListaUsuarios();
                
                if (!success)
                {
                    TempData["ErrorMessage"] = message;
                    return View(new List<UsuarioListViewModel>());
                }

                _logger.LogInformation($"Listado de usuarios obtenido. Total: {usuarios.Count}");
                return View(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener listado de usuarios");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(new List<UsuarioListViewModel>());
            }
        }

        [HttpGet("Search")]
        public IActionResult Search(string q)
        {
            try
            {
                var (success, message, usuarios) = _usuarioService.ObtenerListaUsuarios(q);
                return Json(new { success, message, data = usuarios });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching users");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        /// <summary>
        /// Formulario para crear nuevo usuario
        /// GET: /Usuarios/Create
        /// </summary>
        [HttpGet("Create")]
        public IActionResult Create()
        {
            try
            {
                var viewModel = new UsuarioFormViewModel { Activo = true };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar formulario de creación");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Procesa la creación de nuevo usuario
        /// POST: /Usuarios/Create
        /// </summary>
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UsuarioFormViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                var (success, message, usuarioId) = _usuarioService.CrearUsuario(viewModel);

                if (!success)
                {
                    TempData["ErrorMessage"] = message;
                    return View(viewModel);
                }

                _logger.LogInformation($"Usuario '{viewModel.NombreUsuario}' creado exitosamente");
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Details", new { id = usuarioId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al crear usuario: {viewModel?.NombreUsuario}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(viewModel);
            }
        }

        /// <summary>
        /// Formulario para editar usuario
        /// GET: /Usuarios/Edit/5
        /// </summary>
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            try
            {
                var (success, viewModel) = _usuarioService.ObtenerParaEdicion(id);

                if (!success || viewModel == null)
                {
                    TempData["ErrorMessage"] = "Usuario no encontrado";
                    return RedirectToAction("Index");
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cargar usuario para edición: {id}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Procesa la actualización de usuario
        /// POST: /Usuarios/Edit/5
        /// </summary>
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, UsuarioFormViewModel viewModel)
        {
            try
            {
                // Si no hay cambio de contraseña, remover las validaciones de esos campos
                if (string.IsNullOrEmpty(viewModel.Password) && string.IsNullOrEmpty(viewModel.ConfirmPassword))
                {
                    ModelState.Remove("Password");
                    ModelState.Remove("ConfirmPassword");
                }
                else if (!string.IsNullOrEmpty(viewModel.Password) || !string.IsNullOrEmpty(viewModel.ConfirmPassword))
                {
                    // Si hay cambio de contraseña, deben coincidir
                    if (viewModel.Password != viewModel.ConfirmPassword)
                    {
                        ModelState.AddModelError("ConfirmPassword", "Las contraseñas no coinciden");
                    }
                }

                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                var (success, message) = _usuarioService.ActualizarUsuario(id, viewModel);

                if (!success)
                {
                    TempData["ErrorMessage"] = message;
                    return View(viewModel);
                }

                _logger.LogInformation($"Usuario {id} actualizado exitosamente");
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar usuario: {id}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(viewModel);
            }
        }

        /// <summary>
        /// Detalles del usuario
        /// GET: /Usuarios/Details/5
        /// </summary>
        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            try
            {
                var (success, message, viewModel) = _usuarioService.ObtenerDetalle(id);

                if (!success || viewModel == null)
                {
                    TempData["ErrorMessage"] = message ?? "Usuario no encontrado";
                    return RedirectToAction("Index");
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener detalles del usuario: {id}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Confirmación de eliminación
        /// GET: /Usuarios/Delete/5
        /// </summary>
        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var (success, message, viewModel) = _usuarioService.ObtenerDetalle(id);

                if (!success || viewModel == null)
                {
                    TempData["ErrorMessage"] = "Usuario no encontrado";
                    return RedirectToAction("Index");
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cargar confirmación de eliminación: {id}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Procesa la eliminación del usuario (soft delete)
        /// POST: /Usuarios/Delete/5
        /// </summary>
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var (success, message) = _usuarioService.EliminarUsuario(id);

                if (!success)
                {
                    TempData["ErrorMessage"] = message;
                    return RedirectToAction("Details", new { id });
                }

                _logger.LogInformation($"Usuario {id} eliminado exitosamente");
                TempData["SuccessMessage"] = message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar usuario: {id}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("Details", new { id });
            }
        }

        #region Change Password

        [HttpGet("MyChangePassword")]
        public IActionResult MyChangePassword()
        {
            try
            {
                var username = User?.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                {
                    TempData["ErrorMessage"] = "Usuario no autenticado";
                    return RedirectToAction("Index");
                }

                var (success, message, usuarioId) = _usuarioService.ObtenerIdPorNombre(username);
                if (!success || usuarioId <= 0)
                {
                    TempData["ErrorMessage"] = "No se pudo localizar el usuario";
                    return RedirectToAction("Index");
                }

                return RedirectToAction("ChangePassword", new { id = usuarioId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error redirecting to MyChangePassword");
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet("ChangePassword/{id}")]
        public IActionResult ChangePassword(int id)
        {
            try
            {
                var (success, vm) = _usuarioService.ObtenerParaEdicion(id);
                if (!success || vm == null)
                {
                    TempData["ErrorMessage"] = "Usuario no encontrado";
                    return RedirectToAction("Index");
                }

                var model = new UsuarioChangePasswordViewModel
                {
                    Id = (int)(vm.Id ?? 0),
                    NombreUsuario = vm.NombreUsuario
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cargar ChangePassword: {id}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("ChangePassword/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(int id, UsuarioChangePasswordViewModel model)
        {
            try
            {
                // Ensure NombreUsuario doesn't make ModelState invalid (it's not posted by the form)
                var (vmSuccess, vm) = _usuarioService.ObtenerParaEdicion(id);
                if (vmSuccess && vm != null)
                {
                    model.NombreUsuario = vm.NombreUsuario;
                }
                // Remove NombreUsuario from ModelState so missing/empty value won't fail validation
                ModelState.Remove("NombreUsuario");

                if (string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.ConfirmNewPassword))
                {
                    ModelState.AddModelError("NewPassword", "La nueva contraseña es requerida");
                }

                if (model.NewPassword != model.ConfirmNewPassword)
                {
                    ModelState.AddModelError("ConfirmNewPassword", "Las contraseñas no coinciden");
                }

                if (!ModelState.IsValid)
                    return View(model);

                var (success, message) = _usuarioService.CambiarContrasena(id, model.CurrentPassword, model.NewPassword);
                if (!success)
                {
                    TempData["ErrorMessage"] = message;
                    return View(model);
                }

                // Signal the view to show a toast and perform a delayed redirect
                TempData["PasswordChangedMessage"] = message;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cambiar contraseña para usuario {id}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(model);
            }
        }

        #endregion
    }
}
