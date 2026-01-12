using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.TH;
using MatrixNext.Data.Adapters.TH.Models;
using MatrixNext.Data.Services.TH.Interfaces;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.TH
{
    /// <summary>
    /// Servicio para gestión integral de Empleados
    /// Orquesta operaciones de adapters y aplica reglas de negocio
    /// </summary>
    public class ThEmpleadosService : IThEmpleadosService
    {
        private readonly IThEmpleadosAdapter _empleadosAdapter;
        private readonly IThCatalogosAdapter _catalogosAdapter;
        private readonly ILogger<ThEmpleadosService> _logger;

        public ThEmpleadosService(
            IThEmpleadosAdapter empleadosAdapter,
            IThCatalogosAdapter catalogosAdapter,
            ILogger<ThEmpleadosService> logger)
        {
            _empleadosAdapter = empleadosAdapter;
            _catalogosAdapter = catalogosAdapter;
            _logger = logger;
        }

        #region EMPLEADO PRINCIPAL

        public async Task<ApiResponse<List<EmpleadoDto>>> ObtenerEmpleados(long? id = null, string nombres = null, string apellidos = null, 
            bool? activo = null, byte? serviceLive = null, short? cargo = null, byte? sede = null)
        {
            try
            {
                var empleados = await _empleadosAdapter.ObtenerEmpleados(id, nombres, apellidos, activo, serviceLive, cargo, sede);
                return ApiResponse<List<EmpleadoDto>>.Success(empleados, "Empleados obtenidos correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empleados");
                return ApiResponse<List<EmpleadoDto>>.Error($"Error al obtener empleados: {ex.Message}");
            }
        }

        public async Task<ApiResponse<EmpleadoDto>> ObtenerEmpleadoPorId(long id)
        {
            try
            {
                var empleado = await _empleadosAdapter.ObtenerEmpleadoPorId(id);
                if (empleado == null)
                    return ApiResponse<EmpleadoDto>.Error($"Empleado con ID {id} no encontrado");

                return ApiResponse<EmpleadoDto>.Success(empleado, "Empleado obtenido correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener empleado {id}");
                return ApiResponse<EmpleadoDto>.Error($"Error al obtener empleado: {ex.Message}");
            }
        }

        public async Task<ApiResponse<long>> CrearEmpleado(EmpleadoInputDto input)
        {
            try
            {
                // Validaciones de negocio
                if (string.IsNullOrWhiteSpace(input.Nombres) || string.IsNullOrWhiteSpace(input.Apellidos))
                    return ApiResponse<long>.Error("Nombres y apellidos son requeridos");

                if (input.Identificacion <= 0)
                    return ApiResponse<long>.Error("Identificación inválida");

                var newId = await _empleadosAdapter.CrearEmpleado(input);
                return ApiResponse<long>.Success(newId, "Empleado creado correctamente", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear empleado");
                return ApiResponse<long>.Error($"Error al crear empleado: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ActualizarDatosGenerales(long id, EmpleadoInputDto input)
        {
            try
            {
                var result = await _empleadosAdapter.ActualizarDatosGenerales(id, input);
                if (!result)
                    return ApiResponse<bool>.Error($"No se pudo actualizar el empleado {id}");

                return ApiResponse<bool>.Success(true, "Datos generales actualizados correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar datos generales de {id}");
                return ApiResponse<bool>.Error($"Error al actualizar: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ActualizarDatosLaborales(EmpleadoDatosLaboralesInputDto input)
        {
            try
            {
                var result = await _empleadosAdapter.ActualizarDatosLaborales(input);
                if (!result)
                    return ApiResponse<bool>.Error($"No se pudo actualizar el empleado {input.Id}");

                return ApiResponse<bool>.Success(true, "Datos laborales actualizados correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar datos laborales");
                return ApiResponse<bool>.Error($"Error al actualizar: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ActualizarDatosPersonales(EmpleadoDatosPersonalesInputDto input)
        {
            try
            {
                var result = await _empleadosAdapter.ActualizarDatosPersonales(input);
                if (!result)
                    return ApiResponse<bool>.Error($"No se pudo actualizar el empleado {input.Id}");

                return ApiResponse<bool>.Success(true, "Datos personales actualizados correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar datos personales");
                return ApiResponse<bool>.Error($"Error al actualizar: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ActualizarNomina(EmpleadoNominaInputDto input)
        {
            try
            {
                var result = await _empleadosAdapter.ActualizarNomina(input);
                if (!result)
                    return ApiResponse<bool>.Error($"No se pudo actualizar la nómina del empleado {input.Id}");

                return ApiResponse<bool>.Success(true, "Datos de nómina actualizados correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar nómina");
                return ApiResponse<bool>.Error($"Error al actualizar: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ActualizarSalario(EmpleadoActualizarSalarioInputDto input)
        {
            try
            {
                if (input.Salario <= 0)
                    return ApiResponse<bool>.Error("Salario debe ser mayor a 0");

                var result = await _empleadosAdapter.ActualizarSalario(input);
                if (!result)
                    return ApiResponse<bool>.Error($"No se pudo actualizar el salario del empleado");

                return ApiResponse<bool>.Success(true, "Salario actualizado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar salario");
                return ApiResponse<bool>.Error($"Error al actualizar: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> RetirarEmpleado(long empleadoId, DateTime fechaRetiro, string observacion)
        {
            try
            {
                if (fechaRetiro == default(DateTime))
                    return ApiResponse<bool>.Error("Fecha de retiro es requerida");

                var result = await _empleadosAdapter.RetirarEmpleado(empleadoId, fechaRetiro, observacion);
                if (!result)
                    return ApiResponse<bool>.Error($"No se pudo retirar el empleado");

                return ApiResponse<bool>.Success(true, "Empleado retirado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al retirar empleado");
                return ApiResponse<bool>.Error($"Error al retirar: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ReintegrarEmpleado(long empleadoId, DateTime fechaReintegro)
        {
            try
            {
                if (fechaReintegro == default(DateTime))
                    return ApiResponse<bool>.Error("Fecha de reintegro es requerida");

                var result = await _empleadosAdapter.ReintegrarEmpleado(empleadoId, fechaReintegro);
                if (!result)
                    return ApiResponse<bool>.Error($"No se pudo reintegrar el empleado");

                return ApiResponse<bool>.Success(true, "Empleado reintegrado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al reintegrar empleado");
                return ApiResponse<bool>.Error($"Error al reintegrar: {ex.Message}");
            }
        }

        #endregion

        #region DATOS COMPLEMENTARIOS

        public async Task<ApiResponse<List<ExperienciaLaboralDto>>> ObtenerExperienciasLaborales(long personaId)
        {
            try
            {
                var experiencias = await _empleadosAdapter.ObtenerExperienciasLaborales(personaId);
                return ApiResponse<List<ExperienciaLaboralDto>>.Success(experiencias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener experiencias");
                return ApiResponse<List<ExperienciaLaboralDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<long>> AgregarExperienciaLaboral(ExperienciaLaboralInputDto input)
        {
            try
            {
                if (input.FechaInicio > input.FechaFin)
                    return ApiResponse<long>.Error("Fecha inicio no puede ser mayor a fecha fin");

                var newId = await _empleadosAdapter.AgregarExperienciaLaboral(input);
                return ApiResponse<long>.Success(newId, "Experiencia agregada correctamente", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar experiencia");
                return ApiResponse<long>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> EliminarExperienciaLaboral(long id)
        {
            try
            {
                var result = await _empleadosAdapter.EliminarExperienciaLaboral(id);
                return ApiResponse<bool>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar experiencia");
                return ApiResponse<bool>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<EducacionDto>>> ObtenerEducacion(long personaId)
        {
            try
            {
                var educacion = await _empleadosAdapter.ObtenerEducacion(personaId);
                return ApiResponse<List<EducacionDto>>.Success(educacion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener educación");
                return ApiResponse<List<EducacionDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<long>> AgregarEducacion(EducacionInputDto input)
        {
            try
            {
                var newId = await _empleadosAdapter.AgregarEducacion(input);
                return ApiResponse<long>.Success(newId, "Educación agregada correctamente", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar educación");
                return ApiResponse<long>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> EliminarEducacion(long id)
        {
            try
            {
                var result = await _empleadosAdapter.EliminarEducacion(id);
                return ApiResponse<bool>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar educación");
                return ApiResponse<bool>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<HijoDto>>> ObtenerHijos(long personaId)
        {
            try
            {
                var hijos = await _empleadosAdapter.ObtenerHijos(personaId);
                return ApiResponse<List<HijoDto>>.Success(hijos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener hijos");
                return ApiResponse<List<HijoDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<long>> AgregarHijo(HijoInputDto input)
        {
            try
            {
                var newId = await _empleadosAdapter.AgregarHijo(input);
                return ApiResponse<long>.Success(newId, "Hijo agregado correctamente", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar hijo");
                return ApiResponse<long>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> EliminarHijo(long id)
        {
            try
            {
                var result = await _empleadosAdapter.EliminarHijo(id);
                return ApiResponse<bool>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar hijo");
                return ApiResponse<bool>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<ContactoEmergenciaDto>>> ObtenerContactosEmergencia(long personaId)
        {
            try
            {
                var contactos = await _empleadosAdapter.ObtenerContactosEmergencia(personaId);
                return ApiResponse<List<ContactoEmergenciaDto>>.Success(contactos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener contactos");
                return ApiResponse<List<ContactoEmergenciaDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<long>> AgregarContactoEmergencia(ContactoEmergenciaInputDto input)
        {
            try
            {
                var newId = await _empleadosAdapter.AgregarContactoEmergencia(input);
                return ApiResponse<long>.Success(newId, "Contacto agregado correctamente", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar contacto");
                return ApiResponse<long>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> EliminarContactoEmergencia(long id)
        {
            try
            {
                var result = await _empleadosAdapter.EliminarContactoEmergencia(id);
                return ApiResponse<bool>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar contacto");
                return ApiResponse<bool>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<PromocionDto>>> ObtenerPromociones(long personaId)
        {
            try
            {
                var promociones = await _empleadosAdapter.ObtenerPromociones(personaId);
                return ApiResponse<List<PromocionDto>>.Success(promociones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener promociones");
                return ApiResponse<List<PromocionDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<long>> AgregarPromocion(PromocionInputDto input)
        {
            try
            {
                var newId = await _empleadosAdapter.AgregarPromocion(input);
                return ApiResponse<long>.Success(newId, "Promoción agregada correctamente", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar promoción");
                return ApiResponse<long>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> EliminarPromocion(long id)
        {
            try
            {
                var result = await _empleadosAdapter.EliminarPromocion(id);
                return ApiResponse<bool>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar promoción");
                return ApiResponse<bool>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<SalarioDto>>> ObtenerSalarios(long personaId)
        {
            try
            {
                var salarios = await _empleadosAdapter.ObtenerSalarios(personaId);
                return ApiResponse<List<SalarioDto>>.Success(salarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener salarios");
                return ApiResponse<List<SalarioDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<long>> AgregarSalario(SalarioInputDto input)
        {
            try
            {
                if (input.Monto <= 0)
                    return ApiResponse<long>.Error("Monto debe ser mayor a 0");

                var newId = await _empleadosAdapter.AgregarSalario(input);
                return ApiResponse<long>.Success(newId, "Salario agregado correctamente", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar salario");
                return ApiResponse<long>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> EliminarSalario(long id)
        {
            try
            {
                var result = await _empleadosAdapter.EliminarSalario(id);
                return ApiResponse<bool>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar salario");
                return ApiResponse<bool>.Error($"Error: {ex.Message}");
            }
        }

        #endregion

        #region CATÁLOGOS

        public async Task<ApiResponse<List<AreaDto>>> ObtenerAreas()
        {
            try
            {
                var areas = await _catalogosAdapter.ObtenerAreas();
                return ApiResponse<List<AreaDto>>.Success(areas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener áreas");
                return ApiResponse<List<AreaDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<CargoDto>>> ObtenerCargos()
        {
            try
            {
                var cargos = await _catalogosAdapter.ObtenerCargos();
                return ApiResponse<List<CargoDto>>.Success(cargos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cargos");
                return ApiResponse<List<CargoDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<BandaDto>>> ObtenerBandas()
        {
            try
            {
                var bandas = await _catalogosAdapter.ObtenerBandas();
                return ApiResponse<List<BandaDto>>.Success(bandas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener bandas");
                return ApiResponse<List<BandaDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<EstadoCivilDto>>> ObtenerEstadosCiviles()
        {
            try
            {
                var estados = await _catalogosAdapter.ObtenerEstadosCiviles();
                return ApiResponse<List<EstadoCivilDto>>.Success(estados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estados civiles");
                return ApiResponse<List<EstadoCivilDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<GrupoSanguineoDto>>> ObtenerGruposSanguineos()
        {
            try
            {
                var grupos = await _catalogosAdapter.ObtenerGruposSanguineos();
                return ApiResponse<List<GrupoSanguineoDto>>.Success(grupos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener grupos sanguíneos");
                return ApiResponse<List<GrupoSanguineoDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<SedeDto>>> ObtenerSedes()
        {
            try
            {
                var sedes = await _catalogosAdapter.ObtenerSedes();
                return ApiResponse<List<SedeDto>>.Success(sedes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener sedes");
                return ApiResponse<List<SedeDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<TipoContratoDto>>> ObtenerTiposContrato()
        {
            try
            {
                var tipos = await _catalogosAdapter.ObtenerTiposContrato();
                return ApiResponse<List<TipoContratoDto>>.Success(tipos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tipos contrato");
                return ApiResponse<List<TipoContratoDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<TiempContratoDto>>> ObtenerTiemposContrato()
        {
            try
            {
                var tiempos = await _catalogosAdapter.ObtenerTiemposContrato();
                return ApiResponse<List<TiempContratoDto>>.Success(tiempos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tiempos contrato");
                return ApiResponse<List<TiempContratoDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<EmpresaDto>>> ObtenerEmpresas()
        {
            try
            {
                var empresas = await _catalogosAdapter.ObtenerEmpresas();
                return ApiResponse<List<EmpresaDto>>.Success(empresas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empresas");
                return ApiResponse<List<EmpresaDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<JobFunctionDto>>> ObtenerJobFunctions()
        {
            try
            {
                var jobFunctions = await _catalogosAdapter.ObtenerJobFunctions();
                return ApiResponse<List<JobFunctionDto>>.Success(jobFunctions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener job functions");
                return ApiResponse<List<JobFunctionDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<ParentescoDto>>> ObtenerParentescos()
        {
            try
            {
                var parentescos = await _catalogosAdapter.ObtenerParentescos();
                return ApiResponse<List<ParentescoDto>>.Success(parentescos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener parentescos");
                return ApiResponse<List<ParentescoDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<MotivoCambioSalarioDto>>> ObtenerMotivosCambioSalario()
        {
            try
            {
                var motivos = await _catalogosAdapter.ObtenerMotivosCambioSalario();
                return ApiResponse<List<MotivoCambioSalarioDto>>.Success(motivos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener motivos cambio salario");
                return ApiResponse<List<MotivoCambioSalarioDto>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<TipoSalarioDto>>> ObtenerTiposSalario()
        {
            try
            {
                var tipos = await _catalogosAdapter.ObtenerTiposSalario();
                return ApiResponse<List<TipoSalarioDto>>.Success(tipos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tipos salario");
                return ApiResponse<List<TipoSalarioDto>>.Error($"Error: {ex.Message}");
            }
        }

        #endregion
    }
}
