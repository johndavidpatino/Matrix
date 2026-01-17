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
    public class BriefDisenoMuestralController : Controller
    {
        private readonly IESBriefDisenoMuestralService _service;

        public BriefDisenoMuestralController(IESBriefDisenoMuestralService service)
        {
            _service = service;
        }

        // GET: ES/BriefDisenoMuestral
        public async Task<IActionResult> Index(long? propuestaId, bool pendientes = false)
        {
            try
            {
                if (pendientes)
                {
                    var briefsPendientes = await _service.ObtenerPendientesAsync();
                    ViewBag.Titulo = "Briefs Pendientes de Respuesta";
                    return View(briefsPendientes);
                }

                if (propuestaId.HasValue)
                {
                    var briefs = await _service.ObtenerPorPropuestaAsync(propuestaId.Value);
                    ViewBag.PropuestaId = propuestaId.Value;
                    return View(briefs);
                }

                var todos = await _service.ObtenerTodosAsync();
                return View(todos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los briefs. Por favor intente nuevamente.";
                return View();
            }
        }

        // GET: ES/BriefDisenoMuestral/Create
        public IActionResult Create(long propuestaId)
        {
            ViewBag.PropuestaId = propuestaId;
            return PartialView("_CreateEdit", new ESBriefDisenoMuestralInputDto { PropuestaId = propuestaId });
        }

        // POST: ES/BriefDisenoMuestral/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ESBriefDisenoMuestralInputDto dto)
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
                return Json(new { success = false, message = "Error al crear el brief. Por favor intente nuevamente." });
            }
        }

        // GET: ES/BriefDisenoMuestral/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var brief = await _service.ObtenerPorIdAsync(id);
                if (brief == null)
                {
                    return NotFound();
                }

                var dto = new ESBriefDisenoMuestralInputDto
                {
                    PropuestaId = brief.PropuestaId,
                    Objetivo = brief.Objetivo,
                    Poblacion = brief.Poblacion,
                    Capacidad = brief.Capacidad,
                    Metodologia = brief.Metodologia,
                    NivelesDesagregacion = brief.NivelesDesagregacion,
                    PosiblesMarcos = brief.PosiblesMarcos,
                    Variable = brief.Variable,
                    Observaciones = brief.Observaciones
                };

                ViewBag.Id = id;
                return PartialView("_CreateEdit", dto);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al cargar el brief. Por favor intente nuevamente." });
            }
        }

        // POST: ES/BriefDisenoMuestral/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, ESBriefDisenoMuestralInputDto dto)
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
                return Json(new { success = false, message = "Error al actualizar el brief. Por favor intente nuevamente." });
            }
        }

        // GET: ES/BriefDisenoMuestral/Details/5
        public async Task<IActionResult> Details(long id)
        {
            try
            {
                var brief = await _service.ObtenerPorIdAsync(id);
                if (brief == null)
                {
                    return NotFound();
                }

                return PartialView("_Details", brief);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al cargar los detalles. Por favor intente nuevamente." });
            }
        }

        // POST: ES/BriefDisenoMuestral/Delete/5
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
                return Json(new { success = false, message = "Error al eliminar el brief. Por favor intente nuevamente." });
            }
        }
    }
}
