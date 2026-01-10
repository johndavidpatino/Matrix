using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.GD.Controllers
{
    [Area("GD")]
    [Authorize]
    public class DocumentosMaestroController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Title"] = "Maestro de Documentos";
            return View();
        }
    }
}
