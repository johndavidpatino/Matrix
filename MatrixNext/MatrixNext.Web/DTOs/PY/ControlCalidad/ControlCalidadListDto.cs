namespace MatrixNext.Web.DTOs.PY.ControlCalidad
{
    /// <summary>
    /// DTO para listar Controles de Calidad
    /// </summary>
    public class ControlCalidadListDto
    {
        public long Id { get; set; }
        
        public string Evaluador { get; set; }
        
        public string RolEvaluador { get; set; }
        
        public DateTime Fecha { get; set; }
        
        public string PersonaNombre { get; set; }
        
        public int TipoProceso { get; set; }
        
        public string NombreTipoProceso { get; set; }
        
        public int DetallesCount { get; set; }
        
        public string JobBook { get; set; }
        
        public string NombreTrabajo { get; set; }
    }
}
