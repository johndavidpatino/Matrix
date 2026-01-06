namespace MatrixNext.Data.Modules.CC.DTOs.ProcesosInternos
{
    /// <summary>
    /// DTO para Resumen de Productividad
    /// </summary>
    public class ResumenProductividadDto
    {
        public long IdResumen { get; set; }
        public int Periodo { get; set; }
        public long IdTrabajo { get; set; }
        public string CodigoTrabajo { get; set; } = string.Empty;
        public string NombreTrabajo { get; set; } = string.Empty;
        public string? CodigoActividad { get; set; }
        public string? NombreActividad { get; set; }
        public int TotalUnidades { get; set; }
        public int TotalHoras { get; set; }
        public decimal ProductividadPromedio => TotalHoras > 0 
            ? Math.Round((decimal)TotalUnidades / TotalHoras, 2) 
            : 0;
        public decimal CostoTotal { get; set; }
        public decimal CostoUnitario => TotalUnidades > 0 
            ? Math.Round(CostoTotal / TotalUnidades, 2) 
            : 0;
        public int NumeroEmpleados { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }

    /// <summary>
    /// DTO para filtros de Resumen de Productividad
    /// </summary>
    public class FiltrosResumenProductividadDto
    {
        public int? Periodo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public long? IdTrabajo { get; set; }
        public string? CodigoActividad { get; set; }
    }

    /// <summary>
    /// DTO para datos agregados de productividad
    /// </summary>
    public class ProductividadAgregadaDto
    {
        public int TotalTrabajos { get; set; }
        public int TotalActividades { get; set; }
        public int TotalUnidadesProcesadas { get; set; }
        public int TotalHorasTrabajadas { get; set; }
        public decimal ProductividadGlobal => TotalHorasTrabajadas > 0
            ? Math.Round((decimal)TotalUnidadesProcesadas / TotalHorasTrabajadas, 2)
            : 0;
        public decimal CostoTotalPeriodo { get; set; }
        public int TotalEmpleados { get; set; }
    }
}
