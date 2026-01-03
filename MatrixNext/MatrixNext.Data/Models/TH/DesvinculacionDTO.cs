using System;

namespace MatrixNext.Data.Models.TH
{
    /// <summary>
    /// DTO para resumen de desvinculaciones
    /// </summary>
    public class DesvinculacionEmpleadoResumenDTO
    {
        public int Id { get; set; }
        public long EmpleadoId { get; set; }
        public string NombreEmpleadoCompleto { get; set; }
        public string Cargo { get; set; }
        public DateTime FechaRetiro { get; set; }
        public string EstadoProceso { get; set; }
        public DateTime FechaRegistro { get; set; }
        public long RegistradoPor { get; set; }
    }



    /// <summary>
    /// DTO para información de empleado en desvinculación
    /// </summary>
    public class DesvinculacionEmpleadoInfoDTO
    {
        public string NombreEmpleadoCompleto { get; set; }
        public string EmpleadoId { get; set; }
        public string Cargo { get; set; }
        public string FechaRetiro { get; set; }
    }


}
