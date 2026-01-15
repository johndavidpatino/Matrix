using MatrixNext.Data.Services.IT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.IT.Controllers;

[Area("IT")]
[Authorize] // TODO: Agregar política específica para permisos 133/134
public class ITController : Controller
{
    private readonly ILogger<ITController> _logger;

    public ITController(ILogger<ITController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Dashboard principal del módulo IT con navegación
    /// Migrado desde: WebMatrix/IT/Default.aspx
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }
}
