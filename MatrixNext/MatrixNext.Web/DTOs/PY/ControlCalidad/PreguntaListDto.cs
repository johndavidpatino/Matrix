namespace MatrixNext.Web.DTOs.PY.ControlCalidad
{
    /// <summary>
    /// DTO para listar Preguntas de evaluación
    /// </summary>
    public class PreguntaListDto
    {
        public long IdPregunta { get; set; }
        
        public int IdProceso { get; set; }
        
        public string Pregunta { get; set; }
        
        public bool Activa { get; set; }
        
        public string NombreProceso { get; set; }
        
        public int Orden { get; set; }
    }
}
