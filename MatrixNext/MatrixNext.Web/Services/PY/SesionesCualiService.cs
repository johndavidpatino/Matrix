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
    public class SesionesCualiService : ISesionesCualiService
    {
        private readonly MatrixDbContext _context;
        private readonly IAuditoriaService _auditoria;

        public SesionesCualiService(MatrixDbContext context, IAuditoriaService auditoria)
        {
            _context = context;
            _auditoria = auditoria;
        }

        public async Task<List<SesionesCuali>> ObtenerPorTrabajoAsync(long idTrabajoCuali)
        {
            return await _context.SesionesCuali
                .AsNoTracking()
                .Where(s => s.IdTrabajoCuali == idTrabajoCuali && s.Activo)
                .OrderBy(s => s.FechaProgramada)
                .ToListAsync();
        }

        public async Task<List<SesionesCuali>> ObtenerPorSegmentoAsync(long idSegmento)
        {
            return await _context.SesionesCuali
                .AsNoTracking()
                .Where(s => s.IdSegmento == idSegmento && s.Activo)
                .OrderBy(s => s.FechaProgramada)
                .ToListAsync();
        }

        public async Task<List<SesionesCuali>> ObtenerPorEstadoAsync(string estado)
        {
            return await _context.SesionesCuali
                .AsNoTracking()
                .Where(s => s.Estado == estado && s.Activo)
                .OrderBy(s => s.FechaProgramada)
                .ToListAsync();
        }

        public async Task<SesionesCuali> ObtenerPorIdAsync(long id)
        {
            return await _context.SesionesCuali
                .Include(s => s.Participantes)
                .FirstOrDefaultAsync(s => s.Id == id && s.Activo);
        }

        public async Task<ResultVM<long>> CrearAsync(SesionesCuali sesion, long idUsuario)
        {
            try
            {
                var trabajoExiste = await _context.TrabajosCuali
                    .AnyAsync(t => t.Id == sesion.IdTrabajoCuali && t.Activo);
                if (!trabajoExiste)
                {
                    return ResultVM<long>.Fail("El trabajo cualitativo no existe");
                }

                sesion.FechaCreacion = DateTime.UtcNow;
                sesion.Activo = true;

                _context.SesionesCuali.Add(sesion);
                await _context.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "SesionesCuali",
                    EntidadId = sesion.Id,
                    Accion = "Crear",
                    Detalles = $"Sesión '{sesion.Nombre}' creada para fecha {sesion.FechaProgramada:dd/MM/yyyy}"
                });

                return ResultVM<long>.Ok(sesion.Id, "Sesión creada exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<long>.Fail($"Error al crear sesión: {ex.Message}");
            }
        }

        public async Task<ResultVM<bool>> ActualizarAsync(SesionesCuali sesion, long idUsuario)
        {
            try
            {
                var sesionExistente = await _context.SesionesCuali.FindAsync(sesion.Id);
                if (sesionExistente == null || !sesionExistente.Activo)
                {
                    return ResultVM<bool>.Fail("Sesión no encontrada");
                }

                sesionExistente.Nombre = sesion.Nombre;
                sesionExistente.Tipo = sesion.Tipo;
                sesionExistente.FechaProgramada = sesion.FechaProgramada;
                sesionExistente.FechaEjecucion = sesion.FechaEjecucion;
                sesionExistente.HoraInicio = sesion.HoraInicio;
                sesionExistente.HoraFin = sesion.HoraFin;
                sesionExistente.DuracionEstimada = sesion.DuracionEstimada;
                sesionExistente.DuracionReal = sesion.DuracionReal;
                sesionExistente.Ubicacion = sesion.Ubicacion;
                sesionExistente.Moderador = sesion.Moderador;
                sesionExistente.NumeroParticipantesPlaneado = sesion.NumeroParticipantesPlaneado;
                sesionExistente.NumeroParticipantesReal = sesion.NumeroParticipantesReal;
                sesionExistente.Estado = sesion.Estado;
                sesionExistente.Observaciones = sesion.Observaciones;
                sesionExistente.UrlGrabacion = sesion.UrlGrabacion;
                sesionExistente.Notas = sesion.Notas;
                sesionExistente.FechaModificacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "SesionesCuali",
                    EntidadId = sesion.Id,
                    Accion = "Actualizar",
                    Detalles = $"Sesión '{sesion.Nombre}' actualizada"
                });

                return ResultVM<bool>.Ok(true, "Sesión actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail($"Error al actualizar sesión: {ex.Message}");
            }
        }

        public async Task<ResultVM<bool>> CambiarEstadoAsync(long idSesion, string nuevoEstado, long idUsuario, string observacion = null)
        {
            try
            {
                var sesion = await _context.SesionesCuali.FindAsync(idSesion);
                if (sesion == null || !sesion.Activo)
                {
                    return ResultVM<bool>.Fail("Sesión no encontrada");
                }

                var estadoAnterior = sesion.Estado;
                sesion.Estado = nuevoEstado;
                sesion.FechaModificacion = DateTime.UtcNow;

                if (nuevoEstado == "Ejecutada" && !sesion.FechaEjecucion.HasValue)
                {
                    sesion.FechaEjecucion = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "SesionesCuali",
                    EntidadId = idSesion,
                    Accion = "CambiarEstado",
                    Detalles = $"Estado cambiado de '{estadoAnterior}' a '{nuevoEstado}'. Obs: {observacion ?? "N/A"}"
                });

                return ResultVM<bool>.Ok(true, "Estado cambiado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail($"Error al cambiar estado: {ex.Message}");
            }
        }

        public async Task<ResultVM<bool>> EliminarAsync(long idSesion, long idUsuario)
        {
            try
            {
                var sesion = await _context.SesionesCuali.FindAsync(idSesion);
                if (sesion == null || !sesion.Activo)
                {
                    return ResultVM<bool>.Fail("Sesión no encontrada");
                }

                sesion.Activo = false;
                sesion.FechaModificacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "SesionesCuali",
                    EntidadId = idSesion,
                    Accion = "Eliminar",
                    Detalles = $"Sesión '{sesion.Nombre}' eliminada"
                });

                return ResultVM<bool>.Ok(true, "Sesión eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail($"Error al eliminar sesión: {ex.Message}");
            }
        }

        public async Task<ResultVM<bool>> RegistrarAsistenciaAsync(long idSesion, List<long> idsParticipantes, long idUsuario)
        {
            try
            {
                var sesion = await _context.SesionesCuali.FindAsync(idSesion);
                if (sesion == null || !sesion.Activo)
                {
                    return ResultVM<bool>.Fail("Sesión no encontrada");
                }

                foreach (var idMuestra in idsParticipantes)
                {
                    var participante = new ParticipantesSesion
                    {
                        IdSesion = idSesion,
                        IdMuestra = idMuestra,
                        Asistencia = "Asistió",
                        HoraLlegada = DateTime.Now,
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow
                    };

                    _context.ParticipantesSesion.Add(participante);
                }

                sesion.NumeroParticipantesReal = idsParticipantes.Count;
                await _context.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "SesionesCuali",
                    EntidadId = idSesion,
                    Accion = "RegistrarAsistencia",
                    Detalles = $"{idsParticipantes.Count} participantes registrados en sesión {sesion.Nombre}"
                });

                return ResultVM<bool>.Ok(true, "Asistencia registrada exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail($"Error al registrar asistencia: {ex.Message}");
            }
        }
    }
}
