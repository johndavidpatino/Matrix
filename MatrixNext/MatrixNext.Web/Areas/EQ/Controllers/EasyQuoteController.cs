using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Areas.EQ.Models;
using MatrixNext.Web.Areas.EQ.Services;

namespace MatrixNext.Web.Areas.EQ.Controllers
{
    [Area("EQ")]
    [Authorize]
    [Route("EQ/[controller]/[action]")]
    public class EasyQuoteController : Controller
    {
        private readonly EasyQuoteService _service;

        public EasyQuoteController(EasyQuoteService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public IActionResult Index(long? id)
        {
            // carga inicial (nuevo o existente)
            var model = _service.CargarQuote(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guardar([FromBody] EasyQuoteViewModel vm)
        {
            if (vm == null) return BadRequest("Modelo vacío");
            var result = _service.Guardar(vm);
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Calcular([FromBody] EasyQuoteViewModel vm)
        {
            if (vm == null) return BadRequest("Modelo vacío");
            var result = _service.Calcular(vm);
            return Json(result);
        }
    }
}
