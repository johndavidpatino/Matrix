namespace MatrixNext.Data.Modules.CC.DTOs.ProcesosInternos
{
    /// <summary>
    /// DTO para Reporte de Conteos de Trabajos
    /// </summary>
    public class ReporteConteoDto
    {
        public long IdConteo { get; set; }
        public long IdTrabajo { get; set; }
        public string CodigoTrabajo { get; set; } = string.Empty;
        public string NombreTrabajo { get; set; } = string.Empty;
        public long IdActividad { get; set; }
        public string CodigoActividad { get; set; } = string.Empty;
        public string NombreActividad { get; set; } = string.Empty;
        public string? Categoria { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaConteo { get; set; }
        public string? UsuarioRegistro { get; set; }
        public string? Observaciones { get; set; }
        public byte Estado { get; set; }
        public string EstadoNombre => Estado switch
        {
            0 => "Inactivo",
            1 => "Activo",
            2 => "Consolidado",
            _ => "Desconocido"
        };
    }

    /// <summary>
    /// DTO para filtros de Reporte de Conteos
    /// </summary>
    public class FiltrosReporteConteoDto
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public long? IdTrabajo { get; set; }
        public long? IdActividad { get; set; }
        public string? Categoria { get; set; }
        public byte? Estado { get; set; }
    }
}
