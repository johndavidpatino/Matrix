namespace MatrixNext.Data.DTOs.PY.ControlCalidad
{
    /// <summary>
    /// DTO para obtener detalle completo de una respuesta de evaluaciÃ³n
    /// </summary>
    public class DetalleControlCalidadDetailDto
    {
        public long Id { get; set; }
        
        public long IdControlCalidad { get; set; }
        
        public long IdPregunta { get; set; }
        
        public bool? Cumple { get; set; }
        
        public string Comentarios { get; set; }
        
        public string TextoPregunta { get; set; }

        public string? PreguntaTexto
        {
            get => TextoPregunta;
            set => TextoPregunta = value ?? string.Empty;
        }

        public string? Respuesta { get; set; }

        public int? Calificacion { get; set; }

        public string? Observacion
        {
            get => Comentarios;
            set => Comentarios = value ?? string.Empty;
        }
    }
}

