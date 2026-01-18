using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.Adapters.TH.Models;
using MatrixNext.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Adapters.TH
{
    /// <summary>
    /// Adaptador para gestión de Educación de empleados
    /// </summary>
    public class ThEducacionAdapter
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ThEducacionAdapter> _logger;

        public ThEducacionAdapter(ApplicationDbContext context, ILogger<ThEducacionAdapter> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene lista de educación de un empleado
        /// </summary>
        public async Task<List<EducacionDto>> ObtenerEducacion(long personaId)
        {
            try
            {
                var educaciones = new List<EducacionDto>();

                var resultado = await _context.Database.GetDbConnection().QueryAsync<dynamic>(
                    "TH_Educacion_Get",
                    new { personaId = personaId },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                foreach (var row in resultado)
                {
                    educaciones.Add(new EducacionDto
                    {
                        Id = row.Id,
                        PersonaId = row.PersonaId,
                        Tipo = row.Tipo,
                        Titulo = row.Titulo,
                        Institucion = row.Institucion,
                        Pais = row.Pais,
                        Ciudad = row.Ciudad,
                        FechaInicio = row.FechaInicio,
                        FechaFin = row.FechaFin,
                        Modalidad = row.Modalidad,
                        Estado = row.Estado
                    });
                }

                return educaciones;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener educación de {personaId}");
                throw;
            }
        }

        /// <summary>
        /// Agrega una nueva educación
        /// </summary>
        public async Task<long> AgregarEducacion(EducacionInputDto input)
        {
            try
            {
                var newId = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<long?>(
                    "TH_Educacion_Add",
                    new
                    {
                        personaId = input.PersonaId,
                        tipo = input.Tipo,
                        titulo = input.Titulo,
                        institucion = input.Institucion,
                        pais = input.Pais,
                        ciudad = input.Ciudad,
                        fechaInicio = input.FechaInicio,
                        fechaFin = input.FechaFin,
                        modalidad = input.Modalidad,
                        estado = input.Estado
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Educación agregada con ID {newId}");
                return newId ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar educación");
                throw;
            }
        }

        /// <summary>
        /// Elimina una educación
        /// </summary>
        public async Task<bool> EliminarEducacion(long id)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().ExecuteAsync(
                    "TH_Educacion_Del",
                    new { id = id },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Educación {id} eliminada");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar educación {id}");
                throw;
            }
        }

        /// <summary>
        /// Edita una educación existente
        /// </summary>
        public async Task<bool> EditarEducacion(long id, EducacionInputDto input)
        {
            // STUB: SP TH_Educacion_Edit no existe en legacy
            _logger.LogWarning("[TH] EditarEducacion: SP TH_Educacion_Edit no existe en legacy. Id: {Id}", id);
            return await Task.FromResult(false);
        }
    }
}
