using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.MBO.Controllers;

/// <summary>
/// Controller principal para el área MBO
/// Dashboard de entrada al módulo
/// </summary>
[Area("MBO")]
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Dashboard principal de MBO
    /// Muestra cards de acceso a subsistemas (AOT, Campo, Propuestas)
    /// Requiere permiso 23 (acceso MBO)
    /// GET: /MBO/Home/Index
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        _logger.LogInformation("Usuario accediendo a dashboard principal MBO");
        return View();
    }
}
