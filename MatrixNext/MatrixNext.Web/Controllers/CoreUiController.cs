using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Controllers;

[Authorize]
public class CoreUiController : Controller
{
    [HttpGet("/Core")]
    public IActionResult Index()
    {
        return View();
    }

    private long ObtenerIdUsuarioActual()
    {
        var claim = User?.FindFirst("sub") ?? User?.FindFirst("nameidentifier");
        if (long.TryParse(claim?.Value, out var id))
            return id;
        return 1;
    }
}
