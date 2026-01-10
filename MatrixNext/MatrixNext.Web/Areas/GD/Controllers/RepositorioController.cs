using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.GD.Controllers
{
    [Area("GD")]
    [Authorize]
    public class RepositorioController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Title"] = "Repositorio de Documentos";
            return View();
        }
    }
}
