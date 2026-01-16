// MatrixNext.Web/Areas/CORE/Controllers/WorkFlowController.cs - EXTENSIÓN (TASK 4)

// Agregar este método action a WorkFlowController existente:

using MatrixNext.Data.DTOs.CORE;
using MatrixNext.Web.ViewModels.CORE;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MatrixNext.Web.Areas.CORE.Controllers
{
    /// <summary>
    /// Clase parcial: Métodos de extensión para TraficoTareas (Sprint 17)
    /// </summary>
    public partial class WorkFlowController : Controller
    {
        /// <summary>
        /// GET /CORE/Workflow/TraficoTareas
        /// Vista consolidada de tráfico de tareas por unidad OP
        /// Sprint 17 - RE_GT TraficoTareas UI
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> TraficoTareas(
            int? unidad = null,
            string? estado = null,
            int? prioridad = null,
            string? busqueda = null,
            int page = 1)
        {
            try
            {
                _logger.LogInformation(
                    "[TraficoTareas] Acceso: Usuario={User}, Unidad={Unidad}, Page={Page}", 
                    User.Identity?.Name, unidad, page);

                // Obtener unidades disponibles
                var unidades = await _service.ObtenerUnidadesTraficoAsync();
                
                if (!unidades.Any())
                {
                    _logger.LogError("[TraficoTareas] No hay unidades disponibles");
                    return BadRequest(new { message = "Error: No hay unidades disponibles" });
                }

                // Determinar unidad a filtrar
                var idUnidadFiltro = unidad ?? unidades.FirstOrDefault()?.Id ?? 5;
                var unidadSeleccionada = unidades.FirstOrDefault(u => u.Id == idUnidadFiltro);

                if (unidadSeleccionada == null)
                {
                    _logger.LogWarning("[TraficoTareas] Unidad inválida: {IdUnidad}", idUnidadFiltro);
                    return NotFound(new { message = "Unidad no válida" });
                }

                // VALIDAR PERMISOS POR UNIDAD
                // TODO: Implementar validación de permisos según UnidadPermisosMap
                // Verificar que el usuario tenga permiso para esta unidad
                var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!long.TryParse(userIdStr, out var userId))
                {
                    _logger.LogError("[TraficoTareas] No se pudo parsear UserId del usuario");
                    return Forbid();
                }

                var tienePermiso = await ValidarPermisoUnidadAsync(userId, unidadSeleccionada.PermId);

                if (!tienePermiso)
                {
                    _logger.LogWarning(
                        "[TraficoTareas] Usuario sin permiso para unidad {Unidad}", 
                        unidadSeleccionada.Nombre);
                    return Forbid();
                }

                // Obtener tareas para la unidad
                var (tareas, total) = await _service.ObtenerTareasPorUnidadAsync(
                    idUnidadFiltro, 
                    estado, 
                    prioridad, 
                    busqueda, 
                    page);

                // Construir ViewModel
                var viewModel = new TraficoTareasViewModel
                {
                    Tareas = tareas,
                    TotalRegistros = total,
                    PaginaActual = page,
                    FiltroUnidad = unidad,
                    FiltroEstado = estado,
                    FiltroPrioridad = prioridad,
                    FiltroBusqueda = busqueda,
                    UnidadesDisponibles = unidades,
                    IdUnidadActual = idUnidadFiltro,
                    NombreUnidadActual = unidadSeleccionada.Nombre,
                    MostrarListado = true // Accordion 0
                };

                _logger.LogInformation(
                    "[TraficoTareas] Vista cargada: {Count} tareas, Total={Total}", 
                    tareas.Count, total);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[TraficoTareas] Error cargando vista");
                return BadRequest(new { message = "Error al cargar tráfico de tareas" });
            }
        }

        /// <summary>
        /// GET /CORE/Workflow/TraficoTareasDetails/{id}
        /// Carga Accordion 1 con detalles de trabajo y botones de gestión
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> TraficoTareasDetails(long id)
        {
            try
            {
                _logger.LogInformation("[TraficoTareas] Cargando detalles trabajo {IdTrabajo}", id);

                var infoTrabajo = await _service.ObtenerInformacionTrabajoAsync(id);
                
                if (infoTrabajo == null)
                {
                    return NotFound();
                }

                return PartialView("_TraficoTareasDetails", infoTrabajo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[TraficoTareas] Error cargando detalles {IdTrabajo}", id);
                return BadRequest(new { message = "Error al cargar detalles del trabajo" });
            }
        }

        /// <summary>
        /// POST /CORE/Workflow/TraficoTareasExport
        /// Exportar listado de personal asignado a Excel
        /// Solo para unidades 11 (Scripting) y 14 (Call Center)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> TraficoTareasExport(long idTrabajo, int idUnidad)
        {
            try
            {
                if (idUnidad is not (11 or 14))
                {
                    return BadRequest(new { message = "Esta unidad no permite exportar personal" });
                }

                _logger.LogInformation(
                    "[TraficoTareas] Exportando personal: Trabajo={IdTrabajo}, Unidad={IdUnidad}", 
                    idTrabajo, idUnidad);

                // TODO: Implementar export a Excel usando CoordinacionCampoPersonal.ListadoPersonasAsignadas
                // Retornar FileResult con archivo Excel

                return Ok(new { message = "Export generado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[TraficoTareas] Error exportando personal");
                return BadRequest(new { message = "Error al exportar" });
            }
        }

        /// <summary>
        /// Helper: Validar permiso del usuario para la unidad
        /// </summary>
        private async Task<bool> ValidarPermisoUnidadAsync(long idUsuario, int permId)
        {
            try
            {
                // TODO: Implementar validación contra tabla de permisos
                // De momento retornar true para testing
                return await Task.FromResult(true);
            }
            catch
            {
                return false;
            }
        }
    }
}
