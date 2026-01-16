using MatrixNext.Core.DTOs.RE_GT;
using MatrixNext.Web.Services.RE_GT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MatrixNext.Web.Areas.RE_GT.Controllers
{
    /// <summary>
    /// Controller para gestión de asignación de trabajos a coordinadores de campo
    /// </summary>
    [Area("RE_GT")]
    [Authorize]
    public class AsignacionCampoController : Controller
    {
        private readonly IAsignacionCampoService _service;
        private readonly ILogger<AsignacionCampoController> _logger;

        public AsignacionCampoController(IAsignacionCampoService service, ILogger<AsignacionCampoController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// GET: Muestra página con GridView de trabajos sin asignación
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Usuario {User} accedió a página de AsignacionCampo", User.Identity?.Name);
                
                // Cargar datos iniciales
                var coes = await _service.ObtenerCOEsAsync();
                ViewBag.COEs = coes;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar página de AsignacionCampo");
                return BadRequest(new { success = false, message = "Error al cargar la página" });
            }
        }

        /// <summary>
        /// GET: Obtiene trabajos para llenar grid (AJAX)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerTrabajosGrid(int pageIndex = 0, int pageSize = 10, 
            string nombrePropuesta = "", string jobBook = "", string metCodigo = "")
        {
            try
            {
                var busqueda = new BusquedaAsignacionDto
                {
                    NombrePropuesta = nombrePropuesta,
                    JobBook = jobBook,
                    MetCodigo = metCodigo,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                _logger.LogInformation("Obteniendo trabajos para grid. PageIndex: {PageIndex}, PageSize: {PageSize}",
                    pageIndex, pageSize);

                var (trabajos, totalRecords) = await _service.ObtenerTrabajosParaAsignacionAsync(busqueda);

                return Json(new
                {
                    success = true,
                    data = trabajos,
                    totalRecords = totalRecords,
                    pageIndex = pageIndex,
                    pageSize = pageSize
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo trabajos para grid");
                return Json(new { success = false, message = "Error al obtener trabajos" });
            }
        }

        /// <summary>
        /// GET: Obtiene información de trabajo para modal (AJAX)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerDetallesTrabajo(int idTrabajo)
        {
            try
            {
                if (idTrabajo <= 0)
                {
                    return Json(new { success = false, message = "ID de Trabajo inválido" });
                }

                _logger.LogInformation("Obteniendo detalles de trabajo {IdTrabajo}", idTrabajo);

                var trabajo = await _service.ObtenerTrabajoAsync(idTrabajo);
                if (trabajo == null)
                {
                    _logger.LogWarning("Trabajo no encontrado: {IdTrabajo}", idTrabajo);
                    return Json(new { success = false, message = "El trabajo NO existe" });
                }

                // Obtener usuarios COE
                var usuariosCOE = await _service.ObtenerUsuariosCOEAsync();

                return Json(new
                {
                    success = true,
                    trabajo = trabajo,
                    usuariosCOE = usuariosCOE
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo detalles de trabajo {IdTrabajo}", idTrabajo);
                return Json(new { success = false, message = "Error al obtener detalles" });
            }
        }

        /// <summary>
        /// POST: Valida datos antes de asignación (AJAX)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ValidarAsignacion(int idTrabajo, int idCOE, int? idPersona = null)
        {
            try
            {
                if (idTrabajo <= 0 || idCOE <= 0)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                _logger.LogInformation("Validando asignación. IdTrabajo: {IdTrabajo}, IdCOE: {IdCOE}", 
                    idTrabajo, idCOE);

                // Validar que el trabajo existe
                var (valid, message) = await _service.ValidarTrabajoAsync(idTrabajo);
                if (!valid)
                {
                    _logger.LogWarning("Validación fallida: {Message}", message);
                    return Json(new { success = false, message = message });
                }

                _logger.LogInformation("Validación exitosa para IdTrabajo: {IdTrabajo}", idTrabajo);
                return Json(new { success = true, message = "Validación exitosa" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando asignación");
                return Json(new { success = false, message = "Error al validar" });
            }
        }

        /// <summary>
        /// POST: Realiza la asignación del trabajo (AJAX)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Asignar(AsignacionCampoDto dto)
        {
            try
            {
                if (dto == null || dto.IdTrabajo <= 0 || dto.IdCOE <= 0)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var usuarioId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
                
                _logger.LogInformation("Iniciando asignación de trabajo {IdTrabajo}, usuario {UserId}",
                    dto.IdTrabajo, usuarioId);

                var (success, message) = await _service.AsignarTrabajoCampoAsync(dto, usuarioId);

                if (success)
                {
                    _logger.LogInformation("Asignación exitosa para trabajo {IdTrabajo}", dto.IdTrabajo);
                }
                else
                {
                    _logger.LogWarning("Asignación falló para trabajo {IdTrabajo}: {Message}", dto.IdTrabajo, message);
                }

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error realizando asignación de trabajo {IdTrabajo}", dto?.IdTrabajo);
                return Json(new { success = false, message = "Error al realizar la asignación" });
            }
        }
    }
}
