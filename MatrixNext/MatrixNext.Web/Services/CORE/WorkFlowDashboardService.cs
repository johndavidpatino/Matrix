using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.CORE
{
    /// <summary>
    /// Servicio para Dashboard de WorkFlow (Tráfico de Tareas)
    /// Implementa métricas de carga de tareas, estados y seguimiento de SLA
    /// </summary>
    public class WorkFlowDashboardService : IWorkFlowDashboardService
    {
        private readonly MatrixDbContext _context;
        private readonly ILogger<WorkFlowDashboardService> _logger;

        public WorkFlowDashboardService(MatrixDbContext context, ILogger<WorkFlowDashboardService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResultVM<WorkFlowResumenDTO>> ObtenerResumenGeneralAsync()
        {
            try
            {
                var tareas = await _context.WorkFlows
                    .Include(w => w.UsuariosAsignados)
                    .ToListAsync();

                var hoy = DateTime.Now;
                var resumen = new WorkFlowResumenDTO
                {
                    TotalTareas = tareas.Count,
                    TareasActivas = tareas.Count(t => t.Estado == "EnProgreso"),
                    TareasCompletadas = tareas.Count(t => t.Estado == "Completada"),
                    TareasAnuladas = tareas.Count(t => t.Estado == "Anulada"),
                    TareasAtrasadas = tareas.Count(t =>
                        t.FechaVencimiento.HasValue &&
                        t.FechaVencimiento < hoy &&
                        t.Estado != "Completada"),
                    TareasProximasAvencer = tareas.Count(t =>
                        t.FechaVencimiento.HasValue &&
                        t.FechaVencimiento >= hoy &&
                        t.FechaVencimiento <= hoy.AddDays(3) &&
                        t.Estado != "Completada")
                };

                // Tareas por estado
                resumen.TareasPorEstado = tareas
                    .GroupBy(t => t.Estado ?? "Desconocido")
                    .ToDictionary(g => g.Key, g => g.Count());

                // Tareas por prioridad
                resumen.TareasPorPrioridad = tareas
                    .GroupBy(t => t.Prioridad)
                    .ToDictionary(g => g.Key, g => g.Count());

                return ResultVM<WorkFlowResumenDTO>.Ok(resumen, "Resumen de WorkFlow obtenido exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener resumen general de WorkFlow");
                return ResultVM<WorkFlowResumenDTO>.Fail("Error al obtener resumen general");
            }
        }

        public async Task<ResultVM<List<TareasPorEstadoDTO>>> ObtenerTareasPorEstadoAsync(
            int? idTipoHilo = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            try
            {
                var query = _context.WorkFlows.AsQueryable();

                if (idTipoHilo.HasValue)
                    query = query.Where(t => t.IdTipoHilo == idTipoHilo);

                if (fechaInicio.HasValue)
                    query = query.Where(t => t.FechaCreacion >= fechaInicio);
                if (fechaFin.HasValue)
                    query = query.Where(t => t.FechaCreacion <= fechaFin);

                var tareas = await query.ToListAsync();
                var totalTareas = tareas.Count;
                var hoy = DateTime.Now;

                var tareasPorEstado = tareas
                    .GroupBy(t => t.Estado ?? "Desconocido")
                    .Select(g => new TareasPorEstadoDTO
                    {
                        Estado = g.Key,
                        CantidadTareas = g.Count(),
                        PorcentajeTotal = totalTareas > 0
                            ? Math.Round((decimal)g.Count() / totalTareas * 100, 2)
                            : 0,
                        TareasAtrasadas = g.Count(t =>
                            t.FechaVencimiento.HasValue &&
                            t.FechaVencimiento < hoy &&
                            t.Estado != "Completada"),
                        TareasProximasAvencer = g.Count(t =>
                            t.FechaVencimiento.HasValue &&
                            t.FechaVencimiento >= hoy &&
                            t.FechaVencimiento <= hoy.AddDays(3) &&
                            t.Estado != "Completada")
                    })
                    .OrderByDescending(d => d.CantidadTareas)
                    .ToList();

                return ResultVM<List<TareasPorEstadoDTO>>.Ok(
                    tareasPorEstado,
                    $"{tareasPorEstado.Count} estados distintos encontrados");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tareas por estado");
                return ResultVM<List<TareasPorEstadoDTO>>.Fail("Error al obtener tareas por estado");
            }
        }

        public async Task<ResultVM<List<TareasPorPrioridadDTO>>> ObtenerTareasPorPrioridadAsync(
            int? idTipoHilo = null)
        {
            try
            {
                var query = _context.WorkFlows.AsQueryable();

                if (idTipoHilo.HasValue)
                    query = query.Where(t => t.IdTipoHilo == idTipoHilo);

                var tareas = await query.ToListAsync();
                var hoy = DateTime.Now;

                var tareasPorPrioridad = tareas
                    .GroupBy(t => t.Prioridad)
                    .Select(g => new TareasPorPrioridadDTO
                    {
                        Prioridad = g.Key,
                        NombrePrioridad = ObtenerNombrePrioridad(g.Key),
                        CantidadTareas = g.Count(),
                        TareasAtrasadas = g.Count(t =>
                            t.FechaVencimiento.HasValue &&
                            t.FechaVencimiento < hoy &&
                            t.Estado != "Completada")
                    })
                    .OrderByDescending(d => d.Prioridad)
                    .ToList();

                return ResultVM<List<TareasPorPrioridadDTO>>.Ok(
                    tareasPorPrioridad,
                    $"{tareasPorPrioridad.Count} prioridades encontradas");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tareas por prioridad");
                return ResultVM<List<TareasPorPrioridadDTO>>.Fail("Error al obtener tareas por prioridad");
            }
        }

        public async Task<ResultVM<List<TareaProximaAVencerDTO>>> ObtenerTareasProximasAVencerAsync(int diasAlerta = 3)
        {
            try
            {
                var hoy = DateTime.Now;
                var fechaAlerta = hoy.AddDays(diasAlerta);

                var tareas = await _context.WorkFlows
                    .Where(t =>
                        t.FechaVencimiento.HasValue &&
                        t.FechaVencimiento >= hoy &&
                        t.FechaVencimiento <= fechaAlerta &&
                        t.Estado != "Completada")
                    .Include(w => w.UsuariosAsignados)
                    .ToListAsync();

                var proximasVencer = tareas
                    .Select(t => new TareaProximaAVencerDTO
                    {
                        Id = t.Id,
                        TareaNombre = $"Tarea {t.IdTarea}",
                        IdTrabajo = t.IdTrabajo,
                        TrabajoNombre = $"Trabajo {t.IdTrabajo}",
                        Estado = t.Estado,
                        Prioridad = t.Prioridad,
                        FechaVencimiento = t.FechaVencimiento,
                        DiasHastaVencer = t.FechaVencimiento.HasValue
                            ? (int)(t.FechaVencimiento.Value - hoy).TotalDays
                            : null,
                        UrgenteCritica = t.Prioridad == 2 || (t.FechaVencimiento.HasValue && t.FechaVencimiento <= hoy.AddDays(1)),
                        UsuariosAsignados = t.UsuariosAsignados.Count
                    })
                    .OrderBy(t => t.FechaVencimiento)
                    .ToList();

                return ResultVM<List<TareaProximaAVencerDTO>>.Ok(
                    proximasVencer,
                    $"{proximasVencer.Count} tareas próximas a vencer en los próximos {diasAlerta} días");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tareas próximas a vencer");
                return ResultVM<List<TareaProximaAVencerDTO>>.Fail("Error al obtener tareas próximas a vencer");
            }
        }

        public async Task<ResultVM<List<TareaDetalleDTO>>> ObtenerDetalleTareasAsync(
            int? idTipoHilo = null,
            string? estado = null,
            int? prioridad = null,
            string? busqueda = null,
            int page = 1,
            int pageSize = 20)
        {
            try
            {
                var query = _context.WorkFlows
                    .Include(w => w.UsuariosAsignados)
                    .AsQueryable();

                if (idTipoHilo.HasValue)
                    query = query.Where(t => t.IdTipoHilo == idTipoHilo);

                if (!string.IsNullOrWhiteSpace(estado))
                    query = query.Where(t => t.Estado == estado);

                if (prioridad.HasValue)
                    query = query.Where(t => t.Prioridad == prioridad);

                if (!string.IsNullOrWhiteSpace(busqueda))
                {
                    var searchLower = busqueda.ToLower();
                    query = query.Where(t =>
                        t.Observaciones != null && t.Observaciones.ToLower().Contains(searchLower) ||
                        t.IdTrabajo.ToString().Contains(searchLower) ||
                        t.IdTarea.ToString().Contains(searchLower));
                }

                var totalRecords = await query.CountAsync();
                var tareas = await query
                    .OrderByDescending(t => t.FechaCreacion)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var hoy = DateTime.Now;
                var detalle = tareas.Select(t => new TareaDetalleDTO
                {
                    Id = t.Id,
                    TareaNombre = $"Tarea {t.IdTarea}",
                    IdTrabajo = t.IdTrabajo,
                    TrabajoNombre = $"Trabajo {t.IdTrabajo}",
                    IdTarea = t.IdTarea,
                    IdTipoHilo = t.IdTipoHilo,
                    Estado = t.Estado,
                    Prioridad = t.Prioridad,
                    NombrePrioridad = ObtenerNombrePrioridad(t.Prioridad),
                    FechaVencimiento = t.FechaVencimiento,
                    Atrasada = t.FechaVencimiento.HasValue && t.FechaVencimiento < hoy && t.Estado != "Completada",
                    DiasAtraso = t.FechaVencimiento.HasValue && t.FechaVencimiento < hoy && t.Estado != "Completada"
                        ? (int)(hoy - t.FechaVencimiento.Value).TotalDays
                        : null,
                    UsuariosAsignados = t.UsuariosAsignados.Count,
                    Observaciones = t.Observaciones,
                    FechaCreacion = t.FechaCreacion
                }).ToList();

                return ResultVM<List<TareaDetalleDTO>>.Ok(
                    detalle,
                    $"{detalle.Count} de {totalRecords} tareas obtenidas (Página {page}/{Math.Ceiling((decimal)totalRecords / pageSize)})");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalle de tareas");
                return ResultVM<List<TareaDetalleDTO>>.Fail("Error al obtener detalle de tareas");
            }
        }

        public async Task<ResultVM<List<TareasPorUsuarioDTO>>> ObtenerTareasPorUsuarioAsync()
        {
            try
            {
                var tareas = await _context.WorkFlows
                    .Include(w => w.UsuariosAsignados)
                    .ToListAsync();

                var hoy = DateTime.Now;

                var tareasPorUsuario = tareas
                    .SelectMany(t => t.UsuariosAsignados.Select(u => new { t, u }))
                    .GroupBy(x => x.u.IdUsuario)
                    .Select(g => new TareasPorUsuarioDTO
                    {
                        IdUsuario = g.Key,
                        NombreUsuario = $"Usuario {g.Key}",
                        TotalAsignaciones = g.Count(),
                        TareasActivas = g.Count(x => x.t.Estado == "EnProgreso"),
                        TareasCompletadas = g.Count(x => x.t.Estado == "Completada"),
                        TareasAtrasadas = g.Count(x =>
                            x.t.FechaVencimiento.HasValue &&
                            x.t.FechaVencimiento < hoy &&
                            x.t.Estado != "Completada"),
                        TareasAlta = g.Count(x => x.t.Prioridad == 2)
                    })
                    .OrderByDescending(d => d.TotalAsignaciones)
                    .ToList();

                return ResultVM<List<TareasPorUsuarioDTO>>.Ok(
                    tareasPorUsuario,
                    $"{tareasPorUsuario.Count} usuarios con tareas asignadas");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tareas por usuario");
                return ResultVM<List<TareasPorUsuarioDTO>>.Fail("Error al obtener tareas por usuario");
            }
        }

        #region Métodos auxiliares

        private static string ObtenerNombrePrioridad(int prioridad) => prioridad switch
        {
            1 => "Normal",
            2 => "Alta",
            3 => "Baja",
            _ => "Desconocida"
        };

        #endregion
    }
}
