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
    /// Adaptador para datos complementarios de empleados:
    /// - Hijos
    /// - Contactos de Emergencia
    /// - Promociones
    /// - Salarios
    /// </summary>
    public class ThDatosComplementariosAdapter
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ThDatosComplementariosAdapter> _logger;

        public ThDatosComplementariosAdapter(ApplicationDbContext context, ILogger<ThDatosComplementariosAdapter> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region HIJOS

        public async Task<List<HijoDto>> ObtenerHijos(long personaId)
        {
            try
            {
                var hijos = new List<HijoDto>();

                var resultado = await _context.Database.GetDbConnection().QueryAsync<dynamic>(
                    "TH_Hijos_Get",
                    new { personaId = personaId },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                foreach (var row in resultado)
                {
                    hijos.Add(new HijoDto
                    {
                        Id = row.Id,
                        PersonaId = row.PersonaId,
                        Nombres = row.Nombres,
                        Apellidos = row.Apellidos,
                        Genero = row.Genero,
                        FechaNacimiento = row.FechaNacimiento
                    });
                }

                return hijos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener hijos de {personaId}");
                throw;
            }
        }

        public async Task<long> AgregarHijo(HijoInputDto input)
        {
            try
            {
                var newId = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<long?>(
                    "TH_Hijos_Add",
                    new
                    {
                        personaId = input.PersonaId,
                        nombres = input.Nombres,
                        apellidos = input.Apellidos,
                        genero = input.Genero,
                        fechaNacimiento = input.FechaNacimiento
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Hijo agregado con ID {newId}");
                return newId ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar hijo");
                throw;
            }
        }

        public async Task<bool> EliminarHijo(long id)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().ExecuteAsync(
                    "TH_Hijos_Del",
                    new { id = id },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Hijo {id} eliminado");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar hijo {id}");
                throw;
            }
        }

        #endregion

        #region CONTACTOS EMERGENCIA

        public async Task<List<ContactoEmergenciaDto>> ObtenerContactosEmergencia(long personaId)
        {
            try
            {
                var contactos = new List<ContactoEmergenciaDto>();

                var resultado = await _context.Database.GetDbConnection().QueryAsync<dynamic>(
                    "TH_ContactosEmergencia_Get",
                    new { personaId = personaId },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                foreach (var row in resultado)
                {
                    contactos.Add(new ContactoEmergenciaDto
                    {
                        Id = row.Id,
                        PersonaId = row.PersonaId,
                        Nombres = row.Nombres,
                        Apellidos = row.Apellidos,
                        ParentescoId = row.ParentescoId,
                        TelefonoFijo = row.TelefonoFijo,
                        TelefonoCelular = row.TelefonoCelular
                    });
                }

                return contactos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener contactos emergencia de {personaId}");
                throw;
            }
        }

        public async Task<long> AgregarContactoEmergencia(ContactoEmergenciaInputDto input)
        {
            try
            {
                var newId = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<long?>(
                    "TH_ContactosEmergencia_Add",
                    new
                    {
                        personaId = input.PersonaId,
                        nombres = input.Nombres,
                        apellidos = input.Apellidos,
                        parentescoId = input.ParentescoId,
                        telefonoFijo = input.TelefonoFijo,
                        telefonoCelular = input.TelefonoCelular
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Contacto emergencia agregado con ID {newId}");
                return newId ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar contacto emergencia");
                throw;
            }
        }

        public async Task<bool> EliminarContactoEmergencia(long id)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().ExecuteAsync(
                    "TH_ContactosEmergencia_Del",
                    new { id = id },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Contacto emergencia {id} eliminado");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar contacto emergencia {id}");
                throw;
            }
        }

        #endregion

        #region PROMOCIONES

        public async Task<List<PromocionDto>> ObtenerPromociones(long personaId)
        {
            try
            {
                var promociones = new List<PromocionDto>();

                var resultado = await _context.Database.GetDbConnection().QueryAsync<dynamic>(
                    "TH_Promociones_Get",
                    new { personaId = personaId },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                foreach (var row in resultado)
                {
                    promociones.Add(new PromocionDto
                    {
                        Id = row.Id,
                        PersonaId = row.PersonaId,
                        NuevaAreaId = row.NuevaAreaId,
                        NuevaBandaId = row.NuevaBandaId,
                        NuevoCargoId = row.NuevoCargoId,
                        NuevoLevelId = row.NuevoLevelId,
                        FechaPromocion = row.FechaPromocion
                    });
                }

                return promociones;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener promociones de {personaId}");
                throw;
            }
        }

        public async Task<long> AgregarPromocion(PromocionInputDto input)
        {
            try
            {
                var newId = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<long?>(
                    "TH_Promociones_Add",
                    new
                    {
                        personaId = input.PersonaId,
                        nuevaAreaId = input.NuevaAreaId,
                        nuevaBandaId = input.NuevaBandaId,
                        nuevoCargoId = input.NuevoCargoId,
                        nuevoLevelId = input.NuevoLevelId,
                        fechaPromocion = input.FechaPromocion
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Promoción agregada con ID {newId}");
                return newId ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar promoción");
                throw;
            }
        }

        public async Task<bool> EliminarPromocion(long id)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().ExecuteAsync(
                    "TH_Promociones_Del",
                    new { id = id },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Promoción {id} eliminada");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar promoción {id}");
                throw;
            }
        }

        #endregion

        #region SALARIOS

        public async Task<List<SalarioDto>> ObtenerSalarios(long personaId)
        {
            try
            {
                var salarios = new List<SalarioDto>();

                var resultado = await _context.Database.GetDbConnection().QueryAsync<dynamic>(
                    "TH_Salarios_Get",
                    new { personaId = personaId },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                foreach (var row in resultado)
                {
                    salarios.Add(new SalarioDto
                    {
                        Id = row.Id,
                        PersonaId = row.PersonaId,
                        FechaAplicacion = row.FechaAplicacion,
                        MotivoCambioId = row.MotivoCambioId,
                        Tipo = row.Tipo,
                        Monto = row.Monto
                    });
                }

                return salarios;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener salarios de {personaId}");
                throw;
            }
        }

        public async Task<long> AgregarSalario(SalarioInputDto input)
        {
            try
            {
                var newId = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<long?>(
                    "TH_Salarios_Add",
                    new
                    {
                        personaId = input.PersonaId,
                        fechaAplicacion = input.FechaAplicacion,
                        motivoCambioId = input.MotivoCambioId,
                        tipo = input.Tipo,
                        monto = input.Monto
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Salario agregado con ID {newId}");
                return newId ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar salario");
                throw;
            }
        }

        public async Task<bool> EliminarSalario(long id)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().ExecuteAsync(
                    "TH_Salarios_Del",
                    new { id = id },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Salario {id} eliminado");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar salario {id}");
                throw;
            }
        }

        #endregion
    }
}
