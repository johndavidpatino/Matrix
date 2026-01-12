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
    /// Adaptador para gestión de Experiencia Laboral de empleados
    /// </summary>
    public class ThExperienciaLaboralAdapter
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ThExperienciaLaboralAdapter> _logger;

        public ThExperienciaLaboralAdapter(ApplicationDbContext context, ILogger<ThExperienciaLaboralAdapter> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene lista de experiencias laborales de un empleado
        /// </summary>
        public async Task<List<ExperienciaLaboralDto>> ObtenerExperienciasLaborales(long personaId)
        {
            try
            {
                var experiencias = new List<ExperienciaLaboralDto>();

                var resultado = await _context.Database.GetDbConnection().QueryAsync<dynamic>(
                    "TH_ExperienciaLaboral_Get",
                    new { personaId = personaId },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                foreach (var row in resultado)
                {
                    experiencias.Add(new ExperienciaLaboralDto
                    {
                        Id = row.Id,
                        PersonaId = row.PersonaId,
                        Empresa = row.Empresa,
                        FechaInicio = row.FechaInicio,
                        FechaFin = row.FechaFin,
                        Cargo = row.Cargo,
                        EsInvestigacion = row.EsInvestigacion
                    });
                }

                return experiencias;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener experiencias laborales de {personaId}");
                throw;
            }
        }

        /// <summary>
        /// Agrega una nueva experiencia laboral
        /// </summary>
        public async Task<long> AgregarExperienciaLaboral(ExperienciaLaboralInputDto input)
        {
            try
            {
                var newId = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<long?>(
                    "TH_ExperienciaLaboral_Add",
                    new
                    {
                        personaId = input.PersonaId,
                        empresa = input.Empresa,
                        fechaInicio = input.FechaInicio,
                        fechaFin = input.FechaFin,
                        cargo = input.Cargo,
                        esInvestigacion = input.EsInvestigacion
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Experiencia laboral agregada con ID {newId}");
                return newId ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar experiencia laboral");
                throw;
            }
        }

        /// <summary>
        /// Elimina una experiencia laboral
        /// </summary>
        public async Task<bool> EliminarExperienciaLaboral(long id)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().ExecuteAsync(
                    "TH_ExperienciaLaboral_Del",
                    new { id = id },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Experiencia laboral {id} eliminada");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar experiencia laboral {id}");
                throw;
            }
        }

        /// <summary>
        /// Edita una experiencia laboral existente
        /// </summary>
        public async Task<bool> EditarExperienciaLaboral(long id, ExperienciaLaboralInputDto input)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().ExecuteAsync(
                    "TH_ExperienciaLaboral_Edit",
                    new
                    {
                        id = id,
                        empresa = input.Empresa,
                        fechaInicio = input.FechaInicio,
                        fechaFin = input.FechaFin,
                        cargo = input.Cargo,
                        esInvestigacion = input.EsInvestigacion
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Experiencia laboral {id} editada");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al editar experiencia laboral {id}");
                throw;
            }
        }
    }
}
