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
    /// Adaptador para gestión de Empleados - Delegación a SPs
    /// </summary>
    public class ThEmpleadosAdapter : IThEmpleadosAdapter
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ThEmpleadosAdapter> _logger;

        public ThEmpleadosAdapter(ApplicationDbContext context, ILogger<ThEmpleadosAdapter> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region EMPLEADO PRINCIPAL

        /// <summary>
        /// Obtiene lista de empleados con filtros opcionales
        /// </summary>
        public async Task<List<EmpleadoDto>> ObtenerEmpleados(long? id = null, string nombres = null, string apellidos = null, 
            bool? activo = null, byte? serviceLive = null, short? cargo = null, byte? sede = null)
        {
            try
            {
                var empleados = new List<EmpleadoDto>();

                // Mapeo de resultados desde el SP TH_Empleados_Get
                var resultado = await _context.Database.GetDbConnection().QueryAsync<dynamic>(
                    "TH_Empleados_Get",
                    new
                    {
                        id = id,
                        nombres = nombres,
                        apellidos = apellidos,
                        activo = activo,
                        serviceLive = serviceLive,
                        cargo = cargo,
                        sede = sede
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                foreach (var row in resultado)
                {
                    empleados.Add(MapToEmpleadoDto(row));
                }

                _logger.LogInformation($"Obtenidos {empleados.Count} empleados");
                return empleados;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empleados");
                throw;
            }
        }

        /// <summary>
        /// Obtiene un empleado específico por ID
        /// </summary>
        public async Task<EmpleadoDto> ObtenerEmpleadoPorId(long id)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<dynamic>(
                    "TH_Empleados_Get",
                    new { id = id },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                if (resultado == null)
                {
                    _logger.LogWarning($"Empleado con ID {id} no encontrado");
                    return null;
                }

                return MapToEmpleadoDto(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener empleado con ID {id}");
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo empleado
        /// </summary>
        public async Task<long> CrearEmpleado(EmpleadoInputDto input)
        {
            try
            {
                var newId = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<long?>(
                    "TH_Empleados_DatosGenerales_Add",
                    new
                    {
                        tipoIdentificacion = input.TipoId,
                        identificacion = input.Identificacion,
                        nombres = input.Nombres,
                        apellidos = input.Apellidos,
                        nombrePreferido = input.NombrePreferido,
                        fechaNacimiento = input.FechaNacimiento,
                        sexo = input.Sexo,
                        estadoCivil = input.EstadoCivil,
                        grupoSanguineo = input.GrupoSanguineo,
                        nacionalidad = input.Nacionalidad,
                        foto = input.FotoBase64
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Empleado creado con ID {newId}");
                return newId ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear empleado");
                throw;
            }
        }

        /// <summary>
        /// Actualiza datos generales del empleado
        /// </summary>
        public async Task<bool> ActualizarDatosGenerales(long id, EmpleadoInputDto input)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().ExecuteAsync(
                    "TH_Empleados_DatosGenerales_Edit",
                    new
                    {
                        id = id,
                        tipoIdentificacion = input.TipoId,
                        identificacion = input.Identificacion,
                        nombres = input.Nombres,
                        apellidos = input.Apellidos,
                        nombrePreferido = input.NombrePreferido,
                        fechaNacimiento = input.FechaNacimiento,
                        sexo = input.Sexo,
                        estadoCivil = input.EstadoCivil,
                        grupoSanguineo = input.GrupoSanguineo,
                        nacionalidad = input.Nacionalidad,
                        foto = input.FotoBase64
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Datos generales del empleado {id} actualizados");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar datos generales del empleado {id}");
                throw;
            }
        }

        /// <summary>
        /// Actualiza datos laborales del empleado
        /// </summary>
        public async Task<bool> ActualizarDatosLaborales(EmpleadoDatosLaboralesInputDto input)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().ExecuteAsync(
                    "TH_Empleados_DatosLaborales_Edit",
                    new
                    {
                        id = input.Id,
                        idIStaff = input.IdIStaff,
                        jefeInmediato = input.JefeInmediato,
                        sede = input.Sede,
                        correoIpsos = input.CorreoIpsos,
                        fechaIngreso = input.FechaIngreso,
                        centroCostoId = input.CentroCostoId,
                        tipoContratoId = input.TipoContratoId,
                        tiempoContratoId = input.TiempoContratoId,
                        empresa = input.Empresa,
                        jobFunctionId = input.JobFunctionId,
                        observaciones = input.Observaciones
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Datos laborales del empleado {input.Id} actualizados");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar datos laborales del empleado {input.Id}");
                throw;
            }
        }

        /// <summary>
        /// Actualiza datos personales del empleado
        /// </summary>
        public async Task<bool> ActualizarDatosPersonales(EmpleadoDatosPersonalesInputDto input)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().ExecuteAsync(
                    "TH_Empleados_DatosPersonales_Edit",
                    new
                    {
                        id = input.Id,
                        ciudadId = input.CiudadId,
                        direccion = input.Direccion,
                        nseId = input.NseId,
                        telefonoFijo = input.TelefonoFijo,
                        telefonoCelular = input.TelefonoCelular,
                        emailPersonal = input.EmailPersonal,
                        barrioResidencia = input.BarrioResidencia,
                        localidad = input.Localidad,
                        municipioNacimientoDivipolaId = input.MunicipioNacimientoDivipolaId,
                        tallaCamisetaId = input.TallaCamisetaId
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Datos personales del empleado {input.Id} actualizados");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar datos personales del empleado {input.Id}");
                throw;
            }
        }

        /// <summary>
        /// Actualiza datos de nómina del empleado
        /// </summary>
        public async Task<bool> ActualizarNomina(EmpleadoNominaInputDto input)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().ExecuteAsync(
                    "TH_Empleados_Nomina_Edit",
                    new
                    {
                        id = input.Id,
                        bancoId = input.BancoId,
                        tipoCuentaId = input.TipoCuentaId,
                        numeroCuenta = input.NumeroCuenta,
                        fondoPensionesId = input.FondoPensionesId,
                        fondoCesantiasId = input.FondoCesantiasId,
                        epsId = input.EPSId,
                        cajaCompensacionId = input.CajaCompensacionId,
                        arlId = input.ARLId
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Datos de nómina del empleado {input.Id} actualizados");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar datos de nómina del empleado {input.Id}");
                throw;
            }
        }

        /// <summary>
        /// Actualiza el salario del empleado
        /// </summary>
        public async Task<bool> ActualizarSalario(EmpleadoActualizarSalarioInputDto input)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().ExecuteAsync(
                    "TH_Empleados_DatosLaborales_ActualizarSalario",
                    new
                    {
                        empleadoId = input.EmpleadoId,
                        salario = input.Salario,
                        tipoSalarioId = input.TipoSalarioId
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Salario del empleado {input.EmpleadoId} actualizado");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar salario del empleado {input.EmpleadoId}");
                throw;
            }
        }

        /// <summary>
        /// Retira un empleado de la empresa
        /// </summary>
        public async Task<bool> RetirarEmpleado(long empleadoId, DateTime fechaRetiro, string observacion)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().ExecuteAsync(
                    "TH_Empleados_Retirar",
                    new
                    {
                        empleadoId = empleadoId,
                        fechaRetiro = fechaRetiro,
                        observacion = observacion
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Empleado {empleadoId} retirado el {fechaRetiro:yyyy-MM-dd}");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al retirar empleado {empleadoId}");
                throw;
            }
        }

        /// <summary>
        /// Reintegra un empleado a la empresa
        /// </summary>
        public async Task<bool> ReintegrarEmpleado(long empleadoId, DateTime fechaReintegro)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().ExecuteAsync(
                    "TH_Empleados_Reintegrar",
                    new
                    {
                        empleadoId = empleadoId,
                        fechaReintegro = fechaReintegro
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Empleado {empleadoId} reintegrado el {fechaReintegro:yyyy-MM-dd}");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al reintegrar empleado {empleadoId}");
                throw;
            }
        }

        #endregion

        #region EXPERIENCIA LABORAL

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

        #endregion

        #region EDUCACIÓN

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

        #endregion

        #region HIJOS

        /// <summary>
        /// Obtiene lista de hijos de un empleado
        /// </summary>
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

        /// <summary>
        /// Agrega un nuevo hijo
        /// </summary>
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

        /// <summary>
        /// Elimina un hijo
        /// </summary>
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

        /// <summary>
        /// Obtiene lista de contactos emergencia de un empleado
        /// </summary>
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

        /// <summary>
        /// Agrega un nuevo contacto emergencia
        /// </summary>
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

        /// <summary>
        /// Elimina un contacto emergencia
        /// </summary>
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

        /// <summary>
        /// Obtiene lista de promociones de un empleado
        /// </summary>
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

        /// <summary>
        /// Agrega una nueva promoción
        /// </summary>
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

        /// <summary>
        /// Elimina una promoción
        /// </summary>
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

        /// <summary>
        /// Obtiene lista de salarios de un empleado
        /// </summary>
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

        /// <summary>
        /// Agrega un nuevo registro de salario
        /// </summary>
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

        /// <summary>
        /// Elimina un registro de salario
        /// </summary>
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

        #region CATÁLOGOS

        public async Task<List<AreaDto>> ObtenerAreas()
        {
            return await ObtenerCatalogo<AreaDto>("Areas", "Nombre");
        }

        public async Task<List<CargoDto>> ObtenerCargos()
        {
            return await ObtenerCatalogo<CargoDto>("Cargos", "Nombre");
        }

        public async Task<List<BandaDto>> ObtenerBandas()
        {
            return await ObtenerCatalogo<BandaDto>("Bandas", "Nombre");
        }

        public async Task<List<EstadoCivilDto>> ObtenerEstadosCiviles()
        {
            return await ObtenerCatalogo<EstadoCivilDto>("EstadosCiviles", "Nombre");
        }

        public async Task<List<GrupoSanguineoDto>> ObtenerGruposSanguineos()
        {
            return await ObtenerCatalogo<GrupoSanguineoDto>("GruposSanguineos", "Nombre");
        }

        public async Task<List<SedeDto>> ObtenerSedes()
        {
            return await ObtenerCatalogo<SedeDto>("Sedes", "Nombre");
        }

        public async Task<List<TipoContratoDto>> ObtenerTiposContrato()
        {
            return await ObtenerCatalogo<TipoContratoDto>("TiposContrato", "Nombre");
        }

        public async Task<List<TiempContratoDto>> ObtenerTiemposContrato()
        {
            return await ObtenerCatalogo<TiempContratoDto>("TiemposContrato", "Nombre");
        }

        public async Task<List<EmpresaDto>> ObtenerEmpresas()
        {
            return await ObtenerCatalogo<EmpresaDto>("Empresas", "Nombre");
        }

        public async Task<List<JobFunctionDto>> ObtenerJobFunctions()
        {
            return await ObtenerCatalogo<JobFunctionDto>("JobFunctions", "Nombre");
        }

        public async Task<List<ParentescoDto>> ObtenerParentescos()
        {
            return await ObtenerCatalogo<ParentescoDto>("Parentescos", "Nombre");
        }

        public async Task<List<MotivoCambioSalarioDto>> ObtenerMotivosCambioSalario()
        {
            return await ObtenerCatalogo<MotivoCambioSalarioDto>("MotivosCambioSalario", "Nombre");
        }

        public async Task<List<TipoSalarioDto>> ObtenerTiposSalario()
        {
            return await ObtenerCatalogo<TipoSalarioDto>("TiposSalario", "Nombre");
        }

        private async Task<List<T>> ObtenerCatalogo<T>(string tableName, string nombreCampo) where T : new()
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().QueryAsync<T>(
                    $"SELECT * FROM {tableName} ORDER BY {nombreCampo}"
                );

                return resultado.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener catálogo {tableName}");
                throw;
            }
        }

        #endregion

        #region HELPERS

        private EmpleadoDto MapToEmpleadoDto(dynamic row)
        {
            return new EmpleadoDto
            {
                Id = row.Id,
                TipoId = row.TipoId,
                Identificacion = row.Identificacion,
                Nombres = row.Nombres,
                Apellidos = row.Apellidos,
                NombrePreferido = row.NombrePreferido,
                FechaNacimiento = row.FechaNacimiento,
                Sexo = row.Sexo,
                EstadoCivil = row.EstadoCivil,
                GrupoSanguineo = row.GrupoSanguineo,
                Nacionalidad = row.Nacionalidad,
                UrlFoto = row.UrlFoto,
                Activo = row.Activo,
                FechaIngreso = row.FechaIngreso,
                IdIStaff = row.IdIStaff,
                JefeInmediato = row.JefeInmediato,
                Sede = row.Sede,
                CorreoIpsos = row.CorreoIpsos,
                CentroCostoId = row.CentroCostoId,
                TipoContratoId = row.TipoContratoId,
                TiempoContratoId = row.TiempoContratoId,
                Empresa = row.Empresa,
                JobFunctionId = row.JobFunctionId,
                Observaciones = row.Observaciones
            };
        }

        #endregion
    }
}
