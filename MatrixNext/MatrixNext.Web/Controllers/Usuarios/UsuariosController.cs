using System;
using System.Collections.Generic;
using System.Linq;
using MatrixNext.Data.Models.Usuarios;
using MatrixNext.Data.Services.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Controllers.Usuarios
{
    /// <summary>
    /// Controlador para gestión de Usuarios
    /// Proporciona operaciones CRUD para usuarios del sistema
    /// </summary>
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
    }
}
