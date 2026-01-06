using System;
using System.Collections.Generic;

namespace MatrixNext.Data.Modules.CC.DTOs.Reportes
{
    public class ReportePagoDto
    {
        public long IdPago { get; set; }
        public int Periodo { get; set; }
        public long IdTrabajo { get; set; }
        public string? CodigoTrabajo { get; set; }
        public string? NombreTrabajo { get; set; }
        public long IdEmpleado { get; set; }
        public string? NombreEmpleado { get; set; }
        public decimal ValorPagado { get; set; }
        public DateTime FechaPago { get; set; }
        public string? Estado { get; set; }
        public string? MedioPago { get; set; }
        public string? Observaciones { get; set; }
    }

    public class ReporteActividadProduccionDto
    {
        public long IdRegistro { get; set; }
        public long IdTrabajo { get; set; }
        public string? CodigoTrabajo { get; set; }
        public string? NombreTrabajo { get; set; }
        public string? Actividad { get; set; }
        public decimal Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal CostoTotal => Cantidad * CostoUnitario;
        public DateTime FechaRegistro { get; set; }
        public string? UsuarioRegistro { get; set; }
        public string? Estado { get; set; }
    }

    public class ReporteContabilizacionPstDto
    {
        public long IdPst { get; set; }
        public int Periodo { get; set; }
        public long IdTrabajo { get; set; }
        public string? CodigoTrabajo { get; set; }
        public string? NombreTrabajo { get; set; }
        public string? CodigoPst { get; set; }
        public decimal ValorContabilizado { get; set; }
        public DateTime FechaContabilizacion { get; set; }
        public string? UsuarioContabiliza { get; set; }
        public string? Estado { get; set; }
    }

    public class ReporteVarianzaPresupuestariaDto
    {
        public int Periodo { get; set; }
        public long IdTrabajo { get; set; }
        public string? CodigoTrabajo { get; set; }
        public string? NombreTrabajo { get; set; }
        public decimal Presupuesto { get; set; }
        public decimal Ejecutado { get; set; }
        public decimal Varianza => Ejecutado - Presupuesto;
        public decimal PorcentajeVarianza => Presupuesto > 0 ? Math.Round((Ejecutado / Presupuesto) * 100, 2) : 0;
    }

    public class FiltrosReportePagosDto
    {
        public int? Periodo { get; set; }
        public long? IdTrabajo { get; set; }
        public long? IdEmpleado { get; set; }
        public byte? Estado { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }

    public class FiltrosReporteActividadProduccionDto
    {
        public int? Periodo { get; set; }
        public long? IdTrabajo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }

    public class FiltrosReporteContabilizacionPstDto
    {
        public int? Periodo { get; set; }
        public long? IdTrabajo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }

    public class FiltrosReporteVarianzaPresupuestariaDto
    {
        public int? Periodo { get; set; }
        public long? IdTrabajo { get; set; }
    }
}
