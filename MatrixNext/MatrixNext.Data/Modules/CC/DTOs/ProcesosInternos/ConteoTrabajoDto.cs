namespace MatrixNext.Data.Modules.CC.DTOs.ProcesosInternos
{
    /// <summary>
    /// DTO para Conteo de Trabajos
    /// </summary>
    public class ConteoTrabajoDto
    {
        public long IdConteo { get; set; }
        public long IdTrabajo { get; set; }
        public string? CodigoTrabajo { get; set; }
        public string? NombreTrabajo { get; set; }
        public long IdActividad { get; set; }
        public string? CodigoActividad { get; set; }
        public string? NombreActividad { get; set; }
        public string? Categoria { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaConteo { get; set; }
        public string? UsuarioRegistro { get; set; }
        public string? Observaciones { get; set; }
        public byte Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }

    /// <summary>
    /// DTO para guardar conteo
    /// </summary>
    public class GuardarConteoRequest
    {
        public long IdConteo { get; set; }
        public long IdTrabajo { get; set; }
        public long IdActividad { get; set; }
        public string? Categoria { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaConteo { get; set; }
        public string? Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para actividades por trabajo
    /// </summary>
    public class ActividadTrabajoDto
    {
        public long IdActividad { get; set; }
        public string CodigoActividad { get; set; } = string.Empty;
        public string NombreActividad { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public byte Estado { get; set; }
    }
}
