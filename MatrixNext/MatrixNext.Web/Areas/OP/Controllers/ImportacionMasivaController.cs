using MatrixNext.Web.Services.OP;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.OP.Controllers;

[Area("OP")]
[Authorize]
public class ImportacionMasivaController : Controller
{
    private readonly IOpCargaService _cargaService;
    private readonly ILogger<ImportacionMasivaController> _logger;

    public ImportacionMasivaController(
        IOpCargaService cargaService,
        ILogger<ImportacionMasivaController> logger)
    {
        _cargaService = cargaService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new OpCargaWizardViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(OpCargaFormModel form)
    {
        var model = new OpCargaWizardViewModel { Form = form };
        if (form.Archivo is null || form.Archivo.Length == 0)
        {
            ModelState.AddModelError(nameof(form.Archivo), "Carga un archivo Excel antes de continuar.");
            return View(model);
        }

        var result = await _cargaService.ProcesarArchivoAsync(form.Archivo, form.Tipo);
        model.Result = result;

        if (!result.EsValido)
        {
            _logger.LogWarning("Validación de carga OP_Cuantitativo falló: {Mensaje}", result.Mensaje);
        }
        else
        {
            _logger.LogInformation(
                "Validación de carga OP_Cuantitativo exitosa ({Tipo}, {Nombre})",
                form.Tipo,
                form.Archivo.FileName);
        }

        return View(model);
    }
}
