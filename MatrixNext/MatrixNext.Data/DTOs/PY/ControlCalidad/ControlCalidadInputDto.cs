using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.PY.ControlCalidad
{
    /// <summary>
    /// DTO para crear/editar Control de Calidad
    /// </summary>
    public class ControlCalidadInputDto
    {
        public long? Id { get; set; }
        
        public long? TrabajoId { get; set; }

        public long? TrabajoProcesoId
        {
            get => TrabajoId;
            set => TrabajoId = value;
        }
        
        [Required(ErrorMessage = "El evaluador es requerido")]
        public string Evaluador { get; set; }
        
        [Required(ErrorMessage = "El rol del evaluador es requerido")]
        public string RolEvaluador { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar un analista responsable")]
        public long PersonaId { get; set; }
        
        [Required(ErrorMessage = "La fecha de evaluaciÃ³n es requerida")]
        public DateTime Fecha { get; set; }

        public DateTime FechaControl
        {
            get => Fecha;
            set => Fecha = value;
        }
        
        [Required(ErrorMessage = "El tipo de proceso es requerido")]
        public int TipoProceso { get; set; }

        public string? Observaciones { get; set; }
        
        public List<DetalleControlCalidadInputDto> Detalles { get; set; } = new();
    }
}


