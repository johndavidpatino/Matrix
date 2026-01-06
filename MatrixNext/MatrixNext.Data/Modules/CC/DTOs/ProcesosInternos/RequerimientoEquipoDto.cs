namespace MatrixNext.Data.Modules.CC.DTOs.ProcesosInternos
{
    /// <summary>
    /// DTO para Requerimientos de Equipo
    /// </summary>
    public class RequerimientoEquipoDto
    {
        public long IdRequerimiento { get; set; }
        public long IdTrabajo { get; set; }
        public string? CodigoTrabajo { get; set; }
        public string? NombreTrabajo { get; set; }
        public DateTime FechaRequerimiento { get; set; }
        public string? TipoEquipo { get; set; }
        public int CantidadRequerida { get; set; }
        public int CantidadDisponible { get; set; }
        public int CantidadFaltante => Math.Max(0, CantidadRequerida - CantidadDisponible);
        public string? Justificacion { get; set; }
        public byte EstadoRequerimiento { get; set; }
        public string EstadoNombre => EstadoRequerimiento switch
        {
            0 => "Pendiente",
            1 => "Aprobado",
            2 => "Rechazado",
            3 => "En Proceso",
            4 => "Completado",
            _ => "Desconocido"
        };
        public string? UsuarioSolicita { get; set; }
        public string? UsuarioAprueba { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    /// <summary>
    /// DTO para muestra de generación de requerimientos
    /// </summary>
    public class MuestraRequerimientoDto
    {
        public long IdTrabajo { get; set; }
        public string CodigoTrabajo { get; set; } = string.Empty;
        public int TotalProduccion { get; set; }
        public string? TipoEquipo { get; set; }
        public int CantidadSugerida { get; set; }
        public decimal PorcentajeUtilizacion { get; set; }
    }

    /// <summary>
    /// DTO para guardar requerimiento
    /// </summary>
    public class GuardarRequerimientoRequest
    {
        public long IdRequerimiento { get; set; }
        public long IdTrabajo { get; set; }
        public DateTime FechaRequerimiento { get; set; }
        public string TipoEquipo { get; set; } = string.Empty;
        public int CantidadRequerida { get; set; }
        public string? Justificacion { get; set; }
    }
}
