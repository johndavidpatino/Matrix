namespace MatrixNext.Web.DTOs.PY.ControlCalidad
{
    /// <summary>
    /// DTO para crear/editar detalle de Control de Calidad (respuesta a pregunta)
    /// </summary>
    public class DetalleControlCalidadInputDto
    {
        public long? IdPregunta { get; set; }
        
        public bool? Cumple { get; set; }
        
        public string Comentarios { get; set; }
    }
}
