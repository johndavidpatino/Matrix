using System;
using System.Collections.Generic;

namespace MatrixNext.Data.Adapters.TH.Models
{
    // ==================== EMPLEADO PRINCIPAL ====================

    public class EmpleadoDto
    {
        public long Id { get; set; }
        public byte TipoId { get; set; }
        public long Identificacion { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? NombrePreferido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Sexo { get; set; }
        public int? EstadoCivil { get; set; }
        public int? GrupoSanguineo { get; set; }
        public string? Nacionalidad { get; set; }
        public string? UrlFoto { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public long? IdIStaff { get; set; }
        public long? JefeInmediato { get; set; }
        public byte? Sede { get; set; }
        public string? CorreoIpsos { get; set; }
        public long? CentroCostoId { get; set; }
        public byte? TipoContratoId { get; set; }
        public byte? TiempoContratoId { get; set; }
        public byte? Empresa { get; set; }
        public byte? JobFunctionId { get; set; }
        public string? Observaciones { get; set; }
    }

    public class EmpleadoInputDto
    {
        public long? Id { get; set; } // Null para crear, valor para actualizar
        public byte TipoId { get; set; }
        public long Identificacion { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? NombrePreferido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Sexo { get; set; }
        public int? EstadoCivil { get; set; }
        public int? GrupoSanguineo { get; set; }
        public string? Nacionalidad { get; set; }
        public string? FotoBase64 { get; set; }
    }

    public class EmpleadoDatosLaboralesInputDto
    {
        public long Id { get; set; }
        public long? IdIStaff { get; set; }
        public long? JefeInmediato { get; set; }
        public byte? Sede { get; set; }
        public string? CorreoIpsos { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public long? CentroCostoId { get; set; }
        public byte? TipoContratoId { get; set; }
        public byte? TiempoContratoId { get; set; }
        public byte? Empresa { get; set; }
        public byte? JobFunctionId { get; set; }
        public string? Observaciones { get; set; }
    }

    public class EmpleadoDatosPersonalesInputDto
    {
        public long Id { get; set; }
        public long? CiudadId { get; set; }
        public string? Direccion { get; set; }
        public byte? NseId { get; set; }
        public long? TelefonoFijo { get; set; }
        public long? TelefonoCelular { get; set; }
        public string? EmailPersonal { get; set; }
        public string? BarrioResidencia { get; set; }
        public byte? Localidad { get; set; }
        public int? MunicipioNacimientoDivipolaId { get; set; }
        public byte? TallaCamisetaId { get; set; }
    }

    public class EmpleadoNominaInputDto
    {
        public long Id { get; set; }
        public byte? BancoId { get; set; }
        public byte? TipoCuentaId { get; set; }
        public string? NumeroCuenta { get; set; }
        public byte? FondoPensionesId { get; set; }
        public byte? FondoCesantiasId { get; set; }
        public byte? EPSId { get; set; }
        public byte? CajaCompensacionId { get; set; }
        public byte? ARLId { get; set; }
    }

    public class EmpleadoActualizarSalarioInputDto
    {
        public long EmpleadoId { get; set; }
        public decimal Salario { get; set; }
        public byte? TipoSalarioId { get; set; }
    }

    // ==================== EXPERIENCIA LABORAL ====================

    public class ExperienciaLaboralDto
    {
        public long Id { get; set; }
        public long PersonaId { get; set; }
        public string? Empresa { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Cargo { get; set; }
        public bool? EsInvestigacion { get; set; }
    }

    public class ExperienciaLaboralInputDto
    {
        public long PersonaId { get; set; }
        public string? Empresa { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string? Cargo { get; set; }
        public bool EsInvestigacion { get; set; }
    }

    // ==================== EDUCACIÓN ====================

    public class EducacionDto
    {
        public long Id { get; set; }
        public long PersonaId { get; set; }
        public byte? Tipo { get; set; }
        public string? Titulo { get; set; }
        public string? Institucion { get; set; }
        public string? Pais { get; set; }
        public string? Ciudad { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public byte? Modalidad { get; set; }
        public byte? Estado { get; set; }
    }

    public class EducacionInputDto
    {
        public long PersonaId { get; set; }
        public byte Tipo { get; set; }
        public string? Titulo { get; set; }
        public string? Institucion { get; set; }
        public string? Pais { get; set; }
        public string? Ciudad { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public byte Modalidad { get; set; }
        public byte Estado { get; set; }
    }

    // ==================== HIJOS ====================

    public class HijoDto
    {
        public long Id { get; set; }
        public long PersonaId { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public byte? Genero { get; set; }
        public DateTime? FechaNacimiento { get; set; }
    }

    public class HijoInputDto
    {
        public long PersonaId { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public byte Genero { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }

    // ==================== CONTACTO EMERGENCIA ====================

    public class ContactoEmergenciaDto
    {
        public long Id { get; set; }
        public long PersonaId { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public byte? ParentescoId { get; set; }
        public long? TelefonoFijo { get; set; }
        public long? TelefonoCelular { get; set; }
    }

    public class ContactoEmergenciaInputDto
    {
        public long PersonaId { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public byte ParentescoId { get; set; }
        public long? TelefonoFijo { get; set; }
        public long? TelefonoCelular { get; set; }
    }

    // ==================== PROMOCIÓN ====================

    public class PromocionDto
    {
        public long Id { get; set; }
        public long PersonaId { get; set; }
        public byte? NuevaAreaId { get; set; }
        public byte? NuevaBandaId { get; set; }
        public short? NuevoCargoId { get; set; }
        public byte? NuevoLevelId { get; set; }
        public DateTime? FechaPromocion { get; set; }
    }

    public class PromocionInputDto
    {
        public long PersonaId { get; set; }
        public byte NuevaAreaId { get; set; }
        public byte NuevaBandaId { get; set; }
        public short NuevoCargoId { get; set; }
        public byte NuevoLevelId { get; set; }
        public DateTime FechaPromocion { get; set; }
    }

    // ==================== SALARIO ====================

    public class SalarioDto
    {
        public long Id { get; set; }
        public long PersonaId { get; set; }
        public DateTime? FechaAplicacion { get; set; }
        public byte? MotivoCambioId { get; set; }
        public byte? Tipo { get; set; }
        public decimal? Monto { get; set; }
    }

    public class SalarioInputDto
    {
        public long PersonaId { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public byte? MotivoCambioId { get; set; }
        public byte Tipo { get; set; }
        public decimal Monto { get; set; }
    }

    // ==================== DESVINCULACIÓN ====================

    public class DesvinculacionDto
    {
        public long Id { get; set; }
        public long EmpleadoId { get; set; }
        public DateTime? FechaRetiro { get; set; }
        public string? MotivosDesvinculacion { get; set; }
        public string? Estado { get; set; }
    }

    public class DesvinculacionInputDto
    {
        public long EmpleadoId { get; set; }
        public DateTime FechaRetiro { get; set; }
        public string? MotivosDesvinculacion { get; set; }
    }

    public class DesvinculacionEvaluacionInputDto
    {
        public long DesvinculacionEmpleadoId { get; set; }
        public string? Observaciones { get; set; }
        public bool Aprobado { get; set; }
    }

    // ==================== CATÁLOGOS ====================

    public class AreaDto
    {
        public byte Id { get; set; }
        public string? Nombre { get; set; }
    }

    public class CargoDto
    {
        public short Id { get; set; }
        public string? Nombre { get; set; }
    }

    public class BandaDto
    {
        public byte Id { get; set; }
        public string? Nombre { get; set; }
    }

    public class EstadoCivilDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
    }

    public class GrupoSanguineoDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
    }

    public class SedeDto
    {
        public byte Id { get; set; }
        public string? Nombre { get; set; }
    }

    public class TipoContratoDto
    {
        public byte Id { get; set; }
        public string? Nombre { get; set; }
    }

    public class TiempContratoDto
    {
        public byte Id { get; set; }
        public string? Nombre { get; set; }
    }

    public class EmpresaDto
    {
        public byte Id { get; set; }
        public string? Nombre { get; set; }
    }

    public class JobFunctionDto
    {
        public byte Id { get; set; }
        public string? Nombre { get; set; }
    }

    public class ParentescoDto
    {
        public byte Id { get; set; }
        public string? Nombre { get; set; }
    }

    public class MotivoCambioSalarioDto
    {
        public byte Id { get; set; }
        public string? Nombre { get; set; }
    }

    public class TipoSalarioDto
    {
        public byte Id { get; set; }
        public string? Nombre { get; set; }
    }
}

