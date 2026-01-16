using MatrixNext.Data.DTOs.RE_GT;
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
    /// Controller para gestión de cambios de JobBook Interno (JBI)
    /// </summary>
    [Area("RE_GT")]
    [Authorize]
    public class CambioJBIController : Controller
    {
        private readonly ICambioJBIService _service;
        private readonly ILogger<CambioJBIController> _logger;

        public CambioJBIController(ICambioJBIService service, ILogger<CambioJBIController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// GET: Muestra formulario para cambio de JBI
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Usuario {User} accedió a página de CambioJBI", User.Identity?.Name);
                
                // Cargar fases para dropdown
                var fases = await _service.ObtenerFasesAsync();
                ViewBag.Fases = fases;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar página de CambioJBI");
                return BadRequest(new { success = false, message = "Error al cargar la página" });
            }
        }

        /// <summary>
        /// POST: Valida trabajo y fase antes de cambiar JBI
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ValidarTrabajo(int idTrabajo, int idFase)
        {
            try
            {
                if (idTrabajo <= 0)
                {
                    return Json(new { success = false, message = "ID de Trabajo inválido" });
                }

                if (idFase <= 0)
                {
                    return Json(new { success = false, message = "Debe seleccionar una Fase" });
                }

                // Obtener información del trabajo
                var trabajo = await _service.ObtenerTrabajoAsync(idTrabajo);
                if (trabajo == null)
                {
                    _logger.LogWarning("Intento de validar trabajo inexistente: {IdTrabajo}", idTrabajo);
                    return Json(new { success = false, message = "El ID de Trabajo NO existe" });
                }

                // Validar que la fase existe en presupuestos
                var faseCreadaTask = _service.ValidarFaseCreadaAsync(
                    trabajo.IdPropuesta,
                    trabajo.Alternativa,
                    idFase,
                    trabajo.MetCodigo
                );

                var faseCreada = await faseCreadaTask;
                if (!faseCreada)
                {
                    _logger.LogWarning("Fase {IdFase} no creada en presupuestos para trabajo {IdTrabajo}", idFase, idTrabajo);
                    return Json(new { success = false, message = "La Fase debe estar Creada para poder realizar el cambio de JBI" });
                }

                _logger.LogInformation("Validación exitosa para trabajo {IdTrabajo}, fase {IdFase}", idTrabajo, idFase);
                return Json(new { success = true, message = "Validación exitosa" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar trabajo {IdTrabajo}", idTrabajo);
                return Json(new { success = false, message = "Error al validar la información" });
            }
        }

        /// <summary>
        /// POST: Realiza el cambio de JBI
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Cambiar(CambioJBIDto dto)
        {
            try
            {
                if (dto == null || dto.IdTrabajo <= 0 || dto.IdFase <= 0)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var usuarioId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
                
                _logger.LogInformation("Iniciando cambio de JBI para trabajo {IdTrabajo}, usuario {UserId}", 
                    dto.IdTrabajo, usuarioId);

                var result = await _service.CambiarJBIAsync(dto, usuarioId);
                var (success, message) = result;

                if (success)
                {
                    _logger.LogInformation("Cambio de JBI exitoso para trabajo {IdTrabajo}", dto.IdTrabajo);
                }
                else
                {
                    _logger.LogWarning("Cambio de JBI falló para trabajo {IdTrabajo}: {Message}", dto.IdTrabajo, message);
                }

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al cambiar JBI para trabajo {IdTrabajo}", dto?.IdTrabajo);
                return Json(new { success = false, message = "Error al realizar el cambio. Por favor contacte al administrador" });
            }
        }
    }
}
