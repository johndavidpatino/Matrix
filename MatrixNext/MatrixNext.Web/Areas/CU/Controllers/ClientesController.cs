using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Data.Modules.CU.Clientes.Models;
using MatrixNext.Data.Modules.CU.Clientes.Services;
using System.Security.Claims;

namespace MatrixNext.Web.Areas.CU.Controllers;

/// <summary>
/// Controller para gestión de Clientes y Contactos
/// </summary>
[Area("CU")]
[Authorize]
public class ClientesController : Controller
{
    private readonly IClienteService _service;
    private readonly ILogger<ClientesController> _logger;

    public ClientesController(IClienteService service, ILogger<ClientesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out int userId) ? userId : 0;
    }

    #region Clientes

    /// <summary>
    /// Vista principal de clientes
    /// </summary>
    public async Task<IActionResult> Index(long? idCliente = null)
    {
        await CargarCatalogosViewBag();

        if (idCliente.HasValue)
        {
            var cliente = await _service.ObtenerClientePorIdAsync(idCliente.Value);
            ViewBag.ClienteEditar = cliente;
        }

        var clientes = await _service.ObtenerClientesAsync();
        return View(clientes);
    }

    /// <summary>
    /// Buscar clientes
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Buscar([FromQuery] ClienteBusquedaParams filtros)
    {
        var clientes = await _service.ObtenerClientesAsync(filtros);
        return PartialView("_ListaClientes", clientes);
    }

    /// <summary>
    /// Obtener cliente por ID (para modal)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Obtener(long id)
    {
        var cliente = await _service.ObtenerClientePorIdAsync(id);
        if (cliente == null)
            return NotFound(new { success = false, message = "Cliente no encontrado" });

        return Json(cliente);
    }

    /// <summary>
    /// Modal para crear/editar cliente
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> CreateEdit(long? id = null)
    {
        await CargarCatalogosViewBag();

        if (id.HasValue)
        {
            var cliente = await _service.ObtenerClientePorIdAsync(id.Value);
            if (cliente == null)
                return NotFound();

            var dto = new ClienteCreateEditDto
            {
                Id = cliente.Id,
                Nit = cliente.Nit ?? 0,
                GrupoEconomico = cliente.GrupoEconomico,
                RazonSocial = cliente.RazonSocial,
                IdCiudad = cliente.IdCiudad,
                Apodo = cliente.Apodo,
                IdTipoCliente = cliente.IdTipoCliente,
                Direccion = cliente.Direccion,
                Telefono = cliente.Telefono,
                IdSector = cliente.IdSector,
                Anticipo = cliente.Anticipo,
                Saldo = cliente.Saldo,
                Plazo = cliente.Plazo
            };
            return PartialView("_CreateEditCliente", dto);
        }

        return PartialView("_CreateEditCliente", new ClienteCreateEditDto());
    }

    /// <summary>
    /// Guardar cliente (crear o actualizar)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Guardar([FromBody] ClienteCreateEditDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = string.Join(", ", errors) });
        }

        var userId = GetUserId();

        if (dto.Id.HasValue && dto.Id > 0)
        {
            var (success, message) = await _service.ActualizarClienteAsync(dto, userId);
            return Json(new { success, message });
        }
        else
        {
            var (success, message, id) = await _service.CrearClienteAsync(dto, userId);
            return Json(new { success, message, id });
        }
    }

    #endregion

    #region Contactos

    /// <summary>
    /// Vista de contactos de un cliente
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Contactos(long idCliente)
    {
        var cliente = await _service.ObtenerClientePorIdAsync(idCliente);
        if (cliente == null)
            return RedirectToAction(nameof(Index));

        ViewBag.Cliente = cliente;
        var contactos = await _service.ObtenerContactosPorClienteAsync(idCliente);
        return View(contactos);
    }

    /// <summary>
    /// Lista de contactos (partial)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListaContactos(long idCliente, string? buscar = null)
    {
        var contactos = await _service.ObtenerContactosPorClienteAsync(idCliente);

        if (!string.IsNullOrEmpty(buscar))
        {
            contactos = contactos.Where(c => 
                c.Nombre.Contains(buscar, StringComparison.OrdinalIgnoreCase));
        }

        return PartialView("_ListaContactos", contactos);
    }

    /// <summary>
    /// Modal para crear/editar contacto
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> CreateEditContacto(long idCliente, long? id = null)
    {
        var cliente = await _service.ObtenerClientePorIdAsync(idCliente);
        if (cliente == null)
            return NotFound(new { message = "Cliente no encontrado" });

        ViewBag.Cliente = cliente;

        if (id.HasValue)
        {
            var contacto = await _service.ObtenerContactoPorIdAsync(id.Value);
            if (contacto == null)
                return NotFound();

            var dto = new ContactoCreateEditDto
            {
                Id = contacto.Id,
                Nombre = contacto.Nombre,
                Telefono = contacto.Telefono,
                Celular = contacto.Celular,
                Email = contacto.Email,
                Cargo = contacto.Cargo,
                Activo = contacto.Activo,
                IdCliente = contacto.IdCliente
            };
            return PartialView("_CreateEditContacto", dto);
        }

        return PartialView("_CreateEditContacto", new ContactoCreateEditDto { IdCliente = idCliente });
    }

    /// <summary>
    /// Guardar contacto (crear o actualizar)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GuardarContacto([FromBody] ContactoCreateEditDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = string.Join(", ", errors) });
        }

        var userId = GetUserId();

        if (dto.Id.HasValue && dto.Id > 0)
        {
            var (success, message) = await _service.ActualizarContactoAsync(dto, userId);
            return Json(new { success, message });
        }
        else
        {
            var (success, message, id) = await _service.CrearContactoAsync(dto, userId);
            return Json(new { success, message, id });
        }
    }

    #endregion

    #region Catálogos (API)

    /// <summary>
    /// Obtener países
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Paises()
    {
        var paises = await _service.ObtenerPaisesAsync();
        return Json(paises);
    }

    /// <summary>
    /// Obtener departamentos por país
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Departamentos(int idPais)
    {
        var departamentos = await _service.ObtenerDepartamentosPorPaisAsync(idPais);
        return Json(departamentos);
    }

    /// <summary>
    /// Obtener ciudades por departamento
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Ciudades(int idDepartamento)
    {
        var ciudades = await _service.ObtenerCiudadesPorDepartamentoAsync(idDepartamento);
        return Json(ciudades);
    }

    /// <summary>
    /// Obtener sectores
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Sectores()
    {
        var sectores = await _service.ObtenerSectoresAsync();
        return Json(sectores);
    }

    /// <summary>
    /// Obtener tipos de cliente
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> TiposCliente()
    {
        var tipos = await _service.ObtenerTiposClienteAsync();
        return Json(tipos);
    }

    #endregion

    #region Helpers

    private async Task CargarCatalogosViewBag()
    {
        ViewBag.Paises = await _service.ObtenerPaisesAsync();
        ViewBag.Sectores = await _service.ObtenerSectoresAsync();
        ViewBag.TiposCliente = await _service.ObtenerTiposClienteAsync();
    }

    #endregion
}
