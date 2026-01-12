using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.TH.Models;

namespace MatrixNext.Data.Services.TH.Interfaces
{
    /// <summary>
    /// Servicio para gestión integral de Empleados
    /// Orquesta operaciones de adapters y aplica reglas de negocio
    /// </summary>
    public interface IThEmpleadosService
    {
        // ========== EMPLEADO PRINCIPAL ==========
        Task<ApiResponse<List<EmpleadoDto>>> ObtenerEmpleados(long? id = null, string nombres = null, string apellidos = null, 
            bool? activo = null, byte? serviceLive = null, short? cargo = null, byte? sede = null);
        Task<ApiResponse<EmpleadoDto>> ObtenerEmpleadoPorId(long id);
        Task<ApiResponse<long>> CrearEmpleado(EmpleadoInputDto input);
        Task<ApiResponse<bool>> ActualizarDatosGenerales(long id, EmpleadoInputDto input);
        Task<ApiResponse<bool>> ActualizarDatosLaborales(EmpleadoDatosLaboralesInputDto input);
        Task<ApiResponse<bool>> ActualizarDatosPersonales(EmpleadoDatosPersonalesInputDto input);
        Task<ApiResponse<bool>> ActualizarNomina(EmpleadoNominaInputDto input);
        Task<ApiResponse<bool>> ActualizarSalario(EmpleadoActualizarSalarioInputDto input);
        Task<ApiResponse<bool>> RetirarEmpleado(long empleadoId, DateTime fechaRetiro, string observacion);
        Task<ApiResponse<bool>> ReintegrarEmpleado(long empleadoId, DateTime fechaReintegro);

        // ========== DATOS COMPLEMENTARIOS (Experiencia, Educación, Hijos, etc) ==========
        Task<ApiResponse<List<ExperienciaLaboralDto>>> ObtenerExperienciasLaborales(long personaId);
        Task<ApiResponse<long>> AgregarExperienciaLaboral(ExperienciaLaboralInputDto input);
        Task<ApiResponse<bool>> EliminarExperienciaLaboral(long id);

        Task<ApiResponse<List<EducacionDto>>> ObtenerEducacion(long personaId);
        Task<ApiResponse<long>> AgregarEducacion(EducacionInputDto input);
        Task<ApiResponse<bool>> EliminarEducacion(long id);

        Task<ApiResponse<List<HijoDto>>> ObtenerHijos(long personaId);
        Task<ApiResponse<long>> AgregarHijo(HijoInputDto input);
        Task<ApiResponse<bool>> EliminarHijo(long id);

        Task<ApiResponse<List<ContactoEmergenciaDto>>> ObtenerContactosEmergencia(long personaId);
        Task<ApiResponse<long>> AgregarContactoEmergencia(ContactoEmergenciaInputDto input);
        Task<ApiResponse<bool>> EliminarContactoEmergencia(long id);

        Task<ApiResponse<List<PromocionDto>>> ObtenerPromociones(long personaId);
        Task<ApiResponse<long>> AgregarPromocion(PromocionInputDto input);
        Task<ApiResponse<bool>> EliminarPromocion(long id);

        Task<ApiResponse<List<SalarioDto>>> ObtenerSalarios(long personaId);
        Task<ApiResponse<long>> AgregarSalario(SalarioInputDto input);
        Task<ApiResponse<bool>> EliminarSalario(long id);

        // ========== CATÁLOGOS ==========
        Task<ApiResponse<List<AreaDto>>> ObtenerAreas();
        Task<ApiResponse<List<CargoDto>>> ObtenerCargos();
        Task<ApiResponse<List<BandaDto>>> ObtenerBandas();
        Task<ApiResponse<List<EstadoCivilDto>>> ObtenerEstadosCiviles();
        Task<ApiResponse<List<GrupoSanguineoDto>>> ObtenerGruposSanguineos();
        Task<ApiResponse<List<SedeDto>>> ObtenerSedes();
        Task<ApiResponse<List<TipoContratoDto>>> ObtenerTiposContrato();
        Task<ApiResponse<List<TiempContratoDto>>> ObtenerTiemposContrato();
        Task<ApiResponse<List<EmpresaDto>>> ObtenerEmpresas();
        Task<ApiResponse<List<JobFunctionDto>>> ObtenerJobFunctions();
        Task<ApiResponse<List<ParentescoDto>>> ObtenerParentescos();
        Task<ApiResponse<List<MotivoCambioSalarioDto>>> ObtenerMotivosCambioSalario();
        Task<ApiResponse<List<TipoSalarioDto>>> ObtenerTiposSalario();
    }

    /// <summary>
    /// Servicio para gestión de Desvinculaciones
    /// </summary>
    public interface IThDesvinculacionService
    {
        Task<ApiResponse<List<DesvinculacionDto>>> ObtenerDesvinculaciones(int pageSize, int pageIndex, string textoBuscado);
        Task<ApiResponse<long>> IniciarProcesoDesvinculacion(DesvinculacionInputDto input);
        Task<ApiResponse<List<dynamic>>> ObtenerEvaluacionesDesvinculacion(long desvinculacionId);
        Task<ApiResponse<bool>> GuardarEvaluacionDesvinculacion(DesvinculacionEvaluacionInputDto input, string usuario);
        Task<ApiResponse<bool>> FinalizarProcesoDesvinculacion(long desvinculacionId);
        Task<ApiResponse<string>> GenerarPDFDesvinculacion(long desvinculacionId);
    }

    /// <summary>
    /// Servicio para Catálogos de TH
    /// </summary>
    public interface IThCatalogosService
    {
        Task<ApiResponse<List<AreaDto>>> ObtenerAreas();
        Task<ApiResponse<List<CargoDto>>> ObtenerCargos();
        Task<ApiResponse<List<BandaDto>>> ObtenerBandas();
        Task<ApiResponse<List<EstadoCivilDto>>> ObtenerEstadosCiviles();
        Task<ApiResponse<List<GrupoSanguineoDto>>> ObtenerGruposSanguineos();
        Task<ApiResponse<List<SedeDto>>> ObtenerSedes();
        Task<ApiResponse<List<TipoContratoDto>>> ObtenerTiposContrato();
        Task<ApiResponse<List<TiempContratoDto>>> ObtenerTiemposContrato();
        Task<ApiResponse<List<EmpresaDto>>> ObtenerEmpresas();
        Task<ApiResponse<List<JobFunctionDto>>> ObtenerJobFunctions();
        Task<ApiResponse<List<ParentescoDto>>> ObtenerParentescos();
        Task<ApiResponse<List<MotivoCambioSalarioDto>>> ObtenerMotivosCambioSalario();
        Task<ApiResponse<List<TipoSalarioDto>>> ObtenerTiposSalario();
    }
}
