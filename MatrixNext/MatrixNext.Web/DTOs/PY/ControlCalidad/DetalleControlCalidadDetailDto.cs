namespace MatrixNext.Web.DTOs.PY.ControlCalidad
{
    /// <summary>
    /// DTO para obtener detalle completo de una respuesta de evaluación
    /// </summary>
    public class DetalleControlCalidadDetailDto
    {
        public long Id { get; set; }
        
        public long IdControlCalidad { get; set; }
        
        public long IdPregunta { get; set; }
        
        public bool? Cumple { get; set; }
        
        public string Comentarios { get; set; }
        
        public string TextoPregunta { get; set; }
    }
}
