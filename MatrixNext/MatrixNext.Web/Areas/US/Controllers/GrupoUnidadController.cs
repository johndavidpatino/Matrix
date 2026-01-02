using System;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Data.Services.Usuarios;

namespace MatrixNext.Web.Areas.US.Controllers
{
    [Area("US")]
    [Route("Usuarios/GrupoUnidad")]
    public class GrupoUnidadController : Controller
    {
        private readonly GrupoUnidadService _service;

        public GrupoUnidadController(GrupoUnidadService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var (success, message, unidades) = _service.ObtenerTodos();
            if (!success)
            {
                TempData["ErrorMessage"] = message;
            }

            return View(unidades);
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
