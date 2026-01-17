using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.DTOs;
using MatrixNext.Web.Services.EQ;
using MatrixNext.Web.Areas.EQ.Services.Internal;
using MatrixNext.Web.Areas.EQ.Models;

namespace MatrixNext.Web.Areas.EQ.Controllers
{
    [Area("EQ")]
    [Authorize]
    [Route("EQ/[controller]/[action]")]
    public class EasyQuoteController : Controller
    {
        private readonly EasyCostService _costService;
        private readonly EasyQuoteRetrievalService _retrievalService;

        public EasyQuoteController(
            EasyCostService costService,
            EasyQuoteRetrievalService retrievalService)
        {
            _costService = costService ?? throw new ArgumentNullException(nameof(costService));
            _retrievalService = retrievalService ?? throw new ArgumentNullException(nameof(retrievalService));
        }

        [HttpGet]
        public async Task<IActionResult> Index(long? id)
        {
            // carga inicial (nuevo o existente)
            EasyQuoteViewModel model;

            if (id.HasValue)
            {
                var retrieved = await _retrievalService.GetQuoteByIdAsync(id.Value);
                if (retrieved == null)
                    TempData["Error"] = $"Quote {id} no encontrada";
                model = retrieved ?? new EasyQuoteViewModel(); // Recuperar existente o crear nueva
            }
            else
            {
                model = new EasyQuoteViewModel(); // Nueva quote
            }
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar([FromBody] EasyQuoteViewModel vm)
        {
            if (vm == null) return BadRequest("Modelo vacío");
            
            try
            {
                var result = await _costService.SaveQuoteWithCostAsync(vm);
                return Json(new 
                { 
                    success = true, 
                    id = result.QuoteId, 
                    summary = result.Summary 
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Error al guardar cotización. Por favor intente nuevamente." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Calcular([FromBody] EasyQuoteViewModel vm)
        {
            if (vm == null) return BadRequest("Modelo vacío");
            
            try
            {
                var summary = _costService.CalculateCost(vm);
                return Json(new { success = true, summary });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Error al calcular cotización. Por favor intente nuevamente." });
            }
        }
    }
}
