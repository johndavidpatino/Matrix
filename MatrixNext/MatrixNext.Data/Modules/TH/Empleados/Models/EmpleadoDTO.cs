namespace MatrixNext.Data.Modules.TH.Empleados.Models
{
    /// <summary>
    /// DTO para resumen de empleado - listado principal
    /// </summary>
    public class EmpleadoResumenDTO
    {
        public string TipoIdentificacion { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaRetiro { get; set; }
    }

    /// <summary>
    /// DTO para filtros de búsqueda de empleados
    /// </summary>
    public class EmpleadoFiltroDTO
    {
        public string? TipoIdentificacion { get; set; }
        public string? Identificacion { get; set; }
        public string? Nombre { get; set; }
        public int? IdCargo { get; set; }
        public string? IdMunicipio { get; set; }
        public string? Estado { get; set; }
    }

    /// <summary>
    /// DTO completo de empleado con toda la información
    /// </summary>
    public class EmpleadoDetalleDTO
    {
        // Información básica
        public string TipoIdentificacion { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public string PrimerNombre { get; set; } = string.Empty;
        public string? SegundoNombre { get; set; }
        public string PrimerApellido { get; set; } = string.Empty;
        public string? SegundoApellido { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        
        // Datos personales
        public DateTime? FechaNacimiento { get; set; }
        public string? LugarNacimiento { get; set; }
        public string? Genero { get; set; }
        public string? EstadoCivil { get; set; }
        public string? GrupoSanguineo { get; set; }
        public string? RH { get; set; }
        
        // Contacto
        public string? Direccion { get; set; }
        public string? IdMunicipio { get; set; }
        public string? NombreMunicipio { get; set; }
        public string? Telefono { get; set; }
        public string? Celular { get; set; }
        public string? Email { get; set; }
        
        // Información laboral
        public int? IdCargo { get; set; }
        public string? NombreCargo { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaRetiro { get; set; }
        public string? MotivoRetiro { get; set; }
        public string Estado { get; set; } = "ACTIVO";
        
        // Información adicional
        public string? TallaCamisa { get; set; }
        public string? TallaPantalon { get; set; }
        public string? TallaZapatos { get; set; }
        public bool? TieneHijos { get; set; }
        public int? NumeroHijos { get; set; }
        public bool? TieneVehiculo { get; set; }
        public bool? TieneLicenciaConduccion { get; set; }
        public string? CategoriaLicencia { get; set; }
        
        // Información bancaria
        public string? Banco { get; set; }
        public string? TipoCuenta { get; set; }
        public string? NumeroCuenta { get; set; }
        
        // Auditoría
        public DateTime? FechaCreacion { get; set; }
        public string? UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }
    }

    /// <summary>
    /// DTO para experiencia laboral del empleado
    /// </summary>
    public class ExperienciaLaboralDTO
    {
        public int? Id { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string Empresa { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Funciones { get; set; }
        public string? MotivoRetiro { get; set; }
        public string? TelefonoReferencia { get; set; }
    }

    /// <summary>
    /// DTO para educación del empleado
    /// </summary>
    public class EducacionDTO
    {
        public int? Id { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string NivelEducativo { get; set; } = string.Empty;
        public string Institucion { get; set; } = string.Empty;
        public string TituloObtenido { get; set; } = string.Empty;
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool Graduado { get; set; }
        public string? TarjetaProfesional { get; set; }
    }

    /// <summary>
    /// DTO para hijos del empleado
    /// </summary>
    public class HijoDTO
    {
        public int? Id { get; set; }
        public string IdentificacionEmpleado { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string? Identificacion { get; set; }
        public string Genero { get; set; } = string.Empty;
        public bool EstudiaActualmente { get; set; }
        public string? InstitucionEducativa { get; set; }
        public string? Grado { get; set; }
    }

    /// <summary>
    /// DTO para contactos de emergencia del empleado
    /// </summary>
    public class ContactoEmergenciaDTO
    {
        public int? Id { get; set; }
        public string IdentificacionEmpleado { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Parentesco { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Celular { get; set; }
        public string? Direccion { get; set; }
    }

    /// <summary>
    /// DTO para promociones/cambios de cargo del empleado
    /// </summary>
    public class PromocionDTO
    {
        public int? Id { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public int CargoAnterior { get; set; }
        public string NombreCargoAnterior { get; set; } = string.Empty;
        public int CargoNuevo { get; set; }
        public string NombreCargoNuevo { get; set; } = string.Empty;
        public DateTime FechaCambio { get; set; }
        public string? Motivo { get; set; }
        public string? Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para historial de salarios del empleado
    /// </summary>
    public class SalarioDTO
    {
        public int? Id { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public decimal SalarioAnterior { get; set; }
        public decimal SalarioNuevo { get; set; }
        public DateTime FechaCambio { get; set; }
        public string? Motivo { get; set; }
        public string? Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para estado de diligenciamiento de información del empleado
    /// </summary>
    public class EstadoDiligenciamientoEmpleadoDTO
    {
        public string Identificacion { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public bool DatosBasicosCompletos { get; set; }
        public bool TieneExperienciaLaboral { get; set; }
        public bool TieneEducacion { get; set; }
        public bool TieneContactosEmergencia { get; set; }
        public bool DatosBancariosCompletos { get; set; }
        public int PorcentajeCompletitud { get; set; }
    }

    #region DTOs para Reportes Excel

    /// <summary>
    /// DTO para reporte de información general de empleados
    /// Corresponde a TH_Empleados_Reporte_Info_Result
    /// </summary>
    public class EmpleadoReporteInfoDTO
    {
        public string TipoIdentificacion { get; set; } = string.Empty;
        public string id { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string? nombrePreferido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? Edad { get; set; }
        public string? Genero { get; set; }
        public string? EstadoCivil { get; set; }
        public string? GrupoSanguineo { get; set; }
        public string? Nacionalidad { get; set; }
        public string? EmployeeId { get; set; }
        public string? BUNameITalent { get; set; }
        public string? jobFunction { get; set; }
        public string? JefeInmediato { get; set; }
        public string? Sede { get; set; }
        public string? correoIpsos { get; set; }
        public DateTime? FechaIngresoIpsos { get; set; }
        public string? TipoContrato { get; set; }
        public string? Empresa { get; set; }
        public string? observaciones { get; set; }
        public decimal? SalarioActual { get; set; }
        public string? Banco { get; set; }
        public string? TipoCuenta { get; set; }
        public string? NumeroCuenta { get; set; }
        public string? EPS { get; set; }
        public string? FondoPensiones { get; set; }
        public string? FondoCesantias { get; set; }
        public string? CajaCompensacion { get; set; }
        public string? ARL { get; set; }
        public string? NivelIngles { get; set; }
        public string? CiudadResidencia { get; set; }
        public string? DireccionResidencia { get; set; }
        public string? BarrioResidencia { get; set; }
        public string? Localidad { get; set; }
        public string? NSE { get; set; }
        public string? TelefonoFijo { get; set; }
        public string? TelefonoCelular { get; set; }
        public string? EmailPersonal { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaUltimaActualizacion { get; set; }
        public string? banda { get; set; }
        public string? level { get; set; }
        public string? Area { get; set; }
        public string? Cargo { get; set; }
        public string? Usuario { get; set; }
        public string? TallaCamiseta { get; set; }
        public string? Ciudad_Municipio_Nacimiento { get; set; }
        public string? Departamento_Nacimiento { get; set; }
    }

    /// <summary>
    /// DTO para reporte de hijos de empleados
    /// Corresponde a TH_Hijos_Report_Result
    /// </summary>
    public class EmpleadoHijoReporteDTO
    {
        public string CedulaEmpleado { get; set; } = string.Empty;
        public string Empleado { get; set; } = string.Empty;
        public string NombreHijo { get; set; } = string.Empty;
        public string? Genero { get; set; }
        public DateTime? FechaNacimiento { get; set; }
    }

    /// <summary>
    /// DTO para reporte de educación de empleados
    /// Corresponde a TH_Educacion_Report_Result
    /// </summary>
    public class EmpleadoEducacionReporteDTO
    {
        public string CedulaEmpleado { get; set; } = string.Empty;
        public string Empleado { get; set; } = string.Empty;
        public string? Titulo { get; set; }
        public string? Institucion { get; set; }
        public string? Pais { get; set; }
        public string? Ciudad { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Modalidad { get; set; }
        public string? Tipo { get; set; }
        public string? Estado { get; set; }
    }

    /// <summary>
    /// DTO para reporte de experiencia laboral de empleados
    /// Corresponde a TH_ExperienciaLaboral_Report_Result
    /// </summary>
    public class EmpleadoExperienciaReporteDTO
    {
        public string CedulaEmpleado { get; set; } = string.Empty;
        public string Empleado { get; set; } = string.Empty;
        public string? Empresa { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Cargo { get; set; }
        public bool? EnInvestigacionMercados { get; set; }
    }

    /// <summary>
    /// DTO para reporte de contactos de emergencia de empleados
    /// Corresponde a TH_ContactosEmergencia_Report_Result
    /// </summary>
    public class EmpleadoContactoEmergenciaReporteDTO
    {
        public string CedulaEmpleado { get; set; } = string.Empty;
        public string Empleado { get; set; } = string.Empty;
        public string? ContactoEmergencia { get; set; }
        public string? telefonoCelular { get; set; }
        public string? parentescoTxt { get; set; }
    }

    #endregion

    #region DTOs para Actualización de Datos Maestros

    /// <summary>
    /// DTO para actualización de datos generales del empleado
    /// Equivalente a updateDatosGenerales del legacy
    /// </summary>
    public class ActualizarDatosGeneralesDTO
    {
        public bool EsNuevo { get; set; }
        public long? Id { get; set; }
        public string TipoIdentificacion { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public string PrimerNombre { get; set; } = string.Empty;
        public string? SegundoNombre { get; set; }
        public string PrimerApellido { get; set; } = string.Empty;
        public string? SegundoApellido { get; set; }
        public string? NombrePreferido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Genero { get; set; }
        public int? EstadoCivilId { get; set; }
        public int? GrupoSanguineoId { get; set; }
        public string? Nacionalidad { get; set; }
        public string? FotoBase64 { get; set; } // Foto en base64
        public string? RutaFoto { get; set; } // Ruta del archivo guardado
    }

    /// <summary>
    /// DTO para actualización de datos laborales del empleado
    /// Equivalente a updateDatosLaborales del legacy
    /// </summary>
    public class ActualizarDatosLaboralesDTO
    {
        public long PersonaId { get; set; }
        public string? IdIStaff { get; set; }
        public long? JefeInmediatoId { get; set; }
        public int? SedeId { get; set; }
        public string? CorreoIpsos { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public string? CentroCosto { get; set; }
        public int? TipoContratoId { get; set; }
        public int? TiempoContratoId { get; set; }
        public string? Empresa { get; set; }
        public int? JobFunctionId { get; set; }
        public int? AreaId { get; set; }
        public int? BandaId { get; set; }
        public int? CargoId { get; set; }
        public int? LevelId { get; set; }
        public string? Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para actualización de datos personales del empleado
    /// Equivalente a updateDatosPersonales del legacy
    /// </summary>
    public class ActualizarDatosPersonalesDTO
    {
        public long PersonaId { get; set; }
        public string? MunicipioCiudadId { get; set; }
        public string? Direccion { get; set; }
        public int? NseId { get; set; }
        public string? TelefonoFijo { get; set; }
        public string? TelefonoCelular { get; set; }
        public string? EmailPersonal { get; set; }
        public string? Barrio { get; set; }
        public string? Localidad { get; set; }
        public string? MunicipioNacimientoId { get; set; }
        public int? TallaCamisetaId { get; set; }
    }

    /// <summary>
    /// DTO para actualización de información de nómina
    /// Equivalente a updateNomina del legacy
    /// </summary>
    public class ActualizarNominaDTO
    {
        public long PersonaId { get; set; }
        public int? BancoId { get; set; }
        public int? TipoCuentaId { get; set; }
        public string? NumeroCuenta { get; set; }
        public int? FondoPensionesId { get; set; }
        public int? FondoCesantiasId { get; set; }
        public int? EpsId { get; set; }
        public int? CajaCompensacionId { get; set; }
        public int? ArlId { get; set; }
    }

    /// <summary>
    /// DTO para actualización de nivel de inglés
    /// Equivalente a updateNivelIngles del legacy
    /// </summary>
    public class ActualizarNivelInglesDTO
    {
        public long PersonaId { get; set; }
        public int? NivelInglesId { get; set; }
    }

    #endregion
}
