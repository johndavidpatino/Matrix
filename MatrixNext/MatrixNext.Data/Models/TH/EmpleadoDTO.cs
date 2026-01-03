using System;

namespace MatrixNext.Data.Models.TH
{
    /// <summary>
    /// DTO para resumen de empleados (búsqueda y listado)
    /// Mapea a TH_Empleados_Get_Result de CoreProject
    /// </summary>
    public class EmpleadoResumenDTO
    {
        public long Id { get; set; }
        public string TipoIdentificacion { get; set; }
        public long Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string CorreoIpsos { get; set; }
        public string Celular { get; set; }
        public string GrupoSanguineoTxt { get; set; }
        public string SedeTxt { get; set; }
        public string AreaTxt { get; set; }
        public decimal PorcentajeDiligenciamiento { get; set; }
        public bool Activo { get; set; }
        public string UrlFoto { get; set; }
    }

    /// <summary>
    /// DTO para filtros de búsqueda de empleados
    /// </summary>
    public class EmpleadoFiltroDTO
    {
        public long? Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public bool? Activo { get; set; }
        public ushort? AreaServiceLine { get; set; }
        public byte? Cargo { get; set; }
        public byte? Sede { get; set; }
    }

    /// <summary>
    /// DTO para experiencia laboral
    /// </summary>
    public class ExperienciaLaboralDTO
    {
        public long Id { get; set; }
        public long PersonaId { get; set; }
        public string Empresa { get; set; }
        public string Cargo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool EsInvestigacion { get; set; }
    }

    /// <summary>
    /// DTO para educación de empleados
    /// </summary>
    public class EducacionDTO
    {
        public long Id { get; set; }
        public long PersonaId { get; set; }
        public ushort Tipo { get; set; }
        public string Titulo { get; set; }
        public string Institucion { get; set; }
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public ushort Modalidad { get; set; }
        public ushort Estado { get; set; }
    }

    /// <summary>
    /// DTO para hijos de empleados
    /// </summary>
    public class HijoDTO
    {
        public long Id { get; set; }
        public long PersonaId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public byte Genero { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }

    /// <summary>
    /// DTO para contactos de emergencia
    /// </summary>
    public class ContactoEmergenciaDTO
    {
        public long Id { get; set; }
        public long PersonaId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public byte Parentesco { get; set; }
        public long? TelefonoFijo { get; set; }
        public long? TelefonoCelular { get; set; }
    }

    /// <summary>
    /// DTO para promociones de empleados
    /// </summary>
    public class PromocionDTO
    {
        public long Id { get; set; }
        public long PersonaId { get; set; }
        public byte NuevaAreaId { get; set; }
        public byte NuevaBandaId { get; set; }
        public short NuevoCargoId { get; set; }
        public byte NuevoLevelId { get; set; }
        public DateTime FechaPromocion { get; set; }
    }

    /// <summary>
    /// DTO para salarios de empleados
    /// </summary>
    public class SalarioDTO
    {
        public long Id { get; set; }
        public long PersonaId { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public ushort? MotivoCambio { get; set; }
        public decimal Salario { get; set; }
        public ushort? Tipo { get; set; }
    }

    /// <summary>
    /// DTO para estado de diligenciamiento de empleados
    /// </summary>
    public class EstadoDiligenciamientoEmpleadoDTO
    {
        public long PersonaId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string AreaTxt { get; set; }
        public string CorreoIpsos { get; set; }

        public bool ExperienciaLaboral { get; set; }
        public bool Educacion { get; set; }
        public bool ContactoEmergencia { get; set; }
        public bool HistoricoPosiciones { get; set; }
        public bool Salarios { get; set; }
        public bool DatosLaborales { get; set; }
        public bool DatosPersonales { get; set; }
        public bool Ingles { get; set; }
        public bool Nomina { get; set; }

        public decimal PorcentajeDiligenciamiento { get; set; }
    }
}
