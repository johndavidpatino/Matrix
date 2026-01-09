using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Services.OP.Models;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.OP.Controllers;

/// <summary>
/// Controller para gestión de trabajos cualitativos (COE)
/// Ref: ANALISIS_OP_CUALITATIVO_FASE3_FLUJO1.md § FLUJO 1 (7 pasos)
/// WebForm original: Trabajos.aspx.vb (217 LOC) del módulo OP_Cualitativo
/// Tareas: OP-C01
/// </summary>
[Area("OP")]
[Authorize]
[Route("OP/Cualitativo/Trabajos")]
public class CualitativoTrabajosController : Controller
{
    private readonly IOpCualitativoService _cualitativoService;
    private readonly ILogger<CualitativoTrabajosController> _logger;

    public CualitativoTrabajosController(
        IOpCualitativoService cualitativoService,
        ILogger<CualitativoTrabajosController> logger)
    {
        _cualitativoService = cualitativoService;
        _logger = logger;
    }

    /// <summary>
    /// PASO 1.1-1.2: Index - Lista de trabajos cualitativos filtrada por coordinador
    /// Ref: Trabajos.aspx.vb líneas 21-47 (Page_Load, CargarTrabajos)
    /// </summary>
    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(long? coeId = null)
    {
        try
        {
            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            // Verificar permiso 42 (Coordinador COE)
            var esCoordinador = await _cualitativoService.ValidarPermisoCoordinadorAsync(usuarioId, 42);
            var tienePermisoOperaciones = await _cualitativoService.ValidarPermisoCoordinadorAsync(usuarioId, 148);

            List<TrabajoCualitativoVm> trabajos;
            string error;
            bool success;

            if (esCoordinador)
            {
                // Filtrar por coordinador (PASO 1.2)
                (success, trabajos, error) = await _cualitativoService.ObtenerTrabajosPorCoordinadorAsync(
                    usuarioId, coeId);
            }
            else if (tienePermisoOperaciones)
            {
                // Ver todos los trabajos cualitativos
                (success, trabajos, error) = await _cualitativoService.ObtenerTrabajosPorCoeAsync(
                    coeId, tipo: 2, estado: null);
            }
            else
            {
                // Ver trabajos por COE específico
                (success, trabajos, error) = await _cualitativoService.ObtenerTrabajosPorCoeAsync(
                    coeId, tipo: null, estado: null);
            }

            if (!success)
            {
                TempData["Error"] = error;
                return View(new List<TrabajoCualitativoVm>());
            }

            ViewBag.EsCoordinador = esCoordinador;
            ViewBag.CoeId = coeId;

            return View(trabajos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando trabajos cualitativos");
            TempData["Error"] = "Error cargando trabajos cualitativos";
            return View(new List<TrabajoCualitativoVm>());
        }
    }

    /// <summary>
    /// PASO 1.3: Búsqueda de trabajos (AJAX)
    /// Ref: Trabajos.aspx.vb líneas 49-78 (btnBuscar_Click)
    /// </summary>
    [HttpGet("Search")]
    public async Task<IActionResult> Search(long? coeId, string nombre, string estado)
    {
        try
        {
            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var esCoordinador = await _cualitativoService.ValidarPermisoCoordinadorAsync(usuarioId, 42);

            List<TrabajoCualitativoVm> trabajos;

            if (esCoordinador)
            {
                var (success, data, error) = await _cualitativoService.ObtenerTrabajosPorCoordinadorAsync(
                    usuarioId, coeId);
                trabajos = success ? data : new List<TrabajoCualitativoVm>();
            }
            else
            {
                var (success, data, error) = await _cualitativoService.ObtenerTrabajosPorCoeAsync(
                    coeId, tipo: null, estado);
                trabajos = success ? data : new List<TrabajoCualitativoVm>();
            }

            // Filtros adicionales del lado del servidor
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                trabajos = trabajos.Where(t => 
                    t.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return PartialView("_TrabajosGrid", trabajos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en búsqueda de trabajos");
            return PartialView("_TrabajosGrid", new List<TrabajoCualitativoVm>());
        }
    }

    /// <summary>
    /// PASO 1.5: Configuración de trabajo (modal)
    /// Ref: Trabajos.aspx.vb líneas 145-167 (CargarConfiguracion)
    /// </summary>
    [HttpGet("GetConfiguration")]
    public async Task<IActionResult> GetConfiguration(long trabajoId)
    {
        try
        {
            var (success, data, error) = await _cualitativoService.ObtenerConfiguracionTrabajoAsync(trabajoId);

            if (!success)
            {
                return Json(new { success = false, message = error });
            }

            return Json(new { success = true, data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo configuración trabajo {TrabajoId}", trabajoId);
            return Json(new { success = false, message = "Error obteniendo configuración" });
        }
    }

    /// <summary>
    /// PASO 1.6: Guardar configuración de trabajo
    /// Ref: Trabajos.aspx.vb líneas 171-195 (btnGuardarConfiguracion_Click)
    /// </summary>
    [HttpPost("SaveConfiguration")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveConfiguration([FromBody] ConfiguracionTrabajoVm configuracion)
    {
        try
        {
            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (success, error) = await _cualitativoService.GuardarConfiguracionTrabajoAsync(
                configuracion.TrabajoId, configuracion, usuarioId);

            if (!success)
            {
                return Json(new { success = false, message = error });
            }

            return Json(new { success = true, message = "Configuración guardada exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error guardando configuración trabajo {TrabajoId}", configuracion.TrabajoId);
            return Json(new { success = false, message = "Error guardando configuración" });
        }
    }

    /// <summary>
    /// Details - Ver detalle de un trabajo
    /// Ref: OP-C01 (Details view)
    /// </summary>
    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(long id)
    {
        try
        {
            var (success, data, error) = await _cualitativoService.ObtenerTrabajoDetalleAsync(id);

            if (!success)
            {
                TempData["Error"] = error;
                return RedirectToAction("Index");
            }

            // Obtener info de navegación
            var (navSuccess, navData, navError) = await _cualitativoService.ObtenerNavegacionTrabajoAsync(id);
            ViewBag.Navegacion = navSuccess ? navData : null;

            return View(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando detalle trabajo {Id}", id);
            TempData["Error"] = "Error cargando detalle del trabajo";
            return RedirectToAction("Index");
        }
    }

    /// <summary>
    /// Create - Formulario crear nuevo trabajo (GET)
    /// Ref: OP-C01 (Create view)
    /// </summary>
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View(new TrabajoCualitativoVm());
    }

    /// <summary>
    /// Create - Guardar nuevo trabajo (POST)
    /// Ref: OP-C01 (Create action)
    /// </summary>
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TrabajoCualitativoVm trabajo)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(trabajo);
            }

            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (success, trabajoId, error) = await _cualitativoService.CrearTrabajoAsync(trabajo, usuarioId);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, error);
                return View(trabajo);
            }

            TempData["Success"] = "Trabajo creado exitosamente";
            return RedirectToAction("Details", new { id = trabajoId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando trabajo");
            ModelState.AddModelError(string.Empty, "Error creando trabajo");
            return View(trabajo);
        }
    }

    /// <summary>
    /// Edit - Formulario editar trabajo (GET)
    /// Ref: OP-C01 (Edit view)
    /// </summary>
    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(long id)
    {
        try
        {
            var (success, data, error) = await _cualitativoService.ObtenerTrabajoDetalleAsync(id);

            if (!success)
            {
                TempData["Error"] = error;
                return RedirectToAction("Index");
            }

            return View(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando edición trabajo {Id}", id);
            TempData["Error"] = "Error cargando trabajo";
            return RedirectToAction("Index");
        }
    }

    /// <summary>
    /// Edit - Actualizar trabajo (POST)
    /// Ref: OP-C01 (Edit action)
    /// </summary>
    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, TrabajoCualitativoVm trabajo)
    {
        try
        {
            if (id != trabajo.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(trabajo);
            }

            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (success, error) = await _cualitativoService.ActualizarTrabajoAsync(trabajo, usuarioId);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, error);
                return View(trabajo);
            }

            TempData["Success"] = "Trabajo actualizado exitosamente";
            return RedirectToAction("Details", new { id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando trabajo {Id}", id);
            ModelState.AddModelError(string.Empty, "Error actualizando trabajo");
            return View(trabajo);
        }
    }

    /// <summary>
    /// Delete - Eliminar trabajo (POST AJAX)
    /// Ref: OP-C01 (Delete action)
    /// </summary>
    [HttpPost("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var usuarioId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (success, error) = await _cualitativoService.EliminarTrabajoAsync(id, usuarioId);

            if (!success)
            {
                return Json(new { success = false, message = error });
            }

            return Json(new { success = true, message = "Trabajo eliminado exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando trabajo {Id}", id);
            return Json(new { success = false, message = "Error eliminando trabajo" });
        }
    }

    /// <summary>
    /// PASO 1.7: Navegación a módulos relacionados
    /// Ref: Trabajos.aspx.vb líneas 80-143 (gvTrabajos_RowCommand con 8 redirecciones)
    /// </summary>
    [HttpGet("NavigateTo")]
    public IActionResult NavigateTo(long trabajoId, string destination)
    {
        // Ref: Trabajos.aspx.vb líneas 80-143 (8 casos de navegación)
        return destination switch
        {
            "FichaEntrevista" => RedirectToAction("EditInterview", "CualitativoFichas", new { area = "OP", trabajoId }),
            "FichaSesion" => RedirectToAction("EditSession", "CualitativoFichas", new { area = "OP", trabajoId }),
            "FichaObservacion" => RedirectToAction("EditObservation", "CualitativoFichas", new { area = "OP", trabajoId }),
            "Muestra" => RedirectToAction("Index", "CualitativoMuestra", new { area = "OP", trabajoId }),
            "FiltroReclutamiento" => RedirectToAction("Configure", "CualitativoFiltros", new { area = "OP", trabajoId, tipo = 1 }),
            "FiltroAsistencia" => RedirectToAction("Configure", "CualitativoFiltros", new { area = "OP", trabajoId, tipo = 2 }),
            "Campo" => RedirectToAction("Index", "CualitativoCampo", new { area = "OP", trabajoId }),
            "Programacion" => RedirectToAction("Index", "CualitativoProgramacion", new { area = "OP", trabajoId }),
            _ => RedirectToAction("Index")
        };
    }
}
