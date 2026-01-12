using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.TH.Models;

namespace MatrixNext.Data.Adapters.TH
{
    /// <summary>
    /// Interfaz para adaptador de Empleados - Gestión CRUD principal de empleados
    /// </summary>
    public interface IThEmpleadosAdapter
    {
        // ========== EMPLEADO PRINCIPAL ==========
        Task<List<EmpleadoDto>> ObtenerEmpleados(long? id = null, string nombres = null, string apellidos = null, bool? activo = null, byte? serviceLive = null, short? cargo = null, byte? sede = null);
        Task<EmpleadoDto> ObtenerEmpleadoPorId(long id);
        Task<long> CrearEmpleado(EmpleadoInputDto input);
        Task<bool> ActualizarDatosGenerales(long id, EmpleadoInputDto input);
        Task<bool> ActualizarDatosLaborales(EmpleadoDatosLaboralesInputDto input);
        Task<bool> ActualizarDatosPersonales(EmpleadoDatosPersonalesInputDto input);
        Task<bool> ActualizarNomina(EmpleadoNominaInputDto input);
        Task<bool> ActualizarSalario(EmpleadoActualizarSalarioInputDto input);
        Task<bool> RetirarEmpleado(long empleadoId, DateTime fechaRetiro, string observacion);
        Task<bool> ReintegrarEmpleado(long empleadoId, DateTime fechaReintegro);

        // ========== EXPERIENCIA LABORAL ==========
        Task<List<ExperienciaLaboralDto>> ObtenerExperienciasLaborales(long personaId);
        Task<long> AgregarExperienciaLaboral(ExperienciaLaboralInputDto input);
        Task<bool> EliminarExperienciaLaboral(long id);

        // ========== EDUCACIÓN ==========
        Task<List<EducacionDto>> ObtenerEducacion(long personaId);
        Task<long> AgregarEducacion(EducacionInputDto input);
        Task<bool> EliminarEducacion(long id);

        // ========== HIJOS ==========
        Task<List<HijoDto>> ObtenerHijos(long personaId);
        Task<long> AgregarHijo(HijoInputDto input);
        Task<bool> EliminarHijo(long id);

        // ========== CONTACTOS EMERGENCIA ==========
        Task<List<ContactoEmergenciaDto>> ObtenerContactosEmergencia(long personaId);
        Task<long> AgregarContactoEmergencia(ContactoEmergenciaInputDto input);
        Task<bool> EliminarContactoEmergencia(long id);

        // ========== PROMOCIONES ==========
        Task<List<PromocionDto>> ObtenerPromociones(long personaId);
        Task<long> AgregarPromocion(PromocionInputDto input);
        Task<bool> EliminarPromocion(long id);

        // ========== SALARIOS ==========
        Task<List<SalarioDto>> ObtenerSalarios(long personaId);
        Task<long> AgregarSalario(SalarioInputDto input);
        Task<bool> EliminarSalario(long id);

        // ========== CATÁLOGOS ==========
        Task<List<AreaDto>> ObtenerAreas();
        Task<List<CargoDto>> ObtenerCargos();
        Task<List<BandaDto>> ObtenerBandas();
        Task<List<EstadoCivilDto>> ObtenerEstadosCiviles();
        Task<List<GrupoSanguineoDto>> ObtenerGruposSanguineos();
        Task<List<SedeDto>> ObtenerSedes();
        Task<List<TipoContratoDto>> ObtenerTiposContrato();
        Task<List<TiempContratoDto>> ObtenerTiemposContrato();
        Task<List<EmpresaDto>> ObtenerEmpresas();
        Task<List<JobFunctionDto>> ObtenerJobFunctions();
        Task<List<ParentescoDto>> ObtenerParentescos();
        Task<List<MotivoCambioSalarioDto>> ObtenerMotivosCambioSalario();
        Task<List<TipoSalarioDto>> ObtenerTiposSalario();
    }

    /// <summary>
    /// Interfaz para adaptador de Catálogos TH
    /// </summary>
    public interface IThCatalogosAdapter
    {
        Task<List<AreaDto>> ObtenerAreas();
        Task<List<CargoDto>> ObtenerCargos();
        Task<List<BandaDto>> ObtenerBandas();
        Task<List<EstadoCivilDto>> ObtenerEstadosCiviles();
        Task<List<GrupoSanguineoDto>> ObtenerGruposSanguineos();
        Task<List<SedeDto>> ObtenerSedes();
        Task<List<TipoContratoDto>> ObtenerTiposContrato();
        Task<List<TiempContratoDto>> ObtenerTiemposContrato();
        Task<List<EmpresaDto>> ObtenerEmpresas();
        Task<List<JobFunctionDto>> ObtenerJobFunctions();
        Task<List<ParentescoDto>> ObtenerParentescos();
        Task<List<MotivoCambioSalarioDto>> ObtenerMotivosCambioSalario();
        Task<List<TipoSalarioDto>> ObtenerTiposSalario();
    }

    /// <summary>
    /// Interfaz para adaptador de Desvinculaciones
    /// </summary>
    public interface IThDesvinculacionAdapter
    {
        Task<List<DesvinculacionDto>> ObtenerDesvinculaciones(int pageSize, int pageIndex, string textoBuscado);
        Task<long> IniciarProcesoDesvinculacion(DesvinculacionInputDto input);
        Task<List<dynamic>> ObtenerEvaluacionesDesvinculacion(long desvinculacionId);
        Task<bool> GuardarEvaluacionDesvinculacion(DesvinculacionEvaluacionInputDto input, string usuario);
        Task<bool> FinalizarProcesoDesvinculacion(long desvinculacionId);
        Task<string> GenerarPDFDesvinculacion(long desvinculacionId);
    }
}
