namespace MatrixNext.Web.DTOs.PY.ControlCalidad
{
    /// <summary>
    /// DTO para crear/editar Control de Calidad
    /// </summary>
    public class ControlCalidadInputDto
    {
        public long? Id { get; set; }
        
        public long? TrabajoId { get; set; }
        
        [Required(ErrorMessage = "El evaluador es requerido")]
        public string Evaluador { get; set; }
        
        [Required(ErrorMessage = "El rol del evaluador es requerido")]
        public string RolEvaluador { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar un analista responsable")]
        public long PersonaId { get; set; }
        
        [Required(ErrorMessage = "La fecha de evaluación es requerida")]
        public DateTime Fecha { get; set; }
        
        [Required(ErrorMessage = "El tipo de proceso es requerido")]
        public int TipoProceso { get; set; }
        
        public List<DetalleControlCalidadInputDto> Detalles { get; set; } = new();
    }
}
