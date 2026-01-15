using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.PY.ControlCalidad
{
    /// <summary>
    /// DTO para crear/editar Pregunta de evaluaciÃ³n
    /// </summary>
    public class PreguntaInputDto
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "El tipo de proceso es requerido")]
        public int IdProceso { get; set; }

        public int TipoProceso
        {
            get => IdProceso;
            set => IdProceso = value;
        }
        
        [Required(ErrorMessage = "El texto de la pregunta es requerido")]
        [StringLength(1000, MinimumLength = 5, 
            ErrorMessage = "La pregunta debe tener entre 5 y 1000 caracteres")]
        public string Pregunta { get; set; }
        
        public bool Activa { get; set; } = true;
    }
}


