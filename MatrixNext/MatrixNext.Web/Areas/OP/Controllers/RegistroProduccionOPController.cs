using MatrixNext.Web.Services.OP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MatrixNext.Web.Models.OP.Dtos;

namespace MatrixNext.Web.Areas.OP.Controllers
{
    /// <summary>
    /// Controlador para el registro de actividades de producción en OP.
    /// Permite registrar cantidades, tiempos y detalles de actividades ejecutadas.
    /// Incluye validaciones, cascading dropdowns y búsqueda de JobBooks.
    /// </summary>
    [Area("OP")]
    [Authorize]
    [Route("OP/[controller]")]
    public class RegistroProduccionOPController : Controller
    {
        private readonly IOpRegistroProduccionService _registroService;
        private readonly ILogger<RegistroProduccionOPController> _logger;

        public RegistroProduccionOPController(
            IOpRegistroProduccionService registroService,
            ILogger<RegistroProduccionOPController> logger)
        {
            _registroService = registroService;
            _logger = logger;
        }

        /// <summary>
        /// Muestra el formulario de registro de producción.
        /// </summary>
        /// <returns>Vista con formulario en blanco</returns>
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var modelo = new RegistroProduccionDto
                {
                    UsuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0"),
                    Fecha = DateTime.Now.ToString("yyyy-MM-dd")
                };

                // Cargar unidades iniciales
                var unidades = await _registroService.ObtenerUnidadesAsync();
                ViewBag.Unidades = unidades;

                _logger.LogInformation("Usuario {User} accedió a registro de producción", 
                    User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                return View("Index", modelo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando formulario de registro de producción");
                TempData["Error"] = "Error al cargar el formulario";
                return RedirectToAction("Index", "Trabajos");
            }
        }

        /// <summary>
        /// API para obtener actividades según unidad seleccionada (cascada).
        /// </summary>
        /// <param name="unidadId">ID de la unidad</param>
        /// <returns>JSON con lista de actividades</returns>
        [HttpGet("ObtenerActividades")]
        public async Task<IActionResult> ObtenerActividades(int unidadId = 0)
        {
            try
            {
                // Si no se proporciona unidadId, retornar unidades
                if (unidadId <= 0)
                {
                    var unidades = await _registroService.ObtenerUnidadesAsync();
                    return Json(unidades);
                }

                // Si se proporciona unidadId, retornar actividades
                var actividades = await _registroService.ObtenerActividadesAsync(unidadId);
                return Json(actividades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ObtenerActividades: unidadId={UnidadId}", unidadId);
                return Json(new List<CatalogoItemDto>());
            }
        }

        /// <summary>
        /// API para obtener subactividades según actividad seleccionada (cascada).
        /// </summary>
        /// <param name="actividadId">ID de la actividad</param>
        /// <returns>JSON con lista de subactividades</returns>
        [HttpGet("ObtenerSubactividades")]
        public async Task<IActionResult> ObtenerSubactividades(int actividadId)
        {
            try
            {
                if (actividadId <= 0)
                    return Json(new List<CatalogoItemDto>());

                var subactividades = await _registroService.ObtenerSubactividadesAsync(actividadId);
                return Json(subactividades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo subactividades para actividad {ActividadId}", actividadId);
                return Json(new List<CatalogoItemDto>());
            }
        }

        /// <summary>
        /// API para buscar JobBooks (JBE/JBI/CC).
        /// </summary>
        /// <param name="criterio">Criterio de búsqueda (código, nombre)</param>
        /// <param name="tipo">Tipo de JobBook: JBE, JBI, CC</param>
        /// <returns>JSON con resultados de búsqueda</returns>
        [HttpGet("BuscarJobBooks")]
        public async Task<IActionResult> BuscarJobBooks(string criterio, string tipo = "JBE")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(criterio))
                    return Json(new List<JobBookDto>());

                var jobBooks = await _registroService.BuscarJobBooksAsync(criterio.Trim(), tipo);
                return Json(jobBooks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error buscando JobBooks con criterio {Criterio}", criterio);
                return Json(new List<JobBookDto>());
            }
        }

        /// <summary>
        /// Guarda un registro de actividad de producción.
        /// </summary>
        /// <param name="registro">Datos del registro a guardar</param>
        /// <returns>JSON resultado de la operación</returns>
        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] RegistroProduccionDto registro)
        {
            try
            {
                if (registro == null)
                    return Json(new { success = false, message = "Datos del registro inválidos" });

                // Validar
                var (valido, mensaje) = await _registroService.ValidarRegistroAsync(registro);
                if (!valido)
                {
                    _logger.LogWarning("Validación fallida en registro: {Mensaje}", mensaje);
                    return Json(new { success = false, message = mensaje });
                }

                // Registrar usuario desde claim
                registro.UsuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                // Guardar
                var idRegistro = await _registroService.RegistrarActividadAsync(registro);

                _logger.LogInformation("Actividad registrada exitosamente: ID={IdRegistro}, Usuario={Usuario}", 
                    idRegistro, registro.UsuarioId);

                return Json(new 
                { 
                    success = true, 
                    message = "Actividad registrada exitosamente",
                    id = idRegistro
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando registro de producción");
                return Json(new { success = false, message = "Error al guardar el registro: " + ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un resumen de registros del usuario actual.
        /// </summary>
        /// <returns>JSON con resumen de registros</returns>
        [HttpGet("MisRegistros")]
        public async Task<IActionResult> MisRegistros()
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                // TODO: Implementar obtención de registros del usuario desde BD
                // SELECT * FROM OP_Produccion WHERE PersonaId=@UsuarioId ORDER BY FechaRegistro DESC
                // Por ahora retornar lista vacía
                var registros = new List<RegistroProduccionDto>();

                _logger.LogInformation("Usuario {UsuarioId} consultó sus registros", usuarioId);
                return Json(registros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo mis registros");
                return Json(new List<RegistroProduccionDto>());
            }
        }
    }
}
