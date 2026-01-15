namespace MatrixNext.Web.DTOs.PY.ControlCalidad
{
    /// <summary>
    /// DTO para crear/editar Pregunta de evaluación
    /// </summary>
    public class PreguntaInputDto
    {
        [Required(ErrorMessage = "El tipo de proceso es requerido")]
        public int IdProceso { get; set; }
        
        [Required(ErrorMessage = "El texto de la pregunta es requerido")]
        [StringLength(1000, MinimumLength = 5, 
            ErrorMessage = "La pregunta debe tener entre 5 y 1000 caracteres")]
        public string Pregunta { get; set; }
        
        public bool Activa { get; set; } = true;
    }
}
