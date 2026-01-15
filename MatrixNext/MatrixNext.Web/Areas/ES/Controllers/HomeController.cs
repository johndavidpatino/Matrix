using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.ES.Controllers
{
    [Area("ES")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
