using System;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Data.Services.Usuarios;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.US.Controllers
{
    [Area("US")]
    [Route("Usuarios/Roles")]
    public class RolesController : Controller
    {
        private readonly RolService _rolService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(RolService rolService, ILogger<RolesController> logger)
        {
            _rolService = rolService ?? throw new ArgumentNullException(nameof(rolService));
            _logger = logger;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var (success, message, roles) = _rolService.ObtenerTodos();
            if (!success)
            {
                TempData["ErrorMessage"] = message;
                return View(new System.Collections.Generic.List<object>());
            }

            return View(roles);
        }

        [HttpGet("CreateModal")]
        public IActionResult CreateModal()
        {
            return PartialView("_CreateModal");
        }

        [HttpGet("EditModal/{id}")]
        public IActionResult EditModal(int id)
        {
            return PartialView("_EditModal", id);
        }

        [HttpGet("DeleteModal/{id}")]
        public IActionResult DeleteModal(int id)
        {
            return PartialView("_DeleteModal", id);
        }
    }
}
