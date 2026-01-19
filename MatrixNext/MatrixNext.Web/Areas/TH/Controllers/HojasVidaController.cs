using System.Security.Claims;
using MatrixNext.Data.Modules.TH.HojasVida.Models;
using MatrixNext.Data.Modules.TH.HojasVida.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MatrixNext.Web.Areas.TH.Controllers;

/// <summary>
/// Controller para la gestión de Hojas de Vida (Reclutamiento)
/// Equivalente a: WebMatrix/TH_TalentoHumano/HojasVida.aspx
/// Permiso: TH (Talento Humano)
/// </summary>
[Area("TH")]
[Authorize]
[Route("TH/[controller]")]
public class HojasVidaController : Controller
{
    private readonly IHojaVidaService _service;
    private readonly ILogger<HojasVidaController> _logger;

    public HojasVidaController(IHojaVidaService service, ILogger<HojasVidaController> logger)
    {
        _service = service;
        _logger = logger;
    }

    #region Vistas principales

    /// <summary>
    /// Vista principal de hojas de vida
    /// </summary>
    [HttpGet]
    [Route("")]
    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        await CargarCombosViewBag();
        return View();
    }

    /// <summary>
    /// Lista de hojas de vida (partial para AJAX)
    /// </summary>
    [HttpGet]
    [Route("Lista")]
    public async Task<IActionResult> Lista(
        long? id, 
        string? nombres, 
        string? apellidos, 
        byte? nivelIngles,
        string? keywords,
        byte? anosExperienciaInicio,
        byte? anosExperienciaFin,
        short? nivelEducativo,
        short? ciudadResidencia,
        bool? tieneEntrevista,
        short? profesion)
    {
        var parametros = new BuscarHojasVidaParams
        {
            Id = id,
            Nombres = nombres,
            Apellidos = apellidos,
            NivelIngles = nivelIngles,
            Keywords = keywords,
            AnosExperienciaInicio = anosExperienciaInicio,
            AnosExperienciaFin = anosExperienciaFin,
            NivelEducativo = nivelEducativo,
            CiudadResidencia = ciudadResidencia,
            TieneEntrevista = tieneEntrevista,
            Profesion = profesion
        };
        
        var hojasVida = await _service.ObtenerHojasVidaAsync(parametros);
        return PartialView("_Lista", hojasVida);
    }

    #endregion

    #region CRUD Hojas de Vida

    /// <summary>
    /// Modal para crear nueva hoja de vida
    /// </summary>
    [HttpGet]
    [Route("Create")]
    public async Task<IActionResult> Create()
    {
        await CargarCombosViewBag();
        var model = new HojaVidaCreateEditDto
        {
            TipoIdentificacion = 1,
            EsActualizacion = false
        };
        return PartialView("_CreateEditModal", model);
    }

    /// <summary>
    /// Modal para editar hoja de vida
    /// </summary>
    [HttpGet]
    [Route("Edit/{id}")]
    public async Task<IActionResult> Edit(long id)
    {
        var hojaVida = await _service.ObtenerHojaVidaPorIdAsync(id);
        if (hojaVida == null)
        {
            return NotFound(new { success = false, message = "Hoja de vida no encontrada" });
        }
        
        await CargarCombosViewBag();
        
        var model = new HojaVidaCreateEditDto
        {
            Id = hojaVida.Id,
            TipoIdentificacion = hojaVida.TipoIdentificacion ?? 1,
            Identificacion = hojaVida.Identificacion ?? string.Empty,
            Nombres = hojaVida.Nombres ?? string.Empty,
            Apellidos = hojaVida.Apellidos ?? string.Empty,
            Edad = hojaVida.Edad,
            AnosExperiencia = hojaVida.AnosExperiencia,
            NivelIngles = hojaVida.NivelIngles,
            NumeroCelular = hojaVida.NumeroCelular,
            Correo = hojaVida.Correo,
            CiudadResidencia = hojaVida.CiudadResidencia,
            NivelEducativo = hojaVida.NivelEducativo,
            Profesion = hojaVida.Profesion,
            EsActualizacion = true
        };
        
        return PartialView("_CreateEditModal", model);
    }

    /// <summary>
    /// Guardar hoja de vida (crear o actualizar)
    /// </summary>
    [HttpPost]
    [Route("Save")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save([FromForm] HojaVidaCreateEditDto dto)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Datos inválidos. Verifique los campos requeridos." });
        }
        
        var (success, message, id) = await _service.GuardarHojaVidaAsync(dto);
        
        return Json(new { success, message, id });
    }

    /// <summary>
    /// Ver detalles de la hoja de vida
    /// </summary>
    [HttpGet]
    [Route("Details/{id}")]
    public async Task<IActionResult> Details(long id)
    {
        var hojaVida = await _service.ObtenerHojaVidaPorIdAsync(id);
        if (hojaVida == null)
        {
            return NotFound(new { success = false, message = "Hoja de vida no encontrada" });
        }
        
        // Cargar entrevistas y experiencias laborales
        ViewBag.Entrevistas = await _service.ObtenerEntrevistasAsync(id);
        ViewBag.ExperienciasLaborales = await _service.ObtenerExperienciasLaboralesAsync(id);
        
        return PartialView("_Details", hojaVida);
    }

    #endregion

    #region Entrevistas

    /// <summary>
    /// Obtener entrevistas de una hoja de vida
    /// </summary>
    [HttpGet]
    [Route("Entrevistas/{hojaVidaId}")]
    public async Task<IActionResult> Entrevistas(long hojaVidaId)
    {
        var entrevistas = await _service.ObtenerEntrevistasAsync(hojaVidaId);
        ViewBag.HojaVidaId = hojaVidaId;
        return PartialView("_Entrevistas", entrevistas);
    }

    /// <summary>
    /// Agregar entrevista
    /// </summary>
    [HttpPost]
    [Route("AgregarEntrevista")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarEntrevista([FromForm] HojaVidaEntrevistaCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Datos inválidos" });
        }
        
        var (success, message) = await _service.AgregarEntrevistaAsync(dto);
        
        return Json(new { success, message });
    }

    /// <summary>
    /// Eliminar entrevista
    /// </summary>
    [HttpPost]
    [Route("EliminarEntrevista")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarEntrevista(long id)
    {
        var (success, message) = await _service.EliminarEntrevistaAsync(id);
        
        return Json(new { success, message });
    }

    #endregion

    #region Experiencias Laborales

    /// <summary>
    /// Obtener experiencias laborales de una hoja de vida
    /// </summary>
    [HttpGet]
    [Route("ExperienciasLaborales/{hojaVidaId}")]
    public async Task<IActionResult> ExperienciasLaborales(long hojaVidaId)
    {
        var experiencias = await _service.ObtenerExperienciasLaboralesAsync(hojaVidaId);
        ViewBag.HojaVidaId = hojaVidaId;
        return PartialView("_ExperienciasLaborales", experiencias);
    }

    /// <summary>
    /// Agregar experiencia laboral
    /// </summary>
    [HttpPost]
    [Route("AgregarExperienciaLaboral")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarExperienciaLaboral([FromForm] HojaVidaExperienciaLaboralCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Datos inválidos" });
        }
        
        var (success, message, id) = await _service.AgregarExperienciaLaboralAsync(dto);
        
        return Json(new { success, message, id });
    }

    /// <summary>
    /// Eliminar experiencia laboral
    /// </summary>
    [HttpPost]
    [Route("EliminarExperienciaLaboral")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarExperienciaLaboral(long id)
    {
        var (success, message) = await _service.EliminarExperienciaLaboralAsync(id);
        
        return Json(new { success, message });
    }

    #endregion

    #region Keywords

    /// <summary>
    /// Agregar keyword a hoja de vida
    /// </summary>
    [HttpPost]
    [Route("AgregarKeyword")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarKeyword(long hojaVidaId, string keyword)
    {
        var (success, message) = await _service.AgregarKeywordAsync(hojaVidaId, keyword);
        
        return Json(new { success, message });
    }

    /// <summary>
    /// Eliminar keyword de hoja de vida
    /// </summary>
    [HttpPost]
    [Route("EliminarKeyword")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarKeyword(long hojaVidaId, string keyword)
    {
        var (success, message) = await _service.EliminarKeywordAsync(hojaVidaId, keyword);
        
        return Json(new { success, message });
    }

    #endregion

    #region API Combos (JSON)

    /// <summary>
    /// Obtener profesiones para combo
    /// </summary>
    [HttpGet]
    [Route("api/profesiones")]
    public async Task<IActionResult> GetProfesiones()
    {
        var profesiones = await _service.ObtenerProfesionesAsync();
        return Json(profesiones);
    }

    /// <summary>
    /// Obtener niveles educativos para combo
    /// </summary>
    [HttpGet]
    [Route("api/niveles-educativos")]
    public async Task<IActionResult> GetNivelesEducativos()
    {
        var niveles = await _service.ObtenerNivelesEducativosAsync();
        return Json(niveles);
    }

    /// <summary>
    /// Obtener ciudades para combo
    /// </summary>
    [HttpGet]
    [Route("api/ciudades")]
    public async Task<IActionResult> GetCiudades()
    {
        var ciudades = await _service.ObtenerCiudadesAsync();
        return Json(ciudades);
    }

    #endregion

    #region Helpers

    private async Task CargarCombosViewBag()
    {
        var tiposIdentificacion = await _service.ObtenerTiposIdentificacionAsync();
        var nivelesEducativos = await _service.ObtenerNivelesEducativosAsync();
        var ciudades = await _service.ObtenerCiudadesAsync();
        var profesiones = await _service.ObtenerProfesionesAsync();
        
        ViewBag.TiposIdentificacion = new SelectList(tiposIdentificacion, "Id", "TipoIdentificacion");
        ViewBag.NivelesEducativos = new SelectList(nivelesEducativos, "Id", "NivelEducativo");
        ViewBag.Ciudades = new SelectList(ciudades, "Id", "Ciudad");
        ViewBag.Profesiones = new SelectList(profesiones, "Id", "Profesion");
        
        // Niveles de inglés (lista fija)
        ViewBag.NivelesIngles = new SelectList(new[]
        {
            new { Id = (byte)1, Nombre = "Básico" },
            new { Id = (byte)2, Nombre = "Intermedio" },
            new { Id = (byte)3, Nombre = "Avanzado" },
            new { Id = (byte)4, Nombre = "Nativo/Bilingüe" }
        }, "Id", "Nombre");
    }

    /// <summary>
    /// Obtener el ID del usuario autenticado
    /// </summary>
    private long GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                       ?? User.FindFirst("UserId")?.Value
                       ?? User.FindFirst("sub")?.Value;
        
        if (long.TryParse(userIdClaim, out var userId))
            return userId;
        
        return 0;
    }

    #endregion
}
