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
    }
}
