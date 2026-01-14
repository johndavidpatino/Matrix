using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Data.Adapters.PY
{
    public class PyVariablesControlAdapter : IPyVariablesControlAdapter
    {
        private readonly MatrixDbContext _context;

        public PyVariablesControlAdapter(MatrixDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene variables de control por trabajo y modalidad (EF Core LINQ)
        /// Legacy: oProyecto.ObtenerVariableControlxTrabajoxMod(trabajoId, modalidad)
        /// </summary>
        public async Task<List<VariableControlDto>> ObtenerVariablesControlPorTrabajo(long trabajoId, string? modalidad = null)
        {
            // TODO: Registrar entidad PY_Variables_Control en MatrixDbContext
            await Task.CompletedTask;
            return new List<VariableControlDto>();
            /*
            var query = _context.Set<Entities.PY_Variables_Control>()
                .Where(x => x.TrabajoId == trabajoId);

            if (!string.IsNullOrWhiteSpace(modalidad))
            {
                query = query.Where(x => x.Modalidad == modalidad);
            }

            var resultado = await query
                .Select(x => new VariableControlDto
                {
                    Id = x.Id,
                    TrabajoId = x.TrabajoId,
                    Modalidad = x.Modalidad,
                    VariableControl = x.VariableControl
                })
                .ToListAsync();

            return resultado;
            */
        }

        /// <summary>
        /// Obtiene variable de control por ID
        /// </summary>
        public async Task<VariableControlDto?> ObtenerVariableControlPorId(long id)
        {
            await Task.CompletedTask;
            return null;
            /*
            var entidad = await _context.Set<Entities.PY_Variables_Control>()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null) return null;

            return new VariableControlDto
            {
                Id = entidad.Id,
                TrabajoId = entidad.TrabajoId,
                Modalidad = entidad.Modalidad,
                VariableControl = entidad.VariableControl
            };
            */
        }

        /// <summary>
        /// Guarda o actualiza variable de control (EF Core)
        /// </summary>
        public async Task<long> GuardarVariableControl(VariableControlInputDto input)
        {
            await Task.CompletedTask;
            return input.Id ?? 0;
            /*
            var entidad = input.Id.HasValue
                ? await _context.Set<Entities.PY_Variables_Control>()
                    .FirstOrDefaultAsync(x => x.Id == input.Id.Value)
                : null;

            if (entidad == null)
            {
                // Crear nueva
                entidad = new Entities.PY_Variables_Control
                {
                    TrabajoId = input.TrabajoId,
                    Modalidad = input.Modalidad,
                    VariableControl = input.VariableControl
                };

                _context.Set<Entities.PY_Variables_Control>().Add(entidad);
            }
            else
            {
                // Actualizar existente
                entidad.Modalidad = input.Modalidad;
                entidad.VariableControl = input.VariableControl;
            }

            await _context.SaveChangesAsync();
            return entidad.Id;
            */
        }
    }
}
