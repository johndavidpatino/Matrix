using System.Security.Claims;
using MatrixNext.Data.Modules.TH.Contratistas.Models;
using MatrixNext.Data.Modules.TH.Contratistas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MatrixNext.Web.Areas.TH.Controllers;

/// <summary>
/// Controller para la gestión de Contratistas
/// Equivalente a: WebMatrix/TH_TalentoHumano/Contratistas.aspx
/// Permiso: 131
/// </summary>
[Area("TH")]
[Authorize]
[Route("TH/[controller]")]
public class ContratistasController : Controller
{
    private readonly IContratistaService _service;
    private readonly ILogger<ContratistasController> _logger;

    public ContratistasController(IContratistaService service, ILogger<ContratistasController> logger)
    {
        _service = service;
        _logger = logger;
    }

    #region Vistas principales

    /// <summary>
    /// Vista principal de contratistas
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
    /// Lista de contratistas (partial para AJAX)
    /// </summary>
    [HttpGet]
    [Route("Lista")]
    public async Task<IActionResult> Lista(long? identificacion, string? nombre, bool? activo)
    {
        var parametros = new BuscarContratistasParams
        {
            Identificacion = identificacion,
            Nombre = nombre,
            Activo = activo
        };
        
        var contratistas = await _service.ObtenerContratistasAsync(parametros);
        return PartialView("_Lista", contratistas);
    }

    #endregion

    #region CRUD Contratistas

    /// <summary>
    /// Modal para crear nuevo contratista
    /// </summary>
    [HttpGet]
    [Route("Create")]
    public async Task<IActionResult> Create()
    {
        await CargarCombosViewBag();
        var model = new ContratistaCreateEditDto
        {
            FechaRegistro = DateTime.Today,
            Estado = 1,
            EsActualizacion = false
        };
        return PartialView("_CreateEditModal", model);
    }

    /// <summary>
    /// Modal para editar contratista
    /// </summary>
    [HttpGet]
    [Route("Edit/{identificacion}")]
    public async Task<IActionResult> Edit(long identificacion)
    {
        var contratista = await _service.ObtenerContratistaPorIdAsync(identificacion);
        if (contratista == null)
        {
            return NotFound(new { success = false, message = "Contratista no encontrado" });
        }
        
        await CargarCombosViewBag();
        
        var model = new ContratistaCreateEditDto
        {
            Identificacion = contratista.Identificacion,
            Nombre = contratista.Nombre ?? string.Empty,
            Direccion = contratista.Direccion,
            Email = contratista.Email,
            CiudadId = contratista.CiudadId ?? 0,
            NumeroSymphony = contratista.NumeroSymphony ?? 0,
            DescripcionCuenta = contratista.DescripcionCuenta,
            Telefono = contratista.Telefono,
            FechaRegistro = contratista.FechaRegistro ?? DateTime.Today,
            Estado = contratista.Estado ?? 1,
            Solicitud = contratista.Solicitud,
            Aprobado = contratista.Aprobado,
            Observaciones = contratista.Observaciones,
            Clasificacion = contratista.Clasificacion ?? 0,
            EsActualizacion = true
        };
        
        return PartialView("_CreateEditModal", model);
    }

    /// <summary>
    /// Guardar contratista (crear o actualizar)
    /// </summary>
    [HttpPost]
    [Route("Save")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save([FromForm] ContratistaCreateEditDto dto)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Datos inválidos. Verifique los campos requeridos." });
        }
        
        var userId = GetUserId();
        var (success, message) = await _service.GuardarContratistaAsync(dto, userId);
        
        return Json(new { success, message });
    }

    /// <summary>
    /// Cambiar estado de contratista
    /// </summary>
    [HttpPost]
    [Route("CambiarEstado")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(long identificacion, int estado)
    {
        var userId = GetUserId();
        var (success, message) = await _service.ActualizarEstadoAsync(identificacion, estado, userId);
        
        return Json(new { success, message });
    }

    /// <summary>
    /// Ver detalles del contratista
    /// </summary>
    [HttpGet]
    [Route("Details/{identificacion}")]
    public async Task<IActionResult> Details(long identificacion)
    {
        var contratista = await _service.ObtenerContratistaPorIdAsync(identificacion);
        if (contratista == null)
        {
            return NotFound(new { success = false, message = "Contratista no encontrado" });
        }
        
        return PartialView("_Details", contratista);
    }

    #endregion

    #region Servicios de Contratista

    /// <summary>
    /// Modal de servicios del contratista
    /// </summary>
    [HttpGet]
    [Route("Servicios/{identificacion}")]
    public async Task<IActionResult> Servicios(long identificacion)
    {
        var contratista = await _service.ObtenerContratistaPorIdAsync(identificacion);
        if (contratista == null)
        {
            return NotFound(new { success = false, message = "Contratista no encontrado" });
        }
        
        var servicios = await _service.ObtenerServiciosContratistaAsync(identificacion);
        var serviciosDisponibles = await _service.ObtenerServiciosComboAsync();
        
        ViewBag.ContratistaId = identificacion;
        ViewBag.ContratistaNombre = contratista.Nombre;
        ViewBag.ServiciosDisponibles = new SelectList(serviciosDisponibles, "Id", "Nombre");
        
        return PartialView("_Servicios", servicios);
    }

    /// <summary>
    /// Agregar servicio a contratista
    /// </summary>
    [HttpPost]
    [Route("AgregarServicio")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarServicio([FromForm] ContratistaServicioCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Datos inválidos" });
        }
        
        var userId = GetUserId();
        var (success, message) = await _service.AgregarServicioAsync(dto, userId);
        
        return Json(new { success, message });
    }

    /// <summary>
    /// Actualizar estado de servicio
    /// </summary>
    [HttpPost]
    [Route("ActualizarServicio")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActualizarServicio(long id, bool estado, long contratistaId)
    {
        var userId = GetUserId();
        var (success, message) = await _service.ActualizarEstadoServicioAsync(id, estado, contratistaId, userId);
        
        return Json(new { success, message });
    }

    #endregion

    #region Log de Contratistas

    /// <summary>
    /// Modal de log de contratistas
    /// </summary>
    [HttpGet]
    [Route("Log/{identificacion?}")]
    public async Task<IActionResult> Log(long? identificacion)
    {
        string? nombre = null;
        if (identificacion.HasValue)
        {
            var contratista = await _service.ObtenerContratistaPorIdAsync(identificacion.Value);
            nombre = contratista?.Nombre;
        }
        
        var logs = await _service.ObtenerLogContratistasAsync(identificacion, null);
        
        ViewBag.ContratistaId = identificacion;
        ViewBag.ContratistaNombre = nombre;
        
        return PartialView("_Log", logs);
    }

    /// <summary>
    /// Buscar en log de contratistas
    /// </summary>
    [HttpGet]
    [Route("BuscarLog")]
    public async Task<IActionResult> BuscarLog(long? identificacion, string? nombre)
    {
        var logs = await _service.ObtenerLogContratistasAsync(identificacion, nombre);
        return PartialView("_LogLista", logs);
    }

    #endregion

    #region Helpers

    private async Task CargarCombosViewBag()
    {
        var estados = await _service.ObtenerEstadosAsync();
        var ciudades = await _service.ObtenerCiudadesAsync();
        var clasificaciones = await _service.ObtenerClasificacionesAsync();
        
        ViewBag.Estados = new SelectList(estados, "Id", "Estado");
        ViewBag.Ciudades = new SelectList(ciudades, "Id", "Ciudad");
        ViewBag.Clasificaciones = new SelectList(clasificaciones, "Id", "Clasificacion");
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
