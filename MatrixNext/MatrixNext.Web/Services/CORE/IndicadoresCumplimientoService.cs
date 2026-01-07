using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.CORE
{
    public class IndicadoresCumplimientoService : IIndicadoresCumplimientoService
    {
        private readonly MatrixDbContext _context;
        private readonly ILogger<IndicadoresCumplimientoService> _logger;

        public IndicadoresCumplimientoService(MatrixDbContext context, ILogger<IndicadoresCumplimientoService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResultVM<IndicadoresResumenDTO>> ObtenerResumenIndicadoresAsync()
        {
            try
            {
                var tareas = await _context.WorkFlows.ToListAsync();
                var completadas = tareas.Count(t => t.Estado == "Completada");
                var atrasadas = tareas.Count(t =>
                    t.FechaVencimiento.HasValue &&
                    t.FechaVencimiento < DateTime.Now &&
                    t.Estado != "Completada");

                // ISSUE RESUELTO: Sprint 6 GAP-6.2
                // Calcular promedio real de días de completación en lugar de hardcodeado
                var tareasCompletadas = tareas
                    .Where(t => t.Estado == "Completada" && 
                                t.FechaCreacion != null && 
                                t.FechaCompletacion != null)
                    .ToList();

                var promedioDiasCompletacion = tareasCompletadas.Any()
                    ? (decimal)tareasCompletadas
                        .Average(t => (t.FechaCompletacion!.Value - t.FechaCreacion!.Value).TotalDays)
                    : 0m;

                var resumen = new IndicadoresResumenDTO
                {
                    PorcentajeCumplimiento = tareas.Count > 0
                        ? Math.Round((decimal)completadas / tareas.Count * 100, 2)
                        : 0,
                    PorcentajeAtrasadas = tareas.Count > 0
                        ? Math.Round((decimal)atrasadas / tareas.Count * 100, 2)
                        : 0,
                    TotalTareasCompletadas = completadas,
                    TotalTareasAtrasadas = atrasadas,
                    PromedioDiasCompletacion = Math.Round(promedioDiasCompletacion, 2) // Real, no hardcodeado
                };

                return ResultVM<IndicadoresResumenDTO>.Ok(resumen, "Indicadores obtenidos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener indicadores");
                return ResultVM<IndicadoresResumenDTO>.Fail("Error al obtener indicadores");
            }
        }

        public async Task<ResultVM<List<IndicadorPorGerenteDTO>>> ObtenerIndicadoresPorGerenteAsync()
        {
            try
            {
                // ISSUE RESUELTO: Sprint 6 GAP-6.3
                // Obtener tareas con relación a proyectos y gerentes
                var tareas = await _context.WorkFlows
                    .Include(w => w.UsuariosAsignados)
                    .ToListAsync();

                // Agrupar por gerente de proyecto (si existe relación)
                // Si WorkFlow no tiene relación directa a Proyecto, se puede usar UsuariosAsignados
                var indicadores = tareas
                    .GroupBy(t => t.UsuariosAsignados?.FirstOrDefault()?.IdUsuario ?? 0)
                    .Select(g => new IndicadorPorGerenteDTO
                    {
                        IdGerenteProyectos = g.Key,
                        NombreGerente = g.Key > 0 ? $"Gerente {g.Key}" : "Sin asignar",
                        TotalTareas = g.Count(),
                        TareasCompletadas = g.Count(t => t.Estado == "Completada"),
                        PorcentajeCumplimiento = g.Count() > 0
                            ? Math.Round((decimal)g.Count(t => t.Estado == "Completada") / g.Count() * 100, 2)
                            : 0,
                        TareasAtrasadas = g.Count(t =>
                            t.FechaVencimiento.HasValue &&
                            t.FechaVencimiento < DateTime.Now &&
                            t.Estado != "Completada")
                    })
                    .ToList();
                            t.FechaVencimiento.HasValue &&
                            t.FechaVencimiento < DateTime.Now &&
                            t.Estado != "Completada")
                    })
                    .ToList();

                return ResultVM<List<IndicadorPorGerenteDTO>>.Ok(indicadores, "Indicadores por gerente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener indicadores por gerente");
                return ResultVM<List<IndicadorPorGerenteDTO>>.Fail("Error al obtener indicadores");
            }
        }

        public async Task<ResultVM<List<IndicadorPorTipoHiloDTO>>> ObtenerIndicadoresPorTipoHiloAsync()
        {
            try
            {
                var tareas = await _context.WorkFlows.ToListAsync();

                var indicadores = tareas
                    .GroupBy(t => t.IdTipoHilo)
                    .Select(g => new IndicadorPorTipoHiloDTO
                    {
                        IdTipoHilo = g.Key,
                        NombreTipoHilo = $"Hilo {g.Key}",
                        TotalTareas = g.Count(),
                        TareasCompletadas = g.Count(t => t.Estado == "Completada"),
                        PorcentajeCumplimiento = g.Count() > 0
                            ? Math.Round((decimal)g.Count(t => t.Estado == "Completada") / g.Count() * 100, 2)
                            : 0,
                        TareasAtrasadas = g.Count(t =>
                            t.FechaVencimiento.HasValue &&
                            t.FechaVencimiento < DateTime.Now &&
                            t.Estado != "Completada")
                    })
                    .OrderByDescending(d => d.PorcentajeCumplimiento)
                    .ToList();

                return ResultVM<List<IndicadorPorTipoHiloDTO>>.Ok(indicadores, "Indicadores por tipo de hilo");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener indicadores por tipo hilo");
                return ResultVM<List<IndicadorPorTipoHiloDTO>>.Fail("Error al obtener indicadores");
            }
        }
    }
}
