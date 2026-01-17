using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Data.DTOs.ES;
using MatrixNext.Data.Services.ES;
using System;
using System.Threading.Tasks;

namespace MatrixNext.Web.Areas.ES.Controllers
{
    [Area("ES")]
    [Authorize]
    public class DisenoMuestralController : Controller
    {
        private readonly IESDisenoMuestralService _service;
        private readonly IESBriefDisenoMuestralService _briefService;

        public DisenoMuestralController(
            IESDisenoMuestralService service,
            IESBriefDisenoMuestralService briefService)
        {
            _service = service;
            _briefService = briefService;
        }

        // GET: ES/DisenoMuestral
        public async Task<IActionResult> Index(long? briefId)
        {
            try
            {
                if (briefId.HasValue)
                {
                    var disenos = await _service.ObtenerPorBriefAsync(briefId.Value);
                    ViewBag.BriefId = briefId.Value;
                    return View(disenos);
                }

                var todos = await _service.ObtenerTodosAsync();
                return View(todos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los diseños. Por favor intente nuevamente.";
                return View();
            }
        }

        // GET: ES/DisenoMuestral/Create
        public IActionResult Create(long briefId)
        {
            ViewBag.BriefId = briefId;
            return PartialView("_CreateEdit", new ESDisenoMuestralInputDto { BriefId = briefId });
        }

        // POST: ES/DisenoMuestral/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ESDisenoMuestralInputDto dto)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateEdit", dto);
            }

            try
            {
                var (success, message, id) = await _service.CrearAsync(dto);

                if (success)
                {
                    return Json(new { success = true, message });
                }

                ModelState.AddModelError("", message);
                return PartialView("_CreateEdit", dto);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al crear el diseño. Por favor intente nuevamente." });
            }
        }

        // GET: ES/DisenoMuestral/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var diseno = await _service.ObtenerPorIdAsync(id);
                if (diseno == null)
                {
                    return NotFound();
                }

                var dto = new ESDisenoMuestralInputDto
                {
                    BriefId = diseno.BriefId,
                    MuestroProbabilistico = diseno.MuestroProbabilistico,
                    Objetivo = diseno.Objetivo,
                    Poblacion = diseno.Poblacion,
                    Mercado = diseno.Mercado,
                    Marco = diseno.Marco,
                    Tecnica = diseno.Tecnica,
                    Diseno = diseno.Diseno,
                    Tamano = diseno.Tamano,
                    Fiabilidad = diseno.Fiabilidad,
                    Desagregacion = diseno.Desagregacion,
                    Fuente = diseno.Fuente,
                    Ponderacion = diseno.Ponderacion,
                    Variable = diseno.Variable,
                    ObjetivoT = diseno.ObjetivoT,
                    PoblacionT = diseno.PoblacionT,
                    MercadoT = diseno.MercadoT,
                    MarcoT = diseno.MarcoT,
                    TecnicaT = diseno.TecnicaT,
                    DisenoT = diseno.DisenoT,
                    TamanoT = diseno.TamanoT,
                    FiabilidadT = diseno.FiabilidadT,
                    DesagregacionT = diseno.DesagregacionT,
                    FuenteT = diseno.FuenteT,
                    PonderacionT = diseno.PonderacionT,
                    VariableT = diseno.VariableT,
                    Observaciones = diseno.Observaciones,
                    ObservacionesT = diseno.ObservacionesT
                };

                ViewBag.Id = id;
                return PartialView("_CreateEdit", dto);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al cargar el diseño. Por favor intente nuevamente." });
            }
        }

        // POST: ES/DisenoMuestral/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, ESDisenoMuestralInputDto dto)
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
                return Json(new { success = false, message = "Error al actualizar el diseño. Por favor intente nuevamente." });
            }
        }

        // GET: ES/DisenoMuestral/Details/5
        public async Task<IActionResult> Details(long id)
        {
            try
            {
                var diseno = await _service.ObtenerPorIdAsync(id);
                if (diseno == null)
                {
                    return NotFound();
                }

                return PartialView("_Details", diseno);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al cargar los detalles. Por favor intente nuevamente." });
            }
        }

        // POST: ES/DisenoMuestral/Delete/5
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
                return Json(new { success = false, message = "Error al eliminar el diseño. Por favor intente nuevamente." });
            }
        }
    }
}
