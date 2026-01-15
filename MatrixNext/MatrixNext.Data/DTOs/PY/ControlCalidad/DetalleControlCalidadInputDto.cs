namespace MatrixNext.Data.DTOs.PY.ControlCalidad
{
    /// <summary>
    /// DTO para crear/editar detalle de Control de Calidad (respuesta a pregunta)
    /// </summary>
    public class DetalleControlCalidadInputDto
    {
        public long? IdPregunta { get; set; }

        public long? PreguntaId
        {
            get => IdPregunta;
            set => IdPregunta = value;
        }
        
        public bool? Cumple { get; set; }
        
        public string Comentarios { get; set; }

        public string? Respuesta { get; set; }

        public int? Calificacion { get; set; }
    }
}

