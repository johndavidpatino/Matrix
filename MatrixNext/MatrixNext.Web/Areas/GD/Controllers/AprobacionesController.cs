using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.GD.Controllers
{
    [Area("GD")]
    [Authorize]
    public class AprobacionesController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Title"] = "Aprobaciones de Documentos";
            return View();
        }
    }
}
