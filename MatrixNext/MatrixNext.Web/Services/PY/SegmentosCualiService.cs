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
    /// Servicio para gestión de segmentos de población en trabajos cualitativos.
    /// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 1.2 (SegmentosCuali.aspx.vb)
    /// </summary>
    public class SegmentosCualiService : ISegmentosCualiService
    {
        private readonly MatrixDbContext _context;
        private readonly IAuditoriaService _auditoria;

        public SegmentosCualiService(MatrixDbContext context, IAuditoriaService auditoria)
        {
            _context = context;
            _auditoria = auditoria;
        }

        public async Task<List<SegmentosCuali>> ObtenerPorTrabajoAsync(long idTrabajoCuali)
        {
            return await _context.SegmentosCuali
                .AsNoTracking()
                .Where(s => s.IdTrabajoCuali == idTrabajoCuali && s.Activo)
                .OrderBy(s => s.Orden)
                .ThenBy(s => s.Nombre)
                .ToListAsync();
        }

        public async Task<SegmentosCuali> ObtenerPorIdAsync(long id)
        {
            return await _context.SegmentosCuali
                .Include(s => s.Muestras)
                .Include(s => s.Entrevistadores)
                .FirstOrDefaultAsync(s => s.Id == id && s.Activo);
        }

        public async Task<ResultVM<long>> CrearAsync(SegmentosCuali segmento, long idUsuario)
        {
            try
            {
                // Validar que el trabajo existe
                var trabajoExiste = await _context.TrabajosCuali
                    .AnyAsync(t => t.Id == segmento.IdTrabajoCuali && t.Activo);
                if (!trabajoExiste)
                {
                    return ResultVM<long>.Fail("El trabajo cualitativo especificado no existe");
                }

                // Validar nombre único en el trabajo
                var nombreDuplicado = await _context.SegmentosCuali
                    .AnyAsync(s => s.IdTrabajoCuali == segmento.IdTrabajoCuali && 
                                  s.Nombre == segmento.Nombre && s.Activo);
                if (nombreDuplicado)
                {
                    return ResultVM<long>.Fail($"Ya existe un segmento con el nombre '{segmento.Nombre}' en este trabajo");
                }

                // Validar cuotas
                if (segmento.CuotaMinima.HasValue && segmento.CuotaMaxima.HasValue)
                {
                    if (segmento.CuotaMinima > segmento.CuotaMaxima)
                    {
                        return ResultVM<long>.Fail("La cuota mínima no puede ser mayor que la cuota máxima");
                    }
                }

                segmento.FechaCreacion = DateTime.UtcNow;
                segmento.Activo = true;

                _context.SegmentosCuali.Add(segmento);
                await _context.SaveChangesAsync();

                // Registrar auditoría
                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "SegmentosCuali",
                    EntidadId = segmento.Id,
                    Accion = "Crear",
                    Detalles = $"Segmento '{segmento.Nombre}' creado para trabajo {segmento.IdTrabajoCuali}"
                });

                return ResultVM<long>.Ok(segmento.Id, "Segmento creado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<long>.Fail($"Error al crear segmento: {ex.Message}");
            }
        }

        public async Task<ResultVM<bool>> ActualizarAsync(SegmentosCuali segmento, long idUsuario)
        {
            try
            {
                var segmentoExistente = await _context.SegmentosCuali.FindAsync(segmento.Id);
                if (segmentoExistente == null || !segmentoExistente.Activo)
                {
                    return ResultVM<bool>.Fail("Segmento no encontrado");
                }

                // Validar nombre único (excluyendo el actual)
                var nombreDuplicado = await _context.SegmentosCuali
                    .AnyAsync(s => s.IdTrabajoCuali == segmento.IdTrabajoCuali &&
                                  s.Nombre == segmento.Nombre && 
                                  s.Id != segmento.Id && s.Activo);
                if (nombreDuplicado)
                {
                    return ResultVM<bool>.Fail($"Ya existe otro segmento con el nombre '{segmento.Nombre}'");
                }

                // Validar cuotas
                if (segmento.CuotaMinima.HasValue && segmento.CuotaMaxima.HasValue)
                {
                    if (segmento.CuotaMinima > segmento.CuotaMaxima)
                    {
                        return ResultVM<bool>.Fail("La cuota mínima no puede ser mayor que la cuota máxima");
                    }
                }

                // Actualizar campos
                segmentoExistente.Nombre = segmento.Nombre;
                segmentoExistente.Descripcion = segmento.Descripcion;
                segmentoExistente.NumeroParticipantes = segmento.NumeroParticipantes;
                segmentoExistente.CuotaMinima = segmento.CuotaMinima;
                segmentoExistente.CuotaMaxima = segmento.CuotaMaxima;
                segmentoExistente.CriteriosInclusion = segmento.CriteriosInclusion;
                segmentoExistente.CriteriosExclusion = segmento.CriteriosExclusion;
                segmentoExistente.Notas = segmento.Notas;
                segmentoExistente.Orden = segmento.Orden;
                segmentoExistente.FechaModificacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Registrar auditoría
                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "SegmentosCuali",
                    EntidadId = segmento.Id,
                    Accion = "Actualizar",
                    Detalles = $"Segmento '{segmento.Nombre}' actualizado"
                });

                return ResultVM<bool>.Ok(true, "Segmento actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail($"Error al actualizar segmento: {ex.Message}");
            }
        }

        public async Task<ResultVM<bool>> EliminarAsync(long idSegmento, long idUsuario)
        {
            try
            {
                var segmento = await _context.SegmentosCuali.FindAsync(idSegmento);
                if (segmento == null || !segmento.Activo)
                {
                    return ResultVM<bool>.Fail("Segmento no encontrado");
                }

                // Validar que no tenga muestras asociadas
                var tieneMuestras = await _context.MuestrasCuali
                    .AnyAsync(m => m.IdSegmento == idSegmento && m.Activo);
                if (tieneMuestras)
                {
                    return ResultVM<bool>.Fail("No se puede eliminar el segmento porque tiene muestras asociadas");
                }

                // Soft delete
                segmento.Activo = false;
                segmento.FechaModificacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Registrar auditoría
                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "SegmentosCuali",
                    EntidadId = idSegmento,
                    Accion = "Eliminar",
                    Detalles = $"Segmento '{segmento.Nombre}' eliminado"
                });

                return ResultVM<bool>.Ok(true, "Segmento eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail($"Error al eliminar segmento: {ex.Message}");
            }
        }

        public async Task<ResultVM<long>> DuplicarAsync(long idSegmentoOriginal, long idUsuario)
        {
            try
            {
                var segmentoOriginal = await _context.SegmentosCuali
                    .FirstOrDefaultAsync(s => s.Id == idSegmentoOriginal && s.Activo);

                if (segmentoOriginal == null)
                {
                    return ResultVM<long>.Fail("Segmento original no encontrado");
                }

                // Crear copia del segmento
                var segmentoNuevo = new SegmentosCuali
                {
                    IdTrabajoCuali = segmentoOriginal.IdTrabajoCuali,
                    Nombre = $"{segmentoOriginal.Nombre} (Copia)",
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
                await _context.SaveChangesAsync();

                // Registrar auditoría
                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "SegmentosCuali",
                    EntidadId = segmentoNuevo.Id,
                    Accion = "Duplicar",
                    Detalles = $"Segmento duplicado desde ID {idSegmentoOriginal}"
                });

                return ResultVM<long>.Ok(segmentoNuevo.Id, "Segmento duplicado exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<long>.Fail($"Error al duplicar segmento: {ex.Message}");
            }
        }

        public async Task<int> ObtenerTotalParticipantesPorTrabajoAsync(long idTrabajoCuali)
        {
            return await _context.SegmentosCuali
                .Where(s => s.IdTrabajoCuali == idTrabajoCuali && s.Activo)
                .SumAsync(s => s.NumeroParticipantes);
        }
    }
}
