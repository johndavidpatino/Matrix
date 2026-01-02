using System;

namespace MatrixNext.Data.Modules.TH.Ausencias.Models
{
    /// <summary>
    /// ViewModel para lectura/listado de solicitudes de ausencia
    /// Mapea directamente desde TH_SolicitudAusencia
    /// </summary>
    public class AusenciaViewModel
    {
        public long Id { get; set; }
        public long IdEmpleado { get; set; }
        public string? NombreEmpleado { get; set; }
        public DateTime? FiniCausacion { get; set; }
        public DateTime? FFinCausacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public short? DiasCalendario { get; set; }
        public byte? DiasLaborales { get; set; }
        public byte? Tipo { get; set; }
        public string? TipoNombre { get; set; }
        public byte? Estado { get; set; }
        public string? EstadoNombre { get; set; }
        public long? AprobadoPor { get; set; }
        public string? NombreAprobador { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public long? VoBo1 { get; set; }
        public DateTime? FechaVoBo1 { get; set; }
        public long? VoBo2 { get; set; }
        public DateTime? FechaVoBo2 { get; set; }
        public long? VoBo3 { get; set; }
        public DateTime? FechaVoBo3 { get; set; }
        public long? RegistradoPor { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string? ObservacionesSolicitud { get; set; }
        public string? ObservacionesAprobacion { get; set; }
    }

    /// <summary>
    /// Formulario para crear/editar solicitud de ausencia
    /// Se envía desde el cliente
    /// </summary>
    public class SolicitudAusenciaFormViewModel
    {
        public long IdEmpleado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public byte TipoAusencia { get; set; }
        public string Observaciones { get; set; }
        public long AprobadorId { get; set; }
        public DateTime? FiniCausacion { get; set; }
        public DateTime? FFinCausacion { get; set; }
    }

    /// <summary>
    /// Incapacidad - extiende datos de ausencia con información médica
    /// Mapea desde TH_Ausencia_Incapacidades
    /// </summary>
    public class IncapacidadViewModel
    {
        public long IdSolicitudAusencia { get; set; }
        public byte? EntidadConsulta { get; set; }
        public string IPS { get; set; }
        public string RegistroMedico { get; set; }
        public byte? TipoIncapacidad { get; set; }
        public byte? ClaseAusencia { get; set; }
        public byte? SOAT { get; set; }
        public DateTime? FechaAccidenteTrabajo { get; set; }
        public string Comentarios { get; set; }
        public string CIE { get; set; }
    }

    /// <summary>
    /// Beneficios pendientes de usar por el empleado
    /// </summary>
    public class BeneficioPendienteViewModel
    {
        public int Id { get; set; }
        public long IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string Beneficio { get; set; }
        public int Dias { get; set; }
    }

    /// <summary>
    /// Periodos de vacaciones causadas
    /// </summary>
    public class PeriodoCausadoVacacionesViewModel
    {
        public long Id { get; set; }
        public DateTime FechaInicioPeriodo { get; set; }
        public DateTime FechaFinPeriodo { get; set; }
        public int DiasDisfrutados { get; set; }
        public int DiasPendientes { get; set; }
    }

    /// <summary>
    /// Reporte de vacaciones (para Excel)
    /// </summary>
    public class ReporteVacacionesViewModel
    {
        public long Identificacion { get; set; }
        public string NombreEmpleado { get; set; }
        public string AreaSL { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public int DiasDisfrutados { get; set; }
        public int DiasPendientes { get; set; }
        public string UltimoPeriodoCausado { get; set; }
        public string SeisDias { get; set; }
        public string Estado { get; set; }
    }

    /// <summary>
    /// Reporte de beneficios (para Excel)
    /// </summary>
    public class ReporteBeneficiosViewModel
    {
        public long Identificacion { get; set; }
        public string NombreEmpleado { get; set; }
        public string AreaSL { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public string TipoBeneficio { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public short? DiasCalendario { get; set; }
        public short? DiasLaborales { get; set; }
        public string Estado { get; set; }
    }

    /// <summary>
    /// Reporte de ausentismo (para Excel)
    /// </summary>
    public class ReporteAusentismoViewModel
    {
        public long Identificacion { get; set; }
        public string NombreEmpleado { get; set; }
        public string AreaSL { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public string TipoAusentismo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public short? DiasCalendario { get; set; }
        public short? DiasLaborales { get; set; }
        public string Estado { get; set; }
    }

    /// <summary>
    /// Reporte de incapacidades (para Excel)
    /// </summary>
    public class ReporteIncapacidadesViewModel
    {
        public long Identificacion { get; set; }
        public string NombreEmpleado { get; set; }
        public string AreaSL { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public int? Edad { get; set; }
        public string Cargo { get; set; }
        public string Sede { get; set; }
        public string DiaSemana { get; set; }
        public string Mes { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public short? DiasCalendario { get; set; }
        public short? DiasLaborales { get; set; }
        public string EntidadConsulta { get; set; }
        public string IPSPrestadora { get; set; }
        public string NoRegistroMedico { get; set; }
        public string TipoIncapacidad { get; set; }
        public string ClaseAusencia { get; set; }
        public string SOAT { get; set; }
        public DateTime? FechaAccidenteTrabajo { get; set; }
        public string Comentarios { get; set; }
        public string DXAsociado { get; set; }
        public string CIE { get; set; }
        public string CategoriaDX { get; set; }
        public string Estado { get; set; }
    }

    /// <summary>
    /// Reporte de vacaciones para nómina
    /// </summary>
    public class ReporteVacacionesNominaViewModel
    {
        public long Identificacion { get; set; }
        public string NombreEmpleado { get; set; }
        public string AreaSL { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int DiasCalendarioSolicitados { get; set; }
        public int DiasLaboralesSolicitados { get; set; }
        public int DiasCausados { get; set; }
        public DateTime? InicioPeriodo { get; set; }
        public DateTime? FinPeriodo { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public string AprobadoPor { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string VoBoRRHH { get; set; }
        public DateTime? FechaVoBoRRHH { get; set; }
    }

    /// <summary>
    /// Reporte de solicitudes pendientes
    /// </summary>
    public class ReporteSolicitudesPendientesViewModel
    {
        public long? IdSolicitud { get; set; }
        public long Identificacion { get; set; }
        public string NombreEmpleado { get; set; }
        public string AreaSL { get; set; }
        public int DiasCalendarioSolicitados { get; set; }
        public int DiasLaboralesSolicitados { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Manager { get; set; }
        public DateTime? FechaAprobacionManager { get; set; }
        public string Estado { get; set; }
        public string Tipo { get; set; }
    }

    /// <summary>
    /// Reporte de vacaciones detallado
    /// </summary>
    public class ReporteVacacionesDetalladoViewModel
    {
        public long Identificacion { get; set; }
        public string NombreEmpleado { get; set; }
        public string AreaSL { get; set; }
        public long IdSolicitud { get; set; }
        public string TipoSolicitud { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaInicioSolicitud { get; set; }
        public DateTime FechaFinSolicitud { get; set; }
        public ushort DiasCalendario { get; set; }
        public ushort DiasLaborales { get; set; }
        public string EstadoSolicitud { get; set; }
        public string Aprueba { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string ApruebaRRHH { get; set; }
        public DateTime? FechaAprobacionRRHH { get; set; }
    }

    /// <summary>
    /// Ausencia del equipo (para coordinador)
    /// </summary>
    public class AusenciaEquipoViewModel
    {
        public int SolicitudId { get; set; }
        public long IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int EstadoId { get; set; }
        public string Estado { get; set; }
        public long JefeInmediatoId { get; set; }
        public string JefeInmediato { get; set; }
        public string ObservacionesSolicitud { get; set; }
        public string ObservacionesAprobacion { get; set; }
        public int TipoId { get; set; }
        public string Tipo { get; set; }
        public int AreaId { get; set; }
        public string Area { get; set; }
    }

    /// <summary>
    /// Subordinado del coordinador/jefe
    /// </summary>
    public class SubordinadoViewModel
    {
        public long Id { get; set; }
        public long IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Avatar { get; set; }
    }

    /// <summary>
    /// Resultado de validación
    /// </summary>
    public class ResultadoValidacionViewModel
    {
        public int Result { get; set; }
        public string MensajeResultado { get; set; }
    }

    /// <summary>
    /// Cálculo de días
    /// </summary>
    public class CalculoDiasViewModel
    {
        public int DiasCalendario { get; set; }
        public int DiasLaborales { get; set; }
    }

    /// <summary>
    /// Tipo de ausencia (catálogo)
    /// </summary>
    public class TipoAusenciaViewModel
    {
        public short Id { get; set; }
        public string Tipo { get; set; }
        public string Nombre => Tipo;
    }

    /// <summary>
    /// Aprobador (usuario que puede aprobar)
    /// </summary>
    public class AprobadorViewModel
    {
        public long Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Nombre => NombreCompleto;
    }
}
