using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Areas.EQ.Services;

namespace MatrixNext.Web.Areas.EQ.Controllers
{
    [Area("EQ")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Route("EQ/[controller]/[action]")]
    public class EasyQuoteAdminController : Controller
    {
        private readonly EasyQuoteAdminService _service;

        public EasyQuoteAdminController(EasyQuoteAdminService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public IActionResult Parametros() => View(_service.CargarParametros());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertPrecio([FromBody] MatrixNext.Web.Areas.EQ.Services.Masters.EasyQuoteMasterService.PrecioRow payload)
            => Json(_service.UpsertPrecio(payload));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertValorHora([FromBody] MatrixNext.Web.Areas.EQ.Services.Masters.EasyQuoteMasterService.ValorHoraRow payload)
            => Json(_service.UpsertValorHora(payload));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertInsumo([FromBody] MatrixNext.Web.Areas.EQ.Services.Masters.EasyQuoteMasterService.CostInsumoRow payload)
            => Json(_service.UpsertInsumo(payload));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertEnvio([FromBody] MatrixNext.Web.Areas.EQ.Services.Masters.EasyQuoteMasterService.EnvioTarifaRow payload)
            => Json(_service.UpsertEnvio(payload));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertLocacion([FromBody] MatrixNext.Web.Areas.EQ.Services.Masters.EasyQuoteMasterService.LocacionRow payload)
            => Json(_service.UpsertLocacion(payload));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertMystery([FromBody] MatrixNext.Web.Areas.EQ.Services.Masters.EasyQuoteMasterService.MysteryTarifaRow payload)
            => Json(_service.UpsertMystery(payload));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertCodificacion([FromBody] MatrixNext.Web.Areas.EQ.Services.Masters.EasyQuoteMasterService.CodificacionRow payload)
            => Json(_service.UpsertCodificacion(payload));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertCostUnitario([FromBody] MatrixNext.Web.Areas.EQ.Services.Masters.EasyQuoteMasterService.CostUnitarioOpsRow payload)
            => Json(_service.UpsertCostUnitario(payload));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertMisc([FromBody] MatrixNext.Web.Areas.EQ.Services.Masters.EasyQuoteMasterService.ParamMiscRow payload)
            => Json(_service.UpsertMisc(payload));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertEnvioParam([FromBody] MatrixNext.Web.Areas.EQ.Services.Masters.EasyQuoteMasterService.EnvioParamRow payload)
            => Json(_service.UpsertEnvioParam(payload));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertProductividad([FromBody] MatrixNext.Web.Areas.EQ.Services.Masters.EasyQuoteMasterService.ProductividadCiudadRow payload)
            => Json(_service.UpsertProductividad(payload));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertBaseDatos([FromBody] MatrixNext.Web.Areas.EQ.Services.Masters.EasyQuoteMasterService.BaseDatosRow payload)
            => Json(_service.UpsertBaseDatos(payload));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportPreciosCsv(IFormFile file, string version)
            => Json(_service.ImportPreciosCsv(file, version));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportValorHoraCsv(IFormFile file, string version)
            => Json(_service.ImportValorHoraCsv(file, version));
    }
}
