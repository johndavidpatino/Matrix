namespace MatrixNext.Data.DTOs.PY.ControlCalidad
{
    /// <summary>
    /// DTO para obtener detalle completo de un Control de Calidad
    /// </summary>
    public class ControlCalidadDetailDto
    {
        public long Id { get; set; }
        
        public long TrabajoId { get; set; }
        
        public string Evaluador { get; set; }
        
        public string RolEvaluador { get; set; }
        
        public long PersonaId { get; set; }
        
        public string PersonaNombre { get; set; }
        
        public DateTime Fecha { get; set; }

        public DateTime FechaControl
        {
            get => Fecha;
            set => Fecha = value;
        }
        
        public int TipoProceso { get; set; }
        
        public string NombreTipoProceso { get; set; }
        
        public string JobBook { get; set; }
        
        public string NombreTrabajo { get; set; }

        public string? Estado { get; set; }

        public string? Observaciones { get; set; }

        public string? RegistradoPor { get; set; }

        public DateTime FechaRegistro { get; set; }

        public string? ModificadoPor { get; set; }

        public DateTime? FechaModificacion { get; set; }
        
        public List<DetalleControlCalidadDetailDto> Detalles { get; set; } = new();
    }
}

