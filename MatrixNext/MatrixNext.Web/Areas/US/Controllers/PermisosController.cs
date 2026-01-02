using System;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Data.Services.Usuarios;

namespace MatrixNext.Web.Areas.US.Controllers
{
    [Area("US")]
    [Route("Usuarios/Permisos")]
    public class PermisosController : Controller
    {
        private readonly PermisosService _permisosService;

        public PermisosController(PermisosService permisosService)
        {
            _permisosService = permisosService ?? throw new ArgumentNullException(nameof(permisosService));
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var (success, message, permisos) = _permisosService.ObtenerTodos();
            ViewData["ErrorMessage"] = success ? null : message;
            return View(permisos);
        }
    }
}
