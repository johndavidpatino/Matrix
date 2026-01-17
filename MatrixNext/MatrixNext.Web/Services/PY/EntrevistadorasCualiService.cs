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
    public class EntrevistadorasCualiService : IEntrevistadorasCualiService
    {
        private readonly MatrixDbContext _context;
        private readonly IAuditoriaService _auditoria;

        public EntrevistadorasCualiService(MatrixDbContext context, IAuditoriaService auditoria)
        {
            _context = context;
            _auditoria = auditoria;
        }

        public async Task<List<EntrevistadorasCuali>> ObtenerPorTrabajoAsync(long idTrabajoCuali)
        {
            return await _context.EntrevistadorasCuali
                .AsNoTracking()
                .Where(e => e.IdTrabajoCuali == idTrabajoCuali && e.Activo)
                .OrderBy(e => e.NombreCompleto)
                .ToListAsync();
        }

        public async Task<List<EntrevistadorasCuali>> ObtenerPorSegmentoAsync(long idSegmento)
        {
            return await _context.EntrevistadorasCuali
                .AsNoTracking()
                .Where(e => e.IdSegmento == idSegmento && e.Activo)
                .OrderBy(e => e.NombreCompleto)
                .ToListAsync();
        }

        public async Task<List<EntrevistadorasCuali>> ObtenerDisponiblesAsync()
        {
            return await _context.EntrevistadorasCuali
                .AsNoTracking()
                .Where(e => e.Disponibilidad == "Disponible" && e.Activo)
                .OrderBy(e => e.NombreCompleto)
                .ToListAsync();
        }

        public async Task<EntrevistadorasCuali?> ObtenerPorIdAsync(long id)
        {
            return await _context.EntrevistadorasCuali
                .Include(e => e.Muestras)
                .FirstOrDefaultAsync(e => e.Id == id && e.Activo);
        }

        public async Task<ResultVM<long>> CrearAsync(EntrevistadorasCuali entrevistador, long idUsuario)
        {
            try
            {
                var trabajoExiste = await _context.TrabajosCuali
                    .AnyAsync(t => t.Id == entrevistador.IdTrabajoCuali && t.Activo);
                if (!trabajoExiste)
                {
                    return ResultVM<long>.Fail("El trabajo cualitativo no existe");
                }

                // Validar que el usuario no esté ya asignado al mismo trabajo
                var yaAsignado = await _context.EntrevistadorasCuali
                    .AnyAsync(e => e.IdTrabajoCuali == entrevistador.IdTrabajoCuali &&
                                  e.IdUsuario == entrevistador.IdUsuario && e.Activo);
                if (yaAsignado)
                {
                    return ResultVM<long>.Fail("Este entrevistador ya está asignado al trabajo");
                }

                entrevistador.FechaAsignacion = DateTime.UtcNow;
                entrevistador.FechaCreacion = DateTime.UtcNow;
                entrevistador.Activo = true;

                _context.EntrevistadorasCuali.Add(entrevistador);
                await _context.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "EntrevistadorasCuali",
                    EntidadId = entrevistador.Id,
                    Accion = "Crear",
                    Detalles = $"Entrevistador '{entrevistador.NombreCompleto}' asignado a trabajo {entrevistador.IdTrabajoCuali}"
                });

                return ResultVM<long>.Ok(entrevistador.Id, "Entrevistador asignado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<long>.Fail("Error al asignar entrevistador. Por favor intente nuevamente.");
            }
        }

        public async Task<ResultVM<bool>> ActualizarAsync(EntrevistadorasCuali entrevistador, long idUsuario)
        {
            try
            {
                var entrevistadorExistente = await _context.EntrevistadorasCuali.FindAsync(entrevistador.Id);
                if (entrevistadorExistente == null || !entrevistadorExistente.Activo)
                {
                    return ResultVM<bool>.Fail("Entrevistador no encontrado");
                }

                entrevistadorExistente.NombreCompleto = entrevistador.NombreCompleto;
                entrevistadorExistente.Telefono = entrevistador.Telefono;
                entrevistadorExistente.Email = entrevistador.Email;
                entrevistadorExistente.Especialidad = entrevistador.Especialidad;
                entrevistadorExistente.NumeroEntrevistasAsignadas = entrevistador.NumeroEntrevistasAsignadas;
                entrevistadorExistente.NumeroEntrevistasCompletadas = entrevistador.NumeroEntrevistasCompletadas;
                entrevistadorExistente.PorcentajeCumplimiento = entrevistador.PorcentajeCumplimiento;
                entrevistadorExistente.FechaTermino = entrevistador.FechaTermino;
                entrevistadorExistente.Estado = entrevistador.Estado;
                entrevistadorExistente.NivelExperiencia = entrevistador.NivelExperiencia;
                entrevistadorExistente.Disponibilidad = entrevistador.Disponibilidad;
                entrevistadorExistente.Notas = entrevistador.Notas;
                entrevistadorExistente.FechaModificacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "EntrevistadorasCuali",
                    EntidadId = entrevistador.Id,
                    Accion = "Actualizar",
                    Detalles = $"Entrevistador '{entrevistador.NombreCompleto}' actualizado"
                });

                return ResultVM<bool>.Ok(true, "Entrevistador actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail("Error al actualizar entrevistador. Por favor intente nuevamente.");
            }
        }

        public async Task<ResultVM<bool>> CambiarDisponibilidadAsync(long idEntrevistador, string nuevaDisponibilidad, long idUsuario)
        {
            try
            {
                var entrevistador = await _context.EntrevistadorasCuali.FindAsync(idEntrevistador);
                if (entrevistador == null || !entrevistador.Activo)
                {
                    return ResultVM<bool>.Fail("Entrevistador no encontrado");
                }

                var disponibilidadAnterior = entrevistador.Disponibilidad;
                entrevistador.Disponibilidad = nuevaDisponibilidad;
                entrevistador.FechaModificacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "EntrevistadorasCuali",
                    EntidadId = idEntrevistador,
                    Accion = "CambiarDisponibilidad",
                    Detalles = $"Disponibilidad cambiada de '{disponibilidadAnterior}' a '{nuevaDisponibilidad}'"
                });

                return ResultVM<bool>.Ok(true, "Disponibilidad actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail("Error al cambiar disponibilidad. Por favor intente nuevamente.");
            }
        }

        public async Task<ResultVM<bool>> EliminarAsync(long idEntrevistador, long idUsuario)
        {
            try
            {
                var entrevistador = await _context.EntrevistadorasCuali.FindAsync(idEntrevistador);
                if (entrevistador == null || !entrevistador.Activo)
                {
                    return ResultVM<bool>.Fail("Entrevistador no encontrado");
                }

                // Validar que no tenga muestras asignadas
                var tieneMuestras = await _context.MuestrasCuali
                    .AnyAsync(m => m.IdEntrevistador == idEntrevistador && m.Activo);
                if (tieneMuestras)
                {
                    return ResultVM<bool>.Fail("No se puede eliminar porque tiene muestras asignadas");
                }

                entrevistador.Activo = false;
                entrevistador.FechaModificacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "EntrevistadorasCuali",
                    EntidadId = idEntrevistador,
                    Accion = "Eliminar",
                    Detalles = $"Entrevistador '{entrevistador.NombreCompleto}' eliminado"
                });

                return ResultVM<bool>.Ok(true, "Entrevistador eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail("Error al eliminar entrevistador. Por favor intente nuevamente.");
            }
        }

        public async Task<ResultVM<bool>> ActualizarPorcentajeCumplimientoAsync(long idEntrevistador)
        {
            try
            {
                var entrevistador = await _context.EntrevistadorasCuali.FindAsync(idEntrevistador);
                if (entrevistador == null || !entrevistador.Activo)
                {
                    return ResultVM<bool>.Fail("Entrevistador no encontrado");
                }

                if (entrevistador.NumeroEntrevistasAsignadas > 0)
                {
                    entrevistador.PorcentajeCumplimiento = 
                        (decimal)entrevistador.NumeroEntrevistasCompletadas / 
                        entrevistador.NumeroEntrevistasAsignadas * 100;
                }
                else
                {
                    entrevistador.PorcentajeCumplimiento = 0;
                }

                entrevistador.FechaModificacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ResultVM<bool>.Ok(true, "Porcentaje de cumplimiento actualizado");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail("Error al actualizar porcentaje. Por favor intente nuevamente.");
            }
        }
    }
}
