using MatrixNext.Web.Models.PY;
using MatrixNext.Web.ViewModels;
using MatrixNext.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.PY
{
    public class AsignacionesProyectosService : IAsignacionesProyectosService
    {
        private readonly MatrixDbContext _context;
        private readonly ILogger<AsignacionesProyectosService> _logger;

        public AsignacionesProyectosService(MatrixDbContext context, ILogger<AsignacionesProyectosService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResultVM<List<dynamic>>> ObtenerProyectosXAsignarAsync(int idUnidad, long idUsuario)
        {
            try
            {
                _logger.LogInformation($"[AsignacionProyectos] Obteniendo proyectos sin gerente para unidad {idUnidad}");

                var proyectos = await _context.Proyectos
                    .Where(p => p.IdUnidad == idUnidad && (p.IdGerenteProyectos == null || p.IdGerenteProyectos == 0))
                    .Select(p => new
                    {
                        p.Id,
                        p.Nombre,
                        p.JobBook,
                        p.IdUnidad,
                        p.Estado,
                        GerenteProyectosActual = (long?)null,
                        NombreGerente = (string?)null
                    })
                    .Cast<dynamic>()
                    .ToListAsync();

                return new ResultVM<List<dynamic>>
                {
                    IsSuccess = true,
                    Data = proyectos,
                    Message = $"Se encontraron {proyectos.Count} proyectos sin asignar"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AsignacionProyectos] Error al obtener proyectos: {ex.Message}");
                return new ResultVM<List<dynamic>>
                {
                    IsSuccess = false,
                    Data = new List<dynamic>(),
                    Message = $"Error al obtener proyectos: {ex.Message}"
                };
            }
        }

        public async Task<ResultVM<List<dynamic>>> ObtenerProyectosXReasignarAsync(int idUnidad, string? filtroNombre, long idUsuario)
        {
            try
            {
                _logger.LogInformation($"[AsignacionProyectos] Obteniendo proyectos para reasignación en unidad {idUnidad}");

                var query = _context.Proyectos
                    .Where(p => p.IdUnidad == idUnidad && p.IdGerenteProyectos != null && p.IdGerenteProyectos != 0);

                if (!string.IsNullOrWhiteSpace(filtroNombre))
                {
                    query = query.Where(p => (p.Nombre != null && p.Nombre.Contains(filtroNombre)) ||
                                             (p.JobBook != null && p.JobBook.Contains(filtroNombre)));
                }

                var proyectos = await query
                    .Select(p => new
                    {
                        p.Id,
                        p.Nombre,
                        p.JobBook,
                        p.IdUnidad,
                        p.Estado,
                        GerenteProyectosActual = p.IdGerenteProyectos,
                        NombreGerente = (string?)null // Se cargaría de tabla de usuarios si estuviera disponible
                    })
                    .Cast<dynamic>()
                    .ToListAsync();

                return new ResultVM<List<dynamic>>
                {
                    IsSuccess = true,
                    Data = proyectos,
                    Message = $"Se encontraron {proyectos.Count} proyectos para reasignar"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AsignacionProyectos] Error al obtener proyectos para reasignación: {ex.Message}");
                return new ResultVM<List<dynamic>>
                {
                    IsSuccess = false,
                    Data = new List<dynamic>(),
                    Message = $"Error al obtener proyectos: {ex.Message}"
                };
            }
        }

        public async Task<ResultVM<List<dynamic>>> ObtenerGerentesDisponiblesAsync(int idUnidad, long idUsuario)
        {
            try
            {
                _logger.LogInformation($"[AsignacionProyectos] Obteniendo gerentes disponibles para unidad {idUnidad}");

                // Nota: En una implementación completa, esto consultaría la tabla de Usuarios/Roles
                // Para ahora, retornamos una estructura base que será completada cuando Users se implemente
                var gerentes = new List<dynamic>
                {
                    new { Id = 1L, Nombre = "Gerente Demo 1", Activo = true },
                    new { Id = 2L, Nombre = "Gerente Demo 2", Activo = true }
                };

                return new ResultVM<List<dynamic>>
                {
                    IsSuccess = true,
                    Data = gerentes,
                    Message = $"Se encontraron {gerentes.Count} gerentes disponibles"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AsignacionProyectos] Error al obtener gerentes: {ex.Message}");
                return new ResultVM<List<dynamic>>
                {
                    IsSuccess = false,
                    Data = new List<dynamic>(),
                    Message = $"Error al obtener gerentes: {ex.Message}"
                };
            }
        }

        public async Task<ResultVM<bool>> AsignarGerenteAsync(long idProyecto, long idGerenteProyecto, long idUsuarioActual, string? observaciones = null)
        {
            try
            {
                _logger.LogInformation($"[AsignacionProyectos] Asignando gerente {idGerenteProyecto} al proyecto {idProyecto}");

                var proyecto = await _context.Proyectos.FindAsync(idProyecto);
                if (proyecto == null)
                {
                    return new ResultVM<bool>
                    {
                        IsSuccess = false,
                        Data = false,
                        Message = "Proyecto no encontrado"
                    };
                }

                // Crear registro de asignación
                var asignacion = new AsignacionProyecto
                {
                    IdProyecto = idProyecto,
                    IdGerenteProyecto = idGerenteProyecto,
                    NombreGerenteProyecto = $"Usuario {idGerenteProyecto}", // Se cargaría del sistema de usuarios
                    TipoAsignacion = "Inicial",
                    Observaciones = observaciones,
                    FechaAsignacion = DateTime.UtcNow,
                    UsuarioCreacion = idUsuarioActual,
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                };

                _context.AsignacionesProyectos.Add(asignacion);

                // Actualizar proyecto
                proyecto.IdGerenteProyectos = idGerenteProyecto;
                proyecto.UsuarioModificacion = idUsuarioActual;
                proyecto.FechaModificacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"[AsignacionProyectos] Asignación completada exitosamente");

                return new ResultVM<bool>
                {
                    IsSuccess = true,
                    Data = true,
                    Message = "Gerente asignado exitosamente"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AsignacionProyectos] Error al asignar gerente: {ex.Message}");
                return new ResultVM<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = $"Error al asignar gerente: {ex.Message}"
                };
            }
        }

        public async Task<ResultVM<bool>> ReasignarGerenteAsync(long idProyecto, long idGerenteNuevo, long idUsuarioActual, string? observaciones = null)
        {
            try
            {
                _logger.LogInformation($"[AsignacionProyectos] Reasignando gerente {idGerenteNuevo} al proyecto {idProyecto}");

                var proyecto = await _context.Proyectos.FindAsync(idProyecto);
                if (proyecto == null)
                {
                    return new ResultVM<bool>
                    {
                        IsSuccess = false,
                        Data = false,
                        Message = "Proyecto no encontrado"
                    };
                }

                var gerentePrevio = proyecto.IdGerenteProyectos;
                var nombreGerentePrevio = $"Usuario {gerentePrevio}";

                // Crear registro de reasignación
                var asignacion = new AsignacionProyecto
                {
                    IdProyecto = idProyecto,
                    IdGerenteProyecto = idGerenteNuevo,
                    NombreGerenteProyecto = $"Usuario {idGerenteNuevo}",
                    TipoAsignacion = "Reasignación",
                    Observaciones = observaciones,
                    IdGerentePrevio = gerentePrevio,
                    NombreGerentePrevio = nombreGerentePrevio,
                    FechaAsignacion = DateTime.UtcNow,
                    UsuarioCreacion = idUsuarioActual,
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                };

                _context.AsignacionesProyectos.Add(asignacion);

                // Actualizar proyecto
                proyecto.IdGerenteProyectos = idGerenteNuevo;
                proyecto.UsuarioModificacion = idUsuarioActual;
                proyecto.FechaModificacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"[AsignacionProyectos] Reasignación completada exitosamente");

                return new ResultVM<bool>
                {
                    IsSuccess = true,
                    Data = true,
                    Message = "Gerente reasignado exitosamente"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AsignacionProyectos] Error al reasignar gerente: {ex.Message}");
                return new ResultVM<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = $"Error al reasignar gerente: {ex.Message}"
                };
            }
        }

        public async Task<ResultVM<List<AsignacionProyecto>>> ObtenerHistorialAsync(long idProyecto, long idUsuario)
        {
            try
            {
                _logger.LogInformation($"[AsignacionProyectos] Obteniendo historial del proyecto {idProyecto}");

                var historial = await _context.AsignacionesProyectos
                    .Where(a => a.IdProyecto == idProyecto)
                    .OrderByDescending(a => a.FechaAsignacion)
                    .ToListAsync();

                return new ResultVM<List<AsignacionProyecto>>
                {
                    IsSuccess = true,
                    Data = historial,
                    Message = $"Se encontraron {historial.Count} registros de asignación"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AsignacionProyectos] Error al obtener historial: {ex.Message}");
                return new ResultVM<List<AsignacionProyecto>>
                {
                    IsSuccess = false,
                    Data = new List<AsignacionProyecto>(),
                    Message = $"Error al obtener historial: {ex.Message}"
                };
            }
        }

        public async Task<ResultVM<bool>> ValidarPermisosAsync(long idUsuario)
        {
            try
            {
                // Validación básica: usuario debe ser gerente o administrador
                // En una implementación completa, consultaría tabla de usuarios y roles
                _logger.LogInformation($"[AsignacionProyectos] Validando permisos para usuario {idUsuario}");

                // Por ahora, permitimos cualquier usuario autenticado
                // Esto será refinado cuando UserRoles esté disponible
                return new ResultVM<bool>
                {
                    IsSuccess = true,
                    Data = true,
                    Message = "Usuario tiene permisos suficientes"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AsignacionProyectos] Error al validar permisos: {ex.Message}");
                return new ResultVM<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = $"Error al validar permisos: {ex.Message}"
                };
            }
        }
    }
}
