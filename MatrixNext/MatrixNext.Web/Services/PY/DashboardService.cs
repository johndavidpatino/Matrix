using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.PY
{
    /// <summary>
    /// Servicio para Dashboard de Proyectos y Trabajos
    /// Implementa métricas operacionales y reportes agregados
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly MatrixDbContext _context;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(MatrixDbContext context, ILogger<DashboardService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResultVM<DashboardResumenDTO>> ObtenerResumenGeneralAsync(int? idUnidad = null)
        {
            try
            {
                var query = _context.Trabajos.AsQueryable();

                if (idUnidad.HasValue)
                    query = query.Where(t => _context.Proyectos
                                                 .Where(p => p.Id == t.IdProyecto && p.IdUnidad == idUnidad)
                                                 .Any());

                var trabajos = await query
                    .Include(t => t.Proyecto)
                    .ToListAsync();

                var resumen = new DashboardResumenDTO
                {
                    TotalProyectos = trabajos
                        .Where(t => t.IdProyecto > 0)
                        .Select(t => t.IdProyecto)
                        .Distinct()
                        .Count(),
                    TotalTrabajos = trabajos.Count,
                    TrabajosActivos = trabajos.Count(t => t.Estado == 1), // Activo
                    TrabajosCerrados = trabajos.Count(t => t.Estado == 3), // Cerrado
                    TrabajosAtrasados = trabajos.Count(t => 
                        t.FechaCierre.HasValue && 
                        t.FechaCierre < DateTime.Now && 
                        t.Estado == 1)
                };

                // Trabajos por estado
                resumen.TrabajosPorEstado = trabajos
                    .GroupBy(t => ObtenerNombreEstado(t.Estado))
                    .ToDictionary(g => g.Key, g => g.Count());

                // Trabajos por unidad
                if (!idUnidad.HasValue)
                {
                    resumen.TrabajosPorUnidad = trabajos
                        .Where(t => t.Proyecto != null && t.Proyecto.IdUnidad > 0)
                        .GroupBy(t => t.Proyecto!.IdUnidad.ToString() ?? "Sin Unidad")
                        .ToDictionary(g => g.Key, g => g.Count());
                }

                return ResultVM<DashboardResumenDTO>.Ok(resumen, "Resumen obtenido exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener resumen general de dashboard");
                return ResultVM<DashboardResumenDTO>.Fail("Error al obtener resumen general");
            }
        }

        public async Task<ResultVM<List<TrabajosPorGerenteDTO>>> ObtenerTrabajosPorGerenteAsync(
            int? idUnidad = null,
            long? idGerenteProyectos = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            try
            {
                var query = _context.Trabajos
                    .Include(t => t.Proyecto)
                    .AsQueryable();

                // Filtro por unidad
                if (idUnidad.HasValue)
                    query = query.Where(t => t.Proyecto != null && t.Proyecto.IdUnidad == idUnidad);

                // Filtro por gerente
                if (idGerenteProyectos.HasValue)
                    query = query.Where(t => t.Proyecto != null && 
                                             t.Proyecto.IdGerenteProyectos == idGerenteProyectos);

                // Filtro por fechas
                if (fechaInicio.HasValue)
                    query = query.Where(t => t.FechaCreacion >= fechaInicio);
                if (fechaFin.HasValue)
                    query = query.Where(t => t.FechaCreacion <= fechaFin);

                var trabajos = await query.ToListAsync();

                // Agrupar por gerente
                var trabajosPorGerente = trabajos
                    .Where(t => t.Proyecto?.IdGerenteProyectos > 0)
                    .GroupBy(t => new
                    {
                        IdGerente = t.Proyecto!.IdGerenteProyectos!.Value,
                        NombreGerente = $"Gerente {t.Proyecto.IdGerenteProyectos?.ToString() ?? "?"}"
                    })
                    .Select(g => new TrabajosPorGerenteDTO
                    {
                        IdGerenteProyectos = g.Key.IdGerente,
                        NombreGerente = g.Key.NombreGerente,
                        TotalTrabajos = g.Count(),
                        TrabajosActivos = g.Count(t => t.Estado == 1),
                        TrabajosCompletados = g.Count(t => t.Estado == 3),
                        TrabajosAtrasados = g.Count(t =>
                            t.FechaCierre.HasValue &&
                            t.FechaCierre < DateTime.Now &&
                            t.Estado == 1)
                    })
                    .OrderByDescending(d => d.TotalTrabajos)
                    .ToList();

                return ResultVM<List<TrabajosPorGerenteDTO>>.Ok(
                    trabajosPorGerente,
                    $"{trabajosPorGerente.Count} gerentes con trabajos asignados");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener trabajos por gerente");
                return ResultVM<List<TrabajosPorGerenteDTO>>.Fail("Error al obtener trabajos por gerente");
            }
        }

        public async Task<ResultVM<List<TrabajosPorEstadoDTO>>> ObtenerTrabajosPorEstadoAsync(
            int? idUnidad = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            try
            {
                var query = _context.Trabajos
                    .Include(t => t.Proyecto)
                    .AsQueryable();

                if (idUnidad.HasValue)
                    query = query.Where(t => t.Proyecto != null && t.Proyecto.IdUnidad == idUnidad);

                if (fechaInicio.HasValue)
                    query = query.Where(t => t.FechaCreacion >= fechaInicio);
                if (fechaFin.HasValue)
                    query = query.Where(t => t.FechaCreacion <= fechaFin);

                var trabajos = await query.ToListAsync();
                var totalTrabajos = trabajos.Count;

                var trabajosPorEstado = trabajos
                    .GroupBy(t => new
                    {
                        IdEstado = t.Estado,
                        NombreEstado = ObtenerNombreEstado(t.Estado)
                    })
                    .Select(g => new TrabajosPorEstadoDTO
                    {
                        IdEstado = g.Key.IdEstado,
                        NombreEstado = g.Key.NombreEstado,
                        CantidadTrabajos = g.Count(),
                        PorcentajeTotal = totalTrabajos > 0 
                            ? Math.Round((decimal)g.Count() / totalTrabajos * 100, 2)
                            : 0
                    })
                    .OrderByDescending(d => d.CantidadTrabajos)
                    .ToList();

                return ResultVM<List<TrabajosPorEstadoDTO>>.Ok(
                    trabajosPorEstado,
                    $"{trabajosPorEstado.Count} estados distintos encontrados");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener trabajos por estado");
                return ResultVM<List<TrabajosPorEstadoDTO>>.Fail("Error al obtener trabajos por estado");
            }
        }

        public async Task<ResultVM<List<TrabajoDetalleDTO>>> ObtenerDetalleTrabajosAsync(
            int? idUnidad = null,
            long? idGerenteProyectos = null,
            int? estado = null,
            string? busqueda = null,
            int page = 1,
            int pageSize = 20)
        {
            try
            {
                var query = _context.Trabajos
                    .Include(t => t.Proyecto)
                    .AsQueryable();

                // Filtros
                if (idUnidad.HasValue)
                    query = query.Where(t => t.Proyecto != null && t.Proyecto.IdUnidad == idUnidad);

                if (idGerenteProyectos.HasValue)
                    query = query.Where(t => t.Proyecto != null && 
                                             t.Proyecto.IdGerenteProyectos == idGerenteProyectos);

                if (estado.HasValue)
                    query = query.Where(t => t.Estado == estado);

                if (!string.IsNullOrWhiteSpace(busqueda))
                {
                    query = query.Where(t =>
                        (t.JobBook != null && t.JobBook.Contains(busqueda)) ||
                        (t.Nombre != null && t.Nombre.Contains(busqueda)) ||
                        (t.Proyecto != null && t.Proyecto.JobBook != null && 
                         t.Proyecto.JobBook.Contains(busqueda)));
                }

                // Paginación
                var totalRecords = await query.CountAsync();
                var trabajos = await query
                    .OrderByDescending(t => t.FechaCreacion)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var detalle = trabajos.Select(t => new TrabajoDetalleDTO
                {
                    Id = t.Id,
                    JobBook = t.JobBook,
                    Nombre = t.Nombre,
                    IdProyecto = t.IdProyecto,
                    NombreProyecto = t.Proyecto?.Nombre,
                    IdGerenteProyectos = t.Proyecto?.IdGerenteProyectos,
                    NombreGerente = null,
                    IdUnidad = (int?)t.Proyecto?.IdUnidad,
                    NombreUnidad = null,
                    Estado = t.Estado,
                    NombreEstado = ObtenerNombreEstado(t.Estado),
                    FechaCreacion = t.FechaCreacion,
                    FechaInicioCampo = null,
                    FechaFinalizacionCampo = t.FechaCierre,
                    Atrasado = t.FechaCierre.HasValue && 
                               t.FechaCierre < DateTime.Now && 
                               t.Estado == 1,
                    DiasAtraso = t.FechaCierre.HasValue && 
                                 t.FechaCierre < DateTime.Now && 
                                 t.Estado == 1
                        ? (int)(DateTime.Now - t.FechaCierre.Value).TotalDays
                        : null
                }).ToList();

                return ResultVM<List<TrabajoDetalleDTO>>.Ok(
                    detalle,
                    $"{detalle.Count} de {totalRecords} trabajos obtenidos (Página {page}/{Math.Ceiling((decimal)totalRecords / pageSize)})");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalle de trabajos");
                return ResultVM<List<TrabajoDetalleDTO>>.Fail("Error al obtener detalle de trabajos");
            }
        }

        #region Métodos auxiliares

        private static string ObtenerNombreEstado(int estado) => estado switch
        {
            0 => "Cerrado",
            1 => "Activo",
            2 => "Suspendido",
            3 => "En Proceso",
            4 => "Completado",
            _ => "Desconocido"
        };

        #endregion
    }
}
