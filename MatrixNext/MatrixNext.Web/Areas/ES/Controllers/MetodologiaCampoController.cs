using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Data.DTOs.ES;
using MatrixNext.Data.Services.ES;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MatrixNext.Web.Areas.ES.Controllers
{
    [Area("ES")]
    [Authorize]
    public class MetodologiaCampoController : Controller
    {
        private readonly IESMetodologiaCampoService _service;

        public MetodologiaCampoController(IESMetodologiaCampoService service)
        {
            _service = service;
        }

        // GET: ES/MetodologiaCampo
        public async Task<IActionResult> Index(long? trabajoId, bool pendientes = false)
        {
            try
            {
                if (pendientes)
                {
                    var metodologiasPendientes = await _service.ObtenerPendientesAsync();
                    ViewBag.Titulo = "Metodologías Pendientes";
                    return View(metodologiasPendientes);
                }

                if (trabajoId.HasValue)
                {
                    var metodologias = await _service.ObtenerPorTrabajoAsync(trabajoId.Value);
                    ViewBag.TrabajoId = trabajoId.Value;
                    return View(metodologias);
                }

                var todas = await _service.ObtenerTodosAsync();
                return View(todas);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar las metodologías. Por favor intente nuevamente.";
                return View();
            }
        }

        // GET: ES/MetodologiaCampo/Create
        public IActionResult Create(long trabajoId)
        {
            ViewBag.TrabajoId = trabajoId;
            return PartialView("_CreateEdit", new ESMetodologiaCampoInputDto { TrabajoId = trabajoId });
        }

        // POST: ES/MetodologiaCampo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ESMetodologiaCampoInputDto dto)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateEdit", dto);
            }

            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var usuarioId = !string.IsNullOrEmpty(userIdClaim) ? long.Parse(userIdClaim) : 0;
                var (success, message, id) = await _service.CrearAsync(dto, usuarioId);

                if (success)
                {
                    return Json(new { success = true, message });
                }

                ModelState.AddModelError("", message);
                return PartialView("_CreateEdit", dto);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al crear la metodología. Por favor intente nuevamente." });
            }
        }

        // GET: ES/MetodologiaCampo/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var metodologia = await _service.ObtenerPorIdAsync(id);
                if (metodologia == null)
                {
                    return NotFound();
                }

                var dto = new ESMetodologiaCampoInputDto
                {
                    TrabajoId = metodologia.TrabajoId,
                    NombreEstudio = metodologia.NombreEstudio,
                    Objetivo = metodologia.Objetivo,
                    Mercado = metodologia.Mercado,
                    Marco = metodologia.Marco,
                    Tecnica = metodologia.Tecnica,
                    Diseno = metodologia.Diseno,
                    Instrucciones = metodologia.Instrucciones,
                    Distribucion = metodologia.Distribucion,
                    NivelConfianza = metodologia.NivelConfianza,
                    MargenError = metodologia.MargenError,
                    Desagregacion = metodologia.Desagregacion,
                    Fuente = metodologia.Fuente,
                    Variables = metodologia.Variables,
                    Tasa = metodologia.Tasa,
                    Procedimiento = metodologia.Procedimiento,
                    ObjetivoT = metodologia.ObjetivoT,
                    MercadoT = metodologia.MercadoT,
                    MarcoT = metodologia.MarcoT,
                    TecnicaT = metodologia.TecnicaT,
                    DisenoT = metodologia.DisenoT,
                    InstruccionesT = metodologia.InstruccionesT,
                    DistribucionT = metodologia.DistribucionT,
                    NivelConfianzaT = metodologia.NivelConfianzaT,
                    MargenErrorT = metodologia.MargenErrorT,
                    DesagregacionT = metodologia.DesagregacionT,
                    FuenteT = metodologia.FuenteT,
                    VariablesT = metodologia.VariablesT,
                    TasaT = metodologia.TasaT,
                    ProcedimientoT = metodologia.ProcedimientoT
                };

                ViewBag.Id = id;
                return PartialView("_CreateEdit", dto);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al cargar la metodología. Por favor intente nuevamente." });
            }
        }

        // POST: ES/MetodologiaCampo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, ESMetodologiaCampoInputDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return PartialView("_CreateEdit", dto);
            }

            try
            {
                var (success, message) = await _service.ActualizarAsync(id, dto);

                if (success)
                {
                    return Json(new { success = true, message });
                }

                ModelState.AddModelError("", message);
                ViewBag.Id = id;
                return PartialView("_CreateEdit", dto);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al actualizar la metodología. Por favor intente nuevamente." });
            }
        }

        // GET: ES/MetodologiaCampo/Details/5
        public async Task<IActionResult> Details(long id)
        {
            try
            {
                var metodologia = await _service.ObtenerPorIdAsync(id);
                if (metodologia == null)
                {
                    return NotFound();
                }

                return PartialView("_Details", metodologia);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al cargar los detalles. Por favor intente nuevamente." });
            }
        }

        // POST: ES/MetodologiaCampo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var (success, message) = await _service.EliminarAsync(id);
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar la metodología. Por favor intente nuevamente." });
            }
        }
    }
}
