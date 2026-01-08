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
    public class MuestrasCualiService : IMuestrasCualiService
    {
        private readonly MatrixDbContext _context;
        private readonly IAuditoriaService _auditoria;

        public MuestrasCualiService(MatrixDbContext context, IAuditoriaService auditoria)
        {
            _context = context;
            _auditoria = auditoria;
        }

        public async Task<List<MuestrasCuali>> ObtenerPorTrabajoAsync(long idTrabajoCuali)
        {
            return await _context.MuestrasCuali
                .AsNoTracking()
                .Where(m => m.IdTrabajoCuali == idTrabajoCuali && m.Activo)
                .OrderBy(m => m.NumeroMuestra)
                .ToListAsync();
        }

        public async Task<List<MuestrasCuali>> ObtenerPorSegmentoAsync(long idSegmento)
        {
            return await _context.MuestrasCuali
                .AsNoTracking()
                .Where(m => m.IdSegmento == idSegmento && m.Activo)
                .OrderBy(m => m.NumeroMuestra)
                .ToListAsync();
        }

        public async Task<List<MuestrasCuali>> ObtenerPorEstadoAsync(string estado)
        {
            return await _context.MuestrasCuali
                .AsNoTracking()
                .Where(m => m.Estado == estado && m.Activo)
                .OrderBy(m => m.FechaContacto)
                .ToListAsync();
        }

        public async Task<MuestrasCuali?> ObtenerPorIdAsync(long id)
        {
            return await _context.MuestrasCuali
                .Include(m => m.Segmento)
                .Include(m => m.Entrevistador)
                .FirstOrDefaultAsync(m => m.Id == id && m.Activo);
        }

        public async Task<ResultVM<long>> CrearAsync(MuestrasCuali muestra, long idUsuario)
        {
            try
            {
                var trabajoExiste = await _context.TrabajosCuali
                    .AnyAsync(t => t.Id == muestra.IdTrabajoCuali && t.Activo);
                if (!trabajoExiste)
                {
                    return ResultVM<long>.Fail("El trabajo cualitativo no existe");
                }

                // Validar número de muestra único
                var numeroMuestraDuplicado = await _context.MuestrasCuali
                    .AnyAsync(m => m.IdTrabajoCuali == muestra.IdTrabajoCuali && 
                                  m.NumeroMuestra == muestra.NumeroMuestra && m.Activo);
                if (numeroMuestraDuplicado)
                {
                    return ResultVM<long>.Fail($"Ya existe una muestra con el número '{muestra.NumeroMuestra}'");
                }

                muestra.FechaCreacion = DateTime.UtcNow;
                muestra.Activo = true;

                _context.MuestrasCuali.Add(muestra);
                await _context.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "MuestrasCuali",
                    EntidadId = muestra.Id,
                    Accion = "Crear",
                    Detalles = $"Muestra #{muestra.NumeroMuestra} creada - Participante: {muestra.NombreParticipante}"
                });

                return ResultVM<long>.Ok(muestra.Id, "Muestra creada exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<long>.Fail($"Error al crear muestra: {ex.Message}");
            }
        }

        public async Task<ResultVM<bool>> ActualizarAsync(MuestrasCuali muestra, long idUsuario)
        {
            try
            {
                var muestraExistente = await _context.MuestrasCuali.FindAsync(muestra.Id);
                if (muestraExistente == null || !muestraExistente.Activo)
                {
                    return ResultVM<bool>.Fail("Muestra no encontrada");
                }

                // Validar número de muestra único (excluyendo la actual)
                var numeroMuestraDuplicado = await _context.MuestrasCuali
                    .AnyAsync(m => m.IdTrabajoCuali == muestra.IdTrabajoCuali &&
                                  m.NumeroMuestra == muestra.NumeroMuestra &&
                                  m.Id != muestra.Id && m.Activo);
                if (numeroMuestraDuplicado)
                {
                    return ResultVM<bool>.Fail($"Ya existe otra muestra con el número '{muestra.NumeroMuestra}'");
                }

                muestraExistente.NumeroMuestra = muestra.NumeroMuestra;
                muestraExistente.NombreParticipante = muestra.NombreParticipante;
                muestraExistente.Telefono = muestra.Telefono;
                muestraExistente.Email = muestra.Email;
                muestraExistente.Direccion = muestra.Direccion;
                muestraExistente.Edad = muestra.Edad;
                muestraExistente.Genero = muestra.Genero;
                muestraExistente.Estrato = muestra.Estrato;
                muestraExistente.Ocupacion = muestra.Ocupacion;
                muestraExistente.Estado = muestra.Estado;
                muestraExistente.FechaContacto = muestra.FechaContacto;
                muestraExistente.FechaEjecucion = muestra.FechaEjecucion;
                muestraExistente.DuracionEntrevista = muestra.DuracionEntrevista;
                muestraExistente.CalidadDatos = muestra.CalidadDatos;
                muestraExistente.MotivoRechazo = muestra.MotivoRechazo;
                muestraExistente.Notas = muestra.Notas;
                muestraExistente.FechaModificacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "MuestrasCuali",
                    EntidadId = muestra.Id,
                    Accion = "Actualizar",
                    Detalles = $"Muestra #{muestra.NumeroMuestra} actualizada"
                });

                return ResultVM<bool>.Ok(true, "Muestra actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail($"Error al actualizar muestra: {ex.Message}");
            }
        }

        public async Task<ResultVM<bool>> CambiarEstadoAsync(long idMuestra, string nuevoEstado, long idUsuario)
        {
            try
            {
                var muestra = await _context.MuestrasCuali.FindAsync(idMuestra);
                if (muestra == null || !muestra.Activo)
                {
                    return ResultVM<bool>.Fail("Muestra no encontrada");
                }

                var estadoAnterior = muestra.Estado;
                muestra.Estado = nuevoEstado;
                muestra.FechaModificacion = DateTime.UtcNow;

                if (nuevoEstado == "Entrevistada" && !muestra.FechaEjecucion.HasValue)
                {
                    muestra.FechaEjecucion = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "MuestrasCuali",
                    EntidadId = idMuestra,
                    Accion = "CambiarEstado",
                    Detalles = $"Estado cambiado de '{estadoAnterior}' a '{nuevoEstado}' - Muestra #{muestra.NumeroMuestra}"
                });

                return ResultVM<bool>.Ok(true, "Estado cambiado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail($"Error al cambiar estado: {ex.Message}");
            }
        }

        public async Task<ResultVM<bool>> EliminarAsync(long idMuestra, long idUsuario)
        {
            try
            {
                var muestra = await _context.MuestrasCuali.FindAsync(idMuestra);
                if (muestra == null || !muestra.Activo)
                {
                    return ResultVM<bool>.Fail("Muestra no encontrada");
                }

                muestra.Activo = false;
                muestra.FechaModificacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "MuestrasCuali",
                    EntidadId = idMuestra,
                    Accion = "Eliminar",
                    Detalles = $"Muestra #{muestra.NumeroMuestra} eliminada"
                });

                return ResultVM<bool>.Ok(true, "Muestra eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail($"Error al eliminar muestra: {ex.Message}");
            }
        }

        public async Task<ResultVM<bool>> AsignarEntrevistadorAsync(long idMuestra, long idEntrevistador, long idUsuario)
        {
            try
            {
                var muestra = await _context.MuestrasCuali.FindAsync(idMuestra);
                if (muestra == null || !muestra.Activo)
                {
                    return ResultVM<bool>.Fail("Muestra no encontrada");
                }

                var entrevistadorExiste = await _context.EntrevistadorasCuali
                    .AnyAsync(e => e.Id == idEntrevistador && e.Activo);
                if (!entrevistadorExiste)
                {
                    return ResultVM<bool>.Fail("Entrevistador no encontrado");
                }

                muestra.IdEntrevistador = idEntrevistador;
                muestra.FechaModificacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "MuestrasCuali",
                    EntidadId = idMuestra,
                    Accion = "AsignarEntrevistador",
                    Detalles = $"Entrevistador {idEntrevistador} asignado a muestra #{muestra.NumeroMuestra}"
                });

                return ResultVM<bool>.Ok(true, "Entrevistador asignado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail($"Error al asignar entrevistador: {ex.Message}");
            }
        }
    }
}
