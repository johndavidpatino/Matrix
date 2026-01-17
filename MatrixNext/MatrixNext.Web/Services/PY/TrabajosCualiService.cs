using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.PY;
using MatrixNext.Web.ViewModels;
using MatrixNext.Web.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.PY
{
    /// <summary>
    /// Servicio para gestión de trabajos cualitativos.
    /// Implementa operaciones CRUD y lógica de negocio para investigación cualitativa.
    /// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 1.4 (TrabajosCualitativos.aspx.vb)
    /// </summary>
    public class TrabajosCualiService : ITrabajosCualiService
    {
        private readonly MatrixDbContext _context;
        private readonly IAuditoriaService _auditoria;

        public TrabajosCualiService(MatrixDbContext context, IAuditoriaService auditoria)
        {
            _context = context;
            _auditoria = auditoria;
        }

        public async Task<List<TrabajosCuali>> ObtenerPorProyectoAsync(long idProyecto)
        {
            return await _context.TrabajosCuali
                .AsNoTracking()
                .Where(t => t.IdProyecto == idProyecto && t.Activo)
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        public async Task<TrabajosCuali?> ObtenerPorIdAsync(long id)
        {
            return await _context.TrabajosCuali
                .Include(t => t.Segmentos)
                .Include(t => t.Sesiones)
                .Include(t => t.Muestras)
                .FirstOrDefaultAsync(t => t.Id == id && t.Activo);
        }

        public async Task<List<TrabajosCuali>> ObtenerPorEstadoAsync(string estado)
        {
            return await _context.TrabajosCuali
                .AsNoTracking()
                .Where(t => t.Estado == estado && t.Activo)
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        public async Task<List<TrabajosCuali>> ObtenerPorCoordinadorAsync(long idCoordinador)
        {
            return await _context.TrabajosCuali
                .AsNoTracking()
                .Where(t => t.IdCoordinador == idCoordinador && t.Activo)
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        public async Task<ResultVM<long>> CrearAsync(TrabajosCuali trabajo, long idUsuario)
        {
            try
            {
                // Validar que el proyecto existe
                var proyectoExiste = await _context.Proyectos.AnyAsync(p => p.Id == trabajo.IdProyecto && p.Activo);
                if (!proyectoExiste)
                {
                    return ResultVM<long>.Fail("El proyecto especificado no existe");
                }

                // Validar JobBook único
                if (!string.IsNullOrEmpty(trabajo.JobBook))
                {
                    var jobBookExiste = await _context.TrabajosCuali
                        .AnyAsync(t => t.JobBook == trabajo.JobBook && t.Activo);
                    if (jobBookExiste)
                    {
                        return ResultVM<long>.Fail($"Ya existe un trabajo con el JobBook '{trabajo.JobBook}'");
                    }
                }

                trabajo.FechaCreacion = DateTime.UtcNow;
                trabajo.Activo = true;

                _context.TrabajosCuali.Add(trabajo);
                await _context.SaveChangesAsync();

                // Registrar auditoría
                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "TrabajosCuali",
                    EntidadId = trabajo.Id,
                    Accion = "Crear",
                    Detalles = $"Trabajo cualitativo '{trabajo.Nombre}' creado en proyecto {trabajo.IdProyecto}"
                });

                return ResultVM<long>.Ok(trabajo.Id, "Trabajo cualitativo creado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<long>.Fail("Error al crear trabajo cualitativo. Por favor intente nuevamente.");
            }
        }

        public async Task<ResultVM<bool>> ActualizarAsync(TrabajosCuali trabajo, long idUsuario)
        {
            try
            {
                var trabajoExistente = await _context.TrabajosCuali.FindAsync(trabajo.Id);
                if (trabajoExistente == null || !trabajoExistente.Activo)
                {
                    return ResultVM<bool>.Fail("Trabajo cualitativo no encontrado");
                }

                // Validar JobBook único (excluyendo el actual)
                if (!string.IsNullOrEmpty(trabajo.JobBook))
                {
                    var jobBookDuplicado = await _context.TrabajosCuali
                        .AnyAsync(t => t.JobBook == trabajo.JobBook && t.Id != trabajo.Id && t.Activo);
                    if (jobBookDuplicado)
                    {
                        return ResultVM<bool>.Fail($"Ya existe otro trabajo con el JobBook '{trabajo.JobBook}'");
                    }
                }

                // Actualizar campos
                trabajoExistente.Nombre = trabajo.Nombre;
                trabajoExistente.Descripcion = trabajo.Descripcion;
                trabajoExistente.JobBook = trabajo.JobBook;
                trabajoExistente.Estado = trabajo.Estado;
                trabajoExistente.IdCoordinador = trabajo.IdCoordinador;
                trabajoExistente.IdGerenteProyecto = trabajo.IdGerenteProyecto;
                trabajoExistente.FechaVencimiento = trabajo.FechaVencimiento;
                trabajoExistente.PresupuestoEstimado = trabajo.PresupuestoEstimado;
                trabajoExistente.TipoEstudio = trabajo.TipoEstudio;
                trabajoExistente.NumeroParticipantesEstimado = trabajo.NumeroParticipantesEstimado;
                trabajoExistente.Ubicacion = trabajo.Ubicacion;
                trabajoExistente.Notas = trabajo.Notas;
                trabajoExistente.FechaModificacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Registrar auditoría
                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "TrabajosCuali",
                    EntidadId = trabajo.Id,
                    Accion = "Actualizar",
                    Detalles = $"Trabajo cualitativo '{trabajo.Nombre}' actualizado"
                });

                return ResultVM<bool>.Ok(true, "Trabajo actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail("Error al actualizar trabajo. Por favor intente nuevamente.");
            }
        }

        public async Task<ResultVM<bool>> CambiarEstadoAsync(long idTrabajo, string nuevoEstado, long idUsuario, string? observacion = null)
        {
            try
            {
                var trabajo = await _context.TrabajosCuali.FindAsync(idTrabajo);
                if (trabajo == null || !trabajo.Activo)
                {
                    return ResultVM<bool>.Fail("Trabajo cualitativo no encontrado");
                }

                var estadoAnterior = trabajo.Estado;
                trabajo.Estado = nuevoEstado;
                trabajo.FechaModificacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Registrar auditoría
                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "TrabajosCuali",
                    EntidadId = idTrabajo,
                    Accion = "CambiarEstado",
                    Detalles = $"Estado cambiado de '{estadoAnterior}' a '{nuevoEstado}'. Observación: {observacion ?? "N/A"}"
                });

                return ResultVM<bool>.Ok(true, $"Estado cambiado a '{nuevoEstado}' exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail("Error al cambiar estado. Por favor intente nuevamente.");
            }
        }

        public async Task<ResultVM<bool>> EliminarAsync(long idTrabajo, long idUsuario)
        {
            try
            {
                var trabajo = await _context.TrabajosCuali.FindAsync(idTrabajo);
                if (trabajo == null || !trabajo.Activo)
                {
                    return ResultVM<bool>.Fail("Trabajo cualitativo no encontrado");
                }

                // Validar que no tenga datos asociados
                var validacion = await ValidarEliminacionAsync(idTrabajo);
                if (!validacion.IsSuccess)
                {
                    return validacion;
                }

                // Soft delete
                trabajo.Activo = false;
                trabajo.FechaModificacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Registrar auditoría
                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "TrabajosCuali",
                    EntidadId = idTrabajo,
                    Accion = "Eliminar",
                    Detalles = $"Trabajo cualitativo '{trabajo.Nombre}' eliminado (soft delete)"
                });

                return ResultVM<bool>.Ok(true, "Trabajo eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail("Error al eliminar trabajo. Por favor intente nuevamente.");
            }
        }

        public async Task<ResultVM<long>> DuplicarAsync(long idTrabajoOriginal, string nuevoNombre, long idUsuario)
        {
            try
            {
                var trabajoOriginal = await _context.TrabajosCuali
                    .Include(t => t.Segmentos)
                    .FirstOrDefaultAsync(t => t.Id == idTrabajoOriginal && t.Activo);

                if (trabajoOriginal == null)
                {
                    return ResultVM<long>.Fail("Trabajo cualitativo original no encontrado");
                }

                // Crear copia del trabajo
                var trabajoNuevo = new TrabajosCuali
                {
                    IdProyecto = trabajoOriginal.IdProyecto,
                    IdTrabajoRelacionado = trabajoOriginal.IdTrabajoRelacionado,
                    Nombre = nuevoNombre,
                    Descripcion = trabajoOriginal.Descripcion,
                    Estado = "Creado",
                    JobBook = null, // Se asignará manualmente después
                    IdCoordinador = trabajoOriginal.IdCoordinador,
                    IdGerenteProyecto = trabajoOriginal.IdGerenteProyecto,
                    FechaVencimiento = trabajoOriginal.FechaVencimiento,
                    PresupuestoEstimado = trabajoOriginal.PresupuestoEstimado,
                    TipoEstudio = trabajoOriginal.TipoEstudio,
                    NumeroParticipantesEstimado = trabajoOriginal.NumeroParticipantesEstimado,
                    Ubicacion = trabajoOriginal.Ubicacion,
                    Notas = $"Duplicado de: {trabajoOriginal.Nombre}",
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow
                };

                _context.TrabajosCuali.Add(trabajoNuevo);
                await _context.SaveChangesAsync();

                // Duplicar segmentos
                foreach (var segmentoOriginal in trabajoOriginal.Segmentos)
                {
                    var segmentoNuevo = new SegmentosCuali
                    {
                        IdTrabajoCuali = trabajoNuevo.Id,
                        Nombre = segmentoOriginal.Nombre,
                        Descripcion = segmentoOriginal.Descripcion,
                        NumeroParticipantes = segmentoOriginal.NumeroParticipantes,
                        CuotaMinima = segmentoOriginal.CuotaMinima,
                        CuotaMaxima = segmentoOriginal.CuotaMaxima,
                        CriteriosInclusion = segmentoOriginal.CriteriosInclusion,
                        CriteriosExclusion = segmentoOriginal.CriteriosExclusion,
                        Notas = segmentoOriginal.Notas,
                        Orden = segmentoOriginal.Orden,
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow
                    };

                    _context.SegmentosCuali.Add(segmentoNuevo);
                }

                await _context.SaveChangesAsync();

                // Registrar auditoría
                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "TrabajosCuali",
                    EntidadId = trabajoNuevo.Id,
                    Accion = "Duplicar",
                    Detalles = $"Trabajo duplicado desde ID {idTrabajoOriginal} - Nuevo nombre: '{nuevoNombre}'"
                });

                return ResultVM<long>.Ok(trabajoNuevo.Id, "Trabajo duplicado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<long>.Fail("Error al duplicar trabajo. Por favor intente nuevamente.");
            }
        }

        public async Task<ResultVM<bool>> ValidarEliminacionAsync(long idTrabajo)
        {
            try
            {
                // Verificar si tiene sesiones
                var tieneSesiones = await _context.SesionesCuali
                    .AnyAsync(s => s.IdTrabajoCuali == idTrabajo && s.Activo);
                if (tieneSesiones)
                {
                    return ResultVM<bool>.Fail("No se puede eliminar el trabajo porque tiene sesiones asociadas");
                }

                // Verificar si tiene muestras
                var tieneMuestras = await _context.MuestrasCuali
                    .AnyAsync(m => m.IdTrabajoCuali == idTrabajo && m.Activo);
                if (tieneMuestras)
                {
                    return ResultVM<bool>.Fail("No se puede eliminar el trabajo porque tiene muestras asociadas");
                }

                // Verificar si tiene entrevistadores asignados
                var tieneEntrevistadores = await _context.EntrevistadorasCuali
                    .AnyAsync(e => e.IdTrabajoCuali == idTrabajo && e.Activo);
                if (tieneEntrevistadores)
                {
                    return ResultVM<bool>.Fail("No se puede eliminar el trabajo porque tiene entrevistadores asignados");
                }

                return ResultVM<bool>.Ok(true, "El trabajo puede ser eliminado");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail("Error al validar eliminación. Por favor intente nuevamente.");
            }
        }
    }
}
