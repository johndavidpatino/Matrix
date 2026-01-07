using MatrixNext.Web.Services.CORE;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.CORE.Controllers
{
    [Area("CORE")]
    [Route("api/core/indicadores")]
    [Authorize(Roles = "Administrador,Gerente,Coordinador")]
    public class IndicadoresController : Controller
    {
        private readonly IIndicadoresCumplimientoService _indicadoresService;

        public IndicadoresController(IIndicadoresCumplimientoService indicadoresService)
        {
            _indicadoresService = indicadoresService;
        }

        [HttpGet("resumen")]
        public async Task<IActionResult> ObtenerResumen()
        {
            var resultado = await _indicadoresService.ObtenerResumenIndicadoresAsync();
            return Ok(resultado);
        }

        [HttpGet("por-gerente")]
        public async Task<IActionResult> ObtenerPorGerente()
        {
            var resultado = await _indicadoresService.ObtenerIndicadoresPorGerenteAsync();
            return Ok(resultado);
        }

        [HttpGet("por-tipo-hilo")]
        public async Task<IActionResult> ObtenerPorTipoHilo()
        {
            var resultado = await _indicadoresService.ObtenerIndicadoresPorTipoHiloAsync();
            return Ok(resultado);
        }

        [HttpGet("/core/indicadores")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}
