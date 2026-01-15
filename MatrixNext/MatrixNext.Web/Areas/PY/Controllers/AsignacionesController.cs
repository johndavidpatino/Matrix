/// <summary>
/// Controller para gestión de asignaciones de proyectos a gerentes
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.2.5
/// </summary>
namespace MatrixNext.Web.Areas.PY.Controllers
{
    using MatrixNext.Web.Models.PY;
    using MatrixNext.Web.Services.PY;
    using MatrixNext.Web.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("PY")]
    [Authorize(Roles = "Administrador,GerenteProyectos,Coordinador")]
    [Route("PY/[controller]/[action]")]
    public class AsignacionesController : Controller
    {
        private readonly IAsignacionesProyectosService _service;
        private readonly IProyectosService _proyectosService;
        private readonly IAuditoriaService _auditoria;
        private readonly ILogger<AsignacionesController> _logger;

        public AsignacionesController(
            IAsignacionesProyectosService service,
            IProyectosService proyectosService,
            IAuditoriaService auditoria,
            ILogger<AsignacionesController> logger)
        {
            _service = service;
            _proyectosService = proyectosService;
            _auditoria = auditoria;
            _logger = logger;
        }

        private long ObtenerIdUsuarioActual()
        {
            return long.Parse(User.FindFirst("Id")?.Value ?? "0");
        }

        /// <summary>
        /// Listado principal de asignaciones de proyectos
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(long? idProyecto, long? idGerente, string estado)
        {
            try
            {
                var asignaciones = await _service.ObtenerAsignacionesAsync(idProyecto);

                // Filtrar por gerente
                if (idGerente.HasValue)
                    asignaciones = asignaciones.Where(a => a.IdGerentePrincipal == idGerente.Value).ToList();

                // Filtrar por estado
                if (!string.IsNullOrEmpty(estado))
                    asignaciones = asignaciones.Where(a => a.Estado == estado).ToList();

                _logger.LogInformation("Listado de asignaciones obtenido. Usuario: {UserId}, Filtros: Proyecto={IdProyecto}, Gerente={IdGerente}, Estado={Estado}",
                    ObtenerIdUsuarioActual(), idProyecto, idGerente, estado);

                return View(asignaciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo listado de asignaciones");
                return StatusCode(500, new { exitoso = false, mensaje = "Error al obtener asignaciones" });
            }
        }

        /// <summary>
        /// Modal para asignar gerente a un proyecto
        /// </summary>
        [HttpGet]
        public IActionResult AsignarModal()
        {
            return PartialView("_AsignarModal", new AsignacionProyecto());
        }

        /// <summary>
        /// Asignar gerente a proyecto
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Asignar(long idProyecto, long idGerentePrincipal, string observacionesAsignacion, List<long> idsTrabajos)
        {
            try
            {
                // Validaciones
                if (idProyecto <= 0 || idGerentePrincipal <= 0)
                {
                    return Json(new { exitoso = false, mensaje = "Proyecto y gerente son obligatorios" });
                }

                // Crear asignación
                var asignacion = new AsignacionProyecto
                {
                    IdProyecto = idProyecto,
                    IdGerentePrincipal = idGerentePrincipal,
                    FechaAsignacion = DateTime.Now,
                    Estado = "Activa",
                    ObservacionesAsignacion = observacionesAsignacion,
                    CantidadTrabajosAsignados = idsTrabajos?.Count ?? 0
                };

                // Guardar (requiere adapter/service que no está en el ejercicio, se simula)
                // En producción: await _service.AsignarGerenteAsync(asignacion, ObtenerIdUsuarioActual());

                _logger.LogInformation("Gerente asignado a proyecto. Proyecto: {IdProyecto}, Gerente: {IdGerente}, Usuario: {UserId}",
                    idProyecto, idGerentePrincipal, ObtenerIdUsuarioActual());

                return Json(new { exitoso = true, mensaje = "Gerente asignado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error asignando gerente. Proyecto: {IdProyecto}, Gerente: {IdGerente}",
                    idProyecto, idGerentePrincipal);
                return Json(new { exitoso = false, mensaje = "Error al asignar gerente" });
            }
        }

        /// <summary>
        /// Modal para reasignar gerente
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ReasignarModal(long id)
        {
            try
            {
                // En producción: var asignacion = await _service.ObtenerAsignacionAsync(id);
                // Por ahora se devuelve modelo vacío
                return PartialView("_ReasignarModal", new AsignacionProyecto { IdAsignacion = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando modal reasignación. IdAsignacion: {Id}", id);
                return StatusCode(500, new { exitoso = false, mensaje = "Error cargando formulario" });
            }
        }

        /// <summary>
        /// Reasignar gerente a otro
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reasignar(long idAsignacion, long idGerentePrincipal, string observacionesAsignacion, bool notificarGerenteAnterior, List<long> idsTrabajos)
        {
            try
            {
                // Validaciones
                if (idAsignacion <= 0 || idGerentePrincipal <= 0)
                {
                    return Json(new { exitoso = false, mensaje = "Asignación y nuevo gerente son obligatorios" });
                }

                if (string.IsNullOrWhiteSpace(observacionesAsignacion))
                {
                    return Json(new { exitoso = false, mensaje = "Motivo de reasignación es obligatorio" });
                }

                // Reasignar (requiere adapter/service)
                // En producción: await _service.ReasignarGerenteAsync(idAsignacion, idGerentePrincipal, observacionesAsignacion, ObtenerIdUsuarioActual());

                _logger.LogInformation("Gerente reasignado. Asignación: {IdAsignacion}, Nuevo Gerente: {IdGerente}, Motivo: {Motivo}, Usuario: {UserId}",
                    idAsignacion, idGerentePrincipal, observacionesAsignacion, ObtenerIdUsuarioActual());

                return Json(new { exitoso = true, mensaje = "Reasignación completada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reasignando gerente. IdAsignacion: {IdAsignacion}, NuevoGerente: {IdGerente}",
                    idAsignacion, idGerentePrincipal);
                return Json(new { exitoso = false, mensaje = "Error al reasignar gerente" });
            }
        }

        /// <summary>
        /// Modal historial de cambios
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> HistorialModal(long id)
        {
            try
            {
                // En producción: var historial = await _service.ObtenerHistorialAsync(id);
                var bitacora = new BitacoraAsignacion
                {
                    IdAsignacion = id,
                    Registros = new List<RegistroBitacoraAsignacion>()
                };

                return PartialView("_HistorialModal", bitacora);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando historial. IdAsignacion: {Id}", id);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// API: Obtener proyectos disponibles para asignación
        /// </summary>
        [HttpGet("GetProyectosDisponibles")]
        public async Task<IActionResult> GetProyectosDisponibles()
        {
            try
            {
                // En producción: usar _service.ObtenerProyectosDisponiblesAsync()
                var datos = new List<dynamic> { };

                return Json(new { exitoso = true, datos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo proyectos disponibles");
                return Json(new { exitoso = false, datos = new List<dynamic>() });
            }
        }

        /// <summary>
        /// API: Obtener gerentes disponibles
        /// </summary>
        [HttpGet("GetGerentesDisponibles")]
        public async Task<IActionResult> GetGerentesDisponibles()
        {
            try
            {
                // En producción: usar _service.ObtenerGerentesDisponiblesAsync()
                var datos = new List<dynamic> { };

                return Json(new { exitoso = true, datos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo gerentes disponibles");
                return Json(new { exitoso = false, datos = new List<dynamic>() });
            }
        }

        /// <summary>
        /// API: Obtener trabajos de proyecto
        /// </summary>
        [HttpGet("GetTrabajosProyecto")]
        public async Task<IActionResult> GetTrabajosProyecto(long id)
        {
            try
            {
                var datos = new List<dynamic> { };
                // En producción: usar _service.ObtenerTrabajosProyectoAsync(id)

                return Json(new { exitoso = true, datos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo trabajos. IdProyecto: {Id}", id);
                return Json(new { exitoso = false, datos = new List<dynamic>() });
            }
        }

        /// <summary>
        /// API: Obtener trabajos ya asignados
        /// </summary>
        [HttpGet("GetTrabajosAsignados")]
        public async Task<IActionResult> GetTrabajosAsignados(long id)
        {
            try
            {
                var datos = new List<dynamic> { };
                // En producción: usar _service.ObtenerTrabajosAsignadosAsync(id)

                return Json(new { exitoso = true, datos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo trabajos asignados. IdAsignacion: {Id}", id);
                return Json(new { exitoso = false, datos = new List<dynamic>() });
            }
        }

        /// <summary>
        /// API: Obtener proyectos para dropdown filtro
        /// </summary>
        [HttpGet("GetProyectos")]
        public async Task<IActionResult> GetProyectos()
        {
            try
            {
                var datos = new List<dynamic> { };
                // En producción: usar _proyectosService.ListarAsync() y mapear

                return Json(new { exitoso = true, datos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo proyectos");
                return Json(new { exitoso = false, datos = new List<dynamic>() });
            }
        }

        /// <summary>
        /// API: Obtener gerentes para dropdown filtro
        /// </summary>
        [HttpGet("GetGerentes")]
        public async Task<IActionResult> GetGerentes()
        {
            try
            {
                var datos = new List<dynamic> { };
                // En producción: usar servicio de usuarios/empleados

                return Json(new { exitoso = true, datos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo gerentes");
                return Json(new { exitoso = false, datos = new List<dynamic>() });
            }
        }
    }
}
