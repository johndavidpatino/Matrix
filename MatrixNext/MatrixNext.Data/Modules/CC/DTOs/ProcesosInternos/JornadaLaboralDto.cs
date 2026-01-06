namespace MatrixNext.Data.Modules.CC.DTOs.ProcesosInternos
{
    /// <summary>
    /// DTO para Cálculo de Jornada Laboral
    /// </summary>
    public class JornadaLaboralDto
    {
        public long IdJornada { get; set; }
        public long IdEmpleado { get; set; }
        public string? CodigoEmpleado { get; set; }
        public string? NombreEmpleado { get; set; }
        public int Periodo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int HorasBase { get; set; }
        public int HorasAusencias { get; set; }
        public int HorasExtras { get; set; }
        public int HorasTrabajadas => HorasBase - HorasAusencias + HorasExtras;
        public decimal ValorHora { get; set; }
        public decimal TotalDevengado => HorasTrabajadas * ValorHora;
        public bool Calculado { get; set; }
        public DateTime? FechaCalculo { get; set; }
        public string? UsuarioCalcula { get; set; }
    }

    /// <summary>
    /// DTO para ausencias (integración con TH)
    /// </summary>
    public class AusenciaEmpleadoDto
    {
        public long IdAusencia { get; set; }
        public long IdEmpleado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string? TipoAusencia { get; set; }
        public int DiasAusencia { get; set; }
        public int HorasAusencia { get; set; }
        public bool Aprobada { get; set; }
    }

    /// <summary>
    /// DTO para calcular jornada
    /// </summary>
    public class CalcularJornadaRequest
    {
        public long IdEmpleado { get; set; }
        public int Periodo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int HorasBase { get; set; }
        public int HorasExtras { get; set; }
        public string UsuarioCalcula { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para resumen de jornadas
    /// </summary>
    public class ResumenJornadasDto
    {
        public int TotalEmpleados { get; set; }
        public int TotalHorasBase { get; set; }
        public int TotalHorasAusencias { get; set; }
        public int TotalHorasExtras { get; set; }
        public int TotalHorasTrabajadas => TotalHorasBase - TotalHorasAusencias + TotalHorasExtras;
        public decimal TotalDevengado { get; set; }
        public decimal PromedioHorasPorEmpleado => TotalEmpleados > 0 
            ? Math.Round((decimal)TotalHorasTrabajadas / TotalEmpleados, 2) 
            : 0;
    }
}
